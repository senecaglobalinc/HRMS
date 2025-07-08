import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Associate } from '../../models/associate.model';
import * as servicePath from '../../../../core/service-paths';
import { AssociateInformationService } from '../../services/associateInformation.service';
import * as moment from 'moment';
import { MatPaginator } from '@angular/material/paginator';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { SendEmailComponent } from '../send-email/send-email.component';

@Component({
  selector: 'app-welcome-associate-email',
  templateUrl: './welcome-associate-email.component.html',
  styleUrls: ['./welcome-associate-email.component.scss']
})
export class WelcomeAssociateEmailComponent implements OnInit {

  associateInfo: any;
  private subType: string = "list";
  associateInfoList : Associate[];
  selectedRow : Associate;
  private resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = [
    'EmpCode',
    'EmpName',
    'JoiningDate',
    'SendMail'
  ];
  dataSource: MatTableDataSource<Associate>;
  roleName: String;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  constructor(private _associateinfoService : AssociateInformationService,private _router: Router,private _snackBar: MatSnackBar, public navService: NavService,   private spinner: NgxSpinnerService,
    public dialog: MatDialog,) { 
    this.associateInfo = [];
    this._associateinfoService.WelcomeEmail().subscribe((data) => {
      this.associateInfo = data;
      this.getAssociateInformationList();

    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
 
  
  ngOnInit(): void {
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.getAssociateInformationList();
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
    getAssociateInformationList(){
      this.spinner.show();
      this._associateinfoService.WelcomeEmail().subscribe((res: any) => {
      this.associateInfoList = res;
      this.associateInfoList.forEach((r: any) => {
          if (r.JoiningDate != null){
            r.JoiningDate = moment(r.JoiningDate).format('YYYY-MM-DD');
            console.log(r.JoiningDate)
          }
          else r.JoiningDate = "";
        });
        this.dataSource = new MatTableDataSource(res);
        this.dataSource.paginator = this.paginator;
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
        width: '70vw', height: '50vw',
        disableClose: true,
        data: { element: currentAssociate }
      }); 
      dialogRef.afterClosed().subscribe(result => {
        this.getAssociateInformationList();
      });
    }
    

}
