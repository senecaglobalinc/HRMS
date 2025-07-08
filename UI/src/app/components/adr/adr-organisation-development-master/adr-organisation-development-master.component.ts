import { Component, OnInit, ViewChild } from '@angular/core';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { SelectItem } from 'primeng/components/common/api';
import { AdrOrganisationDevelopmentMasterService } from '../Services/adr-organisation-development-master.service';
import { ADROrganisationDevelopmentData } from '../Models/adr-organisation-development-data.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterDataService } from '../../../services/masterdata.service';
import { GenericType } from '../../../models/dropdowntype.model';
import { delay } from 'q';


@Component({
  selector: 'app-adr-organisation-development-master',
  templateUrl: './adr-organisation-development-master.component.html',
  styleUrls: ['./adr-organisation-development-master.component.scss'],
  providers: [MessageService, AdrOrganisationDevelopmentMasterService]
})
export class AdrOrganisationDevelopmentMasterComponent implements OnInit {
  addOrgActivity: FormGroup;
  btnLabel: string = "";
  formSubmitted = false;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  orgActivitiesList: ADROrganisationDevelopmentData[] = [];
  selectedRow: ADROrganisationDevelopmentData;
  orgActivityId: number = 0;
  public financialYearsList: SelectItem[] = [];
  public financialYearId: number = 0;
  public currentYear: number = 0;
  public selectedYear: number = 0;
  public DisableEdit: boolean = true;
  @ViewChild("dt1") table: any;


  cols = [
    { field: 'ADROrganisationDevelopmentActivity', header: 'Organisation Development Activity' }
  ];

  constructor(private adrOrgDevMasterService: AdrOrganisationDevelopmentMasterService,
    private masterDataService: MasterDataService,
    private messageService: MessageService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.addOrgActivity = new FormGroup({
      ADROrganisationDevelopmentID: new FormControl(null),
      financialYearId: new FormControl(null),
      ADROrganisationDevelopmentActivity: new FormControl(null, [
        Validators.required,
        Validators.maxLength(1000)
      ]),
    });
    this.getFinancialYears();
    this.clear();
  }

  private getFinancialYears(): void {
    this.masterDataService.GetFinancialYears().subscribe((yearsdata: GenericType[]) => {
      this.financialYearsList = [];
      this.financialYearsList.push({ label: 'Select Financial Year', value: null });
      yearsdata.forEach((e: GenericType) => {
        this.financialYearsList.push({ label: e.Name, value: e.Id });
      });
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Financial Year List' });
    }
    );
  }
  public getCurrentFinancialYear(): void {
    this.adrOrgDevMasterService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.financialYearId = yearsdata.Id;
          this.currentYear = +yearsdata.Name.substring(0, 4);
          this.getADROrganisationDevelopment();
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get  current financial years!',
          summary: 'Error Message'
        });
      }
    );
  }
  public getADROrganisationDevelopment(): void {
    this.resetform();
    if (this.financialYearId != null && this.financialYearId != 0) {
      this.adrOrgDevMasterService.GetADROrganisationDevelopment(this.financialYearId).subscribe((res: ADROrganisationDevelopmentData[]) => {
        this.orgActivitiesList = [];
        this.orgActivitiesList = res;
      },
        error => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Organisation Development Activities.' });
        }
      );
      if (this.financialYearsList.length != 0) {
        this.selectedYear = +this.financialYearsList.find(x => x.value == this.financialYearId).label.substring(0, 4);
        if (this.selectedYear < this.currentYear) {
          this.DisableEdit = false;
        }
      }
    }
    else
      this.orgActivitiesList = [];
  }

  private duplicateActivityCheck(activityName: string): boolean {
    if (this.orgActivitiesList) {
      let duplicateActivityList = this.orgActivitiesList.filter(function (activity: ADROrganisationDevelopmentData) {
        return (
          activity.ADROrganisationDevelopmentActivity.toLowerCase() == activityName.toLowerCase()
        );
      });
      if ((this.btnLabel == "Add" && duplicateActivityList.length > 0) || (this.btnLabel == "Update" && duplicateActivityList.length > 1)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Activity Already exists.' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  public toAddorUpdateOrgActivity(): void {
    this.table.reset();
    this.formSubmitted = true;
    if (this.addOrgActivity.valid == true) {
      let duplicateActivity = this.duplicateActivityCheck(this.addOrgActivity.value.ADROrganisationDevelopmentActivity);
      if (!duplicateActivity) return;
      if (this.orgActivityId <= 0) { //Add new Activity
        this.addNewOrgActivity();
      }
      else
        this.updateOrgActivity();
    }
  }

  public editOrgActivity(activity: ADROrganisationDevelopmentData): void {
    this.btnLabel = "Update";
    this.orgActivityId = activity.ADROrganisationDevelopmentID;
    this.addOrgActivity.patchValue(activity);
    this.addOrgActivity.controls['ADROrganisationDevelopmentActivity'].setValue(this.addOrgActivity.value.ADROrganisationDevelopmentActivity);
  }

  private addNewOrgActivity(): void {
    this.adrOrgDevMasterService.CreateADROrganisationDevelopment(this.addOrgActivity.value).subscribe(res => {
      if (res == 1) {
        this.adrOrgDevMasterService.GetADROrganisationDevelopment(this.financialYearId);
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Organisation Activity added' });
        this.resetform();
        this.getADROrganisationDevelopment();
      }
      else if (res == -1)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Activity already exist' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add Organisation Activity' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  private updateOrgActivity(): void {
    this.adrOrgDevMasterService.UpdateADROrganisationDevelopment(this.addOrgActivity.value, this.financialYearId).subscribe(res => {
      if (res == 1) {
        this.adrOrgDevMasterService.GetADROrganisationDevelopment(this.financialYearId);
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Organisation Activity updated' });
        this.resetform();
        this.getADROrganisationDevelopment();
      }
      else if (res == 2627)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Activity already exists' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to update Organisation Activity' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  public resetform(): void {
    this.addOrgActivity.controls["ADROrganisationDevelopmentActivity"].reset();
    this.formSubmitted = false;
    this.btnLabel = "Add";
    this.DisableEdit = true;
    this.orgActivityId = 0;
  }

  public clear(): void {
    this.resetform();
    this.addOrgActivity.reset();
    this.orgActivitiesList = [];
    this.getCurrentFinancialYear();
    this.getADROrganisationDevelopment();
  }

}
