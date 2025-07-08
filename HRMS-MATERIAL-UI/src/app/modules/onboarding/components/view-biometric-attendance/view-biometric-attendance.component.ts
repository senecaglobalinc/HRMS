import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { Subject } from 'rxjs/internal/Subject';
import { Observable } from 'rxjs/Observable';
import { AssociateAttendanceReportService } from 'src/app/modules/reports/services/associate-attendance-report.service';

@Component({
  selector: 'app-view-biometric-attendance',
  templateUrl: './view-biometric-attendance.component.html',
  styleUrls: ['./view-biometric-attendance.component.scss']
})
export class ViewBiometricAttendanceComponent implements OnInit {
  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth', 
    events: [
        ],
  };
  startDate:Date;
  endDate:Date;
  eventsData:any;
  employeeName:string;
  daysWorked:number=0;
  roleName:string;
  public eventData = new BehaviorSubject([]);
  @ViewChild('calender') calendarComponent: FullCalendarComponent;
  private unsubscribe$: Subject<any> = new Subject<any>();


  constructor(private associateAttendanceReport : AssociateAttendanceReportService,private router : Router,private actRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.actRoute.queryParams.subscribe(params => { this.employeeName = params.employeeName });  
    this.roleName= JSON.parse(sessionStorage.AssociatePortal_UserInformation).roleName;
    this.associateAttendanceReport.attendanceDataObservable.pipe(takeUntil(this.unsubscribe$)).subscribe((res:any)=>{
      if(Object.keys(res).length > 0){
        this.startDate = new Date(res.filter.FromDate);
        this.endDate = new Date(res.filter.ToDate);
        this.eventsData = res.eventData;
        this.employeeName =  res.filter.EmployeeName;  
        this.daysWorked = res.daysWorked; 
        this.eventData.next(res.eventData)
        // this.eventData = res.eventData
        this.updateCalender();
      }   
    }) 
  }

  updateCalender(){
    this.getEvents().pipe(takeUntil(this.unsubscribe$)).subscribe(events => {  events.map((event) => {
      if(event.title === 'Holiday'){
        event['backgroundColor'] = 'Orange'
      }
      else if(event.title === 'Weekend'){
        event['backgroundColor'] = 'gray'
      }
      else if(event.title!=null && event.title.toUpperCase().includes("Leave".toUpperCase())){
        event['backgroundColor'] = 'pink'
        event['textColor'] = 'black'
      }
      else if(event.title ==='Present'){
        event['backgroundColor'] = 'green'
      }
      else if(event.title == 'WFO'){
          event['display'] = 'none'
      }

      else if(event.title == 'WFH'){
        event['backgroundColor'] = '#FCB5AC'
        event['textColor'] = 'brown'
        
      }
      else if(event.title == null){
        event['display'] = 'none'
      }
      else if(event.title == "Regularized"){
        event['backgroundColor'] = '#748cab'
        event['color'] = 'edf2f4';
      }
      else if(event.title=='Absent')
      {
        event['backgroundColor']='#66dfe3'
        event['textColor'] = 'red';
      }

      
      else {
        let title = event.title.split(': ')
        // if((title[0] === 'Checkout Time' && title[1] === '') || (title[0] === 'Total Hours' && title[1] === '0') || (title[0] === 'Checkin Time' && title[1] === '')){
        //   event['backgroundColor'] = '#ea3a5a';
        //   event['borderColor']='white';
        //   // event['description']= 'You Missed the punch!!';
        // }
        if(!((title[0] === 'Checkout Time') || (title[0] === 'Total Hours') || (title[0] === 'Checkin Time'))){
          // event['backgroundColor'] = 'red';
          if(title[0]=== 'Punches Not In Pair'){
            event['backgroundColor'] = 'red';
          }
          else{
           event['display'] ='none'
          }
        }
      }
      return event;
    })});
   
    this.calendarOptions = {
      initialView: 'dayGridMonth', 
      initialDate: this.startDate,
      events : this.eventsData,
      eventOrder: 'start',
      eventBorderColor: 'white',
      validRange: {
        start: this.startDate,
        end: this.endDate
      }
     };      
  }

  getEvents(): Observable<any[]> {
    return this.eventData.asObservable();
   }

   ApplyRegularization(){
    this.router.navigate(['/associates/regularization'])
   }

   goBack(){
   
    if(this.roleName=="Associate")
    {
      this.router.navigate([],
        {
          queryParamsHandling: 'merge',
          replaceUrl: true
         }).then(
          () => {
            this.router.navigate(['/associates/biometricattendance'], { queryParams: {'fromDate':this.startDate, 'toDate':this.endDate } });
          }
         );
    
    }
    else{
      this.router.navigate([],
        {
          queryParamsHandling: 'merge',
          replaceUrl: true
         }).then(
          () => {
            this.router.navigate(['/associates/biometricattendancereport'], { queryParams: {'fromDate':this.startDate, 'toDate':this.endDate } });
          }
         );
     // this.router.navigate(['/associates/biometricattendancereport'])
    }
   }

}

