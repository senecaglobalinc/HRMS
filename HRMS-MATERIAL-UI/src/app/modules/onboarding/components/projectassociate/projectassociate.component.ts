import { Component, OnInit, Inject, ViewChild, Injector } from '@angular/core';

import { Projects } from '../../models/assosiateproject.model';
import { Associate } from '../../models/associate.model';
import { HttpClient } from '@angular/common/http';
// import { Http } from "@angular/http";
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectassosiateService } from '../../services/projectassosiate.service';
// import { DataStore } from "../../../shared/datastore";
import * as moment from 'moment';
import { CommonService } from '../../../../core/services/common.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from '../../../../../themeconfig';
import { FormGroup, FormControl, Validators, FormArray, FormGroupDirective } from '@angular/forms';
import { MatTable } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { I } from '@angular/cdk/keycodes';
import { NgxSpinnerService } from 'ngx-spinner';


interface SelectItem {
  value: number;
  label: string;
}


@Component({
  selector: 'app-projectassociate',
  templateUrl: './projectassociate.component.html',
  styleUrls: ['./projectassociate.component.scss'],
  providers: [ProjectassosiateService, CommonService, MasterDataService]
})
export class ProjectassociateComponent implements OnInit {
  newCreatedForm: FormGroup;
  id: number;
  projects: Array<Projects>;
  _Associate = new Associate();
  currentempID: number;
  // @ViewChild("messageToaster") messageToaster: any;
  _resources: any;
  _dataService: Array<Projects>;
  private _serverURL: string;
  roles: any[] = [];
  submitted = false;
  type: string = "new";
  index: number;
  buttonType: string;
  domainList: SelectItem[] = [];
  filterDuplicates: boolean = false;
  themeConfigInput = themeconfig.formfieldappearances;
  addproject: FormGroup;
  projectarray: FormArray;
  dialogResponse: boolean;
  savedmsg: string
  displayedColumns: string[] = ['Id', 'DomainId', 'OrganizationName', 'ProjectName', 'Duration', 'KeyAchievements', 'Actions', 'Action'];
  //dataSource =  USER_INFO;
  dataSource;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  iter = 0;
  @ViewChild("projectsForm") projectsForm: any;
  //@ViewChild("projectDialog") projectDialog: any;
  @ViewChild(MatTable) table: MatTable<any>;

  constructor(public dialog: MatDialog,
    // @Inject(Http) private _http: Http,
    // private _injector: Injector = AppInjector(),
    @Inject(ProjectassosiateService) private _service: ProjectassosiateService,
    private _commonService: CommonService,
    private _snackBar: MatSnackBar,

    private masterDataService: MasterDataService,
    @Inject(Router) private _router: Router,
    private actRoute: ActivatedRoute,
    private spinner: NgxSpinnerService

  ) {
    this.projects = new Array<Projects>();

    this.dataSource = this.projects;

  }

  ngOnInit() {
    this.spinner.show()
    this.actRoute.params.subscribe(params => {
      this.id = params.id;
    });
    // this.id = 218;
    this.currentempID = this.id;
    this.masterDataService.GetDomains().subscribe((res: any[]) => {
      res.forEach((element: any) => {
        this.domainList.push({
          label: element.DomainName,
          value: element.DomainID
        });
      }

      );
      this._service.GetAssociateProjects(this.id).subscribe((res: any) => {
        this.spinner.hide()
        this.projects = res;
        if (this.projects.length !== 0) {
          this.type = 'edit';
        }
        if (this.projects.length === 0) {
          this.type = 'new';
        }
        if (this.projects.length == 0) {
          this.projects.push({
            Id: 0,
            Duration: null,
            RoleMasterId: '',
            OrganizationName: '',
            ProjectName: '',
            RoleName: '',
            KeyAchievements: '',
            DomainId: null
          });
        }
        this.createInitialform();
        this.createFormGroup();
        this.dataSource = this.projects;
      },(error)=>{
        this.spinner.hide()
      });
    },(error)=>{
      this.spinner.hide()
    });
    this.createInitialform();

  }
  createInitialform() {
    this.addproject = new FormGroup({
      projectarray: new FormArray([])
    });
  }

