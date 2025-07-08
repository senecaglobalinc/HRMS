import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import * as servicePath from '../../../service-paths';
import { FinancialYear } from "../../../models/financialyear.model";
import { FinancialYearService } from "../Services/financialyear.service";
import { CommonService } from "../../../services/common.service";
import { Message, ConfirmationService} from "primeng/components/common/api";
import { MessageService } from 'primeng/api';
import { concat } from 'rxjs';
declare var _: any;

@Component({
  selector: 'app-financialyear',
  templateUrl: './financialyear.component.html',
  styleUrls: ['./financialyear.component.scss'],
  providers: [ ConfirmationService, MessageService]
})

export class FinancialyearComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  private errorMessage: Message[] = [];
  public financialYearList: FinancialYear[] = [];
  public financialYear: FinancialYear;
  private currentFinancialYear: number;
  public buttonName: string = "Add";
  public btnTitle: string = "Add";
  private componentName: string;
  public PageSize: number;
  public PageDropDown: number[] = [];
  public formSubmmited: boolean = false;
  public isActive: boolean = false;
  public currentActive: boolean = false;
  public AlreadyError: boolean = false;

  @ViewChild("fromFinancialYear") fromFinancialYear: any
  constructor(
    private _financialYearService: FinancialYearService,
    private _confirmationService: ConfirmationService,
    private _activatedRoute: ActivatedRoute,
    private messageService: MessageService
  ) {
     this.componentName = this._activatedRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.financialYear = new FinancialYear();
  }

  ngOnInit() {
    this.clear();
    this.getFinancialYears();
    this.getCurrentFinanacialYear();
  }

  cols = [
    { field: 'FromYear', header: 'Financial Year' },
    { field: 'ToYear', header: 'Financial ToYear' },
    { field: 'IsActive', header: 'Active' }
  ];

  private getCurrentFinanacialYear(): void {
    this._financialYearService
      .GetCurrentFinancialYear()
      .subscribe((currentYearResponse: number) => {
        this.currentFinancialYear = currentYearResponse;
      });
  }

  private getFinancialYears(): void {
    this._financialYearService.GetFinancialYearList().subscribe(
      (financialYearResponse: FinancialYear[]) => {
        this.financialYearList = financialYearResponse;
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get FinanacialYears List.' });
      }
    );
  }

  private onGroupRowSelect(selectedData: FinancialYear): void {
    this.clear();
    this.buttonName = "Update";
    this.btnTitle = "Update";
    this.isActive = selectedData.IsActive;
    this.currentActive = selectedData.IsActive;
    this.financialYear = selectedData;
  }

  private onFromChange(FromYear: number): boolean {
    this.AlreadyError = false;
    if (FromYear) {
      let numberRegex = /^\d{4}$/;
      let fromNumberValidation = numberRegex.test(FromYear.toString());
      if (fromNumberValidation) {
        if (FromYear > 2015 && FromYear < 9999) {
          this.financialYear.ToYear = FromYear + 1;
          return true;
        } else {
          this.financialYear.ToYear = null;
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Valid year starts from 2016 to 9998.' });
          this.fromFinancialYear.nativeElement.focus();
          this.AlreadyError = true;
          return false;
        }
      } else {
        this.financialYear.ToYear = null;
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter Valid Year.' });
        this.fromFinancialYear.nativeElement.focus();
        this.AlreadyError = true;
        return false;
      }
    } else {
      this.financialYear.ToYear = null;
      this.isActive = false;
      this.AlreadyError = true;
    }
  }

  public onIsActiveChange(fromYear: number, active: boolean): boolean {
    this.isActive = active;
    if (fromYear) {
      if (this.currentFinancialYear == fromYear && active) {
        return true;
      } else if (this.currentFinancialYear == fromYear && !active) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Current financial year should be Active.' });
        return false;
      } else if (this.currentFinancialYear != fromYear && !active) {
        return true;
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'You cannot make this Financial Year as Active.' });
        return false;
      }
    }
  }

  private isActiveExists(): boolean {
    if (this.financialYearList) {
      let currentYearList = this.financialYearList.filter(x=>x.IsActive);
      if (currentYearList.length > 0) {
        return true;
      } else {
        return false;
      }
    }
  }

  private checkForDuplicateYears(fromYear: number, active: boolean): boolean {
    if (this.financialYearList && fromYear) {
      let currentYearList = this.financialYearList.filter(x=>x.FromYear == fromYear && x.IsActive == active);
      if (
        this.buttonName == "Add" &&
        currentYearList &&
        currentYearList.length > 0
      ) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Finanacial Year already Exists.' });
        return false;
      } else if (
        this.buttonName == "Update" &&
        currentYearList &&
        currentYearList.length > 1
      ) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Finanacial Year already Exists.' });
        return false;
      } else if (
        this.buttonName == "Update" &&
        currentYearList &&
        currentYearList.length > 0
      ) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Nothing to Save.' });
        return false;
      } else {
        return true;
      }
    }
  }

  public saveFinancialYear(financialYear: FinancialYear): void {
    this.formSubmmited = true;
    if (financialYear && financialYear.FromYear) {
      if (this.AlreadyError == false) 
      var numberValidation: boolean = this.onFromChange(financialYear.FromYear);
      let checkCurrentFinancialYear: boolean = this.onIsActiveChange(
        financialYear.FromYear,
        this.isActive
      );
      let checkForDuplicateYear: boolean = this.checkForDuplicateYears(
        financialYear.FromYear,
        this.isActive
      ); 
      if (
        numberValidation &&
        checkCurrentFinancialYear &&
        checkForDuplicateYear
      ) {
        financialYear.IsActive = this.isActive;
        let isActiveExist: boolean = this.isActiveExists();
        if (this.isActive && isActiveExist) {
          this._confirmationService.confirm({
            message:
              "Are you sure, Do you want make this financial year as Active",
            header: "Financial Year",
            key: "financialYearConfirmation",
            icon: "fa fa-exclamation-circle",
            accept: () => {
              if (this.buttonName == "Add") {
                this.addFinancialYear(financialYear);
              } else {
                this.updateFinancialYear(financialYear);
              }
            },
            reject: () => {
              return;
            }
          });
        } else {
          if (this.buttonName == "Add") {
            this.addFinancialYear(financialYear);
          } else {
            this.updateFinancialYear(financialYear);
          }
        }
      }
    }
  }

  private addFinancialYear(financialYear: FinancialYear): void {
    this._financialYearService.CreateFinancialYear(financialYear).subscribe(
      (response: number) => {
        if (response == 1) {
          this.clear();
          this.getFinancialYears();
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Finanacial Year Saved Successfully.' });
        } else if (response == -1) {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Finanacial Year already Exists.' });
        } else if (response == -11) {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Nothing to Save.' });
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Save Finanacial Year.' }); 
        }
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Save Finanacial Year.' });
      }
    );
  }

  private updateFinancialYear(financialYear: FinancialYear): void {
    this._financialYearService.UpdateFinancialYear(financialYear).subscribe(
      (response: number) => {
        if (response == 1) {
          this.clear();
          this.getFinancialYears();
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Finanacial Year Updated Successfully.' });
        } else if (response == -1) {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Finanacial Year already Exists.' });
        } else if (response == -11) {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Nothing to Save.' });
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Update Finanacial Year.' });
        }
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Update Finanacial Year.' });
      }
    );
  }

  public clear(): void {
    this.financialYear = {
      Id: 0,
      FromYear: null,
      ToYear: null,
      IsActive: false
    };
    this.buttonName = "Add";
    this.btnTitle = "Add";
    this.formSubmmited = false;
    this.isActive = false;
  }

}
