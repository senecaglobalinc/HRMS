import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import * as servicePath from 'src/app/core/service-paths';
import { Observable } from 'rxjs/Observable';
//import { FinancialYearRoletype } from 'src/app/modules/kra/models/financialyear-roletype.model';
import { FinancialYearRoleType } from '../models/financialyear-roletype.model';
import { FileDetail } from '../models/file-detail.model';


@Injectable({
  providedIn: 'root'
})
export class DownloadKraService { 
  _serviceUrl = environment.EmployeeMicroService; 
  resources = servicePath.API.KRA; 
  constructor(private httpClient : HttpClient) { }

  public getEmployeeRoleTypes(employeeId: number): Observable<Array<FinancialYearRoleType>> {
    let url = this._serviceUrl + this.resources.getEmployeeRoleTypes +"?employeeId="+ employeeId;       
     return this.httpClient.get<Array<FinancialYearRoleType>>(url);
}


public downloadKRA(employeeCode: string, financialYear : string, roleType: string): Observable<FileDetail> {
  let url = this._serviceUrl + this.resources.downloadKRA +"?employeeCode="+ employeeCode +"&financialYear="+ financialYear +"&roleType="+ roleType ;       
   return this.httpClient.get<FileDetail>(url);
}

}
