import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { ProjectDetails } from '../../../models/projects.model';
import { SOW } from '../../admin/models/sow.model';


@Injectable({
  providedIn: 'root'
})
export class SowService {
  serviceUrl = environment.ServerUrl;
  _projectMicroService = environment.ProjMicroService;
  resources = servicePath.API.sow;
  constructor(private httpClient: HttpClient) { }
  SOWData : SOW[] = [];

  public SaveSOW(newSOW){ // data type
    let url = this._projectMicroService + this.resources.createSOW;
    return this.httpClient.post(url,newSOW ) 
  }
  public GetSowByProjectId(projectId: number) {
    var url = this._projectMicroService + this.resources.getSOWDetailsById+projectId;
    return this.httpClient.get(url )
}
public GetProjectsList(userRole : string, empId : number) {
  let _url = this._projectMicroService+ servicePath.API.projects.getProjectList + userRole +"&employeeId="+ empId;
  return this.httpClient.get<ProjectDetails[]>(_url)
  } 
  public CreateAddendum(newAddendum){
    let url = this._projectMicroService + this.resources.createAddendum;
    return this.httpClient.post(url,newAddendum ) 
  }
  public GetAddendumsBySOWId(ID: number , projectId : number) {
    var url = this._projectMicroService + this.resources.GetAddendumsBySOWId+ID + "&projectId=" + projectId;
    return this.httpClient.get(url)
}
public GetSowDetails(projectId : number, Id : number , roleName : string){
    var url = this._projectMicroService + this.resources.GetSowDetails+Id + "&projectId=" + projectId + "&roleName=" + roleName;
    return this.httpClient.get(url )
}

public GetAddendumDetailsById(projectId : number, Id : number , roleName : string){
    var url = this._projectMicroService + this.resources.GetAddendumDetailsById+Id + "&projectId=" + projectId + "&roleName=" + roleName;
    return this.httpClient.get(url )
}

public UpdateSOWDetails(updateSowObj){
  let url = this._projectMicroService + this.resources.updateSOWDetails;
  return this.httpClient.post(url,updateSowObj ) 
}
public UpdateAddendumDetails(updateSowObj){
  let url = this._projectMicroService + this.resources.UpdateAddendumDetails;
  return this.httpClient.post(url,updateSowObj ) 
}

// public UpdateSOWAndAddendumDetails(updateSowObj){
//     let url = this.serviceUrl + this.resources.UpdateSOWAndAddendumDetails;
//     return this.httpClient.post(url,updateSowObj ) 
// }

public DeleteSow(Id){
  let url = this._projectMicroService + this.resources.delete + Id;
  return this.httpClient.post(url,null ) 
}
}
