
import {Injectable, Inject} from '@angular/core';
import { Associate} from '../models/associate.model';
import { Projects} from "../models/assosiateproject.model";
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProjectassosiateService {
  private _resources: any;
  private _serviceUrl = environment.EmployeeMicroService;
 
  roles:any[]=[];
 
  constructor(@Inject(HttpClient) private httpClient: HttpClient) {
         this._resources = servicePath.API.associateprojects;
     }
 
     public GetAssociateProjects(id: number): Observable<Projects> {
         var url = this._serviceUrl + this._resources.get +id;
         return this.httpClient.get<Projects>(url);    
     }
 
     SaveProjectDetails(details: Associate) {
         let _url = this._serviceUrl + this._resources.update;
         return this.httpClient.post(_url, details );     
   
     } 
     
 }