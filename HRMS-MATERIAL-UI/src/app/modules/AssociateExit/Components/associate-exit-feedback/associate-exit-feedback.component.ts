import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import * as servicePath from '../../../../core/service-paths';

import * as moment from 'moment';
import { MatPaginator } from '@angular/material/paginator';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';

import { Associate } from 'src/app/modules/onboarding/models/associate.model';
import { AssociateInformationService } from 'src/app/modules/onboarding/services/associateInformation.service';
import { SendEmailComponent } from 'src/app/modules/onboarding/components/send-email/send-email.component';
import { AssociateExitFeedbackModalComponent } from '../associate-exit-feedback-modal/associate-exit-feedback-modal.component';
import { Validators, FormGroup, FormBuilder, FormGroupDirective} from '@angular/forms';
import { AnalysisData, AnalysisFilterData, AssociateDetailReviewData, ReportsFilterData } from '../../Models/analysis.model';
import { AssociateExitDashboardService } from 'src/app/modules/shared/services/associate-exit-dashboard.service';
import { themeconfig } from '../../../../../themeconfig';


@Component({
  selector: 'app-associate-exit-feedback',
  templateUrl: './associate-exit-feedback.component.html',
  styleUrls: ['./associate-exit-feedback.component.scss']
})
export class AssociateExitFeedbackComponent implements OnInit {
  step=0;
  associateInfo: any;
  private subType: string = "list";
  associateInfoList : AssociateDetailReviewData[];
  selectedRow : Associate;
  searchData: AnalysisFilterData;
  private resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = [
    'AssociateName',
    'InitialRemarks',
    'FinalRemarks',
    'ShowInitialRemarks',
    'Actions'
  ];
  dataSource: MatTableDataSource<AssociateDetailReviewData>;
  roleName: String;
  myForm: FormGroup;
  associate

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  isLoading: boolean;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  themeAppearence = themeconfig.formfieldappearances;


  constructor(private _associateinfoService : AssociateInformationService,private _router: Router,private _snackBar: MatSnackBar, public navService: NavService,   private spinner: NgxSpinnerService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private associateExitDashboardService:AssociateExitDashboardService) { 
    this.associateInfo = [];
    this.searchData = new AnalysisFilterData();
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
 
  
  ngOnInit(): void {
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    // this.getAssociateInformationList();
    this.myForm = this.fb.group({
      FromDate: ['', [Validators.required]],
      ToDate: [ '', [Validators.required]],
    });
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue){
        this.dataSource.filter = filterValue.trim().toLowerCase();
        }
        else{
        this.dataSource = new MatTableDataSource(this.associateInfoList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
          if (typeof data[sortHeaderId] === 'string') {
            return data[sortHeaderId].toLocaleLowerCase();
          }
        
          return data[sortHeaderId];
        };
        }
    } else {
      this.dataSource = new MatTableDataSource(this.associateInfoList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };
      }
  }
   
  
    editAssociatejoining(selectedData: any) {
      let currentID = selectedData.EmpId;
      selectedData.associateType = selectedData.EmpId != 0 ? "edit" : "new";
      this._router.navigate(['/associates/prospectivetoassociate/' + selectedData.associateType + '/' + currentID + '/' + this.subType]);
    }

    submitReport() {
      if(this.myForm.valid){
        this.step = 1;
      }
      this.isLoading = true;
      if (this.myForm.controls.FromDate.value === '' || this.myForm.controls.ToDate.value === '' || this.myForm.controls.FromDate.value === null || this.myForm.controls.ToDate.value === null) {
        this.isLoading = false;
        return;
      }
  
      if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
        if (moment(this.searchData.FromDate).isAfter(new Date())) {
          this.isLoading = false;
          this.snackBar.open('From date should be less than today', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
          });
  
          return false;
        }
        if (moment(this.searchData.ToDate).isAfter(new Date())) {
          this.isLoading = false;
          this.snackBar.open('To date should be less than today', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
          });
          return false;
        }
        if (moment(this.searchData.FromDate).isAfter(this.searchData.ToDate)) {
          this.isLoading = false;
          this.snackBar.open('From Date should be less than To Date', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
          });
          return false;
        }
        this.associateInfoList= [];
        let from_date =  moment(this.searchData.FromDate).format('YYYY-MM-DD');
        let to_date =  moment(this.searchData.ToDate).format('YYYY-MM-DD');
        this.spinner.show();
        this.associateExitDashboardService.associateExitInterviewReview(from_date, to_date).subscribe((res: AssociateDetailReviewData[]) => {
  
          if(res != null && res.length > 0) {          
            this.isLoading = false;
            this.associateInfoList=res;
            this.associateInfoList.forEach((record: AssociateDetailReviewData) => {            
              record.ShowInitialRemarksStr = (record.ShowInitialRemarks == true) ? 'Yes' : 'No';                     
            });
            this.dataSource = new MatTableDataSource(this.associateInfoList);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            this.spinner.hide();
            this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
              if (typeof data[sortHeaderId] === 'string') {
                return data[sortHeaderId].toLocaleLowerCase();
              }
            
              return data[sortHeaderId];
            };          
        }
          else {        
            this.spinner.hide();   
            this.isLoading = false;   
            this.dataSource = new MatTableDataSource(this.associateInfoList);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;         
            this._snackBar.open('No records found', 'x', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }           
         
        },
  
        );
      }
    }

    sendMail(currentAssociate:Associate){
      let dialogRef = this.dialog.open(AssociateExitFeedbackModalComponent, {
        width: '70vw', height: 'auto',
        disableClose: true,
        data: { element: currentAssociate }
      }); 
      dialogRef.afterClosed().subscribe(result => {
        this.submitReport() 
      });
    }

    clear() {
      this.myForm.reset();
      this.associateInfoList=[];
      this.dataSource = new MatTableDataSource(this.associateInfoList);      
      // this.formGroupDirective.resetForm();
      // this.searchFormSubmitted = false;
      // this.errorSummary = '';
      // this.searchData = new AnalysisFilterData();
      // this.showGrid = false;
    }

}
