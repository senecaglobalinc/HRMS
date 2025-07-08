import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { NotificationConfiguration  } from '../models/notificationconfiguration.model';
import * as environmentInformation from '../../../../environments/environment';
import { INotificationConfiguration } from '../../../Interfaces/INotificationConfiguration';
import * as servicePath from '../../../service-paths';
import { of } from 'rxjs';
import { BehaviorSubject } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class NotificationConfigurationService implements INotificationConfiguration {
    subscribe(arg0: (res: any) => void): any {
        throw new Error("Method not implemented.");
    }
    editMode = false;
    notificationTypeData = new BehaviorSubject<NotificationConfiguration[]>([]);
    editObj = new BehaviorSubject<NotificationConfiguration>(new NotificationConfiguration());
    private _serverURL: string;
    private _resources: any;
    constructor(private _http: HttpClient) {
        this._serverURL = environmentInformation.environment.AdminMicroService;
        this._resources = servicePath.API.NotificationConfiguration;
        //         this._resources = servicePath.API.Roles;   
    }
    public GetNotificationCofigurationByNotificationType(notificationTypeId: number, categoryMasterId: number): Observable<NotificationConfiguration> {
        let url = this._serverURL + this._resources.GetNotificationCofigurationByNotificationType + notificationTypeId
            + "&categoryMasterId=" + categoryMasterId;
        return this._http.get<NotificationConfiguration>(url);
    }

    public SaveNotificationCofiguration(NotificationCofigurationDetails: NotificationConfiguration): Observable<boolean> {
        let _url = this._serverURL + this._resources.SaveNotificationCofiguration;
        return this._http.post<boolean>(_url, NotificationCofigurationDetails);
        // .map(res => res.json())
       
    }

    public UpdateNotificationCofiguration(NotificationCofigurationDetails: NotificationConfiguration): Observable<boolean> {
        let _url = this._serverURL + this._resources.UpdateNotificationCofiguration;
        return this._http.post<boolean>(_url, NotificationCofigurationDetails);
        // .map(res => res.json())
       
    }

    public GetFromEmail():Observable<string>{
        let email =this._resources.FromEmail;
        return of(email);
    }

    GetNotificationTypes(){
        return this._http.get(this._serverURL+this._resources.GetNotificationType)
           
    }
}