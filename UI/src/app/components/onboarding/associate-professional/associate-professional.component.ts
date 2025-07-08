import { Component, OnInit } from '@angular/core';
import * as servicePath from '../../../service-paths';
import { MemberShip, Certification, Professional } from '../models/professionaldetails.model';
import { ProfessionalService } from '../services/professional.service';
import { CommonService } from '../../../services/common.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { FormGroup } from '@angular/forms';
import { DropDownType, SkillGroupDropDown, SkillDropDown } from '../../../models/dropdowntype.model';
import { ViewChild } from '@angular/core';
import { Dialog } from 'primeng/components/dialog/dialog';
import { FormBuilder } from '@angular/forms';
import { ProgramType } from '../../shared/utility/enums';
import { Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-associate-professional',
  templateUrl: './associate-professional.component.html',
  styleUrls: ['./associate-professional.component.scss'],
  providers: [CommonService, MessageService]
})
export class AssociateProfessionalComponent implements OnInit {
  myForm: FormGroup;
    formSubmitted: boolean = false;
    employeeId: number;
    certificationList: Professional[] = [];
    memberShipList: Professional[] = [];
    professionalList: Professional[]
    professionalDetail: Professional;
    programType: DropDownType[];
    professionalType: number;
    deleteID: number;
    skillGroups: DropDownType[];
    skills: DropDownType[];
    certification: boolean = false;
    membership: boolean = false;
    addDisplay: boolean = false;
    inputDisable: boolean = false;
    deletedisplay: boolean = false;
    skillGroupID: number;
    date = new Date();
    upDateId: number;
    validYear: number;
    validFromErrorMessage: string = "";
    validUpToErrorMessage: string = "";
    @ViewChild('programTypeDialog') programTypeDialog: Dialog;
   PageSize: number;
   PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  
certificationCols = [
  {field: 'SkillGroupName', header: 'SkillGroupName' },
    {field : 'SkillName', header : 'SkillName'},
    {field : 'Institution', header : 'Institution'},
    {field: 'Specialization', header: 'Specialization' },
    {field : 'ValidFrom', header : 'ValidFrom'},
    {field : 'ValidUpto', header : 'ValidUpto'}
];
membershipCols = [
    {field: 'ProgramTitle', header: 'MembershipTitle' },
  {field : 'Institution', header : 'Institution'},
  {field: 'Specialization', header: 'Specialization' },
  {field : 'ValidFrom', header : 'ValidFrom'},
  {field : 'ValidUpto', header : 'ValidUpto'}
];


  constructor(public fb: FormBuilder,private messageService: MessageService,private actRoute: ActivatedRoute, private _common : CommonService, private _professionalService: ProfessionalService, private masterDataService:MasterDataService) {
    this.programType = new Array<DropDownType>();
    this.skillGroups = new Array<DropDownType>();
    this.skills = new Array<DropDownType>();
    this.professionalList = new Array<Professional>();
    this.professionalDetail = new Professional();
     this.PageSize = this.resources.PageSize;
       this.PageDropDown = this.resources.PageDropDown;
}

