import { Component, OnInit, ViewChild } from '@angular/core';
import { ServiceType } from '../../models/servicetype.model';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroupDirective, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ServiceTypeService } from '../../services/service-type.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from '../../../master-layout/services/nav.service';
import { Router } from '@angular/router';
import { themeconfig } from '../../../../../themeconfig';

@Component({
  selector: 'app-service-type',
  templateUrl: './service-type.component.html',
  styleUrls: ['./service-type.component.scss']
})
export class ServiceTypeComponent implements OnInit {

  displayedColumns: string[] = [
    'ServiceTypeName',
    'ServiceDescription',
    'Edit',
  ];
  dataSource: MatTableDataSource<ServiceType>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private serviceObj: ServiceTypeService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    public route: Router
  ) {
    this.serviceObj.serviceTypeData.subscribe((data) => {
      this.serviceTypeData = data;
      this.dataSource = new MatTableDataSource(this.serviceTypeData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
  serviceTypeData: ServiceType[];
  res: any;
  formSubmitted = false;
  btnLabel = '';
  addServiceType: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  ngOnInit(): void {
    this.addServiceType = new FormGroup({
      ServiceTypeName: new FormControl(null, [Validators.required]),
      ServiceDescription: new FormControl(null, [Validators.required]),
      IsActive: new FormControl(true)
    });

    this.serviceObj.editObj.subscribe((data) => {

      if (this.serviceObj.editMode == true) {
        this.addServiceType.patchValue(data);
        this.addServiceType.get('ServiceTypeName').disable();
        this.btnLabel = 'Update';
      }
    });
    this.btnLabel = 'Save';
    this.Reset();
    this.res = this.serviceObj.GetServiceTypeData();

  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.serviceTypeData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }
  Reset() {
    this.formSubmitted = false;
    this.addServiceType.get('IsActive').patchValue(true);
    this.addServiceType.reset({
      IsActive : this.addServiceType.get('IsActive').value, 
    });
    this.addServiceType.get('ServiceTypeName').enable();
    setTimeout(() => this.formGroupDirective.resetForm({
      IsActive : this.addServiceType.get('IsActive').value,
    }), 0);
    this.serviceObj.editMode = false;
    this.addServiceType.value.IsActive = true;
    this.btnLabel = 'Save';
  }
  setEditObj(editObj) {

    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(editObj);
  }

  CreateServiceType() {
    this.formSubmitted = true;
    var createObj = new ServiceType();

    createObj.ServiceTypeName = this.addServiceType.value.ServiceTypeName;
    createObj.ServiceDescription = this.addServiceType.value.ServiceDescription;
    createObj.IsActive = this.addServiceType.value.IsActive;
    if (this.serviceObj.editMode == true) {
      createObj.ServiceTypeId = this.serviceObj.editObj.value.ServiceTypeId;
      createObj.ServiceTypeName = this.addServiceType.getRawValue().ServiceTypeName;

      createObj.ServiceDescription = this.addServiceType.value.ServiceDescription;
      createObj.IsActive = this.addServiceType.value.IsActive;
    }
    if (this.addServiceType.valid == true) {
      this.serviceObj.CreateServiceTypeArea(createObj).subscribe(
        (res) => {
          if (res) {
            this.serviceObj.GetServiceTypeData();
            if (this.serviceObj.editMode == false) {
              this._snackBar.open(
                'Service Type Record Added Successfully.',
                'x',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this.Reset();
            } else {
              this._snackBar.open(
                'Service Type Record Updated Successfully.',
                'x',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this.Reset();
            }
          } else {
            this._snackBar.open('Unable to Add ServiceType', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

            this.Reset();
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

}
