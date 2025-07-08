import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { KRAStatusData } from 'src/app/modules/kra/models/krastatus.model';
import * as servicePath from 'src/app/core/service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class KraStatusService {
  _serviceUrl = environment.KRAMicroService;
  KRADefineData = new BehaviorSubject<KRAStatusData[]>([]);
  resources = servicePath.API.KRAStatus;
  resource = servicePath.API.KRAWorkFlow;
  constructor(private httpClient : HttpClient) { }



//  public getHRKRAStatuses(financialYearId: number): Observable<Array<KRAStatusData>> {
//         let url = this._serviceUrl + this.resources.getHRKRAStatuses +"/"+ financialYearId;       
//          return this.httpClient.get<Array<KRAStatusData>>(url);
//     }

    public getHRKRAStatuses(financialYearId: number): Observable<Array<KRAStatusData>> {
      let url = this._serviceUrl + this.resources.getHRKRAStatuses +"?financialYearId="+ financialYearId;       
       return this.httpClient.get<Array<KRAStatusData>>(url);
  }

public getCEOKRAStatuses(financialYearId: number): Observable<Array<KRAStatusData>> {
        let url = this._serviceUrl + this.resource.getCEOKRAStatuses +"/"+ financialYearId;       
         return this.httpClient.get<Array<KRAStatusData>>(url);
    }

 public getHODKRAStatuses(financialYearId: number,departmentId: number): Observable<Array<KRAStatusData>> {
          let url = this._serviceUrl + this.resources.getHODKRAStatuses +"/"+ financialYearId+"/"+ departmentId;       
         return this.httpClient.get<Array<KRAStatusData>>(url);
 }

//  public UpdateKRAStatus(financialYearId: number, departmentId: number , sendToHOD=true) {
//         let url ="";
//         if(sendToHOD)
//         {
//         url= this._serviceUrl + this.resources.updateKRAStatusToSendToHOD +"/"+ financialYearId+ "/" + departmentId;       
//         }
//         else url = this._serviceUrl + this.resources.sendToHR +"/"+ financialYearId+ "/" + departmentId;
//         return this.httpClient.post(url, null);
//     }
public UpdateKRAStatus(sendToHodReqObj,sendToHOD=true) {
  let url ="";
  if(sendToHOD)
  {
  // url= this._serviceUrl + this.resources.updateKRAStatusToSendToHOD +"/"+ financialYearId+ "/" + departmentId;     
  url= this._serviceUrl + this.resources.updateKRAStatusToSendToHOD        
  }
  // else url = this._serviceUrl + this.resources.sendToHR +"/"+ financialYearId+ "/" + departmentId;
  return this.httpClient.post(url, sendToHodReqObj, {responseType: 'text'});
}

 public EditByHR(financialYearId: number, departmentId: number) {
        let url ="";
        url = this._serviceUrl + this.resources.editByHR +"/"+ financialYearId+ "/" + departmentId;
        return this.httpClient.post(url, null);
    }

     public CEOAccept(financialYearId: number, departmentId: number , ceoAccept=true) {
        let url= this._serviceUrl + this.resources.ceoAccept +"/"+ financialYearId+ "/" + departmentId + "/"+ ceoAccept;       
        return this.httpClient.post(url, null);
    }

    public approveByCEO(approveByCEORequest) {
      let url= this._serviceUrl + this.resource.approvedByCEO;     
      return this.httpClient.post(url, approveByCEORequest);
  }

     public SendToCEO(financialYearId: number) {
        let url = this._serviceUrl + this.resources.sendToCEO +"/"+ financialYearId;
        return this.httpClient.post(url, null);
     }
}
