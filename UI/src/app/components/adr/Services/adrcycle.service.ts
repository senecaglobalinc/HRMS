import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs/Observable";
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { IADRCycle } from "../Interfaces/IADRCycle";
import { ADRCycle } from "../Models/associate-developement.review.model";

@Injectable({ providedIn: 'root' })

export class AdrcycleService implements IADRCycle {
  private _resources: any;
  private _serverURL: string;
  constructor(private _http: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.AssociateDevelopmentReview;
  }

  public GetADRCycle(): Observable<ADRCycle[]> {
      let url = this._serverURL + this._resources.GetADRCycleList;
      return this._http.get<ADRCycle[]>(url);
  }
  public UpdateADRCycle(updatedADRCycledata: ADRCycle): Observable<any> {
      let url = this._serverURL + this._resources.UpdateADRCycle;
      return this._http.post<any>(url, updatedADRCycledata);
  }

}