  createFormGroup() {
    this.projectarray = new FormArray([]);
    this.projects.forEach(element => {
      this.projectarray = this.addproject.get('projectarray') as FormArray;
      this.projectarray.push(this.BuildForm(element));
    });
  }

  BuildForm(element): any {
    // tslint:disable-next-line:max-line-length
    element = element || { Id: 0, Duration: null, RoleMasterId: '', OrganizationName: '', ProjectName: '', RoleName: '', KeyAchievements: '', DomainId: null };
    return new FormGroup({
      DomainID: new FormControl(element.DomainId, [Validators.required]),
      OrganizationName: new FormControl(element.OrganizationName, [Validators.required, Validators.maxLength(100)]),
      ProjectName: new FormControl(element.ProjectName, [Validators.required, Validators.maxLength(25), Validators.pattern('[a-zA-Z0-9 ]+')]),
      duration: new FormControl(element.Duration, [Validators.required, Validators.maxLength(3), Validators.pattern('^[1-9][0-9]*$')]),
      keyAchievement: new FormControl(element.KeyAchievements, [Validators.required, Validators.pattern('[a-zA-Z ]+')]),

    });
  }

  get testArray(): FormArray {
    return this.addproject.get('projectarray') as FormArray;
  }

  setValueDomainID(i: any) {
    this.addproject.get('DomainID').patchValue(this.projects[i].DomainId);

  }
  setValueOrganizationName(i: any) {
    this.addproject.get('OrganizationName').patchValue(this.projects[i].OrganizationName);
  }
  setValueProjectName(i: any) {
    this.addproject.get('ProjectName').patchValue(this.projects[i].ProjectName);
  }
  setValueduration(i: any) {
    this.addproject.get('duration').patchValue(this.projects[i].Duration);
  }
  setValuekeyAchievement(i: any) {
    this.addproject.get('keyAchievement').patchValue(this.projects[i].KeyAchievements);
  }


  getDomainList() {

    this.masterDataService.GetDomains().subscribe((res: any[]) => {
      res.forEach((element: any) => {
        this.domainList.push({
          label: element.DomainName,
          value: element.DomainID
        });
      }

      );
    });
    if (this.projects.length !== 0) {
      this.type = 'edit';
    } else {
      this.type = 'new';
    }
  }

  filterProjectDuplicates(project: Projects): boolean {
    if (
      this.projects.length > 0 &&
      project &&
      project.DomainId != null &&
      project.OrganizationName !== '' &&
      project.ProjectName !== ''
    ) {
      project.OrganizationName = project.OrganizationName.trim();
      project.ProjectName = project.ProjectName.trim();
      let projectList: Projects[] = this.projects.filter((projectOfList: Projects) => {
        return (
          projectOfList.DomainId == project.DomainId &&
          projectOfList.OrganizationName.toLowerCase() === project.OrganizationName.toLowerCase() &&
          projectOfList.ProjectName.toLowerCase() === project.ProjectName.toLowerCase()
        );
      })
      if (projectList.length > 1) {
        return true;
      }
      else {
        return false;
      }
    }
  }

  OnSubmit(project: Projects[]) {
    if (project.length > 0) {
      this.filterDuplicates = this.filterProjectDuplicates(
        project[project.length - 1]
      );
    }
    // if (project.length >= 1) {
    //   this.type='edit'
    // }
    if (this.type === 'new') {
      this.buttonType = 'Save';
    }
    else {
      this.type = 'edit'
      this.buttonType = 'Update';
    }

    if (this.filterDuplicates === true) {
      alert('warn check dup project details');
      // this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please check Duplicates projects details' });
      // swal("Please check Duplicates", "", "error");
      return false;
    }

    if (this.buttonType === "Save" || this.buttonType === "Update") {
      this.onSaveorUpdate(this.projects);
      return true;
    }

  }

