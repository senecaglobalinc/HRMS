import { Component, OnInit, ViewChild } from '@angular/core';
import { MasterDataService } from 'src/app/services/masterdata.service';
import { ConfirmationService, Message, MessageService } from 'primeng/api';
import { KraMeasurementTypeService } from '../Services/kra.measurement-type.service';
import { DropDownType, GenericType } from 'src/app/models/dropdowntype.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { KRAMeasurementTypeData } from 'src/app/models/kra.measurement-type.model';
import { ActivatedRoute } from '@angular/router';
import * as servicePath from '../../../service-paths';

// Start - This is just for Demo Purpose can be removed later if material data table is not required
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
//import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-kra.measurement-type',
  templateUrl: './kra.measurement-type.component.html',
  styleUrls: ['./kra.measurement-type.component.scss'],
  providers: [MasterDataService, ConfirmationService, KraMeasurementTypeService]
})

export class KraMeasurementTypeComponent {
// Start - This is just for Demo Purpose can be removed later if material data table is not required
displayedColumns: string[] = ['MeasurementType', 'Edit'];
dataSource;
sliceDataSource = []
length: number;
pageSize = 5;
pageSizeOptions: number[] = [5, 10, 20,30];

@ViewChild(MatPaginator) paginator: MatPaginator;
// End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;
  errorMessage: Message[] = [];
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
  kraMeasurement:string;
  valid: boolean = false;

  constructor(
    private _masterDataService: MasterDataService, 
    private _actRoute: ActivatedRoute, 
    private _formBuilder: FormBuilder, 
    private _kraMeasurementTypeService: KraMeasurementTypeService, 
    private _confirmationService: ConfirmationService,
    private messageService: MessageService) {
      this.componentName = this._actRoute.routeConfig.component.name;
      this.buttonTitle = "Add";
      this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
      this.kraMeasurementTypeData = new KRAMeasurementTypeData();
      
  }

  ngOnInit() {
      this.measurementForm = this._formBuilder.group({
          Id: [0],
          MeasurementType: ["", [Validators.required, Validators.pattern(/^[a-zA-Z-&,\s()]*$/), Validators.maxLength(70)]]
      });
      this.clear();
      this.getMeasurementTypes();
  }

onPageChanged(e) {
    let firstCut = e.pageIndex * e.pageSize;
    let secondCut = firstCut + e.pageSize;
    this.sliceDataSource = this.dataSource.slice(firstCut, secondCut);
  }


  cols = [
    { field: 'MeasurementType', header: 'Measurement Type Name' }
  ];

