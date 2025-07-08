import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { ADROrganisationValueData } from '../Models/adr-organisation-value-data.model';
import { AdrOrganisationValueService } from '../Services/adr-organisation-value.service';
import { SelectItem } from 'primeng/components/common/api';
import { MasterDataService } from '../../../services/masterdata.service';
import { GenericType } from '../../../models/dropdowntype.model';

@Component({
  selector: 'app-adr-organisation-value-master',
  templateUrl: './adr-organisation-value-master.component.html',
  styleUrls: ['./adr-organisation-value-master.component.scss'],
  providers: [MessageService, AdrOrganisationValueService]
})
export class AdrOrganisationValueMasterComponent implements OnInit {
  addOrgValue: FormGroup;
  btnLabel: string = "";
  formSubmitted = false;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  orgValuesList: ADROrganisationValueData[] = [];
  selectedRow: ADROrganisationValueData;
  orgValueId: number = 0;
  public financialYearsList: SelectItem[] = [];
  public financialYearId: number = 0;
  public currentYear: number = 0;
  public selectedYear: number = 0;
  public DisableEdit: boolean = true;
  @ViewChild("dt1") table: any;

  cols = [
    { field: 'ADROrganisationValue', header: 'Organisation Value' }
  ];

  constructor(private adrOrgValueMasterService: AdrOrganisationValueService,
    private masterDataService: MasterDataService,
    private messageService: MessageService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.addOrgValue = new FormGroup({
      ADROrganisationValueID: new FormControl(null),
      financialYearId: new FormControl(null),
      ADROrganisationValue: new FormControl(null, [
        Validators.required,
        Validators.maxLength(100)
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
    this.adrOrgValueMasterService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.financialYearId = yearsdata.Id;
          this.currentYear = +yearsdata.Name.substring(0, 4);
          this.getADROrganisationValues();
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

  public getADROrganisationValues(): void {
    this.resetform();
    if (this.financialYearId != null && this.financialYearId != 0) {
      this.adrOrgValueMasterService.GetADROrganisationValues(this.financialYearId).subscribe((res: ADROrganisationValueData[]) => {
        this.orgValuesList = [];
        this.orgValuesList = res;
      },
        error => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Organisation Values.' });
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
      this.orgValuesList = [];
  }

  private duplicateValueCheck(valueName: string): boolean {
    if (this.orgValuesList) {
      let duplicateValuesList = this.orgValuesList.filter(function (value: ADROrganisationValueData) {
        return (
          value.ADROrganisationValue.toLowerCase() == valueName.toLowerCase()
        );
      });
      if ((this.btnLabel == "Add" && duplicateValuesList.length > 0) || (this.btnLabel == "Update" && duplicateValuesList.length > 1)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Value already exists.' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  public toAddorUpdateOrgValue(): void {
    this.table.reset();
    this.formSubmitted = true;
    if (this.addOrgValue.valid == true) {
      let duplicateValue = this.duplicateValueCheck(this.addOrgValue.value.ADROrganisationValue);
      if (!duplicateValue) return;
      if (this.orgValueId <= 0) { //Add new Value
        this.addNewOrgValue();
      }
      else
        this.updateOrgValue();
    }
  }

  public editOrgValue(value: ADROrganisationValueData): void {
    this.btnLabel = "Update";
    this.orgValueId = value.ADROrganisationValueID;
    this.addOrgValue.patchValue(value);
    this.addOrgValue.controls['ADROrganisationValue'].setValue(this.addOrgValue.value.ADROrganisationValue);
  }

  private addNewOrgValue(): void {
    this.adrOrgValueMasterService.CreateADROrganisationValue(this.addOrgValue.value).subscribe(res => {
      if (res == 1) {
        this.adrOrgValueMasterService.GetADROrganisationValues(this.financialYearId);
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Organisation Value added' });
        this.resetform();
        this.getADROrganisationValues();
      }
      else if (res == -1)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Value already exist' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add Organisation Value' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  private updateOrgValue(): void {
    this.adrOrgValueMasterService.UpdateADROrganisationValue(this.addOrgValue.value, this.financialYearId).subscribe(res => {
      if (res == 1) {
        this.adrOrgValueMasterService.GetADROrganisationValues(this.financialYearId);
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Organisation Value updated' });
        this.resetform();
        this.getADROrganisationValues();
      }
      else if (res == 2627)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Organisation Value already exists' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to update Organisation Value' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  public resetform(): void {
    this.addOrgValue.controls["ADROrganisationValue"].reset();
    this.formSubmitted = false;
    this.btnLabel = "Add";
    this.DisableEdit = true;
    this.orgValueId = 0;
  }

  public clear(): void {
    this.resetform();
    this.addOrgValue.reset();
    this.orgValuesList = [];
    this.getCurrentFinancialYear();
    this.getADROrganisationValues();
  }

}
