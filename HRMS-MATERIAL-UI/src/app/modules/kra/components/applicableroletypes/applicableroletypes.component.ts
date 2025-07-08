import { AfterViewInit, Component, ViewChild, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import {SelectionModel} from '@angular/cdk/collections';
//import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { DepartmentDetails } from '../../../master-layout/models/role.model';
import {Grade}   from "../../../admin/models/grade.model";  
import { Roletype } from '../../../kra/models/roletype.model';
import { element } from 'protractor';
import { FinancialYear, SelectedKRAParameters } from '../../../master-layout/models/kra.model';
import { RoleTypeService } from '../../../admin/services/role-type.service'
import { ApplicableRoleTypeService } from "../../services/applicableroletype.service";
import { ApplicableRoleType , ApplicableRoleTypeData} from '../../models/kraapplicableRoleType.model';
import * as servicePath from '../../../../core/service-paths';


import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';


interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-applicableroletypes',
  templateUrl: './applicableroletypes.component.html',
  styleUrls: ['./applicableroletypes.component.scss'],
  
})
export class ApplicableroletypesComponent implements OnInit {
  selectedParameters : SelectedKRAParameters;
  formSubmitted = false;
  roletypedata: any;
  selection = new SelectionModel<any>(true, []);
  themeappeareance = themeconfig.formfieldappearances;
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] =[];
  public roleTypeList: SelectItem[] =[];
  public financialYearList: SelectItem[] = [];
  approleTypeForm: FormGroup;
  btnLabel = "Save";
  applicableRoleTypeList : ApplicableRoleTypeData[];
  list : any;
  selectedApplicableRoleType : ApplicableRoleTypeData;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  dataSource = new MatTableDataSource<ApplicableRoleTypeData>();
  PageDropDown: number[] = [];
 
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  cols = [
    { field: 'FinancialYearName', header: 'Finacial Year' },
    { field: 'DepartmentDescription', header: 'Department' },
    { field: 'GradeName', header: 'Grade' },
    { field: 'RoleTypeName', header: 'Role Type' },
  ];
  displayedColumns: string[] = [
    'FinancialYearName',
    'DepartmentDescription',
    'GradeName',
    'RoleTypeName',
    'Delete',
  ];
  columnsToDisplay: string[] = this.displayedColumns.slice();

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(
      this.displayedColumns,
      event.previousIndex,
      event.currentIndex
    );
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.dataSource.data);
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  constructor( private _snackBar: MatSnackBar,
   public navService: NavService,
    private _masterDataService: MasterDataService,    
    private _roleTypeService: RoleTypeService, 
    private _applicableRoleTypeService: ApplicableRoleTypeService,  
    ) {      
    this.selectedParameters = new SelectedKRAParameters();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;    
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
     }

  ngOnInit() {
     this.approleTypeForm = new FormGroup({
      FinancialYearId: new FormControl(null, [Validators.required]),
      DepartmentId: new FormControl(null, [Validators.required]),
      GradeRoleTypeId: new FormControl(null),    
    });  
    this.getFinancialYears();
    this.getDepartments();  
    this.GetRoleTypes();     
    this.Reset(); 
  }
 
   getFinancialYears(): void {   
     this._masterDataService.GetFinancialYears().subscribe((yearsdata: any[]) => {
      this.financialYearList = [];
      this.financialYearList.push({ label: 'Select Financial Year', value: null });
      yearsdata.forEach((e: any) => {
        this.financialYearList.push({ label: e.FinancialYearName, value: e.Id });
      });
    }, error => {
      this._snackBar.open("Failed to get Financial Year List" + error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      //this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Financial Year List' });
    }
    );
  }

   getDepartments() : void {
    this._masterDataService.GetDepartmentsMasterData().subscribe(
    (res: DepartmentDetails[]) => {
      this.departmentList =[];
      this.departmentList.push({label : "Select Department", value :null})
      res.forEach((element :DepartmentDetails) => {
        this.departmentList.push({
          label : element.Description,
          value : element.DepartmentId
        });
      });
    },
    (error: any) => {     
      this._snackBar.open("Failed to get Departments details" + error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
    }
  );
}

  GetRoleTypes() {
    this._roleTypeService.GetActiveRoleTypes().subscribe((data) => {     
      this.roletypedata = data;
    });
  }   
 getApplicableRoleTypeList(event): void { 
    this.showApplicableRoleTypes(event.value);
   }

   saveApplicableRoleType(): void {
    //  this.formSubmitted = true;     
    this.approleTypeForm.get('FinancialYearId').setValidators([Validators.required])
    this.approleTypeForm.get('FinancialYearId').updateValueAndValidity();
    this.approleTypeForm.get('DepartmentId').setValidators([Validators.required])
    this.approleTypeForm.get('DepartmentId').updateValueAndValidity();
    this.approleTypeForm.get('GradeRoleTypeId').setValidators([Validators.required])
    this.approleTypeForm.get('GradeRoleTypeId').updateValueAndValidity();
     if(this.approleTypeForm.value.FinancialYearId==null ||
     this.approleTypeForm.value.DepartmentId==null||
     this.approleTypeForm.value.GradeRoleTypeId==null) return;
    if (this.approleTypeForm.valid) {     
      if (this._applicableRoleTypeService.editMode == false)
      {
        this._applicableRoleTypeService.addKRAApplicableRoleType(
        this.approleTypeForm.value).subscribe(res => {
        this.formSubmitted = false;
        this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId); 
        this.Clear();          
        if (res == 1) 
         this._snackBar.open('ApplicableRole Type added.', '', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        //this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ApplicableRole Type added' });      
        else if (res == -1)
              this._snackBar.open('ApplicableRole Type already exist.', '', {
              duration: 1000,
             // panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          //this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ApplicableRole Type already exist' });
          else if (res == 0)
           this._snackBar.open('Draft status not found.', '', {
              duration: 1000,
              //panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          //this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Draft status not found' });
        else 
         this._snackBar.open('Unable to add ApplicableRole Type.', '', {
              duration: 1000,
             // panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        //this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add ApplicableRole Type' });      
        },
      error => {
        this._snackBar.open(error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        // this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
      }
      else
      {
        var creatingObj = new ApplicableRoleType();
        creatingObj.applicableRoleTypeId=this._applicableRoleTypeService.editObj.value.ApplicableRoleTypeId;
        creatingObj.financialYearId=this.approleTypeForm.value.FinancialYearId;
        creatingObj.departmentId=this.approleTypeForm.value.DepartmentId;
        creatingObj.gradeRoleTypeId =this.approleTypeForm.value.GradeRoleTypeId;
        this._applicableRoleTypeService.editApplicableRoleType(creatingObj).subscribe(res => { 
        if (res == 1) 
        this._snackBar.open('ApplicableRole Type updated.', '', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        //this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ApplicableRole Type updated' });         
      else if (res == -1)
      this._snackBar.open('ApplicableRole Type already exist.', '', {
              duration: 1000,
             // panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        //this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ApplicableRole Type already exist' });
        else if (res == 0)
        this._snackBar.open('Draft status not found.', '', {
              duration: 1000,
              //panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
        //this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Draft status not found' });
      else 
      {
         this._snackBar.open('Unable to add ApplicableRole Type.', '', {
              duration: 1000,
             // panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
      }
      //this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to update ApplicableRole Type' });
       this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId);
        this.Clear(); 
    },
      error => {
        this._snackBar.open(error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        //this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
      }
  }
  }
    showApplicableRoleTypes(financialYearId: number) 
    { 
    this._applicableRoleTypeService.getApplicableRoleTypes(financialYearId).subscribe(
      (data: ApplicableRoleTypeData[]) => {
        this.applicableRoleTypeList = data;
        this.dataSource = new MatTableDataSource(this.applicableRoleTypeList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
        (error)=>{          
      this._snackBar.open('Failed to get ApplicableRole Types' + error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
    });       
    }

  Reset() {       
    //this.approleTypeForm.reset(); 
     this.approleTypeForm.patchValue({FinancialYearId:null, DepartmentId: null, GradeRoleTypeId:null});
    this.approleTypeForm.controls['FinancialYearId'].setErrors(null);
    this.approleTypeForm.controls['DepartmentId'].setErrors(null);
    this.approleTypeForm.controls['GradeRoleTypeId'].setErrors(null);
    this.applicableRoleTypeList =null;
    this.dataSource = null;
  }

  //  setEditObj(editObj: ApplicableRoleTypeData): void {
  //    this._applicableRoleTypeService.editMode = true;
  //     this._applicableRoleTypeService.editObj.next(editObj);
  // }
   deleteApplicableRoleType(rowData): void {
    this._applicableRoleTypeService.DeleteApplicableRoleType
      (rowData.ApplicableRoleTypeId).subscribe(res => {
        //this.Reset();
        this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId);
        // this.messageService.add({
        //   severity: 'success',
        //   summary: 'Success Message',
        //   detail: 'Deleted Successfully'
        // });
         this._snackBar.open('Deleted Successfully.', '', {
              duration: 1000,
              panelClass:['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
      },
      (error) => {
        this._snackBar.open(error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      });
  }

  Clear() {
    this.approleTypeForm.patchValue({ DepartmentId: null, GradeRoleTypeId: null });
    this.approleTypeForm.controls['DepartmentId'].setErrors(null);
    this.approleTypeForm.controls['GradeRoleTypeId'].setErrors(null);
  }

}


