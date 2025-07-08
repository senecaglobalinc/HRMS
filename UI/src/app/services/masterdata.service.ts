import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { GenericType } from "../models/dropdowntype.model";
import { Grade } from "../components/admin/models/grade.model";
import { DepartmentDetails } from "../models/role.model";
import { RequisitionRoleSkills } from "../models/talentrequisitiondata.model";
import { ProjectDetails } from "../models/projects.model";
import { PracticeArea } from "../models/associate-skills.model";
import { Designation } from "../components/admin/models/designation.model";
import { Client } from "../components/admin/models/client.model";
import { PercentageDropDown } from "../components/talentmanagement/models/associateallocation.model";
import {environment} from '../../environments/environment';
import * as servicePath from '../service-paths';

import { KRAScaleData } from "../models/krascaleData.model";
import { ProjectTypeData } from "../components/admin/Models/projecttype.model";
import { KraRoleData } from "../models/kraRoleData.model";
import { Roletype } from "../components/admin/models/roletype.model";

@Injectable({
    providedIn: 'root'
})
export class MasterDataService {
    private _serverURL: string;
    private _adminMicroService: string;
    private _employeeMicroService: string;
    private _resources: any;
    private _kraMicroService: string;
    private _projectMicroService: string;
    constructor(private _http: HttpClient) {
        this._projectMicroService = environment.ProjMicroService;
        this._serverURL = environment.ServerUrl;
        this._adminMicroService = environment.AdminMicroService;
        this._employeeMicroService = environment.EmployeeMicroService;
        this._resources = servicePath.API.Masterdata;
        this._kraMicroService = environment.KRAMicroService;
    }

    GetDepartmentsMasterData() : Observable<DepartmentDetails[]>{
        var url = this._adminMicroService + servicePath.API.Department.list;
        return this._http.get<DepartmentDetails[]>(url);
    }

    GetDepartments(): Observable<DepartmentDetails[]> {
        var url = this._adminMicroService + this._resources.getDepartments;
        return this._http.get<DepartmentDetails[]>(url);
    }

    GetAllDepartments(): Observable<DepartmentDetails[]> {
        var url = this._serverURL + this._resources.getAllDepartments;
        return this._http.get<DepartmentDetails[]>(url);
    }

    GetRoles(): Observable<any[]> {
        var url = this._adminMicroService + this._resources.getRoles;
        return this._http.get<any[]>(url)
    }

    GetRolesByDepartmentId(departmentId: number): Observable<Array<GenericType>> {
        let url = this._serverURL + this._resources.getRolesByDepartmentID + departmentId;
        return this._http.get<GenericType[]>(url)
    }

    GetRolesByProjectId(ProjectId: number): any {
        var url = this._serverURL + this._resources.getRolesByProjectId + ProjectId;
        return this._http.get(url);
    }

    GetCompetencyAreas(): any {
        var url = this._serverURL + this._resources.getCompetencyAreas;
        return this._http.get<any[]>(url);
    }

    GetSkillGroupsByCompetenctArea(competencyAreaID: number): any {
        var url = this._serverURL + this._resources.getSkillGroupsByCompetenctArea + competencyAreaID;
        return this._http.get(url)
    }

    GetSkillsBySkillGroups(SkillGroupID: number): any {
        let url = this._adminMicroService + this._resources.getSkillsBySkillGroup + SkillGroupID;
        return this._http.get(url);
    }

    GetProficiencyLevels(): any {
        let url = this._serverURL + this._resources.getProficiencyLevels;
        return this._http.get(url);
    }

    GetProjectsList() {
        let _url = this._projectMicroService + this._resources.getProjectList;
        return this._http.get<ProjectDetails[]>(_url)
    }
    
    GetProjectsForDropdown() {
        let _url = this._projectMicroService + this._resources.getProjectsForDropdown;
        return this._http.get<GenericType[]>(_url)
    }

    GetDomains(): any {
        let _url = this._adminMicroService + this._resources.getDomains;
        return this._http.get(_url)
    }

    GetManagersAndCompetencyLeads(): Observable<GenericType[]> {
        let _url = this._employeeMicroService + this._resources.getManagersAndCompetencyLeads;
        return this._http.get<GenericType[]>(_url)
    }

    GetFinancialYears(): Observable<Array<GenericType>> {
        let url = this._adminMicroService  + this._resources.getFinancialYears;
        return this._http.get<GenericType[]>(url);
    }

    GetEmailIDsByString(suggestionString: string): Observable<string[]> {
        let url = this._serverURL + this._resources.getAllEmailIDs + suggestionString;
        return this._http.get<string[]>(url);
    }

    GetEmployeeNameByEmployeeId(employeeId: number): Observable<string> {
        let url = this._serverURL + this._resources.getEmployeeNameByEmployeeId + employeeId;
        return this._http.get<string>(url);
    }

    GetEmployeesAndManagers(suggestionString: string) {
        let url = this._serverURL + this._resources.getEmployeesAndManagers + suggestionString;
        return this._http.get(url);
    }


