import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http'; 
import { KraSetData, KraAspectData} from '../../../models/kra.model';
import { RoleData} from '../../../models/role.model';
import { GenericType } from '../../../models/dropdowntype.model';
import { KRAGroup } from '../../../models/kragroup.model';
import { KRADefinition, KRAWorkFlowData, KRASubmittedGroup } from '../../../models/kradefinition.model';
import { KRAComments } from '../../../models//kracomments.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths'; 
 
@Injectable({
    providedIn: 'root'
})
export class KRAService {
    private _resources: any;
    private _serverURL: string;
    constructor(private http: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.KRA; 
    }

    public getRolesByRoleMasterId(roleMasterID: number): Observable<RoleData> {
        let url = this._serverURL + this._resources.getRolebyID + roleMasterID;
        return this.http.get<RoleData>(url);
    }

      public addKRAComments(kraComments: KRAWorkFlowData): Observable<number> {
        let url = this._serverURL + this._resources.addKRAComments;
        return this.http.post<number>(url,kraComments);
    }

    public getKRAGroups(financialYearId: number): Observable<Array<KRAGroup>> {
        let url = this._serverURL + this._resources.getKRAGroupsByYear + financialYearId;
        return this.http.get<Array<KRAGroup>>(url);
    }
    public getKRAGroupsByFinancialYear(financialYearId: number): Observable<Array<KRAGroup>> {
        let url = this._serverURL + this._resources.getKRAGroupsByFinancialYear + financialYearId;
        return this.http.get<Array<KRAGroup>>(url);
    }
    public createKRAGroup(kraGroupData: KRAGroup) {
        let url = this._serverURL + this._resources.createKRAGroup;
        return this.http.post(url, kraGroupData);
    }

    public getKRAGroupList(departmentID: number): Observable<GenericType[]> {
        let url = this._serverURL + this._resources.getKRAGroupList + departmentID;;
        return this.http.get<GenericType[]>(url);
    }

    public getCurrentFinancialYear(): Observable<GenericType> {
        let url = this._serverURL + this._resources.getCurrentFinancialYear;
        return this.http.get<GenericType>(url);
    }

    public getKraDefinitions(kraGroupId: number, FinancialYearID: number): Observable<Array<KRADefinition>> {
        let url = this._serverURL + this._resources.getKRADefinitionsById + kraGroupId + "&financialYearId=" + FinancialYearID;
        return this.http.get<Array<KRADefinition>>(url);
    }
    public getKraAspects(departmentId: number): Observable<Array<KraAspectData>> {
        let url = this._serverURL + this._resources.getKraAspects + departmentId;
        return this.http.get<Array<KraAspectData>>(url);
    }

    //Not mentioning returning as http delete returns Observable<response> and from service bool is being returned
    public deleteMetricAndTarget(kraDef: KRADefinition) {
        let url = this._serverURL + this._resources.deleteKRAMetricByKRADefinitionId;
        return this.http.post(url, kraDef);
    }

    public deleteKraAspectDetails(kraDefinition: KRADefinition) {
        let url = this._serverURL + this._resources.deleteKRADefinition;
        return this.http.post(url, kraDefinition);
    }

    public CreateKRADefinition(kraRoleData: KRADefinition) {
        let url = this._serverURL + this._resources.createKRADefinition;
        return this.http.post(url, kraRoleData);
    }   

    public updateKraDefinition(kraRoleData: KRADefinition) {
        let url = this._serverURL + this._resources.updateKRADefinition;
        return this.http.post(url, kraRoleData);
    }
    
    public sendForDepartmentApproval(kraWorkFlowdata: KRAWorkFlowData) {
        let url = this._serverURL + this._resources.sendForDepartmentApproval;
        return this.http.post(url, kraWorkFlowdata);
    }
    public approveKRA(kraWorkFlowdata: KRAWorkFlowData) {
        let url = this._serverURL + this._resources.approveKRA;
        return this.http.post(url, kraWorkFlowdata);
    }
    public SendBackForHRMReview(kraWorkFlowdata: KRAWorkFlowData) {
        let url = this._serverURL + this._resources.SendBackForHRMReview;
        return this.http.post(url, kraWorkFlowdata);
    }
    
    public SendtoHRHeadReview(kraWorkFlowdata: KRAWorkFlowData) {
        let url = this._serverURL + this._resources.SendtoHRHeadReview;
        return this.http.post(url, kraWorkFlowdata);
    }
    public SendBacktoDepartmentHead(kraWorkFlowdata: KRAWorkFlowData) {
        let url = this._serverURL + this._resources.SendBacktoDepartmentHead;
        return this.http.post(url, kraWorkFlowdata);
    }

    public deleteKraRoleByYearIdAndRoleId(roleId: number, financialYearId: number) {
        let url = this._serverURL + this._resources.deleteKraRoleByYearIdAndRoleId + roleId + "&financialYearId=" + financialYearId;
        return this.http.post(url, roleId);
    }

    public SubmitKraForHRHeadReview(kraSetData: KraSetData) {
        let url = this._serverURL + this._resources.submitKraForHRHeadReview;
        return this.http.post(url, kraSetData);
    }
    public getKRAComments(financialYearId: number, kRAGroupId: number): Observable<Array<KRAComments>> {
        let url = this._serverURL + this._resources.getKRAComments + financialYearId + "&kRAGroupId=" + kRAGroupId;
        return this.http.get<Array<KRAComments>>(url);
    }    

    public GetFinancialYear(financialYearId: number): Observable<string> {
        let url = this._serverURL + this._resources.getFinancialYear + financialYearId;
        return this.http.get<string>(url);

    }  
    public deleteKRAGroup(deleteKRAGroupData: KRASubmittedGroup) {
        let url = this._serverURL + this._resources.deleteKRAGroup;
        return this.http.post(url, deleteKRAGroupData);
    } 
}

