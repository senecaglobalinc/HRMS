import { Component, OnInit , ViewChild} from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroupDirective} from '@angular/forms';
import * as moment from 'moment';
import { NavService } from '../../../master-layout/services/nav.service';
import { ChecklistService } from '../../../AssociateExit/Services/checklist.service';
import { AssociateExit } from '../../../AssociateExit/Models/associateExit.model';
import { ActivityList } from '../../../AssociateExit/Models/activitymodel';
import * as servicePath from '../../../../core/service-paths';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ktplan-dashboard',
  templateUrl: './ktplan-dashboard.component.html',
  styleUrls: ['./ktplan-dashboard.component.scss']
})
export class KTPlanDashboardComponent implements OnInit {
  associateList : AssociateExit[];  
  activitytype: ActivityList;
  EmployeeId: number;
  roleName: string;
  dashboard: string = "SMDashboard";
  resources = servicePath.API.PagingConfigValue;
  dataSource = new MatTableDataSource();
  pageload: boolean = false;
  isLoading: boolean = false;
  serviceDeptId : number = 0;
 
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
 
  displayedColumns: string[] = [
    'employeeId', 'employeeName', 'designation',
     'exitDate', 'status', 'Action'
  ];

  constructor(
    public navService: NavService,
    private servicemanagerservice: ChecklistService,
    private associateexitservice: AssociateExitDashboardService,
    private route: Router,
    private spinner: NgxSpinnerService,
    private snackBar: MatSnackBar,
){
      this.navService.currentSearchBoxData.subscribe((responseData) => {
      });
     }

   ngOnInit() {
     this.spinner.show();
     
     this.getExitDashboard();
  }

  getExitDashboard() {
    this.isLoading = true;

    this.roleName = JSON.parse(
        sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmployeeId = JSON.parse(
        sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;

      
    switch (this.roleName) {
      
      case ("Quality and Information Security Manager"): {
        this.serviceDeptId = 5;
        break;
      }
      case ("Admin Manager"): {
        this.serviceDeptId = 2;
        break;
      }
      case ("IT Manager"): {
        this.serviceDeptId = 3;
        break;
      }
      case ("Finance Manager"): {
        this.serviceDeptId = 4;
        break;
      }

      case ("HRM"): {
        this.serviceDeptId = 6;
        break;
      }
    
      case ("Training Department Head"): {
        this.serviceDeptId = 7;
        break;
      }

    }

    this.associateexitservice
        .getAssociateExitDashbaord(this.roleName, this.EmployeeId, this.dashboard)//'Service Manager'
        .toPromise().then( (res: any[]) => {
          this.isLoading = false;
        
            this.ModifyDateFormat(res);
            res.forEach( item =>{ 
           
              
            });
            return res;
          }).then(res=>{
                this.dataSource = new MatTableDataSource(res);
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;
                this.spinner.hide();
                this.pageload = true;
            
            
        }).catch(error => {
            this.spinner.hide();
            this.pageload = true;
        });
}
  ModifyDateFormat(data: AssociateExit[]): void {
    data.forEach(e => {
      if (e.ExitDate != null) {
        e.ExitDate = moment(e.ExitDate).format("MM/DD/YYYY");
      }
   
    });
    this.associateList = data;
    this.pageload = true;
    this.spinner.hide();

  }
}
