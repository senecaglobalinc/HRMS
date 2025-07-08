import { Component, Inject, OnInit } from '@angular/core';
import { themeconfig } from '../../../../../themeconfig';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig} from '@angular/material/dialog';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { KraMeasurementTypeService } from 'src/app/modules/kra/services/kra.measurement-type.service';
import { KRAMeasurementTypeData } from 'src/app/modules/kra/models/kra.measurement-type.model';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { AspectMasterService } from 'src/app/modules/kra/services/aspectmaster.service';
import { AspectData, OperatorData, TargetPeriodData } from 'src/app/modules/master-layout/models/kra.model';
import { KrascalemasterService } from 'src/app/modules/kra/services/krascalemaster.service';
import { KRAScaleMaster } from 'src/app/modules/kra/models/krascaleData.model';
//import { MessageService } from 'primeng/api';
import { DefinitionModel } from 'src/app/modules/kra/models/kradefinition.model';
import { KraDefinitionService } from 'src/app/modules/kra/services/kradefinition.service';
import { ApplicableRoleTypeService } from 'src/app/modules/kra/services/applicableroletype.service';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import * as moment from 'moment';
import { isObject } from 'rxjs/internal-compatibility';

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-add-kradlg',
  templateUrl: './add-kradlg.component.html',
  styleUrls: ['./add-kradlg.component.scss'],
  providers: [KraMeasurementTypeService, MasterDataService, AspectMasterService, 
    KrascalemasterService, KraDefinitionService, ApplicableRoleTypeService]
})
export class AddKRAdlgComponent {
  themeappeareance = themeconfig.formfieldappearances;

   addKRA: FormGroup;

  aspectList: SelectItem[];
  measurementTypeList: SelectItem[];
  operatorList: SelectItem[];
  targetPeriodList: SelectItem[];
  scaleList: SelectItem[];
  aspectId: number;
  measurementTypeId: number;
  showScales: boolean = false;
  operatorId: number;
  targetvalue: string;
  scaleId: number;
  targetPeriodId: number;
  //applicableRoleType: number;
  finYearId: number;
  depId: number;
  gradeId: number;
  roleId: number;
  definitionDetailsId:string
  definitionDetails:any
  definitionTransactionId:number;
  editMode = false;
  btntext: string;
  IsHOD = false;
  selectedApplicableRoleTypeId: number;
  selectedkraaspect: string;
  selectedmetrics: string;
  selectedmeasurementtype: string;
  selectedtargetvalue: string;
  selectedscalevalue: number;
  selectedoperator: number;
  selectedtargetperiod: number;
  showCalender:boolean = false;
  targetValueLength:number=3;
  kraDefinitionResponse:DefinitionModel = new DefinitionModel();

  constructor(
    private _snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<AddKRAdlgComponent>,
    private _formBuilder: FormBuilder,
    private _aspectMasterService: AspectMasterService,
    private _kraMeasurementTypeService: KraMeasurementTypeService,
    private _kraScaleMasterService: KrascalemasterService,
    private _masterDataService: MasterDataService,
    private _kraDefinitionService: KraDefinitionService,
    private _appRoleTypeService: ApplicableRoleTypeService,
    //private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) public data: AddKRAdlgComponent) {
      
    }

  ngOnInit() { 
      this.finYearId = this.data.finYearId;
      this.depId = this.data.depId;
      this.gradeId = this.data.gradeId;
      this.roleId = this.data.roleId;
      this.editMode=this.data.editMode;
      this.btntext=this.data.btntext;
      this.IsHOD=this.data.IsHOD;
      this.definitionDetailsId=this.data.definitionDetailsId;

      this.addKRA = this._formBuilder.group({
          kraaspect: [null, [Validators.required]],
          metrics: ['', [Validators.required]],
          measurementtype:[null, [Validators.required]],
          scalevalue: [null, null],
          operator: [null, [Validators.required]],
          // targetvalue: [null, [Validators.required]],
          targetvalue: ['', [Validators.required, Validators.min(0), Validators.max(999)]],
          targetperiod: [null, [Validators.required]],
      });
     // this.getSelectedApplicableRoleTypeId();
      this.getAspects();
      this.getMeasurementTypes();
      this.getOperators();
      this.getTargetPeriods();
      this.getScales(); 
      if(this.editMode)
      {       
        this.addKRA.controls['kraaspect'].disable();
        if(!this.IsHOD){
          this.GetKRAData();
        }
        else{
          this.GetKRADataForHOD()
        }
        
      }
      else  this.addKRA.controls['kraaspect'].enable();
  }

