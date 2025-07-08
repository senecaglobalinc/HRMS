import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { DefinitionModel } from '../../../../app/modules/kra/models/kradefinition.model'
import { environment } from '../../../../environments/environment'
import * as servicePath from '../../../core/service-paths';
import { Observable } from 'rxjs/Observable';
import { ApplicableRoleType } from '../../kra/models/kraapplicableRoleType.model';
@Injectable({ providedIn: 'root' })

export class KraDefinitionService {
    private _resource: any;
    private _resources: any;
    private _serverUrl: string;
    private _kraResource : any;
    constructor(private _http: HttpClient) {
      this._serverUrl = environment.KRAMicroService;
      this._resources = servicePath.API.DefineKRA;
      this._resource = servicePath.API.KRAWorkFlow;
      this._kraResource = servicePath.API.KRA
    }

    public CreateDefinition(definitionModel: DefinitionModel) {
        let url = this._serverUrl + this._resources.createDefinition;
        return this._http.post(url, definitionModel);
    }

    public ImportKRA(definitionModel: DefinitionModel) {
        let url = this._serverUrl + this._resources.importKRA;
        return this._http.post(url, definitionModel);
    }

    public GetDefinitionDetails(Id: string): Observable<Array<DefinitionModel>> {
            let url = this._serverUrl  + this._resources.getDefinitionDetails + Id;
            return this._http.get<Array<DefinitionModel>>(url);
    }
     public UpdateKRA(definitionModel: DefinitionModel) {
        let url = this._serverUrl + this._resources.updateKRA;
        return this._http.post(url, definitionModel);
    }

    public DeleteKRA(definitionDetailId: string): Observable<string> {
        let url = this._serverUrl + this._resources.deleteKRA + definitionDetailId;
        return this._http.post<string>(url, null);
    }

   public DeleteByHODHardDelete(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.deleteKRAByHODHardDelete + definitionDetailId;
        return this._http.post<number>(url, null);
    }
    //     public DeleteKRAByHOD(definitionDetailId: number): Observable<number> {
    //     let url = this._serverUrl + this._resources.deleteKRAByHOD + definitionDetailId;
    //     return this._http.post<number>(url, null);
    // }

     public SetPreviousMetricValues(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.setPreviousMetricValues + definitionDetailId;
        return this._http.post<number>(url, null);
    }

    public SetPreviousTargetValues(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.setPreviousTargetValues + definitionDetailId;
        return this._http.post<number>(url, null);
    }

 public AcceptTargetValue(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.acceptTargetValue + defintionTransactionId + "&username="+username;
        return this._http.post<number>(url, null);
    }

     public AcceptMetricValue(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.acceptMetricValue + defintionTransactionId + "&username="+username;
        return this._http.post<number>(url, null);
    }

     public RejectTargetValue(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.rejectTargetValue + defintionTransactionId + "&username="+username;
        return this._http.post<number>(url, null);
    }

     public RejectMetricValue(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.rejectMetricValue + defintionTransactionId  + "&username="+username;
        return this._http.post<number>(url, null);
    }

    public AcceptDeletedKRAByHOD(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.acceptDeletedKRAByHOD + defintionTransactionId  + "&username="+username;
        return this._http.post<number>(url, null);
    }

        public RejectDeletedKRAByHOD(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.rejectDeletedKRAByHOD + defintionTransactionId  + "&username="+username;
        return this._http.post<number>(url, null);
    }

        public AcceptAddedKRAByHOD(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.acceptAddedKRAByHOD + defintionTransactionId  + "&username="+username;
        return this._http.post<number>(url, null);
    }

        public RejectAddedKRAByHOD(defintionTransactionId: number, username: string): Observable<number> {
        let url = this._serverUrl + this._resources.rejectAddedKRAByHOD + defintionTransactionId  + "&username="+username;
        return this._http.post<number>(url, null);
    }
    
     public AddKRAAgain(definitionDetailId: number): Observable<number> {
        let url = this._serverUrl + this._resources.addKRAAgain + definitionDetailId;
        return this._http.post<number>(url, null);
    }

     public UpdateKRAStatus(financialYearId: number, gradeRoleTypeId: number , status: string) {
        let url = this._serverUrl + this._resources.updateKRAStatus + financialYearId + "&gradeRoleTypeId=" + gradeRoleTypeId +  "&status=" + status;
        return this._http.post(url,null);
    }

    public getKrasByFinancialYearIdAndRoleTypeId(financialYearId:number, roleTypeId : number){
        let url = this._serverUrl + this._resource.GetHODDefinitionsAsync + "?financialYearId=" + financialYearId  +"&roleTypeId="+ roleTypeId
        return this._http.get(url);
    }
    public AddKRAByHOD(addObj) {
        let url = this._serverUrl + this._resource.HODAdd;
        return this._http.post(url, addObj,{responseType: 'text'});
    }
    public EditKRAByHOD(editObj) {
        let url = this._serverUrl + this._resource.HODUpdate;
        return this._http.post(url, editObj,{responseType: 'text'});
    }
    public DeleteKRAByHOD(deleteReqObj) {
        let url = this._serverUrl + this._resource.HODDelete;
        return this._http.post(url, deleteReqObj,{responseType: 'text'});
    }
    public ApproveKRASBYHOD(approveObj) {
        let url = this._serverUrl + this._resource.ApprovedbyHOD;
        return this._http.post(url, approveObj,{responseType: 'text'});
    }
    public SendToOperationHead(requestObj) {
        let url = this._serverUrl + this._resource.SentToOpHead;
        return this._http.post(url, requestObj,{responseType: 'text'})
     }
     public EditedKRASByHOD(requestObj) {
        let url = this._serverUrl + this._resource.EditedByHOD;
        return this._http.post(url, requestObj,{responseType: 'text'})
     }

    public getOperationHeadDefinitions(financialYearId:number, roleTypeId : number){
        let url = this._serverUrl + this._resource.GetOperationHeadDefinitions + "?financialYearId=" + financialYearId  +"&roleTypeId="+ roleTypeId
        return this._http.get(url);
    }
    public AcceptedByOperationHead(reqData){
        let url = this._serverUrl + this._resource.AcceptedByOperationHead;
        return this._http.post(url,reqData,{responseType: 'text'});
    }

    public RejectedByOperationHead(reqData){
        let url = this._serverUrl + this._resource.RejectedByOperationHead;
        return this._http.post(url,reqData,{responseType: 'text'});
    }

    public SendToCEO(reqData){
        let url = this._serverUrl + this._resource.SendToCEO;
        return this._http.post(url,reqData,{responseType: 'text'});
    }

    public GeneratePDF(employeeCode,financialYearId,departmentId,roleId){
        const dId = departmentId == null ? '' : departmentId
        const rId = roleId == null ? '' : roleId 
        let url = this._serverUrl + this._kraResource.GenerateKRAPdf + "?employeeCode=" + employeeCode +"&financialYearId="+financialYearId+"&departmentId="+dId+"&roleId="+rId ;
        return this._http.get(url,{responseType: 'text'})

    }

}