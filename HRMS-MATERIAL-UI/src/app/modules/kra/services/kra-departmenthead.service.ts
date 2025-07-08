import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { GenericType } from '../../../models/dropdowntype.model';
import { KraSetData } from '../../../models/kra.model';
import { KraRoleData } from '../../../models/kraRoleData.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';


@Injectable({
    providedIn: 'root'
})
export class KRAServiceForDepartmentHead {
    private _resources: any;
    private _serverURL: string; 
    constructor(private _http: HttpClient) {
        this._serverURL = environment.ServerUrl; 
        this._resources = servicePath.API.KraDetailsForDepartmentHead;
    }

    public GetDepartmentsById(employeeId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._resources.getDepartmentsByEmployeeId + employeeId;
        return this._http.get<Array<GenericType>>(url);
    }

    public GetKraRolesForDepartmentHead(financialYearId: number, departmentId: number): Observable<Array<KraSetData>> {
        let url = this._serverURL + this._resources.getKraRolesForDH + financialYearId + "&departmentID=" + departmentId;
        return this._http.get<Array<KraSetData>>(url);
    }

    public SendBackToHRHead(kraSetData: KraSetData) {
        let url = this._serverURL + this._resources.sendBackToHRHead;
        return this._http.post(url, kraSetData);
    }

    public CreateKraByDH(kraRoleData: KraRoleData) {
        let url = this._serverURL + this._resources.createKraByDH;
        return this._http.post(url, kraRoleData);
    }

    public UpdateKraByDH(kraRoleData: KraRoleData) {
        let url = this._serverURL + this._resources.updateKraByDH;
        return this._http.post(url, kraRoleData);
    }

    public GetKraWorkFlowPendingWithEmployee(financialYearId: number, departmentId: number): Observable<number> {
        let url = this._serverURL + this._resources.getKraWorkFlowPendingWithEmployee + financialYearId + "&departmentID=" + departmentId;
        return this._http.get<number>(url);
    }

}

 