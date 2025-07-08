import { Component, OnInit,ViewChild } from '@angular/core';

import { Router, ActivatedRoute } from "@angular/router";
import { DropDownType, GenericType } from '../../../models/dropdowntype.model';
import { WorkStation, BayInformation, WorkStationDataCount } from '../../../models/workstation.model';
import { SelectItem, Message } from "primeng/components/common/api";
import { OverlayPanel } from 'primeng/components/overlaypanel/overlaypanel';
import { WorkstationService } from "../services/workstation.service";

import { CommonService } from "../../../services/common.service";
import { MasterDataService } from "../../../services/masterdata.service";
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";
import { SetupChart } from "../../reports/setupchart.component";
import { String } from 'core-js';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';

@Component({  
  selector: 'app-workstation',
  templateUrl: './workstation.component.html',
  styleUrls: ['./workstation.component.scss'],
  providers: [WorkstationService, CommonService, MasterDataService]
})
export class WorkstationComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  public bayDisplay: boolean = false;
  organizationTitle: string = "SENECA GLOBAL IT SERVICES PVT LTD";
  public chartType: string = "bar" // Used to set chart type dynamically.
  bayName: string = "";
  public componentName: string;  
  public bayId: number;
  public DeskNumber: string;
  public SeatNumber: string;
  public numberOfRows: number;
  public numberOfColumns: number = 26;
  
  public totalseats: number;
  public vacantSeats: number;
  public occupiedSeats: number;
  public workStationChartData: any; // used to bind to data table
  public setupChart: SetupChart;
  public seatlength: number = 3;
  public seatNumber: string;
  public wSDetil: WorkStation;
  public isVacant : boolean = false;
  public employeeId : number;
  filteredManagersIds: GenericType[] = [];
  public workStationId:string;
  formSubmitted: boolean = false;
  public myForm:FormGroup;
  public deskId:string;
  public name:string;
  public isError:boolean = false;
  public isOverlayPanel:boolean = true;
  overlayPanel: OverlayPanel;
  cols: number[] = [];
  seatsArray: string[] = [];
  rows: number[] = [];
  multiArray: string[][] = [];
  reserved: string[] = [];
  selected: string[] = [];
  seats: string[] = [];
  names: String[] = [];
  options ;
  public PageDropDown: number[] = [];
  public selectedBayId:number;
  public errorMessage: Message[] = [];
  public bayList: BayInformation[] = [];
  public workStationList: WorkStation[] = [];
  public workStationDetil: WorkStation[] = [];
  public workStationDetailList: WorkStation[] = [];
  public lstWorkStationData: WorkStationDataCount[] = []; // Used to store data return by end point
  public lstWorkStationDataTrim: WorkStationDataCount[] = []; // used to stored trimmed data
  public workStationChartSeriesData: number[] = []; //Used to store series data
  public workStationChartSeriesName: string[] = []; // used to store series names

  public workStationName: string = "";
  public PageSize: number;
  public isOrangeByClicked: boolean = false;
  //

  @ViewChild('workStationChart') workStationChart: any;

  constructor(
    private _workStationService: WorkstationService,
    private masterDataService: MasterDataService,
    private actRoute: ActivatedRoute,
    private messageService: MessageService,
    private _commonService: CommonService) {
    this.setupChart = new SetupChart();
    this.componentName = this.actRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
   
  }

  ngOnInit() {
    this.getBayList();
    this.myForm = 
    new FormGroup({
      associateName: new FormControl(null, [
        Validators.required,
      ])
  });

  }

  public getBayList(): void {
    this._workStationService.GetBayDetails().subscribe(
      (bayInformationResponse: BayInformation[]) => {
        this.bayList = bayInformationResponse;
        this.onSelectionChange(this.bayList[0]);
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Bay details.' });
      }
    );
  }

  onSelectionChange(bay: BayInformation) {
    this.bayName = bay.Description;
    if (bay.Description.indexOf("Orange") >= 0) this.isOrangeByClicked = true;
    else this.isOrangeByClicked = false;
    this.selectedBayId = bay.BayId;
    this.getWorkStationsforChart(bay.BayId);
    this.getWorkStation(bay.BayId);
  }

  public getWorkStationsforChart(id: number): void {
    this.workStationChartSeriesName = [];
    this.workStationChartSeriesData = [];
    this._workStationService.GetWorkStationDataCount(id).subscribe((response: WorkStationDataCount[]) => {
      this.lstWorkStationData = response;
      response.forEach(res => {
        this.workStationChartSeriesName.push(res.Name);
        this.workStationChartSeriesData.push(res.WorkStationCount);
      })

      //Ascending Order
      this.lstWorkStationData = [];
      this.lstWorkStationData = this.lstWorkStationDataTrim.sort((objLeft, objRight) => objLeft.WorkStationCount - objRight.WorkStationCount);

      //Render Chart
      this.renderChart(this.chartType);
    }, (error: any) => {
      console.log(error);
    });
  }

  //Chart Rendering code
  public renderChart(typeOfChart: string): void {
    if (typeOfChart == "bar")
      this.workStationChartData = this.setupChart.RenderBarChart<string[], number[]>("WorkStation", "WorkStation", this.workStationChartSeriesName, this.workStationChartSeriesData);
    else
      this.workStationChartData = this.setupChart.RenderPieChart<string[], number[]>("WorkStation", this.workStationChartSeriesName, this.workStationChartSeriesData);

    if (this.workStationChart) {
      setTimeout(() => {
        this.workStationChart.reinit();
      }, 10);
    }
  }

  //get the seat details
  public getWorkStation(id: number): void {
    this._workStationService.GetWorkStationDetails(id).subscribe(
      (WorkStationResponse: WorkStation[]) => {
        this.workStationList = WorkStationResponse;
        this.reserved = [];
        this.multiArray = [];
        this.seats = [];
        if (this.workStationList != null) {
          for (let i of this.workStationList ) {
            this.seats.push(i.WorkStationCode);
            if (i.IsOccupied) {
                    if (i.WorkStationCode.length == 1)
                      this.reserved.push("00" + i.WorkStationCode)
                    else if (i.WorkStationCode.length == 2)
                      this.reserved.push("0" + i.WorkStationCode)
                    else
                      this.reserved.push(i.WorkStationCode);
                  }
          }
          for (var i = 0; i < this.seats.length; i++) {
                if (this.seats[i].length == 1)
                  this.seats[i] = "00" + this.seats[i];
                else if (this.seats[i].length == 2)
                  this.seats[i] = "0" + this.seats[i];
              }
        }
        this.getWorkSattaionCounts();
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Workstation details.' });
      }
    );
  }

  public getWorkSattaionCounts() {
    this.totalseats = this.workStationList.length;
    this.occupiedSeats = this.workStationList.filter(seat => seat.IsOccupied).length;
    this.vacantSeats = this.workStationList.filter(seat => !seat.IsOccupied).length;
  }

  //return status of each seat
  getStatus = function (seat: string) {
    if (this.reserved.indexOf(seat) !== -1) {
      return 'reserved';
    } else if (this.selected.indexOf(seat) !== -1) {
      return 'selected';
    }
    else if (this.seats.indexOf(seat) !== -1) {
      return 'empty';
    }
  }

  //clear handler
  clearSelected = function () {
    this.selected = [];
  }

  //Buy button handler
  showSelected = function () {
    if (this.selected.length > 0) {
      alert("Selected Cubical: " + this.selected);
    } else {
      alert("No Cubical selected!");
    }
  }

  public onBarChartClick(): void {
    this.chartType = "";
    this.chartType = "bar";
    this.renderChart(this.chartType);
  }
  public onPieChartClick(): void {
    this.chartType = "";
    this.chartType = "pie";
    this.renderChart(this.chartType);
  }
  //seat selection
  seatClicked(event: any, wsId: string, overlayPanel: OverlayPanel) {
    this.workStationId=wsId;
    if (this.reserved.indexOf(wsId) !== -1) {
      //display the seat information
      if (!wsId.includes("-")) {
        wsId = Number(wsId).toString();
      }
      this.getSeatInformation(wsId);
      this.bayDisplay = true;
      this.overlayPanel = overlayPanel;
      this.overlayPanel.toggle(event);
     
      //
    }
    else if(this.selected.indexOf(wsId) !== -1) {
      //need to create a popup to capture the data
      this.selected=this.selected.filter(p=>p!=wsId);
      //
    }
    else if (this.reserved.indexOf(wsId) == -1) {
      this.isVacant = true;
    }
    else{
      this.selected.push(wsId);

    }
  }
  // get associate names
  filteredManagers(event: any): void {
    this.isError = false;
    let suggestionString = event.query;
    this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
        this.filteredManagersIds = [];
        this.filteredManagersIds = managersResponse;
    },
        (error: any) => {
            // if (error._body != undefined && error._body != "")
            //     this._commonService.LogError(this.componentName, error._body).then((data: any) => {
            //     });
            // this.errorMessage = [];
            // this.errorMessage.push({ severity: 'error', summary: 'Failed to get Associates List!' });
        });
}
DeskAllocation(){
  this.formSubmitted = true;
  if(this.myForm.valid){   
  this._workStationService.DeskAllocation(this.myForm.value.associateName.Id,this.workStationId).subscribe(
    (res) => {     
      if(res==1)
      {
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Employee Desk Allocation Successfully done' });
        this.ClearDeskAllocation();
      }
      else if(res.toString().includes("$"))
      {
        this.isError = true;
        let result:string[] = res.toString().split("$")
        this.deskId = result[0];
        this.name = result[1];     
      }
    },
    error => {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed Desk Allocation' });
    }
  );
  }
}
    
