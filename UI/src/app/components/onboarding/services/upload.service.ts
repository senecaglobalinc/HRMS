import { Injectable, Inject } from '@angular/core';
import { Certification } from '../models/professionaldetail.model';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private _resources: any;
  private _serviceUrl = environment.EmployeeMicroService;
  constructor(@Inject(HttpClient) private httpClient: HttpClient) {
    this._resources = servicePath.API.upload;
  }
  public GetUploadData(id: number): Observable<Certification> {
    var url = this._serviceUrl + this._resources.list + id;
    return this.httpClient.get<Certification>(url);
  }

  public GetPAstatus(id: number) {
    var url = this._serviceUrl + this._resources.GetPAstatus + id;
    return this.httpClient.get(url);
  }
  public save(data){
    var url = this._serviceUrl + this._resources.save;
    return this.httpClient.post(url,data);
  }
  public SubmitForApproval(id: number) {
    let ids = {"EmpId":id}
    var url = this._serviceUrl + this._resources.submitForApproval;
    return this.httpClient.post(url, ids);
  }
  public Delete(employeeId: number, id: number) {
    var url = this._serviceUrl + this._resources.delete + id + "/" + employeeId;
    return this.httpClient.delete(url);
  }
}

