import { Component, OnInit, ViewChild } from '@angular/core';
import { GenericType } from '../../../models/dropdowntype.model';
import { AssociateSkillSearch } from '../models/associateSkillSearch.model';
import { AssociateSkillsSearchService } from '../services/associateSkill-search.service';
import { MessageService, SelectItem } from 'primeng/api';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import { Router } from '@angular/router';
import * as servicePath from '../../../service-paths';
import { TruncatePipe } from '../../../Pipes/TruncatePipe';
import { SkillsService } from '../../onboarding/services/skills.service';
import { SkillsData } from '../../../models/project-role.model';
import { TagAssociateList } from '../models/tag-associate.model';
import { TagAssociateService } from '../services/tag-associate.service';
import { AssociateAllocationService } from '../../talentmanagement/services/associate-allocation.service';
import { AssociateAllocation } from '../../../models/associateallocation.model';

@Component({
  selector: 'app-associateSkill-search',
  templateUrl: './associateSkill-search.component.html',
  styleUrls: ['./associateSkill-search.component.scss'],
  providers: [MessageService, BooleanToStringPipe, TruncatePipe]
})
export class AssociateSkillSearchComponent implements OnInit {

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
  isDisplay: boolean = false;  

  cols = [
    { field: "EmployeeName", header: "Employee", type: "normal" },
    { field: "Designation", header: "Designation", type: "normal" },
    { field: "Grade", header: "Grade", type: "needLessSpace" },
    { field: "Experience", header: "Experience", type: "number" },
    { field: "PrimarySkill", header: "Primary Skill(s)", type: this.truncate },
    { field: "SecondarySkill", header: "Secondary Skill(s)", type: this.truncate },
    { field: "ProjectName", header: "Project", type: "normal" },
    { field: "Allocationpercentage", header: "Allocation Percentage", type : 'number' },
    { field: "IsBillable", header: "Billable", type: 'boolean' },
    { field: "IsCritical", header: "Critical", type: 'boolean' },
    { field: "ManagerName", header: "Reporting Manager", type: "normal" },
    { field: "LeadName", header: "Lead Name", type: "normal"},
  ];

  skillsCols = [

    { field: "SkillName", header: "Skill Name" },
    { field: "ProficiencyLevelCode", header: "Proficiency Level", type: "needLessSpace" },
    { field: "SkillExperience", header: "Exp (Months) ", type: "number" },
    { field: "LastUsed", header: "Last Used", type: "number" },
    // { field: "ProficiencyLevelCode", header: "Proficiency Level" },
    // { field: "SkillExperience", header: "Exp (Months) " },
    // { field: "LastUsed", header: "Last Used"},
  ]

  //Tag list fields
  tagListDetails: TagAssociateList[] = [];
  selectedTagListData: TagAssociateList = new TagAssociateList();
  tagListNames: SelectItem[] = [];
  tagListData: TagAssociateList = new TagAssociateList();
  displayTagList: boolean = false;
  loginUserId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
  isNewTagList: string = "0";
  @ViewChild("dt1") table: any;

  constructor(
    private associateSkillSearchService: AssociateSkillsSearchService, 
    private messageService: MessageService,
    private yesNoPipe: BooleanToStringPipe, 
    private truncate: TruncatePipe, 
    private skillservice: SkillsService,
    private tagAssociateService: TagAssociateService,
    private route: Router,
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;

  }

  ngOnInit() {
    this.associateSkillSearchObject.selectedSkillIds = [];
    this.getTagListNames();
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
    if(type == 'boolean'){
      return { 'text-align': 'center' };
    }
  }

