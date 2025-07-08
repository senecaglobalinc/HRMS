import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup, FormGroupDirective } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AssociateAllocation} from '../../../master-layout/models/associateallocation.model';
import * as moment from 'moment';
import { DropDownType, GenericType } from '../../../master-layout/models/dropdowntype.model';
import { themeconfig } from 'src/themeconfig';
import { TemporaryAllocationReleaseService } from '../../services/temporary-allocation-release.service';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import {AllocationChangesService } from '../../services/allocation-changes.service';
import { AssignReportingManagerService } from '../../../project-life-cycle/services/assign-reporting-manager.service';
import { ConfirmDialogComponent } from '../../../project-life-cycle/components/confirm-dialog/confirm-dialog.component';
@Component({
  selector: 'app-allocation-changes',
  templateUrl: './allocation-changes.component.html',
  styleUrls: ['./allocation-changes.component.scss']
})
export class AllocationChangesComponent implements OnInit {
  isDisabled = true;
  EffectiveDate:string;
  themeConfigInput = themeconfig.formfieldappearances;
  formsubmitted: boolean;
  requisitionList: AssociateAllocation;
  componentName: string;
  allocationForm: FormGroup;
  allocationHistory: AssociateAllocation;
  projectsList = [];
  associatesList = [];
  showAllocationHistoryGrid: boolean = false;
  disableBilling: boolean;
  minDate: Date;
  maxDate: Date;
  invalidDates: boolean = false;
  empid: number;
  PageSize: number;
  disableddlProject = false;
  PageDropDown: number[] = [];
  employeeId: any;
  roleName: any;
  filteredOptionsName: Observable<any>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  constructor(private _formBuilder: FormBuilder,
    private _service: AllocationChangesService,
    private actRoute: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router,
    private _tempAllocationRelease: TemporaryAllocationReleaseService,
    private _assignManagerservice: AssignReportingManagerService) { 
      this.componentName = this.actRoute.routeConfig.component.name;
      this.requisitionList = new AssociateAllocation();
    }

  ngOnInit() {
   
    this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.getEmployeesList();
    this.allocationForm = new FormGroup({
      ProjectId: new FormControl(null,[Validators.required]),
      EmployeeId: new FormControl(null,[Validators.required]),
      EffectiveDate: new FormControl(null,[Validators.required]),
      isCritical : new FormControl(false)
    });
    this.maxDate = new Date();
    this.filteredOptionsName = this.allocationForm.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );
  }

  private getEmployeesList(): void {
    this._tempAllocationRelease.GetAssociatesToRelease(this.employeeId, this.roleName).subscribe((res: any) => {
        this.associatesList = []
        //this.associatesList.push({ label: '', value: null });
        if (res.length > 0) {
            res.forEach((element: any) => {
                if (this.associatesList.findIndex(x => x.label == element.EmpName) === -1)
                    this.associatesList.push({ label: element.EmpName, value: element.EmployeeId });
            });
        }
    },(error: any) => {
      this.snackBar.open('Failed to Get Employees!.', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
  }  
    );
 }

 getProjectsByEmployeeId(employeeId: number): void {
   this.allocationForm.reset({
    EmployeeId : this.allocationForm.get('EmployeeId').value
  });
  this.showAllocationHistoryGrid = false;
  this._assignManagerservice.GetProjects(employeeId).subscribe((res: any) => {
    this.projectsList = [];
    if (res.length > 0) {
        res.forEach((element: any) => {
          if (this.projectsList.findIndex(x => x.label == element.ProjectName) == -1) {
            this.projectsList.push({ label: element.ProjectName, value: element.ProjectId })
        };
        });
        if (this.projectsList.length == 1) {
          this.requisitionList.ProjectId = this.projectsList[0].value;
          this.allocationForm.controls['ProjectId'].setValue(this.projectsList[0].value);
          this.disableddlProject = true;
          this.GetCurrentAllocation(employeeId,this.requisitionList.ProjectId);
      }
      else this.disableddlProject = false;
    }
},
    (error: any) => {
        this.snackBar.open('Failed to Get projects by employee!.', 'x', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
    }
); 
} 

  GetCurrentAllocation(employeeId: number, projectId: number): void {
    this._service.GetCurrentAllocationByEmpIdAndProjectId(employeeId, projectId).subscribe((data: AssociateAllocation) => {
      if (data != null) {
        this.allocationHistory = data;
        this.minDate = new Date(data.EffectiveDate);
        this.showAllocationHistoryGrid = true;
        this.requisitionList.AssociateName = data.AssociateName;
        if(data.isCritical == true){
          this.allocationForm.controls['isCritical'].setValue(true);
        }
      }
      else
        this.showAllocationHistoryGrid = false;
    });
  }

  checkValue(event){
    console.log(event.checked);
    if(!event.checked){
      this.OpenConfirmation();
    }
    else {
      this.requisitionList.isCritical = true;
    }
 }

  OpenConfirmation() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      disableClose: true,
      hasBackdrop: true,
      data:{
        message: 'Do you want to change from critical to non-critical?',
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.allocationForm.controls['isCritical'].setValue(false);
       this.requisitionList.isCritical = false;
      }
      else{this.allocationForm.controls['isCritical'].setValue(true);}
    });
  }

  public Cancel() {
    this.allocationForm.reset();
    this.formsubmitted = false;    
    this.showAllocationHistoryGrid = false;
    this.allocationHistory = null;
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.requisitionList = new AssociateAllocation();
  }

  public UpdateAssociateAllocation(requisitionList: AssociateAllocation) {
    this.formsubmitted = true;
    this.requisitionList.EmployeeId = this.allocationForm.value.EmployeeId.value;
    this.requisitionList.isCritical = requisitionList.IsCritical;
    this.requisitionList.EffectiveDate = moment(requisitionList.EffectiveDate).format('YYYY-MM-DD');
    this._service.UpdateAssociateAllocation(this.requisitionList).subscribe((data: any) => {
      if (data.IsSuccessful == true) {
        this.snackBar.open('Associate allocation are updated successfully! ', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.Cancel();
      }
      else if (data.IsSuccessful == false) {
        this.snackBar.open('Unable to update allocation changes', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.Cancel();
      }
    }, (error) => {
      this.snackBar.open('Failed to update allocation changes.', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
      });
      this.Cancel();
  })
  }

  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  clearField() {
    this.allocationForm.controls.EmployeeId.setValue('');
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='EffectiveDate'){
      evt.stopPropagation();
      this.allocationForm.get('EffectiveDate').reset();
    }
  }

  
  private _filterName(value): any {
    let filterValue;
    //this.Cancel();
    if (value && value.EmployeeId) {
      filterValue = value.EmployeeId;
      return this.associatesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue.toString().toLowerCase())
      );
    } else {
      return this.associatesList;
    }
  }

}
