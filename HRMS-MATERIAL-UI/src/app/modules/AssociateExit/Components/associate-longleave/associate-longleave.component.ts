import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig } from '@angular/material/dialog';
import {AssociateResignationData} from '../../Models/associate-resignation.model'
import {AssociateLongLeaveData} from '../../Models/associate-longleave.model'
import { BooleanToStringPipe } from '../../../onboarding/pipes/BooleanToStringPipe';
import { GenericModel } from '../../../master-layout/models/dropdowntype.model';
import { AssignReportingManagerService } from '../../../project-life-cycle/services/assign-reporting-manager.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as moment from 'moment';
import { AssociateResignationService } from '../../Services/associate-resignation.service';
import { AssociateLongleaveService } from '../../Services/associate-longleave.service';
import { EmployeeData } from '../../../admin/models/employee.model';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-associate-longleave',
  templateUrl: './associate-longleave.component.html',
  styleUrls: ['./associate-longleave.component.scss'],
  providers: [BooleanToStringPipe]
})
export class AssociateLongleaveComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  searchAssociate : FormGroup;
  longLeaveForm : FormGroup;
  dialogForm : FormGroup;
  longLeaveData: AssociateLongLeaveData;
  employeeId: number;
  roleName: string;
  SelectedItem : any = null;
  tentativeJoinDate : any;
  RejoinedDate : any;
  formSubmitted = false;
  searchFormSubmitted = false;
  dialogFormSubmitted = false;
  associatesData : AssociateResignationData[] = [];
  resData : AssociateResignationData = null;
  deletedisplay: boolean = false;
  isValidDate:any;
  minDate : any;
  maxDateValue: Date;
  deleteID: number;
  isMaternityCheck:boolean = false;
  employeeList: any[] = [];
  allAssociatesList: any[];
  isVisible:boolean = false;
  isDisable:boolean = false;
  empData: EmployeeData;
  filteredOptionsName: Observable<any>;
  dataSource: MatTableDataSource<AssociateResignationData>;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('AssociateRejoinDialog') associateRejoinDialog: TemplateRef<any>;

  displayedColumns: string[] = ['radioButton','EmployeeName', 'ProjectName', 'ProgramManagerName', 'ReportingManagerName','IsBillable', 'IsCritical', 'Rejoin'];
  private AssociateRejoinDialogRef: MatDialogRef<TemplateRef<any>>;
  constructor(public dialog: MatDialog,
              private service: AssignReportingManagerService,
              private _snackBar: MatSnackBar, 
              private associateResignationService: AssociateResignationService, 
              private yesNoPipe: BooleanToStringPipe,
              private associateLongLeaveService : AssociateLongleaveService) {
                this.longLeaveData = new AssociateLongLeaveData(); 
               }

  ngOnInit(): void {
    this.deletedisplay = false;
    this.isVisible = false;
    this.isMaternityCheck = false;
    this.deleteID = -1;
  this.getAllAssociatesList();
  this.searchAssociate = new FormGroup({
    AssociateName: new FormControl(null,[Validators.required]),
    Maternity : new FormControl(false)
  });

  this.dialogForm = new FormGroup({
    RejoinedDate : new FormControl(null,[Validators.required]),
    reason: new FormControl(null, )
  });

  this.longLeaveForm = new FormGroup({
    LongLeaveStartDate : new FormControl(null,[Validators.required]),
    TentativeJoinDate : new FormControl(null,[Validators.required]),
    Reason : new FormControl(null, )
  });
    this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
  
    if(this.associatesData.length > 0){
      this.isVisible = true;
    }

    this.filteredOptionsName = this.searchAssociate.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );

    this.maxDateValue = new Date();
  }

  radioChangeHandler(associateData : any){
    this.SelectedItem = associateData;
  }

  resetForm(): void {
    this.searchAssociate.reset();
    this.longLeaveForm.reset();
    this.dialogForm.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.isMaternityCheck = false;
    this.formSubmitted = false;
    this.searchFormSubmitted= false;
    this.dialogFormSubmitted= false;
  }

  private _filterName(value): any {
    let filterValue;
    if (typeof value.AssociateName === 'number') {
      return this.employeeList;
    }
    if (value && value.AssociateName) {
      if (typeof value.AssociateName === 'string') {
        filterValue = value.AssociateName.toLowerCase();
      }
      else {
        if (value.AssociateName !== null) {
          filterValue = value.AssociateName.label.toLowerCase();
        }
      }
      return this.employeeList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.employeeList;
    }
  }

  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  clearField() {
    this.searchAssociate.controls.AssociateName.setValue('');
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='LongLeaveStartDate'){
      if(!this.isDisable){
      evt.stopPropagation();
      this.longLeaveForm.get('LongLeaveStartDate').reset();
      } 
    }
    if(fieldName=='TentativeJoinDate'){
      evt.stopPropagation();
      this.longLeaveForm.get('TentativeJoinDate').reset();
    }
    if(fieldName == 'RejoinedDate'){
      evt.stopPropagation();
      this.dialogForm.get('RejoinedDate').reset();
    }
  }

  public IsMaternityCheck(event: any) { 
    this.associatesData = [];
    this.longLeaveData.LongLeaveStartDate = "";
    this.longLeaveData.Reason = "";
    this.tentativeJoinDate = "";
    this.SelectedItem = null;
    this.isVisible = false; 
    if (event.checked) {  
      this.isMaternityCheck = true;
      this.longLeaveData.IsMaternity = true;
      this.getAllAssociatesList();
    }
    else {
      this.isMaternityCheck = false;
      this.longLeaveData.IsMaternity = false;
      this.getAllAssociatesList();
    }
  }

  getAllAssociatesList() {
    this.service.GetAllAssociatesList().subscribe((resp: GenericModel[]) => {
      this.allAssociatesList = [];
      this.employeeList = [];
      this.allAssociatesList = resp;
      this.allAssociatesList.forEach(element => {
        if(this.isMaternityCheck == true) {
          if(element.Gender != null && element.Gender.toLowerCase() != 'male') {
            this.employeeList.push({ label: element.EmpName, value: element.EmpCode });
          }
        }
        else {
          this.employeeList.push({ label: element.EmpName, value: element.EmpCode });
        }
      });
      this.searchAssociate.controls["AssociateName"].reset();
    });
  }

  cancel(): void {
    this.searchAssociate.reset();
    this.searchFormSubmitted= false;
    setTimeout(() => this.formGroupDirective.resetForm(), 0)
    this.isVisible = false;
    this.resetForm();
    this.Clear();
  }


  CalculateMaternityPeriod(maternityStartDate : string) : void {
    if(this.isMaternityCheck == true) {
        if(maternityStartDate != null){
          this.associateLongLeaveService.CalculateMaternityPeriod(moment(maternityStartDate).format('YYYY-MM-DD')).subscribe(
            (res) => {
              if(res != null){
                this.tentativeJoinDate =  res;
              }   
            },
            (error) => {
              this._snackBar.open(error.error, 'x', {
                duration: 3000,
                panelClass: ['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
          );
        }
      }
  }

  getAssociatesById(longLeaveEmployeeId : number): void {
    this.SelectedItem = null;
    this.searchFormSubmitted = true;
    if(this.searchAssociate.valid == true){
      this.associateResignationService.GetAssociatesById(longLeaveEmployeeId, this.employeeId).subscribe(
        (res: AssociateResignationData) => {
          if(res != null){
            this.isVisible = true;
            this.associatesData = [];
            this.resData = res; 
            this.resData.IsBillable = this.yesNoPipe.transform(JSON.parse(res.IsBillable));
            this.resData.IsCritical = this.yesNoPipe.transform(JSON.parse(res.IsCritical));
            this.associatesData.push(this.resData);
            this.dataSource = new MatTableDataSource(this.associatesData);
            if(this.resData.IsLongLeave == true){
              this.isDisable = true;
              this.longLeaveData.EmployeeId = this.resData.EmployeeId;
              this.longLeaveData.LongLeaveStartDate = this.resData.LongLeaveStartDate;
              this.tentativeJoinDate = this.resData.TentativeJoinDate;
              this.longLeaveData.IsLongLeave = true;
              if(this.resData.Reason != null)
              {
                this.longLeaveData.Reason = this.resData.Reason;
              } 
            }
            else {
              this.isDisable = false;
              this.longLeaveData.IsLongLeave = null;
              this.longLeaveData.LongLeaveStartDate = "";
              this.tentativeJoinDate = "";
              this.longLeaveData.Reason = "";
              this.formSubmitted = false;
            }
          }
          else {
            this.associatesData = [];
            this.isVisible = false;
            this._snackBar.open('No records found', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        },
        (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );  
  }
  }

  onCreate() : void {
    this.formSubmitted = true;
    if(this.SelectedItem == null && this.longLeaveData.IsLongLeave == null) {
      this._snackBar.open('Please select Associate', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
    if(this.longLeaveForm.valid == true){
        if(this.longLeaveData.LongLeaveStartDate != null && this.tentativeJoinDate != null) 
        {
          this.isValidDate = this.validateDates(this.longLeaveData.LongLeaveStartDate, this.tentativeJoinDate); 
        }
      
        if(this.isValidDate)
        {
            if(this.longLeaveForm.value.TentativeJoinDate != null){
              this.longLeaveData.TentativeJoinDate = this.longLeaveForm.value.TentativeJoinDate;
            }
            if(this.longLeaveData.IsLongLeave == null)  {
              this.longLeaveData.EmployeeId = this.SelectedItem.EmployeeId;
            }
            this.associateLongLeaveService.CreateAssociateLongLeave(this.longLeaveData).subscribe((res)=>{
              if(res ==true){
                this._snackBar.open('Associate Long Leave saved successfully', '', {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.resetForm();
                this.Clear();
                this.getAllAssociatesList();
              }
              else{
                this.Clear();
                this._snackBar.open('Associate Long Leave cannot be saved', 'x', {
                  duration: 3000,
                  panelClass: ['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }
            },
            error=>{
              this.resetForm();
              this.Clear();
              this._snackBar.open(error.error, 'x', {
                duration: 3000,
                panelClass: ['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            });
          }
    }
  }

  validateDates(resigDate: string, lastWDate: string){
    this.isValidDate = true;
   var startDate = new Date(resigDate);
   var endDate = new Date(lastWDate); 
    if((startDate != null && endDate !=null) && (endDate) <= (startDate)){
      this._snackBar.open('Tentative Join Date should be greater than Long Leave Start date.', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      this.isValidDate = false;
    }
    return this.isValidDate;
  }

  rejoin(event: any) {
    this.RejoinedDate = moment(new Date()).format("YYYY-MM-DD");
    this.minDate = new Date(this.longLeaveData.LongLeaveStartDate);
    this.dialogForm.controls["reason"].reset();
    this.deleteID = event.EmployeeId;
    this.deletedisplay = true;
    this.openDialog();
 }

 
 rejoinAssociate(Reason : string, RejoinDate : string) {  
  RejoinDate = moment(RejoinDate).format("DD-MM-YYYY");
  this.dialogFormSubmitted = true;
  if(this.dialogForm.valid == true){
    this.associateLongLeaveService.RejoinAssociate(this.deleteID, Reason, RejoinDate).subscribe((res) => {
      if(res == true) {
        this._snackBar.open('Associate Rejoined successfully.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.deletedisplay = false;
        this.resetForm();
        this.Clear();
        this.closeDialog();    
        this.getAllAssociatesList(); 
      }
    },(error) => {
      this.deletedisplay = false;
      this._snackBar.open(error.error, 'x', {
       duration: 3000,
       panelClass: ['error-alert'],
       horizontalPosition: 'right',
       verticalPosition: 'top',
     });
   });
  }
}

openDialog(): void {
  const dialogConfig = new MatDialogConfig();
  this.AssociateRejoinDialogRef = this.dialog.open(this.associateRejoinDialog, {
    disableClose: true,
    hasBackdrop: true,
  });
  this.AssociateRejoinDialogRef.afterClosed().subscribe(result => {
  });
}

closeDialog() {
  this.AssociateRejoinDialogRef.close();
}

Clear() {
  this.SelectedItem = null;
  this.longLeaveData.LongLeaveStartDate = "";
  this.tentativeJoinDate = "";
  this.longLeaveData.Reason = "";
  this.longLeaveData.IsMaternity = false;
  this.associatesData = [];
  this.isVisible = false;
}

cancelRejoin() {
  this.deletedisplay = false;
  this.closeDialog();
}
}
