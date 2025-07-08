import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { Certification, MemberShip } from '../models/professionaldetails.model';

@Injectable({
  providedIn: 'root'
})
export class ProfessionalService {

  private _resources: any;
    private _serverURL: string;
    constructor(private _httpclient: HttpClient) {
        // this._serverURL = environment.ServerUrl;
        this._serverURL = environment.EmployeeMicroService;
        this._resources = servicePath.API.professional;
    }
    getSkillGroupCertificate(){
        var url = environment.AdminMicroService + this._resources.getSkillGroupCertificate;
        return this._httpclient.get(url);
    }

   
    getProfessionalDetails(employeeID: number) {
        var url = this._serverURL + this._resources.getProfessionalDetails + employeeID;
        return this._httpclient.get(url);
    }

   
    addCertificationDetails(certificationDetails: Certification) {
        let _url = this._serverURL + this._resources.addCertificationDetails;
        return this._httpclient.post(_url, certificationDetails); 
    }

    addMembershipDetails(membershipDetails: MemberShip) {
        let _url = this._serverURL + this._resources.addMembershipDetails;
        return this._httpclient.post(_url, membershipDetails); 
    }
    updateCertificationDetails(certificationDetails: Certification){
        let _url = this._serverURL + this._resources.updateCertificationDetails;
        return this._httpclient.post(_url, certificationDetails);
    }

    updateMembershipDetails(membershipDetails: MemberShip){
        let _url = this._serverURL + this._resources.updateMembershipDetails;
        return this._httpclient.post(_url, membershipDetails);
    }
    deleteProfessionalDetails(id: number, programType: number) {
        let _url = this._serverURL + this._resources.deleteMembershipDetails + id + "/" + programType;
        return this._httpclient.delete(_url);
    }
}



  







