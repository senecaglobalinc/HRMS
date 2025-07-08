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
export class AdrOrganisationDevelopmentMasterService {

  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) {
   this._serverURL = environment.ServerUrl; 
   this._resources = servicePath.API.ADROrganisationDevelopment;
  }

  public GetADROrganisationDevelopment(financialYearId:number): Observable<ADROrganisationDevelopmentData[]> {
    var url = this._serverURL + this._resources.get  + financialYearId;
    return this._http.get<ADROrganisationDevelopmentData[]>(url);
  }

  public CreateADROrganisationDevelopment(orgDevData: ADROrganisationDevelopmentData): Observable<number> {
    let _url = this._serverURL + this._resources.create;
    return this._http.post<number>(_url, orgDevData);
  }

  public UpdateADROrganisationDevelopment(orgDevData: ADROrganisationDevelopmentData,financialYearId:number): Observable<number> {
    let _url = this._serverURL + this._resources.update + financialYearId;
    return this._http.post<number>(_url, orgDevData);
  }
 
  public getCurrentFinancialYear(): Observable<GenericType> {
    let url = this._serverURL + this._resources.getCurrentFinancialYear;
    return this._http.get<GenericType>(url);
  }

}
