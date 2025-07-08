import { Injectable } from '@angular/core';
import { GenericType } from '../../../models/dropdowntype.model';
import { AssociateAllocation, ReportingManager, ClientBillingRole, InternalBillingRole, RoleDetails } from '../models/associateallocation.model';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { ProjectDetails } from "../models/projects.model";

@Injectable({
    providedIn: 'root'
})
export class AssociateAllocationService {
    private _resources: any;
    private _serverURL: string;
    private _config: any
    private _projectMicroService: string;
    constructor(private _httpclient: HttpClient) {
        this._serverURL = environment.ServerUrl;
        this._resources = servicePath.API.AssociateAllocation;
        this._projectMicroService = environment.ProjMicroService;
    }

    public GetRoleDetailsByTrID(trId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._config.API.requisitionSearch.getRolesByTalentRequisitionId + trId;
        return this._httpclient.get<GenericType[]>(url);
    }

    public GetClientBillingRoles(): Observable<Array<ClientBillingRole>> {
        var url = this._serverURL + this._resources.GetClientBillingRoles;
        return this._httpclient.get<ClientBillingRole[]>(url);
    }

    public GetClientBillingRolesByProjectId(projectId: number): Observable<Array<ClientBillingRole>> {
        var url = this._projectMicroService + this._resources.GetClientBillingRolesByProjectId + projectId;
        return this._httpclient.get<ClientBillingRole[]>(url);
    }

    public GetInternalBillingRoles(): Observable<Array<InternalBillingRole>> {
        var url = this._serverURL + this._resources.GetInternalBillingRoles;
        return this._httpclient.get<InternalBillingRole[]>(url);
    }
    
    public GetEmpAllocationHistory(employeeId: number): Observable<Array<AssociateAllocation>> {
        let _url = this._projectMicroService + this._resources.GetEmpAllocationHistory + employeeId;
        return this._httpclient.get<AssociateAllocation[]>(_url);
    }

    public GetAllocationDetailsByAllocationId(allocationId: number): Observable<Array<AssociateAllocation>> {
        let _url = this._serverURL + this._resources.GetAllocationDetailsByAllocationId + allocationId;
        return this._httpclient.get<AssociateAllocation[]>(_url);
    }

    public GetEmployeePrimaryAllocationProject(employeeId: number): Observable<Array<AssociateAllocation>> {
        let _url = this._projectMicroService + this._resources.GetEmployeePrimaryAllocationProject + employeeId;
        return this._httpclient.get<AssociateAllocation[]>(_url);
    }

    public ResourceAllocate(details: AssociateAllocation) {
        let _url = this._projectMicroService + this._resources.ResourceAllocate;
        return this._httpclient.post(_url, details);
    }

    ViewSkillDetails(requisitionRoleDetailId: number) {
        let _url = this._serverURL + this._resources.GetRequiredSkillsDetails + requisitionRoleDetailId;
        return this._httpclient.get(_url);
    }

    GetMatchingProfiles(requisitionRoleId: number) {
        let _url = this._serverURL + this._resources.GetMatchingProfiles + requisitionRoleId;
        return this._httpclient.get(_url);
    }

    GetAssociateSkills(requisitionRoleId: number, associateId: number) {
        let _url = this._serverURL + this._resources.GetMatchingskillsforAssociate + requisitionRoleId + "&empId=" + associateId;
        return this._httpclient.get(_url);
    }

    ForceClose(requisitionId: number) {
        let _url = this._serverURL + this._resources.forceClose + requisitionId;
        return this._httpclient.get(_url);
    }

    CheckPrimary(empid: number, RoleId: number) {
        let _url = this._serverURL + this._resources.CheckPrimaryRoleofAssociate + empid + "&roleID=" + RoleId;
        return this._httpclient.get(_url);
    }

    SetPrimaryRole(details: any) {
        let _url = this._serverURL + this._resources.SetPrimaryRoleofAssociate;
        return this._httpclient.post(_url, details);
    }

    GetExecutionProjectsList(userRole: string, empId: number) {
        const _url = this._serverURL + this._resources.GetExecutionProjectsList + userRole + "&employeeId=" + empId;
        return this._httpclient.get<ProjectDetails[]>(_url);
      }
      GetRolesByDepartmentId(departmentId: number): Observable<RoleDetails[]> {
        let url = this._projectMicroService + this._resources.getRolesByDepartmentID + departmentId;
        return this._httpclient.get<RoleDetails[]>(url)
    }
}
