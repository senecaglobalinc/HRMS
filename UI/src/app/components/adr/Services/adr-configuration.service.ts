import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/internal/Observable';
import { ADRConfiguration } from '../Models/adr-configuration.model';

@Injectable({
  providedIn: 'root'
})
export class AdrConfigurationService {

  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) {
   this._serverURL = environment.ServerUrl; 
   this._resources = servicePath.API.ADRConfiguration;
  }

  public GetADRConfiguration(): Observable<ADRConfiguration[]> {
    var url = this._serverURL + this._resources.get;
    return this._http.get<ADRConfiguration[]>(url);
  }

  public CreateADRConfiguration(configData: ADRConfiguration): Observable<number> {
    let _url = this._serverURL + this._resources.create;
    return this._http.post<number>(_url, configData);
  }

  public UpdateADRConfiguration(configData: ADRConfiguration): Observable<number> {
    let _url = this._serverURL + this._resources.update;
    return this._http.post<number>(_url, configData);
  }
}
