import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import { FinancialYear } from "../../../models/financialyear.model";
import { IFinancialYear } from "../../../interfaces/IFinanacialYear";
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { of } from "rxjs";


export class MockFinancialService{
  public GetFinancialYearList(){
    return of();
  }

  public GetCurrentFinancialYear() {  return of();}
  public CreateFinancialYear(financialYear: FinancialYear){  return of();}

  public UpdateFinancialYear(financialYear: FinancialYear){  return of();}

}


@Injectable({ providedIn: 'root' })

export class FinancialYearService implements IFinancialYear {
  private _resources: any;
  private _serverURL: string;
  constructor(private _http: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.FinancialYears;
  }

  public GetFinancialYearList(): Observable<FinancialYear[]> {
    var url = this._serverURL + this._resources.GetFinancialYearList;
    return this._http.get<FinancialYear[]>(url);
  }

  public GetCurrentFinancialYear(): Observable<number> {
    var url = this._serverURL + this._resources.GetCurrentFinancialYear;
    return this._http.get<number>(url);
  }

  public CreateFinancialYear(financialYear: FinancialYear): Observable<number> {
    let _url = this._serverURL + this._resources.CreateFinancialYear;
    return this._http.post<number>(_url, financialYear);
  }

  public UpdateFinancialYear(financialYear: FinancialYear): Observable<number> {
    let _url = this._serverURL + this._resources.UpdateFinancialYear;
    return this._http.post<number>(_url, financialYear);
  }
}
