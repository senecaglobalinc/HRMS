import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ResourceReportService } from '../../services/resource-report.service';
import { CommonService } from '../../../../core/services/common.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { AllocationCount, AllocationDetails, ProjectDetails, ResourceAllocationDetails } from '../../models/resourcereportbyproject.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from '../../../../../themeconfig';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { Chart } from 'chart.js';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { timeStamp } from 'console';
import { Location } from '@angular/common';
import * as moment from "moment";
import { filter } from "rxjs/operators";
import { NavigationStart, Event as NavigationEvent } from "@angular/router";
import { NgxSpinnerService } from 'ngx-spinner';

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-resource-report-project',
  templateUrl: './resource-report-project.component.html',
  styleUrls: ['./resource-report-project.component.scss']
})
export class ResourceReportProjectComponent implements OnInit {

  defaultIndex = 1;
  themeConfigInput = themeconfig.formfieldappearances;
  projectsForm: FormGroup;
  projectId: number = 0;
  disableProject : boolean = false;
  clearFieldIcon : boolean = false;
  private chartType: string = "pie";
  private componentName: string;
  projectsList: SelectItem[] = [];
  projList : SelectItem[] = [];
  resourceAllocationList: AllocationCount[] = [];
  billableResourceAllocationList: ResourceAllocationDetails[] = [];
  nonBillableCriticalResourceAllocationList: ResourceAllocationDetails[] = [];
  nonBillableNonCriticalResourceAllocationList: ResourceAllocationDetails[] = [];
  private resourceAllocationCount: AllocationCount;
  private resourceReportByProjectChart : any;
  private resourceReportByPieChart : any;
  canvas: any;
  ctx: any;
  selectedProjectId:any;
  dispChart: boolean;

  filteredProject : Observable<any>;
  previous:string;
  resourceAllocationDataSource: MatTableDataSource<AllocationCount>;
  billableResourceAllocationDataSource: MatTableDataSource<ResourceAllocationDetails>;
  nonBillableCriticalResourceAllocationDataSource: MatTableDataSource<ResourceAllocationDetails>;
  nonBillableNonCriticalResourceAllocationDataSource: MatTableDataSource<ResourceAllocationDetails>;
  @ViewChild('billableResourcePaginator', { static: false }) billableResourcePaginator: MatPaginator;
  @ViewChild('nonBillableCriticalResourcePaginator', { static: false }) nonBillableCriticalResourcePaginator: MatPaginator;
  @ViewChild('nonBillableNonCriticalResourcePaginator', { static: false }) nonBillableNonCriticalResourcePaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('resourceChart') resourceChart: any;
  @ViewChild('resourcePieChart') resourcePieChart: any;


  displayedColumnsResourceAllocation: string[] = ['ProjectName','ResourceCount','BillableCount','NonBillableCriticalCount','NonBillableNonCriticalCount'];
  displayedColumnsbillableNonBillabale: string[] = ['AssociateCode','AssociateName','AllocationPercentage','ClientBillingRoleName','IsPrimaryProject','IsCriticalResource','EffectiveDate'];
  
  constructor(private actRoute: ActivatedRoute,
    private fb: FormBuilder,
    private _resourceReportsService: ResourceReportService,
    private _commonService: CommonService,
    private _snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    public navService: NavService,
    private router: Router,private spinner: NgxSpinnerService) { 
      this.navService.changeSearchBoxData('');

      this.navService.currentSearchBoxData.subscribe(responseData => {
        this.applyFilter(responseData);
      });
  }

  ngOnInit(): void {
    this.actRoute.params.subscribe(params => { this.projectId = params["projectId"]; });
    if(this.projectId > 0){
      this.dispChart=true;
      this.projectId = Number(this.projectId);
      this.disableProject = true;
      this.onProjectChange(this.projectId);
    }
    // this.projectsList = [];
    // this.projectsList.splice(0, 0, { label: '', value: 0 });
    this.getProjectList();
    this.previous = this._resourceReportsService.getPreviousUrl();
    this.projectsForm = this.fb.group({
      ProjectId: new FormControl({ value: null, disabled: this.disableProject })
  });

  }