  ViewAllSkills(employeeData, skillType: string): void {
    let index_1 = 0, isPrimary = false;
    this.displaySkills = true;
    if (skillType == 'PrimarySkill') {
      isPrimary = true;
    }
    this.skillsData = [];
    this.skillservice.GetAllSkillDetails(employeeData.EmployeeId).subscribe((res: SkillsData[]) => {
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
        }
      );
    }
  }

  //get grid data
  searchData(): void {
    this.isDisplay = true;
      this.associateSkillSearchData = [];
    this.selectedAssociateSkillSearch = [];
    this.totalRecords = 0;
    this.first = 0;
    if (this.associateSkillSearchObject.selectedSkillIds != null && this.associateSkillSearchObject.selectedSkillIds.length > 0) {
      this.associateSkillSearchObject.SkillIds = this.associateSkillSearchObject.selectedSkillIds.map(p => p.Id).join(",");
    }
    else
      this.associateSkillSearchObject.SkillIds = null;
    this.associateSkillSearchService.GetEmployeeDetailsBySkill(this.associateSkillSearchObject).subscribe(
      (res: AssociateSkillSearch[]) => {
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
    this.isDisplay = false;
  }

  //Tag list
  addToTagList(): void {
    //check the grid selection and open the popup.
    this.selectedTagListData = new TagAssociateList();
    if (this.selectedAssociateSkillSearch && this.selectedAssociateSkillSearch.length > 0) {
      this.displayTagList = true;
      this.selectedTagListData = new TagAssociateList();
    }
    else {
      this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Select assoiciates to tag' });
    }
  }

  viewTagList(): void {
    //navigate to Tag list screen
    this.route.navigate(['/project/tagAssociate']);
  }

  tagListOnChange(event): void {
    this.selectedTagListData.TagListId = event.value;
    let names = this.tagListNames.filter(p => p.value == event.value);
    if (names.length > 0) {
      this.selectedTagListData.TagListName = names[0].label;
    }
  }

  saveToTagList(): void {
    this.tagListDetails = [];
    if (this.selectedAssociateSkillSearch && this.selectedAssociateSkillSearch.length > 0) {
      this.selectedAssociateSkillSearch.forEach(element => {
        this.tagListData = new TagAssociateList();
        this.tagListData.EmployeeId = element.EmployeeId;
        this.tagListData.ManagerId = this.loginUserId;
        this.tagListData.ProjectId = element.ProjectId;
        if (this.selectedTagListData.TagListId > 0) {
          this.tagListData.TagListId = this.selectedTagListData.TagListId;
          this.tagListData.TagListName = this.selectedTagListData.TagListName;
        }
        else if (this.selectedTagListData.TagListName) {
          this.tagListData.TagListName = this.selectedTagListData.TagListName;
          this.tagListData.TagListId = 0;
        }
        else {
          this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Select or enter Tagged listname' });
          return;
        }
        this.tagListDetails.push(this.tagListData);
      });

      //update
      if (this.selectedTagListData.TagListId > 0) {
        this.tagAssociateService.UpdateTagList(this.tagListDetails).subscribe(((res: number) => {
          if (res == 1) {
            this.cleartheTagListData();
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Tag list updated successfully.' })
            this.searchData();
            this.getTagListNames();
          }
          else if (res == -1) {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Associates already assigned to the selected taglist' })
          }
        }),
          error => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to save the Tag list' })
          });
      }

      //create
      else if (this.selectedTagListData.TagListName) {
        this.tagAssociateService.CreateTagList(this.tagListDetails).subscribe(((res: number) => {
          if (res == 1) {
            this.cleartheTagListData();
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Tag list created successfully.' })
            this.searchData();
            this.getTagListNames();
          }
          else if (res == -1)
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Tag list name already exists for respective Reporting manager/delivery head.' })
        }),
          error => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to save the Tag list' })
          });
      }
    }
  }

  CancelTagList(): void {
    this.displayTagList = false;
    this.tagListDetails = [];
    this.cleartheTagListData();
  }

  getTagListNames(): void {
    this.tagListNames = [];
    this.tagListNames.push({ label: "Select list", value: 0 });
    this.tagAssociateService.GetTagListNamesByManagerId(this.loginUserId).subscribe((res: TagAssociateList[]) => {
      res.forEach(element => {
        this.tagListNames.push({ label: element.TagListName, value: element.Id })
      })
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get Tag list names' })
      });
  }

  cleartheTagListData() {
    this.displayTagList = false;
    this.selectedTagListData = new TagAssociateList();
    this.tagListDetails = [];
    this.tagListData = new TagAssociateList();
  }

  onRadioClick(): void {
    if (this.isNewTagList == "1") {
      this.selectedTagListData.TagListName = "";
    }
    else {
      this.selectedTagListData = new TagAssociateList();
    }
  }
 
}
