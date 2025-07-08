import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { DefinitionModel } from "src/app/models/kradefinition.model";
import { environment } from "src/environments/environment";
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({ providedIn: 'root' })

export class KraDefinitionService {
    private _resources: any;
    private _serverUrl: string;
    constructor(private _http: HttpClient) {
      this._serverUrl = environment.KRAMicroService;
      this._resources = servicePath.API.DefineKRA;
    }

    public CreateDefinition(definitionModel: DefinitionModel) {
        let url = this._serverUrl + this._resources.createDefinition;
        return this._http.post(url, definitionModel);
    }

    public GetDefinitionDetails(Id: number): Observable<Array<DefinitionModel>> {
            let url = this._serverUrl  + this._resources.getDefinitionDetails + Id;
            return this._http.get<Array<DefinitionModel>>(url);
    }
     public UpdateKRA(definitionModel: DefinitionModel) {
        let url = this._serverUrl + this._resources.updateKRA;
        return this._http.post(url, definitionModel);
    }

    public DeleteKRA(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.deleteKRA + definitionDetailId;
        return this._http.post<number>(url, null);
    }

        public DeleteKRAByHOD(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.deleteKRAByHOD + definitionDetailId;
        return this._http.post<number>(url, null);
    }
}