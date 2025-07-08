import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Associate } from '../../models/associate.model';
import * as servicePath from '../../../../core/service-paths';
import { AssociateInformationService } from '../../services/associateInformation.service';
import * as moment from 'moment';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { SendEmailComponent } from '../send-email/send-email.component';
@Component({
  selector: 'app-associateinformation',
  templateUrl: './associateinformation.component.html',
  styleUrls: ['./associateinformation.component.scss']
})
export class AssociateinformationComponent implements OnInit {
  associateInfo: any;
  private subType: string = "list";
  associateInfoList : Associate[];
  selectedRow : Associate;
  private resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = [
    'EmpCode',
    'EmpName',
    'MobileNo',
    'PersonalEmailAddress',
    'BgvStatus',
    'Edit',
  ];
  dataSource: MatTableDataSource<Associate> = new MatTableDataSource();
  roleName: String;
  TotalCount: number;
  pageSize = 10;
  currentPage = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  searchString: any = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  constructor(private _associateinfoService : AssociateInformationService,private _router: Router,private _snackBar: MatSnackBar, public navService: NavService,   private spinner: NgxSpinnerService,
    public dialog: MatDialog,) { 
    this.associateInfoList = [];
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      if(responseData) {
        this.searchString = (responseData.target as HTMLInputElement).value;
      }
      else{
        this.searchString = ''
      }
      if(this.searchString == '' || this.searchString.length >= 3){
        this.currentPage = 0;
        this.getEmployeeCount();
        this.getAssociateInformationList();
      }
    });
  }
 
  
  ngOnInit(): void {
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.getAssociateInformationList();
  }

  getEmployeeCount(){
    this._associateinfoService.getEmployeeCount(this.searchString).subscribe((res: any) => {
      this.TotalCount = res;
    });
  }

  getAssociateInformationList(){
  this.spinner.show();
  this._associateinfoService.getAssociates(this.searchString, this.currentPage + 1, this.pageSize).subscribe((res: any) => {
  this.associateInfoList = res;
  this.associateInfoList.forEach((r: any) => {
      if (r.JoinDate != null)
        r.JoinDate = moment(r.JoinDate).format('YYYY-MM-DD');
      else r.JoinDate = "";
    });
    this.dataSource.data = this.associateInfoList;
    this.dataSource.sort = this.sort;
    this.spinner.hide();
    this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
      if (typeof data[sortHeaderId] === 'string') {
        return data[sortHeaderId].toLocaleLowerCase();
      }
    
      return data[sortHeaderId];
    };
  });
  }

  
    editAssociatejoining(selectedData: any) {
      let currentID = selectedData.EmpId;
      selectedData.associateType = selectedData.EmpId != 0 ? "edit" : "new";
      this._router.navigate(['/associates/prospectivetoassociate/' + selectedData.associateType + '/' + currentID + '/' + this.subType]);
    }

    sendMail(currentAssociate:Associate){
      let dialogRef = this.dialog.open(SendEmailComponent, {
        width: '60vw', height: '30vw',
        disableClose: true,
        data: { element: currentAssociate }
      }); 
      dialogRef.afterClosed().subscribe(result => {
      
      });
    }

    syncProspectiveAssociateData(){
      this._associateinfoService.syncProspectiveAssociateDataToHRMS().subscribe((data:any) => {
        this._snackBar.open(
          data.Message,
          'x',
          {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
        this.getAssociateInformationList();
      },(error)=>{
        this.spinner.hide();
        this._snackBar.open(
          'Some Error Occured While Processing',
          'x',
          {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
      });

    }
    pageChanged(event: PageEvent) {
      this.pageSize = event.pageSize;
      this.currentPage = event.pageIndex;
      this.getAssociateInformationList();
    }
    
}