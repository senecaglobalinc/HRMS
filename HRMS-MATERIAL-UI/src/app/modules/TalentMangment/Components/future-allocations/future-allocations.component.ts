import { Component, OnInit, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { AssociateAllocationService } from '../../services/associate-allocation.service';
import { Observable } from 'rxjs';
import { DropDownType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { map, startWith } from "rxjs/operators";
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { themeconfig } from 'src/themeconfig';
import { AssociateAllocation } from 'src/app/modules/master-layout/models/associateallocation.model';
import * as moment from 'moment';
import { ResourceReportNoncriticalService } from 'src/app/modules/reports/services/resource-report-noncritical.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-future-allocations',
  templateUrl: './future-allocations.component.html',
  styleUrls: ['./future-allocations.component.scss']
})
export class FutureAllocationsComponent implements OnInit, OnChanges {

  futureallocationForm: FormGroup;
  empid: number;
  roleName: any;
  projectsList = [];
  filteredEmployee : Observable<any>;
  filteredProject : Observable<any>;
  employeesList: DropDownType[] = [];
  tempEmployeesList:any=[];
  projectDetails: ProjectsData[] = [];
  formsubmitted: boolean;
  themeConfigInput = themeconfig.formfieldappearances;
  allocationHistory: AssociateAllocation[] = [];
  showAllocationHistoryGrid: boolean = false;
  availableAllocationPercentage: number = 0;
  talentpoolEffectiveDate: string;
  requisitionList: AssociateAllocation;
  projectselected:any;
  markFutureObj:any;
  disableproject: Boolean = false;
  projectId: number;
  projectName: String;
  selectedEmpId: number;
  futuremarkedprojectObj: any;
  date=new Date();
  minDate= new Date();

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _formBuilder: FormBuilder,
    private masterDataService: MasterDataService,
    private _service: AssociateAllocationService,
    private snackBar: MatSnackBar,
    private _NoncriticalResourceReportService: ResourceReportNoncriticalService) { 
      this.requisitionList = new AssociateAllocation();
    }
  ngOnChanges(changes: SimpleChanges): void {
    // this.projectselected = changes.projectselected.currentValue;
  }
  ngOnInit(): void {
    // this.spinner.show();
    this.empid = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.futureallocationForm = this._formBuilder.group({
      'ProjectId': [null,[Validators.required]],
      'EmployeeId': [null, [Validators.required]],
      'tentativeDate': ['', [Validators.required]],
      'remarks':[null],
      'ProjectName':[null]
    });
    this.getProjectList();
    this.getEmployeesList();
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='tentativeDate'){
      evt.stopPropagation();
      this.futureallocationForm.get('tentativeDate').reset();
    }
    if(fieldName=='EmployeeId'){
      evt.stopPropagation();
      this.futureallocationForm.get('EmployeeId').reset();
      this.Cancel();
    }
    if(fieldName=='ProjectId'){
      evt.stopPropagation();
      this.futureallocationForm.get('ProjectId').reset();
    }
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  private getProjectList(): void {
    this.masterDataService.GetProjectsList().subscribe((projectResponse: any[]) => {
      this.projectDetails = projectResponse;
      this.projectsList = [];
      projectResponse.forEach((pr) => {
        this.projectsList.push({ label: pr.Name, value: pr.Id });
      });
      this.projectsList.push({ label:"Others", value:0})
      this.filteredProject = this.futureallocationForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProject(value))
      ); 
    }),
      (error: any) => {
        this.snackBar.open('Failed to get Project List', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
  }

  private getEmployeesList(): void {
    this._NoncriticalResourceReportService.GetAssociatesForFutureAllocation().subscribe((employees: any[]) => {
      this.employeesList = [];
      employees.sort((a, b) => (a.AssociateName > b.AssociateName) ? 1 : ((b.AssociateName > a.AssociateName) ? -1 : 0))
      employees.forEach((employee: any) => {
        if (this.employeesList.findIndex(x => x.label == employee.AssociateName) == -1)
          this.employeesList.push({ label: employee.AssociateName, value: employee.EmployeeId });
      });
      this.tempEmployeesList = this.employeesList
      this.filteredEmployee = this.futureallocationForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterEmployee(value))
      );
    }),
      (error: any) => {
        this.snackBar.open('Failed to get Employees List', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
  }

  private _filterProject(value) {
    
    let filterValue;
    if (typeof value.ProjectId === 'number') {
      return this.projectsList;
    }
    if (value && value.ProjectId) {
      if (typeof value.ProjectId === 'string') {
        filterValue = value.ProjectId.toLowerCase();
      }
      else {
        if (value.ProjectId !== null) {
          filterValue = value.ProjectId.label.toLowerCase();
        }
      }
      return this.projectsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectsList;
    }
  }
  private _filterEmployee(value) {
    
    let filterValue;
    if (typeof value.EmployeeId === 'number') {
      return this.employeesList;
    }
    if (value && value.EmployeeId) {
      if (typeof value.EmployeeId === 'string') {
        filterValue = value.EmployeeId.toLowerCase();
      }
      else {
        if (value.EmployeeId !== null) {
          filterValue = value.EmployeeId.label.toLowerCase();
        }
      }
      return this.employeesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.employeesList;
    }
  }

  public getAllocationHistory(employeeId: number) {
    this.formsubmitted = false;
    this.selectedEmpId = employeeId;
      this._service.GetEmpAllocationHistory(employeeId).subscribe((data: AssociateAllocation[]) => {
        if (data != null && data.length > 0) {
          this.allocationHistory = data;
          this.showAllocationHistoryGrid = true;
          this.requisitionList.AssociateName = data[0].AssociateName;
          this.allocationHistory.forEach(ele => {
          this.projectsList=this.projectsList.filter(x=>x.value!=ele.ProjectId);
           if (ele.ProjectTypeId == 6) {
              this.availableAllocationPercentage = ele.AllocationPercentage;
              this.talentpoolEffectiveDate = moment(ele.EffectiveDate).format('YYYY-MM-DD');
            }
          }
          )
        }
        else
          this.showAllocationHistoryGrid = false;
      });
    }

    Cancel(){
      this.futureallocationForm.reset();
      setTimeout(() => this.formGroupDirective.resetForm(), 0);

      this.requisitionList = new AssociateAllocation();
      this.allocationHistory = [];
      this.showAllocationHistoryGrid = false;
      this.futuremarkedprojectObj = [];
      this.projectselected = null;
    }
    ProjectSelection(event){
      this.projectselected = event.value;
      this.futureallocationForm.controls['ProjectName'].setValue(null);
      if(this.projectselected == 0){
        this.futureallocationForm.controls['ProjectName'].setValidators([Validators.required]);
        this.futureallocationForm.controls['ProjectName'].updateValueAndValidity();
        this.futureallocationForm.controls['ProjectName'].markAsUntouched();
      }
      else{
        this.futureallocationForm.controls['ProjectName'].setValidators([]);
        this.futureallocationForm.controls['ProjectName'].updateValueAndValidity(); 
      }
    }
    MarkFuture(){
      if(this.futureallocationForm.valid){
        if(this.projectselected == 0 || this.projectselected == null || this.projectselected == undefined){
          this.projectId = 0;
          this.projectName = this.futureallocationForm.value.ProjectName;
        }
        else{
          this.projectId = this.futureallocationForm.value.ProjectId.value;
          this.projectName = this.futureallocationForm.value.ProjectId.label;
        }  
        let tentativedate= moment(this.futureallocationForm.value.tentativeDate).format('YYYY-MM-DD');
        this.markFutureObj = {
          "employeeId": this.futureallocationForm.value.EmployeeId.value,
          "projectName": this.projectName,
          "projectId": this.projectId,
          "tentativeDate": tentativedate,
          "remarks": this.futureallocationForm.value.remarks
        }
        this._service.AddAssociateFutureProject(this.markFutureObj).subscribe(res => {
          if(res['IsSuccessful'] == true){
            if(res['Message'] == "Record already exist"){
              this.snackBar.open('Cannot mark more than one future project', '', {
                panelClass: ['error-alert'],
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
            else{
              this.snackBar.open('Successfully marked future project', '', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            }
          }
        },
        error => {
          this.snackBar.open('Failed to mark future project', '', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        })
        this.Cancel();
      }
      else{
        return
      }
    }

  GetAssociateFutureProjectByEmpId(){
    this.futuremarkedprojectObj = [];
    this._service.GetAssociateFutureProjectByEmpId(this.selectedEmpId).subscribe(res => {
      if(res){
        this.futuremarkedprojectObj = res;
      }
    })
  }
  Deactivate() {
    this._service.DiactivateAssociateFutureProjectByEmpId(this.selectedEmpId).subscribe(res => {
      if(res['IsSuccessful']){
        this.snackBar.open('Deactivated Successfully', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      this.GetAssociateFutureProjectByEmpId();
    })
  }
}
