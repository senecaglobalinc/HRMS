import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Message, MessageService } from 'primeng/components/common/api';
import * as moment from 'moment';
import { Appreciation } from '../../Models/associate-developement.review.model';
import { AssociateAppreciationService } from '../../Services/adr.associate-appreciation.service';
import * as servicePath from "../../../../service-paths";

declare var $: any;

@Component({
  selector: 'app-adr-appreciationsof-associate',
  templateUrl: './adr-appreciationsof-associate.component.html',
  styleUrls: ['./adr-appreciationsof-associate.component.scss'],
  providers: [AssociateAppreciationService, MessageService]
})

export class AdrAppreciationsofAssociateComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  private errorMessage: Message[] = [];
  public appreciationsList: Appreciation[];
  public displayMoreInfoDialog: boolean = false;
  private componentName: string;
  public moreAppreciationInfo: string;
  private _employeeId: number;
  public PageSize: number
  public PageDropDown: number[] = [];

  cols = [
    { field: 'FromEmployeeName', header: 'From'},
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
    private messageService: MessageService,
    private sanitizer: DomSanitizer) {
    this._employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.componentName = this.actRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.getAppreciationsList();
  }

  private getAppreciationsList(): void {
    this._AppreciationService.GetReceiveAppreciationsList(this._employeeId).subscribe((appreciationsResponse: Appreciation[]) => {
      this.appreciationsList = appreciationsResponse;
    },
      (error: any) => {
        this.messageService.add({ severity: 'error', detail: 'Failed to get Appreciations List.', summary: 'Error Message' });
      });
  }
 
  public transformSanitizer(style: string) {
    return this.sanitizer.bypassSecurityTrustHtml(style);
  }

  getMoreAppreciationInfo(Appreciation: string): void {
    this.moreAppreciationInfo = Appreciation;
    this.displayMoreInfoDialog = true;
  }
}
