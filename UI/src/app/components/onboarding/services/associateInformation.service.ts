import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';

@Injectable({
  providedIn: 'root'
})
export class AssociateInformationService {
  private EmployeeMicroService = environment.EmployeeMicroService; 
  private resources = servicePath.API.AssociateInformation;
  constructor(private httpClient : HttpClient ) { }

  public getAssociates(){
   return this.httpClient.get(this.EmployeeMicroService + this.resources.getInfoAssociates)
  }

}
