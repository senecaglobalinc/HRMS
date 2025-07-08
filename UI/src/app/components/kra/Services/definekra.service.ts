import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { DefineKRAData } from '../models/definekra.model';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class DefineKraService {
  _serviceUrl = environment.KRAMicroService;
  KRADefineData = new BehaviorSubject<DefineKRAData[]>([]);
  resources = servicePath.API.DefineKRA;
  constructor(private httpClient : HttpClient ) { }

    public getKRADefinitionsById(financialYearId: number, departmentId: number, grade: number, roleTypeId: number ,IsHoD=false): Observable<Array<DefineKRAData>> {
        let url = this._serviceUrl + this.resources.getDefinitionsById + financialYearId + "&departmentId=" + departmentId
        + "&gradeId=" + grade+ "&roleTypeId=" + roleTypeId+ "&IsHOD=" + IsHoD;       
         return this.httpClient.get<Array<DefineKRAData>>(url);
    }

}
