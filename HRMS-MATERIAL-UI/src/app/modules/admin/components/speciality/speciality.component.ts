import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import { SpecialityService } from '../../services/speciality.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { Speciality } from '../../models/speciality.model';
import * as servicePath from '../../../../core/service-paths';


@Component({
  selector: 'app-speciality',
  templateUrl: './speciality.component.html',
  styleUrls: ['./speciality.component.scss']
})
export class SpecialityComponent implements OnInit {

  addSpecialityName: FormGroup;


  themeConfigInput = themeconfig.formfieldappearances;
  formSubmitted = false;
  btnLabel: string = "";
  displayDialog: boolean = false;

  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  specialityList: Speciality[];
  selectedRow: Speciality;





  dataSource: MatTableDataSource<Speciality>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;





  displayedColumns: string[] = ['SuffixName', 'Edit'];




  constructor(private _specialityService: SpecialityService, private _snackBar: MatSnackBar, public navService: NavService) {

    this._specialityService.specialityList.subscribe((data) => {
      this.specialityList = data;
      this.dataSource = new MatTableDataSource(this.specialityList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })


  }

  ngOnInit() {
    this.addSpecialityName = new FormGroup({
      SuffixName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(2)
      ]),
    });
    this._specialityService.specialityEdit.subscribe(data => {
      if (this._specialityService.editMode == true) {
        this.addSpecialityName.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();
    this.getSpecialities();

  }

  addSpeciality(): void {

    if (this.addSpecialityName.valid) {
      this.formSubmitted = true;
      var speciality = new Speciality();
      speciality.SuffixName = this.addSpecialityName.value.SuffixName;
      if (this._specialityService.editMode == true) {
        speciality.SuffixID = this._specialityService.specialityEdit.value.SuffixID;
      }
      if (this.addSpecialityName.valid == true) {
        this._specialityService.createSpeciality(speciality).subscribe(res => {
          if (res) {
            this._specialityService.getSpecialities();
            if (this._specialityService.editMode == false)
              this._snackBar.open('Speciality record added successfully.', '', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            else
              this._snackBar.open('Speciality record updated successfully.', '', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            this.cancel();
          }
          else {
            this._snackBar.open('Unable to add speciality.', '', {
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
    this.addSpecialityName.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this._specialityService.editMode = false;
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
      this.dataSource = new MatTableDataSource(this.specialityList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }



  getSpecialities(): void {
    this._specialityService.getSpecialities();
  }

  editSpecialities(specialityEdit): void {
    this._specialityService.editMode = true;
    this._specialityService.specialityEdit.next(specialityEdit);
  }

}
