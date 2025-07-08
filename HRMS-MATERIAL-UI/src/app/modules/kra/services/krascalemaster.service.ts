
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from 'src/app/core/service-paths';
import { KRAScaleMaster, KRAScaleDetails } from 'src/app/modules/kra/models/krascaleData.model';
import { IKRAScaleMaster } from "src/app/modules/kra/Interfaces/IKRAScaleMaster";

@Injectable({
  providedIn: 'root'
})
export class KrascalemasterService implements IKRAScaleMaster {
  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) { this._serverURL = environment.KRAMicroService; this._resources = servicePath.API.ScaleMaster;}

  public GetKRAScale(): Observable<Array<KRAScaleMaster>> {
    let url = this._serverURL + this._resources.getKRAScale;
    return this._http.get<Array<KRAScaleMaster>>(url);
  }
  public GetKRADescriptionDetails(kraScaleMasterId:number):Observable<Array<KRAScaleDetails>>{
    let url = this._serverURL + this._resources.getKRADescriptionDetails + kraScaleMasterId;
    return this._http.get<Array<KRAScaleDetails>>(url);
  }

  public CreateKRAScale(model: KRAScaleMaster): Observable<number> {
    let _url = this._serverURL + this._resources.createKRAScale;
    return this._http.post<number>(_url, model);
  }
  public UpdateKRAScale(model: KRAScaleMaster): Observable<number> {
    let _url = this._serverURL + this._resources.updateKRAScale;
    return this._http.put<number>(_url, model);
     
  }
  public DeleteKRAScale(kraScaleMasterId: number): Observable<number> {
   let _url = this._serverURL + this._resources.deleteKRAScale + kraScaleMasterId;
        return this._http.post<number>(_url, null);
  }

}
