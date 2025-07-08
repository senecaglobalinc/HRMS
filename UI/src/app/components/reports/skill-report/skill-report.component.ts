import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericType } from '../../../models/dropdowntype.model';
import { MessageService, SelectItem } from 'primeng/api';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import { Router } from '@angular/router';
import * as servicePath from '../../../service-paths';
import { TruncatePipe } from '../../../Pipes/TruncatePipe';
import { SkillsService } from '../../onboarding/services/skills.service';
import { SkillsData } from '../../../models/project-role.model';
import { AssociateAllocationService } from '../../talentmanagement/services/associate-allocation.service';
import { AssociateAllocation } from '../../../models/associateallocation.model';
import { AssociateSkillSearch } from '../../projectLifeCycle/models/associateSkillSearch.model';
import { AssociateSkillsSearchService } from '../../projectLifeCycle/services/associateSkill-search.service';
import { SkillReportService } from '../services/skill-report.service';
import { TalentPoolReportData } from '../models/talentpool.model';
import { take } from 'rxjs/operators';
import { AssociateProjectHistoryService } from '../services/associate-project-history.service';

@Component({
  selector: "app-skill-report",
  templateUrl: "./skill-report.component.html",
  styleUrls: ['./skill-report.component.scss'],
  providers: [SkillReportService, SkillsService, MessageService,BooleanToStringPipe,TruncatePipe]
})
export class SkillReportComponent implements OnInit {

  //fields
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  skillData: GenericType;
  filteredSkills: GenericType[] = [];
  associateSkillSearchData: AssociateSkillSearch[] = [];
  associateSkillSearchObject: AssociateSkillSearch = new AssociateSkillSearch();
  selectedAssociateSkillSearch: AssociateSkillSearch[] = [];
  isPrimary: boolean = false;
  isSecondary: boolean = false;
  isBillable: boolean = false;
  isNonBillable: boolean = false;
  isCritical: boolean = false;
  isNonCritical: boolean = false;
  recordsPerPage = 5;
  first: number = 0;
  gridMessage: string = "No records found";
  totalRecords: number = 0;
  displaySkills: boolean;
  skillsData: SkillsData[] = [];
  EmployeeId: number;
  displayProjectDetails: boolean = false;
  employeeProjectData: AssociateAllocation[];
  headerOfProjectDetails: string = "";
  skillDetailsHeader: string = "";
  heading: string = "";
  showProjectHistory: boolean = false;
  cols = [
    { field: "EmployeeName", header: "Employee", type: "normal" },
    { field: "Designation", header: "Designation", type: "normal" },
    { field: "Grade", header: "Grade", type: "needLessSpace" },
    { field: "Experience", header: "Experience", type: "needLessSpace"},
      // { field: "experience", header: "Exp (Months) ", type: "number" },
    { field: "PrimarySkill", header: "Primary Skill(s)", type: this.truncate },
    { field: "SecondarySkill", header: "Secondary Skill(s)", type: this.truncate },
  ];
  projectCols = [
    { field: "Project", header: "Project" },
    { field: "AllocationPercentage", header: "Allocation Percentage" },
    { field: "Billable", header: "Billable", type: this.yesNoPipe },
    { field: "isCritical", header: "Critical", type: this.yesNoPipe },
    { field: "ManagerName", header: "Reporting Manager" },
    { field: "LeadName", header: "Lead Name" },
  ]

  skillsCols = [

    { field: "SkillName", header: "Skill Name" },
    { field: "ProficiencyLevelCode", header: "Proficiency Level", type: "needLessSpace" },
    // { field: "experience", header: "Exp (Months) ", type: "number" },
    { field: "experience", header: "Exp (Months) ", type: "number" },
    { field: "LastUsed", header: "Last Used", type: "number" },
  ] 
  
  loginUserId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;  
  @ViewChild("dt1") table: any;

