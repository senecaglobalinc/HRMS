import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { HttpClient } from "@angular/common/http";
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { WorkStation, WorkStationDataCount, BayInformation } from "src/app/modules/master-layout/models/workstation.model";


@Injectable({
  providedIn: 'root'
})
export class WorkstationService {
  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) { this._serverURL = environment.ServerUrl; 
    this._resources = servicePath.API.WorkStation;
  }
  
  public GetWorkStationDataCount(id: number): Observable<Array<WorkStationDataCount>> {
    let _url = this._serverURL + this._resources.GetWorkStationDataCount + id;
    return this._http.get<Array<WorkStationDataCount>>(_url);
  }
  public GetBayDetails(): Observable<Array<BayInformation>> {
    let _url = this._serverURL + this._resources.GetBaysList;
    return this._http.get<Array<BayInformation>>(_url);
  }
  public GetWorkStationDetails(id: number): Observable<Array<WorkStation>> {
    let _url = this._serverURL + this._resources.GetWorkStationListByBayId + id;
    return this._http.get<Array<WorkStation>>(_url);
  }
  public GetWorkStationDetailByWorkStationId(workStationCode:string):Observable<Array<WorkStation>>{
    let url=this._serverURL+this._resources.GetWorkStationDetailByWorkStationCode+workStationCode;
    return this._http.get<Array<WorkStation>>(url);
  }
  public DeskAllocation(employeeId:number,workStationId:string){
    let url=this._serverURL+this._resources.DeskAllocation+employeeId +"&workStationId=" + workStationId ;
    return this._http.post(url,employeeId);
  }
  public ReleaseDesk(employeeId:number,workStationId:number){
    let url=this._serverURL+this._resources.ReleaseDesk+employeeId +"&workStationId=" + workStationId ;
    return this._http.post(url,employeeId);
  }
}
