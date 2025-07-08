import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root'
})
export class ProjectdetailsService {

  private _resources: any;
  private _reportURL: string;

  constructor(private httpClient: HttpClient) {
      this._reportURL = environment.ProjMicroService;
      this._resources = servicePath.API.Reports;
  }

  public GetProjectDetailsForReport() {
    var url = this._reportURL + this._resources.GetProjectDetailsReport;
    return this.httpClient.get(url);
  }
}
