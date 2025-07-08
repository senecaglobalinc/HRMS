import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AssociateAttendanceReportService } from 'src/app/modules/reports/services/associate-attendance-report.service';
import { environment } from 'src/environments/environment';
import { RegularizationService } from '../../services/regularization.service';
import * as XLSX from 'xlsx'; 
import { MatTableDataSource } from '@angular/material/table';
import { DropDownType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-attendance-muster-report',
  templateUrl: './attendance-muster-report.component.html',
  styleUrls: ['./attendance-muster-report.component.scss'],
 
  })
export class AttendanceMusterReportComponent implements OnInit {

  constructor(private associateAttendanceReportService: AssociateAttendanceReportService,private service : RegularizationService,private datePipe: DatePipe) { }
yearsList=[];
minYear:number;
montsList:Array<DropDownType>=[];
dataSource: MatTableDataSource<any>;
Year:FormControl;
Month:FormControl;
today=new Date();
sixMonthsAgo:Date;
currentYear=new Date().getFullYear();
selectedMonth=new Date().getMonth()+1;
selectedYear=this.currentYear;
formLists()
{
    // year list
    this.minYear=environment.minYearOfAttendance;
       
    for(let year=this.minYear;year<=this.currentYear;year++)
    {
      this.yearsList.push(year);
    }
    //month list
     let totalMonths = 12;      
     var endDate =new Date(); 
    endDate.setMonth(endDate.getMonth() - totalMonths)

   for(var date=this.today;date > endDate;date.setMonth(date.getMonth() - 1)) 
   {  
      var month=this.datePipe.transform(date, 'MMMM');
      this.montsList.push({label:month ,value:date.getMonth()+1});     
    } 
}
  ngOnInit(): void {
  this.formLists();
  this.createForm();
 
  }
  // FormControls declarations
  createForm()
  {
this.Year = new FormControl(this.selectedYear,[Validators.required]);
this.Month = new FormControl(this.selectedMonth,[Validators.required]);
  }
  onSubmit()
  {
   
   this.associateAttendanceReportService.getMusterReport(this.Year.value, this.Month.value).subscribe((res:any)=>{
    this.dataSource=new MatTableDataSource(res);
    const workSheet = XLSX.utils.json_to_sheet(this.dataSource.data);
    const workBook: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workBook, workSheet, 'Attender Muster sheet');    
    let month=this.montsList.filter(x=>x.value==this.Month.value)[0].label;
    let fileName='AttendanceMusterReport_'+month+'_'+this.Year.value +'.xlsx';
    XLSX.writeFile(workBook, fileName);
    XLSX.utils.book_append_sheet(workBook, workSheet, 'Attender Muster sheet');
    XLSX.writeFile(workBook, 'AttenderMusterReport.xlsx');
   }
   )
  
  }
}



