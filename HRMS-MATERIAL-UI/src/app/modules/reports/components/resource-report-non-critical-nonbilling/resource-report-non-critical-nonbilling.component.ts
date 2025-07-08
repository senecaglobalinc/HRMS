import { Component, OnInit, ViewChild } from '@angular/core';
import { ReportsData } from '../../models/reportsdata.model';
import * as servicePath from '../../../../core/service-paths';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ResourceReportNoncriticalService} from '../../services/resource-report-noncritical.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatOption } from '@angular/material/core';
import { FormControl, FormGroup } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as moment from 'moment';

@Component({
  selector: 'app-resource-report-non-critical-nonbilling',
  templateUrl: './resource-report-non-critical-nonbilling.component.html',
  styleUrls: ['./resource-report-non-critical-nonbilling.component.scss']
})
export class ResourceReportNonCriticalNonbillingComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;

  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  cols: any[] = [];
  selectedColumns: any[];
  displaycols: any[];
  public lstNonCriticalResourceReportData: ReportsData[] = []
  PageSize: number;
  PageDropDown: number[] = [];
  showProjectHistory: boolean = false;
  private resources = servicePath.API.PagingConfigValue;
  totalRecordsCount:number
  lstNonCriticalResourceReportDataSource: MatTableDataSource<ReportsData>;
  nonCriticalResourceReportForm: FormGroup;
  private _onDestroy = new Subject<void>();

  @ViewChild('table') table: MatTable<any>;
  @ViewChild('allSelected') private allSelected: MatOption;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private _NoncriticalResourceReportService: ResourceReportNoncriticalService,  public navService: NavService, private spinner: NgxSpinnerService,private fileExporterService:FileExporterService, private _snackBar: MatSnackBar) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  getData() {
    this.spinner.show()
    this._NoncriticalResourceReportService.GetNonCriticalResourceReport().subscribe(
      (response: ReportsData[]) => {
        this.spinner.hide()
        this.lstNonCriticalResourceReportData = response;
        this.lstNonCriticalResourceReportData.forEach((ele: ReportsData) => {
          ele.Experience = Number(ele.Experience).toFixed(2);
          ele.JoinDate = moment(ele.JoinDate).format("YYYY-MM-DD");
          ele.FutureProjectTentativeDate = ele.FutureProjectTentativeDate && moment(ele.FutureProjectTentativeDate).format("YYYY-MM-DD");
        });
        this.totalRecordsCount = this.lstNonCriticalResourceReportData.length
        this.lstNonCriticalResourceReportDataSource = new MatTableDataSource(this.lstNonCriticalResourceReportData);
        this.lstNonCriticalResourceReportDataSource.paginator = this.paginator;
        this.lstNonCriticalResourceReportDataSource.sort = this.sort;
      },
      (error=> {
        this.spinner.hide();
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Non Critical Resource Report List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
        else{
          this.spinner.hide();
          this._snackBar.open('Some Error occured while fetching the records', 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      }))
  }
  
  ngOnInit() {
    this.cols = [
      { field: 'EmployeeCode', header: 'ID' },
      { field: 'AssociateName', header: 'Name' },
      { field: 'Designation', header: 'Designation' },
      { field: 'Grade', header: 'Grade' },
      { field: 'Technology', header: 'Technology' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'Skill', header: 'Skill' },
      { field: 'Experience', header: 'Experience' },

      { field: 'JoinDate', header: ' SG Joined Date' },
      { field: 'ClientName', header: 'Client' },
      { field: 'IsBillableForExcel', header: 'Billable' }, //, type: this.yesNoPipe
      { field: 'IsCriticalForExcel', header: 'Critical' },
      { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      // { field: 'IsResignedForExcel', header: 'Resigned' },
      // { field: 'ResignationDate', header: 'Resignation Date' },
      // { field: 'LastWorkingDate', header: 'Last Working Date' },
      // { field: 'IsLongLeaveForExcel', header: 'Long Leave' },
      // { field: 'LongLeaveStartDate', header: 'Start Date' },
      // { field: 'TentativeJoinDate', header: 'Tentitive Join Date' },
      { field: 'FutureProjectName', header: 'Future Project'},
      { field: 'FutureProjectTentativeDate', header: 'Future Project Tentative Start Date'}

    ];
    this.defaultSelectedCols();
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.createForm();
    this.nonCriticalResourceReportForm.controls.columnselect.setValue(this.selectedColumns);
    // this.maxDateValue = new Date();
    this.filteredBanksMulti.next(this.cols.slice());
    this.nonCriticalResourceReportForm.controls.columnfilter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });
    this.getData();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.lstNonCriticalResourceReportDataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.lstNonCriticalResourceReportDataSource = new MatTableDataSource(this.lstNonCriticalResourceReportData);
        this.lstNonCriticalResourceReportDataSource.paginator = this.paginator;
        this.lstNonCriticalResourceReportDataSource.sort = this.sort;
      }
    } else {
      this.lstNonCriticalResourceReportDataSource = new MatTableDataSource(this.lstNonCriticalResourceReportData);
      this.lstNonCriticalResourceReportDataSource.paginator = this.paginator;
      this.lstNonCriticalResourceReportDataSource.sort = this.sort;
    }
  }

  private filterBanksMulti() {
    if (!this.cols) {
      return;
    }
    // get the search keyword
    let search = this.nonCriticalResourceReportForm.controls.columnfilter.value;
    if (!search) {
      this.filteredBanksMulti.next(this.cols.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the banks
    this.filteredBanksMulti.next(
      this.cols.filter(ele => ele.header.toLowerCase().indexOf(search) > -1)
    );
  }

  exportAsXLSX(){
    let columnsForExcel=[]
    let columnsForEachRow={}
    for(let i=0;i<this.lstNonCriticalResourceReportDataSource.data.length;i++){
      var eachRowInDataSource = this.lstNonCriticalResourceReportDataSource.data[i]
      columnsForEachRow={}
      for(const key in eachRowInDataSource){
         for(let j=0;j<this.selectedColumns.length;j++){
           if(key === this.selectedColumns[j].field){
             var val = this.selectedColumns[j].header
             columnsForEachRow[val]=eachRowInDataSource[key]
            break
           }
         }
      }
      columnsForExcel.push(columnsForEachRow)
    }
    this.fileExporterService.exportToExcel(columnsForExcel, "Non Critical Resource Report");
  }

  defaultSelectedCols() {
    this.selectedColumns = [

      this.cols[0],
      this.cols[1],
      this.cols[2],
      this.cols[3],
      this.cols[4],
      this.cols[5],
      this.cols[6],
      this.cols[7],
    ];
  }

  createForm() {
    this.nonCriticalResourceReportForm = new FormGroup({
      columnselect: new FormControl(),
      columnfilter: new FormControl(''),
    });
  }

  alterTable(event: any) {
    if (!this.allSelected.selected) {
      this.selectedColumns = event.value;
      this.nonCriticalResourceReportForm.controls.columnselect.setValue(this.selectedColumns);
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
      this.selectedColumns = [...this.cols.map(item => item), 0];
      this.nonCriticalResourceReportForm.controls.columnselect
        .patchValue(this.selectedColumns);
      this.displaycols = this.cols.map(col => col.header);
      this.table.renderRows();
    } else {
      this.nonCriticalResourceReportForm.controls.columnselect.patchValue([]);
      this.selectedColumns = [];
      this.displaycols = [];
      this.table.renderRows();
    }
  }

}
