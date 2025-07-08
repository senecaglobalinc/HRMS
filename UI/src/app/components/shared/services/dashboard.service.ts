import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { SkillsData } from '../../../models/project-role.model';
import { ResponseContentType } from '@angular/http';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private _roleName: string;
    private _userName: string;
    private _serverURL: string;
    private _hrm: string;

    public notificationCount: number;
    _employeeServerURL: string;
    _resources: any;
    constructor(private httpClient: HttpClient) {
        let loginData: any = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]);
        this._roleName = loginData.roleName;
        this._userName = loginData.email;
        this._serverURL = environment.ServerUrl;
        this._employeeServerURL = environment.EmployeeMicroService;
        this._resources = servicePath.API.Dashboard;
    }



    getDashboardNotificationCount() {
        let _url = this._serverURL;
        switch (this._roleName) {
            case 'HRM': {
                _url += "/SystemNotification/GetNotificationCount?notificationTypeId=2";
                break;
            }
            case 'RM': {
                _url += "/SystemNotification/GetNotificationCount?notificationTypeId=2";
                break;
            }
            case 'ITHead': {
                _url += "/SystemNotification/CountNotifications";
                break;
            }
            case 'FinanceHead': {
                _url += "/SystemNotification/CountFinanceNotifications";
                break;
            }
            case 'AdminHead': {
                _url += "/SystemNotification/CountAdminNotifications";
                break;
            }
            case 'HRA': {
                _url += "/SystemNotification/GetHRANotificationCount?notificationTypeId=2";
                break;
            }
            case 'SystemAdmin': {
                _url += "/SystemNotification/CountAdminNotifications";
                break;
            }
            case 'Program Manager': {
                _url += "/ProjectRoleAssignment/CountRoleNotifications";
                break;
            }
            default: {
                _url += "/KRA/GetAssociateKRADetails?emailId=" + this._userName;
                break;
            }
        }
        return this.httpClient.get(_url);
    }

    getHRHeadDetails() {
        let _url = this._employeeServerURL + this._resources.getPendingProfiles;
        return this.httpClient.get(_url)
    }

    getSkillDetails() {
        let _url = this._serverURL + "/Dashboard/GetSkillDetails";
        return this.httpClient.get(_url)
    }

    public CreateEmployeeSkill(skillsData: SkillsData) {
        let _url = this._serverURL + "/Dashboard/CreateEmployeeSkill";
        return this.httpClient.post(_url, skillsData);
    }

    getAssociateResignationDetails() {
        let _url = this._serverURL + "/AssociateResignation/ResignationDashboard";
        return this.httpClient.get(_url)
    }

    getFinanceNotifications() {
        let _url = this._serverURL + "/SystemNotification/GetFinanceNotifications";

        return this.httpClient.get(_url)
    }

    getItNotifications() {
        let _url = this._serverURL + "/SystemNotification/GetNotifications";
        return this.httpClient.get(_url)

    }
    getAdminNotifications() {
        let _url = this._serverURL + "/SystemNotification/GetAdminNotifications";
        return this.httpClient.get(_url)

    }
    getHRANotifications() {
        let _url = this._serverURL + "/Dashboard/GetHRADetails";

        return this.httpClient.get(_url)
    }
    getAssociateKRAs(userName: string) {
        let _url = this._serverURL + "/KRA/GetAssociateKRADetails?emailId=" + userName

        return this.httpClient.get(_url)
    }
    updateItStatus(empCode: string, taskId: string) {
        let _url = this._serverURL + "/SystemNotification/UpdateItNotification?employeeCode=" + empCode + "&taskId=" + taskId;
        return this.httpClient.get(_url)
    }
    updateFinanceStatus(empCode: string, taskId: string) {
        let _url = this._serverURL + "/SystemNotification/UpdateFinanceNotification?employeeCode=" + empCode + "&taskId=" + taskId
        return this.httpClient.get(_url)
    }
    updateAdminStatus(empCode: string, taskId: string) {
        let _url = this._serverURL + "/SystemNotification/UpdateAdminNotification?employeeCode=" + empCode + "&taskId=" + taskId;
        return this.httpClient.get(_url)
    }
    generatePDF(id: number) {
        let _url = this._employeeServerURL + "/EmployeeFiles/GeneratePDFReport/" + id;
        const httpOptions = {
            'responseType': 'arraybuffer' as 'json'
        };
        return this.httpClient.get<ArrayBuffer>(_url, httpOptions);
    }

    profileApproval(empId: number, reqtype: string, reason: string) {
        let _url = this._employeeServerURL + servicePath.API.PAssociate.profileApproval;
        let details = {
            EmpId: empId,
            Status: reqtype,
            Remarks: reason
        };
        return this.httpClient.post(_url, details)
    }

    public GetAssociateCodeByEmpID(empId: number) {
        var url = this._employeeServerURL + "/Employee/GetById/" + empId
        return this.httpClient.get(url);
    }
}
