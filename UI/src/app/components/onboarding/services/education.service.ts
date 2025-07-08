import { Injectable,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { Associate } from '../models/associate.model';

@Injectable({
  providedIn: 'root'
})
export class EducationService {
  private _resources: any;
  private EmployeeMicroService = environment.EmployeeMicroService;
  constructor( @Inject(HttpClient) private _httpClient: HttpClient) {
    this._resources = servicePath.API.education;
}
SaveEducationDetails(details: Associate)
{ 
    let url = this.EmployeeMicroService + this._resources.save;
     return this._httpClient.post(url,details) 
  }


  public GetQualifications(empId : number){
    var url = this.EmployeeMicroService + this._resources.list + empId;
    return this._httpClient.get(url)
}

}