  //constructor
  constructor(
    private skillReportsService: SkillReportService,
    private associateSkillSearchService: AssociateSkillsSearchService, 
    private messageService: MessageService, 
    private yesNoPipe: BooleanToStringPipe, 
    private truncate: TruncatePipe, 
    private skillservice: SkillsService,
    private route: Router,
    private associateProjectHistoryService: AssociateProjectHistoryService,
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.associateSkillSearchObject.selectedSkillIds = [];   
  }

  clearData(): void {
    this.skillData = null;
  }

  getStyles(type: string) {
    if (type == 'number') {
      return { 'text-align': 'right', 'width': '120px' };
    }
    if (type == 'needLessSpace') {
      return { 'width': '100px' };
    }
  }

  ViewAllSkills(employeeData, skillType: string): void {
    let index_1 = 0, isPrimary = false;
    this.displaySkills = true;
    if (skillType == 'PrimarySkill') {
      isPrimary = true;
    }
    this.skillsData = [];
    this.skillservice.GetAssociateSkillsById(employeeData.EmployeeId).subscribe((res: SkillsData[]) => {
      this.skillDetailsHeader = "Skill details of " + res[0].EmployeeName;
      res.forEach((e) => {
        if (e.isPrimary == isPrimary)
          this.skillsData[index_1++] = e;
      })
    });

  }

  GetProjectDetailsOfEmployee(employeeData) {
    this.displayProjectDetails = true;
    this.associateSkillSearchService.GetProjectDetailByEmployeeId(employeeData.EmployeeId).subscribe((res: AssociateAllocation[]) => {
      this.employeeProjectData = res;
      this.headerOfProjectDetails = "Project details of " + this.employeeProjectData[0].AssociateName;
    });
  }

  canDisableCheckBox(): boolean {
    if (this.associateSkillSearchObject.selectedSkillIds.length == 0) {
      this.associateSkillSearchObject.IsPrimary = false;
      this.associateSkillSearchObject.IsSecondary = false;
      return true;
    }
    else
      return false;
  }

  getSkillsBySearchString(event: any): void {
    let searchString: string = event.query;
    if (searchString != '' && searchString.length > 0) {
      this.associateSkillSearchService.GetSkillsBySearchString(searchString).subscribe(
        (skillsList: GenericType[]) => {
          this.filteredSkills = [];
          this.filteredSkills = skillsList;
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get Skills' });
          //});
        }
      );
    }
  }

  //get grid data
  searchData(): void {
    this.table.reset();
    this.associateSkillSearchData = [];
    this.selectedAssociateSkillSearch = [];
    this.totalRecords = 0;
    this.first = 0;
    if (this.associateSkillSearchObject.selectedSkillIds != null && this.associateSkillSearchObject.selectedSkillIds.length > 0) {
      this.associateSkillSearchObject.SkillIds = this.associateSkillSearchObject.selectedSkillIds.map(p => p.Id).join(",");
    }
    else
      this.associateSkillSearchObject.SkillIds = null;
    this.skillReportsService.GetEmployeesBySkill(this.associateSkillSearchObject).subscribe((res: AssociateSkillSearch[]) => {
        this.associateSkillSearchData = res;
        if (!this.associateSkillSearchData || this.associateSkillSearchData.length <= 0) {
          this.messageService.add({ severity: 'info', summary: 'Information Message', detail: 'No records found' })
          this.gridMessage = "No records found";
        }
        else
          this.gridMessage = "";
        this.totalRecords = this.associateSkillSearchData.length;
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get Employees' })
      }

    )
  }

  getRows(event): void {
    this.recordsPerPage = event.rows;
  }

  clear() {
    this.associateSkillSearchObject = new AssociateSkillSearch();
    this.selectedAssociateSkillSearch = [];
    this.associateSkillSearchObject.selectedSkillIds = [];
    this.associateSkillSearchData = [];
    this.totalRecords = 0;
    this.gridMessage = "No records found";
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


