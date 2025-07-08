import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { GenericType } from 'src/app/models/dropdowntype.model';
import { OrganizationKras, AssociateKras, CustomKras } from 'src/app/models/associate-kras.model';

@Injectable({ providedIn: 'root' })

export class CustomKRAService {
    private _resources: any;
    private _serverURL: string;
    constructor(private _http: HttpClient) {
      this._serverURL = environment.ServerUrl;
      this._resources = servicePath.API.CustomKras;
    }

    public GetProjectsByProgramManagerId(employeeId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._resources.getProjectsByProgramManagerId + employeeId;
        return this._http.get<Array<GenericType>>(url);    
    }

    public GetEmployeesByProjectId(projectId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._resources.getEmployeesByProjectId + projectId;
        return this._http.get<Array<GenericType>>(url); 
    }

    public GetEmployeesByDepartment(employeeId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._resources.getEmployeesByDepartment + employeeId;
        return this._http.get<Array<GenericType>>(url); 
    }

    public SaveCustomKras(customKras: CustomKras) {
        let url = this._serverURL + this._resources.saveCustomKras;
        return this._http.post(url, customKras); 
    }

    public EditCustomKras(customKras: CustomKras) {
        let url = this._serverURL + this._resources.editCustomKras;
        return this._http.post(url, customKras); 
    }

    public DeleteCustomKra(customKraId: number) {
        let url = this._serverURL + this._resources.deleteCustomKra + customKraId;
        return this._http.post(url, customKraId); 
    }

    public GetOrganizationAndCustomKrasOfEmployee(employeeId: number, financialYearId: number): Observable<AssociateKras> {
        let url = this._serverURL + this._resources.getOrganizationAndCustomKrasOfEmployee + employeeId + "&financialYearID=" + financialYearId;
        return this._http.get<AssociateKras>(url); 
    }

}

