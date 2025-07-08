import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
//import { FullCalendarModule } from "@fullcalendar/angular";
import { CalendarOptions, EventInput } from '@fullcalendar/core'; 
import { BehaviorSubject, Observable } from 'rxjs';
import {
  AttendanceDetailReportData
} from '../../../reports/models/associate-attendance-report.model';
// import Tooltip from 'tooltip.js';
import { FullCalendarComponent } from '@fullcalendar/angular';

@Component({
  selector: 'app-biometric-attendance-dialog',
  templateUrl: './biometric-attendance-dialog.component.html',
  styleUrls: ['./biometric-attendance-dialog.component.scss']
})
export class BiometricAttendanceDialogComponent implements OnInit {
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
  private dialogRef: MatDialogRef<BiometricAttendanceDialogComponent>
 ,  @Inject(MAT_DIALOG_DATA) public data: AttendanceDetailReportData
  ) {  
    // this.data.eventData = [
    // {title: 'Checkin Time: 09:45', date: '2023-06-01',allDay:true},
    // {title: 'Checkout Time: null', date: '2023-06-01',allDay:true},
    // {title: 'Total Hours: 0', date: '2023-06-01',allDay:true}]
    this.eventData.next(data.eventData)
    this.getEvents().subscribe(events => { this.events = events.map((event) => {
      if(event.title === 'Holiday'){
        event['backgroundColor'] = 'Orange'
      }
      else if(event.title === 'Weekend'){
        event['backgroundColor'] = 'gray'
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
    this.employeeName =  data.filter.EmployeeName;  
    this.daysWorked = data.daysWorked; 

    this.calendarOptions = {
      initialView: 'dayGridMonth', 
      initialDate: data.filter.FromDate,
      // height: 850,
      events : data.eventData,
      eventOrder: 'start',
      eventBorderColor: 'white',
      // eventRender: (info) => {
      //   // Check your condition here to determine the background color
      //   if (info.event.extendedProps.backgroundColor) {
      //     info.el.style.backgroundColor = info.event.extendedProps.backgroundColor;
      //   }
      // }
      // // eventDisplay: this.renderEvents.bind(this),
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
    //   // if(Object.keys(info.event.extendedProps).length > 0){
    //   //   const tooltip = tippy(info.el, {
    //   //     content: info.event.extendedProps.description, // You can customize this with other event properties
    //   //     arrow: true
    //   //   });
    //   // }
      
    // }
   

}
