import { Component, OnInit, Inject } from '@angular/core';
import { AbscondService } from '../../Services/abscond.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-view-abscond-detailed-status',
  templateUrl: './view-abscond-detailed-status.component.html',
  styleUrls: ['./view-abscond-detailed-status.component.scss']
})
export class ViewAbscondDetailedStatusComponent implements OnInit {

  EmpId: number;
  statusDetails: any;
  rolesAndDepartments: any;
  constructor(private abscondService:AbscondService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.EmpId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeId;
    this.rolesAndDepartments = JSON.parse(
      sessionStorage.getItem('RolesAndDepartments')
    )
    this.getStatusDetails();
  }
   
  getStatusDetails(){
    this.abscondService.GetAbscondSubStatus(this.data.value).subscribe(res => {
      this.statusDetails =res
      if(this.statusDetails['AssociateExitStatusCode'] === 'AbscondConfirmed'){
        if(this.statusDetails['ActivitiesSubStatus']){
          this.statusDetails['ActivitiesSubStatus'].forEach((dept,i) => {
            this.rolesAndDepartments.forEach(e => {
              if(e.DepartmentId === dept.DepartmentId)
              this.statusDetails['ActivitiesSubStatus'][i]['DepartmentName'] = e.DepartmentCode
            })
          })
        }
      }
    })
  }

  getColor(status){
    if(status === 'DepartmentActivityInProgress'){
      return 'orange'
    }
    else if(status === 'DepartmentActivityCompleted'){
      return 'green'
    }
    else{
      return 'white'
    }
  }
}
