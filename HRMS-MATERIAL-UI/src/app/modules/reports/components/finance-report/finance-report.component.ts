import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FinanceReportService } from '../../services/finance-report.service';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { ReportsData } from '../../models/reportsdata.model';
import { ReportsFilterData } from '../../models/reportsfilter.model';
import { FinanceReportFilterData } from '../../models/reportsfilter.model';
import { ProjectDetails } from '../../../onboarding/models/projects.model';
import { Validators, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import * as servicePath from '../../../../core/service-paths';
import * as moment from 'moment';
import { BooleanToStringPipe } from '../../pipes/BooleanToStringPipe';
import { themeconfig } from '../../../../../themeconfig';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { FinanceReportData } from '../../models/reportsfilter.model';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';

import { ReplaySubject } from 'rxjs';
import { Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { NavService } from '../../../master-layout/services/nav.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatOption } from '@angular/material/core';
import { DataSource } from '@angular/cdk/table';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { NgxSpinnerService } from 'ngx-spinner';



@Component({
  selector: 'app-finance-report',
  templateUrl: './finance-report.component.html',
  styleUrls: ['./finance-report.component.scss'],
  providers: [FinanceReportService, MasterDataService, BooleanToStringPipe]

})

export class FinanceReportComponent implements OnInit {
  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filteredcols: any[];
  displaycolsfields: any[];
  dataSource: MatTableDataSource<ReportsData>;
  themeAppearence = themeconfig.formfieldappearances;
  Tablegroup: FormGroup;
  displaycols = [];
  displaycolsExcel = [];
  searchData: FinanceReportData;
  projectsList: any[] = [];
  financeReport: ReportsData[] = [];
  errorMessage: any[];
  searchFormSubmitted: boolean = false;
  componentName: string; myForm: FormGroup;
  @ViewChild('financeFilter') financeFilter: any;
  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  //@ViewChildren(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort, {static: false}) sort: MatSort;
  
   @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('table') table: MatTable<any>;

  @ViewChild('allSelected') private allSelected: MatOption;
  errorSummary: string = '';
  reportsFilterData: ReportsFilterData;
  totalRecordsCount: number;
  fromDate: Date;
  toDate: Date;
  maxDateValue: Date;
  daaa: any;
  cols: any[] = [];
  columnOptions: any[] = [];
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  filterDisplay: boolean = false;
  selectedColumns: any[];
  colsForExcel: any[];
  private _onDestroy = new Subject<void>();
  loading: boolean;
  public reportStatus: boolean = false;
  isLoading: boolean = false;
  firstDate: Date;
  lastDate: Date;

  filteredProject : Observable<any>;
  selectedProjectId:any;
  step=0;

  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  constructor(private _financeReportService: FinanceReportService,
    private yesNoPipe: BooleanToStringPipe,
    private activatedRoute: ActivatedRoute,
    private masterDataService: MasterDataService, private fb: FormBuilder, public navService: NavService, private snackBar: MatSnackBar,private fileExporterService:FileExporterService, private spinner: NgxSpinnerService) {
    // this.searchData = new FinanceReportFilterData();
    this.searchData = new FinanceReportData();
    this.reportsFilterData = new ReportsFilterData();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;

  }

  ngOnInit() {
    this.getProjects();

    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
    this.cols = [
      { field: 'EmployeeName', header: 'Associate Name'},
      { field: 'ProjectName', header: 'Project' },
      { field: 'Allocationpercentage', header: '(%) Allocation' },
      { field: 'FromDate', header: 'From Date' },
      { field: 'ToDate', header: 'To Date' },
      { field: 'IsBillableForExcel', header: 'Billable' },
      { field: 'IsCriticalForExcel', header: 'Critical' },
      { field: 'EmployeeCode', header: 'Associate ID' },
      // { field: 'EmployeeName', header:'Associate Name' },
      { field: 'DesignationName', header: 'Designation Name' },
      { field: 'GradeName', header: 'Grade' },
      // { field:'ProjectName', header:'Project' },
      { field: 'SkillCode', header: 'Skill' },
      { field: 'RoleName', header: 'Role' },
      { field: 'ClientName', header: 'Client' },
      // { field: 'Allocationpercentage', header: '(%) Allocation' },
      { field: 'ClientBillingPercentage', header: '(%) Client Billing' },
      { field: 'InternalBillingPercentage', header: '(%) Internal Billing' },
      { field: 'ClientBillingRoleCode', header: 'Client Billing Role' },
      { field: 'InternalBillingRoleCode', header: 'Internal Billing Role' },

      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      { field: 'IsResignedForExcel', header: 'Resigned' },
      { field: 'IsLongLeaveForExcel', header: 'Long Leave' },

    ];

    this.columnOptions = [];

    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.cols.length; i++) {

      this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });

    }
    this.selectedColumns = [
      this.cols[0],
      this.cols[1],
      this.cols[2],
      this.cols[3],
      this.cols[4],
      this.cols[5],
      this.cols[6]

    ];
    this.colsForExcel = [
      this.cols[7],
      this.cols[0],
      this.cols[8],
      this.cols[9],
      this.cols[10],
      this.cols[12],
      this.cols[1],
      this.cols[2],
      this.cols[13],
      this.cols[15],
      this.cols[5],
      this.cols[6],
      this.cols[18],
      this.cols[19],
      this.cols[3],
      this.cols[4],
    ]
    this.filteredcols = this.cols;
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.displaycolsfields = this.selectedColumns.map(col => col.field);
    this.displaycolsExcel = this.colsForExcel.map(col => col.header);
    this.myForm = this.fb.group({
      fromDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      ProjectId: ['', null],
      columnselect: [],
      columnfilter: ['']
    });
    this.myForm.controls.columnselect.setValue(this.selectedColumns);
    this.maxDateValue = new Date();
    this.filteredBanksMulti.next(this.cols.slice());
    this.myForm.controls.columnfilter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });

    this.getDates();
    
  }

 

  clearInput(evt: any, fieldName): void {
    if(fieldName=='toDate'){
      evt.stopPropagation();
      this.myForm.get('toDate').reset();
    }
    if(fieldName=='fromDate'){
      evt.stopPropagation();
      this.myForm.get('fromDate').reset();
    }
  }

  private getDates(): void {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.firstDate = new Date(y, m, 1);
    this.lastDate = new Date();
  }


  // tslint:disable-next-line:use-lifecycle-interface
  // ngAfterViewInit() {     this.dataSource.sort = this.sort.toArray()[0];   }
 // ngAfterViewInit() { this.dataSource.sort = this.sort; }
 ngAfterViewInit() {
 this.dataSource = new MatTableDataSource();
 this.dataSource.paginator = this.paginator.toArray()[0];
 this.dataSource.sort = this.sort.toArray()[0];
 
}
  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.dataSource = new MatTableDataSource(this.financeReport);
        this.dataSource.paginator = this.paginator.toArray()[0];
        this.dataSource.sort = this.sort.toArray()[0];
        // this.dataSource.paginator = this.paginator;
        // this.dataSource.sort = this.sort;
        
      }
    } else {
      this.dataSource = new MatTableDataSource(this.financeReport);
      this.dataSource.paginator = this.paginator.toArray()[0];
      this.dataSource.sort = this.sort.toArray()[0];
      // this.dataSource.paginator = this.paginator;
      // this.dataSource.sort = this.sort;
      
    }
  }
  private filterBanksMulti() {
    if (!this.cols) {
      return;
    }
    // get the search keyword
    let search = this.myForm.controls.columnfilter.value;
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
  filter(event: any) {

    let filterValue = this.myForm.controls.columnfilter.value;
    if (filterValue) {
      return this.filteredcols = this.cols.filter(option => option.header.toLowerCase().includes(filterValue));
    }
    else {
      return this.cols;
    }
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
  

  
  getProjects(): void {
    this.masterDataService.GetProjectsList().subscribe((res: ProjectDetails[]) => {
      let projList: any[] = [];
      this.projectsList = [];
      projList.push({ label: '', value: null });
      res.forEach((element: ProjectDetails) => {
        projList.push({ label: element.ProjectName, value: element.ProjectId });
      });
      this.projectsList = projList.filter(
        (project, index, arr) => arr.findIndex(t => t.value === project.value) === index);

        this.filteredProject = this.myForm.valueChanges.pipe(
          startWith(''),
          map((value) => this._filterProject(value))
          ); 
    },
      (error: any) => {
        this.errorMessage = [];
        this.snackBar.open('Failed to Get Project List!', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
    );
  }

  private _filterProject(value) {
    let filterValue;
    if (value && value.ProjectId) {
      filterValue = value.ProjectId.toLowerCase();
      return this.projectsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectsList;
    }
  }

  clearField() {
    this.myForm.controls.ProjectId.setValue(null);
    this.searchData.ProjectId = this.myForm.controls.ProjectId.value;
  }

  selectedChangeIds(item){
    this.selectedProjectId = item.value;
    this.searchData.ProjectId = this.selectedProjectId;

  }

  fetchResourceReport() {
    this.spinner.show()
    if(this.myForm.valid){
      this.step = 1;
    }
  
    this.isLoading = true;
    this.searchFormSubmitted = true;
    if (this.myForm.controls.fromDate.value === '' || this.myForm.controls.toDate.value === '' || this.myForm.controls.fromDate.value === null || this.myForm.controls.toDate.value === null) {
      this.isLoading = false;
      this.spinner.hide()
      return;
    }

    if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
      this.errorSummary = '';
      if (moment(this.searchData.FromDate).isSameOrAfter(new Date())) {
        this.errorSummary = 'From date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this.snackBar.open('From date should be less than today', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });

        return false;
      }
      if (moment(this.searchData.ToDate).isSameOrAfter(new Date())) {
        this.errorSummary = 'To date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this.snackBar.open('To date should be less than today', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
      if (moment(this.searchData.FromDate).isSameOrAfter(this.searchData.ToDate)) {
        this.errorSummary = 'From Date should be less than To Date';
        this.isLoading = false;
        this.spinner.hide()
        this.snackBar.open('From Date should be less than To Date', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
      this.searchData.RowsPerPage = 10;
      this.searchData.PageNumber = 1;
      this.reportsFilterData = new ReportsFilterData();
      this.reportsFilterData.financeReportFilterData = new FinanceReportFilterData();
      this.reportsFilterData.financeReportFilterData.FromDate = moment(this.searchData.FromDate).toDate();
      this.reportsFilterData.financeReportFilterData.ToDate = moment(this.searchData.ToDate).toDate()
      this.reportsFilterData.financeReportFilterData.ProjectId = this.searchData.ProjectId;
      this.reportStatus = true;
      this.searchData.FromDate = moment(this.searchData.FromDate).format('YYYY-MM-DD');
      this.searchData.ToDate = moment(this.searchData.ToDate).format('YYYY-MM-DD');
      this._financeReportService.GetFinanceReport(this.searchData).subscribe((reportsData: Array<ReportsData>) => {
        this.spinner.hide()
        if (reportsData) {
          this.reportStatus = false;
          this.financeReport = reportsData;
          this.financeReport.forEach((record: ReportsData) => {
            record.FromDate = moment(record.FromDate).format('DD-MM-YYYY');
            record.ToDate = moment(record.ToDate).format('DD-MM-YYYY');
            record.IsBillableForExcel = (record.IsBillable == true) ? 'Yes' : 'No';
            record.IsCriticalForExcel = (record.IsCritical == true) ? 'Yes' : 'No';
            record.IsResignedForExcel = (record.IsResigned == true) ? 'Yes': null;
            record.IsLongLeaveForExcel =(record.IsLongLeave == true) ? 'Yes': null;
          });
          this.isLoading = false;
          this.dataSource = new MatTableDataSource<ReportsData>(this.financeReport);
 

           //this.dataSource.paginator = this.paginator;
           //this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator.toArray()[0];
          this.dataSource.sort = this.sort.toArray()[0]; 
          // this.dataSource.sortingDataAccessor = (item, property) => {
          //     return item[property];
          //   };

          
          this.totalRecordsCount = this.financeReport.length;
          this.fromDate = moment(this.searchData.FromDate).toDate();
          this.toDate = moment(this.searchData.ToDate).toDate();
          this.filterDisplay = false;
        }
        else {
          this.isLoading = false;this.spinner.hide()
        }
      },
        (error: any) => {
          this.spinner.hide()
          this.errorMessage = [];
          this.isLoading = false;
          this.snackBar.open('Project Resource Not Found', 'x', {
            duration: 5000,
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
          });
        });
    }

  }
 
  clearFilter() {
    this.formGroupDirective.resetForm();
    this.myForm.controls.columnselect.setValue(this.selectedColumns);
    this.searchFormSubmitted = false;
    this.errorSummary = '';
    // this.searchData = new FinanceReportFilterData();
    this.searchData = new FinanceReportData();

    this.financeReport = [];
    // this.dataSource = new MatTableDataSource(this.financeReport);
    // this.dataSource.paginator = this.paginator;
    // this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator.toArray()[0];
    this.dataSource.sort = this.sort.toArray()[0];
    this.totalRecordsCount = this.financeReport.length;
   
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.displaycolsfields = this.selectedColumns.map(col => col.field);


  }

  onFilter() {
    this.getProjects();
    this.filterDisplay = true;
    this.myForm.reset();
    this.searchFormSubmitted = false;
    this.errorSummary = '';
  }

  clearErrorSummary(event: any) {
    this.errorSummary = '';
  }

  exportAsXLSX() {
    const columnsForExcel = this.dataSource.data.map(x => ({
      'Associate Id': x.EmployeeCode,
      'Associate Name': x.EmployeeName,
      'Designation Name': x.DesignationName,
      'Grade': x.GradeName,
      'Skill': x.SkillCode,
      'Client': x.ClientName,
      'Project': x.ProjectName,
      '(%) Allocation': x.Allocationpercentage,
      '(%) Client Billing': x.ClientBillingPercentage,
      'Client Billing Role': x.ClientBillingRoleCode,
      'Billable': x.IsBillableForExcel,
      'Critical': x.IsCriticalForExcel,
      'Reporting Manager': x.ReportingManagerName,
      'Program Manager': x.ProgramManagerName,
      'From Date': x.FromDate,
      'To Date': x.ToDate
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, "Finance Report");
  }
}


