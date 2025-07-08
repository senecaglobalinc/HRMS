import { Component, OnInit } from '@angular/core';
import { SelectItem, Message, MessageService } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
//import swal from 'sweetalert2';
import * as moment from 'moment';
import { EmployeeStatusService } from '../../services/employeestatus.service';
import { EmployeeData } from '../../models/employee.model';
@Component({
    selector: 'app-update-employee-status-form',
    templateUrl: './update-employee-status-form.component.html',
    styleUrls: ['./update-employee-status-form.component.css'],
    providers: [EmployeeStatusService, MessageService]
})
export class UpdateEmployeeStatusFormComponent implements OnInit {
    _usersList: SelectItem[];
    _status: SelectItem[];
    _empData: EmployeeData;
    userform: FormGroup;
    formSubmitted: boolean;
    lastDate: Date;
    enableDate: boolean = false;

    constructor(private _service: EmployeeStatusService, private fb: FormBuilder, private messageService: MessageService) {
        this.initializeStatus();
    }

    ngOnInit() {
        this._empData = new EmployeeData();
        this.getUsersList();
        this.getDates();
        this.userform = this.fb.group({
            'EmpId': new FormControl('', Validators.required),
            'IsActive': new FormControl('', Validators.required),
            'LastWorkingDate': new FormControl('')
        });
        this.clear();
    }

    initializeStatus() {
        this._service.GetResignStatus().subscribe((res: any) => {
            this._status = [];
            this._status.push({ label: 'Select Status', value: null });
            this._status.push({ label: 'Inactive', value: false });
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

    getDates() {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth(), d = date.getDate();
        this.lastDate = new Date(y, m, d);
    }

    onStatusChange(isActive: boolean) {
        if (isActive == false)
            this.enableDate = true;
        else
            this.enableDate = false;
    }

    updateEmployeeStatus() {
        this.userform.value.LastWorkingDate = moment(this.userform.value.LastWorkingDate).format('YYYY-MM-DD');
        // moment(this._empData.LastWorkingDate, "DD-MM-YYYY").toDate();
        this._service.UpdateEmployeeStatus(this.userform.value).subscribe((data) => {
            if (data != null)
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate status updated successfully' });
            // swal("", 'Associate status updated successfully', "success");
            this._service.ReleaseTalentPool(this.userform.value.EmpId).subscribe((res: any) => {
               if(res.IsSuccessful)
               this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate Release from Talent pool.' });
            });
            this.getUsersList();
            this.clearValues();
            this.enableDate = false;
        }, (error) => {
            //swal("Failed to update the status of the associate", error._body, "error");
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });

            // this.clearValues();
        });
    }

    clearValues = function () {
        this._empData.ID = null;
        this._empData.EmpID = null;
        this._empData.IsActive = null;
        this.userform.reset();
    }

    clear = function () {
        this.formSubmitted = false;
        this.enableDate = false;
        this.clearValues();
    }

    onSubmit() {
        this.formSubmitted = true;
        if (this.userform.valid) {
            this.formSubmitted = false;
            this.updateEmployeeStatus();
        }
    }
}