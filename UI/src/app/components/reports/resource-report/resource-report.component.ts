import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ResourceReportService } from '../services/resource-report.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { CommonService } from '../../../services/common.service';
import * as moment from "moment";
import { ReportsData } from '../models/reportsdata.model';
import { UtilizationReportFilterData, ReportsFilterData } from '../models/reportsfilter.model';
import { AllocationDetails } from '../models/resourcereportbyproject.model';
import { GenericType, DropDownType } from '../../../models/dropdowntype.model';
import { ProjectDetails } from '../../../models/projects.model';
import { Client } from '../../admin/models/client.model';
import { Grade } from '../../admin/models/grade.model';
import { Designation } from '../../admin/models/designation.model';
import { SelectItem, MessageService } from 'primeng/api';
import { PracticeArea } from '../../../models/associate-skills.model';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import * as servicePath from '../../../service-paths';
import {ConfirmationService} from 'primeng/api';
import { PercentageDropDown } from '../../talentmanagement/models/associateallocation.model';
declare let alasql: any;
declare var _: any;
@Component({
  selector: 'app-resource-report',
  templateUrl: './resource-report.component.html',
  styleUrls: ['./resource-report.component.scss'],
  providers: [MessageService, ResourceReportService, CommonService, MasterDataService, BooleanToStringPipe,ConfirmationService]
})
export class ResourceReportComponent implements OnInit {
  cols: any[] = [];
  columnOptions: any[] = [];
  PageSize: number = 5;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  associateUtilizationReportList: ReportsData[] = [];
  private associateUtilizationReportExcel: ReportsData[] = [];
  reportsFilterData: ReportsFilterData;
  private resourceFilter: UtilizationReportFilterData;
  private errorMessage: MessageService[] = [];
  componentName: string;
  filterDisplay: boolean = false;
  technologyList: SelectItem[] = [];
  associatesList: SelectItem[] = [];
  projectsList: SelectItem[] = [];
  gradesList: SelectItem[] = [];
  experienceList: SelectItem[] = [];
  clinentsList: SelectItem[] = [];
  programManagersList: SelectItem[] = [];
  BillableList: SelectItem[] = [];
  CriticalList: SelectItem[] = [];
  designationsList: SelectItem[] = [];
  percentageList: SelectItem[] = [];
  expList: GenericType[] = [];
  totalRecordsCount: number = 0;
  afterSearchFilter: boolean = false;
  allBillable: number = -1;
  allCritical: number = -1;
  utilizationPercentage: number = 0;
  isBillable = new Array<DropDownType>();
  isCritical = new Array<DropDownType>();
  private userRole: any;
  private employeeId: any;
  selectedColumns: any[];
  constructor(private actRoute: ActivatedRoute, private resourceReportService: ResourceReportService, private masterDataService: MasterDataService, private messageService: MessageService, private commonService: CommonService
    , private yesNoPipe: BooleanToStringPipe,  private confirmationService: ConfirmationService) {
    this.reportsFilterData = new ReportsFilterData();
    this.reportsFilterData.utilizationReportFilterData = new UtilizationReportFilterData();
    this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    }
  ngOnInit() {
    this.userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.resetFilter();
    this.cols = [
      
      { field: 'EmployeeCode', header: 'ID' },
      { field: 'EmployeeName', header: 'Name' },
      { field: 'DesignationName', header: 'Designation' },
      { field: 'GradeName', header: 'Grade' },
      { field: 'ExperienceExcludingCareerBreak', header: 'Experience' },
      { field: 'Technology', header: 'Technology' },
      { field: 'JoinDate', header: ' SG JoinDate' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'ClientName', header: 'Client' },
      { field: 'IsBillableForExcel', header: 'Billable' }, //, type: this.yesNoPipe
      { field: 'IsCriticalForExcel', header: 'Critical' },
      { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
      { field: 'SkillCode', header: 'Skill' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
   
  
      
    ];
    this.columnOptions = [];
    for (let i = 0; i < this.cols.length; i++) {
      this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });
    }
    this.selectedColumns =[
      { field: 'EmployeeCode', header: 'ID' },
      { field: 'EmployeeName', header: 'Name' },
      { field: 'DesignationName', header: 'Designation' },
      { field: 'GradeName', header: 'Grade' },
      { field: 'ExperienceExcludingCareerBreak', header: 'Experience' },
      { field: 'Technology', header: 'Technology' },
      { field: 'JoinDate', header: ' SG JoinDate' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'ClientName', header: 'Client' },
      { field: 'IsBillableForExcel', header: 'Billable' }, //, type: this.yesNoPipe
      { field: 'IsCriticalForExcel', header: 'Critical' },
      { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
 
 
      
      ]
      this.isBillable.push({ label: 'All', value: -1 });
      this.isBillable.push({ label: 'Yes', value: 1 });
      this.isBillable.push({ label: 'No', value: 0 });

      this.isCritical.push({ label: 'All', value: -1 });
      this.isCritical.push({ label: 'Yes', value: 1 });
      this.isCritical.push({ label: 'No', value: 0 });

    
  }
  

  onFilter(): void {
    if (!this.afterSearchFilter) {
      debugger;
      
      if (this.reportsFilterData.utilizationReportFilterData.IsBillable == -1) this.reportsFilterData.utilizationReportFilterData.IsBillable = 0;
      if (this.reportsFilterData.utilizationReportFilterData.IsCritical == -1) this.reportsFilterData.utilizationReportFilterData.IsCritical = 0
      this.associatesList = [];
      this.associatesList.splice(0, 0, { label: '', value: 0 });
      this.projectsList = [];
      this.projectsList.splice(0, 0, { label: '', value: 0 });
      this.gradesList = [];
      this.gradesList.splice(0, 0, { label: '', value: 0 });
      this.experienceList = [];
      this.experienceList.splice(0, 0, { label: '', value: 0 });
      this.clinentsList = [];
      this.clinentsList.splice(0, 0, { label: '', value: 0 });
      this.programManagersList = [];
      this.programManagersList.splice(0, 0, { label: '', value: 0 });
      this.BillableList = [];
      this.BillableList.splice(0, 0, { label: '', value: 0 });
      this.CriticalList = [];
      this.CriticalList.splice(0, 0, { label: '', value: 0 });
      this.designationsList = [];
      this.designationsList.splice(0, 0, { label: '', value: 0 });
      this.percentageList = [];
      this.percentageList.splice(0, 0, { label: '', value: 0 });
      this.technologyList = [];
      this.technologyList.splice(0, 0, { label: '', value: 0 });
      this.expList = [
        { Id: 1, Name: "0-5" },
        { Id: 2, Name: "5-10" },
        { Id: 3, Name: "10-15" },
        { Id: 4, Name: "15-20" },
        { Id: 5, Name: "20-25" },
        { Id: 6, Name: "25-30" },
        { Id: 7, Name: "30-35" },
        { Id: 8, Name: "35-40" }];
      this.getAssociateList();
      this.getExperienceList();
      this.getProjectList();
      this.getGradeList();
      this.getClientList();
      this.getProgramManagerList();
      this.getDesignationsList();
      this.getPercentageList();
      this.getTechnologyList();
      this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
      this.reportsFilterData.utilizationReportFilterData.IsCritical = -1;
      //this.resetFilter();
    }
    this.filterDisplay = true;
  }
  getAssociateList(): void {
    this.masterDataService.GetAllAssociateList().subscribe((associateResponse: GenericType[]) => {
      associateResponse.forEach((associateResponse: GenericType) => {
        this.associatesList.push({ label: associateResponse.Name, value: associateResponse.Id });
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Associate List' });
      };

  }

  getProjectList(): void {
    this.masterDataService.GetProjectsList().subscribe((projectResponse: ProjectDetails[]) => {
      let projList : any[] = [];
      this.projectsList = [];
      projList.push({ label: '', value: null });
      projectResponse.forEach((projectResponse: ProjectDetails) => {
        projList.push({ label: projectResponse.ProjectName, value: projectResponse.ProjectId });
      });
      this.projectsList = projList.filter(
        (project, index, arr) => arr.findIndex(t => t.value === project.value) === index);
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Project List' });

      };
  }

  getGradeList(): void {
    this.masterDataService.GetGradesDetails().subscribe((gradeResponse: Grade[]) => {
      gradeResponse.forEach((gradeResponse: Grade) => {
        this.gradesList.push({ label: gradeResponse.GradeName, value: gradeResponse.GradeId })
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Grade List' });
      };
  }

  getExperienceList(): void {
    this.expList.forEach((experience: GenericType) => {
      this.experienceList.push({ label: experience.Name, value: experience.Id });
    });
  }

  getClientList(): void {
    this.masterDataService.GetClientList().subscribe((clientResponse: any) => {
      clientResponse.forEach((clientResponse) => {
        this.clinentsList.push({ label: clientResponse.ClientName, value: clientResponse.ClientId })
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Client List' });
      };
  }

  getProgramManagerList(): void {
    this.masterDataService.GetProgramManagers().subscribe((programManagerResponse: GenericType[]) => {
      programManagerResponse.forEach((programManagerResponse: GenericType) => {
        this.programManagersList.push({ label: programManagerResponse.Name, value: programManagerResponse.Id })
      })
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Program Managers List' });
      };
  }

  getDesignationsList(): void {
    this.masterDataService.GetDesignationList().subscribe((designationResponse: Designation[]) => {
      designationResponse.forEach((designationResponse: Designation) => {
        this.designationsList.push({ label: designationResponse.DesignationName, value: designationResponse.DesignationId })
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Designation List' });
      };
  }

  getPercentageList(): void {
    this.masterDataService.GetAllocationPercentages().subscribe((percentageResponse: PercentageDropDown[]) => {
      percentageResponse.forEach((percentageResponse: PercentageDropDown) => {
        if (percentageResponse.Percentage != 0)
          this.percentageList.push({ label: JSON.stringify(percentageResponse.Percentage), value: percentageResponse.AllocationPercentageId });
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Percentage List' });
      };
  }
  getTechnologyList(): void {
    this.masterDataService.GetPractiseAreas().subscribe((clientResponse: PracticeArea[]) => {
      clientResponse.forEach((clientResponse: PracticeArea) => {
        this.technologyList.push({ label: clientResponse.PracticeAreaCode, value: clientResponse.PracticeAreaId })
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Technology List' });
      };
  }

  filterExperience(ExperienceId: number): string {
    let experienceRange: string = '';
    if (this.expList.length > 0 && ExperienceId) {
      experienceRange = this.expList[ExperienceId - 1].Name;
    }
    return experienceRange;
  }

  checkBillable(event){
    debugger;
    if(event.value == -1){
      this.onAllBillableChange(event);
    }
    else
    this.onBillableChange(event);
  }
 

  onBillableChange(event: any) {
    let billable = event.value;
    this.allBillable = 0;
    this.reportsFilterData.utilizationReportFilterData.IsBillable = billable;
  }
  onAllBillableChange(event: any) {
    this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
    this.allBillable = -1;
  }


  checkCritical(event){
    if(event.value == -1){
      this.onAllCriticalChange(event);
    }
    else
    this.onCriticalChange(event);
  }

  onCriticalChange(event: any) {
    let critical = event.value;
    this.allCritical = 0;
    this.reportsFilterData.utilizationReportFilterData.IsCritical = critical;
  }



  onAllCriticalChange(event: any) {
    this.reportsFilterData.utilizationReportFilterData.IsCritical = -1;
    this.allCritical = -1;
  }

  onsearchByFilter(): void {
    this.afterSearchFilter = true;
    this.filterDisplay = false;
    this.searchByFilter(false);
  }

  searchByFilter(isExportRequiered: boolean): void {
    debugger;
    this.getsliderValue();
    this.reportsFilterData.utilizationReportFilterData.isExportToExcel = isExportRequiered;

    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.IsBillable == 0 && this.allBillable == -1)
      this.reportsFilterData.utilizationReportFilterData.IsBillable = this.allBillable;
    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.IsCritical == 0 && this.allCritical == -1)
      this.reportsFilterData.utilizationReportFilterData.IsCritical = this.allCritical;
    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.ExperienceId && this.reportsFilterData.utilizationReportFilterData.ExperienceId != 0)
      this.reportsFilterData.utilizationReportFilterData.ExperienceRange = this.filterExperience(this.reportsFilterData.utilizationReportFilterData.ExperienceId);
    this.resourceReportService.ResourceReportByFilters(this.reportsFilterData.utilizationReportFilterData).subscribe((resourceReportResponse: ReportsData[]) => {
      this.associateUtilizationReportList = resourceReportResponse;
      this.associateUtilizationReportList.forEach((ele: ReportsData) => {
        ele.Experience = Number(ele.Experience).toFixed(2);
        ele.JoinDate = moment(ele.JoinDate).format("YYYY-MM-DD");
        ele.ExperienceExcludingCareerBreak = Number(ele.ExperienceExcludingCareerBreak).toFixed(2);
        ele.IsBillableForExcel = (ele.IsBillable == true) ? 'Yes': 'No';
        ele.IsCriticalForExcel =(ele.IsCritical == true) ? 'Yes': 'No';
      });
      //this.totalRecordsCount = resourceReportResponse.TotalCount;
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Resource Report List' });
      };
  }

  OpenConfirmationDialog() {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to clear ?',
      accept: () => {
        this.resetFilter();
      },
      reject: () => {

      }
    });
  }


  resetFilter(): void {
    this.allBillable = -1;
    this.allCritical = -1;
    this.utilizationPercentage  = 0;
    this.reportsFilterData.utilizationReportFilterData = {
      EmployeeId: 0,
      ProjectId: 0,
      GradeId: 0,
      DesignationId: 0,
      ClientId: 0,
      AllocationPercentageId: 0,
      
      ProgramManagerId: 0,
      ExperienceId: 0,
      ExperienceRange: null,
      Experience: 0,
      IsBillable: -1,
      IsCritical: 0,
  
      isExportToExcel: false,
      PracticeAreaId: 0,
      RowsPerPage: 20,
      PageNumber: 1,
    };
  }

  getsliderValue() {
    if (this.utilizationPercentage > 0) {
      let res: any = this.percentageList.find(Percentage => Percentage.label == this.utilizationPercentage.toString());
      this.reportsFilterData.utilizationReportFilterData.AllocationPercentageId = res.value;
    }

  }
}




