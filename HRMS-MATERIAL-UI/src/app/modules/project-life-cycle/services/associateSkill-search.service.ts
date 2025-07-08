import {Injectable} from '@angular/core'
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { AssociateSkillSearch } from '../models/associateSkillSearch.model'
import { GenericType } from '../../../models/dropdowntype.model';

@Injectable({
    providedIn: 'root'
  })
export class AssociateSkillsSearchService{

    serviceUrl = environment.ServerUrl;
    resources = servicePath.API.SkillSearch;

    constructor(private httpClient : HttpClient){

    }

    public GetEmployeeDetailsBySkill(skillData: AssociateSkillSearch) {
        var url = this.serviceUrl + this.resources.getEmployeeDetailsBySkill;
        return this.httpClient.post<AssociateSkillSearch[]>(url, skillData)
    }

    public GetSkillsBySearchString(searchString: string){
        let _url = this.serviceUrl + this.resources.GetSkillsBySearchString + searchString;
        return this.httpClient.get<GenericType[]>(_url);
      }

    public GetProjectDetailByEmployeeId(empId : number){
        let _url = this.serviceUrl + this.resources.GetProjectDetailByEmployeeId + empId ;
        return this.httpClient.get(_url);

    }
}