import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { HttpClient } from '@angular/common/http';
import { CloneKRA } from "../../../models/clonekra.model";
import { ICloneKRAs } from "../../../interfaces/ICloneKRAs";
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';

@Injectable({
  providedIn: 'root'
})
export class CloneKRAService implements ICloneKRAs {
  private _resources: any;
  private _serverURL: string;
    constructor(private _http: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.KRA;
  }

  public CloneKRAs(cloneKRA: CloneKRA): Observable<number> {
    let _url = this._serverURL + this._resources.CloneKras;
    return this._http.post<number>(_url, cloneKRA);
  }
}
