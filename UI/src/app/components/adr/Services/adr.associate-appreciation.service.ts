import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Appreciation, ADRCycle } from '../Models/associate-developement.review.model';
import { GenericType } from 'src/app/models/dropdowntype.model';
import { IAssociateAppreciation } from '../Interfaces/IAssociateAppreciation';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths'; 

@Injectable()
export class AssociateAppreciationService implements IAssociateAppreciation {
    private _resources: any;
    private _serverURL: string;
  
    constructor(private http: HttpClient) {
      this._serverURL = environment.ServerUrl;
      this._resources = servicePath.API.ADRAssociateAppreciation;
    }

    public GetSentAppreciationsList(employeeID: number): Observable<Appreciation[]> {
        let url = this._serverURL + this._resources.GetSentAppreciationsList + employeeID;
        return this.http.get<Appreciation[]>(url);
    }

    public GetReceiveAppreciationsList(employeeID: number): Observable<Appreciation[]> {
        let url = this._serverURL + this._resources.GetReceiveAppreciationsList + employeeID;
        return this.http.get<Appreciation[]>(url);
    }

    GetADRCycleList(): Observable<ADRCycle[]> {
      //  let url = this._serverURL + this._resources.GetADRCycleList;
        return this.http.get<ADRCycle[]>(`${environment.ServerUrl}/${servicePath.API.ADRAssociateAppreciation.GetADRCycleList}`);
    }

    public GetFinancialYearList(): Observable<GenericType[]> {
        let url = this._serverURL + this._resources.GetFinancialYearList;
        return this.http.get<GenericType[]>(url);
    }

    public GetAppreciationTypeList(): Observable<GenericType[]> {
        let url = this._serverURL + this._resources.GetAppreciationTypeList;
        return this.http.get<GenericType[]>(url);
    }

    getSourceOfOriginList(): Observable<Appreciation[]> {
        return this.http.get<Appreciation[]>(`${environment.ServerUrl}/${servicePath.API.ADRAssociateAppreciation.GetSourceOfOriginList}`);
    }

    public SendAnAppreciation(appreciation: Appreciation): Observable<boolean> {
        let url = this._serverURL + this._resources.SendAnAppreciation;
        return this.http.post<boolean>(url, appreciation);
    }

    public UpdateAnAppreciation(appreciation: Appreciation): Observable<boolean> {
        let url = this._serverURL + this._resources.UpdateAnAppreciation;
        return this.http.post<boolean>(url, appreciation);
    }
    
    public DeleteAnAppreciation(rowIdForDelete: number): Observable<boolean> {
        let url = this._serverURL + this._resources.SendAnAppreciation;
        return this.http.post<boolean>(url, rowIdForDelete);
    }
}