import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/internal/Observable';
import { ADRSections } from '../Models/adr-sections.model';

@Injectable({
  providedIn: 'root'
})
export class AdrSectionsService {

  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) {
   this._serverURL = environment.ServerUrl; 
   this._resources = servicePath.API.ADRSections;
  }

  public GetADRSections(): Observable<ADRSections[]> {
    var url = this._serverURL + this._resources.get;
    return this._http.get<ADRSections[]>(url);
  }

  public GetADRMeasurementAreas() : Observable<ADRSections[]> {
    var url = this._serverURL + this._resources.getADRMeasurementAreas;
    return this._http.get<ADRSections[]>(url);
  }

  public CreateADRSections(sections : ADRSections): Observable<number> {
    let _url = this._serverURL + this._resources.create;
    return this._http.post<number>(_url, sections);
  }

  public UpdateADRSections(sections: ADRSections): Observable<number> {
    let _url = this._serverURL + this._resources.update;
    return this._http.post<number>(_url, sections);
  }
}
