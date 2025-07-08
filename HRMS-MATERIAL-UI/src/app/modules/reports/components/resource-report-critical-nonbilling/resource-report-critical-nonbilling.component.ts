import { Component, OnInit, ViewChild } from '@angular/core';
import { ResourceReportCriticalService } from '../../services/resource-report-critical.service';
import { EmployeeReportData } from '../../models/employee.model';
import * as servicePath from '../../../../core/service-paths';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AllColorCodes } from '../../../../core/color-codes';
import { NavService } from '../../../master-layout/services/nav.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ReportsData } from '../../models/reportsdata.model';
import { FileExporterService } from 'src/app/core/services/file-exporter.service'
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatOption } from '@angular/material/core';
import { FormControl, FormGroup } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-resource-report-critical-nonbilling',
  templateUrl: './resource-report-critical-nonbilling.component.html',
  styleUrls: ['./resource-report-critical-nonbilling.component.scss']

})
export class ResourceReportCriticalNonbillingComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;

  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  cols: any[] = [];
  selectedColumns: any[];
  displaycols: any[];
  public lstCriticalResourceReportData: ReportsData[] = []
  lstEmployees: EmployeeReportData[] = []; // Used to store employee details of a specific pool
  public heading: string;
  colorCodes = AllColorCodes;
  PageSize: number;
  PageDropDown: number[] = [];
  showProjectHistory: boolean = false;
  private resources = servicePath.API.PagingConfigValue;
  totalRecordsCount:number
  private _onDestroy = new Subject<void>();
  lstCriticalResourceReportDataSource: MatTableDataSource<ReportsData>;
  criticalResourceReportForm: FormGroup;
  @ViewChild('table') table: MatTable<any>;
  @ViewChild('allSelected') private allSelected: MatOption;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private _criticalResourceReportService: ResourceReportCriticalService,  public navService: NavService, private spinner: NgxSpinnerService,private fileExporterService:FileExporterService, private _snackBar: MatSnackBar) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  getData() {
    this.spinner.show()
    this._criticalResourceReportService.GetCriticalResourceReport().subscribe(
      (response: ReportsData[]) => {
        this.spinner.hide()
        this.lstCriticalResourceReportData = response;
        this.lstCriticalResourceReportData.forEach((ele: ReportsData) => {
          ele.Experience = Number(ele.Experience).toFixed(2);
        });
        this.totalRecordsCount = this.lstCriticalResourceReportData.length
        this.lstCriticalResourceReportDataSource = new MatTableDataSource(this.lstCriticalResourceReportData);
        this.lstCriticalResourceReportDataSource.paginator = this.paginator;
        this.lstCriticalResourceReportDataSource.sort = this.sort;
      },
      (error=> {
        this.spinner.hide();
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Critical Resource Report List'], horizontalPosition: 'right',
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
    ];
    this.defaultSelectedCols();
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.createForm();
    this.criticalResourceReportForm.controls.columnselect.setValue(this.selectedColumns);
    // this.maxDateValue = new Date();
    this.filteredBanksMulti.next(this.cols.slice());
    this.criticalResourceReportForm.controls.columnfilter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });
    this.getData();
  }

  private filterBanksMulti() {
    if (!this.cols) {
      return;
    }
    // get the search keyword
    let search = this.criticalResourceReportForm.controls.columnfilter.value;
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
    this.criticalResourceReportForm = new FormGroup({
      columnselect: new FormControl(),
      columnfilter: new FormControl(''),
    });
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.lstCriticalResourceReportDataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.lstCriticalResourceReportDataSource = new MatTableDataSource(this.lstCriticalResourceReportData);
        this.lstCriticalResourceReportDataSource.paginator = this.paginator;
        this.lstCriticalResourceReportDataSource.sort = this.sort;
      }
    } else {
      this.lstCriticalResourceReportDataSource = new MatTableDataSource(this.lstCriticalResourceReportData);
      this.lstCriticalResourceReportDataSource.paginator = this.paginator;
      this.lstCriticalResourceReportDataSource.sort = this.sort;
    }
  }

  exportAsXLSX(){
    let columnsForExcel=[]
    let columnsForEachRow={}
    for(let i=0;i<this.lstCriticalResourceReportDataSource.data.length;i++){
      var eachRowInDataSource = this.lstCriticalResourceReportDataSource.data[i]
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
    this.fileExporterService.exportToExcel(columnsForExcel, "Critical Resource Report");
  }

  alterTable(event: any) {
    if (!this.allSelected.selected) {
      this.selectedColumns = event.value;
      this.criticalResourceReportForm.controls.columnselect.setValue(this.selectedColumns);
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
      this.criticalResourceReportForm.controls.columnselect
        .patchValue(this.selectedColumns);
      this.displaycols = this.cols.map(col => col.header);
      this.table.renderRows();
    } else {
      this.criticalResourceReportForm.controls.columnselect.patchValue([]);
      this.selectedColumns = [];
      this.displaycols = [];
      this.table.renderRows();
    }
  }
  }