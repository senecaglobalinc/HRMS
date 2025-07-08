import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { HttpClient } from '@angular/common/http';
import { KRAMeasurementTypeData } from 'src/app/modules/kra/models/kra.measurement-type.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from 'src/app/core/service-paths';

@Injectable({ providedIn: 'root' })

export class KraMeasurementTypeService {
    private _resources: any;
    private _serverUrl: string;
    constructor(private _http: HttpClient) {
      this._serverUrl = environment.KRAMicroService;
      this._resources = servicePath.API.MeasurementType;
    }

    GetKRAMeasurementType(): Observable<KRAMeasurementTypeData[]> {      
        var url = this._serverUrl + this._resources.getMeasurementType;
        return this._http.get<KRAMeasurementTypeData[]>(url);
    }

    public DeleteKraMeasurementType(MeasurementTypeId: number) {
        let url = this._serverUrl + this._resources.deleteMeasurementType + MeasurementTypeId;
        return this._http.post(url, MeasurementTypeId); 
    }

    public CreateKraMeasurementType(MeasurementType: string) {
        
        let url = this._serverUrl + this._resources.createMeasurementType;
        let model = { MeasurementType: MeasurementType };
        return this._http.post<boolean>(url, model); 
    }

    public UpdateKraMeasurementType(kraMeasurementTypeData: KRAMeasurementTypeData): Observable<boolean> {
        
        let url = this._serverUrl + this._resources.updateMeasurementType;
        return this._http.put<boolean>(url, kraMeasurementTypeData);
    }
}