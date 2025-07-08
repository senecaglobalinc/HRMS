import { AssociateExit } from '../../Models/associateExit.model';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { ChecklistService } from '../../Services/checklist.service';
import { EmployeeData } from '../../../admin/models/employee.model';
import { EmployeeStatusService } from '../../../admin/services/employeestatus.service';

import { Component, OnInit, ViewChild, Input} from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import * as moment from 'moment';
import { themeconfig } from 'src/themeconfig';
import {MatSnackBar} from '@angular/material/snack-bar';
import { DropDownType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { ResourceRelease } from 'src/app/modules/TalentMangment/models/resourcerelease.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { map, startWith } from 'rxjs/operators';
import { Observable } from 'rxjs-compat';
import { MatSort } from '@angular/material/sort';
  
interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-associate-exit-by-hr',
  templateUrl: './associate-exit-by-hr.component.html',
  styleUrls: ['./associate-exit-by-hr.component.scss'],
  providers: [EmployeeStatusService]
})
export class AssociateExitByHrComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  themeAppearence = themeconfig.formfieldappearances;
  exitReasonsList= [];
  exitTypesList= [];
  filteredExitReasons: Observable<any>;
  filteredExitTypes: Observable<any>;
  showAllocationHistoryGrid: boolean = false;
  showAbscond: boolean = false;
  showSeparation: boolean = false;
  showTermination: boolean = false;
  dateofjoining: Date
  requisitionlist: AssociateExit
  allocationhistory: AssociateExit[]= []
  userform: FormGroup;
  response: any;
  submitted = false;
  _empData: EmployeeData;
  _usersList = [];
  exitreasonlist =[];
  exittypelist =[];
  formSubmitted = false;
  _status = [];
  lastDate: Date;
  enableDate: boolean = false;
  abscondForm: FormGroup;
  employeesList: DropDownType[] = [];
  employeeId: number;
  associatesList = [];
  roleName: string="";
  releaseResource: ResourceRelease;
  PMScreen: boolean = false;
  UserRole : string;
  disableAssociateName: boolean = false;
  openform: boolean = false;
  @Input() max: any;
  selectedEmpdata=[]
  today = new Date(); 
  ae:any

  dataSource: MatTableDataSource<any>;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  


  displayedColumns = ['DateOfJoin', 'Designation','ReportingManager'];

 constructor(private _service: EmployeeStatusService,private _snackBar: MatSnackBar, 
  private fb: FormBuilder, private checklist: ChecklistService, private _masterDataService: MasterDataService) {

  this.today.setDate(this.today.getDate());
}
  ngOnInit() {

    this._empData = new EmployeeData();
    this.getUsersList();
   
    this.userform = this.fb.group({
     
        'EmpId': [null, [Validators.required]],
      });
    
    this.abscondForm = this.fb.group({
      'ExitType': [null, [Validators.required]],
        'ExitReason': [null, [Validators.required]],
        'IsActive': ['', [Validators.required]],
        'LastWorkingDate': ['', [Validators.required]],
        'tagretDate': [null, [Validators.required]],
        'associateRemarks': [null, [Validators.required]],
    });
    this.clearValues();
    this.getExitTypesList();
    this.getExitReasonsList();
    
  }

  

getUsersList() {
    this._service.GetAssociateNames().subscribe((res: AssociateExit[]) => {
        let dataList: any[] = res;
        this._usersList = [];
        this._usersList.push({ label: 'Select Associate', value: null });
        dataList.forEach(e => {
            this._usersList.push({ label: e.EmpName, value: e.EmpId });
        });
        if (this.employeeId > 0) {
          this.disableAssociateName = true;
          this.userform.controls['EmpId'].setValue(this.employeeId);
  
         this.getCurrentUserDetails(this.employeeId);
        }
    });
}

public getCurrentUserDetails(employeeId: number){
  if(employeeId>0)
  {
    this.checklist.GetByEmployeeId(employeeId).subscribe((data: any[]) => {
      console.log(data);

      this.showAllocationHistoryGrid = true;



      
            this.selectedEmpdata.push(data);
      
        console.log(this.selectedEmpdata) 
      this.dataSource = new MatTableDataSource<EmployeeData>(this.selectedEmpdata);
      console.log(this.dataSource)
      
      }
  );
}
}


getExitReasonsList(){
  this._masterDataService.GetExitReasons().subscribe((exitReasonsResponse:any[]) => {
    let reasonlist: any[] = exitReasonsResponse;
    this.exitReasonsList.push({label: 'Select Category', value:null})
    reasonlist.forEach((exitReasonsResponse) => {
      this.exitReasonsList.push({ label: exitReasonsResponse.Name, value: exitReasonsResponse.Id });
    });
    
  }),
    (error: any) => {
      if (error._body != undefined && error._body != "")
      this._snackBar.open(error.error, 'x', {duration: 1000, panelClass:['Failed to get Cause Category'],horizontalPosition: 'right',
      verticalPosition: 'top' });
    };
}


getExitTypesList(){
  this._masterDataService.GetExitTypes().subscribe((exitTypesResponse:any[]) => {
    let typelist: any[] = exitTypesResponse;
    this.exitTypesList.push({label: 'Select Type', value:null})
    typelist.forEach((exitTypesResponse) => {
      this.exitTypesList.push({ label: exitTypesResponse.Name , value: exitTypesResponse.Id });
    });
  }),
    (error: any) => {
      if (error._body != undefined && error._body != "")
      this._snackBar.open(error.error, 'x', {duration: 1000, panelClass:['Failed to get Exit Types'],horizontalPosition: 'right',
      verticalPosition: 'top' });
    };
}

onStatusChange(isActive: boolean) {
    if (isActive == false)
        this.enableDate = true;
    else
        this.enableDate = false;
}

updateEmployeeStatus() {
  this.userform.value.LastWorkingDate = moment(this.userform.value.LastWorkingDate).format('YYYY-MM-DD');
    this._service.UpdateEmployeeStatus(this.userform.value).subscribe((data) => {
        if(data != null){
       
        this._snackBar.open('Associate status updated successfully.', 'x', {
          duration: 1000,
          panelClass:['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
    }
        this.getUsersList();
        this.clearValues();
        this.enableDate = false;
        
    }, (error) => {
        this._snackBar.open(error.error, 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

    });
}

onClick(){
 this.openform = true;
}

clearValues = function () {
 
    this.abscondForm.reset();
    
}

onSubmit() {
}
}
