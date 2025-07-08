import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ResourceReportService } from '../services/resource-report.service';
import { ActivatedRoute } from '@angular/router';
import { MasterDataService } from '../../../services/masterdata.service';
import { CommonService } from '../../../services/common.service';
import { ClientService } from '../../admin/services/client.service';
import { GenericType } from "../../../models/dropdowntype.model";
import * as servicePath from '../../../service-paths';
import { SelectItem, Message } from 'primeng/components/common/api';
import { UIChart } from 'primeng/components/chart/chart';
import * as moment from "moment";
import { MessageService } from 'primeng/api';
import { AllocationDetails, AllocationCount, ResourceAllocationDetails, ProjectDetails } from '../models/resourcereportbyproject.model';
import { BooleanToStringPipe } from "../../../Pipes/BooleanToStringPipe";
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
  selector: 'app-resource-report-project',
  templateUrl: './resource-report-project.component.html',
  styleUrls: ['./resource-report-project.component.scss'],
  providers: [ResourceReportService, MasterDataService, CommonService, MessageService, ClientService, BooleanToStringPipe]
})
export class ResourceReportProjectComponent implements OnInit {
  private errorMessage: Message[] = [];
  projectsList: SelectItem[] = [];
  private componentName: string;
  projectId: number = 0;
  private resourceAllocationCount: AllocationCount;
  resourceAllocationList: AllocationCount[] = [];
  billableResourceAllocationList: ResourceAllocationDetails[] = [];
  nonBillableResourceAllocationList: ResourceAllocationDetails[] = [];
  private resourceReportByProjectChart: any;
  private options: any;
  private chartType: string = "bar";
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  public cols: any[];
  public billcols: any[];
  disableProject : boolean = false;
  projectsForm: FormGroup;
  @ViewChild('resourceChart') resourceChart: any;

  constructor(private actRoute: ActivatedRoute, private _resourceReportsService: ResourceReportService, private _masterDataService: MasterDataService, 
    private _commonService: CommonService,
    private _formBuilder: FormBuilder,
     private messageService: MessageService, private _clientService: ClientService,private router: Router,
    private yesNoPipe: BooleanToStringPipe) {
    this.resourceAllocationCount = new AllocationCount();
    this.componentName = this.actRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.actRoute.params.subscribe(params => { this.projectId = params["projectId"]; });
    if(this.projectId > 0){
      this.disableProject = true;
      this.onProjectChange(this.projectId);
    }
    this.projectsList = [];
    this.projectsList.splice(0, 0, { label: '', value: 0 });
    this.getProjectList();
    this.cols = [
      { field: "ProjectName", header: "Project Name" },
      { field: "ResourceCount", header: "No.Of.Resources" },
      { field: "BillableCount", header: "Billable Resource" },
      { field: "NonBillableCount", header: "Non Billable Resource" },
    ]
    this.billcols = [
      { field: "AssociateCode", header: "Associate Code" },
      { field: "AssociateName", header: "Associate Name" },
      { field: "AllocationPercentage", header: "Allocation(%)" },
      { field: "InternalBillingRoleName", header: "Internal Billing Role" },
      { field: "ClientBillingRoleName", header: "Client Billing Role" },
      { field: "IsPrimaryProject", header: "Primary", type: this.yesNoPipe },
      { field: "IsCriticalResource", header: "Critical", type: this.yesNoPipe }
    ]
    this.projectsForm = this._formBuilder.group({
      'ProjectId': [null, [Validators.required]],
    });
  }