  ngOnInit() {
    this.formSubmitted = false;
        this.actRoute.params.subscribe(params => { this.employeeId = params['id']; });
        this.skillGroupID = -1;
        this.professionalType = -1;
        this.upDateId = -1;
        this.deleteID = -1;
        this.deletedisplay = false;
        this.formSubmitted = false;
        this.inputDisable = false;
        this.certification = false;
        this.membership = false;
        this.addDisplay = false;
        this.programType.push({ label: 'Select ProgramType', value: null });
        this.programType.push({ label: 'Certificate', value: ProgramType.certification });
        this.programType.push({ label: 'Member Ship', value: ProgramType.membership });
        this.myForm = this.fb.group({
            'programType': [null, [Validators.required]],
            'skillGroupId': [null, null],
            'skillId': [null, null],
            'programTitle': [null, [Validators.pattern('[A-Za-z][a-zA-Z\\s]{2,64}')]],
            'specialization': [null, [Validators.required, Validators.pattern('[A-Za-z][a-zA-Z\\s]{2,64}')]],
            'institution': [null, [Validators.required, Validators.pattern('[A-Za-z][a-zA-Z\\s]{2,64}')]],
            'validFrom': [null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$')]],
            'validUpTo': [null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$')]]
        });
        this.getProfessionalDetail();
        this.skillGroups = [];
        this.skillGroups.push({ label: 'Select Skill Group', value: null });
        this.skills = [];
        this.skills.push({ label: 'Select Skill Name', value: null });
  }

   getProfessionalDetail() {
      this._professionalService.getProfessionalDetails(this.employeeId).subscribe((professionalList: Professional[]) => {
        this.fillCertifications(professionalList);
        this.fillMembership(professionalList);
    }, (error: any) => {
        this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to get Professional Details'});
    });
}
 fillCertifications(professionalList: Array<Professional>) {
    this.certificationList = professionalList.filter(x=>x.ProgramType==ProgramType.certification);
    return this.certificationList;
}
 fillMembership(professionalList: Array<Professional>) {
    this.memberShipList = professionalList.filter(x=>x.ProgramType==ProgramType.membership);
    return this.memberShipList;
}
 getSkillGroup() {
    this._professionalService.getSkillGroupCertificate().subscribe((skillGroupList: SkillGroupDropDown[]) => {
        this.skillGroups = [];
        this.skillGroups.push({ label: 'Select Skill Group', value: null });
        skillGroupList.forEach((e: SkillGroupDropDown) => {
            this.skillGroups.push({ label: e.SkillGroupName, value: e.SkillGroupId })
        });
    }, (error: any) => {
        this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to get Skill Group List.'});
    });
}
 skilGroupChange(event: any) {
    this.skillGroupID = -1;
    this.skillGroupID = event.value;
    this.myForm.controls['skillId'].setValue(null);
    this.getSkillNames()
}
 getSkillNames() {
    this.masterDataService.GetSkillsBySkillGroups(this.skillGroupID).subscribe((skillList: SkillDropDown[]) => {
        this.skills = [];
        this.skills.push({ label: 'Select Skill Name', value: null });
        skillList.forEach((e: SkillDropDown) => {
            this.skills.push({ label: e.SkillName, value: e.SkillId })
        });

    }, (error: any) => {
        this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to get Skill Group List.'});
    });
}
 addCertificationOrMembership() {
    this.getSkillGroup();
    this.getSkillNames();
    this.validFromErrorMessage = "";
    this.validUpToErrorMessage = "";
    this.myForm.reset();
    this.formSubmitted = false;
    this.inputDisable = false;
    this.professionalDetail.ProgramType = null;
    this.membership = false;
    this.certification = false;
    this.addDisplay = true;
}
 programTypeChange(event: any) {
    this.validFromErrorMessage = "";
    this.validUpToErrorMessage = "";
    this.myForm.controls['skillGroupId'].setValue(null);
    this.myForm.controls['skillId'].setValue(null);
    this.myForm.controls['programTitle'].setValue(null);
    this.myForm.controls['institution'].setValue("");
    this.myForm.controls['specialization'].setValue("");
    this.myForm.controls['institution'].setValue("");
    this.myForm.controls['validFrom'].setValue(null);
    this.myForm.controls['validUpTo'].setValue(null);
    this.formSubmitted = false;
    if (event.value == ProgramType.certification) {
        this.certification = true;
        this.membership = false;
    }
    else if (event.value == ProgramType.membership) {
        this.membership = true;
        this.certification = false;
    }
    else {
        this.membership = false;
        this.certification = false;
    }
}
 getCertificationEdit(event: any) {
    this.skillGroupID = event.SkillGroupId;
    this.getSkillGroup();
    this.getSkillNames();
    this.addDisplay = true;
    this.certification = true;
    this.membership = false;
    this.inputDisable = true;
    this.upDateId = event.Id;
    this.professionalDetail.ProgramType = event.ProgramType;
    this.professionalDetail.SkillGroupId = event.SkillGroupId;
    this.professionalDetail.CertificationId = event.CertificationId;
    this.professionalDetail.Specialization = event.Specialization;
    this.professionalDetail.Institution = event.Institution;
    this.professionalDetail.ValidFrom = event.ValidFrom;
    this.professionalDetail.ValidUpto = event.ValidUpto;
}
 getMembershipEdit(event: any) {
    this.addDisplay = true;
    this.membership = true;
    this.certification = false;
    this.inputDisable = true;
    this.upDateId = event.Id;
    this.professionalDetail.ProgramType = event.ProgramType;
    this.professionalDetail.ProgramTitle = event.ProgramTitle;
    this.professionalDetail.Specialization = event.Specialization;
    this.professionalDetail.Institution = event.Institution;
    this.professionalDetail.ValidFrom = event.ValidFrom;
    this.professionalDetail.ValidUpto = event.ValidUpto;
}
 deleteCertificateOrMembership(event: any) {
    this.deleteID = event.Id;
    this.professionalType = event.ProgramType;
    this.deletedisplay = true;
}
 deleteProfessionalDetails() {
    this._professionalService.deleteProfessionalDetails(this.deleteID, this.professionalType).subscribe((data) => {
        this.messageService.add({severity:'success', summary: 'Success Message', detail:'Professional Details Deleted successfully.'});
        this.deletedisplay = false;
        this.getProfessionalDetail();
        this.formSubmitted = false;
        this.myForm.reset();
    }, (error) => {
        this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to Delete Professional Details.'});
    });
}
 cancelDeleteCancel() {
    this.deletedisplay = false;
}
 saveCertificationOrMembership() {
    this.formSubmitted = true;
    if (!this.myForm.valid) return;
    let currentYear = this.date.getFullYear();
    if (this.professionalDetail.ValidFrom > currentYear) return false;
    if (this.professionalDetail.ValidFrom > this.professionalDetail.ValidUpto) return false;
    switch (this.professionalDetail.ProgramType) {
        case ProgramType.certification:
            let certDetail: Certification;
            certDetail = new Certification();
            certDetail.EmployeeId = this.employeeId;
            if (this.upDateId) {
                certDetail.Id = this.upDateId;
            }
            certDetail.SkillGroupId = this.myForm.controls['skillGroupId'].value;
            certDetail.CertificationId = this.myForm.controls['skillId'].value;
            certDetail.ProgramType = this.professionalDetail.ProgramType;
            certDetail.Specialization = this.professionalDetail.Specialization;
            certDetail.Institution = this.professionalDetail.Institution;
            certDetail.ValidFrom = this.professionalDetail.ValidFrom;
            certDetail.ValidUpto = this.professionalDetail.ValidUpto;
            if (certDetail && (!certDetail.SkillGroupId || !certDetail.CertificationId))
                return;
            for (var i = 0; i < this.certificationList.length; i++) {
                if (certDetail.Id == -1 && (this.certificationList[i].CertificationId == certDetail.CertificationId && this.certificationList[i].SkillGroupId == certDetail.SkillGroupId
                    && this.certificationList[i].ProgramType == certDetail.ProgramType && this.certificationList[i].Institution.toLowerCase() == certDetail.Institution.toLowerCase()
                    && this.certificationList[i].Specialization.toLowerCase() == certDetail.Specialization.toLowerCase() && this.certificationList[i].ValidFrom == certDetail.ValidFrom 
                    && this.certificationList[i].ValidUpto == certDetail.ValidUpto)) {
                    this.messageService.add({severity:'warn', summary: 'waning Message', detail:'Certification details already exists.'});
                    return;
                }
            }
            if (certDetail && certDetail.Id == -1) {
                this._professionalService.addCertificationDetails(certDetail).subscribe((data) => {
                    this.messageService.add({severity:'success', summary: 'Success Message', detail:'Certification Details saved successfully.'});
                    this.getProfessionalDetail();
                    this.formSubmitted = false;
                    this.myForm.reset();
                }, (error) => {
                    this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to save Certification Details.'});
                });
            }
            if (certDetail && certDetail.Id != -1) {
                this._professionalService.updateCertificationDetails(certDetail).subscribe((data) => {
                    this.messageService.add({severity:'success', summary: 'Success Message', detail:'Certification Details Updated successfully.'});
                    this.getProfessionalDetail();
                    this.formSubmitted = false;
                    this.myForm.reset();
                    this.upDateId = -1;
                }, (error) => {
                    this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to Updated Certification Details.'});
                });
            }
            break;
        case ProgramType.membership:
            let memDetails: MemberShip
            memDetails = new MemberShip();
            memDetails.EmployeeId = this.employeeId;
            if (this.upDateId) {
                memDetails.Id = this.upDateId;
            }
            memDetails.ProgramType = this.professionalDetail.ProgramType;
            memDetails.ProgramTitle = this.myForm.controls['programTitle'].value;
            memDetails.Specialization = this.professionalDetail.Specialization;
            memDetails.Institution = this.professionalDetail.Institution;
            memDetails.ValidFrom = this.professionalDetail.ValidFrom;
            memDetails.ValidUpto = this.professionalDetail.ValidUpto;
            if (memDetails && !memDetails.ProgramTitle)
                return;
            for (var i = 0; i < this.memberShipList.length; i++) {
                if (memDetails.Id == -1 && (this.memberShipList[i].ProgramTitle == memDetails.ProgramTitle && this.memberShipList[i].ProgramType == memDetails.ProgramType
                    && this.memberShipList[i].Institution.toLowerCase() == memDetails.Institution.toLowerCase()
                    && this.memberShipList[i].Specialization.toLowerCase() == memDetails.Specialization.toLowerCase() 
                    && this.memberShipList[i].ValidFrom == memDetails.ValidFrom && this.memberShipList[i].ValidUpto == memDetails.ValidUpto)) {
                    this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Membership details already exists.'});
                    return;
                }
            }
            if (memDetails && memDetails.Id == -1) {
                this._professionalService.addMembershipDetails(memDetails).subscribe((data) => {
                    this.messageService.add({severity:'success', summary: 'Success Message', detail:'Membership Details saved successfully.'});
                    this.getProfessionalDetail();
                    this.formSubmitted = false;
                    this.myForm.reset();
                }, (error) => {
                    this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to save Membership Details.'});
                });
            }
            if (memDetails && memDetails.Id != -1) {
                this._professionalService.updateMembershipDetails(memDetails).subscribe((data) => {
                    this.messageService.add({severity:'success', summary: 'Success Message', detail:'Membership Details Updated successfully.'});
                    this.getProfessionalDetail();
                    this.formSubmitted = false;
                    this.myForm.reset();
                    this.upDateId = -1;
                }, (error) => {
                    this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to Updated Membership Details.'});
                });
            }
            break;
        default:
            break;
    }
    this.addDisplay = false;
}
 cancelOrclearValues() {
    this.addDisplay = false;
    this.myForm.reset();
    this.formSubmitted = false;
}

 onlyNumbers(event: any) {
    this._common.onlyNumbers(event)
};
 validFromYear(event: any) {
    this.validFromErrorMessage = "";
    let currentYear = this.date.getFullYear();
    this.validYear = event.target.value;
    if (this.validYear > currentYear) {
        this.validFromErrorMessage = "Valid From must be less than current year";
        return false;
    }
}
 validUpToYear(event: any) {
    this.validUpToErrorMessage = "";
    if (this.validYear > event.target.value) {
        this.validUpToErrorMessage = "Valid Up To must be greater than Valid From";
        return false;
    }
}

}







