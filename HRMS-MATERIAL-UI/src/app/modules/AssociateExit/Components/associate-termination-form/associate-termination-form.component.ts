import { Router } from '@angular/router';
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
  selector: 'app-associate-termination-form',
  templateUrl: './associate-termination-form.component.html',
  styleUrls: ['./associate-termination-form.component.scss'],
  providers: [EmployeeStatusService]
})
export class AssociateTerminationFormComponent implements OnInit {
  
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
    terminationForm: FormGroup;
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
  
   constructor(private _service: EmployeeStatusService,private _snackBar: MatSnackBar, private _router:Router,
    private fb: FormBuilder, private checklist: ChecklistService, private _masterDataService: MasterDataService) {
  
    this.today.setDate(this.today.getDate());
  }
    ngOnInit() {
  
      this._empData = new EmployeeData();
      this.getUsersList();
     
      this.userform = this.fb.group({
       
          'EmpId': [null, [Validators.required]],
        });
      
      this.terminationForm = this.fb.group({
        'ExitType': [null, [Validators.required]],
          'ExitReason': [null, [Validators.required]],
          'IsActive': [null, [Validators.required]],
          'LastWorkingDate': [null, [Validators.required]],
          'tagretDate': [null, [Validators.required]],
          'associateRemarks': [null, [Validators.required]],
      });
      this.clearValues();
      
      
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
      this.checklist.GetByEmployeeId(employeeId).subscribe((data: any) => {
        console.log(data.DateOfJoin = moment(data.DateOfJoin).format("MM/DD/YYYY"));
  
        this.showAllocationHistoryGrid = true;
  
  
  
        
              this.selectedEmpdata.push(data);
        
        this.dataSource = new MatTableDataSource<EmployeeData>(this.selectedEmpdata);
        
        }
    );
  }
  else{
    this.showAllocationHistoryGrid = false;
  }
  }
  
  
  getExitReasonsList(){
    this._masterDataService.GetExitReasons().subscribe((data:any[]) => {
      console.log(data);
      this.exitreasonlist.push({label: 'Select Category', value:null})
      data.forEach(e=>{this.exitreasonlist.push({label:e.Name, value: e.Id})})
      console.log(this.exitreasonlist);
    });
  }
  
  onStatusChange(isActive: boolean) {
      if (isActive == false)
          this.enableDate = true;
      else
          this.enableDate = false;
  }
  
  
  clearValues = function () {

      this.showAllocationHistoryGrid = false;
      this.terminationForm.reset();
      this.userform.reset();
      
  }
  goto(){
    this._router.navigate(['/associateexit/uploadevidence']);
  }
  
  onSubmit() {
  }
  }
  