  ngAfterViewInit(){

     this.canvas = <HTMLCanvasElement> document.getElementById('resourceReportByProjectPieChart');
     this.ctx = this.canvas.getContext('2d');

  }

  getProjectList(): void {
    this._masterDataService.GetProjectsList().subscribe((projectResponse: GenericType[]) => {
     
      this.projectsList = [];
      this.projList = [];
      this.projList.push({ label: '', value: null });
      projectResponse.forEach((projectResponse: GenericType) => {
        this.projList.push({ label: projectResponse.Name, value: projectResponse.Id });
      });
      if(this.projectId > 0){
        let obj = this.projList.find(o=>o.value === this.projectId)
        this.projectsForm.controls['ProjectId'].setValue(obj.label);
      }
      this.projectsList = this.projList.filter(
        (project, index, arr) => arr.findIndex(t => t.value === project.value) === index);

      this.filteredProject = this.projectsForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProject(value))
        ); 
    },
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
          });
          this._snackBar.open('Failed to get Project List.', 'x', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      }
    );
  }

  private _filterProject(value) {
    if(value)
      this.clearFieldIcon = true;
    let filterValue;
    if (value && value.ProjectId) {
      filterValue = value.ProjectId.toLowerCase();
      return this.projectsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectsList;
    }
  }

  clearField() {
    this.projectsForm.controls.ProjectId.setValue(null);
    this.clearFieldIcon = false;
    this.onProjectChange(0);
  }

  selectedChangeIds(item){
    this.dispChart=true;
    this.selectedProjectId = item.value;
    this.onProjectChange(this.selectedProjectId);

  }

  onProjectChange(projectId): void {
    this.getResourceReportByProjectId(projectId);
  }

  getResourceReportByProjectId(projectId: number): void {
    this.spinner.show()
    if (projectId != 0) {
      this._resourceReportsService.GetResourceReportByProjectId(projectId).subscribe((resourceReportResponse: AllocationDetails) => {
        this.spinner.hide()
        this.resourceAllocationList = [];
        this.billableResourceAllocationList = [];
        this.nonBillableCriticalResourceAllocationList = [];
        this.nonBillableNonCriticalResourceAllocationList = [];
        this.resourceAllocationCount = new AllocationCount();

        if (resourceReportResponse && resourceReportResponse.AllocationCount) {
          if (resourceReportResponse.AllocationCount) {
            this.resourceAllocationCount = resourceReportResponse.AllocationCount;
            if (this.chartType == "pie") {
              this.getResourceReportPieChart();
            }
            else {
              this.getResourceReportBarChart();
            }
            this.resourceAllocationList.push({
              ProjectName: this.resourceAllocationCount.ProjectName,
              ResourceCount: this.resourceAllocationCount.ResourceCount,
              BillableCount: this.resourceAllocationCount.BillableCount,
              NonBillableCriticalCount: this.resourceAllocationCount.NonBillableCriticalCount,
              NonBillableNonCriticalCount:this.resourceAllocationCount.NonBillableNonCriticalCount
            });
          this.resourceAllocationDataSource = new MatTableDataSource(this.resourceAllocationList);
          }
          if (resourceReportResponse.lstBillableResources && resourceReportResponse.lstBillableResources.length > 0) {
            this.billableResourceAllocationList = resourceReportResponse.lstBillableResources;
            this.billableResourceAllocationList.forEach((record: ResourceAllocationDetails) => {
              record.EffectiveDate = moment(record.EffectiveDate).format('DD-MM-YYYY');
            });
            this.billableResourceAllocationDataSource = new MatTableDataSource(this.billableResourceAllocationList);
            this.billableResourceAllocationDataSource.paginator = this.billableResourcePaginator;
            this.billableResourceAllocationDataSource.sort = this.sort;
          }
          if (resourceReportResponse.lstNonBillableCriticalResources && resourceReportResponse.lstNonBillableCriticalResources.length > 0) {
            this.nonBillableCriticalResourceAllocationList = resourceReportResponse.lstNonBillableCriticalResources;
            this.nonBillableCriticalResourceAllocationList.forEach((record: ResourceAllocationDetails) => {
              record.EffectiveDate = moment(record.EffectiveDate).format('DD-MM-YYYY');
            });
            this.nonBillableCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableCriticalResourceAllocationList);
            this.nonBillableCriticalResourceAllocationDataSource.paginator = this.nonBillableCriticalResourcePaginator;
            this.nonBillableCriticalResourceAllocationDataSource.sort = this.sort;

          }
          if (resourceReportResponse.lstNonBillableNonCriticalResources && resourceReportResponse.lstNonBillableNonCriticalResources.length > 0) {
            this.nonBillableNonCriticalResourceAllocationList = resourceReportResponse.lstNonBillableNonCriticalResources;
            this.nonBillableNonCriticalResourceAllocationList.forEach((record: ResourceAllocationDetails) => {
              record.EffectiveDate = moment(record.EffectiveDate).format('DD-MM-YYYY');
            });
            this.nonBillableNonCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableNonCriticalResourceAllocationList);
            this.nonBillableNonCriticalResourceAllocationDataSource.paginator = this.nonBillableNonCriticalResourcePaginator;
            this.nonBillableNonCriticalResourceAllocationDataSource.sort = this.sort;

          }
        }
        else {
          this._snackBar.open('No records found', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
        (error: any) => {
          this.spinner.hide()
          if (error._body != undefined && error._body != "")
            this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
            });
            this._snackBar.open('Failed to get Resource Report Deatils', 'x', {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        });
    }
    else {
      this.spinner.hide()
      this.resourceAllocationCount = new AllocationCount();
      this.resourceAllocationList = [];
      this.billableResourceAllocationList = [];
      this.nonBillableCriticalResourceAllocationList = [];
      this.nonBillableNonCriticalResourceAllocationList
      this.resourceAllocationDataSource = new MatTableDataSource(this.resourceAllocationList);

      this.billableResourceAllocationDataSource = new MatTableDataSource(this.billableResourceAllocationList);
      this.billableResourceAllocationDataSource.paginator = this.billableResourcePaginator;
      this.billableResourceAllocationDataSource.sort = this.sort;

      this.nonBillableCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableCriticalResourceAllocationList);
      this.nonBillableCriticalResourceAllocationDataSource.paginator = this.nonBillableCriticalResourcePaginator;
      this.nonBillableCriticalResourceAllocationDataSource.sort = this.sort;

      this.nonBillableNonCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableNonCriticalResourceAllocationList);
      this.nonBillableNonCriticalResourceAllocationDataSource.paginator = this.nonBillableNonCriticalResourcePaginator;
      this.nonBillableNonCriticalResourceAllocationDataSource.sort = this.sort;

      this.resourceReportByProjectChart.destroy();
      this.resourceReportByPieChart.destroy();
      
    }
  }

  onBarChartClick(resourceAllocationCount: AllocationCount) {
    this.resourceAllocationCount = resourceAllocationCount;
    this.getResourceReportBarChart();
  }

  onPieChartClick(resourceAllocationCount: AllocationCount) {
    this.resourceAllocationCount = resourceAllocationCount;
    this.getResourceReportPieChart();
  }

  getResourceReportBarChart(): void {
    const canvas = <HTMLCanvasElement> document.getElementById('resourceReportByProjectChart');
    const ctx = canvas.getContext('2d');
  
    if (this.resourceReportByProjectChart) {
      this.resourceReportByProjectChart.destroy();
    }

     this.resourceReportByProjectChart = new Chart(ctx,{
      type : 'bar',
      data : {
        labels: [this.resourceAllocationCount.ProjectName],
        datasets: [
          {
            label: 'Billable Resource(s)',
            backgroundColor: '#00b33c',
            borderColor: '#009933',
            data: [this.resourceAllocationCount.BillableCount]
          },
          {
            label: 'NonBillable Critical Resource(s)',
            backgroundColor: '#ffbb33',
            borderColor: '#ffaa00',
            data: [this.resourceAllocationCount.NonBillableCriticalCount]
          },
          {
            label: 'NonBillable NonCritical Resource(s)',
            backgroundColor: '#ff6666',
            borderColor: '#ff6666',
            data: [this.resourceAllocationCount.NonBillableNonCriticalCount]
          }
        ]
        },
     
    options : {
      title: {
        display: true,
        text: this.resourceAllocationCount.ProjectName,
        fontSize: 16
      },
      legend: {
        position: 'bottom'
      },
      scaleShowVerticalLines: false,
    responsive: true,
    scales: {
            xAxes: [{
                stacked: true
            }],
            yAxes: [{
                stacked: true
            }]
        }
    }
    });
    if (this.resourceChart) {
      setTimeout(() => {
        this.resourceChart.reinit();
      }, 10);
    }
  }


  getResourceReportPieChart(): void {
   
    // const canvas = <HTMLCanvasElement> document.getElementById('resourceReportByProjectPieChart');
    // const ctx = canvas.getContext('2d');
 
    if (this.resourceReportByPieChart) {
      this.resourceReportByPieChart.destroy();
    }
     this.resourceReportByPieChart = new Chart(this.ctx,{
      type : 'pie',
      data : {
        labels : ['Billable','NonBillable critical','NonBillable NonCritical'],
        datasets: [{
            label: 'No. of Resources',
            backgroundColor: ['#00b33c','#ffbb33', '#ff6666'],
            borderColor: ['#009933','#ffaa00', 'ff6666'],
            data: [this.resourceAllocationCount.BillableCount, this.resourceAllocationCount.NonBillableCriticalCount, this.resourceAllocationCount.NonBillableNonCriticalCount]
          }]
        },
      options : {
        responsive:true,
        maintainAspectRatio: false,
        legend: {
          position: 'bottom'
        },
      }
    });

    if (this.resourcePieChart) {
      setTimeout(() => {
        this.resourcePieChart.reinit();
      }, 10);
    }

  }






  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.resourceAllocationDataSource.filter = filterValue.trim().toLowerCase();
      this.billableResourceAllocationDataSource.filter = filterValue.trim().toLowerCase();
      this.nonBillableCriticalResourceAllocationDataSource.filter = filterValue.trim().toLowerCase();
      this.nonBillableNonCriticalResourceAllocationDataSource.filter = filterValue.trim().toLowerCase();
    } else {

      this.resourceAllocationDataSource = new MatTableDataSource(this.resourceAllocationList);

      this.billableResourceAllocationDataSource = new MatTableDataSource(this.billableResourceAllocationList);
      this.billableResourceAllocationDataSource.paginator = this.billableResourcePaginator;
      this.billableResourceAllocationDataSource.sort = this.sort;

      this.nonBillableCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableCriticalResourceAllocationList);
      this.nonBillableCriticalResourceAllocationDataSource.paginator = this.nonBillableCriticalResourcePaginator;
      this.nonBillableCriticalResourceAllocationDataSource.sort = this.sort;

      this.nonBillableNonCriticalResourceAllocationDataSource = new MatTableDataSource(this.nonBillableNonCriticalResourceAllocationList);
      this.nonBillableNonCriticalResourceAllocationDataSource.paginator = this.nonBillableNonCriticalResourcePaginator;
      this.nonBillableNonCriticalResourceAllocationDataSource.sort = this.sort;
    }
  }

  backToProjects(){
    if(this.previous=='/reports/servicereport'){
      this.router.navigate(['/reports/servicereport'])
    }
    else{
    this.router.navigate(['/project/dashboard/']);
  }
}


  changeChart(e) {
    this.defaultIndex = e.index;
    if (e.index == 0) {
      this.getResourceReportBarChart();
    }
    else if (e.index == 1) {
      this.getResourceReportPieChart();
    }
  }


}
