import { Component, OnInit } from '@angular/core';
import { DropDownType, GenericType } from '../../../models/dropdowntype.model';
import { KRAScaleData } from '../../../models/krascaleData.model';
import { CommonService } from '../../../services/common.service';
import { KraAspectData } from '../../../models/kra.model';
import { KRADefinition, MetricAndTarget } from '../../../models/kradefinition.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { MenuItem, MessageService, ConfirmationService } from 'primeng/api';
import { KRAService } from '../Services/kra.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { KRAMeasurementType,Months, KRAStatusCodes } from '../../shared/utility/enums';
import * as moment from "moment";
import { Router, ActivatedRoute } from '@angular/router';
import {themeconfig} from 'themeconfig';
  


@Component({
  selector: 'app-add-kradefinition',
  templateUrl: './add-kradefinition.component.html',
  styleUrls: ['./add-kradefinition.component.scss'],
  providers: [MasterDataService, KRAService, ConfirmationService, MessageService]})

  export class AddKradefinitionComponent implements OnInit {

    themeappeareance = themeconfig.formfieldappearances;
    addNewKra:boolean=false;

  loggedinUserRole: string;
    componentName: string;
    financialYearName: string;
    financialYearId: number;
    kraGroupId: number;
    kraTitle: string;
    kraDefinitionData: KRADefinition;
    kraMericAndTarget: MetricAndTarget;
    kraDefinitionList: KRADefinition[] = [];
    kraDefinitionForm: FormGroup;
    kraFormSubmitted: boolean = false;
    errorMessage: Message[] = [];
    kraAspectsList: DropDownType[] = [];
    menuItems: MenuItem[];
    disableAspect: boolean = false;
    selectedKraDefinitionData: KRADefinition;
    buttonType: string = 'add';
    deleteMetric: boolean = false;
    hideContextMenu: boolean = false;
    departmentId: number;
    type: string;
    showAddSection: boolean = false;
    KRAMetricsAndTargets: any[] = [];
    measurementList: DropDownType[] = [];
    periodicityList: DropDownType[] = [];
    measureTypeList: DropDownType[] = [];
    scalevaluesList: DropDownType[] = [];
    scaleDescriptionList: GenericType[] = [];
    isscaleType: boolean = false;
    scalemasterList: KRAScaleData[] = [];
    isValidTargetvalue: boolean = false;
    showScaleDescriptionDialog: boolean = false;
    IsCollapsed: boolean = false;
    IsDateMeasurementType: boolean = false;
    yearsRange: string = "";
    
  
      constructor(private _kraService: KRAService, private masterDataService: MasterDataService,
        private _router: Router, private _commonService: CommonService,private _formBuilder: FormBuilder,private _confirmationService: ConfirmationService, private actRoute: ActivatedRoute,private messageService: MessageService ) {  
        this.componentName = this.actRoute.routeConfig.component.name;
        let KraGroupDetails = JSON.parse(sessionStorage["KraGroupDetails"]);
        this.kraGroupId = KraGroupDetails.KRAGroupId;
        this.financialYearId = KraGroupDetails.FinancialYearId;        
        this.kraTitle = KraGroupDetails.KRATitle;
        this.departmentId = KraGroupDetails.DepartmentId;
        this.actRoute.params.subscribe(params => { this.financialYearName = params['financialyear']; });
        this.actRoute.params.subscribe(params => { this.type = params['type'] });
        this.setYearsRange();
        this.kraDefinitionData = new KRADefinition();
        this.kraMericAndTarget = new MetricAndTarget();
        this.kraDefinitionList = new Array<KRADefinition>();
        this.selectedKraDefinitionData = new KRADefinition();
        if (this.type == "add")
            this.showAddSection = true;
        else if (this.type == "view")
            this.showAddSection = false;
    }

    setYearsRange() {
        this.yearsRange = this.financialYearName.replace(' - ', ':');
    }
  ngOnInit() {
    this.loggedinUserRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    
    this.kraDefinitionForm = this._formBuilder.group({
        aspect: [null, [Validators.required]],
        metric: ['', [Validators.required]],
        measurement: ['', [Validators.required]],
        measureType: ['', [Validators.required]],
        targetValue: [null],
        KRATargetValueAsDate: [null],
        targettext: [''],
        periodicity: ['', [Validators.required]],
        scaleDesc: [''],
        scalevalues: ['']

    });
    
    this.getKraAspects();
    this.getMeasurements();
    this.getMeasureTypes();  
    this.getScaleValues();
    this.getScaleDescriptions();
    this.getPeriodicity();
    this.getKraDefinitions();
     
  }
  cols = [{ field: 'KRAAspectName', header: 'KRA Aspect'}];
  
  private getKraAspects(): void {
    this.kraAspectsList = [];
    this.kraAspectsList.push({ label: 'Select KRA Aspect', value: null });
    this._kraService.getKraAspects(this.departmentId).subscribe((aspectsData: KraAspectData[]) => {
      if (aspectsData.length > 0) {
        aspectsData.forEach((e: KraAspectData) => {
          this.kraAspectsList.push({ label: e.KRAAspectName, value: e.KRAAspectID });

        });
      }
    },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get kra aspects.'
        });
      }
    );
  }
  private getMeasurements(): void {
    this.measurementList = [];
    this.measurementList.push({ label: 'Select Operator', value: null });
    this.masterDataService.GetKRAOperators().subscribe((res: GenericType[]) => {
        res.forEach(element => {
            this.measurementList.push({ label: element.Name, value: element.Id });
        });
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to get measurements.'
      });
    });
  }
  private getScaleValues() {
    this.masterDataService.getKRAScales().subscribe((res: KRAScaleData[]) => {
        this.scalevaluesList = [];
        this.scalevaluesList.push({ label: 'Select Scale', value: null });
        this.scalemasterList = res;
        res.forEach(element => {
            this.scalevaluesList.push({ label: element.ScaleLevel, value: element.ScaleID });
        });
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to get measurements.' 
      });
    });
  }
  private ShowScaleControls() {
    if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Scale) {
        this.isscaleType = true;
        this.IsDateMeasurementType = false;
    }
    else if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Date) {
        this.kraMericAndTarget.KRAScaleMasterId = null;
        this.kraMericAndTarget.KRAScaleDescription = '';
        this.isscaleType = false;
        this.IsDateMeasurementType = true;
        this.isValidTargetvalue = false;
        this.kraDefinitionForm.controls['KRATargetValueAsDate'].reset();
    }
    else {
        this.kraMericAndTarget.KRAScaleMasterId = null;
        this.kraMericAndTarget.KRAScaleDescription = '';
        this.isscaleType = false;
        this.isValidTargetvalue = false;
        this.IsDateMeasurementType = false;
        this.kraDefinitionForm.controls['targetValue'].reset();
        this.kraDefinitionForm.controls['KRATargetValueAsDate'].reset();
    }
}
private getScaleDescriptions() {
    this.masterDataService.getKRAScaleValues().subscribe((res: GenericType[]) => {
        this.scaleDescriptionList = [];
        this.scaleDescriptionList = res;
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to scale description list.'
      });
    });
  }
  private getScaleDesriptionByScaleMasterId() {
    if (this.kraMericAndTarget.KRAScaleMasterId != null) {
        this.kraDefinitionForm.controls['targetValue'].reset();
        this.kraDefinitionForm.controls['KRATargetValueAsDate'].reset();
        if ((this.scaleDescriptionList.filter(i => i.Id == this.kraMericAndTarget.KRAScaleMasterId)).length > 0)
            this.kraMericAndTarget.KRAScaleDescription = this.scaleDescriptionList.filter(i => i.Id == this.kraMericAndTarget.KRAScaleMasterId)[0].Name;
        else
            this.kraMericAndTarget.KRAScaleDescription = '';
    }
    else {
        this.kraDefinitionForm.controls['targetValue'].reset();
        this.kraDefinitionForm.controls['KRATargetValueAsDate'].reset();
        this.isValidTargetvalue = false;
        this.kraMericAndTarget.KRAScaleDescription = '';
    }
  }
  private validateTargetValue(kraMeasurementTypeId: number, kraScaleMasterId: number, targetvalue?: number, KRATargetValueAsDate?: Date) {
    let res: KRAScaleData[] = [];
    let scaleRegex = /^\d{0,2}(\.\d{1,2})?$/;
    let scaleValidation = targetvalue != null ? scaleRegex.test((targetvalue).toString()) : null;
    let percentageRegex =/^\d{0,3}(\.?\d{1,2})?$/;
    let percentageValidation = targetvalue != null ? percentageRegex.test((targetvalue).toString()) : null;
    let numberRegex = /^\d{1,3}(\.?\d{1,2})?$/;
    let numberValidation = targetvalue != null ? numberRegex.test((targetvalue).toString()) : null;
    if (kraMeasurementTypeId == KRAMeasurementType.Scale && kraScaleMasterId && targetvalue) {
        res = this.scalemasterList.filter(i => {
            return i.ScaleID == kraScaleMasterId
        }
        );
        if (targetvalue < res[0].MinimumScale || targetvalue > res[0].MaximumScale || !scaleValidation) {
            this.isValidTargetvalue = true;
        }
        else {
            this.isValidTargetvalue = false;
        }
    }
    else if (kraMeasurementTypeId == KRAMeasurementType.Percentage && targetvalue) {
        if (targetvalue > 100 || !percentageValidation) {
            this.isValidTargetvalue = true;
        }
        else {
            this.isValidTargetvalue = false;
        }
    }
    else if (kraMeasurementTypeId == KRAMeasurementType.Number && targetvalue) {
        if (targetvalue > 999 || !numberValidation) {
            this.isValidTargetvalue = true;
        }
        else {
            this.isValidTargetvalue = false;
        }
    }
    else if (kraMeasurementTypeId == KRAMeasurementType.Date && KRATargetValueAsDate) {
        let fnYear: any[] = this.financialYearName.split(' - ');
        var givenmonthandYear = moment(KRATargetValueAsDate).format("YYYY-MM-DD").split('-');
        if (((Number(givenmonthandYear[0]) == Number(fnYear[0]) && (Number(givenmonthandYear[1]) >= Months.April)) || (Number(givenmonthandYear[0]) == Number(fnYear[1])) && (Number(givenmonthandYear[1]) <= Months.March))) {
            this.isValidTargetvalue = false;
        }
        else {
            this.isValidTargetvalue = true;
        }
    }
    else {
        this.isValidTargetvalue = false;
    }
  }
  private getPeriodicity(): void {
    this.masterDataService.GetKRATargetPeriods().subscribe((res: GenericType[]) => {
        this.periodicityList = [];
        this.periodicityList.push({ label: 'Select Period', value: null });
        res.forEach(element => {
            this.periodicityList.push({ label: element.Name, value: element.Id });
        });
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to get periods.'
      });
    });
  }
  private getMeasureTypes(): void {
    this.masterDataService.GetKRAMeasurementType().subscribe((res: GenericType[]) => {
        this.measureTypeList = [];
        this.measureTypeList.push({ label: 'Select Measurement Type', value: null });
        res.forEach(element => {
            this.measureTypeList.push({ label: element.Name, value: element.Id });
        });
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to get Measurement Type.'
      });
    });
  }
  private getKraDefinitions(): void {
    if (this.financialYearId && this.kraGroupId) {
        this._kraService.getKraDefinitions(this.kraGroupId, this.financialYearId).subscribe((resultData: KRADefinition[]) => {
            if (resultData.length > 0) {
                this.kraDefinitionList = [];
                this.kraDefinitionList = resultData;
            }
            else this.kraDefinitionList = [];

        }, 
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error Message',
            detail: 'Failed to get KRA definitions.'
          });
        });
    }
  }
  public onBackButtonClick(): void {
    this._router.navigate(['/kra/kradefinitions/' + this.financialYearId + '/' + this.departmentId]);
  }
  private save(): void {
    this.kraFormSubmitted = true;
    this.createKraDefinition();
  }
  private createKraDefinition(): void {
    if (this.kraDefinitionForm.valid && !this.isValidTargetvalue) {
        if (this.kraMericAndTarget.KRAMeasurementTypeId != KRAMeasurementType.Date && this.kraMericAndTarget.KRATargetValue == null) return;
        if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Date && this.kraMericAndTarget.KRATargetValueAsDate == null) return;
        if (this.kraMericAndTarget.Metric.trim().length <= 0) {
          this.messageService.add({ severity: 'warn',
           summary: 'Warn Message', detail: 'Please provide valid Metric.' }); 
           return;
        }
        else {
            if (this.kraMericAndTarget.KRATargetText != undefined && this.kraMericAndTarget.KRATargetText != null)
                this.kraMericAndTarget.KRATargetText = this.kraMericAndTarget.KRATargetText.trim();
            this.kraDefinitionData = new KRADefinition();
            this.kraDefinitionData.KRAAspectId = this.kraMericAndTarget.KRAAspectId;
            this.kraDefinitionData.KRAGroupId = this.kraGroupId;
            this.kraDefinitionData.FinancialYearId = this.financialYearId;
            this.kraDefinitionData.RoleName = this.loggedinUserRole;
            this.kraDefinitionData.lstMetricAndTarget = [];
            this.kraDefinitionData.lstMetricAndTarget.push(
                {
                    KRAAspectId: this.kraMericAndTarget.KRAAspectId,
                    KRADefinitionId: this.kraMericAndTarget.KRADefinitionId,
                    Metric: this.kraMericAndTarget.Metric.trim(),
                    KRAOperatorId: this.kraMericAndTarget.KRAOperatorId,
                    KRAMeasurementTypeId: this.kraMericAndTarget.KRAMeasurementTypeId,
                    KRAScaleMasterId: this.kraMericAndTarget.KRAScaleMasterId,
                    KRAScaleDescription: this.kraMericAndTarget.KRAScaleDescription,
                    KRATargetValue: this.kraMericAndTarget.KRATargetValue,
                    KRATargetText: this.kraMericAndTarget.KRATargetText,
                    KRATargetPeriodId: this.kraMericAndTarget.KRATargetPeriodId,
                    Operator: this.kraMericAndTarget.Operator,
                    // Operator: '',
                    KRAMeasurementType: '',
                    ScaleLevel: '',
                    KRATargetPeriod: '',
                    TargetDescription: '',
                    KRATargetValueAsDate: this.kraMericAndTarget.KRATargetValueAsDate
                });
            this._kraService.CreateKRADefinition(this.kraDefinitionData).subscribe((data: any) => {
                if (data > 0)
                this.messageService.add({ severity: 'success',
                  summary: 'Success Message', detail: 'KRA definition added successfully.' }); 
                else
                this.messageService.add({ severity: 'error',
                  summary: 'Error Message', detail: 'You are not authorized to add KRA.' }); 
                this.cancel();
                this.getKraDefinitions();

            },
            (error: any) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error Message',
                detail: 'Failed to add KRA definition.'
              });
            });
        }
    }
  }
  ShowScaleDescription(KRAScaleMasterId: number) {
    if (KRAScaleMasterId != null) {
        this.kraFormSubmitted = false;
        if (KRAScaleMasterId == undefined) {
          this.messageService.add({ severity: 'warn',
                  summary: 'Please select user', detail: '' });
            return;
        }
        this.showScaleDescriptionDialog = true;
    }
  }
