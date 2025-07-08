import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { ClientBillingRoleDetails } from '../models/client-billing-role.model';

@Injectable({
    providedIn: 'root'
})
export class ClientBillingRoleService {
    serviceUrl = environment.ServerUrl;
    _projectMicroService = environment.ProjMicroService;
    resources = servicePath.API.ClientBillingRole;
    constructor(private httpClient: HttpClient) { }

    public SaveClientBillingRole(clientBillingRoleData: ClientBillingRoleDetails) {
        let url = this._projectMicroService + this.resources.create;
        return this.httpClient.post(url, clientBillingRoleData);
    }

    public UpdateClientBillingRole(clientBillingRoleData: ClientBillingRoleDetails) {
        let url = this._projectMicroService + this.resources.update;
        return this.httpClient.post(url, clientBillingRoleData);
    }

    public GetClientBillingRolesByProjectId(projectId: number) {
        var url = this._projectMicroService + this.resources.getClientBillingRolesByProjectId + projectId;
        return this.httpClient.get(url);
    }

    public DeleteClientBillingRole(clientBillingRoleId: number) {
        let url = this._projectMicroService + this.resources.delete + clientBillingRoleId;
        return this.httpClient.post(url, null);
    }
    public closeClientBillingRecord(cBRId : number , endDate ){
        let url = this._projectMicroService + this.resources.CloseClientBillingRole + cBRId + "/" + endDate;
        
        return this.httpClient.get(url);
    }


}
