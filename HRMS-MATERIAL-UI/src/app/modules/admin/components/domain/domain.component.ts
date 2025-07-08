import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormGroupDirective,
} from '@angular/forms';
import { DomainMasterData } from '../../models/domainmasterdata.model';
import { DomainMasterService } from '../../services/domainmaster.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
// import {themeconfig} from 'themeconfig';

@Component({
  selector: 'app-domain',
  templateUrl: './domain.component.html',
  styleUrls: ['./domain.component.scss'],
})
export class DomainComponent implements OnInit {
  addDomainName: FormGroup;

  domainsList: DomainMasterData[];
  themeConfigInput = themeconfig.formfieldappearances;
  formSubmitted = false;
  btnLabel: string = '';
  displayedColumns: string[] = ['DomainName', 'Edit'];

  dataSource: MatTableDataSource<DomainMasterData>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private _domainService: DomainMasterService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this._domainService.domainsList.subscribe((data) => {
      this.domainsList = data;
      this.dataSource = new MatTableDataSource(this.domainsList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit(): void {
    this.addDomainName = new FormGroup({
      DomainName: new FormControl('', [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(2),
      ])
    });

    this._domainService.domainEdit.subscribe((data) => {
      if (this._domainService.editMode == true) {
        this.addDomainName.patchValue(data);
        this.btnLabel = 'UPDATE';
      }
    });
    this.btnLabel = 'SAVE';
    this.cancel();
    this.getDomains();
  }


  editDomains(domainEdit): void {
    this._domainService.editMode = true;
    this._domainService.domainEdit.next(domainEdit);
  }

  addDomain(): void {
    this.formSubmitted = true;
    var domain = new DomainMasterData();
    domain.DomainName = this.addDomainName.value.DomainName;
    if (this._domainService.editMode == true) {
      domain.DomainID = this._domainService.domainEdit.value.DomainID;
    }
    if (this.addDomainName.valid == true) {
      this._domainService.createDomain(domain).subscribe(
        (res) => {
          if (res) {
            this._domainService.getDomains();
            if (this._domainService.editMode == false) {
              this._snackBar.open('Domain record added successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            } else {
              this._snackBar.open('Domain record updated successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            }
          } else {
            this._snackBar.open('Unable to add domain.', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

            this.cancel();
          }
        },
        (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
    }
  }

  cancel(): void {
    this.formSubmitted = false;
    this.addDomainName.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this._domainService.editMode = false;
    this.btnLabel = 'SAVE';
  }

  getDomains(): void {
    this._domainService.getDomains();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.domainsList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }
}
