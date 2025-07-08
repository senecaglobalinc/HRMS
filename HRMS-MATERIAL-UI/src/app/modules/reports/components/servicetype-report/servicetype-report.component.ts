import { filter } from 'rxjs/operators';
import { ProjectDetails } from './../../models/resourcereportbyproject.model';
import { ResourceReportByProjectComponent } from './../resource-report-by-project/resource-report-by-project.component';
import { ProjectResponse } from './../../models/projects-report.model';
import { ProjectTypeData } from './../../../admin/models/projecttype.model';
import { ProjectTypeService } from './../../../admin/services/project-type.service';
import { Component, OnInit, ViewChild, TemplateRef, ViewChildren, QueryList,ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { themeconfig } from '../../../../../themeconfig';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig} from '@angular/material/dialog';
import { MatOption } from '@angular/material/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import {ServiceTypeReport, ServiceTypeReportEmp, ServiceTypeDropDown} from '../../models/service-typereport.model';
import {ServiceTypeReportService} from '../../services/servicetype-report.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ResourceReportService } from '../../services/resource-report.service';
import { AllocationDetails } from '../../models/resourcereportbyproject.model';
import * as moment from 'moment';

@Component({
  selector: 'app-servicetype-report',
  templateUrl: './servicetype-report.component.html',
  styleUrls: ['./servicetype-report.component.scss']
})
export class ServicetypeReportComponent implements OnInit, AfterViewInit {

  themeAppearence = themeconfig.formfieldappearances;
  @ViewChild('AssociateDialog') associateDialog: TemplateRef<any>;
  @ViewChild('counttable', { static: false }) counttablepaginator: MatPaginator;
  // @ViewChild('emptable', {static: false})
  // set paginator(value: MatPaginator) {
  //   if (this.selectedEmpColumns){
  //     this.selectedEmpColumns.paginator = value;
  //   }
  // }
  //@ViewChild('counttable', { static: false }) counttablesort: MatSort;
   @ViewChild('emptable', { static: false }) emptablepaginator: MatPaginator;
  @ViewChild('emptable', { static: false }) emptablesort: MatSort;
  @ViewChild('prjtable', { static: false }) projtablepaginator: MatPaginator;
  @ViewChild('prjtable', { static: false }) projtablesort: MatSort;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  projectType: ProjectTypeData[];
  projectRes: ProjectResponse[];
  serviceType: ServiceTypeDropDown[] ;
  count: ServiceTypeReport[];
  emp: ServiceTypeReportEmp[];
  projectname:string;
  displaycols = ['ServiceName', 'ServiceDescription', 'ProjectCount'];
  displayEmpcols = ['ProjectCode', 'ProjectName', 'Actual StartDate', 'Status' ]
  selectedColumns: MatTableDataSource<ServiceTypeReport> ; 
  ProjectId: number;
  selectedEmpColumns: MatTableDataSource<ServiceTypeReportEmp>; 
  selectedProjColumns: MatTableDataSource<ProjectResponse>;
  previous:string;
  step=0;
  projid:number;
  filter = null;
  @ViewChild('allSelected') private allSelected: MatOption;
  searchUserForm: FormGroup;
  
  private AssociateDialogRef: MatDialogRef<TemplateRef<any>>
  constructor(private _resourceReportsService: ResourceReportService,private cdr: ChangeDetectorRef,public dialog: MatDialog, private fb: FormBuilder, private service: ServiceTypeReportService, 
    private spinner: NgxSpinnerService, private _snackBar: MatSnackBar, private _router: Router) { }

