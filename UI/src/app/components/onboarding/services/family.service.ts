import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Associate } from '../models/associate.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
  
  @Injectable({  
    providedIn: 'root'
  })
  
export class FamilyService 
{  
  private _resources = servicePath.API.family;
  private _serverURL: string;
  private EmployeeMicroService = environment.EmployeeMicroService;    
  
  constructor(private httpClient: HttpClient) {}
   
  public GetFamilyDetails(id: number) 
  {
   return this.httpClient.get<Associate>(this.EmployeeMicroService + this._resources.list + id);
  }

  public SaveFamilyDetails(details: Associate) 
  {  
   let _url = this.EmployeeMicroService + this._resources.save;
      return this.httpClient.post(_url, details);
  
  }
  public DeleteFamilyDetails(familyDetailsID: number) 
  {
   //let _url = this._serverURL + this._resources.deleteFamilyDetails + familyDetailsID;
   //return this._http.post(_url, familyDetailsID ); 
  }
  
}
  
  
  
