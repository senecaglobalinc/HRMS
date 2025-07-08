import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/internal/Observable';
import { ADROrganisationValueData } from '../Models/adr-organisation-value-data.model';
import { GenericType } from '../../../models/dropdowntype.model';

@Injectable({
  providedIn: 'root'
})
export class AdrOrganisationValueService {

  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) {
   this._serverURL = environment.ServerUrl; 
   this._resources = servicePath.API.ADROrganisationValue;
  }

  public GetADROrganisationValues(financialYearId:number): Observable<ADROrganisationValueData[]> {
    var url = this._serverURL + this._resources.get + financialYearId;
    return this._http.get<ADROrganisationValueData[]>(url);
  }

  public CreateADROrganisationValue(orgValueData: ADROrganisationValueData): Observable<number> {
    let _url = this._serverURL + this._resources.create;
    return this._http.post<number>(_url, orgValueData);
  }

  public UpdateADROrganisationValue(orgValueData: ADROrganisationValueData,financialYearId:number): Observable<number> {
    let _url = this._serverURL + this._resources.update + financialYearId;
    return this._http.post<number>(_url, orgValueData);
  }
  
  public getCurrentFinancialYear(): Observable<GenericType> {
    let url = this._serverURL + this._resources.getCurrentFinancialYear;
    return this._http.get<GenericType>(url);
  
  }
}