  OnUpdate() {
    this.buttonType = "Update";
  }

  OnSave() {
    this.buttonType = "Save";
  }

  setNewProjectButtonType() {
    this.buttonType = "AddNewProject";
  }

  onNewProject() {
    if (this.newCreatedForm != undefined) {
      this.newCreatedForm.markAllAsTouched();
      this.setProjectRequiredValidations('DomainID', this.newCreatedForm);
      this.setProjectRequiredValidations('OrganizationName', this.newCreatedForm);
      this.setProjectRequiredValidations('ProjectName', this.newCreatedForm);
      this.setProjectRequiredValidations('duration', this.newCreatedForm);
      this.setProjectRequiredValidations('keyAchievement', this.newCreatedForm);
    }
    if (this.addproject.controls.projectarray.valid) {
      this.projects.push({
        Id: 0,
        Duration: null,
        RoleMasterId: "",
        OrganizationName: "",
        ProjectName: "",
        RoleName: "",
        KeyAchievements: "",
        DomainId: null
      });
      this.projectarray = this.addproject.get('projectarray') as FormArray;
      this.projectarray.push(this.newCreatedForm = this.BuildForm({
        Id: 0,
        Duration: null,
        RoleMasterId: "",
        OrganizationName: "",
        ProjectName: "",
        RoleName: "",
        KeyAchievements: "",
        DomainId: null
      }));
      this.dynamicProjectValidations('DomainID', this.newCreatedForm);
      this.dynamicProjectValidations('OrganizationName', this.newCreatedForm);
      this.dynamicProjectValidations('ProjectName', this.newCreatedForm);
      this.dynamicProjectValidations('duration', this.newCreatedForm);
      this.dynamicProjectValidations('keyAchievement', this.newCreatedForm);
      this.dataSource = this.projects;
      this.table.renderRows();
      return false;
    }
    else {
      return false;
    }
  }

  dynamicProjectValidations(projControl: string, newFormProjValidate: FormGroup) {
    newFormProjValidate.controls[projControl].setErrors(null);
    newFormProjValidate.controls[projControl].clearValidators();
    newFormProjValidate.controls[projControl].updateValueAndValidity();
  }

  setProjectRequiredValidations(projFormControl: string, newProjDetails: FormGroup) {
    newProjDetails.controls[projFormControl].setValidators([Validators.required])
    newProjDetails.controls[projFormControl].updateValueAndValidity();
    if (projFormControl === 'DomainID') {
      newProjDetails.controls[projFormControl].setValidators([Validators.required])
      newProjDetails.controls[projFormControl].updateValueAndValidity();
    }
    else if (projFormControl === 'OrganizationName') {
      newProjDetails.controls[projFormControl].setValidators([Validators.required, Validators.maxLength(100)])
      newProjDetails.controls[projFormControl].updateValueAndValidity();
    }
    else if (projFormControl === 'ProjectName') {
      newProjDetails.controls[projFormControl].setValidators([Validators.required, Validators.maxLength(25), Validators.pattern('[a-zA-Z0-9 ]+')])
      newProjDetails.controls[projFormControl].updateValueAndValidity();
    }
    else if (projFormControl === 'duration') {
      newProjDetails.controls[projFormControl].setValidators([Validators.required, Validators.maxLength(3), Validators.pattern('^[1-9][0-9]*$')])
      newProjDetails.controls[projFormControl].updateValueAndValidity();
    }
    else if (projFormControl === 'keyAchievement') {
      newProjDetails.controls[projFormControl].setValidators([Validators.required, Validators.pattern('[a-zA-Z ]+')])
      newProjDetails.controls[projFormControl].updateValueAndValidity();
    }
  }

