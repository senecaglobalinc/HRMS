import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
//import { FullCalendarModule } from "@fullcalendar/angular";
import { CalendarOptions, EventInput } from '@fullcalendar/core'; 
import { BehaviorSubject, Observable } from 'rxjs';
import {
  AttendanceReport,
  AttendanceReportFilter,
  AttendanceDetailReport, 
  SelectItem,
  AttendanceDetailReportData
} from '../../models/associate-attendance-report.model';
// import Tooltip from 'tooltip.js';
import { FullCalendarComponent } from '@fullcalendar/angular';


@Component({
  selector: 'app-attendance-report-dialog',
  templateUrl: './attendance-report-dialog.component.html',
  styleUrls: ['./attendance-report-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AttendanceReportDialogComponent implements OnInit {
employeeName:string;
daysWorked:number;
calendarOptions: CalendarOptions = {
  initialView: 'dayGridMonth', 
  events: [
      ],
};

events:any;
@ViewChild('calendar') calendarComponent: FullCalendarComponent; // the #calendar in the template

public eventData = new BehaviorSubject([]);

constructor(
  private dialogRef: MatDialogRef<AttendanceReportDialogComponent>
 ,  @Inject(MAT_DIALOG_DATA) public data: AttendanceDetailReportData
  ) {      
    this.eventData.next(data.eventData)
    this.getEvents().subscribe(events => { this.events = events.map((event) => {
      // event.allDay = true
      if(event.title === 'Holiday'){
        event['backgroundColor'] = 'Orange'
      }
      else if(event.title === 'Weekend'){
        event['backgroundColor'] = 'gray'
      }
      else if(event.title ==='Present'){
        event['backgroundColor'] = 'green'
      }
      else {
        let title = event.title.split(': ')
        // if((title[0] === 'Checkout Time' && title[1] === 'null') || (title[0] === 'Total Hours' && title[1] === '0') || (title[0] === 'Checkin Time' && title[1] === 'null')){
        //   event['backgroundColor'] = 'red';
        //   event['borderColor']='white';
        //   // event['description']= 'You Missed the punch!!';  
        // }
        if(!((title[0] === 'Checkout Time') || (title[0] === 'Total Hours') || (title[0] === 'Checkin Time'))){
          event['backgroundColor'] = 'red';
        }
      }
      return event;
    })});
    this.employeeName =  data.filter.EmployeeName;  
    this.daysWorked = data.daysWorked; 

    this.calendarOptions = {
      initialView: 'dayGridMonth', 
      initialDate: data.filter.FromDate,
      // height: 750,
      events : data.eventData,
      eventBorderColor: 'white'
      // eventDidMount: this.eventDidMount.bind(this)
     };      
  }

  ngOnInit(): void {
   
  }

  onClose(): void {
     this.dialogRef.close();
    }

    getEvents(): Observable<any[]> {
      return this.eventData.asObservable();
     }

    // eventDidMount(info) {
    //   console.log(info)
    //   if(Object.keys(info.event.extendedProps).length > 0){
    //     const tooltip = tippy(info.el, {
    //       content: info.event.extendedProps.description, // You can customize this with other event properties
    //       arrow: true
    //     });
    //   }
      
    // }
  }
    
    

