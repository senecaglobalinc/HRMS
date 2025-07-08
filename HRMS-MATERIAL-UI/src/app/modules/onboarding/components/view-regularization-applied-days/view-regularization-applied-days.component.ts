import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { RejectDialogComponent } from 'src/app/modules/shared/components/reject-dialog/reject-dialog.component';
import { AttendanceRegularization } from '../../models/regularization.model';
import { RegularizationService } from '../../services/regularization.service';

@Component({
  selector: 'app-view-regularization-applied-days',
  templateUrl: './view-regularization-applied-days.component.html',
  styleUrls: ['./view-regularization-applied-days.component.scss']
})
export class ViewRegularizationAppliedDaysComponent implements OnInit {

  dataSource = new MatTableDataSource<AttendanceRegularization>();
  displayedColumns: string[] = [
    'select',
    'RegularizationAppliedDate',
    'InTime',
    'OutTime',
    'Location',
    'Remarks'
  ];
  id:string;
  AssociateName:string;
  regularizedDays: AttendanceRegularization[] =[];
  EmpId: number;
  roleName:string;
  @ViewChild(MatPaginator, { static: false }) Paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  
  selection = new SelectionModel<AttendanceRegularization>(true, []);
  approveOrRejectRequestObj : AttendanceRegularization = new AttendanceRegularization()


 
  constructor( private actRoute: ActivatedRoute,
    private service : RegularizationService,
    private _snackBar : MatSnackBar,
    private router : Router,
    private dialog : MatDialog,public navService: NavService) { }

  ngOnInit(): void {
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.actRoute.params.subscribe(params => { this.id = params.id;this.AssociateName=params.associateName });
    this.GetAssociateSubmittedAttendanceRegularizationById();
  }

  ApproveOrReject(buttonName : string){
    this.approveOrRejectRequestObj = new AttendanceRegularization()
    this.approveOrRejectRequestObj.SubmittedBy = this.id;
    this.approveOrRejectRequestObj.ApprovedBy = this.EmpId;
    this.approveOrRejectRequestObj.RegularizationDates=[];
    this.selection.selected.forEach(s => {console.log(s),
      this.approveOrRejectRequestObj.RegularizationDates.push(s.RegularizationAppliedDate)
    });

    if(this.approveOrRejectRequestObj.RegularizationDates.length ===0){
      this._snackBar.open('Please Select the Records', 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return;
    }

    if(buttonName === 'Approve'){
      this.approveOrRejectRequestObj.IsApproved = true;
      this.approveOrRejectRegularizations(buttonName);
    }
    else if(buttonName === 'Reject'){
      const dialogRef = this.dialog.open(RejectDialogComponent,{
        width:'400px',
        disableClose: true,
       
      });
      dialogRef.afterClosed().subscribe(result => {
        // this.closeProject();
        if(result){
        this.approveOrRejectRequestObj.RemarksByRM = result.rejectReason;
        this.approveOrRejectRequestObj.IsApproved = false;
        this.approveOrRejectRequestObj.RejectedBy=JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
        this.approveOrRejectRequestObj.RejectedDate=new Date();
        this.approveOrRejectRegularizations(buttonName);
        }
      });
    }
  }

  GetAssociateSubmittedAttendanceRegularizationById(){
    this.regularizedDays = [];
    this.service.GetAssociateSubmittedAttendanceRegularizationById(this.id,this.roleName).subscribe((res : AttendanceRegularization[])=>{
      this.regularizedDays = res;
      this.dataSource = new MatTableDataSource(this.regularizedDays);
      this.dataSource.paginator = this.Paginator;
      this.dataSource.sort = this.sort;
      if(this.regularizedDays.length ==0){
        this.onBackClick();
      }
    },
    (error)=>{
      this.regularizedDays = [];
      this.dataSource = new MatTableDataSource(this.regularizedDays);
      this.dataSource.paginator = this.Paginator;
      this.dataSource.sort = this.sort;
      if(this.regularizedDays.length ==0){
        this.onBackClick();
      }
    })

  }

  onBackClick(){
    this.router.navigate(['/shared/dashboard'])
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.filteredData.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.filteredData.forEach(row => this.selection.select(row));
  }

  approveOrRejectRegularizations(buttonName:string){
    this.service.ApproveOrRejectAttendanceRegularizationDetails(this.approveOrRejectRequestObj).subscribe((res:any)=>{
      if(res.IsSuccessful){
        if(buttonName === 'Approve'){
          this._snackBar.open(
            'Approved Regularization',
            'x',
            {
              duration: 3000,
              panelClass : ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            }
          );
        }
        else if(buttonName === 'Reject'){
          this._snackBar.open(
            'Rejected Regularization',
            'x',
            {
              duration: 3000,
              panelClass : ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            }
          );
        }
        // this.onBackClick();
        this.GetAssociateSubmittedAttendanceRegularizationById();
        this.navService.changeSearchBoxData('');
        this.selection.clear();
      }
      else{
        this._snackBar.open(
          'Failed Updating Records',
          'x',
          {
            duration: 3000,
            panelClass : ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
        this.GetAssociateSubmittedAttendanceRegularizationById();
        this.navService.changeSearchBoxData('');
        this.selection.clear();
      }
    },
    (error)=>{
      this._snackBar.open(
        'Failed Updating Records',
        'x',
        {
          duration: 3000,
          panelClass : ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      this.navService.changeSearchBoxData('');
      this.GetAssociateSubmittedAttendanceRegularizationById();
      this.selection.clear();
    })
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.dataSource = new MatTableDataSource(this.regularizedDays);
        this.dataSource.paginator = this.Paginator;
        this.dataSource.sort = this.sort;
      }
    } else {
      this.dataSource = new MatTableDataSource(this.regularizedDays);
      this.dataSource.paginator = this.Paginator;
      this.dataSource.sort = this.sort;
    }
  }

}
