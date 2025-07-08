import { Component, OnInit, ViewChild, Inject } from '@angular/core';
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
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs-compat/operator/map';
import { startWith } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, FormControl } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { DailogTagListComponent } from '../dailog-tag-list/dailog-tag-list.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

@Component({
  selector: 'app-skill-data-list',
  templateUrl: './skill-data-list.component.html',
  styleUrls: ['./skill-data-list.component.scss']
})
export class SkillDataListComponent implements OnInit {
  PageSize: number;
  PageDropDown: number[] = [];
  // filteredSkills: GenericType[] = [];
  // associateSkillSearchData: AssociateSkillSearch[] = [];
  // associateSkillSearchObject: AssociateSkillSearch = new AssociateSkillSearch();
  selectedAssociateSkillSearch: AssociateSkillSearch[] = [];
  private selectedEmpInfo: any;

  displaySkills: boolean;
  loginUserId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
  // isNewTagList = 0;
   isNewTagList: string;
align = 'start';
  // isNewTagList:  number ;
  skillsData: SkillsData[];
  EmployeeId: number;
  displayProjectDetails: boolean = false;
  displayTagList: boolean = false;
  tagListNames = [];
  tagListDetails: TagAssociateList[] = [];
  tagListData: TagAssociateList = new TagAssociateList();
  selectedEmployeeId: number;
  skillsearchForm: FormGroup;
  headerOfProjectDetails: string = "";
  skillDetailsHeader: string = "";
  isDisplay: boolean = false;
  resources = servicePath.API.PagingConfigValue;



  dataSource: MatTableDataSource<SkillsData[]>;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;


  displayedColumns: string[] = ['SkillName','ProficiencyLevelCode',
  'SkillExperience','LastUsed'];


  constructor(
    public dialogRef: MatDialogRef<SkillDataListComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{skillsData:SkillsData[]},
    private skillservice: SkillsService,

  ) {



    this.skillsData = data.skillsData;

   }

  ngOnInit(): void {
  }
 
  onNoClick(): void {
    this.dialogRef.close();
  }

}