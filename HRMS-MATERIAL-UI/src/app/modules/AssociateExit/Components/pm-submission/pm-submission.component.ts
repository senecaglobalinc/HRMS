import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonDialogComponent } from 'src/app/modules/shared/components/common-dialog/common-dialog.component';
import { themeconfig } from 'src/themeconfig';
import { AssociateExit, AssociateExitPMRequest } from '../../Models/associateExit.model';
import { PmApprovalService } from '../../Services/pm-approval.service';
import { ResignastionService } from '../../Services/resignastion.service';

@Component({
  selector: 'app-pm-submission',
  templateUrl: './pm-submission.component.html',
  styleUrls: ['./pm-submission.component.scss']
})
export class PMSubmissionComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  PMApprovalform: FormGroup;
  AssociateExit: AssociateExit;
  employeeId: number;
  resignationApproved = false;
  btnLabel = 'Submit';
  UserRole: string;
  showClientImpact = true;
  sendText = false;
  PMApprovalObj: AssociateExitPMRequest;
  currentloggedInUser: number;

  constructor(private actRoute: ActivatedRoute,
              private _PmApprovalService: PmApprovalService,
              private _snackBar: MatSnackBar,
              private _router: Router,
              private dialog: MatDialog,
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
      this.currentloggedInUser = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).employeeId;
    }

    if (this.UserRole === 'Operations Head') {
      this.showClientImpact = false;
    }


    this.createPMApprovalForm();
    this.getExitDetailsById();

  }
  getExitDetailsById() {
    this.spinner.show();
    this._resignationService.getExitDetailsById(this.employeeId).subscribe((res: any) => {
      this.spinner.hide();
      this.AssociateExit = new AssociateExit();
      this.AssociateExit = res;
      if (this.AssociateExit.Status !== 'ResignationSubmitted') {

        this.PMApprovalform.patchValue({
          impactOnClientDelivery: this.AssociateExit.ImpactOnClientDelivery,
          impactOnClientDeliveryDetail: this.AssociateExit.ImpactOnClientDeliveryDetail,
          rehireEligibility: this.AssociateExit.RehireEligibility,
          rehireEligibilityDetail: this.AssociateExit.RehireEligibilityDetail,
          resignationRecommendation: this.AssociateExit.ResignationRecommendation,
        });

        this.PMApprovalform.disable();
      }
    });
  }
  LastWorkingDateChange(LWD: any) {
    this.AssociateExit.ExitDate = LWD;
  }

  createPMApprovalForm() {
    this.PMApprovalform = new FormGroup({
      impactOnClientDelivery: new FormControl(false, [Validators.required]),
      impactOnClientDeliveryDetail: new FormControl(null),
      rehireEligibility: new FormControl(false, [Validators.required]),
      rehireEligibilityDetail: new FormControl(null),
    });

  }

  Submit() {
    if (this.PMApprovalform.valid) {
      const dialogRef = this.dialog.open(CommonDialogComponent, {
        width: '300px',
        disableClose: true,
        data: {
          heading: 'Confirmation',
          message: 'Are you sure, You want to Submit?',
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result === true) {
          this.PMApprovalObj = {
            AssociateExitId: this.AssociateExit.AssociateExitId,
            EmployeeId: this.AssociateExit.EmployeeId,
            ExitTypeId: this.AssociateExit.ExitTypeId,
            RehireEligibility: this.PMApprovalform.value.rehireEligibility,
            RehireEligibilityDetail: this.PMApprovalform.value.rehireEligibilityDetail,
            ImpactOnClientDelivery: this.PMApprovalform.value.impactOnClientDelivery,
            ImpactOnClientDeliveryDetail: this.PMApprovalform.value.impactOnClientDeliveryDetail,
            ProgramManagerId: this.currentloggedInUser
          };
          this.spinner.show();
          this._PmApprovalService.ReviewByPM(this.PMApprovalObj).subscribe(res => {
            const IsSuccessful = 'IsSuccessful';
            this.spinner.hide();
            if (res[IsSuccessful]) {
              this._snackBar.open('Submitted Details Successfully.', 'x', {
                duration: 1000,
                panelClass: ['success-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              if(this.UserRole == 'Team Lead'){
                this._router.navigate(['/shared/dashboard']);
              }
              else{
                this._router.navigate(['/shared/exit-actions']);
              }
            }
            else {
              this._snackBar.open('Unable to submit details', 'x', {
                duration: 1000,
                panelClass: ['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
          },
            (error) => {
              this.spinner.hide();
              this._snackBar.open(error.error, 'x', {
                duration: 1000,
                panelClass: ['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            });
        }
      });
    }
  }


}