  private getAspects(): void {
    this._aspectMasterService.GetAspectMasterList().subscribe((res: AspectData[]) => {
      this.aspectList = [];
      this.aspectList.push({ label: 'Select Aspect', value: null});
      res.forEach((e: AspectData) => {
        this.aspectList.push({ label: e.AspectName, value: e.AspectId})
      })
    },
    (error: any) => {
      this._snackBar.open('Failed to get Aspects.', 'x', {​​​​​​​
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }​​​​​​​);
    }
    );
  }  

  private getMeasurementTypes(): void {
    this._kraMeasurementTypeService.GetKRAMeasurementType().subscribe((res: KRAMeasurementTypeData[]) => {
      this.measurementTypeList = [];
      this.measurementTypeList.push({ label: 'Select Measurement Type', value: null });
      res.forEach((e: KRAMeasurementTypeData) => {
        this.measurementTypeList.push({ label: e.MeasurementType, value: e.Id})
      });
    },
    (error: any) => {
      this._snackBar.open('Failed to get Measurement Types.', 'x', {​​​​​​​
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }​​​​​​​);
    }
    );
  }

  private getOperators(): void {
    this._masterDataService.GetKRAOperators().subscribe((res: OperatorData[]) => {
      this.operatorList = [];
      this.operatorList.push({ label: 'Select Operator', value: null});
      res.forEach((e: OperatorData) => {
        this.operatorList.push({ label: e.OperatorValue, value: e.OperatorId})
      });
    },
    (error: any) => {
      this._snackBar.open('Failed to get Operators.', 'x', {​​​​​​​
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }​​​​​​​);
    }
    );
  }

  private getTargetPeriods(): void {
    this._masterDataService.GetKRATargetPeriods().subscribe((res: TargetPeriodData[]) => {
      this.targetPeriodList = [];
      this.targetPeriodList.push({ label: 'Select Target Period', value: null});
      res.forEach((e: TargetPeriodData) => {
        this.targetPeriodList.push({ label: e.TargetPeriodValue, value: e.TargetPeriodId})
      });
    },
    (error: any) => {
      this._snackBar.open('Failed to get Target Periods.', 'x', {​​​​​​​
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }​​​​​​​);
    }
    );
  }

  private getScales(): void {
    this._kraScaleMasterService.GetKRAScale().subscribe((res: KRAScaleMaster[]) => {
      this.scaleList = [];
      this.scaleList.push({ label: 'Select Scale', value: null});
      res.forEach((e: KRAScaleMaster) => {
        this.scaleList.push({ label: e.ScaleTitle + '(' + e.MinimumScale.toString() + ' - ' + e.MaximumScale.toString() + ')', value: e.ScaleID})
      });
    },
    (error: any) => {
      this._snackBar.open('Failed to get Scales.', 'x', {​​​​​​​
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }​​​​​​​);
    }
    );
  }

  private getSelectedApplicableRoleTypeId(): void {
    var selectedRoleId: number = this.roleId;
    // this._appRoleTypeService.getApplicableRoleTypeById(this.finYearId,
    //   this.depId, null, this.gradeId)
    //   .subscribe(
    //     (res: any[]) => {
    //       if (res) {
    //         res.forEach((element: any) => {
    //            if (element.RoleTypeId == selectedRoleId) {
    //              this.selectedApplicableRoleTypeId = element.ApplicableRoleTypeId;
    //            }
    //         })
    //       }
    //     }
    //   );
  }

private GetKRAData(): void {
    var selectedRoleId: number = this.roleId;
    this._kraDefinitionService.GetDefinitionDetails(this.definitionDetailsId)
      .subscribe(
        (res: any[]) => {
          if (res) {             
            res.forEach((element: any) => {
              this.kraDefinitionResponse.aspectId = element.AspectId;
              this.kraDefinitionResponse.metric = element.Metric;
              this.kraDefinitionResponse.measurementTypeId =element.MeasurementTypeId;
              this.kraDefinitionResponse.targetValue = element.TargetValue;
              this.kraDefinitionResponse.operatorId = element.OperatorId;
              this.kraDefinitionResponse.targetPeriodId = element.TargetPeriodId;
              if(this.kraDefinitionResponse.scaleId != null){
                this.kraDefinitionResponse.scaleId = element.ScaleId;
              }
              else{
                this.kraDefinitionResponse.scaleId=0;
              }
              this.selectedkraaspect=this.aspectId=element.AspectId;
              this.selectedmetrics =  this.addKRA.value.metrics=element.Metric;
              this.selectedmeasurementtype= this.measurementTypeId=element.MeasurementTypeId;
              this.selectedscalevalue =this.scaleId= element.ScaleId;            
              this.selectedoperator = this.operatorId=element.OperatorId;
              this.selectedtargetvalue = this.addKRA.value.targetvalue= element.TargetValue;
              this.selectedtargetperiod = this.targetPeriodId=element.TargetPeriodId;                    
             
              this.addKRA.controls["kraaspect"].setValue(element.AspectId);
              this.addKRA.controls["metrics"].setValue(element.Metric);
              this.addKRA.controls["measurementtype"].setValue(element.MeasurementTypeId);
              this.addKRA.controls["scalevalue"].setValue(element.ScaleId);
              this.addKRA.controls["operator"].setValue(element.OperatorId);
              this.addKRA.controls["targetvalue"].setValue(element.TargetValue);
              this.addKRA.controls["targetperiod"].setValue(element.TargetPeriodId);

              if (element.MeasurementTypeId == 4) { // scale
                this.showScales = true;
                this.targetValueLength = 1;
                // this.addKRA.controls['scalevalue'].setValidators([null, Validators.required]);
                // // this.selectedscalevalue = null;
                // this.addKRA.get('scalevalue').updateValueAndValidity();
              }
              else {
                this.showScales = false;
                this.targetValueLength = 3;
              //   this.selectedscalevalue = this.scaleId = null;
              //  this.addKRA.controls['scalevalue'].setValidators([null, null]);
              //   // this.addKRA.get('scalevalue').clearValidators();
              //   this.addKRA.get('scalevalue').updateValueAndValidity();
              }  
              if(element.MeasurementTypeId == 3){ //date 
                this.showCalender = true;
              }
              else{
                this.showCalender =false;
              }
              
              if (element.MeasurementTypeId == 5) //text
              {
                this.targetValueLength = 150;
                this.addKRA.get('operator').clearValidators();
                this.addKRA.get('operator').updateValueAndValidity();
              }
              else {
                this.addKRA.get('operator').setValidators([Validators.required]);
                this.addKRA.get('operator').updateValueAndValidity();
              }
            })
          }
        }
      );
  }