  onSaveorUpdate(qual: Array<Projects>) {
    if (this.type === 'new') {
      this.buttonType = 'Save';
    }
    else {
      this.buttonType = 'Update';
    }
    if (this.newCreatedForm !== undefined) {
      this.setProjectRequiredValidations('DomainID', this.newCreatedForm);
      this.setProjectRequiredValidations('OrganizationName', this.newCreatedForm);
      this.setProjectRequiredValidations('ProjectName', this.newCreatedForm);
      this.setProjectRequiredValidations('duration', this.newCreatedForm);
      this.setProjectRequiredValidations('keyAchievement', this.newCreatedForm);
    }
    for (var i = 0; i < qual.length; i++) {
      if (
        !qual[i].ProjectName ||
        !qual[i].OrganizationName ||
        !qual[i].Duration ||
        qual[i].Duration == 0 ||
        qual[i].Duration.toString().charAt(0) == '0' ||
        !qual[i].KeyAchievements || qual[i].KeyAchievements.trim().length == 0 ||
        !qual[i].DomainId
      ) {
        // this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please provide complete details' });
        // swal("Please provide complete details", "", "error");
        return false;
      }
    }
    if (this.filterDuplicates === true) {
      this._snackBar.open('Please check Duplicates project details', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      // this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please check Duplicates projects details' });
      // swal("Please check Duplicates", "", "error");
      return false;
    }
    this._Associate.EmpId = this.currentempID;
    this._Associate.Projects = [];
    for (var i = 0; i < qual.length; i++) {
      this._Associate.Projects.push(qual[i]);
    }

    this._service.SaveProjectDetails(this._Associate).subscribe(
      data => {
        if (this.type == "new")
          this.savedmsg = "Project details saved successfully."
        else
          this.savedmsg = "Project details updated successfully."

        this._snackBar.open(this.savedmsg, 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.getDomainList();

      },
      error => {
        this._snackBar.open('Failed to save projects details', 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

      }
    );

    // return false;
  }

  Delete(index: number) {
    this.index = index;
    this.openDialog('Confirmation', 'Do you want to Delete ?', index);
  }


  openDialog(Heading, Message, index): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: '300px',
      data: { heading: Heading, message: Message }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        this.projects.splice(this.index, 1);
        this.dataSource = this.projects;
        this.projectarray.removeAt(this.index);
        this.table.renderRows();
        const dialogConf = this.dialog.open(MessageDialogComponent, {
          disableClose: true,
          hasBackdrop: true,
          width: '300px',
          data: { heading: 'Confirmation', message: 'Project details deleted successfully' }
        })
      }
    });
  }


  onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
  }

  onRoleChage(event: any, selectedProject: any) {
    let roleId = event.target.value;
    let count = this.validate(selectedProject.projectName, roleId);
    if (count == 1) {
      event.target.value = "";
      this._snackBar.open('Duplicate projects are not allowed', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      // this.messageService.add({ severity: 'warn', summary: 'warning Message', detail: 'Duplicate project are not allowed' });
      // swal("", "Duplicate project are not allowed", "warning");
    }
  }
  onProjectChange(event: any, selectedProject: any) {
    let count = this.validate(
      selectedProject.projectName,
      selectedProject.roleID
    );
    if (count == 2) {
      selectedProject.projectName = "";
      this._snackBar.open('Duplicate projetcs are not allowed', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      // this.messageService.add({ severity: 'warn', summary: 'warning Message', detail: 'Duplicate project are not allowed' });
      // swal("", "Duplicate project are not allowed", "warning");
    }
  }
  validate(projectName: any, roleId: any): number {
    let count = 0;

    this.projects.forEach((p: any) => {
      if (
        p.projectName.toUpperCase() === projectName.toUpperCase() &&
        p.roleID === roleId
      ) {
        count++;
      }
    });
    return count;
  }

  omit_special_char(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k === 8 ||
      k === 32 ||
      k === 46 ||
      k === 44 ||
      (k >= 48 && k <= 57)
    );
  }

}

