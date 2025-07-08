import { Component, OnInit, ViewChild, QueryList, ViewChildren } from '@angular/core';
import { Router } from '@angular/router';
import * as servicePath from '../../../../../core/service-paths';
import { themeconfig } from 'src/themeconfig';
import { GenericType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { AssociateSkillSearch } from 'src/app/modules/project-life-cycle/models/associateSkillSearch.model';
import { SkillsData } from 'src/app/modules/master-layout/models/project-role.model';
import { AssociateAllocation } from 'src/app/modules/master-layout/models/associateallocation.model';
import { TagAssociateList } from 'src/app/modules/project-life-cycle/models/tag-associate.model';
import { AssociateskillsearchService } from 'src/app/modules/project-life-cycle/services/associateskillsearch.service';
import { TagAssociateService } from 'src/app/modules/project-life-cycle/services/tag-associate.service';
import { SkillsService } from 'src/app/modules/admin/services/skills.service';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs-compat/operator/map';
import { startWith } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { DailogTagListComponent } from '../dailog-tag-list/dailog-tag-list.component';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { SkillDataListComponent } from '../skill-data-list/skill-data-list.component';
import { MatSort } from '@angular/material/sort';
import { MatTableExporterModule } from 'mat-table-exporter';
import { TableUtil } from "../../../table.util";
import { ProjectHistoryListComponent } from '../project-history-list/project-history-list.component';


export interface SkillAssociate {
  label: string;
  value: number;
}

@Component({
  selector: 'app-associateskillsearch',
  templateUrl: './associateskillsearch.component.html',
  styleUrls: ['./associateskillsearch.component.scss']
})
export class AssociateskillsearchComponent implements OnInit {
    themeConfigInput = themeconfig.formfieldappearances;
  
    PageSize: number;
    PageDropDown: number[] = [];
    resources = servicePath.API.PagingConfigValue;
    skillData: GenericType;
    filteredSkills: GenericType[];
    resultData: GenericType[];
    options: SkillAssociate[];
    associateSkillSearchData: AssociateSkillSearch[];
    associateSearchData: AssociateSkillSearch[];
    associateSkillSearchObject = new AssociateSkillSearch();
    selectedSkillIds: any = [];
    skillsearchForm: FormGroup;
    formSubmitted: boolean;

    selectedAssociateSkillSearch: AssociateSkillSearch[];
    isPrimary: boolean = false;
    isSecondary: boolean = false;
    isBillable = false;
    isNonBillable = false;
    isCritical = false;
    isNonCritical = false;
    recordsPerPage = 5;
    first: number = 0;
    gridMessage: string = "No records found";
    totalRecords: number = 0;
    displaySkills: boolean;
    skillsData: SkillsData[] = [];
    filteredOptions: Observable<any>;
    Ids = [];
    step=0;
    // can't send listing need Id'sfonSkillSelected
    isLoading: boolean = false;

    EmployeeId: number;
    displayProjectDetails: boolean = false;
    employeeProjectData: AssociateAllocation[];
    headerOfProjectDetails: string = "";
    skillDetailsHeader: string = "";
    isDisplay = false;
    isSearch = false;
    selectedChangeSkill: any;
    skillList: GenericType[];
    isTagButton = true;
  
    dataSource : MatTableDataSource<AssociateSkillSearch>;

    @ViewChild('table') table: MatTable<any>;

    selection = new SelectionModel<AssociateSkillSearch>(true, []);
    @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
    // @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
    // @ViewChildren(MatSort) sort = new QueryList<MatSort>();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  

 
    displayedColumns: string[] = [ 'EmployeeName', 'Experience', 'PrimarySkill', 'SecondarySkill',
    'Allocationpercentage', 'IsBillable', 'IsCritical', 'ManagerName', 'LeadName'
  ,'Designation', 'Grade', 'ProjectDetails'];

  displayedColumnsForExcel: string[] = [ 'EmployeeName', 'Experience', 'PrimarySkill', 'SecondarySkill',
    'Allocationpercentage', 'IsBillable', 'IsCritical', 'ManagerName', 'LeadName'
  ,'Designation', 'Grade'];

  
    //Tag list fields
    tagListDetails: TagAssociateList[] = [];
    selectedTagListData: TagAssociateList = new TagAssociateList();
    tagListNames = [];
    tagListData: TagAssociateList = new TagAssociateList();
    displayTagList: boolean = false;
    loginUserId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    isNewTagList: string = "0";
    selectedrows: any = [];
    isLoad: boolean = false;

    // @ViewChild("dt1") table: any;
    fileNameDialogRef: MatDialogRef<DailogTagListComponent>;
    constructor(
      private associateSkillSearchService: AssociateskillsearchService,
  
      public navService: NavService,
      private snackBar: MatSnackBar,
      private dialog: MatDialog,
      private skillservice: SkillsService,
      private tagAssociateService: TagAssociateService,
      private route: Router, private fb: FormBuilder,
    ) {
         
      this.navService.currentSearchBoxData.subscribe((responseData) => {
        this.applyFilter(responseData);
      });
  
    }
  
  
  
    ngOnInit() {
      // this.navService.currentSearchBoxData.subscribe((responseData) => {
      //   this.applyFilter(responseData);
      // });
      this.skillsearchForm = this.fb.group({
        'selectedSkillIds': ['',[ Validators.required]],
        'IsPrimary': [false],
        'IsSecondary': [false],
        'IsBillable': [false],
        'IsnonBillable': [false],
        'IsCritical': [false],
        'IsnonCritical':[false],
  
  
      });
  
  
      this.associateSkillSearchService.GetActiveSkillsForDropdown().subscribe(res => {
        res;
        this.skillList = res;
  
      })
      this.associateSkillSearchObject.selectedSkillIds = [];
      // this.getTagListNames();
  
    }


    exporter() {
      TableUtil.exportTableToExcel("ExampleMaterialTable");
    }
    ngAfterViewInit() {   
      // this.dataSource.sort = this.sort.toArray()[0];   
    }
  
      applyFilter(event: Event) {
        if (event) {
          const filterValue = (event.target as HTMLInputElement).value;
          if (filterValue){
            this.dataSource.filter = filterValue.trim().toLowerCase();
            }
            else{
            this.dataSource = new MatTableDataSource(this.associateSkillSearchData);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            }
        } else {
          this.dataSource = new MatTableDataSource(this.associateSkillSearchData);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          }
      }

   
  
  
    onRowChange(event: any, element: any) {
      if (event.checked == true && element) {
        this.selectedAssociateSkillSearch.push(element);
        if (this.selectedAssociateSkillSearch.length > 0) {
          this.isTagButton = false;
        }
      }
      else {
        if (event.checked == false && element) {
          let index = this.selectedAssociateSkillSearch.findIndex(ele => ele.EmployeeId == element.EmployeeId);
          this.selectedAssociateSkillSearch.splice(index, 1);
          if (this.selectedAssociateSkillSearch.length == 0) {
            this.isTagButton = true;
          }
        }
      }
    }
    openDialog(): void {
      this.saveToTagList();
      let dialogRef;
      if(this.tagListData){
        dialogRef = this.dialog.open(DailogTagListComponent, {
  
          width: '800px', height: '600',
          data: {
            selectedAssociateSkillSearch: this.selectedAssociateSkillSearch,
          }
    
        });
      }
  
      
      dialogRef.afterClosed().subscribe(result => {
        console.log(result);
  
      });
    }
  
     
    
    canDisableCheckBox(): boolean {
      if (this.associateSkillSearchObject.selectedSkillIds.length === 0) {
        // this.skillsearchForm.value.IsPrimary = false;
        // this.skillsearchForm.value.IsSecondary = false;
        return false;
      }
      else
        return true;
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
      if (type == 'boolean') {
        return { 'text-align': 'center' };
      }
    }
  
  
    GetProjectDetailsOfEmployee(employeeData) {
      this.displayProjectDetails = true;
      this.associateSkillSearchService.GetProjectDetailByEmployeeId(employeeData.EmployeeId).subscribe((res: AssociateAllocation[]) => {
        this.employeeProjectData = res;
        this.headerOfProjectDetails = "Project details of " + this.employeeProjectData[0].AssociateName;
      });
    }
  
  
    // html option.value
    search($event) {
      const id = this.skillList.find(_ => _.Name === $event);
      // this.skillsearchForm.value.selectedSkillIds += id;
    }
  
    onSkillSelected(item: any) {
      this.selectedSkillIds = [];
      this.selectedSkillIds.push(item);
      this.isSearch = true;
    }
    GetActiveSkillsForDropdown(event: any): void {
      if (event) {
         let searchString = event.target.value.toLowerCase();
        //let searchString = event.target;
  
        if (searchString === '') {
          this.filteredSkills = this.skillList;
        }
        //this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
        this.filteredSkills = [];
  
        // this.filteredSkills = this.skillList.filter(_ => _.Name.toLowerCase().includes(searchString.toLowerCase()))
        this.skillList.forEach((v) => {
          // if (this.filteredSkills.findIndex(x => x.Name == v.Name) === -1 && v.Name.toLowerCase().indexOf(searchString) > -1) {
          //   this.filteredSkills.push({ Id: v.Id, Name: v.Name });
          // }
            if (this.filteredSkills.findIndex(x => x.Name == v.Name) === -1 && v.Name.toLowerCase().indexOf(searchString) > -1) {
            this.filteredSkills.push({ Id: v.Id, Name: v.Name });
          }
        });
      }
      else {
        // this.filteredSkills=[];
        //  this.pushFilteredManagersIds();
      }
    }
    cliearField(fieldName) {
      if (fieldName == 'selectedSkillIds') {
        this.skillsearchForm.controls.selectedSkillIds.setValue('');
        this.filteredSkills = [];
      }
  
    }


    searchData() {
      this.isDisplay = true;
      this.paginator.firstPage();
   
   
      this.associateSkillSearchData = [];
      this.selectedAssociateSkillSearch = [];
      this.totalRecords = 0;
      this.first = 0;
  
  
      let search_skill_object: AssociateSkillSearch = new AssociateSkillSearch();
      search_skill_object.selectedSkillIds = this.skillsearchForm.value.selectedSkillIds;
      this.isDisplay = true;
  
      //need to change in API start
      search_skill_object.IsPrimary = (this.skillsearchForm.value.IsPrimary === null ? false : this.skillsearchForm.value.IsPrimary) ;
      search_skill_object.IsSecondary = (this.skillsearchForm.value.IsSecondary === null ? false : this.skillsearchForm.value.IsSecondary) ;
      search_skill_object.IsBillable = (this.skillsearchForm.value.IsBillable === null ? false : this.skillsearchForm.value.IsBillable) ;
      search_skill_object.IsnonBillable = (this.skillsearchForm.value.IsnonBillable === null ? false : this.skillsearchForm.value.IsnonBillable) ;
      search_skill_object.IsCritical = (this.skillsearchForm.value.IsCritical === null ? false : this.skillsearchForm.value.IsCritical) ;
      search_skill_object.IsnonCritical = (this.skillsearchForm.value.IsnonCritical === null ? false : this.skillsearchForm.value.IsnonCritical) ;
      search_skill_object.SkillIds = this.skillsearchForm.value.selectedSkillIds.Id
      // if (this.selectedSkillIds != null && this.selectedSkillIds.length > 0) {
      //   search_skill_object.SkillIds = this.selectedSkillIds.map(p => p.Id).join(",");
      
      // }
      // else {
      //   search_skill_object.SkillIds = null;
       
      //    return false;
        

       
      // }  
  
      this.isLoad = true;
      this.associateSkillSearchService.GetEmployeeDetailsBySkill(search_skill_object).subscribe(
        (res: AssociateSkillSearch[]) => {
          this.isLoad = false;
          this.associateSkillSearchData = res;
          
          if (!this.associateSkillSearchData || this.associateSkillSearchData.length <= 0) {

            this.snackBar.open('No records found', '', {
              duration: 1000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            // this.messageService.add({ severity: 'info', summary: 'Information Message', detail: 'No records found' })
            this.gridMessage = "No records found";
            this.clear();
          }
          else
            this.gridMessage = "";
          this.totalRecords = this.associateSkillSearchData.length;
          this.dataSource = new MatTableDataSource<AssociateSkillSearch>(this.associateSkillSearchData);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        },
        error => {
          this.snackBar.open('Unable to get Employees', '', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() =>
            this.formGroupDirective.resetForm(), 0)
          // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get Employees' })
        }
  
      )
      // this.skilldata.paginator = this.paginator;
  
    }
  
    getRows(event): void {
      this.recordsPerPage = event.rows;
    }

    displayFn(skill: any) {
      return skill && skill ? skill.Name : '';
    }
  
    clear() {
      this.isLoad = false;
      this.isSearch = false;
      this.skillsearchForm.reset();
      this.associateSkillSearchObject = new AssociateSkillSearch();
      this.dataSource =  new MatTableDataSource([]);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      // this.dataSource.paginator = this.paginator;
      // this.dataSource.sort = this.sort;
      
  
      this.selectedAssociateSkillSearch = [];
      this.associateSkillSearchObject.selectedSkillIds = [];
      this.associateSkillSearchData = [];
      this.totalRecords = 0;
      this.gridMessage = "No records found";
      this.isDisplay = false;
      // this.search(event);
      this.filteredSkills = [];
    }
  
  
  
  
  
    //Tag list
    addToTagList(): void {
      //check the grid selection and open the popup.
      this.selectedTagListData = new TagAssociateList();
      if (this.selectedAssociateSkillSearch && this.selectedAssociateSkillSearch.length > 0) {
  
        this.isTagButton = false;
        this.displayTagList = true;
        this.selectedTagListData = new TagAssociateList();
      }
      else {
        this.snackBar.open('Select assoiciates to tag', '', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        setTimeout(() =>
          this.formGroupDirective.resetForm(), 0)
        // this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Select assoiciates to tag' });
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
            // this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Select or enter Tagged listname' });
            return;
          }
          this.tagListDetails.push(this.tagListData);
        });
  
        //update
        if (this.selectedTagListData.TagListId > 0) {
          this.tagAssociateService.UpdateTagList(this.tagListDetails).subscribe(((res: number) => {
            if (res == 1) {
              this.cleartheTagListData();
              // this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Tag list updated successfully.' })
              this.searchData();
              this.getTagListNames();
            }
            else if (res == -1) {
              // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Associates already assigned to the selected taglist' })
            }
          }),
            error => {
              // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to save the Tag list' })
            });
        }
  
        //create
        else if (this.selectedTagListData.TagListName) {
          this.tagAssociateService.CreateTagList(this.tagListDetails).subscribe(((res: number) => {
            if (res == 1) {
              this.cleartheTagListData();
              // this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Tag list created successfully.' })
              this.searchData();
              this.getTagListNames();
            }
            else if (res == -1)
              console.log('Tag list name already exists')
            // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Tag list name already exists for respective Reporting manager/delivery head.' })
          }),
            error => {
              // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to save the Tag list' })
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
          // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get Tag list names' })
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
    selectedChangeIds(frmCntrl, item) {
      if (frmCntrl == 'ddlProgramManager') {
        this.selectedChangeSkill = item;
      }
  
    }
    onChange(id: number, isChecked: boolean) {
      if (isChecked) {
        this.Ids.push(id);
      } else {
        const index = this.Ids.indexOf(id);
        if (index > -1) {
          this.Ids.splice(index, 1);
        }
      }
  
    }
    openSkillDialog(empID, skillType): void {
      this.ViewAllSkills(empID, skillType);
      // this.dataSource.paginator = this.paginator;
			// this.dataSource.sort = this.sort;
      
    }
  
  
    ViewAllSkills(empID, skillType: string): void {
      let index_1 = 0, isPrimary = false;
      this.displaySkills = true;
      if (skillType == 'PrimarySkill') {
        isPrimary = true;
      }
      this.skillsData = [];
      this.skillservice.GetAllSkillDetails(empID).subscribe((res: SkillsData[]) => {
        this.skillDetailsHeader = "Skill details of " + res[0].EmployeeName;
        res.forEach((e) => {
          if (e.IsPrimary == isPrimary)
            this.skillsData[index_1++] = e;
        })
        let dialogRef;
      if(this.displaySkills){
        dialogRef = this.dialog.open(SkillDataListComponent, {
  
          width: '800px', height: '600',
          data: {
            skillsData: this.skillsData,
          }
    
        });
      }
      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });
      });
  
    }

  openProjectHistory(EmpId: number, EmpName: string){
    let projectDetailsDailog = this.dialog.open(ProjectHistoryListComponent, {
  
      width: '800px',
      data: {
        EmployeeId: EmpId,
        EmployeeName: EmpName
      }

    });
  }
  }