import { Component, OnInit, ViewChild } from '@angular/core';
import { Seniority } from '../../models/seniority.model';
import { SeniorityService } from '../../services/seniority.service';
import * as servicePath from '../../../../core/service-paths';
import { FormGroup, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from 'src/themeconfig';
import { now } from 'moment';



@Component({
  selector: 'app-seniority',
  templateUrl: './seniority.component.html',
  styleUrls: ['./seniority.component.scss']
})
export class SeniorityComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  addSeniorityName: FormGroup;
  btnLabel: string = "";
  displayDialog: boolean = false;
  formSubmitted = false;

  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  seniorityList: Seniority[];
  selectedRow: Seniority;

  dataSource: MatTableDataSource<Seniority>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  displayedColumns: string[] = ['PrefixName', 'Edit'];




  constructor(private _seniorityService: SeniorityService, private _snackBar: MatSnackBar, public navService: NavService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;




    this._seniorityService.seniorityList.subscribe((data) => {
      this.seniorityList = data;
      this.dataSource = new MatTableDataSource(this.seniorityList);

      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;


    });


    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  ngOnInit() {
    this.addSeniorityName = new FormGroup({
      PrefixName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(2),
      ]),
    });
    this._seniorityService.seniorityEdit.subscribe(data => {
      if (this._seniorityService.editMode == true) {
        this.addSeniorityName.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();

    this._seniorityService.seniorityList.subscribe((data) => {
      this.seniorityList = data;
      this.dataSource = new MatTableDataSource(this.seniorityList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.getSeniorities();


  }

  getSeniorities(): void {
    this._seniorityService.getSeniorities();
  }

  editSeniorities(seniorityEdit): void {
    this._seniorityService.editMode = true;
    this._seniorityService.seniorityEdit.next(seniorityEdit);
  }


  addSeniority(): void {
    if (this.addSeniorityName.valid) {
      this.formSubmitted = true;
      var seniority = new Seniority();
      seniority.PrefixName = this.addSeniorityName.value.PrefixName;
      if (this._seniorityService.editMode == true) {
        seniority.PrefixID = this._seniorityService.seniorityEdit.value.PrefixID;
      }
      if (this.addSeniorityName.valid == true) {
        this._seniorityService.createSeniority(seniority).subscribe(res => {
          if (res) {
            this._seniorityService.getSeniorities();
            if (this._seniorityService.editMode == false)
              this._snackBar.open('Seniority record added successfully.', '', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            else
              this._snackBar.open('Seniority record updated successfully.', '', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            this.cancel();
          }
          else {
            this._snackBar.open('Unable to add seniority.', '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
          }
        },
          error => {
            this._snackBar.open(error.error, '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });

      }

    }
  }

  cancel(): void {
    this.formSubmitted = false;
    this.addSeniorityName.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this._seniorityService.editMode = false;
    this.btnLabel = "SAVE";
  }

  omit_special_char(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 38 ||
      k == 44 ||
      k == 45
    );
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.seniorityList);
      setTimeout(() => {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }, 1000);
    }
  }

}

