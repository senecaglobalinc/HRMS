import { Component, OnInit, ViewChild } from '@angular/core';
import { TalentpoolreportService } from '../../services/talentpoolreport.service';
import {
  TalentpoolDataCount,
  TalentPoolReportData,
} from '../../models/talentpool.model';
import { EmployeeReportData } from '../../models/employee.model';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import * as servicePath from '../../../../core/service-paths';
import { take } from 'rxjs/operators';
import { AssociateProjectHistoryService } from '../../services/associate-project-history.service';
import { MatSort } from '@angular/material/sort';
import { Chart } from 'chart.js';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AllColorCodes } from '../../../../core/color-codes';
import { NavService } from '../../../master-layout/services/nav.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { ReportsData } from '../../models/reportsdata.model';
import * as moment from 'moment';

@Component({
  selector: 'app-talentpool',
  templateUrl: './talentpool.component.html',
  styleUrls: ['./talentpool.component.scss'],
})
export class TalentpoolComponent implements OnInit {
  chartType = 'bar'; // Used to set chart type dynamically.
  lstTalentpoolData: TalentpoolDataCount[] = []; // Used to store data return by end point
  private lstTalentpoolDataTrim: TalentpoolDataCount[] = []; // used to stored trimmed data

  private talentpoolChartSeriesData: number[] = []; // Used to store series data
  private talentpoolChartSeriesName: string[] = []; // used to store series names

  lstEmployees: EmployeeReportData[] = []; // Used to store employee details of a specific pool

  options: any;
  // private setupChart: SetupChart;
  talentPoolName = '';
  public heading: string;
  colorCodes = AllColorCodes;
  PageSize: number;
  PageDropDown: number[] = [];
  showProjectHistory = false;
  private resources = servicePath.API.PagingConfigValue;
  canvas: any;
  dataSource: MatTableDataSource<any>;
  talentpoolReportData: ReportsData[];

  displayedColumns: string[] = ['ProjectName', 'ResourceCount'];
  displayedColumnss: string[] = [
    'EmployeeCode',
    'EmployeeName',
    'Designation',
    'Grade',
    'Experience',
    'IsResigned',
    'IsLongLeave',
    'DurationInDays',
    'FutureProjectName',
    'FutureProjectTentativeDate'
  ];

  employeedataSource: MatTableDataSource<EmployeeReportData>;
  lstTalentpoolDataSource: MatTableDataSource<TalentpoolDataCount>;

  @ViewChild('resourceReportByProjectChart') resourceReportByProjectChart: any;
  @ViewChild('resourceReportByPieProjectChart') resourceReportByPieProjectChart: any;

  @ViewChild('sortTalentpoolData') sortTalentpoolData: MatSort;
  @ViewChild('sortEmployeeData') sortEmployeeData: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  ctx(arg0: any): any {
    throw new Error('Method not implemented.');
  }

  constructor(private _talentpoolReportsService: TalentpoolreportService,
              private fileExporterService: FileExporterService,
              public navService: NavService,
              private spinner: NgxSpinnerService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  }

