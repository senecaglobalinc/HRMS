import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root'
})

export class AssociateInformationService {
  private _serviceUrl = environment.ServerUrl;
  private EmployeeMicroService = environment.EmployeeMicroService; 
  private resources = servicePath.API.AssociateInformation;
  private empSendMail = servicePath.API.WelcomeEmail;
 httpOptionsForFormDataRegister = {
    headers: new HttpHeaders(
      { 
        'Accept': 'multipart/form-data' 
      }), 
    responseType : 'text' as 'json'
  
  };
  
  constructor(private httpClient : HttpClient ) { }

  public getAssociates(searchstring: string, pageIndex: number, pageSize: number){
   return this.httpClient.get(this.EmployeeMicroService + this.resources.getInfoAssociates+'?searchString='+searchstring+'&pageIndex='+pageIndex+'&pageSize='+pageSize)
  }

  public WelcomeEmail(){
    return this.httpClient.get(this.EmployeeMicroService + this.empSendMail.WelcomeEmail);
   }

   public sendWelcomeEmail(reqObj){
    return this.httpClient.post(this.EmployeeMicroService + this.empSendMail.SendWelcomeEmail,reqObj,this.httpOptionsForFormDataRegister)
   }

   public syncProspectiveAssociateDataToHRMS(){
    return this.httpClient.get(this.EmployeeMicroService + this.resources.prospectiveToHrms)
  }

  public getEmployeeCount(searchString = ''){
    return this.httpClient.get(this.EmployeeMicroService + this.resources.getEmployeeCount+searchString)
   }

}
