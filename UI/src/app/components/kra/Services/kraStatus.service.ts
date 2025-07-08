import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { KRAStatusData } from '../models/kraStatus.model';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class KraStatusService {
  _serviceUrl = environment.KRAMicroService;
  KRADefineData = new BehaviorSubject<KRAStatusData[]>([]);
  resources = servicePath.API.KRAStatus;
  constructor(private httpClient : HttpClient ) { }


 public getHRKRAStatuses(financialYearId: number): Observable<Array<KRAStatusData>> {
        let url = this._serviceUrl + this.resources.getHRKRAStatuses +"/"+ financialYearId;       
         return this.httpClient.get<Array<KRAStatusData>>(url);
    }

 public getHODKRAStatuses(financialYearId: number,departmentId: number): Observable<Array<KRAStatusData>> {
          let url = this._serviceUrl + this.resources.getHODKRAStatuses +"/"+ financialYearId+"/"+ departmentId;       
         return this.httpClient.get<Array<KRAStatusData>>(url);
 }

 public UpdateKRAStatus(financialYearId: number, departmentId: number , sendToHOD=true) {
        let url ="";
        if(sendToHOD)
        {
        url= this._serviceUrl + this.resources.updateKRAStatusToSendToHOD +"/"+ financialYearId+ "/" + departmentId;
        }
        else url = this._serviceUrl + this.resources.sendToHR +"/"+ financialYearId+ "/" + departmentId;
        return this.httpClient.post(url, null);
    }
     public SendToCEO(financialYearId: number) {
        let url = this._serviceUrl + this.resources.sendToCEO +"/"+ financialYearId;
        return this.httpClient.post(url, null);
     }
}
