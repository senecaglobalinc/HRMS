import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, ValidationErrors, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { SOW } from 'src/app/modules/admin/models/sow.model';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { themeconfig } from 'src/themeconfig';
import { ProjectCreationService } from '../../services/project-creation.service';
import { SowService } from '../../services/sow.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';
import { ProjectCreationComponent } from '../project-creation/project-creation.component'

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-sow',
  templateUrl: './sow.component.html',
  styleUrls: ['./sow.component.scss']
})
export class SowComponent implements OnInit {

  editMode = false;
  sowSignedDate: any;
  startSOWSignedDate:any;
  sowlist: SelectItem[] = [];
  sowSignedDates: any[] = []
  projectsList: SelectItem[];
  SOWData: SOW[] = [];
  isSow = 1;
  state: string = "";
  disableAddendum: boolean = false;
  currentRole = "";
  projectId: any;
  projectIdSubscription: Subscription;
  subscriptionProjectState: Subscription;
  formSubmitted = false;
  AddendumformSubmitted = false;
  AddendumData: any[] = [];

  btnLabel = "Add";
  CancelBtnLabelSOW = "Clear";
  CancelBtnLabelAddendum = "Clear";
  addButton = "Add";

  themeConfigInput = themeconfig.formfieldappearances;

  addSOW: FormGroup;
  addAddendum: FormGroup;

  displayedColumnsSOW: string[] = ['SOWId', 'SOWFileName', 'SOWSignedDate', 'Edit', 'Delete'];
  displayedColumnsAddendum: string[] = ['AddendumNo', 'AddendumDate', 'RecipientName', 'Note', 'Edit'];

  dataSourceSOW: MatTableDataSource<SOW>;
  dataSourceAddendum: MatTableDataSource<any>;

  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  startdate: any;



