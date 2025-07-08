import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/internal/Observable';
import { ADROrganisationDevelopmentData } from '../Models/adr-organisation-development-data.model';
import { GenericType } from '../../../models/dropdowntype.model';

@Injectable({
    providedIn: 'root'
})
export class AdrOrganisationDevelopmentService {
    private _resources: any;
    private _serverURL: string;

    constructor(private _http: HttpClient) {
        this._serverURL = environment.ServerUrl;
        this._resources = servicePath.API.ADROrganisationDevelopmentEntry;
    }
    public GetADROrganisationDevelopmentDetail(EmployeeID: number,financialYearId: number): Observable<ADROrganisationDevelopmentData[]> {
        var url = this._serverURL + this._resources.get + EmployeeID + "&financialYearId=" + financialYearId;
        return this._http.get<ADROrganisationDevelopmentData[]>(url);
    }
    public getCurrentFinancialYear(): Observable<GenericType> {
        let url = this._serverURL + this._resources.getCurrentFinancialYear;
        return this._http.get<GenericType>(url);
      }
}