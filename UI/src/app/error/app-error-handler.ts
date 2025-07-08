import { ErrorHandler, Injectable } from "@angular/core";
import * as environmentInformation from '../../environments/environment';
import * as servicePath from '../service-paths';
import { HttpClient } from "@angular/common/http";
@Injectable()
export class AppErrorHandler implements ErrorHandler {
    private _resources: any;
    private _serverURL: string;
    constructor(private _http: HttpClient) {
        this._serverURL = environmentInformation.environment.ServerUrl;
        this._resources = servicePath.API;
    }
    handleError(error: any): void {
        let url = this._serverURL + this._resources.logError;
        let errorObject = {
            "FileName": "globalError",
            "ErrorMessage": error.message,
        }
        this._http.post(url, errorObject).subscribe(res => {

        });
    }
}