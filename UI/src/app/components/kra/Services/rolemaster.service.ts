import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
//import { Util } from 'app/utility/util';
import 'rxjs/Rx';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RolemasterService {

  private _resources: any;
  private _serverURL: string;
   

    constructor(private _http: HttpClient) { this._serverURL = environment.ServerUrl;
      this._resources = servicePath.API.Roles;}

      public GetLoggedInUserRoles(employeeId: number, finacialYearId: number) {
        let url = this._serverURL + this._resources.Roles.GetLoggedInUserRoles + employeeId + "&finanicalYearId=" + finacialYearId;
        return this._http.get(url);
    }
}
