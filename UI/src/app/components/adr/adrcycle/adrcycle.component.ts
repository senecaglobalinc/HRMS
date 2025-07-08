import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/components/common/api';
import { AdrcycleService } from '../Services/adrcycle.service';
import { ADRCycle } from '../Models/associate-developement.review.model';
import * as servicePath from "../../../service-paths";

declare var _: any;

@Component({
  selector: 'app-adrcycle',
  templateUrl: './adrcycle.component.html',
  styleUrls: ['./adrcycle.component.scss'],
  providers: [ AdrcycleService, MessageService]
})

export class AdrcycleComponent {
  resources = servicePath.API.PagingConfigValue;
  public adrCycleList: ADRCycle[] = [];
  private selectedADRCycleData: ADRCycle;
  private componentName: string;
  public PageSize: number;
  public PageDropDown: number[] = [];

  cols = [
    { field: 'ADRCycleID', header: 'ADRCycle ID'},
    { field: 'ADRCycle', header: 'ADR Cycle'},
    { field: 'IsActive', header: 'IsActive'},
  ];

  constructor(
    private actRoute: ActivatedRoute, 
    private messageService: MessageService, 
    private adrcycleService: AdrcycleService,
    ) {
    this.componentName = this.actRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.GetADRCyclelist();
  }

  private GetADRCyclelist(): void {
    this.adrcycleService.GetADRCycle().subscribe((adrCycleResponseList: ADRCycle[]) => {
      this.adrCycleList = adrCycleResponseList;
      // this.adrCycleList.forEach((adrCycleResponse: ADRCycle) => { adrCycleResponse.checked = adrCycleResponse.IsActive == true; });
    },
      (error: any) => {
        this.messageService.add({ severity: "error", detail: "Failed to get ADR Cycle.", summary: "Error Message" });
      });
  }

  onHandleCycleChange(adrCycleSelectedData: ADRCycle, event: any): void {
    let isActiveForCurrentcycle = adrCycleSelectedData.checked;
    let currentDate = new Date();
    let currentMonth = currentDate.getMonth();
    if (adrCycleSelectedData.checked == false) {
      if (adrCycleSelectedData.ADRCycleID == 1 && (currentMonth >= 3 && currentMonth <= 6)) {
        this.selectedADRCycleData = adrCycleSelectedData;
        isActiveForCurrentcycle = true;
      }
      else if (adrCycleSelectedData.ADRCycleID == 2 && (currentMonth >= 7 && currentMonth <= 10)) {
        this.selectedADRCycleData = adrCycleSelectedData;
        isActiveForCurrentcycle = true;
      }
      else if (adrCycleSelectedData.ADRCycleID == 3 && (currentMonth >= 11 || currentMonth <= 2)) {
        this.selectedADRCycleData = adrCycleSelectedData;
        isActiveForCurrentcycle = true;
      }
      else {
        this.messageService.add({ severity: "warn", detail: 'Sorry! ' + adrCycleSelectedData.ADRCycle + ' is not Current Cycle, Unable to Activate', summary: "Warning Message" });
        event.preventDefault();
      }
    }
    else {
      if (adrCycleSelectedData.ADRCycleID == 1 && (currentMonth >= 3 && currentMonth <= 6)) {
        this.messageService.add({ severity: "warn", detail: 'Sorry! ' + adrCycleSelectedData.ADRCycle + ' is Current Cycle, Unable to Dectivate', summary: "Warning Message" });
        event.preventDefault();
      }
      if (adrCycleSelectedData.ADRCycleID == 2 && (currentMonth >= 7 && currentMonth <= 10)) {
        this.messageService.add({ severity: "warn", detail: 'Sorry! ' + adrCycleSelectedData.ADRCycle + ' is Current Cycle, Unable to Dectivate', summary: "Warning Message" });
        event.preventDefault();
      }
      if (adrCycleSelectedData.ADRCycleID == 3 && (currentMonth >= 11 || currentMonth <= 2)) {
        this.messageService.add({ severity: "warn", detail: 'Sorry! ' + adrCycleSelectedData.ADRCycle + ' is Current Cycle, Unable to Dectivate', summary: "Warning Message" });
        event.preventDefault();
      }
    }
    if (isActiveForCurrentcycle) {
      this.adrCycleList.forEach((adrCycleResponse: ADRCycle) => {
        if (adrCycleSelectedData.ADRCycleID != adrCycleResponse.ADRCycleID)
          adrCycleResponse.checked = false;
      });
    }
  }

  updateADRCycle(): void {
    if (this.selectedADRCycleData.checked == true)
      this.selectedADRCycleData.IsActive = true;
    this.adrcycleService.UpdateADRCycle(this.selectedADRCycleData).subscribe((response) => {
      this.messageService.add({ severity: "success", detail: "ADR Cycle is Updated Successfuly", summary: "Success Message" });
    },
      (error: any) => {
        this.messageService.add({ severity: "error", detail: "Failed to Update ADR Cycle!", summary: "Error Message" });
      });
  }
}