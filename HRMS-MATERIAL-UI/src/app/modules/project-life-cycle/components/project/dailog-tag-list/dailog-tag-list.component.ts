import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { AssociateSkillSearch } from 'src/app/modules/project-life-cycle/models/associateSkillSearch.model';
import { AssociateskillsearchService } from 'src/app/modules/project-life-cycle/services/associateskillsearch.service';
// import { BooleanToStringPipe } from '../../pipes/booleantostringpipe';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SkillsService } from 'src/app/modules/admin/services/skills.service';
import { SkillsData } from 'src/app/modules/master-layout/models/project-role.model';
import { TagAssociateList } from 'src/app/modules/project-life-cycle/models/tag-associate.model';
import { TagAssociateService } from 'src/app/modules/project-life-cycle/services/tag-associate.service';
import { themeconfig } from 'src/themeconfig';
import { Validators, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-dailog-tag-list',
  templateUrl: './dailog-tag-list.component.html',
  styleUrls: ['./dailog-tag-list.component.scss']
})
export class DailogTagListComponent implements OnInit {

 
  themeConfigInput = themeconfig.formfieldappearances;

  PageSize: number;
  PageDropDown: number[] = [];
  selectedAssociateSkillSearch: AssociateSkillSearch[] = [];
  private selectedEmpInfo: any;
  displaySkills: boolean;
  loginUserId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
   isNewTagList: string;
align = 'start';
   skillsData: SkillsData[] = [];
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
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(
    public dialogRef: MatDialogRef<DailogTagListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private skillservice: SkillsService,
    private tagAssociateService: TagAssociateService,
    private route: Router, private fb: FormBuilder,
  ) {
this.selectedAssociateSkillSearch =this.data.selectedAssociateSkillSearch;

  }

  ngOnInit(): void {
    this.skillsearchForm = this.fb.group({
      'TagListId': [null, Validators.required],
      'TagListName': [null, Validators.required],

    });
    this.isNewTagList = '0';
    this.getTagListNames();

  }

  viewTagList(): void {
    //navigate to Tag list screen
    this.route.navigate(['/project/tagAssociate']);
  }

  clientbillingRoleSelected(clientbillingroleinfo: any){
    this.selectedEmpInfo = clientbillingroleinfo;

  }
 saveToTagList(): void {
    this.tagListDetails = [];
    if (this.selectedAssociateSkillSearch && this.selectedAssociateSkillSearch.length > 0) {
      this.selectedAssociateSkillSearch.forEach(element => {

        this.tagListData = new TagAssociateList();
        this.tagListData.EmployeeId = element.EmployeeId;
        this.tagListData.ManagerId = this.loginUserId;
        this.tagListData.ProjectId = element.ProjectId;
        if (this.skillsearchForm.value.TagListId > 0) {
          this.tagListData.TagListId = this.skillsearchForm.value.TagListId;
          this.tagListData.TagListName = this.skillsearchForm.value.TagListName;
        }
        else if (this.skillsearchForm.value.TagListName) {
          this.tagListData.TagListName = this.skillsearchForm.value.TagListName;
          this.tagListData.TagListId = 0;
        }
        else {
          this.snackBar.open('Select or enter Tagged listname', '', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() => 
          this.formGroupDirective.resetForm(), 0)
          return;
        }
        this.tagListDetails.push(this.tagListData);
      });

      //update
      if (this.skillsearchForm.value.TagListId > 0) {
        this.tagAssociateService.UpdateTagList(this.tagListDetails).subscribe(((res: number) => {
          if (res == 1) {
            this.cleartheTagListData();
            this.snackBar.open('Tag list updated successfully', '', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() => 
            this.formGroupDirective.resetForm(), 0)
            // this.searchData();
            this.getTagListNames();
          }
          else if (res == -1) {
            this.snackBar.open('Associates already assigned to the selected taglist', '', {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() => 
            this.formGroupDirective.resetForm(), 0)
            // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Associates already assigned to the selected taglist' })
          }
        }),
          error => {
            this.snackBar.open('Unable to save the Tag list', '', {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() => 
            this.formGroupDirective.resetForm(), 0)
            // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to save the Tag list' })
          });
      }

      //create
      else if (this.skillsearchForm.value.TagListName) {
        this.tagAssociateService.CreateTagList(this.tagListDetails).subscribe(((res: number) => {
          if (res == 1) {
            this.cleartheTagListData();
            this.snackBar.open('Tag list created successfully.', '', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() => 
            this.formGroupDirective.resetForm(), 0)
            // this.searchData();
            this.getTagListNames();
          }
          else if (res == -1)
          this.snackBar.open('Tag list name already exists', '', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() => 
          this.formGroupDirective.resetForm(), 0)
            // this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Tag list name already exists for respective Reporting manager/delivery head.' })
        }),
          error => {
            this.snackBar.open('Unable to save the Tag list', '', {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() => 
            this.formGroupDirective.resetForm(), 0)
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

  onNoClick(): void {
    this.dialogRef.close();
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
        this.snackBar.open('Unable to get Tag list names', '', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        setTimeout(() =>
          this.formGroupDirective.resetForm(), 0)
      });
  }

  cleartheTagListData() {
    this.skillsearchForm.reset();
    this.displayTagList = false;
    // this.selectedTagListData = new TagAssociateList();
    this.tagListDetails = [];
    this.tagListData = new TagAssociateList();
  }

  onRadioClick(): void {
    if (this.isNewTagList == "1") {
      this.skillsearchForm.controls['TagListName'].setValue("");
    }
    else {
      this.skillsearchForm.controls['TagListId'].setValue(0);
      this.skillsearchForm.controls['TagListName'].setValue("");
    }
  }
}

