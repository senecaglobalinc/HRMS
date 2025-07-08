import { Component, Inject, OnInit } from '@angular/core';
import { themeconfig } from 'themeconfig';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { KraMeasurementTypeService } from '../Services/kra.measurement-type.service';
import { KRAMeasurementTypeData } from 'src/app/models/kra.measurement-type.model';
import { MasterDataService } from 'src/app/services/masterdata.service';
import { AspectMasterService } from '../Services/aspectmaster.service';
import { AspectData, OperatorData, TargetPeriodData } from 'src/app/models/kra.model';
import { KrascalemasterService } from '../Services/krascalemaster.service';
import { KRAScaleMaster } from 'src/app/models/krascaleData.model';
import { MessageService } from 'primeng/api';
import { DefinitionModel } from 'src/app/models/kradefinition.model';
import { KraDefinitionService } from '../Services/kradefinition.service';
import { ApplicableRoleTypeService } from '../Services/applicableroletype.service';

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-add-kradlg',
  templateUrl: './add-kradlg.component.html',
  styleUrls: ['./add-kradlg.component.scss'],
  providers: [KraMeasurementTypeService, MasterDataService, AspectMasterService, 
    KrascalemasterService, KraDefinitionService, ApplicableRoleTypeService, MessageService]
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
  definitionDetailsId:number;
  editMode = false;
  IsHOD = false;
  selectedApplicableRoleTypeId: number;
  selectedkraaspect: string;
  selectedmetrics: string;
  selectedmeasurementtype: string;
  selectedtargetvalue: string;
  selectedscalevalue: number;
  selectedoperator: number;
  selectedtargetperiod: number;

  constructor(
    public dialogRef: MatDialogRef<AddKRAdlgComponent>,
    private _formBuilder: FormBuilder,
    private _aspectMasterService: AspectMasterService,
    private _kraMeasurementTypeService: KraMeasurementTypeService,
    private _kraScaleMasterService: KrascalemasterService,
    private _masterDataService: MasterDataService,
    private _kraDefinitionService: KraDefinitionService,
    private _appRoleTypeService: ApplicableRoleTypeService,
    private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) public data: AddKRAdlgComponent) {
      
    }

  ngOnInit() {      
      this.finYearId = this.data.finYearId;
      this.depId = this.data.depId;
      this.gradeId = this.data.gradeId;
      this.roleId = this.data.roleId;
      this.editMode=this.data.editMode;
      this.IsHOD=this.data.IsHOD;
      this.definitionDetailsId=this.data.definitionDetailsId;

      this.addKRA = this._formBuilder.group({
          kraaspect: [null, [Validators.required]],
          metrics: ['Achieve required competency development (Associate to acquire HR domain, Technology & Tools, Management, Process, Information Security, and Communication competence to meet current department role and career development requirements identified for the year)', [Validators.required]],
          measurementtype:[null, [Validators.required]],
          scalevalue: [null, null],
          operator: [null, [Validators.required]],
          targetvalue: [null, [Validators.required]],
          //targetvalue: ['', [Validators.required, Validators.min(1), Validators.max(5)]],
          targetperiod: [null, [Validators.required]],
      });
      this.getSelectedApplicableRoleTypeId();
      this.getAspects();
      this.getMeasurementTypes();
      this.getOperators();
      this.getTargetPeriods();
      this.getScales();     
      if(this.editMode)
      {       
        this.addKRA.controls['kraaspect'].disable();
        this.GetKRAData();
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
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Aspects.' });
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
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Measurement Types.' });
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
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Operators.' });
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
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Target Periods.' });
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
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Scales.' });
    }
    );
  }

  private getSelectedApplicableRoleTypeId(): void {
    var selectedRoleId: number = this.roleId;
    this._appRoleTypeService.getApplicableRoleTypeById(this.finYearId,
      this.depId, null, this.gradeId)
      .subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((element: any) => {
               if (element.RoleTypeId == selectedRoleId) {
                 this.selectedApplicableRoleTypeId = element.ApplicableRoleTypeId;
               }
            })
          }
        }
      );
  }

