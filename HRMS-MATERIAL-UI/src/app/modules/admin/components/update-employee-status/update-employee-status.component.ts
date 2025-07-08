import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { EmployeeStatusService } from '../../services/employeestatus.service';
import { RoleService } from '../../services/role.service';
import { Department } from '../../models/department.model';
import { EmployeeData } from '../../models/employee.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import * as moment from 'moment';
import { themeconfig } from 'src/themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { map, startWith } from "rxjs/operators";
import { AssociateAllocationService } from 'src/app/modules/TalentMangment/services/associate-allocation.service';
import { AssociateAllocation } from 'src/app/modules/master-layout/models/associateallocation.model';
import { NgxSpinnerService } from 'ngx-spinner';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-update-employee-status',
  templateUrl: './update-employee-status.component.html',
  styleUrls: ['./update-employee-status.component.scss'],
  providers: [EmployeeStatusService]
})
export class UpdateEmployeeStatusComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  counter:number;
  userform: FormGroup;
  response: any;
  submitted = false;
  _empData: EmployeeData;
  _usersList = [];
  formSubmitted = false;
  _status = [];
  firstDate: Date;
  enableDate: boolean = false;
  @Input() max: any;
  today = new Date();
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  departmentList: SelectItem[];
  filteredOptionsName: Observable<any>;
  allocationHistory: AssociateAllocation[] = [];
  showAllocationHistoryGrid: boolean = false;
  isDateError:boolean=false
  dateErrorMessage:string;


  constructor(private _service: EmployeeStatusService, private allocationservice: AssociateAllocationService, private _rolesService: RoleService, private _snackBar: MatSnackBar, private fb: FormBuilder, private spinner: NgxSpinnerService,) {
    this.initializeStatus();
    this.today.setDate(this.today.getDate());
  }
  ngOnInit() {
    this.spinner.show()
    this._empData = new EmployeeData();
    this.getDepartmentList();
    this.getDates();

    this.userform = this.fb.group({

      EmpId: ['', [Validators.required]],
      IsActive: [false],
      LastWorkingDate: ['', Validators.required],
      DepartmentId: ['',[Validators.required]]
    });
    this.clear();
    this.counter=0;
  }
  get f() { return this.userform.controls; }

  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }
  getDepartmentList(): void {
    this._rolesService.getDepartmentList().subscribe((res: Department[]) => {
      this.spinner.hide()
      this.departmentList = [];
      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });
    }, (error) => {
      this.spinner.hide()
      this._snackBar.open(error.error.text, 'x', {
        duration: 3000, 
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='LastWorkingDate'){
      this.isDateError=false
      evt.stopPropagation();
      this.userform.get('LastWorkingDate').reset();
    }
    if(fieldName=='EmpId'){
      evt.stopPropagation();
      this.userform.get('EmpId').reset();
    }
  }

  initializeStatus() {
    this._status = [];
      this._status.push({ label: 'Inactive', value: false });
  }

  private _filterName(value) {
    let filterValue;
    if (typeof value.EmpId === 'number') {
      return this._usersList;
    }
    if (value && value.EmpId) {
      if (typeof value.EmpId === 'string') {
        filterValue = value.EmpId.toLowerCase();
      }
      else {
        if (value.EmpId !== null) {
          filterValue = value.EmpId.label.toLowerCase();
        }
      }
      return this._usersList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this._usersList;
    }
  }
  getDates() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth()
    this.firstDate = new Date(y, m-1, 1)
  }

  onDepartmentChange(value) {
    this.isDateError=false
    this.userform.get('LastWorkingDate').reset();
    this.userform.get('EmpId').reset();
    this._usersList = [];
    this.filteredOptionsName = null;
    this._service.GetAssociatesByDepartment(value).subscribe((data) => {
      let dataList: any = data;
      dataList.forEach(e => {
        this._usersList.push({ label: e.Name, value: e.Id });
      });
      this.filteredOptionsName = this.userform.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterName(value))
      );

    },(error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    })
  }

  updateEmployeeStatus() {
    if(this.userform.value.EmpId.value != undefined)
      this.userform.value.EmpId = this.userform.value.EmpId.value
    this.userform.value.LastWorkingDate = moment(this.userform.value.LastWorkingDate).format('YYYY-MM-DD');
    console.log(this.userform.value)    
    this._service.UpdateEmployeeStatus(this.userform.value).subscribe((data) => {
      if (data != null) {
        this.clear();
        this.enableDate = false;
        this.isDateError = false
        this._snackBar.open('Associate Deactivated Successfully.', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    }, (error) => {
      if (error.error !=null && (error.error.includes('Invalid date. Date should be greater than'))) {
        this.isDateError = true
        this.dateErrorMessage = error.error
      }
      else {
        this.clear();
        this.enableDate = false;
        this._snackBar.open(error.error, 'x', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    });
  }

  clearValues = function () {
    this._empData.ID = null;
    this._empData.EmpID = null;
    this._empData.IsActive = null;
    this._usersList = [];
    this.filteredOptionsName = null;
    this.userform.reset();
    this.showAllocationHistoryGrid = false;
  }

  clear = function () {
    this.enableDate = false;
    this.clearValues();
    setTimeout(() =>
      this.formGroupDirective.resetForm(), 0)
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.userform.valid) {
      this.updateEmployeeStatus();
    }
  }

  public getAllocationHistory(employeeId: number) {
      this.allocationservice.GetEmpAllocationHistory(employeeId).subscribe((data: AssociateAllocation[]) => {
        if (data != null && data.length > 0) {
          this.allocationHistory = data;
          this.showAllocationHistoryGrid = true;          
        }
        else
          this.showAllocationHistoryGrid = false;
      },
      (error) => {
        this._snackBar.open(error.error, 'x', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
  }
  removeDateErrorMessage(){
    this.isDateError=false
  }

}
