import { Component, OnInit, ViewChildren, QueryList, ViewChild } from '@angular/core';
import { Validators, FormGroup, FormBuilder, FormGroupDirective} from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { AnalysisData, AnalysisFilterData, ReportsFilterData } from '../../Models/analysis.model'
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as servicePath from '../../../../core/service-paths';
import * as moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { AssociateExitCauseFormComponent } from '../associate-exit-cause-form/associate-exit-cause-form.component';
import { ViewAnalysisFormComponent } from '../view-analysis-form/view-analysis-form.component';
import { TableUtil } from "../../Services/exportUtil";

@Component({
  selector: 'app-associate-exit-analysis',
  templateUrl: './associate-exit-analysis.component.html',
  styleUrls: ['./associate-exit-analysis.component.scss']
})
export class AssociateExitAnalysisComponent implements OnInit {
  themeAppearence = themeconfig.formfieldappearances;
  step = 0;
  dataSource: MatTableDataSource<AnalysisData>;
  searchData: AnalysisFilterData;
  analysisData: AnalysisData[];
  searchFormSubmitted: boolean = false;
  displayCause: boolean = false;
  displaySelectProject: boolean = false;
  showGrid: boolean = false;
  actionexport = true;
  errorSummary: string = '';
  reportsFilterData: ReportsFilterData;
  totalRecordsCount: number;
  FromDate: Date;
  ToDate: Date;
  fromDate: string;
  toDate: string;
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  filterDisplay: boolean = false;
  isLoading: boolean = false;
  showHideDetails: boolean = false;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  myForm: FormGroup;

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;


  displayedColumns: string[] = [
    'EmployeeCode', 'EmployeeName', 'ExitDate', 'ExitType',
    'ExitReasonDetail', 'SummaryOfExitFeedback', 'RootCause', 'ActionItem',
    'TagretDate', 'ActualDate', 'Responsibility', 'Remarks', 'Status', 'Action'
  ];

  constructor(private causeService: ExitAnalysisService,
    public dialog: MatDialog,
    private fb: FormBuilder,
    public navService: NavService,
    private snackBar: MatSnackBar) {
    this.searchData = new AnalysisFilterData();
    this.reportsFilterData = new ReportsFilterData();

    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;

  }

  ngOnInit() {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
    this.myForm = this.fb.group({
      FromDate: ['', [Validators.required]],
      ToDate: [ '', [Validators.required]],
    });

  }
  show_hide_details() {
    this.showHideDetails = !this.showHideDetails;
  }
  ngAfterViewInit() { this.dataSource.sort = this.sort.toArray()[0]; }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.dataSource = new MatTableDataSource(this.analysisData);
        this.dataSource.paginator = this.paginator.toArray()[0];
        this.dataSource.sort = this.sort.toArray()[0];
      }
    } else {
      this.dataSource = new MatTableDataSource(this.analysisData);
      this.dataSource.paginator = this.paginator.toArray()[0];
      this.dataSource.sort = this.sort.toArray()[0];
    }
  }


  submitReport() {
    this.isLoading = true;
    this.step = 1;
    this.searchFormSubmitted = true;
    if (this.myForm.controls.FromDate.value === '' || this.myForm.controls.ToDate.value === '' || this.myForm.controls.FromDate.value === null || this.myForm.controls.ToDate.value === null) {
      this.isLoading = false;
      return;
    }

    if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
      this.errorSummary = '';
      if (moment(this.searchData.FromDate).isSameOrAfter(new Date())) {
        this.errorSummary = 'From date should be less than today';
        this.isLoading = false;
        this.snackBar.open('From date should be less than today', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });

        return false;
      }
      if (moment(this.searchData.ToDate).isSameOrAfter(new Date())) {
        this.errorSummary = 'To date should be less than today';
        this.isLoading = false;
        this.snackBar.open('To date should be less than today', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
      if (moment(this.searchData.FromDate).isSameOrAfter(this.searchData.ToDate)) {
        this.errorSummary = 'From Date should be less than To Date';
        this.isLoading = false;
        this.snackBar.open('From Date should be less than To Date', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
      this.reportsFilterData = new ReportsFilterData();
      this.reportsFilterData.causeFilterData = new AnalysisFilterData();
      this.reportsFilterData.causeFilterData.FromDate = moment(this.searchData.FromDate).toDate();
      this.reportsFilterData.causeFilterData.ToDate = moment(this.searchData.ToDate).toDate();
      let from_date = this.reportsFilterData.causeFilterData.FromDate.getFullYear() + "/" + (this.reportsFilterData.causeFilterData.FromDate.getMonth() + 1) + "/" + this.reportsFilterData.causeFilterData.FromDate.getDate();
      let to_date = this.reportsFilterData.causeFilterData.ToDate.getFullYear() + "/" + (this.reportsFilterData.causeFilterData.ToDate.getMonth() + 1) + "/" + this.reportsFilterData.causeFilterData.ToDate.getDate();
      this.causeService.getAssociateExitAnalysis(from_date, to_date).subscribe((res: AnalysisData[]) => {

        if (res) {
          this.analysisData = res;
          this.isLoading = false;
          this.showGrid = true;
          this.dataSource = new MatTableDataSource<AnalysisData>(this.analysisData);
          this.dataSource.paginator = this.paginator.toArray()[0];
          this.dataSource.sort = this.sort.toArray()[0];
          this.totalRecordsCount = this.analysisData.length;
          this.FromDate = moment(this.searchData.FromDate).toDate();
          this.ToDate = moment(this.searchData.ToDate).toDate();
          this.filterDisplay = false;
        }
        else {
          this.isLoading = false;
        }
      },

      );
    }
  }

  onEdit(selectedData: any): void {
    selectedData = selectedData;
    let originaldata = this.dataSource;

    let dialogRef = this.dialog.open(AssociateExitCauseFormComponent, {
      width: '60vw', height: '30vw',
      disableClose: true,
      data: { element: selectedData }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.submitReport();
    });
  }


  onView(selectedData: any): void {
    selectedData = selectedData;
    let originaldata = this.dataSource;

    let dialogRef = this.dialog.open(ViewAnalysisFormComponent, {
      width: '60vw', height: '25vw',
      disableClose: true,
      data: { element: selectedData }
    });
    dialogRef.afterClosed().subscribe(result => {
      });
  }

  clear() {
    this.formGroupDirective.resetForm();
    this.searchFormSubmitted = false;
    this.errorSummary = '';
    this.searchData = new AnalysisFilterData();
    this.showGrid = false;
  }

  exporter() {
    TableUtil.exportTableToExcel("ExampleMaterialTable");
    this.actionexport = false;
  }
}