  constructor(private _sowService: SowService,
    private _commonService: CommonService,
    private _projectCreationService: ProjectCreationService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    public dialog: MatDialog,) {

    this.navService.changeSearchBoxData('');

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });

  }
  
  ngOnInit(): void {
    
    this.currentRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.CreateSowForm();
    this.CreateAddendumForm();
    this.projectIdSubscription = this._projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
      if (this.projectId > 0) {
        this.GetSowByProjectId(this.projectId);
        this.GetProjectsList();
      }
    });
    this.btnLabel = "Add";
    this.GetProjectsList();
    this.disableAddendumOption();
  }

  CreateSowForm() {
    //to initialize SOW form
    this.addSOW = new FormGroup({
      SOW: new FormControl(true),
      ProjectId: new FormControl(null, [Validators.required]),
      SOWId: new FormControl(null, [Validators.required,
      Validators.maxLength(50)]),
      SOWSignedDate: new FormControl(null, [Validators.required, this.ValidationDateSOW.bind(this)]),
      SOWFileName: new FormControl(null, [Validators.required,
      Validators.maxLength(50)]),
      RoleName: new FormControl(null),
      Id: new FormControl(null)
    });
    if (this.projectId > 0) {
      this.addSOW.patchValue({
        ProjectId: this.projectId
      });
    }
    if (this.currentRole) {
      this.addSOW.patchValue({
        RoleName: this.currentRole
      });
    }
  }

  CreateAddendumForm() {
    //to initialize addendum form
    this.addAddendum = new FormGroup({
      Addendum: new FormControl(true),
      ProjectId: new FormControl(null),
      SOWId: new FormControl(null),
      AddendumNo: new FormControl(null, [
        Validators.required,
        //Validators.pattern("^[0-9]*$")
      ]),
      RecipientName: new FormControl(null, [
        Validators.required,
        Validators.pattern("[a-zA-Z ]*$")
      ]),
      Note: new FormControl(null, [Validators.required],),
      AddendumDate: new FormControl(null, [Validators.required]),
      Id: new FormControl(null, [Validators.required]),
      RoleName: new FormControl(null),
      AddendumId: new FormControl(null)
    });
    if (this.projectId > 0) {
      this.addAddendum.patchValue({
        ProjectId: this.projectId
      });
    }
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='AddendumDate'){
      evt.stopPropagation();
      this.addAddendum.get('AddendumDate').reset();
    }
    if(fieldName=='SOWSignedDate'){
      evt.stopPropagation();
      this.addSOW.get('SOWSignedDate').reset();
    }
  }

  GetProjectsList(): void {
    //to get the list of projects
    this.projectsList = [];
    this._projectCreationService.GetProjectDetailsbyID(this.projectId).subscribe(
      (res: ProjectsData) => {
        this.startdate = res.ActualStartDate;
        this.projectsList.push({
          label: res.ProjectName,
          value: res.ProjectId
        });

        if (this.projectId > 0) {
          this.addSOW.patchValue({
            ProjectId: this.projectId
          });
          this.addAddendum.patchValue({
            ProjectId: this.projectId
          });
        }
      }
    ),
      error => {
      };
  }

  disableAddendumOption(): void {
    //to disable Addendum radio button
    this.subscriptionProjectState = this._projectCreationService.GetProjectState().subscribe((state: string) => {
      this.state = state;
      if (this.state == "Drafted" || this.state == "SubmittedForApproval")
        this.disableAddendum = true;
      else if (this.state == "Created") {
        if (this.SOWData.length == 0) {
          this.disableAddendum = true;
        }
      }
      else
        this.disableAddendum = false;
    });
  }


  GetSowByProjectId(currentProjectId: number) {
    //to get SOW based on ProjectID
    this.editMode = false;
    if (currentProjectId != null) {
      this.sowlist = [];
      this._sowService.GetSowByProjectId(currentProjectId).subscribe(
        (response: any[]) => {
          this.SOWData = response;
          this.SOWData.forEach((d: any) => {
            this.sowlist.push({ label: d.SOWId, value: d.Id });
            this.sowSignedDates.push({ label: d.Id, value: d.SOWSignedDate });
            d.SOWSignedDate = moment(d.SOWSignedDate).format("YYYY-MM-DD");
          });
          this.ChangeFormat(this.SOWData);
          this.EnableDisableAddendum();
          this.dataSourceSOW = new MatTableDataSource(this.SOWData);
          this.dataSourceSOW.paginator = this.paginator
          this.dataSourceSOW.sort = this.sort;
          this.dataSourceSOW.sortingDataAccessor = (data, header) => data[header].toLowerCase();
        }

      );
    }
  }

  GetSows(event) {
    //to get SOW's by Project Id
    this.GetSowByProjectId(event.value);
  }

  ChangeFormat(SowData): void {
    //to change date format
    let i;
    if (this.isSow == 1)
      for (i = 0; i < SowData.length; i++) {
        if (SowData[i].SOWSignedDate != null)
          SowData[i].SOWSignedDate = moment(SowData[i].SOWSignedDate).format(
            "MM/DD/YYYY"
          );
      }
    else
      for (i = 0; i < SowData.length; i++) {
        if (SowData[i].AddendumDate != null)
          SowData[i].AddendumDate = moment(SowData[i].AddendumDate).format(
            "MM/DD/YYYY"
          );
      }
  }

  EnableDisableAddendum() {
    //Enable Addendum when SOWs are >= 1
    if (this.state == "Created") {
      if (this.SOWData.length == 0)
        this.disableAddendum = true;
      else
        this.disableAddendum = false;
    }
  }

  canEnableSowId() {
    //to make SOWID editable or not
    if ((this.state == "Drafted" || this.state == "SubmittedForApproval") && this.currentRole == 'Department Head')
      return false;
    if ((this.state == "Drafted") && this.currentRole != 'Department Head')
      return false;
    return true;
  }

  ValidationDateSOW(control: FormControl) : ValidationErrors | null{
    const selection: any = control.value;
    if (Date.parse(moment(selection).format("DD-MM-YYYY")) < Date.parse(moment(this.startdate).format("DD-MM-YYYY"))) {
      return { ValidationDateSOW: true };
    }
    return null;
  }

  setSOWDateDynamic(){
    const selection: any = this.addSOW.controls.SOWSignedDate.value;
    if (Date.parse(moment(selection).format("DD-MM-YYYY")) < Date.parse(moment(this.startdate).format("DD-MM-YYYY"))) {
      return true;
    }
    return false;
  }

  AddSOW(): any {
    //invokes when add button is clicked in SOW form
    this.formSubmitted = true;
    this.setSOWRequiredValidations('SOWId', this.addSOW);
    this.setSOWRequiredValidations('SOWSignedDate', this.addSOW);
    this.setSOWRequiredValidations('SOWFileName', this.addSOW);
    if (this.editMode == false) {
      this.addSOW.value.SOWSignedDate = moment(
        this.addSOW.value.SOWSignedDate
      ).format("YYYY-MM-DD");
      if (this.addSOW.valid == true && this.isSow == 1) {
        this.saveSOW();
      }
    }
    else {
      // edit was clicked
      this.addSOW.value.SOWSignedDate = moment(
        this.addSOW.value.SOWSignedDate
      ).format("YYYY-MM-DD");
      if (this.addSOW.valid == true && this.isSow == 1) {
        this.updateSOW();
      }

    }
  }

  saveSOW() {
    //to save SOW
    let proId = this.addSOW.value.ProjectId;

    if (this.addSOW.value.Id == null) {
      this.addSOW.value.Id = 0;
    }
    this._sowService.SaveSOW(this.addSOW.value).subscribe(res => {
      if (res == 1) {
        this.GetSowByProjectId(proId);
        this.EnableDisableAddendum();
        this._snackBar.open('SOW saved successfully.', 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
         this.ResetSow();
      }
      else if (res == -1) {
        this._snackBar.open('SOW already exist', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

      }
      else if (res == 2627) {
        this._snackBar.open('SOW already exist', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else {
        this._snackBar.open('Unable to save SOW.', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      error => {
        this._snackBar.open(error.error, 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
    });
  }

  updateSOW() {
    //to update SOW
    let proId = this.addSOW.value.ProjectId;
    this.addSOW.value.RoleName = this.currentRole;
    this._sowService.UpdateSOWDetails(
      this.addSOW.value
    ).subscribe(res => {
      if (res == 1) {
        this.sowSignedDates=[];
        this.GetSowByProjectId(proId);

        this._snackBar.open('SOW updated successfully.', 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
         this.ResetSow();
      }
      else if (res == -1) {
        this._snackBar.open('SOW already exist', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else if (res == 2627) {
        this._snackBar.open('SOW already exist', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else {
        this._snackBar.open('Unable to update SOW.', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      error => {
        this._snackBar.open(error.error, 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
    });
    // this.ResetSow();
  }

  setSOWRequiredValidations(sowFormControl: string, newSOWDetails: FormGroup) {
    newSOWDetails.controls[sowFormControl].setValidators([Validators.required])
    newSOWDetails.controls[sowFormControl].updateValueAndValidity();
    if (sowFormControl === 'RecipientName') {
      newSOWDetails.controls[sowFormControl].setValidators([Validators.required, Validators.pattern("[a-zA-Z ]*$")])
    }
    else if (sowFormControl === 'SOWSignedDate'){
      if (this.setSOWDateDynamic()){
        newSOWDetails.controls[sowFormControl].setErrors({invalidNumber:true});
      }
    }
  }


  EditSOW(SowObj): void {
    //to edit SOW details
    this.editMode = true;

    if (this.isSow == 1) {
      this.addButton = "Update";
      this.CancelBtnLabelSOW = "Cancel";
    }
    else {
      this.btnLabel = "Update";
      this.CancelBtnLabelAddendum = "Cancel";
    }
    if (this.isSow == 1) {
      // edit sow
      this._sowService.GetSowDetails(
        this.projectId,
        SowObj.Id,
        this.currentRole
      ).subscribe((res: any) => {
        this.PopulateSowForm(res);
      });
    } else {
      // edit addendum
      this._sowService.GetAddendumDetailsById(
        this.projectId,
        SowObj.AddendumId,
        this.currentRole
      ).subscribe(res => {
        this.PopulateAddendum(res);
      });
    }
  }

  cancelDialog() {
    let messageInfo: string;
    if (this.editMode) {
      messageInfo = 'Do you want to cancel?';
    }
    else {
      messageInfo = 'Do you want to clear?';
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: {
        message: messageInfo,
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.ResetSow();
      }
    });
  }


  deleteDialog(rowData) {
    // method to open delete dialog
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: { message: "Do you want to delete SOW?" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
        this.deleteSOW(rowData);
    });

  }


  deleteSOW(SOW) {
    //to delete SOW
    this._sowService.DeleteSow(SOW.Id).subscribe((res:any) => {
      console.log(res)
      if (res["IsSuccessful"]) {
        this.GetSowByProjectId(this.projectId);
        // this.ResetSow();
        this._snackBar.open(res["Message"], 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else{
        this._snackBar.open(res["Message"], 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    }
    );
  }

  PopulateSowForm(res) {
    //to populate SOW form
    this.addSOW.patchValue({
      SOW: true,
      ProjectId: res.ProjectId,
      SOWId: res.SOWId,
      SOWSignedDate:
        res.SOWSignedDate != null
          ? new Date(moment(res.SOWSignedDate).format("MM/DD/YYYY"))
          : null,
      SOWFileName: res.SOWFileName,
      Id: res.Id,
      RoleName: this.currentRole
    });
  }


  ResetSow() {
    //to Reset Sow
    this.editMode = false;
    this.addButton = "Add";
    this.CancelBtnLabelSOW = "Clear";
    this.addSOW.get('SOWId').enable();
    this.formSubmitted = false;
    this.CreateSowForm();
    this.dynamicSOWValidations('SOWId', this.addSOW);
    this.dynamicSOWValidations('SOWSignedDate', this.addSOW);
    this.dynamicSOWValidations('SOWFileName', this.addSOW);
  }

  dynamicSOWValidations(sowControl: string, newFormSOWValidate: FormGroup) {
    newFormSOWValidate.controls[sowControl].setErrors(null);
    newFormSOWValidate.controls[sowControl].clearValidators();
    newFormSOWValidate.controls[sowControl].updateValueAndValidity();
  }

  GetLabelOfSow(id: number): string {
    //to get SOWs
    var i, label;
    for (i = 0; i < this.sowlist.length; i++) {
      if (id == this.sowlist[i].value) {
        return this.sowlist[i].label;
      }
    }
    return null;
  }

  GetSowSignedDate(id) {
    var i;
    for (i = 0; i < this.sowSignedDates.length; i++) {
      if (id == this.sowSignedDates[i].label) {
        return this.sowSignedDates[i].value;
      }
    }
  }

  GetAddendumsBySOWId(event): void {
    this.dynamicSOWValidations('Note', this.addAddendum);
    this.dynamicSOWValidations('AddendumNo', this.addAddendum);
    this.dynamicSOWValidations('RecipientName', this.addAddendum);
    this.dynamicSOWValidations('AddendumDate', this.addAddendum);
    this.addAddendum.controls.Note.setValue('')
    this.addAddendum.controls.AddendumNo.setValue('')
    this.addAddendum.controls.RecipientName.setValue('')
    this.addAddendum.controls.AddendumDate.setValue('')
    //to get SOW based on SOWID
    this.sowSignedDate = this.GetSowSignedDate(event.value)
    this._sowService.GetAddendumsBySOWId(event.value, this.projectId).subscribe(
      (res: any[]) => {
        this.AddendumData = res;
        this.ChangeFormat(this.AddendumData);
        this.dataSourceAddendum = new MatTableDataSource(this.AddendumData);
        this.dataSourceAddendum.paginator = this.paginator;
        this.dataSourceAddendum.sort = this.sort;
        this.dataSourceAddendum.sortingDataAccessor = (data, header) => data[header].toLowerCase();

      }
    );
  }

  AddAddendum() {
    //invokes when add button in clicked in Addendum form
    this.AddendumformSubmitted = true;
    this.setSOWRequiredValidations('Note', this.addAddendum);
    this.setSOWRequiredValidations('AddendumNo', this.addAddendum);
    this.setSOWRequiredValidations('RecipientName', this.addAddendum);
    this.setSOWRequiredValidations('AddendumDate', this.addAddendum);
    this.setSOWRequiredValidations('Id', this.addAddendum);
    if (this.isSow == 0 && this.editMode == false && this.addAddendum.valid) {
      this.saveAddendum();
    }
    else if (this.isSow == 0) {
      // edit is clicked
      console.log("Update")
      this.addAddendum.value.RoleName = this.currentRole;
      this.addAddendum.value.SOWId = this.GetLabelOfSow(this.addAddendum.value.Id);
      this.addAddendum.value.AddendumDate = moment(this.addAddendum.value.AddendumDate).format("YYYY-MM-DD");
      if (this.addAddendum.valid == true) {
        this.updateAddendum();
      }
    }
  }

  saveAddendum() {
    //to save Addendum
    this.addAddendum.value.RoleName = this.currentRole;
    this.addAddendum.value.ProjectId = this.projectId;
    this.addAddendum.value.SOWId = this.GetLabelOfSow(
      this.addAddendum.value.Id
    );
    if (this.addAddendum.value.AddendumId == null) {
      this.addAddendum.value.AddendumId = 0;
    }
    let Id = this.addAddendum.value.Id;
    this.addAddendum.value.AddendumDate = moment(
      this.addAddendum.value.AddendumDate
    ).format("YYYY-MM-DD");
    if (this.addAddendum.valid == true) {
      this._sowService.CreateAddendum(this.addAddendum.value).subscribe(
        res => {
          if (res == true) {
            this.GetAddendumsBySOWId({ value: Id });
            this._snackBar.open('Addendum saved successfully.', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //this.ResetAddendum();
          }
          this.ResetAddendum();
        },
        error => {
          var sMessage ="Addendum No. Already Exists";
          var sfMsg ="Unable to update Addendum.";
          if(error.error.text==sMessage)
          {
            sfMsg = error.error.text;

          }
          this._snackBar.open(sfMsg, 'x', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
      // this.ResetAddendum();
    }
  }

  updateAddendum() {
    //to update Addendum
    let duplicateaddno=false
    let Id = this.addAddendum.value.Id;
    let addendumid = this.addAddendum.value.AddendumId

   
    this.AddendumData.forEach(element => {
        if(element.AddendumId!=addendumid && element.AddendumNo==this.addAddendum.value.AddendumNo){
          duplicateaddno=true
          return;   
        }
    });
  
    if(!duplicateaddno){
    this._sowService.UpdateAddendumDetails(
      this.addAddendum.value
    ).subscribe(
      res => {
        if (res == true) {
          this.GetAddendumsBySOWId({ value: Id });
          this._snackBar.open('Addendum updated successfully.', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        if (res == false) {
          this._snackBar.open('Unable to update Addendum.', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        this.ResetAddendum();
      },
      error => {
        var sMessage = "Addendum No. Already Exists";
        var sfMsg = "Unable to update Addendum.";
        if (error.error.text == sMessage) {
          sfMsg = error.error.text;

        }

        this._snackBar.open(sfMsg, 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
    }
    else{
      this._snackBar.open('Addendum No. already exists', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
    }
    // this.ResetAddendum();
  }

  PopulateAddendum(res) {
    //to populate addendum form
    this.addAddendum.patchValue({
      Addendum: true,
      ProjectId: res.ProjectId,
      SOWId: res.SOWId,
      AddendumNo: res.AddendumNo,
      RecipientName: res.RecipientName,
      Note: res.Note,
      AddendumDate:
        res.AddendumDate != null
          ? new Date(moment(res.AddendumDate).format("MM/DD/YYYY"))
          : null,
      Id: res.Id,
      AddendumId: res.AddendumId,
      RoleName: this.currentRole
    });
  }


  ResetAddendum() {
    //to Reset Addendum
    this.CancelBtnLabelAddendum = "Clear";
    this.editMode = false;
    this.btnLabel = "Add";
    this.AddendumformSubmitted = false;
    this.CreateAddendumForm();
    this.dynamicSOWValidations('Note', this.addAddendum);
    this.dynamicSOWValidations('AddendumNo', this.addAddendum);
    this.dynamicSOWValidations('RecipientName', this.addAddendum);
    this.dynamicSOWValidations('AddendumDate', this.addAddendum);
    this.dynamicSOWValidations('Id', this.addAddendum);
  }

  cancelAddendumDialog() {
    let messageInfo: string;
    if (this.editMode) {
      messageInfo = 'Do you want to cancel?';
    }
    else {
      messageInfo = 'Do you want to clear?';
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: {
        message: messageInfo,
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.ResetAddendum();
      }
    });
  }


  radioChange(event: Event) {
    if (this.isSow == 0) {
      this.dataSourceSOW = new MatTableDataSource(this.SOWData);
      this.dataSourceSOW.paginator = this.paginator;
      // this.dataSourceSOW.sort = this.sort;
    }
    else {
      this.dataSourceAddendum = new MatTableDataSource(this.AddendumData);
      this.dataSourceAddendum.paginator = this.paginator;
      // this.dataSourceAddendum.sort = this.sort;
    }

  }
  onEmpty(event : any, controlName : string){
    if (event.target.value.length === 0 && controlName === 'SOWId')
      this.addSOW.controls.SOWId.patchValue(null);
    
    else if (event.target.value.length === 0 && controlName === 'SOWFileName')
      this.addSOW.controls.SOWFileName.patchValue(null);
    
    else if (event.target.value.length === 0 && controlName === 'AddendumNo')
      this.addAddendum.controls.AddendumNo.patchValue(null);

    else if (event.target.value.length === 0 && controlName === 'RecipientName')
      this.addAddendum.controls.RecipientName.patchValue(null);  
    
    else if (event.target.value.length === 0 && controlName === 'Note')
      this.addAddendum.controls.Note.patchValue(null);   
        
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (this.isSow == 1)
        this.dataSourceSOW.filter = filterValue.trim().toLowerCase();
      if (this.isSow == 0)
        this.dataSourceAddendum.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceSOW = new MatTableDataSource(this.SOWData);
      this.dataSourceSOW.paginator = this.paginator;
      this.dataSourceSOW.sort = this.sort;

      this.dataSourceAddendum = new MatTableDataSource(this.AddendumData);
      this.dataSourceAddendum.paginator = this.paginator;
      this.dataSourceAddendum.sort = this.sort;
      this.dataSourceSOW.sortingDataAccessor = (data, header) => data[header].toLowerCase();
      this.dataSourceAddendum.sortingDataAccessor = (data, header) => data[header].toLowerCase();
    }
  }


}
