
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DomSanitizer } from "@angular/platform-browser";
import { SelectItem, MessageService } from "primeng/components/common/api";
import { Message } from "primeng/components/common/api";
import * as moment from "moment";
import { AssociateAppreciationService } from "../../Services/adr.associate-appreciation.service";
import { KRAService } from "src/app/components/kra/Services/kra.service";
import { MasterDataService } from "src/app/services/masterdata.service";
import { Appreciation, ADRCycle } from "../../Models/associate-developement.review.model";
import { GenericType } from "src/app/models/dropdowntype.model";
import * as servicePath from "../../../../service-paths";

declare var _: any;
declare var $: any;

@Component({
  selector: 'app-adr-appreciate-associate',
  templateUrl: './adr-appreciate-associate.component.html',
  styleUrls: ['./adr-appreciate-associate.component.scss'],
  providers: [AssociateAppreciationService, KRAService, MasterDataService, MessageService]
})

export class AdrAppreciateAssociateComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  public appreciationsList: Appreciation[];
  public sendAppreciation: Appreciation;
  private componentName: string;
  public appreciateDisplay: boolean = false;
  public displayMoreInfoDialog: boolean = false;
  private filteredAssociateIds: GenericType[];
  public adrCycleList: SelectItem[];
  public appreciationTypeList: SelectItem[];
  public sourceOfOriginList: SelectItem[];
  private appreciationTypeNames: GenericType[] = [];
  private _employeeId: number;
  public moreAppreciationInfo: string;
  public sentUpdateBtn: boolean = false;
  private currentDate: Date = new Date();
  public PageSize: number;
  public PageDropDown: number[] = [];

  cols = [
    { field: 'ToEmployeeName', header: 'To'},
    { field: 'SourceOfOrigin', header: 'Source Of Origin'},
    { field: 'ADRCycle', header: 'ADR Cycle'},
    { field: 'FinancialYear', header: 'Financial Year'},
    { field: 'AppreciationType', header: 'Appreciation Type'},
    { field: 'AppreciationDate', header: 'Date'},
    { field: 'AppreciationMessage', header: 'Appreciation Message'},
  ];

  constructor(
    private actRoute: ActivatedRoute,
    private _AppreciationService: AssociateAppreciationService,
    private kraService: KRAService,
    private masterDataService: MasterDataService,
    private sanitizer: DomSanitizer,
    private messageService: MessageService,
  ) {
    this.sendAppreciation = new Appreciation();
    this._employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.componentName = this.actRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.getAppreciationsList();
    this.getCurrentFinancialYear();
    this.getSourceOfOriginList();
  }

  private getAppreciationsList(): void {
    this._AppreciationService
      .GetSentAppreciationsList(this._employeeId)
      .subscribe(
        (appreciationsResponse: Appreciation[]) => {
          
          this.appreciationsList = appreciationsResponse;
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', detail: 'Failed to get Appreciations List.', summary: 'Error Message' });
        }
      );
  }

  public transformSanitizer(style: string) {
    let appreciateString = this.sanitizer.bypassSecurityTrustHtml(style);
    return appreciateString;
  }

  getMoreAppreciationInfo(Appreciation: string): void {
    this.moreAppreciationInfo = Appreciation;
    this.displayMoreInfoDialog = true;
  }

  appreciateAnAssociate(): void {
    this.sentUpdateBtn = false;
    this.appreciateDisplay = true;
    this.clearAppreciation();
    this.getADRCycleList();
    this.getAppreciationTypeList();
    this.getSourceOfOriginList();
    this.sendAppreciation.AppreciationDate = moment(this.currentDate).format(
      "MM-DD-YYYY"
    );
  }

  public getCurrentFinancialYear(): void {
    this.kraService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.sendAppreciation.FinancialYearID = yearsdata.Id;
          this.sendAppreciation.FinancialYear = yearsdata.Name;
        }
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', detail: 'Failed to get  current financial years.', summary: 'Error Message' });
      }
    );
  }

  private clearAppreciation(): void {
    this.sendAppreciation.AssociateNames = [];
    this.sendAppreciation.AppreciationTypeID = null;
    this.sendAppreciation.AppreciationMessage = "";
  }

  private getADRCycleList(): void {
    this._AppreciationService.GetADRCycleList().subscribe(
      (adrCycleResponse: ADRCycle[]) => {
        let adrCycle = adrCycleResponse.filter(x=>x.IsActive == true);
        this.sendAppreciation.ADRCycleID = adrCycle[0].ADRCycleID;
        this.adrCycleList = [];
        this.adrCycleList.push({ label: "Select ADR Cycle", value: null });
        adrCycleResponse.forEach((adrCycle: ADRCycle) => {
          this.adrCycleList.push({
            label: adrCycle.ADRCycle,
            value: adrCycle.ADRCycleID
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', detail: 'Failed to get ADR Cycle List.', summary: 'Error Message' });
      }
    );
  }

  private getAppreciationTypeList(): void {
    this._AppreciationService.GetAppreciationTypeList().subscribe(
      (appreciationTypeResponse: GenericType[]) => {
        this.appreciationTypeList = [];
        this.appreciationTypeNames = appreciationTypeResponse;
        this.appreciationTypeList.push({
          label: "Select Appreciation Type",
          value: null
        });
        appreciationTypeResponse.forEach((appreciationType: GenericType) => {          
          this.appreciationTypeList.push({
            label: appreciationType.Name,
            value: appreciationType.Id
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', detail: 'Failed to get Appreciation Type List.', summary: 'Error Message' });
      }
    );
  }

  private getSourceOfOriginList(): void {
    this._AppreciationService.getSourceOfOriginList().subscribe(
      (sourceOfOriginResponse : Appreciation[]) => {
        this.sourceOfOriginList = [];       
        this.sourceOfOriginList.push({
          label: "Select source of origin",
          value: null
        });
        sourceOfOriginResponse.forEach((sourceOfOrigin : Appreciation) => {
          ;
          this.sourceOfOriginList.push({
            label: sourceOfOrigin.SourceOfOriginName,
            value: sourceOfOrigin.SourceOfOriginId
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', detail: 'Failed to get Source Of Origin List.', summary: 'Error Message' });
      }
    );
  }

  private filteredAssociatesMultiple(event: any): void {
    let suggestionString = event.query;
    this.masterDataService.GetAllAssociateList().subscribe(
      (associateListResponse: GenericType[]) => {
        this.filteredAssociateIds = this.filteredAssociateId(
          suggestionString,
          associateListResponse
        );
      },
      (error: any) => {
        if (error._body != undefined && error._body != "")
        this.messageService.add({ severity: 'error', detail: 'Failed to get Associates List.', summary: 'Error Message' });
      }
    );
  }

  private filteredAssociateId(
    suggestionString: string,
    associateListResponse: GenericType[]
  ): GenericType[] {
    let filtered: GenericType[] = [];
    for (let i = 0; i < associateListResponse.length; i++) {
      let associateResponse = associateListResponse[i];
      if (
        associateResponse.Name
          .toLowerCase()
          .includes(suggestionString.toLowerCase()) == true
      ) {
        filtered.push(associateResponse);
      }
    }
    return filtered;
  }

  sendAnAppreciation(sendAppreciation: Appreciation): void {
    this.getAppreciationTypeList();
    this.getSourceOfOriginList();
    sendAppreciation.FromEmployeeID = this._employeeId;
    if (this.sentUpdateBtn == false) {
      if (sendAppreciation.AppreciationTypeID == null) {
        this.messageService.add({ severity: 'warn', detail: 'Please enter Appreciation Type.', summary: 'Warning Message' });
        return;
      }
      if (sendAppreciation.AssociateNames.length == 0) {
        this.messageService.add({ severity: 'warn', detail: 'Please enter Recepients names.', summary: 'Warning Message' });
        return;
      }
      if (sendAppreciation.SourceOfOriginId == null) {
        this.messageService.add({ severity: 'warn', detail: 'Please enter Source of origin.', summary: 'Warning Message' });
        return;
      }
      if (!sendAppreciation.AppreciationMessage) {
        this.messageService.add({ severity: 'warn', detail: 'Please enter Appreciation', summary: 'Warning Message' });
        return;
      }
      this._AppreciationService.SendAnAppreciation(sendAppreciation).subscribe((response: boolean) => {
            this.appreciateDisplay = false;
            this.messageService.add({ severity: 'success', detail: 'Appreciation sent Successfully.', summary: 'Success Message' });
            this.getAppreciationsList();
            this.getSourceOfOriginList();
          },
          (error: any) => {
            this.messageService.add({ severity: 'error', detail: 'Failed to send Appreciation.', summary: 'Error Message' });
          }
        );
    } else {
      this._AppreciationService.UpdateAnAppreciation(sendAppreciation).subscribe((response: boolean) => {
            this.appreciateDisplay = false;
            this.messageService.add({ severity: 'success', detail: 'Updated Appreciation sent Successfully', summary: 'Success Message' });
            this.getAppreciationsList();
            this.getSourceOfOriginList();
          },
          (error: any) => {
            this.messageService.add({ severity: 'error', detail: 'Failed to send updated Appreciation.', summary: 'Error Message' });
          }
        );
    }
  }

  onEditAppreciation(sendAnAppreciation: Appreciation): void {
    this.sentUpdateBtn = true;
    this.getADRCycleList();
    this.getAppreciationTypeList();
    this.sendAppreciation = sendAnAppreciation;
    this.sendAppreciation.AppreciationTypeID = this.getAppreciationTypeId(
      sendAnAppreciation.AppreciationType
    );
    this.sendAppreciation.SourceOfOriginId = this.getSourceOfOriginId(
      sendAnAppreciation.SourceOfOriginName 
    );
    this.appreciateDisplay = true;
  }

  private getAppreciationTypeId(AppreciationTypeName: string): number {
    if (this.appreciationTypeList.length > 0) {
      let AppreciationType: SelectItem[] = _.filter(
        this.appreciationTypeList,
        function(Appreciation: SelectItem) {
          return Appreciation.label == AppreciationTypeName;
        }
      );
      return AppreciationType[0].value;
    }
  }

  getSourceOfOriginId(sourceOfOrigin: string): number {
    if (this.sourceOfOriginList.length > 0) {
      let SourceOfOrigin: SelectItem[] = _.filter(
        this.sourceOfOriginList,
        function(Appreciation: SelectItem) {
          return Appreciation.label == sourceOfOrigin;
        }
      );
      return SourceOfOrigin[0].value;
    }
  }

  onDeleteAppreciation(sendAnAppreciation: Appreciation): void {
      this._AppreciationService.DeleteAnAppreciation(sendAnAppreciation.ID).subscribe((response: boolean) => {
          this.messageService.add({ severity: 'success', detail: 'Appreciation is deleted Successfully.', summary: 'Success Message' });
      },
        (error: any) => {
          this.messageService.add({ severity: 'error', detail: 'Failed to delete Appreciation.', summary: 'Error Message' });
        });
  }
}
