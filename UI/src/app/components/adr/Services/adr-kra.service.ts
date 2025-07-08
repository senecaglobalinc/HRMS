import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { ADRData } from '../Models/adr-kra.model';

@Injectable({
  providedIn: 'root'
})

export class AdrKraService {
  private _resources: any;
  private _serverUrl: string;

  constructor(private http: HttpClient) {
    this._serverUrl = environment.ServerUrl;
    this._resources = servicePath.API.ADRKra;
  }

  public GetAssociateADRDetail(EmployeeID: number, FinancialYearID: number, AdrCycleId: number, ): Observable<ADRData[]> {
    let url = this._serverUrl + this._resources.GetAssociateADRDetail + EmployeeID + "&financialYearID=" + FinancialYearID + "&adrCycleId=" + AdrCycleId;
    return this.http.get<ADRData[]>(url);
  }
  
}
