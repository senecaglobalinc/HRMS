import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResignastionService } from 'src/app/modules/AssociateExit/Services/resignastion.service';

@Component({
  selector: 'app-view-detailed-status',
  templateUrl: './view-detailed-status.component.html',
  styleUrls: ['./view-detailed-status.component.scss']
})
export class ViewDetailedStatusComponent implements OnInit {

  EmpId: number;
  statusDetails: any;
  rolesAndDepartments: any;
  constructor(private resignation:ResignastionService,
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
    this.resignation.GetResignationSubStatus(this.data.value).subscribe(res => {
      this.statusDetails =res
      if(this.statusDetails['AssociateExitStatusCode'] === 'ResignationInProgress'){
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
