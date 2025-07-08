import {Injectable} from '@angular/core'
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { AssociateSkillSearch } from '../models/associateSkillSearch.model'
import { GenericType } from '../../master-layout/models/dropdowntype.model';

@Injectable({
  providedIn: 'root'
})
export class AssociateskillsearchService {

 
  serviceUrl = environment.ServerUrl;
  resources = servicePath.API.SkillSearch;
  resource = servicePath.API.Reports;
reportUrl = environment.ReportMicroService;
adminUrl = environment.AdminMicroService;

  constructor(private httpClient : HttpClient){

  }

  // http://sg-srv-kas:2025/report/api/v1/Reports/GetEmployeeDetailsBySkill

  public GetEmployeeDetailsBySkill(skillData: AssociateSkillSearch) {
      var url = this.reportUrl + this.resource.GetEmployeeDetailsBySkill;
      return this.httpClient.post<AssociateSkillSearch[]>(url, skillData)
  }

  // public GetSkillsBySearchString(searchString: string){
  //     let _url = this.adminUrl + this.resources.GetSkillsBySearchString + searchString;
  //     return this.httpClient.get<GenericType[]>(_url);
  //   }

   public GetActiveSkillsForDropdown(){
      let _url = this.adminUrl + this.resources.GetActiveSkillsForDropdown;
      return this.httpClient.get<GenericType[]>(_url);
    }
    public GetProjectDetailByEmployeeId(empId : number){
      let _url = this.serviceUrl + this.resources.GetProjectDetailByEmployeeId + empId ;
      return this.httpClient.get(_url);

  }
}