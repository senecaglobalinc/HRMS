import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ApplicableRoleType, ApplicableRoleTypeData } from '../models/kraapplicableRoleType.model';
import { BehaviorSubject } from 'rxjs';
import * as servicePath from '../../../core/service-paths'
//'src/app/core/service-paths';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class ApplicableRoleTypeService {

  _serviceUrl = environment.KRAMicroService;
  _adminServiceUrl = environment.AdminMicroService;
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
public getDepartments(): Observable<Array<any>> {
  let url = this._adminServiceUrl + this.resources.getDepartments  ;   
  return this.httpClient.get<Array<any>>(url);
} 

public getGradesByDepartment(departmentId: number): Observable<Array<any>> {
  let url = this._adminServiceUrl + this.resources.getGradesByDepartment  ;  
  if(departmentId>0) url=url+ "?DepartmentId=" + departmentId;
  return this.httpClient.get<Array<any>>(url);
} 
public getRoleTypesByGrade(departmentId: number, gradeId: number): Observable<Array<any>> {
  let url = this._adminServiceUrl + this.resources.getRoleTypesByGrade  ;
  if(departmentId>0) url=url+ "?DepartmentId=" + departmentId;
  if(gradeId>0)url=url+ "&GradeId=" + gradeId;
    
  return this.httpClient.get<Array<any>>(url);
} 

public getRoleTypesByFinancialYearAndDepartment(financialYearId: number, departmentId: number): Observable<Array<any>> {
  let url = this._adminServiceUrl + this.resources.getRoleTypesByDeptAndFY + financialYearId + "&departmentId=" + departmentId; 
  return this.httpClient.get<Array<any>>(url);
} 

public getGradesForSelectedRole(financialYearId:number,departmentId:number,roleTypeId:number){
  let url = this._adminServiceUrl + this.resources.getGradesBySelectedRoleType + financialYearId + "&departmentId=" + departmentId + "&roleTypeId=" + roleTypeId; 
  return this.httpClient.get<Array<any>>(url);
}


}
