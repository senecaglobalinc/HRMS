import { Component, OnInit, ViewChild } from '@angular/core';
import { DomainDataCount } from '../../models/domainreport.model';
import { EmployeeReportData } from '../../models/employee.model';
// import { SetupChart } from '../../setupchart.component';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../../core/service-paths';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { DomainReportService } from '../../services/domain-report.service';
import { MatPaginator } from '@angular/material/paginator';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatTableModule } from '@angular/material/table';
//import {ChartsModule} from 'ng2-charts/ng2-charts';
import { Chart } from 'chart.js';
import { AllColorCodes } from '../../../../core/color-codes';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-domain-report',
  templateUrl: './domain-report.component.html',
  styleUrls: ['./domain-report.component.scss'],
})
export class DomainReportComponent implements OnInit {
  ctx(arg0: any): any {
    throw new Error('Method not implemented.');
  }
  canvas: any;
  data: any;
  chartType: string = 'bar'; // Used to set chart type dynamically.
  lstDomainData: DomainDataCount[] = [];
  private domainChartSeriesData: number[] = [];
  private domainChartSeriesName: string[] = [];
  domainChartData: any;
  options: any;
  colorCodes = AllColorCodes;
  lstEmployees: EmployeeReportData[] = []; // Used to store employee details of a specific pool
  // private setupChart: SetupChart;
  domainName: string = '';
  employeedisplayedColumns: string[] = [
    'EmployeeCode',
    'EmployeeName',
    'Designation',
    'Grade',
  ];
  domaindisplayedcolumns: string[] = ['DomainName', 'ResourceCount'];

  employeedataSource: MatTableDataSource<EmployeeReportData>;
  domaindataSource: MatTableDataSource<DomainDataCount>;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('domainChart') domainChart: any;
  @ViewChild('domainPieChart') domainPieChart: any;
  

  constructor(
    private _http: HttpClient,
    private _resourceDomainReportsService: DomainReportService,
    public navService: NavService,
    private spinner: NgxSpinnerService
  ) {
    this.employeedataSource = new MatTableDataSource(this.lstEmployees);
    this.employeedataSource.paginator = this.paginator;
    this.employeedataSource.sort = this.sort;

    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
  ngOnInit(): void {
    this.getDomainReportsCount();
  }

  getDomainReportsCount(): void {
    this.spinner.show()
    this._resourceDomainReportsService
      .GetDomainDataCount()
      .subscribe((domainResourceCount: DomainDataCount[]) => {
        this.spinner.hide()
        this.lstDomainData = [];
        this.lstDomainData = domainResourceCount;
        domainResourceCount.forEach((res: any) => {
          this.domainChartSeriesData.push(res.ResourceCount);
          this.domainChartSeriesName.push(res.DomainName);
        });
        if (
          this.domainChartSeriesData.length > 0 &&
          !this.domainChartSeriesData.includes(0)
        )
          this.domainChartSeriesData.push(0);

        if (
          this.domainChartSeriesData.length > 0 &&
          this.domainChartSeriesName.length > 0
        )
          this.renderChart('bar');
        this.domaindataSource = new MatTableDataSource(this.lstDomainData);
        this.domaindataSource.paginator = this.paginator;
        this.domaindataSource.sort = this.sort;
      });

    this.domainChartSeriesData;
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.employeedataSource.filter = filterValue.trim().toLowerCase();
      } else {
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort;
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);
      this.employeedataSource.paginator = this.paginator;
      this.employeedataSource.sort = this.sort;
    }
  }

  renderChart(typeOfChart: string): void {
    if (typeOfChart == 'bar') this.getBarChart();
    else this.getPieChart();
  }
  selectedbarData(event: any) {
    let data = this.domainChart.getElementsAtEvent(event);
    this.domainName = data[0]._model.label;

    let domainData: DomainDataCount = this.lstDomainData.find(
      (data) => data.DomainName.toLowerCase() == this.domainName.toLowerCase()
    );
    this.spinner.show()
    this._resourceDomainReportsService
      .GetEmployeesByDomainId(domainData.DomainID)
      .subscribe((response: EmployeeReportData[]) => {
        this.spinner.hide()
        this.lstEmployees = [];
        if (response.length > 0) this.lstEmployees = response;
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort; 
      });
  }



  selectedPieData(event: any) {
    this.spinner.show()
    let data = this.domainPieChart.getElementsAtEvent(event);
    this.domainName = data[0]._model.label;

    let domainData: DomainDataCount = this.lstDomainData.find(
      (data) => data.DomainName.toLowerCase() == this.domainName.toLowerCase()
    );

    this._resourceDomainReportsService
      .GetEmployeesByDomainId(domainData.DomainID)
      .subscribe((response: EmployeeReportData[]) => {
        this.spinner.hide()
        this.lstEmployees = [];
        if (response.length > 0) this.lstEmployees = response;
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort; 
      });
  }
 
  changeChart(e) {
    if (e.index == 0) {
      this.getBarChart();
    }
    else if (e.index == 1) {
      this.getPieChart();
    }

  }

  getBarChart(): void {
    const canvas = document.getElementById('domainChart');
    const ctx = canvas;
    this.domainChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: this.domainChartSeriesName,
        datasets: [
          {
            label: 'Domain',
            backgroundColor: this.colorCodes,
            data: this.domainChartSeriesData,
          },
        ],
      },
      options: {
        legend: {
          position: 'bottom',
        },
        responsive: true,
        maintainAspectRatio: false,
         barWidth: 1,
        scales: {
          yAxes: [
            {
              ticks: {
                stepSize: 10,
                beginAtZero: true,
              },
            },
          ],
        },
        events: ['click'],
        tooltips: {
          mode: 'nearest',
        },
      },
    });
  }

  getPieChart(): void {

   const canvas = document.getElementById('domainPieChart');
    const ctx = canvas;
    this.domainPieChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: this.domainChartSeriesName,
        datasets: [
          {
            backgroundColor: this.colorCodes,
            data: this.domainChartSeriesData,
          },
        ],
      },
      options: {
        legend: {
          position: 'bottom',
        },
        responsive: false,
      },
    });
  }
}