//   getMetricAndTargetByID(event: any) {
//     let aspectId: any;
//     let targetDescription: string = '';
//     if (event.KRAAspectId != undefined)
//         aspectId = event.KRAAspectId;
//     else aspectId = event;

//     if (aspectId) {
//         for (var i = 0; i < event.lstMetricAndTarget.length; i++) {
//             if (event.lstMetricAndTarget[i].KRAMeasurementTypeId == KRAMeasurementType.Scale) {
//                 event.lstMetricAndTarget[i].TargetDescription = event.lstMetricAndTarget[i].Operator + event.lstMetricAndTarget[i].KRATargetValue;
//             }
//             else if (event.lstMetricAndTarget[i].KRAMeasurementTypeId == KRAMeasurementType.Percentage) {
//                 event.lstMetricAndTarget[i].TargetDescription = event.lstMetricAndTarget[i].Operator + event.lstMetricAndTarget[i].KRATargetValue + "%";
//             }
//             else if (event.lstMetricAndTarget[i].KRAMeasurementTypeId == KRAMeasurementType.Date) {
//                 event.lstMetricAndTarget[i].TargetDescription = event.lstMetricAndTarget[i].Operator + this.formateDate(event.lstMetricAndTarget[i].KRATargetValueAsDate);
//             }
//             else {
//                 event.lstMetricAndTarget[i].TargetDescription = event.lstMetricAndTarget[i].Operator + event.lstMetricAndTarget[i].KRATargetValue;
//             }
//             event.lstMetricAndTarget[i] = event.lstMetricAndTarget[i];
//         }
//     }
//   }
  
  formateDate(givenDate?: string): string {
    let formatedDate: string = "";
    if (givenDate != '' && typeof (givenDate) != 'undefined' && givenDate != null) {
        formatedDate = givenDate.split("T")[0];
        formatedDate = moment(givenDate).format('YYYY-MM-DD');
    }
    return formatedDate;
  }
  CheckUserRole(statusId: number) {
    if (this.type != 'view') {
        if (this.loggedinUserRole.indexOf("HRM") != -1 && (statusId == KRAStatusCodes.Draft || statusId == KRAStatusCodes.SendBackForHRMReview)) {
            this.hideContextMenu = false;
            this.loggedinUserRole = "HRM";
        }
        else if (this.loggedinUserRole.indexOf("Department Head") != -1 && (statusId == KRAStatusCodes.SubmittedForDepartmentHeadReview || statusId == KRAStatusCodes.SendBackForDepartmentHeadReview)) {
            this.hideContextMenu = false;
            this.loggedinUserRole = "Department Head";

        }
        else if (this.loggedinUserRole.indexOf("HR Head") != -1 && statusId == KRAStatusCodes.SubmittedForHRHeadReview) {
            this.hideContextMenu = false;
            this.loggedinUserRole = "HR Head";

        }
    }
  }
  private addKraMetric(kraSet: KRADefinition): void {
    this.kraDefinitionData = new KRADefinition();
    this.kraDefinitionData.KRAAspectId = kraSet.KRAAspectId;
    this.buttonType = 'add'
    this.disableAspect = true;
    this.kraFormSubmitted = false;
    this.kraDefinitionForm.controls['metric'].reset();
  }
  OnBeforeTogle(event: any) {
    this.IsCollapsed = true;
  }
  private editKraMetric(kraSet: KRADefinition, lstmetric: MetricAndTarget, i: number): void {
    this.IsCollapsed = false;
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    this.kraDefinitionData = new KRADefinition();
    this.kraMericAndTarget.KRAAspectId = kraSet.KRAAspectId;
    this.kraMericAndTarget.Metric = kraSet.lstMetricAndTarget[i].Metric;
    this.kraMericAndTarget.KRADefinitionId = kraSet.lstMetricAndTarget[i].KRADefinitionId;
    this.kraMericAndTarget.KRAOperatorId = kraSet.lstMetricAndTarget[i].KRAOperatorId;
    this.kraMericAndTarget.KRAMeasurementTypeId = kraSet.lstMetricAndTarget[i].KRAMeasurementTypeId;
    this.kraMericAndTarget.KRATargetValue = kraSet.lstMetricAndTarget[i].KRATargetValue;
    this.kraMericAndTarget.KRATargetText = kraSet.lstMetricAndTarget[i].KRATargetText;
    this.kraMericAndTarget.KRATargetPeriodId = kraSet.lstMetricAndTarget[i].KRATargetPeriodId;
    if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Scale) {
        this.isscaleType = true;
        this.IsDateMeasurementType = false;
    }
    else if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Date) {
        if (kraSet.lstMetricAndTarget[i].KRATargetValueAsDate != undefined && kraSet.lstMetricAndTarget[i].KRATargetValueAsDate != "" && kraSet.lstMetricAndTarget[i].KRATargetValueAsDate != null)
            this.kraMericAndTarget.KRATargetValueAsDate = moment(kraSet.lstMetricAndTarget[i].KRATargetValueAsDate).format('YYYY-MM-DD');
        this.kraMericAndTarget.KRAScaleMasterId = null;
        this.kraMericAndTarget.KRAScaleDescription = '';
        this.isscaleType = false;
        this.IsDateMeasurementType = true;
        this.isValidTargetvalue = false;
    }
    else {
        this.kraMericAndTarget.KRAScaleMasterId = null;
        this.kraMericAndTarget.KRAScaleDescription = '';
        this.isscaleType = false;
        this.isValidTargetvalue = false;
        this.IsDateMeasurementType = false;
    }
    this.kraMericAndTarget.KRAScaleMasterId = kraSet.lstMetricAndTarget[i].KRAScaleMasterId;
    if ((this.scaleDescriptionList.filter(i => i.Id == this.kraMericAndTarget.KRAScaleMasterId)).length > 0)
        this.kraMericAndTarget.KRAScaleDescription = this.scaleDescriptionList.filter(i => i.Id == this.kraMericAndTarget.KRAScaleMasterId)[0].Name;
    else
        this.kraMericAndTarget.KRAScaleDescription = '';
    this.kraDefinitionData.StatusId = kraSet.StatusId;
    this.kraDefinitionData.KRAGroupId = this.kraGroupId;
    this.kraDefinitionData.FinancialYearId = this.financialYearId;
    this.CheckUserRole(this.kraDefinitionData.StatusId);
    this.kraDefinitionData.RoleName = this.loggedinUserRole;
    this.disableAspect = true;
    this.buttonType = 'edit'
    this.kraFormSubmitted = false;
  } 
  private deleteKraMetric(selecredkradefinition: KRADefinition, selectedmetric: MetricAndTarget, i: number): void {
    this.deleteMetric = true;
    this._confirmationService.confirm({
        message: 'Are you sure, you want to delete this metric?',
        header: 'Metric Delete Confirmation',
        key: 'kraMetric',
        icon: 'fa fa-trash',
        accept: () => {
            this.CheckUserRole(selecredkradefinition.StatusId);
            selecredkradefinition.RoleName = this.loggedinUserRole;
            selecredkradefinition.lstMetricAndTarget = [];
            selecredkradefinition.lstMetricAndTarget.push(
                {
                    KRAAspectId: selectedmetric.KRAAspectId,
                    KRADefinitionId: selectedmetric.KRADefinitionId,
                    Metric: selectedmetric.Metric.trim(),
                    KRAOperatorId: selectedmetric.KRAOperatorId,
                    KRAMeasurementTypeId: selectedmetric.KRAMeasurementTypeId,
                    KRAScaleMasterId: selectedmetric.KRAScaleMasterId,
                    KRAScaleDescription: selectedmetric.KRAScaleDescription,
                    KRATargetValue: selectedmetric.KRATargetValue,
                    KRATargetText: selectedmetric.KRATargetText,
                    KRATargetPeriodId: selectedmetric.KRATargetPeriodId,
                    Operator: '',
                    KRAMeasurementType: '',
                    ScaleLevel: '',
                    KRATargetPeriod: '',
                    TargetDescription: ''
                });
            this.deleteKraMetricAndTarget(selecredkradefinition);
        },
        reject: () => {
        }
    });
}
deleteKraMetricAndTarget(selecredkradefinition: KRADefinition) {
    this._kraService.deleteMetricAndTarget(selecredkradefinition).subscribe((result: any) => {
        if (result > 0)
        this.messageService.add({ severity: 'success',
           summary: 'KRA Metric has been deleted.', detail: '' }); 
        else
        this.messageService.add({ severity: 'error',
           summary: 'You are not authorized to delete KRA', detail: '' }); 
        this.getKraDefinitions();
    },
        (error) => {
          this.messageService.add({ severity: 'error',
           summary: 'Error Message', detail: 'Failed to delete KRA Metric.' }); 
        });
    this.deleteMetric = false;
  }
  private deleteKraAspect(kraDef: KRADefinition): void {
    this._confirmationService.confirm({
        message: 'Are you sure, you want to delete aspect - ' + kraDef.KRAAspectName,
        header: 'KRA Aspect Delete Confirmation',
        key: 'kraAspect',
        icon: 'fa fa-trash',
        accept: () => {
            kraDef.RoleName = this.loggedinUserRole;
            kraDef.KRAGroupId = this.kraGroupId;
            this.deleteKraAspectData(kraDef);
        },
        reject: () => {
        }
    });
}
  deleteKraAspectData(kraDef: KRADefinition) {
    if (kraDef != null) {
        this._kraService.deleteKraAspectDetails(kraDef).subscribe((result: any) => {
          this.messageService.add({ severity: 'success',
           summary: 'KRA Aspect has been deleted', detail: '' }); 
            this.getKraDefinitions();
        },
            (error) => {
            this.messageService.add({ severity: 'error',
             summary: 'Error Message', detail: 'Failed to delete KRA Aspect.' }); 
            });       
    }
  }
  private update(): void {
    this.kraFormSubmitted = true;
    this.updateKraDefinition();
  }
  private updateKraDefinition(): void {
    if (this.kraDefinitionForm.valid && !this.isValidTargetvalue) {
        if (this.kraMericAndTarget.KRAMeasurementTypeId != KRAMeasurementType.Date && this.kraMericAndTarget.KRATargetValue == null) return;
        if (this.kraMericAndTarget.KRAMeasurementTypeId == KRAMeasurementType.Date && this.kraMericAndTarget.KRATargetValueAsDate == null) return;
        if (this.kraMericAndTarget.Metric.trim().length <= 0) {
          this.messageService.add({ severity: 'warn',
             summary: 'Please provide valid Metric and/or Target details', detail: '' }); 
            return;
        }
        else {
            if (this.kraMericAndTarget.KRATargetText != undefined && this.kraMericAndTarget.KRATargetText != null)
                this.kraMericAndTarget.KRATargetText = this.kraMericAndTarget.KRATargetText.trim();
            this.kraDefinitionData.RoleName = this.loggedinUserRole;
            this.kraDefinitionData.lstMetricAndTarget = [];
            this.kraDefinitionData.lstMetricAndTarget.push(
                {
                    KRAAspectId: this.kraMericAndTarget.KRAAspectId,
                    KRADefinitionId: this.kraMericAndTarget.KRADefinitionId,
                    Metric: this.kraMericAndTarget.Metric.trim(),
                    KRAOperatorId: this.kraMericAndTarget.KRAOperatorId,
                    KRAMeasurementTypeId: this.kraMericAndTarget.KRAMeasurementTypeId,
                    KRAScaleMasterId: this.kraMericAndTarget.KRAScaleMasterId,
                    KRAScaleDescription: this.kraMericAndTarget.KRAScaleDescription,
                    KRATargetValue: this.kraMericAndTarget.KRATargetValue,
                    KRATargetText: this.kraMericAndTarget.KRATargetText,
                    KRATargetPeriodId: this.kraMericAndTarget.KRATargetPeriodId,
                    Operator: '',
                    KRAMeasurementType: '',
                    ScaleLevel: '',
                    KRATargetPeriod: '',
                    TargetDescription: '',
                    KRATargetValueAsDate: this.kraMericAndTarget.KRATargetValueAsDate
                });
            this._kraService.updateKraDefinition(this.kraDefinitionData).subscribe((data: any) => {
                if (data > 0)
                this.messageService.add({ severity: 'success',
                  summary: 'Success Message', detail: 'KRA data updated successfully.' }); 
                else
                this.messageService.add({ severity: 'error',
                   summary: 'Error Message', detail: 'You are not authorized to update KRA.' });
                this.cancel();
                this.buttonType = 'add';
                this.getKraDefinitions();
            }, (error) => {
              this.messageService.add({ severity: 'error',
                    summary: 'Error Message', detail: 'Failed to update KRA data.' }); 
            });
        }
    }
  }
  cancelMetricDelete() {
    this.deleteMetric = false;
  }
  onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
  }
  private cancel(): void {
    this.disableAspect = false;
    this.kraDefinitionForm.reset();
    this.kraFormSubmitted = false;
    this.buttonType = "add";
    this.kraMericAndTarget.KRAScaleMasterId = null;
    this.kraMericAndTarget.KRAScaleDescription = '';
    this.isscaleType = false;
    this.IsDateMeasurementType=false;
    this.isValidTargetvalue = false;
    this.kraDefinitionForm.controls['targetValue'].reset();
    this.kraDefinitionForm.controls['KRATargetValueAsDate'].reset();
    this.getKraDefinitions();
}
}