  getData() {
    this.spinner.show();
    this._talentpoolReportsService.GetTalentpoolResourceCount().subscribe(
      (response: TalentpoolDataCount[]) => {
        this.spinner.hide();
        this.lstTalentpoolData = response;

        response.forEach((res) => {
          this.talentpoolChartSeriesName.push(
            res.ProjectName.replace('Talent Pool - ', '')
          );
          this.talentpoolChartSeriesData.push(res.ResourceCount);
        });

        this.lstTalentpoolData.forEach((talenPoolData: TalentpoolDataCount) => {
          talenPoolData.ProjectName = talenPoolData.ProjectName.replace(
            'Talent Pool - ',
            ''
          );
          this.lstTalentpoolDataTrim.push(talenPoolData);
        });

        if (
          this.talentpoolChartSeriesData.length > 0 &&
          !this.talentpoolChartSeriesData.includes(0)
        ) {
          this.talentpoolChartSeriesData.push(0);
        }

        // Ascending Order
        this.lstTalentpoolData = [];
        this.lstTalentpoolData = this.lstTalentpoolDataTrim.sort(
          (objLeft, objRight) => objLeft.ResourceCount - objRight.ResourceCount
        );
        this.lstTalentpoolDataSource = new MatTableDataSource(this.lstTalentpoolData);
        // this.lstTalentpoolDataSource.paginator = this.paginator;
        this.lstTalentpoolDataSource.sort = this.sortTalentpoolData;

        // Render Chart
        this.renderChart(this.chartType);
      },
      (error: any) => { this.spinner.hide(); }
    );
  }
  ngOnInit() {
    this.getData();
    this.lstTalentpoolDataSource = new MatTableDataSource(this.lstTalentpoolData);
    // this.lstTalentpoolDataSource.paginator = this.paginator;
    this.lstTalentpoolDataSource.sort = this.sortTalentpoolData;
  }
  applyFilter(event: Event) {
    debugger;
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.employeedataSource.filter = filterValue.trim().toLowerCase();
      } else {
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sortEmployeeData;
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);
      this.employeedataSource.paginator = this.paginator;
      this.employeedataSource.sort = this.sortEmployeeData;
    }
  }
  renderChart(typeOfChart: string): void {
    // Chart Rendering code
    if (typeOfChart === 'bar') {
      this.getResourceReportBarChart();
    }
    else {
      this.getResourceReportPieChart();
    }
  }

  selectedbarData(event: any) {
    this.spinner.show();
    // Get Talent Pool project Id based on pool name.
    const data = this.resourceReportByProjectChart.getElementsAtXAxis(event);
    if (data.length > 0) {
      this.talentPoolName = (data[0]._model.label as string).toUpperCase();
    }
    const talentpoolproject: TalentpoolDataCount = this.lstTalentpoolData.find(
      (talentPoolData) => talentPoolData.ProjectName.toUpperCase() === this.talentPoolName.toUpperCase()
    );
    this.lstTalentpoolDataSource = new MatTableDataSource(this.lstTalentpoolData);
    // this.lstTalentpoolDataSource.paginator = this.paginator;
    this.lstTalentpoolDataSource.sort = this.sortTalentpoolData;

    this._talentpoolReportsService
      .GetEmployeesByTalentPoolProjectId(talentpoolproject.ProjectID)
      .subscribe((response: EmployeeReportData[]) => {
        this.spinner.hide();
        this.lstEmployees = [];
        if (response.length > 0) {
          this.lstEmployees = response;
        }
        this.lstEmployees.forEach((ele: EmployeeReportData) => {
          ele.IsResigned = (ele.IsResigned.toString() === 'true') ? 'Yes' : null;
          ele.IsLongLeave = (ele.IsLongLeave.toString() === 'true') ? 'Yes' : null;
          ele.FutureProjectTentativeDate = ele.FutureProjectTentativeDate && moment(ele.FutureProjectTentativeDate).format('DD-MM-YYYY')
        });
        console.log(this.lstEmployees)
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sortEmployeeData;
      }, (error) => { this.spinner.hide(); });
  }


  selectedPieData(event: any) {
    // Get Talent Pool project Id based on pool name.
    const data = this.resourceReportByPieProjectChart.getElementsAtEvent(event);
    this.talentPoolName = (data[0]._model.label as string).toUpperCase();
    const talentpoolproject: TalentpoolDataCount = this.lstTalentpoolData.find(
      (talentPoolData) => talentPoolData.ProjectName.toUpperCase() === this.talentPoolName.toUpperCase()
    );
    this.lstTalentpoolDataSource = new MatTableDataSource(
      this.lstTalentpoolData
    );
    // this.lstTalentpoolDataSource.paginator = this.paginator;
    this.lstTalentpoolDataSource.sort = this.sortTalentpoolData;

    this._talentpoolReportsService
      .GetEmployeesByTalentPoolProjectId(talentpoolproject.ProjectID)
      .subscribe((response: EmployeeReportData[]) => {
        this.lstEmployees = [];
        if (response.length > 0) {
          this.lstEmployees = response;
        }
        this.lstEmployees.forEach((ele: EmployeeReportData) => {
          ele.IsResigned = (String(ele.IsResigned).toLowerCase() === 'true') ? 'Yes' : null;
          ele.IsLongLeave = (String(ele.IsLongLeave).toLowerCase() === 'true') ? 'Yes' : null;
        });
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sortEmployeeData;
      });
  }

  onBarChartClick(): void {
    this.chartType = 'bar';
    this.renderChart(this.chartType);
  }
  onPieChartClick(): void {
    this.chartType = 'pie';
    this.renderChart(this.chartType);
  }
  getResourceReportBarChart(): void {
    const canvas = document.getElementById('resourceReportByProjectChart');
    const ctx = canvas;
    this.resourceReportByProjectChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: this.talentpoolChartSeriesName,
        datasets: [
          {
            label: 'Talent Pool',
            backgroundColor: this.colorCodes,
            data: this.talentpoolChartSeriesData,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        barWidth: 1,
        scales: {
          yAxes: [
            {
              ticks: {
                stepSize: 5,
                beginAtZero: true,
              },
            },
          ],
        },
        legend: {
          position: 'bottom',
        },
      },
    });
  }

  getResourceReportPieChart(): void {
    const canvas = document.getElementById('resourceReportByPieProjectChart');
    const ctx = canvas;
    this.resourceReportByPieProjectChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: this.talentpoolChartSeriesName,
        datasets: [
          {
            backgroundColor: this.colorCodes,
            data: this.talentpoolChartSeriesData,
          },
        ],
      },
      options: {
        legend: {
          position: 'bottom',
        },
        responsive: true,
        maintainAspectRatio: false,
      },
    });
  }

  changeChart(e) {
    if (e.index === 0) {
      this.getResourceReportBarChart();
    }
    else if (e.index === 1) {
      this.getResourceReportPieChart();
    }

  }

  async getExportData() {
    const response = await this._talentpoolReportsService.GetTalentPoolResourceReport().toPromise();
    return response;
  }

  async exportAsXLSX() {
    this.talentpoolReportData = await this.getExportData();
    this.talentpoolReportData.forEach((ele: ReportsData) => {
      ele.Experience = Number(ele.Experience).toFixed(2);
      ele.JoinDate = moment(ele.JoinDate).format('YYYY-MM-DD');
      ele.FutureProjectTentativeDate = ele.FutureProjectTentativeDate && moment(ele.FutureProjectTentativeDate).format('YYYY-MM-DD');
      ele.ExperienceExcludingCareerBreak = Number(ele.ExperienceExcludingCareerBreak).toFixed(2);
      ele.IsBillableForExcel = (ele.IsBillable === true) ? 'Yes' : 'No';
      ele.IsCriticalForExcel = (ele.IsCritical === true) ? 'Yes' : 'No';
      ele.IsResignedForExcel = (ele.IsResigned === true) ? 'Yes' : null;
      ele.IsLongLeaveForExcel = (ele.IsLongLeave === true) ? 'Yes' : null;
    });
    this.dataSource = new MatTableDataSource(this.talentpoolReportData);
    const columnsForExcel = this.dataSource.data.map(x => ({
      ID: x.EmployeeCode,
      Name: x.EmployeeName,
      Designation: x.DesignationName,
      Grade: x.GradeName,
      Experience: x.Experience,
      Technology: x.Technology,
      'SG Joined Date': x.JoinDate,
      Project: x.ProjectName,
      Client: x.ClientName,
      Billable: x.IsBillableForExcel,
      Critical: x.IsCriticalForExcel,
      '(%) Utilization': x.Allocationpercentage,
      Lead: x.LeadName,
      'Reporting Manager': x.ReportingManagerName,
      Skill: x.SkillCode,
      'Program Manager': x.ProgramManagerName,
      Resigned: x.IsResignedForExcel,
      'Resignation Date': x.ResignationDate,
      'Last Working Date': x.LastWorkingDate,
      'Long Leave': x.IsLongLeaveForExcel,
      'Long Leave Start Date': x.LongLeaveStartDate,
      'Long Leave Tentative Join Date': x.TentativeJoinDate,
      'Future Project': x.FutureProjectName,
      'Future Project Tentative Date': x.FutureProjectTentativeDate
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, 'TalentPool Report');
  }

}
