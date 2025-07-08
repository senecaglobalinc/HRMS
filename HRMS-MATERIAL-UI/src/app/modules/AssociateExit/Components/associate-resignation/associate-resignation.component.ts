import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig } from '@angular/material/dialog';
import {AssociateResignationData} from '../../Models/associate-resignation.model'
import { BooleanToStringPipe } from '../../../onboarding/pipes/BooleanToStringPipe';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { AssignReportingManagerService } from '../../../project-life-cycle/services/assign-reporting-manager.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EmployeeData } from '../../../admin/models/employee.model';
import * as moment from 'moment';
import { AssociateResignationService } from '../../Services/associate-resignation.service';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-associate-resignation',
  templateUrl: './associate-resignation.component.html',
  styleUrls: ['./associate-resignation.component.scss'],
  providers: [BooleanToStringPipe, DatePipe]
  
})
export class AssociateResignationComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  searchAssociate : FormGroup;
  resignationForm : FormGroup;
  dialogForm : FormGroup;
  resignationData: AssociateResignationData;
  employeeId: number;
  roleName: string;
  SelectedItem : any = null;
  LastWorkingDate : any;
  formSubmitted = false;
  searchFormSubmitted = false;
  dialogFormSubmitted = false;
  isValidDate:any;
  associatesData : AssociateResignationData[] = [];
  resData : AssociateResignationData = null;
  deletedisplay: boolean = false;
  RevokedDate : any;
  minDate : any;
  maxDateValue: Date;
  deleteID: number;
  employeeList: any[] = [];
  allAssociatesList: any[];
  isVisible:boolean = false;
  isDisable:boolean = false;
  empData: EmployeeData;
  filteredOptionsName: Observable<any>;
  dataSource: MatTableDataSource<AssociateResignationData>;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('ResignationRevokeDialog') resignationRevokeDialog: TemplateRef<any>;

  displayedColumns: string[] = ['radioButton','EmployeeName', 'ProjectName', 'ProgramManagerName', 'ReportingManagerName','IsBillable', 'IsCritical', 'Revoke'];
  private ResignationRevokeDialogRef: MatDialogRef<TemplateRef<any>>;

  constructor(public dialog: MatDialog,private datePipe: DatePipe,private service: AssignReportingManagerService,private _snackBar: MatSnackBar, private associateResignationService: AssociateResignationService, private yesNoPipe: BooleanToStringPipe) {
    this.resignationData = new AssociateResignationData(); 
   }

  ngOnInit(): void {
    this.deletedisplay = false;
    this.isVisible = false;
    this.deleteID = -1;

    let userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.getAllAssociatesList();

    this.searchAssociate = new FormGroup({
      AssociateName: new FormControl(null,Validators.required)
    });

    this.resignationForm = new FormGroup({
      ResignationDate : new FormControl(null,[Validators.required]),
      LastWorkingDate : new FormControl(null,[Validators.required]),
      ReasonDescription : new FormControl(null, )
    });

    this.dialogForm = new FormGroup({
      RevokedDate : new FormControl(null,[Validators.required]),
      reason: new FormControl(null, [Validators.required])
    });

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
    this.resignationForm.reset();
    this.dialogForm.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
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
    if(fieldName=='ResignationDate'){
      if(!this.isDisable){
      evt.stopPropagation();
      this.resignationForm.get('ResignationDate').reset();
      }
    }
    if(fieldName=='LastWorkingDate'){
      evt.stopPropagation();
      this.resignationForm.get('LastWorkingDate').reset();
    }
    if(fieldName == 'RevokedDate'){
      evt.stopPropagation();
      this.dialogForm.get('RevokedDate').reset();
    }
  }

  getAllAssociatesList() {
    this.service.GetAllAssociatesList().subscribe((resp: GenericType[]) => {
      this.allAssociatesList = [];
      this.allAssociatesList = resp;
      this.allAssociatesList.forEach(element => {
        if (this.employeeList.findIndex(x => x.label == element.EmpName) == -1)
          this.employeeList.push({ label: element.EmpName, value: element.EmpCode });
      });
      
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

  CalculateLastWorkingDate(resignationDate:string) : void {
    if(resignationDate != null){
      this.associateResignationService.CalculateNoticePeriod(moment(resignationDate).format('YYYY-MM-DD')).subscribe(
        (res: any) => {
          if(res != null){
            this.LastWorkingDate =  res;
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

  getAssociatesById(resignEmployeeId : number): void {
    this.SelectedItem = null;
    this.searchFormSubmitted = true;
    if(this.searchAssociate.valid == true){
      this.associateResignationService.GetAssociatesById(resignEmployeeId, this.employeeId).subscribe(
        (res: AssociateResignationData) => {
          if(res != null){
            this.isVisible = true;
            this.associatesData = [];
            this.resData = res; 
            this.resData.IsBillable = this.yesNoPipe.transform(JSON.parse(res.IsBillable));
            this.resData.IsCritical = this.yesNoPipe.transform(JSON.parse(res.IsCritical));
            this.associatesData.push(this.resData);
            this.dataSource = new MatTableDataSource(this.associatesData);
            if(this.resData.IsResigned == true){
              this.isDisable = true;
              this.resignationData.EmployeeId = this.resData.EmployeeId;
              this.resignationData.ResignationDate = this.resData.ResignationDate;
              this.LastWorkingDate = this.resData.LastWorkingDate;
              this.resignationData.IsResigned = true;
              if(this.resData.ReasonDescription != null)
              {
                this.resignationData.ReasonDescription = this.resData.ReasonDescription;
              }  
            }
            else {
              this.isDisable = false;
              this.resignationData.IsResigned = null;
              this.resignationData.ResignationDate = "";
              this.LastWorkingDate = "";
              this.resignationData.ReasonDescription = "";
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
    if(this.SelectedItem == null && this.resignationData.IsResigned == null) {
      this._snackBar.open('Please select Associate', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
    if(this.resignationForm.valid == true){
        if(this.resignationData.ResignationDate != null && this.LastWorkingDate != null) 
        {
          this.isValidDate = this.validateDates(this.resignationData.ResignationDate, this.LastWorkingDate); 
        }
      
        if(this.isValidDate)
        {
            if(this.resignationForm.value.LastWorkingDate != null){
              this.resignationData.LastWorkingDate = this.resignationForm.value.LastWorkingDate;
            }
            if(this.resignationData.IsResigned == null)  {
              this.resignationData.EmployeeId = this.SelectedItem.EmployeeId;
            }
            this.associateResignationService.CreateAssociateResignation(this.resignationData).subscribe((res)=>{
              if(res ==true){
                this._snackBar.open('Associate Resignation saved successfully', '', {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.resetForm();
                this.Clear();
              }
              else{
                this.Clear();
                this._snackBar.open('Associate Resignation cannot be saved', 'x', {
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
   var endDate = new Date(this.datePipe.transform(lastWDate, 'yyyy-MM-dd')); 
    if((startDate != null && endDate !=null) && (endDate) < (startDate)){
      this._snackBar.open('Last Working date should be greater or equal to Resignation date.', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      this.isValidDate = false;
    }
    return this.isValidDate;
  }

  revokeResignation(event: any) {
    this.RevokedDate = moment(new Date()).format("YYYY-MM-DD");
    this.minDate = new Date(this.resignationData.ResignationDate);
    this.dialogForm.controls["reason"].reset();
    this.deleteID = event.EmployeeId;
    this.deletedisplay = true;
    this.openDialog();
 }

 revokeAssociateResignation(ReasonDescription : string, RevokeDate : string) {
  this.dialogFormSubmitted = true;
  if(this.dialogForm.valid == true){
    this.associateResignationService.RevokeResignation(this.deleteID, ReasonDescription, RevokeDate).subscribe((res) => {
      if(res == true) {
        this._snackBar.open('Associate Resignation Revoked successfully.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.deletedisplay = false;
        this.resetForm();
        this.Clear();
        this.closeDialog();
      }
    }, (error) => {
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
  this.ResignationRevokeDialogRef = this.dialog.open(this.resignationRevokeDialog, {
    disableClose: true,
    hasBackdrop: true,
  });
  this.ResignationRevokeDialogRef.afterClosed().subscribe(result => {
  });
}

closeDialog() {
  this.ResignationRevokeDialogRef.close();
}

  Clear() {
    this.SelectedItem = null;
    this.resignationData.ResignationDate = "";
    this.LastWorkingDate = "";
    this.resignationData.ReasonDescription = "";
    this.associatesData = [];
    this.isVisible = false;
  }

  cancelRevoke() {
    this.deletedisplay = false;
    this.closeDialog();
}

}
