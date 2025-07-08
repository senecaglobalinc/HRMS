
import { Component, OnInit } from "@angular/core";
import { MasterDataService } from "../../../services/masterdata.service";
import {  SelectItem } from "primeng/components/common/api";
import * as servicePath from "../../../service-paths";
import { GenericType } from "../../../models/dropdowntype.model";
import { MessageService } from "primeng/api";
import { KRAService } from "../../kra/Services/kra.service";
import { AdrKraService } from "../Services/adr-kra.service";
import { ADRCycle } from "../Models/adrcycle.model";
import { ADRData } from '../Models/adr-kra.model';
import { DomSanitizer } from "@angular/platform-browser";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
  selector: 'app-adr-kras',
  templateUrl: './adr-kras.component.html',
  styleUrls: ['./adr-kras.component.scss'],
  providers: [
    AdrKraService,
    MasterDataService,
    KRAService,
  ]
})

export class AdrKrasComponent implements OnInit {
  associateKraForm: FormGroup;
  resources = servicePath.API.PagingConfigValue;
  loggedinUserRole: string;
  loggedInEmployeeId: number;
  public overideExisting = 0;
  adrData: ADRData[] = [];
  public currentfinancialYearId: number = 0;
  public currentAdrCycleId: number = 0;
  public financialYearId: number = 0;
  public financialYearsList: SelectItem[] = [];
  _selectedFinancialYearId: number;
  public associateKraData = [];
  public rowspansList = [];
  public adrCyclesList = [];
  public associatesContributionsDisplay: boolean = false;
  public managerFeedbackDisplay: boolean = false;
  public managerFeedback: string = "";
  public associateRating: string = "";
  public KRAMeasurementType: string = ""; 
  public KRAAspectTarget: string = ""; 
  public managerRating: string = "";
  public kraStatus: string;

  constructor(
    private adrKraService: AdrKraService,
    private _kraService: KRAService,
    private masterDataService: MasterDataService,
    private messageService: MessageService,
    private sanitizer: DomSanitizer,
  ) {
  }

  ngOnInit() {
    this.loggedinUserRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.loggedinUserRole = 'Associate';
    this.loggedInEmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.associateKraForm = new FormGroup({
      AspectId : new FormControl(''),
      AspectName : new FormControl(''),
      KRADefinitionId : new FormControl(''),
      Metric : new FormControl(''),
      FinancialYearId : new FormControl(''),
      Operator : new FormControl(''),
      KRAMeasurementType : new FormControl(''),
      KRATargetPeriod : new FormControl(''),
      TargetValue: new FormControl(''),
      AssociateKRAMapperId : new FormControl(''),
      AssociateADRMasterId : new FormControl(''),
      ADRCycleID : new FormControl(''),
      StatusId : new FormControl(''),
      StatusCode : new FormControl(''),
      AssociateADRDetailId : new FormControl(''),
      Contribution : new FormControl(''),
      SelfRating : new FormControl(''),
      ManagerComments : new FormControl(''),
      Rating  : new FormControl(''),
    });

    this.getFinancialYears();
    this.getLastThreeAdrCylces();
    this.getCurrentFinancialYear();
  }

  tabs = [
    { tab: "Cycle1", header: "Cycle 1" },
    { tab: "Cycle1", header: "Cycle 2" },
    { tab: "Cycle1", header: "Cycle 3" },
  ]