  GetKRADataForHOD(){
    this.selectedkraaspect=this.aspectId=this.data.definitionDetails.AspectId;
              this.addKRA.value.metrics=this.data.definitionDetails.Metric;
              this.measurementTypeId=this.data.definitionDetails.MeasurementTypeId;
              this.scaleId= this.data.definitionDetails.ScaleId;            
              this.operatorId=this.data.definitionDetails.OperatorId;
              this.addKRA.value.targetvalue= this.data.definitionDetails.TargetValue;
              this.targetPeriodId=this.data.definitionDetails.TargetPeriodId;                    
             
              this.addKRA.controls["kraaspect"].setValue(this.data.definitionDetails.AspectId);
              this.addKRA.controls["metrics"].setValue(this.data.definitionDetails.Metric);
              this.addKRA.controls["measurementtype"].setValue(this.data.definitionDetails.MeasurementTypeId);
              this.addKRA.controls["scalevalue"].setValue(this.data.definitionDetails.ScaleId);
              this.addKRA.controls["operator"].setValue(this.data.definitionDetails.OperatorId);
              this.addKRA.controls["targetvalue"].setValue(this.data.definitionDetails.TargetValue);
              this.addKRA.controls["targetperiod"].setValue(this.data.definitionDetails.TargetPeriodId);

              if (this.data.definitionDetails.MeasurementTypeId == 4) { //scale
                this.showScales = true;
                this.targetValueLength = 1;
                // this.addKRA.controls['scalevalue'].setValidators([null, Validators.required]);
                // // this.selectedscalevalue = null;
                // this.addKRA.get('scalevalue').updateValueAndValidity();
              }
              else {
                this.showScales = false;
                this.targetValueLength = 3;
              //   this.selectedscalevalue = this.scaleId = null;
              //  this.addKRA.controls['scalevalue'].setValidators([null, null]);
              //   // this.addKRA.get('scalevalue').clearValidators();
              //   this.addKRA.get('scalevalue').updateValueAndValidity();
              }   
    if (this.data.definitionDetails.MeasurementTypeId == 5) //text
    {
      this.targetValueLength = 150;
      this.addKRA.get('operator').clearValidators();
      this.addKRA.get('operator').updateValueAndValidity();
    }
    else {
      this.addKRA.get('operator').setValidators([Validators.required]);
      this.addKRA.get('operator').updateValueAndValidity();
    }
   
  }
  onAspectChanged(aspectId: number) {
    this.aspectId = aspectId;
  }

  onMeasurementChanged(measurementTypeId: number) {
    this.showCalender=false
    this.addKRA.get('targetvalue').reset();
    this.measurementTypeId = measurementTypeId;  
    if (measurementTypeId == 4) //scale
    {
      this.showScales = true; 
      this.targetValueLength = 1;    
    }    
    else 
    {
      this.showScales = false;
      this.targetValueLength = 3;
    }
    if(measurementTypeId == 3) //date
    {
      this.showCalender = true
    }
    else{
      this.showCalender = false
    }
    if(measurementTypeId == 5) //text
    {
      this.targetValueLength = 150;
      this.addKRA.get('operator').clearValidators();   
      this.addKRA.get('operator').updateValueAndValidity();
    }
    else{
      this.addKRA.get('operator').setValidators([Validators.required]);
      this.addKRA.get('operator').updateValueAndValidity();
    }
  }

