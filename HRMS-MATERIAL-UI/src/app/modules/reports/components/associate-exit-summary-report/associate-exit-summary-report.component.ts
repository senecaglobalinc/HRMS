import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AssociateExitReportService } from '../../services/associate-exit-report.service';
import {
  ChartData,
  AssociateExitReportFilter,
  AssociateExitReportResponse
} from '../../models/associate-exit-report.model';
import { GenericType, GenericModel } from '../../../../modules/master-layout/models/dropdowntype.model';
import { Validators, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import * as servicePath from '../../../../core/service-paths';
import { take } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Chart } from 'chart.js';
import { MatTableDataSource } from '@angular/material/table';
import { AllColorCodes } from '../../../../core/color-codes';
import { NavService } from '../../../master-layout/services/nav.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { ReportsData } from '../../models/reportsdata.model';
import * as moment from 'moment';

@Component({
  selector: 'app-associate-exit-summary-report',
  templateUrl: './associate-exit-summary-report.component.html',
  styleUrls: ['./associate-exit-summary-report.component.scss']
})
export class AssociateExitSummaryReportComponent implements OnInit {
  themeAppearence = themeconfig.formfieldappearances;
  chartType = 'pie'; // Used to set chart type dynamically.
  lstAssociateExitData: ChartData[] = []; // Used to store data return by end point
  private lstAssociateExitDataTrim: ChartData[] = []; // used to stored trimmed data
  reportDetails: AssociateExitReportResponse[] = [];
  private associateExitChartSeriesData: number[] = []; // Used to store series data
  private associateExitChartSeriesName: string[] = []; // used to store series names
  reportName : string = '';
  reportValue : string = '';
  filter:AssociateExitReportFilter;
  lstEmployees: AssociateExitReportResponse[] = []; 
  componentName: string; myForm: FormGroup;
  totalRecordsCount: number;
  errorMessage: any[];
  searchFormSubmitted: boolean = false;
  summaryList: any[] = [];
  options: any;
  searchData: AssociateExitReportFilter; 
  errorSummary: string = '';
  step=0;
  loading: boolean;
  public reportStatus: boolean = false;
  isLoading: boolean = false;
  firstDate: Date;
  lastDate: Date;
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
  
  displayedColumns: string[] = ['Label', 'Value'];
  displayedColumnss: string[] = ['AssociateCode','AssociateName','Grade','Gender','Department','TechnologyGroup', 'JoinDate',"ExitDate", 'Project', 'ProgramManager','ReportingManager', 'ExitType', 'ExitCause', 'RehireEligibility', 'LegalExit','ImpactOnClientDelivery','ServiceTenure','ServiceTenureWithSG','ServiceTenurePriorToSG','ServiceTenureRange','ServiceTenureWithSGRange','FinancialYear','Quarter'];

  employeedataSource: MatTableDataSource<AssociateExitReportResponse>;
  lstAssociateExitDataSource: MatTableDataSource<ChartData>;

  @ViewChild('resourceReportByProjectChart') resourceReportByProjectChart: any;
  @ViewChild('resourceReportByPieProjectChart') resourceReportByPieProjectChart: any;

  @ViewChild('sort1') sort1: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('sort2') sort2: MatSort; 

  ctx(arg0: any): any {
    throw new Error('Method not implemented.');
  }

  constructor(private associateExitReportService: AssociateExitReportService,
              private fileExporterService: FileExporterService,
              public navService: NavService,
              private spinner: NgxSpinnerService,
              private _snackBar: MatSnackBar,
              private fb: FormBuilder
              ) {
    this.searchData = new AssociateExitReportFilter();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {   
   
    this.myForm = this.fb.group({
      fromDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      reportType: ['', [Validators.required]]      
    });   
    
    this.getSummaryTypes();  
  }

  getSummaryTypes(): void {
    this.associateExitReportService.GetAssociateExitReportTypes().subscribe((res: GenericType[]) => {      
      this.summaryList = [];
 //    this.summaryList.push({ label: '', value: null });
      res.forEach((element: GenericType) => {
        this.summaryList.push({ label: element.Name, value: element.Id });
      });          
    },
      (error: any) => {        
        this._snackBar.open('Failed to Get Summary Type List!', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
            verticalPosition: 'top',
        });
      }
    );
  }