  getProjectList(): void {        
    this._masterDataService.GetProjectsForDropdown().subscribe((projectResponse: GenericType[]) => {
      projectResponse.forEach((projectResponse: GenericType) => {
        this.projectsList.push({ label: projectResponse.Name, value: projectResponse.Id });
      });     
      if(this.projectId > 0){
        this.projectsForm.controls['ProjectId'].setValue(this.projectId);
      }
      
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
          });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Project List' });
      };
  }

  onProjectChange(projectId): void {
    this.getResourceReportByProjectId(projectId);
  }

  getResourceReportByProjectId(projectId: number): void {
    if (projectId != 0) {
      this._resourceReportsService.GetResourceReportByProjectId(projectId).subscribe((resourceReportResponse: AllocationDetails) => {
        this.resourceAllocationList = [];
        this.billableResourceAllocationList = [];
        this.nonBillableResourceAllocationList = [];
        this.resourceAllocationCount = new AllocationCount();

        if (resourceReportResponse && resourceReportResponse.AllocationCount) {
          if (resourceReportResponse.AllocationCount) {
            this.resourceAllocationCount = resourceReportResponse.AllocationCount;
            if (this.chartType == "bar") {
              this.getResourceReportBarChart();
            }
            else {
              this.getResourceReportPieChart();
            }

            this.resourceAllocationList.push({
              ProjectName: this.resourceAllocationCount.ProjectName,
              ResourceCount: this.resourceAllocationCount.ResourceCount,
              BillableCount: this.resourceAllocationCount.BillableCount,
              NonBillableCount: this.resourceAllocationCount.NonBillableCount
            });
          }
          if (resourceReportResponse.lstBillableResources && resourceReportResponse.lstBillableResources.length > 0) {
            this.billableResourceAllocationList = resourceReportResponse.lstBillableResources;
          }
          if (resourceReportResponse.lstNonBillableResources && resourceReportResponse.lstNonBillableResources.length > 0) {
            this.nonBillableResourceAllocationList = resourceReportResponse.lstNonBillableResources;
          }
        }
        else {
          this.messageService.add({severity: 'info', summary: 'Info', detail: 'No records found'  });
        }
      }),
        (error: any) => {
          if (error._body != undefined && error._body != "")
            this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
            });
          this.messageService.add({severity: 'error', summary: 'Error Message', detail:'Failed to get Resorce Report Details'  });
        };
    }
    else {
      this.resourceAllocationCount = new AllocationCount();
      this.resourceAllocationList = [];
      this.billableResourceAllocationList = [];
      this.nonBillableResourceAllocationList = [];
    }
  }

  getResourceReportBarChart(): void {
    this.chartType = "bar";
    this.resourceReportByProjectChart = {
      labels: [this.resourceAllocationCount.ProjectName],
      datasets: [
        {
          label: 'Billable',
          backgroundColor: '#00b33c',
          borderColor: '#009933',
          data: [this.resourceAllocationCount.BillableCount]
        },
        {
          label: 'NonBillable',
          backgroundColor: '#ffbb33',
          borderColor: '#ffaa00',
          data: [this.resourceAllocationCount.NonBillableCount]
        }
      ]
    };

    this.options = {
      title: {
        display: true,
        text: this.resourceAllocationCount.ProjectName,
        fontSize: 16
      },
      legend: {
        position: 'bottom'
      },
      responsive: false,
      maintainAspectRatio: false,
      barWidth: 1,
      scales: {
        yAxes: [{
          ticks: {
            stepSize: 1,
            beginAtZero: true
          }
        }]
      }
    };

    if (this.resourceChart) {
      setTimeout(() => {
        this.resourceChart.reinit();
      }, 10);
    }
  }

  getResourceReportPieChart(): void {
    this.chartType = "pie";
    this.resourceReportByProjectChart = {
      labels: ['BillableCount', 'NonBillableCount'],
      datasets: [
        {
          data: [this.resourceAllocationCount.BillableCount, this.resourceAllocationCount.NonBillableCount],
          backgroundColor: [
            "#00b33c",
            "#ffbb33"
          ],
          borderColor: [
            "#009933",
            "#ffaa00"
          ]
        }]
    };

    this.options = {
      title: {
        display: true,
        text: this.resourceAllocationCount.ProjectName,
        fontSize: 16
      },
      legend: {
        position: 'bottom'
      },
      responsive: false,
      maintainAspectRatio: false,
    };

    if (this.resourceChart) {
      setTimeout(() => {
        this.resourceChart.reinit();
      }, 10);
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

  backToProjects(){
    this.router.navigate(['/project/dashboard/']);
  }

}
