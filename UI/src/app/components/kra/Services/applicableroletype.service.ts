import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ApplicableRoleType, ApplicableRoleTypeData } from '../../../models/kraApplicableRoleType.model';
import { BehaviorSubject } from 'rxjs';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class ApplicableRoleTypeService {

  _serviceUrl = environment.KRAMicroService;
  editMode = false;
  editObj = new BehaviorSubject<ApplicableRoleTypeData>(new ApplicableRoleTypeData());
  applicableRoleTypeData = new BehaviorSubject<ApplicableRoleTypeData[]>([]);
  resources = servicePath.API.ApplicableRoleType;
  constructor(private httpClient : HttpClient ) { }

public getApplicableRoleType(financialYearId: number, departmentId: number, gradeRoleTypeId: number): Observable<Array<ApplicableRoleTypeData>> {
        let url = this._serviceUrl + this.resources.getApplicableRoleType + financialYearId + "&DepartmentId=" + departmentId;
        + "&GradeRoleTypeId=" + gradeRoleTypeId;
        return this.httpClient.get<Array<ApplicableRoleTypeData>>(url);
}
public getApplicableRoleTypes(financialYearId: number): Observable<Array<ApplicableRoleTypeData>> {
        let url = this._serviceUrl + this.resources.getApplicableRoleType+financialYearId;
        return this.httpClient.get<Array<ApplicableRoleTypeData>>(url);
}

 public addKRAApplicableRoleType(applicableRoleTypeData: ApplicableRoleType) {
        let url = this._serviceUrl + this.resources.addKRAApplicableRoleType;
        return this.httpClient.post(url, applicableRoleTypeData);
    }

 public editApplicableRoleType(applicableRoleTypeData: ApplicableRoleType) {
        let url = this._serviceUrl + this.resources.update;
        return this.httpClient.post(url, applicableRoleTypeData);
    } 

     public updateRoleTypeStatus(applicableRoleTypeData: ApplicableRoleType) {
        let url = this._serviceUrl + this.resources.updateRoleTypeStatus;
        return this.httpClient.post(url,applicableRoleTypeData);
    } 

     public DeleteApplicableRoleType(applicableRoleTypeRoleId: number) {
        let url = this._serviceUrl + this.resources.delete + applicableRoleTypeRoleId;
        return this.httpClient.post(url, null);
    }

    public getApplicableRoleTypeById(financialYearId: number, departmentId: number, gradeRoleTypeId: number, gradeId: number): Observable<Array<any>> {
        let url = this._serviceUrl + this.resources.getApplicableRoleType + financialYearId ;
        if(departmentId>0) url=url+ "&DepartmentId=" + departmentId;
        if(gradeRoleTypeId>0)url=url+ "&GradeRoleTypeId=" + gradeRoleTypeId;
        else url=url+ "&GradeRoleTypeId=";
        if(gradeId>0)url=url+ "&gradeId=" + gradeId;
        
        return this.httpClient.get<Array<any>>(url);
} 
}
