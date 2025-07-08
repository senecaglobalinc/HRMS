import { Component, OnInit } from '@angular/core';
import { ResourceReportService } from '../services/resource-report.service';
import { ActivatedRoute } from '@angular/router';
import { MasterDataService } from '../../../services/masterdata.service';
import { ClientService } from '../../admin/services/client.service';
import * as servicePath from '../../../service-paths';
import { SelectItem, Message } from 'primeng/components/common/api';
import {MessageService} from 'primeng/api';
import { ReportsData } from '../models/reportsdata.model';
import { UtilizationReportFilterData, ReportsFilterData } from '../models/reportsfilter.model';
import { PracticeArea } from '../../../models/associate-skills.model';
import {ConfirmationService} from 'primeng/api';

@Component({
  selector: 'app-resourcereportbytechnology',
  templateUrl: './resource-report-by-technology.component.html',
  styleUrls: ['./resource-report-by-technology.component.scss'],
  providers: [ResourceReportService, MasterDataService, MessageService, ClientService,ConfirmationService]
})
export class ResourceReportByTechnologyComponent implements OnInit {
  cols: any[] = [];
  columnOptions: any[] = [];

  associateUtilizationReportList: ReportsData[] = [];
  private associateUtilizationReportExcel: ReportsData[] = [];
  reportsFilterData: ReportsFilterData;
  private resources = servicePath.API.PagingConfigValue;
  private resourceFilter: UtilizationReportFilterData;
  private errorMessage: Message[] = [];
  private componentName: string;
  filterDisplay: boolean = false;
  technologyList: SelectItem[] = []
  totalRecordsCount: number = 0;
  private afterSearchFilter: boolean = false;
  PageSize: number;
  PageDropDown: number[] = [];
  private utilizationPercentage: number = 0;
  isExportToExcel: boolean = false;
  PracticeAreaId: number = 0;
  selectedColumns: any[];

  constructor(private actRoute: ActivatedRoute, private confirmationService: ConfirmationService,
    private resourceReportService: ResourceReportService, private masterDataService: MasterDataService, private messageService: MessageService) {
    this.reportsFilterData = new ReportsFilterData();
    this.reportsFilterData.utilizationReportFilterData = new UtilizationReportFilterData();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
   this.resetFilter();
    this.cols = [
      { field: 'EmployeeCode', header: 'ID' },
      { field: 'EmployeeName', header: 'Name' },
      { field: 'DesignationName', header: 'Designation' },
      { field: 'DepartmentName', header: 'Department' },
      { field: 'SkillCode', header: 'Skill' },
      { field: 'GradeName', header: 'Grade' },
      { field: 'Experience', header: 'Actual Exp' },
      { field: 'ExperienceExcludingCareerBreak', header: 'Exclu. Career Break' },
      { field: 'Technology', header: 'Technology' },
      { field: 'ProjectName', header: 'Project' },
      // { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      // { field: 'AadharNumber', header: 'AadharNumber' },
      { field: 'EmployeeType', header: 'EmployeeType' }
    ];

    this.technologyList = [];
    this.technologyList.splice(0, 0, { label: 'Select Technology', value: 0 });
    this.getTechnologyList();

    this.columnOptions = [];
    for (let i = 0; i < this.cols.length; i++) {
      this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });
    }
    this.selectedColumns = [
      { field: 'EmployeeCode', header: 'ID' },
      { field: 'EmployeeName', header: 'Name' },
      { field: 'DesignationName', header: 'Designation' },
      { field: 'DepartmentName', header: 'Department' },     
      { field: 'GradeName', header: 'Grade' },
      { field: 'Experience', header: 'Actual Exp' },
      { field: 'ExperienceExcludingCareerBreak', header: 'Exclu. Career Break' },
      { field: 'Technology', header: 'Technology' },
      { field: 'ProjectName', header: 'Project' },
      // { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      // { field: 'AadharNumber', header: 'AadharNumber' },
      { field: 'EmployeeType', header: 'EmployeeType' }
    ]
  }

  resetFilter(): void {
    this.reportsFilterData.utilizationReportFilterData = new UtilizationReportFilterData();
    this.reportsFilterData.utilizationReportFilterData.isExportToExcel = false;
  }

  OpenConfirmationDialog() {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to clear ?',
      accept: () => {
        this.resetFilter()
      },
      reject: () => {

      }
    });
  }

  onFilter(): void {
    this.filterDisplay = true;
  }

  getTechnologyList(): void {
    this.masterDataService.GetPractiseAreas().subscribe((clientResponse: PracticeArea[]) => {
      clientResponse.forEach((clientResponse: PracticeArea) => {
        this.technologyList.push({ label: clientResponse.PracticeAreaCode, value: clientResponse.PracticeAreaId })
      });
    }),
      (error: any) => {
        this.messageService.add({ summary: 'Failed to get Technology List', severity: 'error', sticky: true });
      };
  }

  onsearchByFilter(): void {
    this.afterSearchFilter = true;
    this.filterDisplay = false;
    this.searchByFilter(false);
  }

  searchByFilter(isExportRequiered: boolean): void {
    this.reportsFilterData.utilizationReportFilterData.isExportToExcel = isExportRequiered;
    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.PracticeAreaId != 0)
      this.resourceReportService.GetUtilizationReportsByTechnology(this.reportsFilterData).subscribe((resourceReportResponse: ReportsFilterData) => {
        if (resourceReportResponse.reportsData) {
          this.associateUtilizationReportList = resourceReportResponse.reportsData;
          this.totalRecordsCount = resourceReportResponse.TotalCount;
        }
        else {
          this.messageService.add({ summary: 'Failed to get Resource Report List.', severity: 'error', sticky: true });
        }
      },
        (error: any) => {
          this.messageService.add({ summary: 'Failed to get Resource Report List.', severity: 'error', sticky: true });
        });
  }
}
