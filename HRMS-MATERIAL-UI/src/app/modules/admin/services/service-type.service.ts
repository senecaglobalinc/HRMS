import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ServiceType } from '../models/servicetype.model';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
  })
  export class ServiceTypeService {
    serviceUrl = environment.AdminMicroService;
    editMode = false;
    editObj = new BehaviorSubject<ServiceType>(new ServiceType());
    resources = servicePath.API.ServiceType;
    serviceTypeData = new BehaviorSubject<ServiceType[]>([]);
    constructor(private httpClient: HttpClient) {}
  
    GetServiceTypeData() {
      this.httpClient
        .get(this.serviceUrl + this.resources.GetAll)
        .subscribe((res: ServiceType[]) => {
          this.serviceTypeData.next(res);
        });
    }
  
    CreateServiceTypeArea(createObj: ServiceType) {
      if (this.editMode == false)
        return this.httpClient.post(
          this.serviceUrl + this.resources.Create,
          createObj
        );
      else {
        return this.httpClient.post(
          this.serviceUrl + this.resources.Update,
          createObj
        );
      }
    }
  }