ClearDeskAllocation(): void {
 this.formSubmitted = false;
 this.myForm.reset();
 this.isVacant = false;
 this.isError = false;
}
  //get the reserved seat information
  public getSeatInformation(wsId: string) {
    this.names = [];
    this.workStationDetil = [];

    this._workStationService.GetWorkStationDetailByWorkStationId(wsId).subscribe(      
      (res: WorkStation[]) => {
        if (res != null && res.length > 0) {
          //prepare the view data
          this.cols = res.map(p => p.EmployeeId);
          this.cols = Array.from(new Set(this.cols.map((item: any) => item)));
          for (var i = 0; i < this.cols.length; i++) {
            this.wSDetil = new WorkStation();
            this.wSDetil.EmployeeId = res[0].EmployeeId;
            this.wSDetil.WorkStationId = res[0].WorkStationId;
            this.wSDetil.AssociateName = res.filter(p => p.EmployeeId == this.cols[i]).map(p => p.AssociateName)[0];

            this.names = [];
            this.names = res.filter(p => p.EmployeeId == this.cols[i]).map(p => p.LeadName);
            this.names = Array.from(new Set(this.names.map((item: any) => item)));
            this.wSDetil.LeadName = this.names.join(",");

            this.names = [];
            this.names = res.filter(p => p.EmployeeId == this.cols[i]).map(p => p.ProjectName);
            this.names = Array.from(new Set(this.names.map((item: any) => item)));
            this.wSDetil.ProjectName = this.names.join(",");
            this.workStationDetil.push(this.wSDetil);
          }
        }
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Workstation details.' });
      }
    )
  }
  ReleaseDesk(){
    
    this._workStationService.ReleaseDesk(this.wSDetil.EmployeeId,this.wSDetil.WorkStationId).subscribe(res=>{
        if(res==1)
        {
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Desk Release Successfully done' });
         // this.ClearDeskAllocation();
          this.getWorkStation(this.selectedBayId);
         // this.isOverlayPanel=false;detail
         this.overlayPanel.toggle(event);
         
        }
      },
      (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed To Release Desk' });
      });
  }
}


