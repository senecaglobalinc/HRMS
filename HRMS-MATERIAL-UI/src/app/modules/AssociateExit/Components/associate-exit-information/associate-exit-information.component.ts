import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { AssociateExit } from '../../Models/associateExit.model';
//import { Associate } from './../../../onboarding/models/associate.model';
import { Component, OnInit , ViewChild} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import {MatDialog} from '@angular/material/dialog';
import { FormGroupDirective} from '@angular/forms';
import {AssociateProjectsComponent} from '../associate-projects/associate-projects.component'
import { MatSnackBar } from '@angular/material/snack-bar';
import * as moment from 'moment';
import { themeconfig } from '../../../../../themeconfig';

  @Component({
    selector: 'app-associate-exit-information',
    templateUrl: './associate-exit-information.component.html',
    styleUrls: ['./associate-exit-information.component.scss']
  })

  export class AssociateExitInformationComponent implements OnInit {

   EmployeeId: number;
   roleName: string;
   ExitReasonId: number;

  dataSourceAssociateExit: MatTableDataSource<any>;
  @ViewChild(MatSort, { static: true }) sortAssociateExit: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
      'EmpCode',
      'EmpName',
      'ExitDate',
      'ExFeedInt',
      'ExClr',
      'Kt'
    ];
    

  constructor(private _associateExitDashboardService: ExitAnalysisService,public navService: NavService,private _router: Router,public dialog: MatDialog,private _snackBar: MatSnackBar) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      //this.applyFilter(responseData);
    });
   }
  ngOnInit():void {this.roleName = JSON.parse(
    sessionStorage["AssociatePortal_UserInformation"]
  ).roleName;
  this.EmployeeId = JSON.parse(
    sessionStorage["AssociatePortal_UserInformation"]
  ).employeeId;
  //this.ExitReasonId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).exitReasonId;
  
  this.GetAssociateExitList();
}
/*applyFilter(event: Event) {
  if (event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceAssociateExit.filter = filterValue.trim().toLowerCase();
  } else {
    this.dataSourceAssociateExit = new MatTableDataSource(this.dataSourceAssociateExit.data);
  }
  if (this.dataSourceAssociateExit.paginator) {
    this.dataSourceAssociateExit.paginator.firstPage();
  }
  this.dataSourceAssociateExit.paginator = this.paginator;
  this.dataSourceAssociateExit.sort = this.sort;
}*/
GetAssociateExitList():void{
  this._associateExitDashboardService.GetAssociateExitAnalysis().subscribe((res: any[]) => {
      res.forEach(e => { e.ExitDate = moment(e.ExitDate).format("MM/DD/YYYY"); });
      this.dataSourceAssociateExit = new MatTableDataSource(res);
      this.dataSourceAssociateExit.sort = this.sort;
  });
  
}


    onFeedback(selectedData: any) {
      this._router.navigate(['/associateexit/exitfeedback/'+ selectedData.EmployeeId]);
  }
  onClearence(selectedData: any) {
    this._router.navigate(['/associateexit/deptchecklist/' + selectedData.EmployeeId]);
  }
  // onKnowledge(selectedData: any) {
  //   this._router.navigate(['/shared/KtForm/Associate/'+selectedData.EmployeeId+'/'+ 88]);
  // }
  openDialog(selectedData) {

    this._associateExitDashboardService.GetAssociateTransitionPlan(selectedData.EmployeeId).subscribe((data:any[])=>{
      if(!data.length){
        this._snackBar.open("Data not found", 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }).afterDismissed().subscribe(res => {
          // this.onBack();
        });
      }
      else if(data.length > 1){
          let projects=[];
          data.forEach((data)=>{
            projects.push({'projectId':data.ProjectId,'projectName':data.ProjectName,'EmpCode':selectedData.EmployeeId})
          })
          this._associateExitDashboardService.SetAssociateProjects(projects)
          const dialogRef = this.dialog.open(AssociateProjectsComponent);
          dialogRef.afterClosed().subscribe(result => {
            console.log(`Dialog result: ${result}`);
          });
      }else{
        this._router.navigate(['/shared/KtForm/Associate/'+selectedData.EmployeeId+'/'+data[0].ProjectId]);
      }
      
      
    })
    
  }
  goto(){
    this._router.navigate(['/associateexit/associateexitbyhr']);
  }

}


