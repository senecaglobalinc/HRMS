import { Component, OnInit, ViewChild } from '@angular/core';
import { DomainDataCount } from '../../reports/models/domainreport.model';
import { EmployeeReportData } from '../../reports/models/employee.model';
import { SetupChart } from '../setupchart.component';
import { HttpClient} from '@angular/common/http';
import * as servicePath from "../../../service-paths";
import { DomainReportService } from '../Services/domain-report.service';

@Component({
    selector: 'app-domain-report',
    templateUrl: './domain-report.component.html',
    styleUrls: ['./domain-report.component.scss'],
})
export class DomainReportComponent implements OnInit {
    data: any;
    chartType: string = "bar" // Used to set chart type dynamically.
    lstDomainData: DomainDataCount[] = [];
    private domainChartSeriesData: number[] = [];
    private domainChartSeriesName: string[] = [];
    domainChartData: any;
    options: any;
    lstEmployees: EmployeeReportData[] = []; // Used to store employee details of a specific pool
    private setupChart: SetupChart;
    domainName: string = "";
    PageSize: number = 9;
    PageDropDown: number[] = [10];
    private resources = servicePath.API.PagingConfigValue;
    cols = [
        { field: "DomainName", header: "Domain" },
        { field: "ResourceCount", header: "Resource" },
    ]
    cols2 = [
        { field: "EmployeeCode", header: "Employee Code" },
        { field: "EmployeeName", header: "Employee Name" },
        { field: "Designation", header: "Designation" },
        { field: "Grade", header: "Grade" },
    ]

    @ViewChild('domainChart') domainChart: any;
    constructor(private _http: HttpClient, private _resourceDomainReportsService: DomainReportService) {
        this.setupChart = new SetupChart();
        this.PageSize = this.resources.PageSize;
        this.PageDropDown = this.resources.PageDropDown;
    }

    ngOnInit() {
        this.getDomainReportsCount();
    }

    getDomainReportsCount(): void {
        this._resourceDomainReportsService.GetDomainDataCount().subscribe((domainResourceCount: DomainDataCount[]) => {
            this.lstDomainData = [];
            this.lstDomainData = domainResourceCount;
            domainResourceCount.forEach((res: any) => {
                this.domainChartSeriesData.push(res.ResourceCount);
                this.domainChartSeriesName.push(res.DomainName);
            })
            if(this.domainChartSeriesData.length > 0 && !this.domainChartSeriesData.includes(0))
                this.domainChartSeriesData.push(0);
            
            if (this.domainChartSeriesData.length > 0 && this.domainChartSeriesName.length > 0)
                this.renderChart();                                
        });

    }
    renderChart(typeOfChart: string = "bar"): void {

        //Chart Rendering code
        if (typeOfChart == "bar")
            this.domainChartData = this.setupChart.RenderBarChart<string[], number[]>("Domain", "Domain", this.domainChartSeriesName, this.domainChartSeriesData);
        else
            this.domainChartData = this.setupChart.RenderPieChart<string[], number[]>("Domain", this.domainChartSeriesName, this.domainChartSeriesData);
        if (this.domainChart) {
            setTimeout(() => {
                this.domainChart.reinit();
            }, 10);
        }
    }

    selectedbarData(event: any, data: any) {
        //Get Domain Id based on Domain name.
        this.domainName = (event.element._model.label as string).toUpperCase();
        let domainData: DomainDataCount = this.lstDomainData.find(data => data.DomainName.toUpperCase() == this.domainName);

        this._resourceDomainReportsService.GetEmployeesByDomainId(domainData.DomainID).subscribe((response: EmployeeReportData[]) => {
            this.lstEmployees = [];
            if (response.length > 0)
                this.lstEmployees = response;
        })
    }
    onBarChartClick(): void {
        this.chartType = "";
        this.chartType = "bar";
        this.renderChart();
    }
    onPieChartClick(): void {
        this.chartType = "";
        this.chartType = "pie";
        this.renderChart(this.chartType);
    }


}
