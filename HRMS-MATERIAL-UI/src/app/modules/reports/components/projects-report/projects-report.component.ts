import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {ProjectResponse} from '../../models/projects-report.model';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import {ProjectsService} from '../../services/projects-report.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatOption } from '@angular/material/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ReplaySubject, Subject } from 'rxjs';
import { themeconfig } from '../../../../../themeconfig';
import { takeUntil } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';
import { TableUtil } from "../../services/exportUtil";
@Component({
  selector: 'app-projects-report',
  templateUrl: './projects-report.component.html',
  styleUrls: ['./projects-report.component.scss']
})
export class ProjectsReportComponent implements OnInit {
projectsInfo: MatTableDataSource<ProjectResponse> ;
projectDetails: ProjectResponse[];
pageLoad = false;
actionexport = true;
selectedColumns: any[];
themeAppearence = themeconfig.formfieldappearances;
@ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild('allSelected') private allSelected: MatOption;
  @ViewChild('table') table: MatTable<any>;
  @ViewChild('exporter') exporter;
  private _onDestroy = new Subject<void>();
  cols: any[] = [];
  myForm: FormGroup;
  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filteredcols: any[];
  displaycolsfields: any[];
  columnOptions: any[] = [];
  displaycols = [];
  totalRecordsCount = 0;
  showHideDetails: boolean = false;
  showCols: any;
  constructor(private service: ProjectsService, private spinner: NgxSpinnerService, private _snackBar: MatSnackBar, private fb: FormBuilder, public datepipe: DatePipe) { }

  ngOnInit(): void {

    this.myForm = this.fb.group({
     
      columnselect: [],
      columnfilter: ['']
    });
    this.projectDetails = [];
   
    this.cols = [
      { field: 'ProjectCode', header: 'Project Code' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'ClientName', header: 'Client' },
      { field: 'ManagerName', header: 'Program Manager' },
      { field: 'ActualStartDate', header: 'Start Date' },
      { field: 'ProjectState', header: 'Status' },
      { field: 'ActualEndDate', header: 'End Date' },
      { field: 'ProjectTypeDescription', header: 'Project Type' },
     
      { field: 'PracticeAreaCode', header: 'Technology' },
      
  
    ];
    this.selectedColumns = [
      this.cols[0],
      this.cols[1],
      this.cols[2],
      this.cols[3],
      this.cols[4],
      this.cols[5],
      

    ];
    this.columnOptions = [];
    for (let i = 0; i < this.cols.length; i++) {

      this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });

    }
this.spinner.show();
this.GetAll();


this.filteredcols = this.cols;
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.displaycolsfields = this.selectedColumns.map(col => col.field);
    this.myForm.controls.columnselect.setValue(this.selectedColumns);
    
    this.filteredBanksMulti.next(this.cols.slice());
    this.myForm.controls.columnfilter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });
this.pageLoad = true
  }
  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  ModifyDateFormat(data: ProjectResponse[]): void {
    data.forEach(e => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format('MM/DD/YYYY');
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format('MM/DD/YYYY');
      }  
    })
    this.projectsInfo =new MatTableDataSource(data);
    this.projectsInfo.paginator = this.paginator;
      this.projectsInfo.sort = this.sort;
  }
 
  GetAll()
  {
    this.service.GetAllProjects().toPromise().then((res: ProjectResponse[])=>{
      this.projectDetails = res;
      this.ModifyDateFormat(this.projectDetails);
      this.totalRecordsCount = res.length;
      console.log(this.projectDetails.length);
      
      this.spinner.hide();
    }).catch(error=>{
    this.spinner.hide();
    this._snackBar.open('Error Occured While Fetching Project Details', 'x', {
    duration: 2000,
    horizontalPosition: 'right',
    verticalPosition: 'top',
    });
    });;
  }
  alterTable(event: any) {
    if (!this.allSelected.selected){
    this.selectedColumns = event.value;
    this.myForm.controls.columnselect.setValue(this.selectedColumns);
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.table.renderRows();
    }
  }

  tosslePerOne(all) {
    if (this.allSelected.selected) {
      this.allSelected.deselect();
      return false;
    }
    if (
      this.selectedColumns.length ==
      this.cols.length
    )
      this.allSelected.select();
  }

  toggleAllSelection() {
    if (this.allSelected.selected) {
      this.selectedColumns= [...this.cols.map(item => item),0];
      this.myForm.controls.columnselect
        .patchValue(this.selectedColumns);
      this.displaycols = this.cols.map(col => col.header);
      this.table.renderRows();
    } else {
      this.myForm.controls.columnselect.patchValue([]);
      this.selectedColumns=[];
      this.displaycols = [];
      this.table.renderRows();
    }
  }
  private filterBanksMulti() {
    if (!this.cols) {
      return;
    }
   
    let search = this.myForm.controls.columnfilter.value;
    if (!search) {
      this.filteredBanksMulti.next(this.cols.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
  
    this.filteredBanksMulti.next(
      this.cols.filter(ele => ele.header.toLowerCase().indexOf(search) > -1)
    );
  }
  filter(event: any) {

    let filterValue = this.myForm.controls.columnfilter.value;
    if (filterValue) {
      return this.filteredcols = this.cols.filter(option => option.header.toLowerCase().includes(filterValue));
    }
    else {
      return this.cols;
    }
  }
  exporterIntoExcel() {
    this.showCols = this.selectedColumns;
    this.selectedColumns = this.cols;
    this.myForm.controls.columnselect.setValue(this.selectedColumns);
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.table.renderRows();
    
    
  }
  export()
  {
  
   this.exporter.exportTable('csv',{fileName: 'Projects Report'})
    this.actionexport = false;
   
  }
}
