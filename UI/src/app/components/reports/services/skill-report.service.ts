import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { AssociateSkillSearch } from '../../projectLifeCycle/models/associateSkillSearch.model';

@Injectable({
  providedIn: 'root'
})
export class SkillReportService {
  private resources: any;
  private serviceUrl: string;

  constructor(private httpclient: HttpClient) {
    this.serviceUrl = environment.ServerUrl;
    this.resources = servicePath.API.Reports;
  }
 
  public GetEmployeesBySkill(skillData: AssociateSkillSearch) {
    var url = this.serviceUrl + this.resources.GetEmployeesBySkill;
    return this.httpclient.post<AssociateSkillSearch[]>(url, skillData)
}
  public GetSkillsBySearchString(searchString: string) {
    let _url = this.serviceUrl + servicePath.API.SkillSearch.GetSkillsBySearchString + searchString;
    return this.httpclient.get(_url);
  }
  
}
