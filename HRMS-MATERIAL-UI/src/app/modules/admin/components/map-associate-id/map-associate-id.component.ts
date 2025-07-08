import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
  ValidationErrors,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { map, startWith } from 'rxjs/operators';

import { themeconfig } from '../../../../../themeconfig';
import { EmployeeData } from '../../models/employee.model';
import { EmployeeStatusService } from '../../services/employeestatus.service';
import { Observable } from 'rxjs';

interface SelectItem {
  value : number;
  label : string;
}
@Component({
  selector: 'app-map-associate-id',
  templateUrl: './map-associate-id.component.html',
  styleUrls: ['./map-associate-id.component.scss'],
  providers: [EmployeeStatusService],
})

export class MapAssociateIdComponent implements OnInit {
  controlName: string;
  matchList:SelectItem[];
  usersList = [];
  emailsList = [];
  empData: EmployeeData;
  formSubmitted: boolean;
  mapAssociateId: FormGroup;
  themeAppearence = themeconfig.formfieldappearances;
  filteredOptionsName: Observable<any>;
  filteredOptionsEmail: Observable<any>;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  selected = this.usersList;
  constructor(
    private service: EmployeeStatusService,
    private snackBar: MatSnackBar
  ) {
    this.getUsersList();
    this.getEmailsList();
  }

  ngOnInit(): void {
    this.empData = new EmployeeData();
    this.mapAssociateId = new FormGroup({
      associateName: new FormControl('', [Validators.required]),
      associateEmail: new FormControl('', [Validators.required]),
    });

    this.clear();
  }

  private _filterName(value): any {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else {
      if (value !== null) {
        filterValue = value.label.toLowerCase();
      } else {
        return this.usersList;
      }
    }

    return this.usersList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }
  private _filterEmail(value: any): any {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else {
      if (value !== null) {
        filterValue = value.label.toLowerCase();
      } else {
        return this.emailsList;
      }
    }

    return this.emailsList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }

  displayFn(user: any) {
    return user ? user.label : '';
  }
  
  getFormControl(value:any){
    this.controlName = value;
  }
  
  getEmailsList() {
    this.service.GetAssociates().subscribe((res: any) => {
      // tslint:disable-next-line:prefer-const
      let dataList: any[] = res;
      this.emailsList = [];
      dataList.forEach((e) => {
        this.emailsList.push({ label: e.EmailAddress, value: e.UserId });
      });
      this.filteredOptionsEmail = this.mapAssociateId
        .get('associateEmail')
        .valueChanges.pipe(
          startWith(''),
          map((value) => this._filterEmail(value))
        );
    });
  }
  getUsersList() {
    this.service.GetAssociateNames().subscribe((res: any[]) => {
      // tslint:disable-next-line:prefer-const
      let dataList: any[] = res;
      this.usersList = [];
      dataList.forEach((e) => {
        this.usersList.push({ label: e.EmpName, value: e.EmpId });
      });
      this.filteredOptionsName = this.mapAssociateId
        .get('associateName')
        .valueChanges.pipe(
          startWith(''),
          map((value) => this._filterName(value))
        );
    });
  }
  onEmailChange(event: any, empID: any) {
    const selectedUserId = this.mapAssociateId.get('associateEmail').value
      .value;
    if (selectedUserId === null) {
      this.mapAssociateId.controls.associateEmail.setValue('');
    }
    this.empData.UserId = selectedUserId;
    if (empID !== undefined && selectedUserId !== undefined) {
      let empName = '';
      this.usersList.forEach((e) => {
        if (e.value === empID) {
          empName = e.label;
        }
      });
      const formatedEmpName = empName.replace(' ', '.').trim().toLowerCase();
      let email = '';
      this.emailsList.forEach((e) => {
        if (e.value === selectedUserId) {
          email = e.label;
        }
      });
      const formatedEmail = email.split('@')[0].toLowerCase();

      // if (formatedEmpName !== formatedEmail) {
      //   event.option.value = '';
      // }
    }
  }

  onUserChange(event: any, empID: any) {
    const selectedUserId = this.mapAssociateId.get('associateName').value.value;
    if (selectedUserId === null) {
      this.mapAssociateId.controls.associateName.setValue('');
    }
    this.empData.EmpId = selectedUserId;
    if (empID !== undefined && selectedUserId !== undefined) {
      let empName = '';
      this.usersList.forEach((e) => {
        if (e.value === selectedUserId) {
          empName = e.label;
        }
      });
      const formatedEmpName = empName.replace(' ', '.').trim().toLowerCase();
      let email = '';
      this.emailsList.forEach((e) => {
        if (e.value === empID) {
          email = e.label;
        }
      });
      const formatedEmail = email.split('@')[0].toLowerCase();

      // if (formatedEmpName !== formatedEmail) {
      //   event.option.value = '';
      // }
    }
  }

  mapAssociateIdMethod() {
    this.service.MapAssociateId(this.empData).subscribe(
      (data) => {
        if (data !== null) {
          this.snackBar.open('Associate Id Mapped', 'x', {
            duration: 3000,
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
          });
        }

        this.clearValues();
        this.getUsersList();
        this.getEmailsList();
      },
      (error) => {
        this.clearValues();
        this.snackBar.open('Failed to Map Associate', 'x', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
    );
  }

  clearValues() {
    this.empData.EmpId = null;
    this.empData.UserId = null;
    this.mapAssociateId.reset();
  }

  clear() {
    this.formSubmitted = false;
    this.clearValues();
  }

  clearInput(event : any, fieldName: string){
    if (fieldName == 'associateName'){
      event.stopPropagation();
      this.mapAssociateId.get('associateName').reset();
      this.mapAssociateId.get('associateEmail').reset();
    }
    else if (fieldName == 'associateEmail'){
      event.stopPropagation();
      this.mapAssociateId.get('associateEmail').reset();
    }
  }

  onSubmit() {
    const enteredName = this.mapAssociateId.get('associateName').value;
    const enteredEmail = this.mapAssociateId.get('associateEmail').value;
    if (enteredName === 'Select Associate') {
      this.mapAssociateId.controls.associateName.setValue('');
    }
    if (enteredEmail === 'Select Email') {
      this.mapAssociateId.controls.associateEmail.setValue('');
    }
    // if (this.usersList.find(x => x.label === enteredName) && this.emailsList.find(x => x.label === enteredEmail)){

    //}
    //else{
    //return;
    //}
    this.formSubmitted = true;
    this.mapAssociateId.markAllAsTouched();

    if (this.mapAssociateId.valid) {
      this.formSubmitted = false;
      this.mapAssociateIdMethod();
    }
  }
}
