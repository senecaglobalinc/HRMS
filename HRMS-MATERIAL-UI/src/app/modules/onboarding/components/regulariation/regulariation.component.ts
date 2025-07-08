import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AssociateAttendanceReportService } from 'src/app/modules/reports/services/associate-attendance-report.service';
import { themeconfig } from 'src/themeconfig';
import { NotPunchInDate, AttendanceRegularization } from '../../models/regularization.model';
import { RegularizationService } from '../../services/regularization.service'

@Component({
  selector: 'app-regulariation',
  templateUrl: './regulariation.component.html',
  styleUrls: ['./regulariation.component.scss']
})
export class RegulariationComponent implements OnInit {
  Locations = ['Work From Home'];
  regularizationForm : FormGroup;
  dateWiseRegularization : FormArray;
  toBeRegularizedDays = [];
  themeConfigInput = themeconfig.formfieldappearances;
  exportFromTime = { hour: 9, minute: 1, meriden: 'PM', format: 24 };
  exportToTime = { hour: 18, minute: 1, meriden: 'PM', format: 24 };
  reportingManagers = [{label : '', value : null}]
  notPunchInReqObj : NotPunchInDate =  new NotPunchInDate();
  applyRegularization : AttendanceRegularization = new AttendanceRegularization();
  employeeCode : string;
  startDate:string;
  endDate:string;
  private unsubscribe$: Subject<any> = new Subject<any>();

  constructor(private formBuilder : FormBuilder, 
    private service : RegularizationService,
    private _snackBar : MatSnackBar,
    private associateAttendanceReport : AssociateAttendanceReportService,
    private router : Router) { }

  ngOnInit(): void {
    this.employeeCode = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeCode;
    this.createForm();
    // this.getAbsentDays();
    this.getDatesRange();
  }

  getDatesRange(){
    this.associateAttendanceReport.attendanceDataObservable.pipe(takeUntil(this.unsubscribe$)).subscribe((res:any)=>{
      if(Object.keys(res).length > 0){
        this.startDate = res.filter.FromDate;
        this.endDate = res.filter.ToDate;
        this.getAbsentDays();
      }   
    }) 
  }

  createForm(){
    this.regularizationForm  = new FormGroup({
      reportingManager : new FormControl(1,[Validators.required]),
      remarksByReportingManager : new FormControl(),
      dateWiseRegularization : new FormArray([]),
    })
   
  }
  
 
  applyDateWiseRegularization(): FormGroup {
      return this.formBuilder.group({
       Location: new FormControl({value: 'Work From Home', disabled:true},[Validators.required]),
       date : new FormControl(),
       fromTime:new FormControl("09:01",[Validators.required]),
       toTime : new FormControl("18:01",Validators.required),
       Remarks : new FormControl()
      });
  }

  onDelete(index:number){
    const remove = this.regularizationForm.get('dateWiseRegularization') as FormArray;
        remove.removeAt(index);
        console.log(this.regularizationForm,index,remove.length)
        if(index == 0 && remove.length == 0){
          this.toBeRegularizedDays = [];
        }

  }

  getAbsentDays(){
    this.toBeRegularizedDays = [];
    this.notPunchInReqObj  =  new NotPunchInDate();
    this.notPunchInReqObj.associateId = this.employeeCode;
    // this.notPunchInReqObj.fromDate = "2023-09-6T07:13:06.671Z";
    // this.notPunchInReqObj.toDate = "2023-09-26T07:13:06.671Z";
    this.notPunchInReqObj.fromDate = this.startDate;
    this.notPunchInReqObj.toDate = this.endDate;
    this.service.GetNotPunchInDate(this.notPunchInReqObj).subscribe((res:any)=>{
       this.reportingManagers[0].label =res.ReportingManager;
       this.reportingManagers[0].value = res.ReportingManagerId
      this.toBeRegularizedDays =   res.AttendanceRegularizationDates;
     this.buildForm();
    })
    // let res = { 
    //   "ReportingManagerId": 172, 
    //   "reportingManager": "Kalyan Penumutchu", 
    //   "AttendanceRegularizationDates": 
    //      [ 
    //     "2023-09-01",  
    //     "2023-09-02", 
    //     "2023-09-03",
    //     "2023-09-04",  
    //     "2023-09-05",
    //     "2023-09-06", 
    //     "2023-09-07",  
    //     "2023-09-08",   
    //     "2023-09-09",  
    //     "2023-09-10", 
    //     "2023-09-11" 
    //   ]
    //  }
    //  this.toBeRegularizedDays =   res.AttendanceRegularizationDates;
    //  this.buildForm();
    
  }

  buildForm(){
    this.regularizationForm  = new FormGroup({
      reportingManager : new FormControl(this.reportingManagers[0].value,[Validators.required]),
      remarksByReportingManager : new FormControl(),
      dateWiseRegularization : new FormArray([]),
    })

    for(let i=0;i<this.toBeRegularizedDays.length;i++){

      this.dateWiseRegularization = this.regularizationForm.get('dateWiseRegularization') as FormArray;
      this.dateWiseRegularization.push(this.applyDateWiseRegularization());
      
      this.regularizationForm.controls['dateWiseRegularization']['controls'][i].get('date').setValue(this.toBeRegularizedDays[i]);
      // this.regularizationForm.controls['dateWiseRegularization']['controls'][i].setValue(null);
    }
  }

  ApplyRegularization(){
    
    if(this.regularizationForm.invalid){
      this._snackBar.open(
        'Please fill the feilds highlighted in red color!',
        'x',
        {
          duration: 3000,
          panelClass : ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }
    let saveAttendanceRegularization = [];
  
    this.regularizationForm.value.dateWiseRegularization.forEach(element => {
      let reqObj = new AttendanceRegularization();
      reqObj.RegularizationAppliedDate = element.date;  
      var location=this.dateWiseRegularization.controls[0]["controls"].Location.value;
      if(location == 'Work From Home'){
        reqObj.Location = 'WFH'
      }     
      reqObj.InTime = element.fromTime;
      reqObj.OutTime = element.toTime;
      reqObj.Remarks = element.Remarks;
      reqObj.RemarksByRM = this.regularizationForm.value.remarksByReportingManager;
      reqObj.SubmittedTo = this.regularizationForm.value.reportingManager;
      reqObj.SubmittedBy = this.employeeCode
      saveAttendanceRegularization.push(reqObj);
    });
    this.service.saveAttendanceRegularizationDetails(saveAttendanceRegularization).subscribe((res)=>{
      this.getAbsentDays();
      this._snackBar.open(
        'Sucessfully Applied Regularization',
        'x',
        {
          duration: 3000,
          panelClass : ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    },
    (error)=>{
      this.getAbsentDays();
      this._snackBar.open(
        'Failed Applying Regularization',
        'x',
        {
          duration: 3000,
          panelClass : ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    })
  }

  goBack(){
    this.router.navigate(['/associates/viewAttendance'])
   }

}
