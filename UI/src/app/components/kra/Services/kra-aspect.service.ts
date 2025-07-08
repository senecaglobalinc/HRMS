import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable'
import 'rxjs/Rx';
import { AspectData, KraAspectData } from 'src/app/models/kra.model';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { FormGroup } from '@angular/forms';
import { DepartmentDetails } from 'src/app/models/role.model';

@Injectable({
  providedIn: 'root'
})
export class KraAspectService {
  private _resources: any;
  private _serverURL: string;

  constructor(private _http: HttpClient) {
    this._serverURL = environment.KRAMicroService;
    this._resources = servicePath.API.KraAspect;
  }

  public getKraAspects(departmentId: number): Observable<Array<AspectData>> {
    var url = this._serverURL + this._resources.getKraAspects + departmentId;
    return this._http.get<Array<AspectData>>(url)
  }

  GetDepartments(): Observable<DepartmentDetails[]> {
    var url = this._serverURL + this._resources.getDepartmentsList;
    return this._http.get<DepartmentDetails[]>(url);

  }
  public createKraAspect(data) {
    let _url = this._serverURL + this._resources.creatKraAspect + '/' + data.departmentId;
    return this._http.post(_url, data.aspectIds);

  }

  public updateKraSet(kraAspectData: AspectData) {
    let _url = this._serverURL + this._resources.updateKraAspect;// + kraAspectData;
    return this._http.post(_url, kraAspectData);

  }

  public DeleteKraAspect(aspectData: AspectData) {
    let _url = this._serverURL + this._resources.deleteKRAAspect;
    return this._http.post(_url, aspectData);
  }
}