private GetKRAData(): void {
    var selectedRoleId: number = this.roleId;
    this._kraDefinitionService.GetDefinitionDetails(this.definitionDetailsId)
      .subscribe(
        (res: any[]) => {
          if (res) {             
            res.forEach((element: any) => {              
              this.selectedkraaspect=this.aspectId=element.AspectId;
              this.selectedmetrics =  this.addKRA.value.metrics=element.Metric;
              this.selectedmeasurementtype= this.measurementTypeId=element.MeasurementTypeId;
              this.selectedscalevalue =this.scaleId= element.ScaleId;            
              this.selectedoperator = this.operatorId=element.OperatorId;
              this.selectedtargetvalue = this.targetvalue= element.TargetValue;
              this.selectedtargetperiod = this.targetPeriodId=element.TargetPeriodId;                    
             
              if (element.MeasurementTypeId == 1) {
                this.showScales = true;
                // this.addKRA.controls['scalevalue'].setValidators([null, Validators.required]);
                // // this.selectedscalevalue = null;
                // this.addKRA.get('scalevalue').updateValueAndValidity();
              }
              else {
                this.showScales = false;
              //   this.selectedscalevalue = this.scaleId = null;
              //  this.addKRA.controls['scalevalue'].setValidators([null, null]);
              //   // this.addKRA.get('scalevalue').clearValidators();
              //   this.addKRA.get('scalevalue').updateValueAndValidity();
              }       
            })
          }
        }
      );
  }
  onAspectChanged(aspectId: number) {
    this.aspectId = aspectId;
  }

  onMeasurementChanged(measurementTypeId: number) {
    this.measurementTypeId = measurementTypeId;  
    if (measurementTypeId == 1) 
    {
      this.showScales = true;      
      // this.addKRA.controls['scalevalue'].setValidators([null, Validators.required]);
      // // this.selectedscalevalue = null;
      // this.addKRA.get('scalevalue').updateValueAndValidity();       
    }    
    else 
    {
      this.showScales = false;
      // this.selectedscalevalue = this.scaleId = null;
      //  this.addKRA.controls['scalevalue'].setValidators([null, null]);
      // // this.addKRA.get('scalevalue').clearValidators();
      // this.addKRA.get('scalevalue').updateValueAndValidity(); 
    }    
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

      definitionModel.ApplicableRoleTypeId = this.selectedApplicableRoleTypeId;

      definitionModel.AspectId = this.aspectId;
      definitionModel.MeasurementTypeId = this.measurementTypeId;

      if (this.measurementTypeId == 1) definitionModel.ScaleId = this.scaleId;
      else definitionModel.ScaleId = 0;

      definitionModel.OperatorId = this.operatorId;
      definitionModel.TargetPeriodId = this.targetPeriodId;
      // definitionModel.Metric = this.addKRA.controls.metrics.value;
      // definitionModel.TargetValue = this.addKRA.controls.targetvalue.value;
      definitionModel.Metric = this.addKRA.value.metrics;
      definitionModel.TargetValue = this.selectedtargetvalue;

      definitionModel.IsHODApproved = false;
      definitionModel.IsCEOApproved = false;
      definitionModel.IsDeleted = false;
      definitionModel.IsHOD = this.IsHOD;
      definitionModel.SourceDefinitionId = 0;

      if (this.editMode == false) {       
        this._kraDefinitionService
          .CreateDefinition(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this.messageService.add({
                severity: "error",
                summary: "Error Message",
                detail: error.error
              });
            }
          );
      } else {
        definitionModel.DefinitionDetailsId = this.definitionDetailsId;
        this._kraDefinitionService
          .UpdateKRA(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              this.messageService.add({
                severity: "error",
                summary: "Error Message",
                detail: error.error
              });
            }
          );
      }  
    }
  }

}
