import { Component, OnInit, Inject, TemplateRef, ViewChildren, QueryList } from '@angular/core';
import * as servicePath from '../../../../core/service-paths';
import { MemberShip, Certification, Professional } from '../../models/professionaldetails.model';
import { ProfessionalService } from '../../services/professional.service';
import { CommonService } from '../../../../core/services/common.service';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { FormGroup, FormGroupDirective } from '@angular/forms';
import { DropDownType, SkillGroupDropDown, SkillDropDown } from '../../../master-layout/models/dropdowntype.model';
import { ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ProgramType } from '../../../shared/utility/enums';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig } from '@angular/material/dialog';

import { themeconfig } from '../../../../../themeconfig';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { ConstantPool } from '@angular/compiler';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-associate-professional',
  templateUrl: './associate-professional.component.html',
  styleUrls: ['./associate-professional.component.scss']
})
export class AssociateProfessionalComponent implements OnInit {
  myForm: FormGroup;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  formSubmitted: boolean = false;
  employeeId: number;
  themeAppearence = themeconfig.formfieldappearances;
  certificationList: Professional[] = [];
  memberShipList: Professional[] = [];
  professionalList: Professional[];
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
  dialogResponse: boolean;
  skillGroupID: number;
  date = new Date();
  upDateId: number;
  validYear: number;
  validFromErrorMessage: string = "";
  validUpToErrorMessage: string = "";
  dataSource: MatTableDataSource<Professional>;
  dataSource2: MatTableDataSource<Professional>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  @ViewChild('AssociateProfessionalDialog') associateProfessionalDialog: TemplateRef<any>;

  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;

  private AssociateProfessionalDialogRef: MatDialogRef<TemplateRef<any>>;
  certificationCols = [
    { field: 'SkillGroupName', header: 'SkillGroupName' },
    { field: 'SkillName', header: 'SkillName' },
    { field: 'Institution', header: 'Institution' },
    { field: 'Specialization', header: 'Specialization' },
    { field: 'ValidFrom', header: 'ValidFrom' },
    { field: 'ValidUpto', header: 'ValidUpto' }
  ];
  displayedColumns = ['SkillGroupName', 'SkillName', 'Institution', 'Specialization', 'ValidFrom', 'ValidUpto', 'Action', 'Delete'];
  membershipCols = [
    { field: 'ProgramTitle', header: 'MembershipTitle' },
    { field: 'Institution', header: 'Institution' },
    { field: 'Specialization', header: 'Specialization' },
    { field: 'ValidFrom', header: 'ValidFrom' },
    { field: 'ValidUpto', header: 'ValidUpto' }
  ];
  membershipCols1 = ['MembershipTitle', 'Institution', 'Specialization', 'ValidFrom', 'ValidUpto', 'Action2', 'Delete2'];