  onKeyDown(e,measurementtypeId:number){

    const excludedKeys = [8, 37, 39, 46];
    let regex: RegExp = /^([a-zA-Z0-9 ]*)$/;
    if (!regex.test(e.key)) { // checking for special characters
      // when the user is giving special characters in between the input
      e.preventDefault();
    }

    else{
      if(measurementtypeId == 3 || measurementtypeId == 5) // don't do anything for text or date
      {
        return;
      }
  
      else {
        if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105) || (excludedKeys.includes(e.keyCode)))) { 
          e.preventDefault();
        }
      }

    }
  }

  clearInput(): void {
    this.addKRA.get('targetvalue').reset();
  }


  onOperatorChanged(operatorId: number) {
    this.operatorId = operatorId;
  }

  onScaleChanged(scaleId: number) {
    this.scaleId =this.selectedscalevalue= scaleId
  }

  onTargetPeriodChanged(targetPeriodId: number) {
    this.targetPeriodId = targetPeriodId;
  }

  onNoClick(): void {    
    this.dialogRef.close(true);
  }

  onAddClick(): void { 
  
    if (this.addKRA.valid) {

      let definitionModel: DefinitionModel;
      definitionModel = new DefinitionModel();
      definitionModel.roleTypeId = this.roleId;
      definitionModel.financialYearId = this.finYearId;      
      definitionModel.aspectId = this.aspectId;
      definitionModel.measurementTypeId = this.measurementTypeId;
      //definitionModel.definitionId = '';

     
      definitionModel.operatorId = this.operatorId;
      definitionModel.targetPeriodId = this.targetPeriodId;
      // definitionModel.Metric = this.addKRA.controls.metrics.value;
      // definitionModel.TargetValue = this.addKRA.controls.targetvalue.value;
      definitionModel.metric = this.addKRA.value.metrics;
      if(typeof(this.addKRA.value.targetvalue) != "string"){
        definitionModel.targetValue = moment(this.addKRA.value.targetvalue).format('DD-MM-YYYY')
      }
      else {
      definitionModel.targetValue = this.addKRA.value.targetvalue;
      }
      definitionModel.currentUser=JSON.parse(sessionStorage.AssociatePortal_UserInformation).email;
      definitionModel.createdBy=JSON.parse(sessionStorage.AssociatePortal_UserInformation).fullName;
      // definitionModel.IsHODApproved = false;
      // definitionModel.IsCEOApproved = false;
      // definitionModel.IsDeleted = false;
      // definitionModel.IsHOD = this.IsHOD;
      // definitionModel.SourceDefinitionId = 0;
      definitionModel.isActive =  true;
      if (this.measurementTypeId == 4){
        definitionModel.scaleId = this.scaleId;
      } 
      else {
        definitionModel.scaleId = 0;
      }
      
      if (this.editMode == false) {   
        if(this.IsHOD){
          this._kraDefinitionService
          .AddKRAByHOD(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this._snackBar.open(error.error, 'x', {​​​​​​​
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }​​​​​​​);
            }
          );
        }
        else{
        this._kraDefinitionService
          .CreateDefinition(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this._snackBar.open(error.error, 'x', {​​​​​​​
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }​​​​​​​);
            }
          );
        }
      } else {
        definitionModel.definitionId = this.definitionDetailsId;
        definitionModel.modifiedBy=JSON.parse(sessionStorage.AssociatePortal_UserInformation).fullName;
        definitionModel.definitionTransactionId = this.data.definitionTransactionId
        if(this.IsHOD){
          this._kraDefinitionService
          .EditKRAByHOD(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this._snackBar.open(error.error, 'x', {​​​​​​​
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }​​​​​​​);
            }
          );
        }
      else{
        if(definitionModel.aspectId == this.kraDefinitionResponse.aspectId && this.kraDefinitionResponse.measurementTypeId == definitionModel.measurementTypeId && this.kraDefinitionResponse.metric == definitionModel.metric && this.kraDefinitionResponse.operatorId == definitionModel.operatorId && this.kraDefinitionResponse.targetPeriodId == definitionModel.targetPeriodId && this.kraDefinitionResponse.targetValue == definitionModel.targetValue && this.kraDefinitionResponse.scaleId == definitionModel.scaleId){

          this._snackBar.open('No Changes to Update', 'x', {​​​​​​​
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }​​​​​​​);
          return;
        }
        this._kraDefinitionService
          .UpdateKRA(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this._snackBar.open(error.error, 'x', {​​​​​​​
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }​​​​​​​);
            }
          );
      }
      }  
    }
  }

}
