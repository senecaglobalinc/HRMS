import { Injectable } from '@angular/core';
import { Associate } from '../models/associate.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import 'rxjs/Rx';
import { env } from 'process';

@Injectable({
  providedIn: 'root'
})
export class PersonalService {
  private EmployeeMicroService = environment.EmployeeMicroService;    
  private AdmintMicroService = environment.AdminMicroService;
  private resources = servicePath.API.personal;
  constructor(private httpClient : HttpClient ) { }

  public GetBusinessValues(valueKey: string){
      return this.httpClient.get(this.EmployeeMicroService + this.resources.getBusinessValues + valueKey);
}

  public GetHRAdvisors() {
    return this.httpClient.get(this.AdmintMicroService + this.resources.getHRAdvisors);
  }

  public GetEmpTypes() {
      return this.httpClient.get(this.EmployeeMicroService + this.resources.getEmpTypes);
  }

  public GetTechnologies() {
      return this.httpClient.get(this.AdmintMicroService + this.resources.getTechnologies);
  }

  public GetPersonalDetails(id: number): Observable<Associate> {
      return this.httpClient.get<Associate>(this.EmployeeMicroService + this.resources.getByPAId + id);
  }

  public GetEmployeePersonalDetails(id: number): Observable<Associate> {
      return this.httpClient.get<Associate>(this.EmployeeMicroService + this.resources.getByEmpId + id);
  }
  SavePersonalDetails(details: Associate) {
      let _url = this.EmployeeMicroService + this.resources.save;
      return this.httpClient.post(_url, details );
     
  }
  UpdatePersonalDetails(details: Associate) {
      let _url = this.EmployeeMicroService + this.resources.update;
      return this.httpClient.post(_url, details);           
  }
}