  omitSpecialChar(event: any) {
      let k: any;
      k = event.charCode;
      return (k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 44 || k == 45 || k == 38;
  }

  public validateMeasurementType(measurementType: string) {
      let numberRegex = /^[a-zA-Z-&,\s]*$/;
      this.valid = numberRegex.test(measurementType);
      if (!this.valid) {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Measurement Type.' });
          return;
      }
  }

  private getMeasurementTypes(): void {
      this._kraMeasurementTypeService.GetKRAMeasurementType().subscribe((res: KRAMeasurementTypeData[]) => {
        this.measureTypeList = [];
        this.measureTypeList = res;
        this.length = res.length;
        this.dataSource = res;
        this.sliceDataSource = this.dataSource.slice(0,this.pageSize);
      },
          (error: any) => {
              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Measurement Type.' });
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
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Measurement Type Already exists.' });
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
      if(this.measurementForm.valid){
          this.measurementForm.value.MeasurementType.trim().replace(/  +/g, ' ');
          let position: number = this.measurementForm.value.MeasurementType.indexOf('-', 0);
      let position1: number = this.measurementForm.value.MeasurementType.indexOf('&', 0);
      let position2: number = this.measurementForm.value.MeasurementType.indexOf(',', 0)
      if (this.measurementForm.value.MeasurementType.charAt(position) == this.measurementForm.value.MeasurementType.charAt(position + 1) || this.measurementForm.value.MeasurementType.charAt(position1) == this.measurementForm.value.MeasurementType.charAt(position1 + 1) || this.measurementForm.value.MeasurementType.charAt(position2) == this.measurementForm.value.MeasurementType.charAt(position2 + 1)) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning Message',
          detail: 'Nothing to Save..'
        });
        return;
      }
       //this.valid = false;
       if (this.measurementForm.value.MeasurementType.trim() == "") {this.measurementForm.reset(); return;}
       let duplicateAspect = this.duplicateMeasurementTypeCheck(this.measurementForm.value.MeasurementType);
       if (!duplicateAspect) return;
     } else return;
     if (this.saveButton == "Add") {
        this._kraMeasurementTypeService.CreateKraMeasurementType(this.measurementForm.value.MeasurementType).subscribe((createResponse: boolean) => {
            if (createResponse) {
            this.messageService.add({
              severity: 'success',
              summary: 'Success Message',
              detail: 'Measurement Type Created successfully'
            });
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
            this.messageService.add({
              severity: 'error',
              summary: 'Error Message',
              detail: 'Failed to Create Measurement Type.'
            });
          }
        },
        error => {
            //if (error._body != undefined && error._body != "")
            this.messageService.add({
              severity: 'error',
              summary: 'Error Message',
              detail: 'Failed to Create Measurement Type.'
            });
          }
        );
      }
      else {
        let dataChange = this.dataChangeCheck(this.measurementForm.value);
        if (!dataChange) return;
        this._kraMeasurementTypeService.UpdateKraMeasurementType(this.measurementForm.value).subscribe((updateResponse: boolean) => {
            if (updateResponse) {
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Measurement Type Updated successfully' });
            this.getMeasurementTypes();
            this.clear();
          }
          // else if (updateResponse == -1) {
          //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Aspect already exists!.' });
          // }
          // else if (updateResponse == -14) {
          //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'This Aspect is mapped to KRAs, You cannot update' });
          // }
          // else if (updateResponse == -15) {
          //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid aspect name length'
          //   });
          // }
          // else if (updateResponse == -16) {
          //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Aspect'
          //   });
          // }
          else {
            this.messageService.add({
              severity: 'error', summary: 'Error Message', detail: 'Failed to Update Measurement Type.'
            });
          }
  
        },
          error => {
            //if (error._body != undefined && error._body != "")
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Update Measurement Type.' });
          }
        );
      }
    }

    private editMeasurementType(measurement: KRAMeasurementTypeData): void {
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
    this.measurementForm.patchValue({MeasurementType: null, Id: 0 });
    this.measurementForm.controls['MeasurementType'].setErrors(null);
    this.valid = false;
    this.saveButton = "Add";
    this.measurementFormSubmitted = false;
  }

//   private createKraMeasurementType() {
//       this.measurementFormSubmitted = true;
//       if (this.measurementForm.valid) {
//           if (this.measurementType != undefined && this.measurementType != "") {
//               let position: number = this.measurementType.indexOf('-', 0);
//               let position1: number = this.measurementType.indexOf('&', 0);
//               let position2: number = this.measurementType.indexOf(',', 0)
//               if (this.measurementType.trim().length == 0
//                   || !this.valid || this.measurementType.charAt(position) == this.measurementType.charAt(position + 1) || this.measurementType.charAt(position1) == this.measurementType.charAt(position1 + 1)
//                   || this.measurementType.charAt(position2) == this.measurementType.charAt(position2 + 1)) {
//                   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Measurement Type.' });
//               } else {
//                   this.kraMeasurementTypeData.MeasurementType = this.measurementType.trim();
//                   this._kraMeasurementTypeService.CreateKraMeasurementType(this.kraMeasurementTypeData).subscribe(
//                       (data: number) => {
//                           if (data == 1) {
//                               this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRA Measurement Type added successfully.' });
//                             } else if (data == -1) {
//                               this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'KRA Measurement Type already exists.' });
//                           } else {
//                               this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to add KRA Measurement Type.' });
//                           }
//                           this.measurementFormSubmitted = false;
//                           this.measurementType = "";
//                           this.getMeasurementTypes();
//                       },
//                       (error: any) => {
//                           this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Sorry! Failed to add KRA Measurement Type.' });
//                       });
//               }
//           }
//       }
//   }

