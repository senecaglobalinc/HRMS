import { Component, OnInit, ViewChild } from '@angular/core';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
//import { ConfirmationService, Message, MessageService } from 'primeng/api';
import { KraMeasurementTypeService } from 'src/app/modules/kra/services/kra.measurement-type.service';
// import { DropDownType, GenericType } from '../../modules/master-layout/models/dropdowntype.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { KRAMeasurementTypeData } from 'src/app/modules/kra/models/kra.measurement-type.model';
import { ActivatedRoute } from '@angular/router';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import * as servicePath from '../../../../core/service-paths';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
// Start - This is just for Demo Purpose can be removed later if material data table is not required
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
//import { tap } from 'rxjs/operators';
//import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

@Component({
  selector: 'app-kra.measurement-type',
  templateUrl: './kra.measurement-type.component.html',
  styleUrls: ['./kra.measurement-type.component.scss'],
  providers: [MasterDataService, KraMeasurementTypeService]
})

export class KraMeasurementTypeComponent {
  // Start - This is just for Demo Purpose can be removed later if material data table is not required
  //displayedColumns: string[] = ['MeasurementType', 'Edit'];
  sliceDataSource = []
  length: number;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20, 30];

  // End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;
  // errorMessage: Message[] = [];
  componentName: string;
  measureTypeList: KRAMeasurementTypeData[] = [];
  buttonTitle: string;
  measurementForm: FormGroup;
  PageSize: number;
  PageDropDown: number[] = [];
  measurementFormSubmitted: boolean;
  saveButton: string = "Add";
  private measurementTypeDataBeforeEdit: KRAMeasurementTypeData;
  measurementType: string;
  kraMeasurementTypeData: KRAMeasurementTypeData;
  kraMeasurement: string;
  valid: boolean = false;
  dataSource = new MatTableDataSource<KRAMeasurementTypeData>();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  cols = [
    { field: 'MeasurementType', header: 'Measuremen Type Name' }
  ];
  displayedColumns: string[] = [
    'MeasurementType',
    'Edit',
  ];
  columnsToDisplay: string[] = this.displayedColumns.slice();

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(
      this.displayedColumns,
      event.previousIndex,
      event.currentIndex
    );
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.dataSource.data);
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  constructor(private _snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    private _actRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _kraMeasurementTypeService: KraMeasurementTypeService,
    public navService: NavService,
    // private _confirmationService: ConfirmationService,
    private dialog: MatDialog,
  ) {
    this.componentName = this._actRoute.routeConfig.component.name;
    this.buttonTitle = "Add";
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.kraMeasurementTypeData = new KRAMeasurementTypeData();
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
  ngOnInit() {
    this.measurementForm = this._formBuilder.group({
      Id: [0],
      MeasurementType: ["", [Validators.required, Validators.pattern(/^[a-zA-Z-&,\s()]*$/), Validators.maxLength(70)]]
    });
    this.clear();
    this.getMeasurementTypes();
  }
  omitSpecialChar(event: any) {
    let k: any;
    k = event.charCode;
    return (k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 44 || k == 45 || k == 38;
  }

  public validateMeasurementType(measurementType: string) {
    let numberRegex = /^[a-zA-Z-&,\s]*$/;
    this.valid = numberRegex.test(measurementType);
    if (!this.valid) {
      this._snackBar.open('Enter valid Measurement Type.', '', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      //snackBar  this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Measurement Type.' });
      return;
    }
  }

  private getMeasurementTypes(): void {
    this._kraMeasurementTypeService.GetKRAMeasurementType().subscribe((res: KRAMeasurementTypeData[]) => {
      this.measureTypeList = [];
      this.measureTypeList = res;
      this.length = res.length;
      this.sliceDataSource = res;
      // this.sliceDataSource = this.dataSource.slice(0,this.pageSize);
      this.dataSource = new MatTableDataSource(this.sliceDataSource);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    },
      (error: any) => {
        this._snackBar.open("Failed to get Measurement Type." + error.error, 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackBar  this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Measurement Type.' });
      }
    );
  }

  private duplicateMeasurementTypeCheck(measurementType: string): boolean {
    if (this.measureTypeList) {
      let duplicateMeasureList = this.measureTypeList.filter(function (measure: KRAMeasurementTypeData) {
        return (
          measure.MeasurementType.toLowerCase() == measurementType.toLowerCase()
        );
      });
      if ((this.saveButton == "Add" && duplicateMeasureList.length > 0) || (this.saveButton == "Update" && duplicateMeasureList.length > 1)) {
        this._snackBar.open('Measurement Type Already exists.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackBar this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Measurement Type Already exists.' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  private dataChangeCheck(measurement: KRAMeasurementTypeData): boolean {
    if (measurement && this.measurementTypeDataBeforeEdit) {
      if ((this.measurementTypeDataBeforeEdit.Id == measurement.Id) && (this.measurementTypeDataBeforeEdit.MeasurementType == measurement.MeasurementType)) {

        return false;
      }
      else {
        return true;
      }
    }
  }

  public submitForm(): void {
    //this.aspectFormSubmitted = true;
    if (this.measurementForm.valid) {
      this.measurementForm.value.MeasurementType.trim().replace(/  +/g, ' ');
      let position: number = this.measurementForm.value.MeasurementType.indexOf('-', 0);
      let position1: number = this.measurementForm.value.MeasurementType.indexOf('&', 0);
      let position2: number = this.measurementForm.value.MeasurementType.indexOf(',', 0)
      if (this.measurementForm.value.MeasurementType.charAt(position) == this.measurementForm.value.MeasurementType.charAt(position + 1) || this.measurementForm.value.MeasurementType.charAt(position1) == this.measurementForm.value.MeasurementType.charAt(position1 + 1) || this.measurementForm.value.MeasurementType.charAt(position2) == this.measurementForm.value.MeasurementType.charAt(position2 + 1)) {
        this._snackBar.open('Nothing to Save..', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackBar // this.messageService.add({
        //   severity: 'warn',
        //   summary: 'Warning Message',
        //   detail: 'Nothing to Save..'
        // });
        return;
      }
      //this.valid = false;
      if (this.measurementForm.value.MeasurementType.trim() == "") { this.measurementForm.reset(); return; }
      let duplicateAspect = this.duplicateMeasurementTypeCheck(this.measurementForm.value.MeasurementType);
      if (!duplicateAspect) return;
    } else return;
    if (this.saveButton == "Add") {
      this._kraMeasurementTypeService.CreateKraMeasurementType(this.measurementForm.value.MeasurementType).subscribe((createResponse: boolean) => {
        if (createResponse) {
          this._snackBar.open('Measurement Type Created successfully.', '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar // this.messageService.add({
          //   severity: 'success',
          //   summary: 'Success Message',
          //   detail: 'Measurement Type Created successfully'
          // });
          this.getMeasurementTypes();
          this.clear();
        }
        // else if (createResponse == -1) {          
        //   this.messageService.add({
        //     severity: 'warn',
        //     summary: 'Warning Message',
        //     detail: 'Aspect already exists!.'
        //   });
        // }
        else {
          this._snackBar.open('Failed to Create Measurement Type.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar  // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error Message',
          //   detail: 'Failed to Create Measurement Type.'
          // });
        }
      },
        error => {
          //if (error._body != undefined && error._body != "")
          this._snackBar.open('Failed to Create Measurement Type.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar  // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error Message',
          //   detail: 'Failed to Create Measurement Type.'
          // });
        }
      );
    }
    else {
      let dataChange = this.dataChangeCheck(this.measurementForm.value);
      if (!dataChange) return;
      this._kraMeasurementTypeService.UpdateKraMeasurementType(this.measurementForm.value).subscribe((updateResponse: boolean) => {
        if (updateResponse) {
          this._snackBar.open('Measurement Type Updated successfully.', '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar  this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Measurement Type Updated successfully' });
          this.getMeasurementTypes();
          this.clear();
        }
        else {
          this._snackBar.open('Failed to Update Measurement Type.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar this.messageService.add({
          //   severity: 'error', summary: 'Error Message', detail: 'Failed to Update Measurement Type.'
          // });
        }

      },
        error => {
          this._snackBar.open('Failed to Update Measurement Type.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackBar this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Update Measurement Type.' });
        }
      );
    }
  }

  editMeasurementType(measurement: KRAMeasurementTypeData): void {
    this.saveButton = "Update";
    this.measurementTypeDataBeforeEdit = new KRAMeasurementTypeData();
    this.measurementTypeDataBeforeEdit = measurement;
    this.kraMeasurementTypeData = measurement;
    this.kraMeasurement = "";
    this.kraMeasurement = measurement.MeasurementType;
    this.measurementForm.patchValue(measurement);
  }
  public clear(): void {
    this.kraMeasurementTypeData = new KRAMeasurementTypeData();
    this.kraMeasurementTypeData = {
      Id: 0,
      MeasurementType: ""
    };
    this.measurementForm.patchValue({ MeasurementType: null, Id: 0 });
    this.measurementForm.controls['MeasurementType'].setErrors(null);
    this.valid = false;
    this.saveButton = "Add";
    this.measurementFormSubmitted = false;
  }
  editMeasurement(Id: number, kraMeasurementType: string) {
    this.buttonTitle = "Update";
    if (Id && kraMeasurementType && kraMeasurementType != "") {
      this.kraMeasurementTypeData = new KRAMeasurementTypeData();
      this.kraMeasurementTypeData.Id = Id;
      this.kraMeasurementTypeData.MeasurementType = kraMeasurementType;
      this.measurementType = kraMeasurementType;
    }
  }

  // deleteMeasurement(Id: number) { 
  // //private deleteMeasurement(Id: number) {
  //   if (Id != null) {

  //     const dialogRef = this.dialog.open()
  //             ,{
  //         data: {
  //           message: 'Are you sure, you want to delete this Measurement Type?',
  //         }
  //       });
  //     dialogRef.afterClosed().subscribe(result => {
  //       if (result) {
  //         this._kraMeasurementTypeService.DeleteKraMeasurementType(Id).subscribe(
  //           (data: number) => {
  //             if (data == 1) {
  //               this._snackBar.open('KRA Measurement Type deleted successfully.', '', {
  //                 duration: 1000,
  //                 panelClass: ['success-alert'],
  //                 horizontalPosition: 'right',
  //                 verticalPosition: 'top',
  //               });
  //               //snackBar this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRA Measurement Type deleted successfully.' });
  //             } else if (data == 0) {
  //               this._snackBar.open('Failed to delete KRA Measurement Type.', '', {
  //                 duration: 1000,
  //                 horizontalPosition: 'right',
  //                 verticalPosition: 'top',
  //               });
  //               //snackBar this.messageService.add({ severity: 'warn', summary: 'Wraning Message', detail: 'Failed to delete KRA Measurement Type.' });
  //             } else if (data == 9) {
  //               this._snackBar.open('Child Dependency exists.', '', {
  //                 duration: 1000,
  //                 horizontalPosition: 'right',
  //                 verticalPosition: 'top',
  //               });
  //               //snackBar this.messageService.add({ severity: 'warn', summary: 'Wraning Message', detail: 'Child Dependency exists.' });
  //             } else {
  //               this._snackBar.open('Failed to delete KRA Measurement Type.', '', {
  //                 duration: 1000,
  //                 horizontalPosition: 'right',
  //                 verticalPosition: 'top',
  //               });
  //               //snackBar this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to delete KRA Measurement Type.' });
  //             }
  //             this.buttonTitle = "Add";
  //             this.measurementFormSubmitted = false;
  //             this.measurementType = "";
  //             this.getMeasurementTypes();
  //           },
  //           (error: any) => {
  //             this._snackBar.open('Failed to delete KRA Measurement Type.', '', {
  //               duration: 1000,
  //               horizontalPosition: 'right',
  //               verticalPosition: 'top',
  //             });
  //             //snackBar this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Sorry! Failed to delete KRA Measurement Type.' });
  //           });
  //       }
  //       else {
  //       }
  //     });
  //   }
  // }

  public cancel(): void {
    this.buttonTitle = "Add";
    this.measurementForm.reset();
    this.measurementFormSubmitted = false;
  }
}
