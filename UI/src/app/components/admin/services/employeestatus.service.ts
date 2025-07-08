import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
//import 'rxjs/Rx';
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';

@Injectable()
export class EmployeeStatusService {

    private _resources: any;
    private EmployeeMicroService: any;
    private projectService: string;
    constructor(private httpClient: HttpClient) {
        this._resources = servicePath.API.EmployeeStatus;
        this.EmployeeMicroService = environmentInformation.environment.EmployeeMicroService;
        this.projectService = environmentInformation.environment.ProjMicroService;
    }

    GetAssociateNames() {
        let url = this.EmployeeMicroService + this._resources.GetNames;
        return this.httpClient.get(url);
    }

    GetAssociates() {
        let url = this.EmployeeMicroService + this._resources.GetUsers;
        return this.httpClient.get(url);
    }

    GetResignStatus() {
        let url = this.EmployeeMicroService + this._resources.GetResignStatus;
        return this.httpClient.get(url);
    }

    UpdateEmployeeStatus(empData: any) {
        let url = this.EmployeeMicroService + this._resources.UpdateEmployeeStatus;
        return this.httpClient.post(url, empData)

    }

    MapAssociateId(empData: any) {
        let url = this.EmployeeMicroService + this._resources.MapAssociateId;
        return this.httpClient.post(url, empData)
    }

    ReleaseTalentPool(employeeId: number) {
        let url = this.projectService + this._resources.ReleaseTalentPool + employeeId;
        return this.httpClient.post(url, '')
    }
}