  private editMeasurement(Id: number, kraMeasurementType: string) {
      this.buttonTitle = "Update";
      if (Id && kraMeasurementType && kraMeasurementType != "") {
          this.kraMeasurementTypeData = new KRAMeasurementTypeData();
          this.kraMeasurementTypeData.Id = Id;
          this.kraMeasurementTypeData.MeasurementType = kraMeasurementType;
          this.measurementType = kraMeasurementType;
      }
  }

//   private updateKraMeasurementType() {
//       this.measurementFormSubmitted = true;
//       if (
//           this.kraMeasurementTypeData.Id && this.measurementType && this.measurementType != "") {
//           let position: number = this.measurementType.indexOf('-', 0);
//           let position1: number = this.measurementType.indexOf('&', 0);
//           let position2: number = this.measurementType.indexOf(',', 0)
//           if (this.measurementType.trim().length == 0
//               || !this.valid || this.measurementType.charAt(position) == this.measurementType.charAt(position + 1) || this.measurementType.charAt(position1) == this.measurementType.charAt(position1 + 1)
//               || this.measurementType.charAt(position2) == this.measurementType.charAt(position2 + 1)) {
//               this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Measurement Type.' });
//           } else {
//               this.kraMeasurementTypeData.MeasurementType = this.measurementType.trim();
//               this._kraMeasurementTypeService.UpdateKraMeasurementType(this.kraMeasurementTypeData).subscribe(
//                   (data: number) => {
//                       if (data == 1) {

//                           this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRA Measurement Type updated successfully.' });
//                       } else if (data == -1) {
//                           this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'KRA Measurement Type already exists.' });
//                       } else if (data == -11) {
//                           this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Nothing to Save.' });
//                       } else if (data == 9) {
//                           this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Child Dependency exists.' });
//                       } else {
//                           this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to update KRA Measurement Type.' });
//                       }
//                       this.buttonTitle = "Add";
//                       this.measurementFormSubmitted = false;
//                       this.measurementType = "";
//                       this.getMeasurementTypes();
//                   },
//                   (error: any) => {
//                       this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Sorry! Failed to update KRA Measurement Type.' });
//                   });
//           }
//       }
//   }

  private deleteMeasurement(Id: number) {
      if (Id != null) {
          this._confirmationService.confirm({
              message: 'Are you sure, you want to delete this Measurement Type?',
              header: 'Measurement Type Confirmation',
              key: 'kraMeasurementType',
              icon: 'fa fa-trash',
              accept: () => {
                  this._kraMeasurementTypeService.DeleteKraMeasurementType(Id).subscribe(
                      (data: number) => {
                          if (data == 1) {
                              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRA Measurement Type deleted successfully.' });
                          } else if (data == 0) {
                              this.messageService.add({ severity: 'warn', summary: 'Wraning Message', detail: 'Failed to delete KRA Measurement Type.' });
                          } else if (data == 9) {
                              this.messageService.add({ severity: 'warn', summary: 'Wraning Message', detail: 'Child Dependency exists.' });
                          } else {
                              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to delete KRA Measurement Type.' });
                          }
                          this.buttonTitle = "Add";
                          this.measurementFormSubmitted = false;
                          this.measurementType = "";
                          this.getMeasurementTypes();
                      },
                      (error: any) => {
                          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Sorry! Failed to delete KRA Measurement Type.' });
                      });
              },
              reject: () => {
              }
          });
      }
  }

  public cancel(): void {
      this.buttonTitle = "Add";
      this.measurementForm.reset();
      this.measurementFormSubmitted = false;
  }
}
