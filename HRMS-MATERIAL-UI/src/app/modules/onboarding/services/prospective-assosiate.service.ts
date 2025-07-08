import { Injectable } from '@angular/core';
import { Associate } from '../models/associate.model';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProspectiveAssosiateService {
  // private _serverURL: string;
  private _resources: any;
  private _assosiateresources:any;
  private EmployeeMicroService : any;
  private _serviceUrl = environment.ServerUrl;
  constructor(private httpClient: HttpClient) {
    this._resources = servicePath.API.PAssociate;
    this._assosiateresources = servicePath.API.associates;  
    this.EmployeeMicroService = environment.EmployeeMicroService;                      
  }

  public list(): Observable<Associate> {      
      var url = this.EmployeeMicroService + this._resources.list;

      return this.httpClient.get<Associate>(url);
  }

     public GetAssociateDetailslist(): Observable<Associate> {      
      var url = this._serviceUrl + this._resources.getAssociateDetails;

      return this.httpClient.get<Associate>(url);
  }

     public GetAssociateDetailsByEmpID(empID: number, roleName: string): Observable<Associate> {
         var url = this._serviceUrl + this._resources.getAssociateDetailsByEmpID + empID + "&roleName=" + roleName;
         return this.httpClient.get<Associate>(url);
     }
     

  public DeletePA(empID:number,reason:string){
      var url=this._serviceUrl+this._resources.deletePA+empID+"&reason="+reason        
      return this.httpClient.get(url);
  }

  getPADetailsById(id:number) {        
    let _url = this.EmployeeMicroService + this._resources.get+id;
    return this.httpClient.get(_url);         
}

  public GetJoinedAssoicates(){
      var url = this._serviceUrl + this._resources.getJoinedAssociates;

      return this.httpClient.get(url);
  }

  UpdatePADetails(newAssociate:Associate) {        
    let _url = this.EmployeeMicroService + this._resources.update;
      return this.httpClient.post(_url, newAssociate);
        
}
}

