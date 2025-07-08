import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, ReplaySubject, Subject } from 'rxjs';
import { themeconfig } from '../../../../../themeconfig';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { DropDownType, GenericType, GenericModel } from '../../../master-layout/models/dropdowntype.model';
import { ReportsData } from '../../models/reportsdata.model';
import { ReportsFilterData, UtilizationReportFilterData } from '../../models/reportsfilter.model';
import { ProjectDetails } from '../../models/resourcereportbyproject.model';
import { startWith, map, takeUntil } from 'rxjs/operators';
import * as moment from "moment";
import { ResourceReportService } from '../../services/resource-report.service';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatTableExporterModule } from 'mat-table-exporter';
import { MatOption } from '@angular/material/core';
import { FileExporterService } from 'src/app/core/services/file-exporter.service'
import { NgxSpinnerService } from 'ngx-spinner';


interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-resource-report',
  templateUrl: './resource-report.component.html',
  styleUrls: ['./resource-report.component.scss']
})

export class ResourceReportComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;

  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filteredcols: any[];
  displaycolsfields: any[];
  displaycols = [];
  private _onDestroy = new Subject<void>();
  filteredAssociates: Observable<any>;
  filteredTechnology: Observable<any>;
  filteredProject: Observable<any>;
  filteredProgramManager: Observable<any>;
  filteredClient: Observable<any>;
  filteredExperience: Observable<any>;
  filteredGrade: Observable<any>;
  filteredDesignation: Observable<any>;
  expandFilter: boolean = true;

  step = 0;
  afterSearchFilter: boolean = false;
  cols: any[] = [];
  columnOptions: any[] = [];
  PageSize: number = 5;
  PageDropDown: number[] = [];
  // private resources = servicePath.API.PagingConfigValue;
  associateUtilizationReportList: ReportsData[] = [];
  private associateUtilizationReportExcel: ReportsData[] = [];
  reportsFilterData: ReportsFilterData;
  private resourceFilter: UtilizationReportFilterData;
  componentName: string;
  filterDisplay: boolean = false;
  technologyList: SelectItem[] = [];
  associatesList: SelectItem[] = [];
  projectsList: SelectItem[] = [];
  gradesList: SelectItem[] = [];
  experienceList: SelectItem[] = [];
  clinentsList: SelectItem[] = [];
  programManagersList: SelectItem[] = [];
  BillableList: SelectItem[] = [];
  CriticalList: SelectItem[] = [];
  designationsList: SelectItem[] = [];
  percentageList: SelectItem[] = [];
  expList: GenericType[] = [];
  totalRecordsCount: number = 0;
  allBillable: number = -1;
  allCritical: number = -1;
  utilizationPercentage: number = 0;
  utilizationPercentageFromUI: number = 0;
  isBillable = new Array<DropDownType>();
  isCritical = new Array<DropDownType>();
  private userRole: any;
  private employeeId: any;
  selectedColumns: any[];
  resourceReportForm: FormGroup;

  selectedEmployeeId: any;
  selectedPracticeAreaId: any;
  selectedProjectId: any;
  selectedProgramManagerId: any;
  selectedClientId: any;
  selectedExperienceId: any;
  selectedGradeId: any;
  selectedDesignationId: any;

  dataSource: MatTableDataSource<ReportsData>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('table') table: MatTable<any>;

  @ViewChild('allSelected') private allSelected: MatOption;

  constructor(private _masterDataService: MasterDataService, private _snackBar: MatSnackBar,
    private _resourceReportService: ResourceReportService, public navService: NavService, private fileExporterService:FileExporterService, private spinner: NgxSpinnerService) {
    this.reportsFilterData = new ReportsFilterData();
    this.reportsFilterData.utilizationReportFilterData = new UtilizationReportFilterData();
    this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })

  }

  ngOnInit(): void {

    this.userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.resetFilter();

    this.cols = [

      { field: 'EmployeeCode', header: 'ID' },
      { field: 'EmployeeName', header: 'Name' },
      { field: 'DesignationName', header: 'Designation' },
      { field: 'GradeName', header: 'Grade' },
      { field: 'ExperienceExcludingCareerBreak', header: 'Experience' },
      { field: 'Technology', header: 'Technology' },
      { field: 'JoinDate', header: ' SG Joined Date' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'ClientName', header: 'Client' },
      { field: 'IsBillableForExcel', header: 'Billable' }, //, type: this.yesNoPipe
      { field: 'IsCriticalForExcel', header: 'Critical' },
      { field: 'Allocationpercentage', header: '(%) Utilization' },
      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
      { field: 'SkillCode', header: 'Skill' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      { field: 'IsResignedForExcel', header: 'Resigned' },
      { field: 'ResignationDate', header: 'Resignation Date' },
      { field: 'LastWorkingDate', header: 'Last Working Date' },
      { field: 'IsLongLeaveForExcel', header: 'Long Leave' },
      { field: 'LongLeaveStartDate', header: 'Long Leave Start Date' },
      { field: 'TentativeJoinDate', header: 'Long Leave Tentative Join Date' },
      { field: 'FutureProjectName', header: 'Future Project'},
      { field: 'FutureProjectTentativeDate', header: 'Future Project Tentative Start Date'}

    ];

    this.columnOptions = [];
    for (let i = 0; i < this.cols.length; i++) {
      this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });
    }

    this.defaultSelectedCols();

    this.filteredcols = this.cols;
    this.displaycols = this.selectedColumns.map(col => col.header);
    this.displaycolsfields = this.selectedColumns.map(col => col.field);
    this.createForm();
    this.resourceReportForm.controls.columnselect.setValue(this.selectedColumns);
    // this.maxDateValue = new Date();
    this.filteredBanksMulti.next(this.cols.slice());
    this.resourceReportForm.controls.columnfilter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });

    this.isBillable.push({ label: 'All', value: -1 });
    this.isBillable.push({ label: 'Yes', value: 1 });
    this.isBillable.push({ label: 'No', value: 0 });

    this.isCritical.push({ label: 'All', value: -1 });
    this.isCritical.push({ label: 'Yes', value: 1 });
    this.isCritical.push({ label: 'No', value: 0 });


    if (!this.afterSearchFilter) {

      if (this.reportsFilterData.utilizationReportFilterData.IsBillable == -1) this.reportsFilterData.utilizationReportFilterData.IsBillable = 0;
      if (this.reportsFilterData.utilizationReportFilterData.IsCritical == -1) this.reportsFilterData.utilizationReportFilterData.IsCritical = 0
      this.associatesList = [];
      this.associatesList.splice(0, 0, { label: '', value: 0 });
      this.projectsList = [];
      this.projectsList.splice(0, 0, { label: '', value: 0 });
      this.gradesList = [];
      this.gradesList.splice(0, 0, { label: '', value: 0 });
      this.experienceList = [];
      this.experienceList.splice(0, 0, { label: '', value: 0 });
      this.clinentsList = [];
      this.clinentsList.splice(0, 0, { label: '', value: 0 });
      this.programManagersList = [];
      this.programManagersList.splice(0, 0, { label: '', value: 0 });
      this.BillableList = [];
      this.BillableList.splice(0, 0, { label: '', value: 0 });
      this.CriticalList = [];
      this.CriticalList.splice(0, 0, { label: '', value: 0 });
      this.designationsList = [];
      this.designationsList.splice(0, 0, { label: '', value: 0 });
      this.percentageList = [];
      this.percentageList.splice(0, 0, { label: '', value: 0 });
      this.technologyList = [];
      this.technologyList.splice(0, 0, { label: '', value: 0 });
      this.expList = [
        { Id: 1, Name: "0-5" },
        { Id: 2, Name: "5-10" },
        { Id: 3, Name: "10-15" },
        { Id: 4, Name: "15-20" },
        { Id: 5, Name: "20-25" },
        { Id: 6, Name: "25-30" },
        { Id: 7, Name: "30-35" },
        { Id: 8, Name: "35-40" }];
      this.getAssociateList();
      this.getExperienceList();
      this.getProjectList();
      this.getGradeList();
      this.getClientList();
      this.getProgramManagerList();
      this.getDesignationsList();
      this.getPercentageList();
      this.getTechnologyList();

      this.resourceReportForm.controls["IsBillable"].setValue(-1);
      this.resourceReportForm.controls["IsCritical"].setValue(-1);

      this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
      this.reportsFilterData.utilizationReportFilterData.IsCritical = -1;

      //this.resetFilter();
    }
    this.filterDisplay = true;
  }

  ngAfterViewInit() { this.dataSource.sort = this.sort; }
  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
        console.log(filterValue,this.dataSource.filter)
      }
      else {
        this.dataSource = new MatTableDataSource(this.associateUtilizationReportList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    } else {
      this.dataSource = new MatTableDataSource(this.associateUtilizationReportList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  private filterBanksMulti() {
    if (!this.cols) {
      return;
    }
    // get the search keyword
    let search = this.resourceReportForm.controls.columnfilter.value;
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

    let filterValue = this.resourceReportForm.controls.columnfilter.value;
    if (filterValue) {
      return this.filteredcols = this.cols.filter(option => option.header.toLowerCase().includes(filterValue));
    }
    else {
      return this.cols;
    }
  }
  alterTable(event: any) {
    if (!this.allSelected.selected) {
      this.selectedColumns = event.value;
      this.resourceReportForm.controls.columnselect.setValue(this.selectedColumns);
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
      this.resourceReportForm.controls.columnselect
        .patchValue(this.selectedColumns);
      this.displaycols = this.cols.map(col => col.header);
      this.table.renderRows();
    } else {
      this.resourceReportForm.controls.columnselect.patchValue([]);
      this.selectedColumns = [];
      this.displaycols = [];
      this.table.renderRows();
    }
  }





  resetFilter(): void {

    this.allBillable = -1;
    this.allCritical = -1;
    this.utilizationPercentage = 0;
    this.reportsFilterData.utilizationReportFilterData = {
      EmployeeId: 0,
      ProjectId: 0,
      GradeId: 0,
      DesignationId: 0,
      ClientId: 0,
      AllocationPercentageId: 0,

      ProgramManagerId: 0,
      ExperienceId: 0,
      ExperienceRange: null,
      Experience: 0,
      IsBillable: -1,
      IsCritical: -1,

      isExportToExcel: false,
      PracticeAreaId: 0,
      RowsPerPage: 20,
      PageNumber: 1,
    };
  }

  reset() {
    this.reportsFilterData.utilizationReportFilterData = {
      EmployeeId: 0,
      ProjectId: 0,
      GradeId: 0,
      DesignationId: 0,
      ClientId: 0,
      AllocationPercentageId: 0,

      ProgramManagerId: 0,
      ExperienceId: 0,
      ExperienceRange: null,
      Experience: 0,
      IsBillable: -1,
      IsCritical: -1,

      isExportToExcel: false,
      PracticeAreaId: 0,
      RowsPerPage: 20,
      PageNumber: 1,
    };
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
      this.cols[8],
      this.cols[9],
      this.cols[10],
      this.cols[11],
      this.cols[12],
      this.cols[13],
      this.cols[15],
      this.cols[16],
      this.cols[19]

    ];
  }


  createForm() {

    this.resourceReportForm = new FormGroup({
      EmployeeId: new FormControl(null),
      PracticeAreaId: new FormControl(null),
      ProjectId: new FormControl(null),
      ProgramManagerId: new FormControl(null),
      ClientId: new FormControl(null),
      ExperienceId: new FormControl(null),
      GradeId: new FormControl(null),
      DesignationId: new FormControl(null),
      UtilizationPercentage: new FormControl(null),
      IsBillable: new FormControl(null),
      IsCritical: new FormControl(null),
      columnselect: new FormControl(),
      columnfilter: new FormControl(''),
    });
  }

  clearField(fieldName) {

    if (fieldName == 'EmployeeId') {
      this.resourceReportForm.controls.EmployeeId.setValue(null);
    }
    if (fieldName == 'PracticeAreaId') {
      this.resourceReportForm.controls.PracticeAreaId.setValue(null);
    }
    if (fieldName == 'ProjectId') {
      this.resourceReportForm.controls.ProjectId.setValue(null);
    }
    if (fieldName == 'ProgramManagerId') {
      this.resourceReportForm.controls.ProgramManagerId.setValue(null);
    }
    if (fieldName == 'ClientId') {
      this.resourceReportForm.controls.ClientId.setValue(null);
    }
    if (fieldName == 'ExperienceId') {
      this.resourceReportForm.controls.ExperienceId.setValue(null);
    }
    if (fieldName == 'GradeId') {
      this.resourceReportForm.controls.GradeId.setValue(null);
    }
    if (fieldName == 'DesignationId') {
      this.resourceReportForm.controls.DesignationId.setValue(null);
    }
  }

  getAssociateList(): void {
    this._masterDataService.GetAllAssociateList().subscribe((associateResponse: GenericModel[]) => {
      associateResponse.forEach((associateResponse: GenericModel) => {
        if(associateResponse.DepartmentId != null && associateResponse.DepartmentId == 1) {
          this.associatesList.push({ label: associateResponse.Name, value: associateResponse.Id });
          }  
      });

      this.filteredAssociates = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterAssociates(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Associate List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };

  }

  private _filterAssociates(value) {
    let filterValue;
    if (value && value.EmployeeId) {
      filterValue = value.EmployeeId.toLowerCase();
      return this.associatesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.associatesList;
    }
  }

  getProjectList(): void {
    this._masterDataService.GetProjectsList().subscribe((projectResponse: GenericType[]) => {
      let projList: any[] = [];
      this.projectsList = [];
      projList.push({ label: '', value: null });
      projectResponse.forEach((projectResponse: GenericType) => {
        projList.push({ label: projectResponse.Name, value: projectResponse.Id });
      });
      this.projectsList = projList.filter(
        (project, index, arr) => arr.findIndex(t => t.value === project.value) === index);

      this.filteredProject = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProject(value))
      );
    },
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Project List'], horizontalPosition: 'right',
            verticalPosition: 'top'
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



  getGradeList(): void {
    this._masterDataService.GetGradesDetails().subscribe((gradeResponse: GenericType[]) => {
      gradeResponse.forEach((gradeResponse: GenericType) => {
        this.gradesList.push({ label: gradeResponse.Name, value: gradeResponse.Id })
      });

      this.filteredGrade = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterGrade(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Grade List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }

  private _filterGrade(value) {

    let filterValue;
    if (value && value.GradeId) {
      filterValue = value.GradeId.toLowerCase();
      return this.gradesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.gradesList;
    }
  }

  getExperienceList(): void {
    this.expList.forEach((experience: GenericType) => {
      this.experienceList.push({ label: experience.Name, value: experience.Id });
    });

    this.filteredExperience = this.resourceReportForm.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterExperience(value))
    );
  }

  private _filterExperience(value) {

    let filterValue;
    if (value && value.ExperienceId) {
      filterValue = value.ExperienceId.toLowerCase();
      return this.experienceList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.experienceList;
    }
  }

  getClientList(): void {
    this._masterDataService.GetClientList().subscribe((clientResponse: GenericType[]) => {
      clientResponse.forEach((clientResponse: GenericType) => {
        this.clinentsList.push({ label: clientResponse.Name, value: clientResponse.Id })
      });

      this.filteredClient = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterClient(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Client List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }

  private _filterClient(value) {

    let filterValue;
    if (value && value.ClientId) {
      filterValue = value.ClientId.toLowerCase();
      return this.clinentsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.clinentsList;
    }
  }


  getProgramManagerList(): void {
    this._masterDataService.GetProgramManagers().subscribe((programManagerResponse: GenericType[]) => {
      programManagerResponse.forEach((programManagerResponse: GenericType) => {
        this.programManagersList.push({ label: programManagerResponse.Name, value: programManagerResponse.Id })
      })
      this.filteredProgramManager = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProgramManager(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Program Manager List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }

  private _filterProgramManager(value) {

    let filterValue;
    if (value && value.ProgramManagerId) {
      filterValue = value.ProgramManagerId.toLowerCase();
      return this.programManagersList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.programManagersList;
    }
  }

  getDesignationsList(): void {
    this._masterDataService.GetDesignationListForDropDown().subscribe((designationResponse: GenericType[]) => {
      designationResponse.forEach((designationResponse: GenericType) => {
        this.designationsList.push({ label: designationResponse.Name, value: designationResponse.Id })
      });

      this.filteredDesignation = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterDesignation(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Designation List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }

  private _filterDesignation(value) {

    let filterValue;
    if (value && value.DesignationId) {
      filterValue = value.DesignationId.toLowerCase();
      return this.designationsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.designationsList;
    }
  }

  getPercentageList(): void {
    this._masterDataService.GetAllocationPercentages().subscribe((percentageResponse: GenericType[]) => {
      percentageResponse.forEach((percentageResponse: GenericType) => {
        if (percentageResponse.Name != "0")
          this.percentageList.push({ label: percentageResponse.Name, value: percentageResponse.Id });
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Percentage List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }
  getTechnologyList(): void {

    this._masterDataService.GetPractiseAreas().subscribe((clientResponse: GenericType[]) => {
      clientResponse.forEach((clientResponse: GenericType) => {

        this.technologyList.push({ label: clientResponse.Name, value: clientResponse.Id })
      });
      this.filteredTechnology = this.resourceReportForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterTechnology(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Technology List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };
  }

  private _filterTechnology(value) {

    let filterValue;
    if (value && value.PracticeAreaId) {
      filterValue = value.PracticeAreaId.toLowerCase();
      return this.technologyList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.technologyList;
    }
  }

  checkBillable(event) {

    if (event.value == -1) {
      this.onAllBillableChange(event);
    }
    else
      this.onBillableChange(event);
  }


  onBillableChange(event: any) {
    let billable = event.value;
    this.allBillable = 0;
    this.reportsFilterData.utilizationReportFilterData.IsBillable = billable;
  }
  onAllBillableChange(event: any) {
    this.reportsFilterData.utilizationReportFilterData.IsBillable = -1;
    this.allBillable = -1;
  }


  checkCritical(event) {
    if (event.value == -1) {
      this.onAllCriticalChange(event);
    }
    else
      this.onCriticalChange(event);
  }

  onCriticalChange(event: any) {
    let critical = event.value;
    this.allCritical = 0;
    this.reportsFilterData.utilizationReportFilterData.IsCritical = critical;
  }

  onAllCriticalChange(event: any) {
    this.reportsFilterData.utilizationReportFilterData.IsCritical = -1;
    this.allCritical = -1;
  }

  onsearchByFilter(): void {
    this.step = 1;
    this.afterSearchFilter = true;
    this.filterDisplay = false;
    this.searchByFilter(false);
  }

  expandCollapsFilter() {
    this.expandFilter = this.expandFilter;
  }

  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl == 'EmployeeId') {
      this.selectedEmployeeId = item.value;
    }
    else if (frmCntrl == 'PracticeAreaId') {
      this.selectedPracticeAreaId = item.value;
    }
    else if (frmCntrl == 'ProjectId') {
      this.selectedProjectId = item.value;
    }
    else if (frmCntrl == 'ProgramManagerId') {
      this.selectedProgramManagerId = item.value;
    }
    else if (frmCntrl == 'ClientId') {
      this.selectedClientId = item.value;
    }
    else if (frmCntrl == 'ExperienceId') {
      this.selectedExperienceId = item.value;
    }
    else if (frmCntrl == 'GradeId') {
      this.selectedGradeId = item.value;
    }
    else if (frmCntrl == 'DesignationId') {
      this.selectedDesignationId = item.value;
    }


  }

  searchByFilter(isExportRequiered: boolean): void {

    this.spinner.show();
    if (!(this.resourceReportForm.value.EmployeeId == null))
      this.reportsFilterData.utilizationReportFilterData.EmployeeId = this.selectedEmployeeId;
    if (!(this.resourceReportForm.value.PracticeAreaId == null))
      this.reportsFilterData.utilizationReportFilterData.PracticeAreaId = this.selectedPracticeAreaId;
    if (!(this.resourceReportForm.value.ProjectId == null))
      this.reportsFilterData.utilizationReportFilterData.ProjectId = this.selectedProjectId;
    if (!(this.resourceReportForm.value.ProgramManagerId == null))
      this.reportsFilterData.utilizationReportFilterData.ProgramManagerId = this.selectedProgramManagerId;
    if (!(this.resourceReportForm.value.ClientId == null))
      this.reportsFilterData.utilizationReportFilterData.ClientId = this.selectedClientId;
    if (!(this.resourceReportForm.value.ExperienceId == null))
      this.reportsFilterData.utilizationReportFilterData.ExperienceId = this.selectedExperienceId;
    if (!(this.resourceReportForm.value.GradeId == null))
      this.reportsFilterData.utilizationReportFilterData.GradeId = this.selectedGradeId;
    if (!(this.resourceReportForm.value.DesignationId == null))
      this.reportsFilterData.utilizationReportFilterData.DesignationId = this.selectedDesignationId;
    if (!(this.resourceReportForm.value.IsBillable == null))
      this.reportsFilterData.utilizationReportFilterData.IsBillable = this.resourceReportForm.value.IsBillable;
    if (!(this.resourceReportForm.value.IsCritical == null))
      this.reportsFilterData.utilizationReportFilterData.IsCritical = this.resourceReportForm.value.IsCritical;
    this.getsliderValue();
    this.reportsFilterData.utilizationReportFilterData.isExportToExcel = isExportRequiered;

    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.IsBillable == 0 && this.allBillable == -1)
      this.reportsFilterData.utilizationReportFilterData.IsBillable = this.allBillable;
    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.IsCritical == 0 && this.allCritical == -1)
      this.reportsFilterData.utilizationReportFilterData.IsCritical = this.allCritical;
    if (this.reportsFilterData.utilizationReportFilterData && this.reportsFilterData.utilizationReportFilterData.ExperienceId && this.reportsFilterData.utilizationReportFilterData.ExperienceId != 0)
      this.reportsFilterData.utilizationReportFilterData.ExperienceRange = this.filterExperience(this.reportsFilterData.utilizationReportFilterData.ExperienceId);
    this._resourceReportService.ResourceReportByFilters(this.reportsFilterData.utilizationReportFilterData).subscribe((resourceReportResponse: ReportsData[]) => {
      this.spinner.hide();
      this.associateUtilizationReportList = resourceReportResponse;
      console.log(this.associateUtilizationReportList)
      this.associateUtilizationReportList.forEach((ele: ReportsData) => {
        ele.Experience = Number(ele.Experience).toFixed(2);
        ele.JoinDate = moment(ele.JoinDate).format("YYYY-MM-DD");
        ele.FutureProjectTentativeDate = ele.FutureProjectTentativeDate && moment(ele.FutureProjectTentativeDate).format("YYYY-MM-DD");
        ele.ExperienceExcludingCareerBreak = Number(ele.ExperienceExcludingCareerBreak).toFixed(2);
        ele.IsBillableForExcel = (ele.IsBillable == true) ? 'Yes' : 'No';
        ele.IsCriticalForExcel = (ele.IsCritical == true) ? 'Yes' : 'No';
        ele.IsResignedForExcel = (ele.IsResigned == true) ? 'Yes': null;
        ele.IsLongLeaveForExcel =(ele.IsLongLeave == true) ? 'Yes': null;
      });

      this.dataSource = new MatTableDataSource<ReportsData>(this.associateUtilizationReportList);
      this.totalRecordsCount = this.associateUtilizationReportList.length;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      
    },
      (error: any) => {
        this.spinner.hide();
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Resource Report List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      });
    this.reset();
  }


  getsliderValue() {

    if (this.utilizationPercentageFromUI > 0) {
      let res: any = this.percentageList.find(Percentage => Percentage.label == this.utilizationPercentageFromUI.toString());
      this.reportsFilterData.utilizationReportFilterData.AllocationPercentageId = res.value;
    }

  }

  filterExperience(ExperienceId: number): string {
    let experienceRange: string = '';
    if (this.expList.length > 0 && ExperienceId) {
      experienceRange = this.expList[ExperienceId - 1].Name;
    }
    return experienceRange;
  }

  clear() {

    this.formGroupDirective.resetForm();
    this.createForm;
    this.resetFilter;
    // this.defaultSelectedCols();

    this.resourceReportForm.controls["columnselect"].setValue(this.selectedColumns);
    this.resourceReportForm.controls["IsBillable"].setValue(-1);
    this.resourceReportForm.controls["IsCritical"].setValue(-1);

    this.associateUtilizationReportList = [];

    this.dataSource = new MatTableDataSource(this.associateUtilizationReportList);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.totalRecordsCount = this.associateUtilizationReportList.length;

    this.displaycols = this.selectedColumns.map(col => col.header);
    this.displaycolsfields = this.selectedColumns.map(col => col.field);
  }

  exportAsXLSX(){
    // const columnsForExcel = this.dataSource.data.map(x => ({
    //   'ID': x.EmployeeCode,
    //   'Name': x.EmployeeName,
    //   'Designation': x.DesignationName,
    //   'Grade': x.GradeName,
    //   'Experience': x.Experience,
    //   'Technology': x.Technology,
    //   'SG JoinDate': x.JoinDate,
    //   'Project': x.ProjectName,
    //   'Client': x.ClientName,
    //   'Billable': x.IsBillableForExcel,
    //   'Critical': x.IsCriticalForExcel,
    //   '(%) Utilization':x.Allocationpercentage,
    //   'Lead': x.LeadName,
    //   'Reporting Manager': x.ReportingManagerName,
    //   'Skill': x.SkillCode,
    //   'Program Manager': x.ProgramManagerName,
    //   'Resigned': x.IsResignedForExcel,
    //   'Resigned Date': x.ResignationDate,
    //   'Last Working Date': x.LastWorkingDate,
    //   'Long Leave': x.IsLongLeaveForExcel,
    //   'Start Date': x.LongLeaveStartDate,
    //   'Tentative Join Date': x.TentativeJoinDate
    // }));
    let columnsForExcel=[]
    let columnsForEachRow={}
    for(let i=0;i<this.dataSource.data.length;i++){
      var eachRowInDataSource = this.dataSource.data[i]
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
    this.fileExporterService.exportToExcel(columnsForExcel,'Resource Report')
  }
  
}
