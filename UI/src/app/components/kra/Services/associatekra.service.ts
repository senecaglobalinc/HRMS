import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { AssociateKras } from '../../../models/associate-kras.model';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { AssociateRoleMappingData } from 'src/app/models/associaterolemappingdata.model';
import { GenericType } from 'src/app/models/dropdowntype.model';

@Injectable({
  providedIn: 'root'
})

export class AssociatekraService {
  private _resources: any;
  private _serverUrl: string;

  constructor(private http: HttpClient) {
    this._serverUrl = environment.ServerUrl;
    this._resources = servicePath.API.AssociateKras;
  }

  public GetEmployeesByDepartmentIdAndProjectId(departmentId: number, projectId: number, financialYearID: number): Observable<AssociateRoleMappingData[]> {
    let url = this._serverUrl + this._resources.getEmployeesByProjectID + departmentId + "&projectId=" + projectId + "&financialYearID=" + financialYearID;
    return this.http.get<AssociateRoleMappingData[]>(url);
  }

  public GetAssociateKRAs(EmployeeId: number, FinancialYearID: number): Observable<AssociateKras> {
    let url = this._serverUrl + this._resources.GetAssociateKRAs + EmployeeId + "&financialYearID=" + FinancialYearID;
    return this.http.get<AssociateKras>(url);
  }

  public generatePDFforAllAssociates(overideExisting ){
    let url= this._serverUrl+ this._resources.GenerateKRAPdfForAllAssociates + overideExisting;
    return this.http.get(url);
  }

  public GenerateKRAPdfSelectedAllAssociates(empList : AssociateRoleMappingData[]){
    let url= this._serverUrl+ this._resources.GenerateKRAPdfSelectedAllAssociates;

    return this.http.post(url, empList);
  }
}
