import { Component, OnInit } from '@angular/core';
import { EmployeeData } from '../../models/employee.model';
import { EmployeeStatusService } from '../../services/employeestatus.service';
import { SelectItem } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-map-associate-id-form',
  templateUrl: './map-associate-id-form.component.html',
  styleUrls: ['./map-associate-id-form.component.css'],
  providers: [EmployeeStatusService, MessageService]
})
export class MapAssociateIdFormComponent implements OnInit {
  _usersList: SelectItem[];
    _emailsList: SelectItem[];
    _empData: EmployeeData;
    userform: FormGroup;
    formSubmitted: boolean;

    constructor(private _service: EmployeeStatusService, private fb: FormBuilder, private messageService: MessageService) {
    }

    ngOnInit() {
        this._empData = new EmployeeData();
        this.getUsersList();
        this.getEmailsList();
        this.userform = this.fb.group({
            'ddlEmp': new FormControl('', Validators.required),
            'ddlEmail': new FormControl('', Validators.required)
        });
        this.clear();
    }

    getEmailsList() {
        this._service.GetAssociates().subscribe((res: any) => {
            let dataList: any[] = res;
            this._emailsList = [];
            this._emailsList.push({ label: 'Select Email', value: null });
            dataList.forEach(e => {
                this._emailsList.push({ label: e.EmailAddress, value: e.UserId });
            });
        });
    }
    getUsersList() {
        this._service.GetAssociateNames().subscribe((res: any[]) => {
            let dataList: any[] = res;
            this._usersList = [];
            this._usersList.push({ label: 'Select Associate', value: null });
            dataList.forEach(e => {
                this._usersList.push({ label: e.EmpName, value: e.EmpId });
            });
        });
    }

    onEmailChange(event: any, empID: any) {
        let selectedUserId = event.value;
        if (empID != undefined && selectedUserId != undefined) {
            let empName: string = "";
            this._usersList.forEach(e => {
                if (e.value == empID)
                    empName = e.label;
            });
            let formatedEmpName = empName.replace(' ', '.').trim().toLowerCase();
            let email: string = "";
            this._emailsList.forEach(e => {
                if (e.value == selectedUserId)
                    email = e.label;
            });
            let formatedEmail = email.split('@')[0].toLowerCase();
            
            if (formatedEmpName != formatedEmail) {
                event.value = "";
                // swal("", 'Associate name and email are not matching, Are you sure you want to continue?', "warning");
            }
        }
    }

    onUserChange(event: any, empID: any) {
        let selectedUserId = event.value;

        if (empID != undefined && selectedUserId != undefined) {
            let empName: string = "";
            this._usersList.forEach(e => {
                if (e.value == selectedUserId)
                    empName = e.label;
            });
            let formatedEmpName = empName.replace(' ', '.').trim().toLowerCase();
            let email: string = "";
            this._emailsList.forEach(e => {
                if (e.value == empID)
                    email = e.label;
            });
            let formatedEmail = email.split('@')[0].toLowerCase();
            
            if (formatedEmpName != formatedEmail) {
                event.value = "";
                // swal("", 'Associate name and email are not matching, Are you sure you want to continue?', "warning");
            }
        }
    }

    mapAssociateId() {
        this._service.MapAssociateId(this._empData).subscribe((data) => {
            if(data != null){
                this.messageService.add({severity:'success', summary: 'Success Message', detail:'Associate Id mapped'});
            }

            this.clearValues();
            this.getUsersList();
            this.getEmailsList();
        }, (error) => {         
            this.clearValues();
            this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});
        });
    }

    clearValues = function () {
        this._empData.EmpId = null;
        this._empData.UserId = null;
        this.userform.reset();
    }

    clear = function () {
        this.formSubmitted = false;
        this.clearValues();
    }

    onSubmit() {
        this.formSubmitted = true;
        if (this.userform.valid) {
            this.formSubmitted = false;
            this.mapAssociateId();
        }
    }
}
