import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Subscription } from 'rxjs';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { Associate } from '../../../onboarding/models/associate.model';
import { ActivityData, ActivityDetails, ActivityList } from '../../Models/activitymodel';
import { themeconfig } from 'src/themeconfig';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { AssociateExit } from '../../Models/associateExit.model';
import { ResignastionService } from '../../Services/resignastion.service';
import { MasterDataService } from 'src/app/core/services/masterdata.service';

@Component({
  selector: 'app-view-associate-exit-details',
  templateUrl: './view-associate-exit-details.component.html',
  styleUrls: ['./view-associate-exit-details.component.scss']
})
export class ViewAssociateExitDetailsComponent implements OnInit {


  themeConfigInput = themeconfig.formfieldappearances;
  currentPath: string;
  employeeId: number;
  serviceDeptID: number;
  empId: number;
  AssociateExit: AssociateExit;
  dashboard: string;
  hideBack = false;
  showback = true;
  UserRole: string;
  showProject = false;
  previous: string;
  constructor(
    private _router: Router,
    public dialog: MatDialog,
    public fb: FormBuilder,
    private actRoute: ActivatedRoute,
    private _resignationService: ResignastionService,
    private exitanalysis: ExitAnalysisService
  ) { }

  ngOnInit(): void {
    this.previous = this.exitanalysis.getPreviousUrl();
    this.actRoute.url.subscribe(url => { this.currentPath = url[0].path; });
    this.actRoute.params.subscribe(params => {
      this.empId = params.id;
    });
    if (sessionStorage.getItem('AssociatePortal_UserInformation') != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).roleName;
      this.UserRole = currentRole;
      this.employeeId = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).employeeId;
    }

    this._resignationService.getExitDetailsById(this.empId).subscribe((res: any) => {

      this.AssociateExit = new AssociateExit();
      this.AssociateExit = res;
      const resignationDetails = {
        ReasonId: this.AssociateExit.ReasonId,
        ReasonDetail: this.AssociateExit.ReasonDetail,
        RehireEligibility: this.AssociateExit.RehireEligibility,
        RehireEligibilityDetail: this.AssociateExit.RehireEligibilityDetail,
        ImpactOnClientDelivery: this.AssociateExit.ImpactOnClientDelivery,
        ImpactOnClientDeliveryDetail: this.AssociateExit.ImpactOnClientDeliveryDetail
      };
      this._resignationService.setResigReasonDtls(resignationDetails);
      if (this.AssociateExit.DepartmentId === 1){
        this.showProject = true;
      }

    });

  }

  onBack() {
    if (this.previous === '/associateexit/associateexitinformation') {
      this._router.navigate(['associateexit/associateexitinformation']);
    }
    else if(this.currentPath === 'abscond' || this.currentPath === 'abscond-deptchecklist'){
      this._router.navigate(['associateexit/abscond-dashboard'])
    }
    else {
      this._router.navigate(['shared/exit-actions']);
    }

  }
}
