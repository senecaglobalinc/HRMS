import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { AssociateRoleMappingData } from 'src/app/models/associaterolemappingdata.model';
import { GenericType } from 'src/app/models/dropdowntype.model';
import { IMapAssociateRole } from 'src/app/Interfaces/IMapAssociateRole';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths'; 

@Injectable({ providedIn: 'root' })

export class MapAssociateRoleService implements IMapAssociateRole{
    private _resources: any;
    private _serverUrl: string;
    constructor(private http: HttpClient) {
    this._serverUrl= environment.ServerUrl;
    this._resources = servicePath.API.MapAssociateRole; 
    }

    public GetEmployeesByDepartmentIdAndProjectId(departmentId:number, projectId: number, isNew:boolean):Observable<AssociateRoleMappingData[]> {
        let url = this._serverUrl + this._resources.getEmployeesByProjectID + departmentId + "&projectId=" + projectId + "&isNew=" + isNew;
        return this.http.get<AssociateRoleMappingData[]>(url);
    }

    public GetKraRolesByDepartmentId(DepartmentId:number):Observable<GenericType[]> {
        let url = this._serverUrl + this._resources.getKraRolesByDepartmentId + DepartmentId;
        return this.http.get<GenericType[]>(url);
    }

    public MapAssociateRole(associateRoleMappingData: AssociateRoleMappingData):Observable<number> {
        let url = this._serverUrl +this._resources.mapRole;
        return this.http.post<number>(url, associateRoleMappingData);     
    }

    public GetEmployeesByKraRoleIdAndFinancialYearId(financialYearId:number, KraRoleId: number, isNew:boolean):Observable<AssociateRoleMappingData[]> {
        let url = this._serverUrl + this._resources.getEmployeesByKraRoleIdAndFinancialYearId + KraRoleId + "&financialYearId=" + financialYearId;
        return this.http.get<AssociateRoleMappingData[]>(url);
    }

}