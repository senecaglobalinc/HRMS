import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject } from "rxjs";
import { NotificationType  } from '../models/notificationconfiguration.model';
import { INotificationType } from '../../../Interfaces/INotificationType';
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';


@Injectable({
    providedIn: 'root'
})
export class NotificationTypeService {
    // subscribe(arg0: (res: any) => void): any {
    //     throw new Error("Method not implemented.");
    //   }
    editMode = false;
    notificationTypeData = new BehaviorSubject<NotificationType[]>([]);
    editObj = new BehaviorSubject<NotificationType>(new NotificationType());
    private _serverURL: string;
    private _resources: any;

    constructor(private _httpclient: HttpClient) {
        this._serverURL = environmentInformation.environment.AdminMicroService;
       this._resources = servicePath.API.NotificationType;
    }

    GetNotificationTypes(){
       this._httpclient.get(this._serverURL+this._resources.GetNotificationType)
       .subscribe((res: NotificationType[])=>{
          this.notificationTypeData.next(res);
         
      });
                            
     }
   
   
    public AddNotificationType(createObj: NotificationType) {
        let _url = this._serverURL + this._resources.AddNotificationType;
        if(this.editMode ==false)
        return this._httpclient.post(_url, createObj);
 
    }

    public UpdateNotificationType(createObj: NotificationType) {
        let _url = this._serverURL + this._resources.UpdateNotificationType;
        if(this.editMode ==true)
        return this._httpclient.post(_url,  createObj);
     
    }
  

    public DeleteNotificationType(notificationTypeId: number,categoryId:number) {
        let _url = this._serverURL + this._resources.DeleteNotificationType + notificationTypeId+"&categoryId="+categoryId
        return this._httpclient.post(_url, null);
   
    }

}