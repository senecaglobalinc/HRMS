import { Component, OnInit, ViewChild } from '@angular/core';
import { error } from 'util';
import { TalentpoolreportService } from '../services/talentpoolreport.service';
import { TalentpoolDataCount, TalentPoolReportData } from '../models/talentpool.model';
import { EmployeeReportData } from '../models/employee.model';
import { SetupChart } from '../setupchart.component';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import * as servicePath from "../../../service-paths";
import { MessageService } from 'primeng/api';
import { take } from 'rxjs/operators';
import { AssociateProjectHistoryService } from '../services/associate-project-history.service';

@Component({
  selector: 'app-talentpool',
  templateUrl: './talentpool.component.html',
  styleUrls: ['./talentpool.component.scss']
})

export class TalentpoolComponent implements OnInit {
  chartType: string = "bar" // Used to set chart type dynamically.
  lstTalentpoolData: TalentpoolDataCount[] = []; // Used to store data return by end point
  private lstTalentpoolDataTrim: TalentpoolDataCount[] = []; // used to stored trimmed data
  private talentpoolChartSeriesData: number[] = []; //Used to store series data
  private talentpoolChartSeriesName: string[] = []; // used to store series names
  talentpoolChartData: any; // used to bind to data table
  lstEmployees: EmployeeReportData[] = []; // Used to store employee details of a specific pool
  options: any;
  private setupChart: SetupChart;
  talentPoolName: string = "";
  public heading: string;
  PageSize: number;
  PageDropDown: number[] = [];
  showProjectHistory: boolean = false;
  private resources = servicePath.API.PagingConfigValue;
  
  cols = [
    { field: 'ProjectName', header: 'Talent Pool'},
    { field: 'ResourceCount', header: 'Resources'} 
  ];

  cols1 = [
    { field: 'EmployeeCode', header: 'Employee Code'},
    { field: 'EmployeeName', header: 'Employee Name'},
    { field: 'Designation', header: 'Designation'}, 
    { field: 'Grade', header: 'Grade'},
    { field: 'Experience', header: 'Experience'}
  ];

  @ViewChild('talentpoolChart') talentpoolChart: any;

  constructor(
    private _http: HttpClient, 
    private _talentpoolReportsService: TalentpoolreportService,
    private associateProjectHistoryService: AssociateProjectHistoryService,
    private messageService: MessageService) {
    this.setupChart = new SetupChart();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._talentpoolReportsService.GetTalentpoolResourceCount().subscribe((response: TalentpoolDataCount[]) => {
      this.lstTalentpoolData = response;
      response.forEach(res => {
        this.talentpoolChartSeriesName.push(res.ProjectName.replace('Talent Pool - ', ''));
        this.talentpoolChartSeriesData.push(res.ResourceCount);
      })

      //Remove "Talent Pool - " sentence from Project Name.
      this.lstTalentpoolData.forEach((talenPoolData: TalentpoolDataCount) => {
        talenPoolData.ProjectName = talenPoolData.ProjectName.replace('Talent Pool - ', '');
        this.lstTalentpoolDataTrim.push(talenPoolData);
      })

      if(this.talentpoolChartSeriesData.length > 0 && !this.talentpoolChartSeriesData.includes(0))
      this.talentpoolChartSeriesData.push(0);

      //Ascending Order
      this.lstTalentpoolData = [];
      this.lstTalentpoolData = this.lstTalentpoolDataTrim.sort((objLeft, objRight) => objLeft.ResourceCount - objRight.ResourceCount);

      //Render Chart
      this.renderChart(this.chartType);
    }, (error: any) => {
    });
  }

  renderChart(typeOfChart: string): void {
    //Chart Rendering code
    if (typeOfChart == "bar")
      this.talentpoolChartData = this.setupChart.RenderBarChart<string[], number[]>("Talent Pool", "Talent Pool", this.talentpoolChartSeriesName, this.talentpoolChartSeriesData);
    else
      this.talentpoolChartData = this.setupChart.RenderPieChart<string[], number[]>("Talent Pool", this.talentpoolChartSeriesName, this.talentpoolChartSeriesData);

    if (this.talentpoolChart) {
      setTimeout(() => {
        this.talentpoolChart.reinit();
      }, 10);
    }
  }

  selectedbarData(event: any) {
    //Get Talent Pool project Id based on pool name.
    this.talentPoolName = (event.element._model.label as string).toUpperCase();
    let talentpoolproject: TalentpoolDataCount = this.lstTalentpoolData.find(data => data.ProjectName.toUpperCase() == this.talentPoolName);

    this._talentpoolReportsService.GetEmployeesByTalentPoolProjectId(talentpoolproject.ProjectID).subscribe((response: EmployeeReportData[]) => {
      this.lstEmployees = [];
      if (response.length > 0)
        this.lstEmployees = response;
    })
  }

  onBarChartClick(): void {
    this.chartType = "";
    this.chartType = "bar";
    this.renderChart(this.chartType);
  }

  onPieChartClick(): void {
    this.chartType = "";
    this.chartType = "pie";
    this.renderChart(this.chartType);
  }

  public getProjectHistoryByEmployee(talentPoolReportData: TalentPoolReportData): void {
    if (talentPoolReportData.EmployeeId > 0) {
      this.associateProjectHistoryService.GetProjectHistoryByEmployee(talentPoolReportData.EmployeeId).pipe(take(1)).subscribe(res => {
        this.showProjectHistory = true;
        this.associateProjectHistoryService.SetProjectHistory(res);
        this.heading = "Project history of " + talentPoolReportData.EmployeeName;
      });
    }
  }

}