  private getFinancialYears(): void {
    this.masterDataService.GetFinancialYears().subscribe(
      (yearsdata: GenericType[]) => {
        this.financialYearsList = [];
        this.financialYearsList.push({
          label: "Select Financial Year",
          value: null
        });
        yearsdata.forEach((e: GenericType) => {
          this.financialYearsList.push({ label: e.Name, value: e.Id });
        });
      },
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Finacial Year List"
        });
      }
    );
  }

  public getCurrentFinancialYear(): void {
    this._kraService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.currentfinancialYearId = yearsdata.Id;
          this.currentAdrCycleId = 3;
          this.GetAssociateADRDetail(this.loggedInEmployeeId, this.currentfinancialYearId, this.currentAdrCycleId);
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          detail: "Failed to get current financial year!",
          summary: "Error Message"
        });
      }
    );
  }

  private getLastThreeAdrCylces(): void {
    this.adrCyclesList = 
      [{FinancialYearId: 11, ADRCycleId: 1 }, {FinancialYearId: 10, ADRCycleId: 2 }, {FinancialYearId: 10, ADRCycleId: 3 }]
  }
 
  public GetAssociateADRDetail(EmployeeId: number, FinancialYearId: number, ADRCycleId: number): void {
    this.associateKraData = [];
    this.rowspansList = [];
    this.kraStatus = 'Approved';
    // this.kraStatus = 'Submitted';
    this.kraStatus = 'Drafted';
    this.adrData = [];
    if (FinancialYearId == null || ADRCycleId == null) {
      this.adrData = [];
      return;
    }

    this.adrKraService.GetAssociateADRDetail(EmployeeId, FinancialYearId, ADRCycleId).subscribe((res: ADRData[]) => {
      this.adrData = [];
      this.adrData = res;

      // Logic to calculate the RowSpan and Number of records for each KRA Aspect
      this.rowspansList = [0];
      for (let i = 0; i < this.adrData.length; i++ ) 
      {
        var KRAAspect = this.adrData[i].AspectName;
        if (i > 0 && this.adrData[i].AspectName != this.adrData[i - 1].AspectName) 
        {
          this.rowspansList.push(i);
        }
        if (this.adrData[i].KRAMeasurementType == "Percentage") {
          var KRAAspectTarget = this.adrData[i].Operator + " " + this.adrData[i].TargetValue + "% (" + this.adrData[i].KRATargetPeriod + ")";
        } 
        else {
          var KRAAspectTarget = this.adrData[i].Operator + " " + this.adrData[i].TargetValue + " (" + this.adrData[i].KRATargetPeriod + ")";
        }
        this.associateKraData.push({
          AspectCount: this.adrData.filter(obj => obj.AspectName === KRAAspect).length,
          AspectId: this.adrData[i].AspectId,
          AspectName: this.adrData[i].AspectName,
          KRADefinitionId: this.adrData[i].KRADefinitionId,
          Metric: this.adrData[i].Metric,
          KRAAspectTarget: KRAAspectTarget,
          KRAMeasurementType: this.adrData[i].KRAMeasurementType,
          Operator: this.adrData[i].Operator,
          TargetValue: this.adrData[i].TargetValue,
          KRATargetPeriod: this.adrData[i].KRATargetPeriod,
          AssociateADRMasterId: this.adrData[i].AssociateADRMasterId,
          AssociateADRDetailId: this.adrData[i].AssociateADRDetailId,
          AssociateKRAMapperId: this.adrData[i].AssociateKRAMapperId,
          Contribution: this.adrData[i].Contribution,
          SelfRating: this.adrData[i].SelfRating,
          ManagerComments: this.adrData[i].ManagerComments,
          Rating: this.adrData[i].Rating,
          StatusId: this.adrData[i].StatusId,
          StatusCode: this.adrData[i].StatusCode
        });
      }
    },
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Associate KRA Data"
        });
      }
    );
  }

  setSelectedTab(event) {
    // this.adrKraService.SetSeletedTab(event.index);
    if (event.index == 0) {
      this.GetAssociateADRDetail(this.loggedInEmployeeId, 11, 3);
    }
    else if (event.index == 1) {
      this.GetAssociateADRDetail(this.loggedInEmployeeId, 10, 5);
    }
    else if (event.index == 2) {
      this.GetAssociateADRDetail(this.loggedInEmployeeId, 10, 6);
    }
  } 

  public displayAssociateContributions(selectedData: ADRData): void {
    this.associatesContributionsDisplay = true;
    this.associateKraFormPatch(selectedData);
  }

  public displayManagerFeedback(selectedData: ADRData): void {
    this.managerFeedbackDisplay = true;
    this.associateKraFormPatch(selectedData);
  }

  public associateKraFormPatch(selectedData: ADRData): void {
    this.associateKraForm.patchValue({
      AspectId : selectedData.AspectId,
      AspectName : selectedData.AspectName,
      KRADefinitionId : selectedData.KRADefinitionId,
      Metric : selectedData.Metric,
      FinancialYearId : selectedData.FinancialYearId,
      Operator : selectedData.Operator,
      KRAMeasurementType : selectedData.KRAMeasurementType,
      KRATargetPeriod : selectedData.KRATargetPeriod,
      TargetValue: selectedData.TargetValue,
      AssociateKRAMapperId : selectedData.AssociateKRAMapperId,
      AssociateADRMasterId : selectedData.AssociateADRMasterId,
      ADRCycleID : selectedData.ADRCycleID,
      StatusId : selectedData.StatusId,
      StatusCode : selectedData.StatusCode,
      AssociateADRDetailId : selectedData.AssociateADRDetailId,
      Contribution : selectedData.Contribution,
      SelfRating : selectedData.SelfRating,
      ManagerComments : selectedData.ManagerComments,
      Rating : selectedData.Rating,
    });
    if (selectedData.KRAMeasurementType == "Percentage") {
      this.KRAAspectTarget = selectedData.Operator + " " + selectedData.TargetValue + "% (" + selectedData.KRATargetPeriod + ")";
    } else {
      this.KRAAspectTarget = selectedData.Operator + " " + selectedData.TargetValue + " (" + selectedData.KRATargetPeriod + ")";
    }
  }

  public updateAssociateContributions(selectedData: ADRData) {
    selectedData.Contribution = this.associateKraForm.value.Contribution;
    selectedData.SelfRating = this.associateKraForm.value.SelfRating;
    this.associatesContributionsDisplay = false;
    this.messageService.add({ severity: 'success', detail: 'Data saved Successfully.', summary: 'Success Message' });
  }

  public CancelComments() {
    this.associatesContributionsDisplay = false;
    this.managerFeedbackDisplay = false;
    this.associateKraForm.reset();
    this.KRAMeasurementType = null;
  }

  private transformSanitizer(style: string) {
  let comments = this.sanitizer.bypassSecurityTrustHtml(style);
    return comments;
  }

}
