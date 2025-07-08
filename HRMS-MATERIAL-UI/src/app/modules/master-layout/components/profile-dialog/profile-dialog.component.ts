import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PersonalService } from "../../../onboarding/services/personal.service";
import { Profile } from "../../../master-layout/models/user.model"
import * as moment from 'moment';
import { ChecklistService } from 'src/app/modules/AssociateExit/Services/checklist.service';

@Component({
  selector: 'app-profile-dialog',
  templateUrl: './profile-dialog.component.html',
  styleUrls: ['./profile-dialog.component.scss']
})
export class ProfileDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private _service : ChecklistService) { }
  
  employeeId:number = JSON.parse(
    sessionStorage.AssociatePortal_UserInformation
  ).employeeId;

  profileDetails = new Profile();

  ngOnInit(): void {
    this.getProfileDetails();
  }

  getProfileDetails(){
    this._service.GetByEmployeeId(this.employeeId).subscribe((res:Profile)=>{
      this.profileDetails.EmployeeName = res.EmployeeName;
      this.profileDetails.Designation = res.Designation;
      this.profileDetails.Grade = res.Grade;
      this.profileDetails.DateOfJoin = moment(res.DateOfJoin).format("DD-MM-YYYY");
      this.profileDetails.BloodGroup = res.BloodGroup
      this.profileDetails.ReportingManager = res.ReportingManager;
      this.profileDetails.ProgramManager = res.ProgramManager;
      this.profileDetails.Lead = res.Lead;
      this.profileDetails.DepartmentId=res.DepartmentId;
    },
    (error)=>{

    })
  }
  
  onClearClick(){
    this.dialogRef.close();
  }
}