  ngOnInit(): void {
    //this.spinner.show();
    this.projectname = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).ProjectName
    this.previous = this._resourceReportsService.getPreviousUrl();
    //this.selectedColumns = new MatTableDataSource([]);
   // this.selectedEmpColumns = new MatTableDataSource([]);
    this.count = [];
    this.emp = [];
    // this.selectedColumns.paginator = this.counttablepaginator;
    // this.selectedEmpColumns.paginator = this.emptablepaginator;
    // this.selectedColumns.sort = this.counttablesort;
    // this.selectedEmpColumns.sort = this.emptablesort;
    this.searchUserForm = this.fb.group({
      ServiceType: new FormControl()
    });
    this.service.GetProjectTypeDropdown().toPromise().then((res: ProjectTypeData[])=>{
          this.projectType = res;
          this.spinner.hide();
    }).catch(error=>{
      this.spinner.hide();
      this._snackBar.open('Error Occured While Fetching ServiceTypes', 'x', {
        duration: 2000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
   });;
  }
  EmployeePop(serviceType: ServiceTypeReport)
  {
   
    if(serviceType.ResourceCount != 0)
    {

    this.spinner.show();
    
    this.GetServiceTypeProj(serviceType.ServiceTypeId);
    console.log(this.previous);
    }
    else{
      this._snackBar.open('There are No Projects To display', 'x', {
        duration: 2000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }    
  }
  ResourceReportByProject(projectid: ProjectResponse){
    this._resourceReportsService.GetResourceReportByProjectId(projectid.ProjectId);
    setTimeout(() => {
      this.closeDialog();
      this._router.navigate(['/reports/resourcereportsbyproject'+ '/' + projectid.ProjectId]);
    }, 500);
  }
  GetServiceTypeCount(filter: string)
  {
    this.service.GetServiceTypeReportCount(filter).toPromise().then((res: ServiceTypeReport[])=>{
      
      this.selectedColumns = new MatTableDataSource(res);
    
      
      this.selectedColumns.paginator = this.counttablepaginator;
      this.selectedColumns.sort = this.sort;
      this.count = res;
      this.spinner.hide();
      console.log(filter);
      
}).catch(error=>{
  this.spinner.hide();
  this._snackBar.open('Error Occured While Fetching ServiceType Count', 'x', {
    duration: 2000,
    horizontalPosition: 'right',
    verticalPosition: 'top',
  });
});;
  }
  ngAfterViewInit ()
  {
// this.selectedColumns.paginator = this.counttablepaginator;
//     this.selectedEmpColumns.paginator = this.emptablepaginator;
//     this.selectedColumns.sort = this.sort;
//     this.selectedEmpColumns.sort = this.sort;
  }
  GetServiceTypeProj(serviceTypeId: number)
  {
    this.service.GetServiceTypeReportProject(serviceTypeId).toPromise().then((res: ProjectResponse[]) => {
        res.forEach(e => { if (e.ActualStartDate!=null) {e.ActualStartDate= moment(e.ActualStartDate).format("MM/DD/YYYY");}})
      this.selectedProjColumns = new MatTableDataSource(res);
     // console.log(this.emptablepaginator)
      this.cdr.detectChanges();
      this.selectedProjColumns.paginator = this.projtablepaginator;
      this.selectedProjColumns.sort = this.sort;
      this.projectRes = res;
      this.spinner.hide();
      this.openDialog();
    }).catch(error => {
      this.spinner.hide();
      this._snackBar.open('Error Occured While Fetching ServiceType Employees', 'x', {
        duration: 2000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });;
  }
  Clear()
  {
    this.searchUserForm.reset();
  }
  tosslePerOne(all){ 
    
    if (this.allSelected.selected) {  
     this.allSelected.deselect();
     return false;
 }
   if(this.searchUserForm.controls.ServiceType.value.length==this.projectType.length)
     this.allSelected.select();
 
 }
   toggleAllSelection() {
    
     if (this.allSelected.selected) {
       this.searchUserForm.controls.ServiceType
         .patchValue([...this.projectType.map(item => item.ProjectTypeId), 0]);
     } else {
       this.searchUserForm.controls.ServiceType.patchValue([]);
     }
   }
  openDialog(): void {
    const dialogConfig = new MatDialogConfig();
    this.AssociateDialogRef = this.dialog.open(this.associateDialog);
    this.AssociateDialogRef.updateSize('80%', '80%');
    this.AssociateDialogRef.afterClosed().subscribe(result => {console.log(`Dialog result: ${result}`);
    });
  }
  closeDialog() {
    this.AssociateDialogRef.close();
  }
  Apply()
  {
    this.step=1;
    
   this.spinner.show();
    this.filter=null;
    var i=1;
    if (this.searchUserForm.controls.ServiceType.value) {


      for (var val of this.searchUserForm.controls.ServiceType.value) {
        if (val == 0) {
          this.filter = "-1";
          break;
        }

        if (i <= this.searchUserForm.controls.ServiceType.value.length - 1) {
          this.filter = this.filter + val + ',';
        }
        else {
          this.filter = this.filter + val;
          break;
        }
        i++;
      }
    }
   if(this.filter == null)
   {
     this.filter = "-1";
   }
   this.GetServiceTypeCount(this.filter);
   
  }
  
}
