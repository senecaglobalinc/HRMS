import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { themeconfig } from 'src/themeconfig';
import { AssociateExit } from '../../Models/associateExit.model';
import { PmApprovalService } from '../../Services/pm-approval.service';
import { ResignastionService } from '../../Services/resignastion.service';

@Component({
  selector: 'app-pm-approval',
  templateUrl: './pm-approval.component.html',
  styleUrls: ['./pm-approval.component.scss']
})
export class PmApprovalComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  PMApprovalform: FormGroup;
  AssociateExit: AssociateExit;
  employeeId: number;
  resignationApproved = false;
  btnLabel = 'Accept';
  UserRole: string;
  showClientImpact = true;
  sendText: boolean;
  changedLWD: any;
  updateLWD = false;
  constructor(private actRoute: ActivatedRoute,
              private _PmApprovalService: PmApprovalService,
              private _snackBar: MatSnackBar,
              private _router: Router,
              private spinner: NgxSpinnerService,
              private _resignationService: ResignastionService) { }

  ngOnInit(): void {
    this.actRoute.params.subscribe(params => {
      this.employeeId = params.id;
    });

    if (sessionStorage.getItem('AssociatePortal_UserInformation') != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).roleName;
      this.UserRole = currentRole;
    }

    if (this.UserRole === 'Operations Head') {
      this.showClientImpact = false;
    }


    this.createPMApprovalForm();
    this.getPMApprovalData();

  }

  LastWorkingDateChange(LWD: any) {
    this.changedLWD = LWD;
  }

  createPMApprovalForm() {
    this.PMApprovalform = new FormGroup({
      impactOnClientDelivery: new FormControl(null),
      impactOnClientDeliveryDetail: new FormControl(null),
      rehireEligibility: new FormControl(null),
      rehireEligibilityDetail: new FormControl(null),
      resignationRecommendation: new FormControl(null),
    });

  }

  getPMApprovalData() {
    this.spinner.show();
    this._resignationService.getExitDetailsById(this.employeeId).subscribe((res: any) => {
      this.spinner.hide();
      this.AssociateExit = new AssociateExit();
      this.AssociateExit = res;
      this.changedLWD = this.AssociateExit.ExitDate;

      if (this.UserRole === 'HRM') {

        this.PMApprovalform.patchValue({
          impactOnClientDelivery: this.AssociateExit.ImpactOnClientDelivery,
          impactOnClientDeliveryDetail: this.AssociateExit.ImpactOnClientDeliveryDetail,
          rehireEligibility: this.AssociateExit.RehireEligibility,
          rehireEligibilityDetail: this.AssociateExit.RehireEligibilityDetail,
          resignationRecommendation: this.AssociateExit.ResignationRecommendation,
        });

        this.PMApprovalform.controls.impactOnClientDelivery.disable();
        this.PMApprovalform.controls.impactOnClientDeliveryDetail.disable();
        this.PMApprovalform.controls.rehireEligibilityDetail.disable();
        this.PMApprovalform.controls.rehireEligibility.disable();
        if (this.AssociateExit.Status === 'ResignationReviewed') {
          this.btnLabel = 'Accept';
        }
        else {
          this.btnLabel = 'Update';
        }
        this.resignationApproved = true;


      }
      else {
        this.PMApprovalform.disable();
      }
    });

  }

  approveResignation() {
    if(this.AssociateExit.ExitDate){      
      if (this.btnLabel === 'Accept') {
        this.AssociateExit.ExitDate = this.changedLWD;
        this.AssociateExit.SubmitType = 'Approve';
        this.AssociateExit.ImpactOnClientDelivery = this.PMApprovalform.value.impactOnClientDelivery;
        this.AssociateExit.ImpactOnClientDeliveryDetail = this.PMApprovalform.value.impactOnClientDeliveryDetail;
        this.AssociateExit.RehireEligibility = this.PMApprovalform.value.rehireEligibility;
        this.AssociateExit.RehireEligibilityDetail = this.PMApprovalform.value.rehireEligibilityDetail;
        this.AssociateExit.ResignationRecommendation = this.PMApprovalform.value.resignationRecommendation;


        this.spinner.show();
        this._PmApprovalService.submitPMApproval(this.AssociateExit).subscribe(res => {
          this.spinner.hide();
          if (res) {
            this._snackBar.open('Resignation Accepted successfully.', 'x', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this._router.navigate(['/shared/exit-actions']);
          }
        },
          error => {
            this.spinner.hide();
            this._snackBar.open('Unable to accept resignation', 'x', {
              duration: 1000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });
      }
      else {
        if (moment(this.changedLWD).format('YYYY-MM-DD') === moment(this.AssociateExit.ExitDate).format('YYYY-MM-DD')) {
          this._snackBar.open('No changes to update', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else {
          this.AssociateExit.ExitDate = moment(this.changedLWD).format('YYYY-MM-DD');
          this.spinner.show();
          this.updateLWD =  false;
          this._PmApprovalService.submitPMApproval(this.AssociateExit).subscribe(res => {
            this.spinner.hide();
            if (res && res['IsSuccessful']) {
              if(res['Message']){
                this._snackBar.open(res['Message'], 'x', {
                  duration: 5000,
                  panelClass: ['warning-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.updateLWD =  true;
              }
              else{
                this._snackBar.open('Last Working Date Updated successfully.', 'x', {
                  duration: 1000,
                  panelClass: ['success-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this._router.navigate(['/shared/exit-actions']);
              }
            }
          }, error => {
            this.spinner.hide();
            this._snackBar.open('Unable to update Last Working Date', 'x', {
              duration: 1000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });
        }

      }
    }
  }

  withdrawRequest() {
    this.PMApprovalform.controls.resignationRecommendation.setValidators(Validators.required);
    this.PMApprovalform.controls.resignationRecommendation.updateValueAndValidity();
    if (this.PMApprovalform.value.resignationRecommendation) {
      this._PmApprovalService.withdrawRequest(this.AssociateExit.EmployeeId, this.PMApprovalform.value.resignationRecommendation).
        subscribe(res => {
          if (res) {
            this._snackBar.open('Resignation Withdrawl Request Sent.', 'x', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this._router.navigate(['/shared/exit-actions']);
          }
        },
        (error) => {
          this._snackBar.open('Unable to Send Request.', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        });
    }
  }

  reject() {

  }

}
