import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { themeconfig } from 'src/themeconfig';
import {wfhAttendance} from 'src/app/modules/onboarding/models/wfhAttendance.model';
import { WfhAttendanceService } from 'src/app/modules/onboarding/services/wfh-attendance.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BiometricAttendanceDialogComponent } from '../biometric-attendance-dialog/biometric-attendance-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { BiometricAttendanceFoundDialogComponent } from '../biometric-attendance-found-dialog/biometric-attendance-found-dialog.component';
import { Router } from '@angular/router';
import { AssignReportingManagerService } from 'src/app/modules/project-life-cycle/services/assign-reporting-manager.service';


enum AttendanceSigningCode {
        Initial = 0,
        SignedIn = 1,
        SignedOut = 2,
        SignIn = 'Sign In',
        SignOut = 'Sign Out'
}


@Component({
  selector: 'app-wfh-attendance',
  templateUrl: './wfh-attendance.component.html',
  styleUrls: ['./wfh-attendance.component.scss']
})


export class WfhAttendanceComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  locations = ['Work From Office', 'Work From Home'];
  location = new FormControl({value:"Work From Home",disabled:true}, [Validators.required]);
  remarks = new FormControl(null);
  saveSignIn : wfhAttendance = new wfhAttendance();
  dataSource: MatTableDataSource<wfhAttendance> = new MatTableDataSource();
  btnLabel:string;
  diasbaleButton:boolean = false;
  empCode : string;
  allowedWfoInHrms:boolean = false;
 empName :string;
 empId:number;
 projectDetails:any;
 isDisabled=true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  selectedColumns = ['Employee_Code','punch-In-time','punch-out-time','Date','Summary'];
  punchesInfo=[];
  punchesData = []
  

  constructor(private _service : WfhAttendanceService,private _snackBar: MatSnackBar,public dialog: MatDialog,private router : Router,private service: AssignReportingManagerService) { }

  ngOnInit(): void {
   
    this.empCode = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeCode;
    this.empName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).fullName;
    this.empId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.allowedWfoInHrms = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).allowedWfoInHrms;
    this.getLoginStatus();    
    this.GetAssociateProject();    
  }
  GetAssociateProject()
  {
    this.service.GetProjects(this.empId).subscribe((res: any) => {
      this.projectDetails=res;
      let project=  this.projectDetails.filter(x=>x.ProjectName.toLowerCase()=="THF IT Helpdesk".toLowerCase());
      if(project.length!=0 || this.allowedWfoInHrms)
      {
        if(!this.isDisabled)
        {
           this.location.enable();
        }
        else{
             this.location.disable();
        }
        this.location.setValue("Work From Office");
      }
    });
  }

  getLoginStatus(){
    this._service.GetloginStatus(this.empCode).subscribe((res:any)=>{
      this.diasbaleButton =false
      if((res.Item == AttendanceSigningCode.Initial)){
        this.btnLabel = AttendanceSigningCode.SignIn;
        this.isDisabled=false;
      }
      else if(res.Item == AttendanceSigningCode.SignedIn){
        this.isDisabled=true;
        this.btnLabel = AttendanceSigningCode.SignOut;         
        this.getAttendanceDetails();
      }

      else if(res.Item == AttendanceSigningCode.SignedOut){
        this.btnLabel = AttendanceSigningCode.SignIn;
        
        this.getAttendanceDetails();
      }
      else{
        let dialogRef = this.dialog.open(BiometricAttendanceFoundDialogComponent, {
          // height: '400px',
          // width: '600px',
        });
        dialogRef.afterClosed().subscribe(() => {
          this.router.navigate(['/associates/biometricattendance'])
          
        });
      }

    },(error)=>{
      this.btnLabel = AttendanceSigningCode.SignIn
      this.diasbaleButton = true
      this._snackBar.open('Error Ocurred, Please try again!', 'x', {
        duration: 1000, 
        panelClass: ['error-alert'], 
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    })
  }
  onButtonClick(event){
    this.saveSignIn = new wfhAttendance()
    if(event.target.innerText == AttendanceSigningCode.SignIn){
      this.saveSignIn.SignedStatus = AttendanceSigningCode.SignedIn;
    }
    else if(event.target.innerText == AttendanceSigningCode.SignOut){
      this.saveSignIn.SignedStatus = AttendanceSigningCode.SignedOut;
    }

    this.saveAttendanceDetails();
  }

  getAttendanceDetails(){
    this._service.GetAttendanceDetails(this.empCode).subscribe((res:any)=>{
      let attendanceData =[];
      this.punchesInfo =[];
      this.punchesData = [];
      this.remarks.setValue(res.Item.Remarks)
      if(!(res.Item.SUMMARY == 'Punches Not In Pair')){
        res.Item.SUMMARY = ' - '
      }
      attendanceData.push(res.Item);  
      this.punchesInfo = res.Item.PunchInfoLog.split(',');
      // this.punchesInfo = ["Signed In : 12:09$Location : WFH","Signed_Out : 14:49$Location : WFH"]
      this.punchesInfo.forEach((element)=>{
        let val = element.split("$");
        let SignedStatus = val[0];
        let location = val[1].split(":");
        this.punchesData.push({label: SignedStatus,value:location[1]})
      })
      this.dataSource.data = attendanceData;

    },(error)=>{

    })
  }

  saveAttendanceDetails(){
    //doint the same for sign in and signout
    // if(this.location.value == null){
    //   this._snackBar.open('Please select location!', 'x', {
    //     duration: 1000,
    //     horizontalPosition: 'right',
    //     verticalPosition: 'top'
    //   });
    //   return
    // }
    this.saveSignIn.asscociateId = this.empCode;
    this.saveSignIn.asscociateName = this.empName;
    this.saveSignIn.remarks = this.remarks.value;
    if(this.location.value == 'Work From Home'){
      this.saveSignIn.location = 'WFH';
    }
    else if(this.location.value == 'Client Location'){
      this.saveSignIn.location = 'CL';
    }
    else if(this.location.value=='Work From Office')
    {
      this.saveSignIn.location = 'WFO';
    }
    else{
      this.saveSignIn.location = this.location.value
    }
   
    this._service.SaveAttendanceDetais(this.saveSignIn).subscribe((res:any)=>{
      this.getLoginStatus();
      this.clearFormData();
      if(!res.IsSuccessful){
        this._snackBar.open('Failed' + this.btnLabel + '!', 'x', {
          duration: 1000, 
          panelClass: ['error-alert'], 
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
      }
      else{
        if(this.btnLabel == AttendanceSigningCode.SignIn){
          this.location.disable();
          this._snackBar.open('Signed-in Successfully!', 'x', {
            duration: 1000, 
            panelClass: ['success-alert'], 
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });
        }
        else if(this.btnLabel == AttendanceSigningCode.SignOut && res.IsSuccessful){
          this._snackBar.open('Signed-out Successfully!', 'x', {
            duration: 1000, 
            panelClass: ['success-alert'], 
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });
        }
      }
    },(error)=>{
      this.clearFormData();
      this._snackBar.open('Failed' + AttendanceSigningCode.SignOut +'!', 'x', {
        duration: 1000, 
        panelClass: ['error-alert'], 
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });

    })
  }
  clearFormData(){
    // this.location.reset();
    this.remarks.reset();
  }

}