    GetAllLeadsManagers(suggestionString: string) {
        let url = this._serverURL + this._resources.getAllLeadsManagers + suggestionString;
        return this._http.get(url);
    }


    GetAllAssociateList(): Observable<GenericType[]> {
        let _url = this._serverURL + this._resources.getAllAssociateList;
        return this._http.get<GenericType[]>(_url);
    }

    GetMasterSkillList(): Observable<RequisitionRoleSkills[]> {
        var url = this._serverURL + this._resources.getMasterSkillList;
        return this._http.get<RequisitionRoleSkills[]>(url);
    }

    GetPractiseAreas(): Observable<PracticeArea[]> {
        var url = this._adminMicroService + this._resources.getPractiseAreas;
        return this._http.get<PracticeArea[]>(url);
    }

    GetGradesDetails(): Observable<Grade[]> {
        let _url = this._serverURL + this._resources.getGradesDetails;
        return this._http.get<Grade[]>(_url);
    }

    GetGradesMasterData(): Observable<Grade[]> {
        let _url = this._adminMicroService + servicePath.API.Grades.list;
        return this._http.get<Grade[]>(_url);
    }

    GetClientList(): Observable<Client[]> {
        let _url = this._adminMicroService + servicePath.API.projects.getClients;
        return this._http.get<Client[]>(_url);
    }

    GetDesignationList(): Observable<Designation[]> {
        let _url = this._serverURL + this._resources.getDesignationList;
        return this._http.get<Designation[]>(_url);
    }

    GetAllocationPercentages(): Observable<PercentageDropDown[]> {
        var url = this._projectMicroService + this._resources.getAllocationPercentages;
        return this._http.get<PercentageDropDown[]>(url);
    }

    GetProgramManagers(): Observable<GenericType[]> {
        var url = this._serverURL + this._resources.getProgramManagers;
        return this._http.get<GenericType[]>(url)
    }

    GetProjectTypes(): Observable<ProjectTypeData[]> {
        let url = this._adminMicroService + this._resources.getProjectTypes;
        return this._http.get<ProjectTypeData[]>(url)
    }

    // GetProjectTypes(): Observable<ProjectType[]> {
    //     let _url = this._serverURL + this._resources.getProjectTypes;
    //     return this._http.get<ProjectType[]>(_url  );
    // }

    GetRoleCategories(): Observable<GenericType[]> {
        var url = this._serverURL + this._resources.getRoleCategory;
        return this._http.get<GenericType[]>(url);
    }
    GetKRAOperators(): Observable<any> {
        var url = this._kraMicroService + this._resources.getKRAOperators;
        return this._http.get(url);
    }
    GetKRAMeasurementType(): Observable<GenericType[]> {
        var url = this._serverURL + this._resources.getKRAMeasurementType;
        return this._http.get<GenericType[]>(url  );
    }
    GetKRATargetPeriods(): Observable<any> {
        var url = this._kraMicroService + this._resources.getKRATargetPeriods;
        return this._http.get(url);
    }
    getKRAScales(): Observable<KRAScaleData[]> {
        var url = this._serverURL + this._resources.getKRAScales;
        return this._http.get<KRAScaleData[]>(url  )
    }
    getKRAScaleValues(): Observable<GenericType[]> {
        var url = this._serverURL + this._resources.getKRAScaleValues;
        return this._http.get<GenericType[]>(url  );
    }

    GetKraRoles(): Observable<KraRoleData[]>{
        var url = this._serverURL + this._resources.getKRARoles;
        return this._http.get<KraRoleData[]>(url  );
    }

    GetDepartmentByDepartmentTypeId(departmentTypeId: number): Observable<Array<GenericType[]>> {
        let url = this._serverURL + this._resources.getDepartmentByDepartmentTypeId + departmentTypeId;
        return this._http.get<any[]>(url  );
    }

    GetDepartmentTypes(): Observable<GenericType[]> {
        var url = this._serverURL + this._resources.getDepartmentTypes;
        return this._http.get<any[]>(url  );
    }

    GetDesignationByString(suggestionString: string): Observable<Array<GenericType>> {
        let url = this._adminMicroService + this._resources.getDesignationByString + suggestionString;
        return this._http.get<any[]>(url  );
    }

    GetCategories() {
        var url = this._adminMicroService + this._resources.GetCategories;
        return this._http.get(url  )
    }

    getGradeByDesignation(designationId: number): Observable<Grade> {
        let url = this._adminMicroService + this._resources.getGradeByDesignation + designationId;
        return this._http.get<Grade>(url);
    }

    GetRoleTypesByGrade(gradeId: number) {
        let url = this._kraMicroService + servicePath.API.RoleType.getRoleTypesByGrade + gradeId;
        return this._http.get(url);
    }

    GetFinancialYearMasterData(){
        let url = this._kraMicroService + servicePath.API.FinancialYear.getFinancialYears;
        return this._http.get(url);
    }
}
