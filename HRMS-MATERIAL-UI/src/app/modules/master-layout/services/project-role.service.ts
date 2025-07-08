import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../../app/core/service-paths';

@Injectable({
    providedIn: 'root'
})
export class ProjectRoleService {
    private _serverURL: string;
    private _resources: any;
    private _roleInstance: any;
    private _config: any;
    private _menuInstance: any;

    constructor(private _http: HttpClient) {
        this._serverURL = environmentInformation.environment.ServerUrl;
        this._resources = servicePath.API.Roles;
        this._menuInstance = servicePath.API.Menu;
        this._roleInstance = servicePath.API.ProjectRoleAllocation;
    }
    GetRoleDataById(RoleId: number, ProjectId: number) {
        const url = this._serverURL + this._roleInstance.GetRoleDataById + RoleId + '&projectId=' + ProjectId;

        return this._http.get(url);
    }

    GetAssignedRoles(ProjectId: number) {
        const url = this._serverURL + this._roleInstance.GetAssignedRoles + ProjectId;

        return this._http.get(url);
    }

    AssignRole(projectRoleData: any) {
        const url = this._serverURL + this._roleInstance.AssignProjectRole;

        return this._http.post(url, projectRoleData);

    }

     GetRoleNotifications(employeeId: Number) {
            const url = this._serverURL + this._roleInstance.GetRoleNotifications + employeeId;

            return this._http.get(url);
        }


    RoleApproval(projectRoleData: any) {
        const url = this._serverURL + this._roleInstance.ApproveRole;

        return this._http.post(url, projectRoleData);
    }

    RoleRejection(projectRoleData: any) {
        const url = this._serverURL + this._roleInstance.RejectRole;

        return this._http.post(url, projectRoleData);

    }

    GetProjectManagerOrLeadId(EmployeeId: number) {
        const url = this._serverURL + this._roleInstance.GetProjectManagerOrLeadId + EmployeeId;

        return this._http.get(url);
    }
    getMenuDetails(roleName: string) {
            const _url = this._serverURL + '/Menu/GetMenuDetails?roleName=' + roleName;
            return  this._http.get(_url);
    }
}