  constructor(public dialog: MatDialog, private snackBar: MatSnackBar, public fb: FormBuilder, private actRoute: ActivatedRoute, private _common: CommonService, private _professionalService: ProfessionalService, private masterDataService: MasterDataService,private spinner: NgxSpinnerService) {
    this.programType = new Array<DropDownType>();
    this.skillGroups = new Array<DropDownType>();
    this.skills = new Array<DropDownType>();
    this.professionalList = new Array<Professional>();
    this.professionalDetail = new Professional();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.spinner.show()
    this.formSubmitted = false;
    this.actRoute.params.subscribe(params => { this.employeeId = params.id; });
    // this.employeeId = 219;
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
    this.programType.push({ label: 'Certificate', value: ProgramType.certification });
    this.programType.push({ label: 'Member Ship', value: ProgramType.membership });
    this.myForm = this.fb.group({
      programType: [null, [Validators.required]],
      skillGroupId: [null, null],
      skillId: [null, null],
      programTitle: [null, [Validators.pattern('[A-Za-z][a-zA-Z\\s]{2,64}')]],
      specialization: [null, [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]],
      institution: [null, [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]],
      validFrom: [null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$')]],
      validUpTo: [null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$')]]
    });
    this.getProfessionalDetail();
    this.skillGroups = [];
    this.skillGroups.push({ label: 'Select Skill Group', value: null });
    this.skills = [];
    this.skills.push({ label: 'Select Skill Name', value: null });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  applyFilter1(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource2.filter = filterValue.trim().toLowerCase();
  }
  openDialog(): void {
    const dialogConfig = new MatDialogConfig();
    this.AssociateProfessionalDialogRef = this.dialog.open(this.associateProfessionalDialog, {
      disableClose: true,
      hasBackdrop: true,
    });


    this.AssociateProfessionalDialogRef.afterClosed().subscribe(result => {
    });
  }
  closeDialog() {
    this.AssociateProfessionalDialogRef.close();
  }
  getProfessionalDetail() {
    this._professionalService.getProfessionalDetails(this.employeeId).subscribe((professionalList: Professional[]) => {
      this.spinner.hide()
      this.fillCertifications(professionalList);
      this.fillMembership(professionalList);
    }, (error: any) => {
      this.spinner.hide()
      this.snackBar.open('Failed to get Professional Details', 'x', {
        duration: 5000,
        panelClass:['error-alert'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    });
  }
  fillCertifications(professionalList: Array<Professional>) {
    this.certificationList = professionalList.filter(x => x.ProgramType === ProgramType.certification);
    this.dataSource = new MatTableDataSource(this.certificationList);
    this.dataSource.paginator = this.paginator.toArray()[0];
    this.dataSource.sort = this.sort.toArray()[0];
    return this.certificationList;
  }
  fillMembership(professionalList: Array<Professional>) {
    this.memberShipList = professionalList.filter(x => x.ProgramType === ProgramType.membership);
    this.dataSource2 = new MatTableDataSource(this.memberShipList);
    this.dataSource2.paginator = this.paginator.toArray()[1];
    this.dataSource2.sort = this.sort.toArray()[1];
    return this.memberShipList;
  }
  getSkillGroup() {
    this._professionalService.getSkillGroupCertificate().subscribe((skillGroupList: SkillGroupDropDown[]) => {
      this.skillGroups = [];
      skillGroupList.forEach((e: SkillGroupDropDown) => {
        this.skillGroups.push({ label: e.SkillGroupName, value: e.SkillGroupId })
      });
    }, (error: any) => {
      // this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to get Skill Group List.'});
      this.snackBar.open('Failed to get Skill Group List.', 'x', {
        duration: 5000,
        panelClass:['error-alert'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    });
  }
  skilGroupChange(event: any) {
    this.professionalDetail.SkillGroupId = this.myForm.controls.skillGroupId.value;
    this.skillGroupID = -1;
    this.skillGroupID = event.value;
    this.myForm.controls.skillId.setValue(null);
    this.getSkillNames();
  }
  getSkillNames() {
    this.masterDataService.GetSkillsBySkillGroups(this.skillGroupID).subscribe((skillList: SkillDropDown[]) => {
      this.skills = [];
      this.skills.push({ label: 'Select Skill Name', value: null });
      skillList.forEach((e: SkillDropDown) => {
        this.skills.push({ label: e.SkillName, value: e.SkillId });
      });

    }, (error: any) => {
      this.snackBar.open('Failed to get Skill Group List.', 'x', {
        duration: 5000,
        panelClass:['error-alert'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    });
  }
  addCertificationOrMembership() {
    this.getSkillGroup();
    this.getSkillNames();
    this.validFromErrorMessage = '';
    this.validUpToErrorMessage = '';
    this.myForm.reset();
    this.formSubmitted = false;
    this.inputDisable = false;
    if (this.inputDisable) {
      this.myForm.controls.programType.disable();
      this.myForm.controls.skillGroupId.disable();
      this.myForm.controls.skillId.disable();
    }
    else {
      this.myForm.controls.programType.enable();
      this.myForm.controls.skillGroupId.enable();
      this.myForm.controls.skillId.enable();
    }
    this.professionalDetail.ProgramType = null;
    this.membership = false;
    this.certification = false;
    this.addDisplay = true;
    this.openDialog();
  }
  programTypeChange(event: any) {
    this.professionalDetail.ProgramType = this.myForm.controls.programType.value;
    this.validFromErrorMessage = '';
    this.validUpToErrorMessage = '';
    this.myForm.controls.programType.setValue(event.value);
    if (event.value === ProgramType.certification) {
      this.myForm.controls.skillGroupId.reset();
      this.myForm.controls.skillId.reset();
      this.myForm.controls.institution.reset();
      this.myForm.controls.specialization.reset();
      this.myForm.controls.institution.reset();
      this.myForm.controls.validFrom.reset();
      this.myForm.controls.validUpTo.reset();
    }
    if (event.value === ProgramType.membership) {
      this.myForm.controls.programTitle.reset();
      this.myForm.controls.institution.reset();
      this.myForm.controls.specialization.reset();
      this.myForm.controls.institution.reset();
      this.myForm.controls.validFrom.reset();
      this.myForm.controls.validUpTo.reset();
    }
    this.formSubmitted = false;
    if (event.value === ProgramType.certification) {
      this.certification = true;
      this.membership = false;
    }
    else if (event.value === ProgramType.membership) {
      this.membership = true;
      this.certification = false;
    }
    else {
      this.membership = false;
      this.certification = false;
    }

  }
  getCertificationEdit(event: any) {
    this.openDialog();
    this.skillGroupID = event.SkillGroupId;
    this.getSkillGroup();
    this.getSkillNames();
    this.addDisplay = true;
    this.certification = true;
    this.membership = false;
    this.inputDisable = true;
    this.upDateId = event.Id;
    if (this.inputDisable) {
      this.myForm.controls.programType.disable();
      this.myForm.controls.skillGroupId.disable();
      this.myForm.controls.skillId.disable();
    }
    else {
      this.myForm.controls.programType.enable();
      this.myForm.controls.skillGroupId.enable();
      this.myForm.controls.skillId.enable();
    }
    this.myForm.controls.programType.setValue(event.ProgramType);
    this.myForm.controls.skillGroupId.setValue(event.SkillGroupId);
    this.myForm.controls.skillId.setValue(event.CertificationId);
    this.myForm.controls.institution.setValue(event.Institution);
    this.myForm.controls.specialization.setValue(event.Specialization);
    this.myForm.controls.validFrom.setValue(event.ValidFrom);
    this.myForm.controls.validUpTo.setValue(event.ValidUpto);
    this.professionalDetail.ProgramType = event.ProgramType;
    this.professionalDetail.SkillGroupId = event.SkillGroupId;
    this.professionalDetail.CertificationId = event.CertificationId;
    this.professionalDetail.Specialization = event.Specialization;
    this.professionalDetail.Institution = event.Institution;
    this.professionalDetail.ValidFrom = event.ValidFrom;
    this.professionalDetail.ValidUpto = event.ValidUpto;
    this.professionalDetail.ProgramType = event.ProgramType;
    this.professionalDetail.SkillGroupId = event.SkillGroupId;
    this.professionalDetail.CertificationId = event.CertificationId;
    this.professionalDetail.Specialization = event.Specialization;
    this.professionalDetail.Institution = event.Institution;
    this.professionalDetail.ValidFrom = event.ValidFrom;
    this.professionalDetail.ValidUpto = event.ValidUpto;
  }
  getMembershipEdit(event: any) {
    this.openDialog();
    this.membership = true;
    this.certification = false;
    this.inputDisable = true;
    if (this.inputDisable) {
      this.myForm.controls.programType.disable();
      this.myForm.controls.skillGroupId.disable();
      this.myForm.controls.skillId.disable();
    }
    else {
      this.myForm.controls.programType.enable();
      this.myForm.controls.skillGroupId.enable();
      this.myForm.controls.skillId.enable();
    }
    this.upDateId = event.Id;
    this.myForm.controls.programType.setValue(event.ProgramType);
    this.myForm.controls.institution.setValue(event.Institution);
    this.myForm.controls.specialization.setValue(event.Specialization);
    this.myForm.controls.validFrom.setValue(event.ValidFrom);
    this.myForm.controls.validUpTo.setValue(event.ValidUpto);
    this.myForm.controls.programTitle.setValue(event.ProgramTitle);
    this.professionalDetail.ProgramType = event.ProgramType;
    this.professionalDetail.ProgramTitle = event.ProgramTitle;
    this.professionalDetail.Specialization = event.Specialization;
    this.professionalDetail.Institution = event.Institution;
    this.professionalDetail.ValidFrom = event.ValidFrom;
    this.professionalDetail.ValidUpto = event.ValidUpto;
  }
  deleteCertificateOrMembership(event: any) {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: '300px',
      data: { heading: 'Confirmation', message: 'Do you want to Delete?' }
    });


    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        this.deleteID = event.Id;
        this.professionalType = event.ProgramType;
        this.deletedisplay = true;
        this.deleteProfessionalDetails();
        const dialogConf = this.dialog.open(MessageDialogComponent, {
          disableClose: true,
          hasBackdrop: true,
          width: '300px',
          data: { heading: 'Confirmation', message: 'Professional Details deleted successfully' }
        })
      }
    });

  }
  deleteProfessionalDetails() {
    this._professionalService.deleteProfessionalDetails(this.deleteID, this.professionalType).subscribe((data) => {
      this.deletedisplay = false;
      this.getProfessionalDetail();
      this.formSubmitted = false;
      this.myForm.reset();
    }, (error) => {
      this.snackBar.open('Failed to Delete Professional Details.', 'x', {
        duration: 5000,
        panelClass:['error-alert'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    });
  }
  cancelDeleteCancel() {
    this.closeDialog();
  }

  dynamicClearValidations(eduControl: string, newFormEduValidate: FormGroup){
    newFormEduValidate.controls[eduControl].setErrors(null);
    newFormEduValidate.controls[eduControl].clearValidators();
    newFormEduValidate.controls[eduControl].updateValueAndValidity();       
  }

  saveCertificationOrMembership() {

    this.formSubmitted = true;
    
    if (this.myForm.controls.programType.value === 3){
      this.dynamicClearValidations('programTitle', this.myForm)
      this.myForm.controls['skillId'].setValidators([Validators.required])
      this.myForm.controls['skillId'].updateValueAndValidity();
      this.myForm.controls['skillGroupId'].setValidators([Validators.required])
      this.myForm.controls['skillGroupId'].updateValueAndValidity();
    }
    else if (this.myForm.controls.programType.value === 4){
      this.myForm.controls['programTitle'].setValidators([Validators.required, Validators.pattern('[A-Za-z][a-zA-Z\\s]{2,64}')])
      this.myForm.controls['programTitle'].updateValueAndValidity();
      this.dynamicClearValidations('skillId', this.myForm)
      this.dynamicClearValidations('skillGroupId', this.myForm)
    }
    
    if (this.myForm.invalid) return;
    this.professionalDetail.ValidFrom = this.myForm.controls.validFrom.value;
    this.professionalDetail.ValidUpto = this.myForm.controls.validUpTo.value;

    let currentYear = this.date.getFullYear();
    if (this.professionalDetail.ValidFrom > currentYear || this.professionalDetail.ValidFrom > this.professionalDetail.ValidUpto) {
      if (this.professionalDetail.ValidFrom > currentYear) {
        this.snackBar.open('Valid From should be current year or less', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
      else {
        this.snackBar.open('Valid From should be less than Valid To', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
      return false;
    }

    switch (this.professionalDetail.ProgramType) {
      case ProgramType.certification:
        let certDetail: Certification;
        certDetail = new Certification();
        certDetail.EmployeeId = this.employeeId;
        if (this.upDateId) {
          certDetail.Id = this.upDateId;
        }
        certDetail.SkillGroupId = this.myForm.controls.skillGroupId.value;
        certDetail.CertificationId = this.myForm.controls.skillId.value;
        certDetail.ProgramType = this.professionalDetail.ProgramType;
        certDetail.Specialization = this.myForm.controls.specialization.value;
        certDetail.Institution = this.myForm.controls.institution.value;
        certDetail.ValidFrom = this.myForm.controls.validFrom.value;
        certDetail.ValidUpto = this.myForm.controls.validUpTo.value;
        if (certDetail && (!certDetail.SkillGroupId || !certDetail.CertificationId)) {
          return;
        }
        for (var i = 0; i < this.certificationList.length; i++) {
          if (certDetail.Id == -1 && (this.certificationList[i].CertificationId == certDetail.CertificationId && this.certificationList[i].SkillGroupId == certDetail.SkillGroupId
            && this.certificationList[i].ProgramType == certDetail.ProgramType && this.certificationList[i].Institution.toLowerCase() == certDetail.Institution.toLowerCase()
            && this.certificationList[i].Specialization.toLowerCase() == certDetail.Specialization.toLowerCase() && this.certificationList[i].ValidFrom == certDetail.ValidFrom
            && this.certificationList[i].ValidUpto == certDetail.ValidUpto)) {
            this.snackBar.open('Certification details already exists.', 'x', {
              duration: 5000,
              panelClass:['error-alert'],
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            return;
          }
        }
        if (certDetail && certDetail.Id === -1) {
          this._professionalService.addCertificationDetails(certDetail).subscribe((data) => {
            this.snackBar.open('Certification Details saved successfully.', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            this.getProfessionalDetail();
            this.formSubmitted = false;
            this.formGroupDirective.resetForm();
            //this.closeDialog();
          }, (error) => {
            this.snackBar.open('Failed to save Certification Details.', 'x', {
              duration: 5000,
              panelClass:['error-alert'],
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
          });
        }
        if (certDetail && certDetail.Id !== -1) {
          this._professionalService.updateCertificationDetails(certDetail).subscribe((data) => {
            this.snackBar.open('Certification Details Updated successfully.', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            this.getProfessionalDetail();
            this.formSubmitted = false;
            this.myForm.reset();
            this.upDateId = -1;
          }, (error) => {
            this.snackBar.open('Failed to Update Certification Details.', 'x', {
              duration: 5000,
              panelClass:['error-alert'],
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
          });
        }
        break;
      case ProgramType.membership:
        let memDetails: MemberShip;
        memDetails = new MemberShip();
        memDetails.EmployeeId = this.employeeId;
        if (this.upDateId) {
          memDetails.Id = this.upDateId;
        }
        memDetails.ProgramType = this.myForm.controls.programType.value;
        memDetails.ProgramTitle = this.myForm.controls.programTitle.value;
        memDetails.Specialization = this.myForm.controls.specialization.value;
        memDetails.Institution = this.myForm.controls.institution.value;
        memDetails.ValidFrom = this.myForm.controls.validFrom.value;
        memDetails.ValidUpto = this.myForm.controls.validUpTo.value;
        if (memDetails && !memDetails.ProgramTitle) {
          return;
        }
        for (var i = 0; i < this.memberShipList.length; i++) {
          if (memDetails.Id == -1 && (this.memberShipList[i].ProgramTitle == memDetails.ProgramTitle && this.memberShipList[i].ProgramType == memDetails.ProgramType
            && this.memberShipList[i].Institution.toLowerCase() == memDetails.Institution.toLowerCase()
            && this.memberShipList[i].Specialization.toLowerCase() == memDetails.Specialization.toLowerCase()
            && this.memberShipList[i].ValidFrom == memDetails.ValidFrom && this.memberShipList[i].ValidUpto == memDetails.ValidUpto)) {
            this.snackBar.open('Membership details already exists.', 'x', {
              duration: 5000,
              panelClass:['error-alert'],
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            return;
          }
        }
        if (memDetails && memDetails.Id === -1) {
          this._professionalService.addMembershipDetails(memDetails).subscribe((data) => {
            this.snackBar.open('Membership Details saved successfully.', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            this.getProfessionalDetail();
            this.formSubmitted = false;
            this.formGroupDirective.reset();
            this.closeDialog();
          }, (error) => {
            this.snackBar.open('Failed to save Membership Details.', 'x', {
              duration: 5000,
              panelClass:['error-alert'],
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
          });
        }
        if (memDetails && memDetails.Id !== -1) {
          this._professionalService.updateMembershipDetails(memDetails).subscribe((data) => {
            this.snackBar.open('Membership Details Updated successfully.', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
            this.getProfessionalDetail();
            this.formSubmitted = false;
            this.myForm.reset();
            this.upDateId = -1;
            this.closeDialog();
          }, (error) => {
            this.snackBar.open('Failed to Update Membership Details.', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
            });
          });
        }
        break;
      default:
        break;
    }
    this.addDisplay = false;
    this.closeDialog();
  }
  cancelOrclearValues() {
    this.addDisplay = false;
    this.myForm.reset();
    this.formSubmitted = false;
    this.closeDialog();
  }

  onlyNumbers(event: any) {
    this._common.onlyNumbers(event)
  };
  validFromYear(event: any) {
    this.validFromErrorMessage = '';
    let currentYear = this.date.getFullYear();
    this.validYear = event.target.value;
    if (this.validYear > currentYear) {
      this.validFromErrorMessage = 'Valid From must be less than current year';
      return false;
    }
  }
  validUpToYear(event: any) {
    this.validUpToErrorMessage = '';
    if (this.validYear > event.target.value) {
      this.validUpToErrorMessage = 'Valid Up To must be greater than Valid From';
      return false;
    }
  }

}