  refreshExitData()
  {
    this.employeedataSource = new MatTableDataSource(this.reportDetails);    
    this.employeedataSource.paginator = this.paginator;
    this.employeedataSource.sort = this.sort1;         
  }

  fetchAssociateExitReport() {
    
    if(this.myForm.valid){
      this.step = 1;
    }   
    this.isLoading = true;
    this.searchFormSubmitted = true;
    if (this.myForm.controls.fromDate.value === '' || this.myForm.controls.toDate.value === '' || this.myForm.controls.fromDate.value === null || this.myForm.controls.toDate.value === null || this.myForm.controls.reportType.value === null || this.myForm.controls.reportType.value.value === undefined) {
      this.isLoading = false;     
      return;
    }    
    
    this.spinner.show()
    if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
      this.errorSummary = '';
      if (moment(this.searchData.FromDate).isAfter(new Date())) {
        this.errorSummary = 'From date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('From date should be less than today', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

        return false;
      }
      if (moment(this.searchData.ToDate).isAfter(new Date())) {
        this.errorSummary = 'To date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('To date should be less than today', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
      if (moment(this.searchData.FromDate).isAfter(this.searchData.ToDate)) {
        this.errorSummary = 'From Date should be less than To Date';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('From Date should be less than To Date', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }      
      this.reportValue = '';
      this.searchData.FromDate = moment(this.searchData.FromDate).format('YYYY-MM-DD');
      this.searchData.ToDate = moment(this.searchData.ToDate).format('YYYY-MM-DD');
      this.getChartData(this.searchData);
      this.GetAssociateExitReportDetails(this.searchData);
      this.totalRecordsCount = this.reportDetails.length;  
    }
  }

  GetAssociateExitReportDetails(filter: AssociateExitReportFilter): void{   
    this.spinner.show();
    this.reportDetails = [];
    this.associateExitReportService.GetAssociateExitGridReport(filter).subscribe(
      (res: AssociateExitReportResponse[]) => {
        if(res != null && res.length > 0
          ) {       
          this.reportDetails = res; 
          this.reportDetails.forEach((record: AssociateExitReportResponse) => {            
            record.ImpactOnClientDeliveryStr = (record.ImpactOnClientDelivery == true) ? 'Yes' : 'No'; 
            record.RehireEligibilityStr = (record.RehireEligibility == true) ? 'Yes' : 'No';
            record.LegalExitStr = (record.LegalExit == true) ? 'Yes' : 'No';           
          });
          this.employeedataSource = new MatTableDataSource(this.reportDetails);
          this.employeedataSource.paginator = this.paginator;
          this.employeedataSource.sort = this.sort1; 

          switch(filter.ReportType)
          {
            case 1:
              this.employeedataSource.filterPredicate =
      (data: AssociateExitReportResponse, filter: string) => data.ExitCause == filter; 
              break;
              case 2:
              this.employeedataSource.filterPredicate =
      (data: AssociateExitReportResponse, filter: string) => data.RehireEligibilityStr == filter; 
              break;
              case 3:
              this.employeedataSource.filterPredicate =
      (data: AssociateExitReportResponse, filter: string) => data.Gender.startsWith(filter); 
              break;
              case 4:
                this.employeedataSource.filterPredicate =
        (data: AssociateExitReportResponse, filter: string) => data.Grade == filter; 
                break;
                case 5:
                  this.employeedataSource.filterPredicate =
          (data: AssociateExitReportResponse, filter: string) => data.ImpactOnClientDeliveryStr == filter; 
                  break;
                  case 6:
                    this.employeedataSource.filterPredicate =
            (data: AssociateExitReportResponse, filter: string) => data.LegalExitStr == filter; 
                    break;
                    case 7:
                      this.employeedataSource.filterPredicate =
              (data: AssociateExitReportResponse, filter: string) => data.ProgramManager == filter; 
                      break;
                      case 8:
                        this.employeedataSource.filterPredicate =
                (data: AssociateExitReportResponse, filter: string) => data.TechnologyGroup == filter; 
                        break;
                        case 9:
                          this.employeedataSource.filterPredicate =
                  (data: AssociateExitReportResponse, filter: string) => data.ServiceTenureRange == filter; 
                          break;
                          case 10:
                            this.employeedataSource.filterPredicate =
                    (data: AssociateExitReportResponse, filter: string) => data.ServiceTenureWithSGRange == filter; 
                            break;
                            case 11:
                              this.employeedataSource.filterPredicate =
                      (data: AssociateExitReportResponse, filter: string) => data.ExitType == filter; 
                              break;
          }
          
          this.spinner.hide();
        }
        else {  
          this.spinner.hide();
          this.employeedataSource = new MatTableDataSource(this.reportDetails);
          this.employeedataSource.paginator = this.paginator;
          this.employeedataSource.sort = this.sort1; 
          this._snackBar.open('No records found', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
      (error: any) => {        
        this.spinner.hide();
        this._snackBar.open('Unable to get Associate Exit details', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  } 

  getChartData(filter: AssociateExitReportFilter) {
    this.spinner.show();
    this.associateExitChartSeriesName = [];
    this.associateExitChartSeriesData = [];
    this.lstAssociateExitData = [];
    this.associateExitReportService.GetAssociateExitChartReport(filter).subscribe(
      (response: ChartData[]) => {
        this.spinner.hide();
        this.lstAssociateExitData = response;


        response.forEach((res) => {
          if(res.Label == 'True')
          res.Label =  'Yes';
          else  if(res.Label == 'False')
          res.Label =  'No';         
          this.associateExitChartSeriesName.push(res.Label);
          this.associateExitChartSeriesData.push(res.Value);
        });      

        // Render Chart
        this.renderChart(this.chartType);       
        this.lstAssociateExitDataSource = new MatTableDataSource(this.lstAssociateExitData);       
        this.lstAssociateExitDataSource.sort = this.sort2       
      },
      (error: any) => { this.spinner.hide(); }
    );
  }
  
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.employeedataSource.filter = filterValue.trim().toLowerCase();
      } else {
        this.employeedataSource = new MatTableDataSource(this.reportDetails);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort1; 
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.reportDetails);
      this.employeedataSource.paginator = this.paginator;
      this.employeedataSource.sort = this.sort1; 
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

  clearInput(evt: any, fieldName): void {
    if(fieldName=='toDate'){
      evt.stopPropagation();
      this.myForm.get('toDate').reset();
    }
    if(fieldName=='fromDate'){
      evt.stopPropagation();
      this.myForm.get('fromDate').reset();
    }
  }

  selectedbarData(event: any) {
    this.spinner.show();    
    const data = this.myChart.getElementsAtXAxis(event);   
      if (data.length > 0) {
      let filterValue  = (data[0]._model.label as string);
      this.reportValue = filterValue; 
      this.employeedataSource.filter = filterValue;            
    }
    this.spinner.hide();    
  }

  selectedPieData(event: any) {    
    this.spinner.show();
    const data = this.myPieChart.getElementsAtEvent(event);    
    if (data.length > 0) {
      let filterValue  = (data[0]._model.label as string);
      this.reportValue = filterValue; 
      this.employeedataSource.filter = filterValue;        
    }
    this.spinner.hide();    
  }

  onBarChartClick(): void {
    this.chartType = 'bar';
    this.renderChart(this.chartType);
  }
  onPieChartClick(): void {
    this.chartType = 'pie';
    this.renderChart(this.chartType);
  }

   plugin = {
    id: 'emptyChart',
    afterDraw(chart, args, options) {
        const { datasets } = chart.data;
        let hasData = false;

        for (let dataset of datasets) {
            //set this condition according to your needs
            if (dataset.data.length > 0 && dataset.data.some(item => item !== 0)) {
                hasData = true;
                break;
            }
        }

        if (!hasData) {
            
            const { chartArea: { left, top, right, bottom }, ctx } = chart;
            const centerX = (left + right) / 2;
            const centerY = (top + bottom) / 2;

            chart.clear();
            ctx.save();
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            ctx.fillText('No data', centerX, centerY);
            ctx.restore();
        }
    }
};

   myChart : any = null;
  getResourceReportBarChart(): void {     
     
    const canvas = document.getElementById('resourceReportByProjectChart');  
    if( this.myChart ) 
    {
      this.myChart?.destroy();    
    }
    
    const ctx = canvas;    
    this.myChart = new Chart(ctx,   {
    plugins: [this.plugin],
      type: 'bar',
      data: {
        labels: this.associateExitChartSeriesName,
        datasets: [
          {
            label: this.reportName,
            backgroundColor: this.colorCodes,
            data: this.associateExitChartSeriesData,
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
myPieChart : any = null;
  getResourceReportPieChart(): void {
    const canvas = document.getElementById('resourceReportByPieProjectChart');
    const ctx = canvas;
    if( this.myPieChart ) 
    {
      this.myPieChart?.destroy();    
    }
    this.myPieChart = new Chart(ctx, {
      plugins: [this.plugin],
      type: 'pie',
      data: {
        labels: this.associateExitChartSeriesName,
        datasets: [
          {
            backgroundColor: this.colorCodes,
            data: this.associateExitChartSeriesData,
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
      this.chartType = 'pie';
      this.getResourceReportPieChart();
    }
    else if (e.index === 1) {
      this.chartType = 'bar';
      this.getResourceReportBarChart();
    }

  }

  
 
  clearFilter() {
   this.myForm.reset();
   this.myForm.clearValidators(); 
    this.searchFormSubmitted = false;
    this.errorSummary = '';    
    this.searchData = new AssociateExitReportFilter();
    this.reportName = '';
    this.reportValue = '';
    this.associateExitChartSeriesName = [];
    this.associateExitChartSeriesData = [];
    this.renderChart(this.chartType);    
    this.lstAssociateExitData = [];
    this.reportDetails = [];
    this.lstAssociateExitDataSource = new MatTableDataSource(this.lstAssociateExitData);    
    this.lstAssociateExitDataSource.sort = this.sort2; 
    this.employeedataSource = new MatTableDataSource(this.reportDetails);
    this.employeedataSource.paginator = this.paginator;
    this.employeedataSource.sort = this.sort1; 
    this.totalRecordsCount = this.reportDetails.length;    
    }
 
  exportAsXLSX() {    
    this.spinner.show();
    const columnsForExcel = this.employeedataSource.data.map(x => ({
      'Associate Code': x.AssociateCode,
      'Associate Name': x.AssociateName,
      'Grade': x.Grade,
      'Gender': x.Gender,
      'Department': x.Department,
      'Technology Group': x.TechnologyGroup,     
      'Project': x.Project,
      'Program Manager': x.ProgramManager,
      'Reporting Manager': x.ReportingManager, 
      'Join Date':  new Date(moment(x.JoinDate).format('MM/DD/YYYY')),
      'Exit Date': new Date(moment(x.ExitDate).format('MM/DD/YYYY')),
      'Exit Type': x.ExitType,
      'Exit Cause': x.ExitCause,
      'Eligible for Rehire': x.RehireEligibilityStr,
      'Impact On Client Delivery': x.ImpactOnClientDeliveryStr,
      'Legal Exit': x.LegalExitStr,
      'Service Tenure':x.ServiceTenure,
      'Service Tenure With SG':x.ServiceTenureWithSG,
      'Service Tenure Prior To SG':x.ServiceTenurePriorToSG,
      'Service Tenure Range':x.ServiceTenureRange,
      'Service Tenure With SG Range':x.ServiceTenureWithSGRange,
      'Financial Year': x.FinancialYear,
      'Quarter': x.Quarter,      
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, "Associate Exit Report");    
    this.spinner.hide();
  }

}
