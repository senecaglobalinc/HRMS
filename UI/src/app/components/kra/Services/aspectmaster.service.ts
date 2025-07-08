import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { AspectData } from "../../../models/kra.model";
import { IAspectMaster } from "../../../Interfaces/IAspectMaster";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { JsonPipe } from "@angular/common";

@Injectable({
  providedIn: 'root'
})

export class AspectMasterService implements IAspectMaster {
  private _resources: any;
  private _serverURL: string;
  constructor(private _http: HttpClient) { this._serverURL = environment.KRAMicroService; this._resources = servicePath.API.AspectMaster; }
  public GetAspectMasterList(): Observable<AspectData[]> {
    var url = this._serverURL + this._resources.GetAspects;
    return this._http.get<AspectData[]>(url);
  }

  public CreateAspect(aspectName: string) {
    let _url = this._serverURL + this._resources.CreateAspect;
    let model = { AspectName: aspectName };
    return this._http.post<boolean>(_url, model);
  }

  public UpdateAspect(model: AspectData): Observable<boolean> {
    let _url = this._serverURL + this._resources.UpdateAspect;
    return this._http.put<boolean>(_url, model);
  }

  public DeleteAspect(aspectId: number): Observable<number> {
    let _url = this._serverURL + this._resources.DeleteAspect + aspectId;
    return this._http.post<number>(_url, aspectId);
  }

}
