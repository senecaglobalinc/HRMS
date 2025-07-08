import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, FormGroupDirective } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GradesService } from '../../services/grades.service';
import { RoleTypeService } from '../../services/role-type.service';
import { Roletype } from '../../models/roletype.model';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { RoleService } from '../../services/role.service';
import { Department } from '../../models/department.model';
import { MasterDataService } from "src/app/core/services/masterdata.service";
import { GenericType } from '../../../master-layout/models/dropdowntype.model';

@Component({
  selector: 'app-role-type',
  templateUrl: './role-type.component.html',
  styleUrls: ['./role-type.component.scss']
})
export class RoleTypeComponent implements OnInit {
  formSubmitted = false;
  buttonTitle: string = 'SAVE';
  roleTypeForm: FormGroup;
  gradeList: any; 
  departmentId :number = 0;
  roleTypeId : number = 0;
  roleTypeList: any;
  financialYearList= [];
  departmentList: any;
  dataSource = new MatTableDataSource<Roletype>();
  editMode: boolean = false;
  _selectedFinancialYearId: string;
  public financialYearId: number = 0;
  themeConfigInput = themeconfig.formfieldappearances;
  displayedColumns: string[] = ['Department','RoleTypeName', 'RoleTypeDescription','Grade', 'Edit'];

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private fb: FormBuilder,
    private _roleTypeService: RoleTypeService,
    private _gradeService: GradesService,
    private _rolesService: RoleService,
    private _masterDataService: MasterDataService,
    private _snackBar: MatSnackBar, public navService: NavService) {
    this.GetFinancialYears();    
    this.GetDepartments();
    this.GetGrades();   
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  ngOnInit(): void {
    this.roleTypeForm = new FormGroup({
      financialYearId: new FormControl('', [Validators.required]),      
      DepartmentId: new FormControl('', [Validators.required]),
      RoleTypeId: new FormControl('', [Validators.required]),
      GradeId: new FormControl('', [Validators.required]),
      GradeRoleTypeId: new FormControl(0),     
      IsActive: new FormControl(true),
      GradeIds: new FormControl('')
    })
  }

  onchange(event) {   
    this.financialYearId = event.value;
    this.GetGradeRoleTypes();   
    this.GetRoleTypes();      
  }

  onDepartmentChange(event)
  {
    this.departmentId = event.value;
    this.GetRoleTypes();   
  }

  GetGrades() {
    this._gradeService.getGradesDetails();
    this._gradeService.GradesData.subscribe((data) => {
      this.gradeList = data;
    });
  }
  GetGradeRoleTypes() {
    this._roleTypeService.GetGradeRoleTypes(this.financialYearId, this.departmentId, this.roleTypeId).subscribe((data) => {
      this.roleTypeList = data;
      this.dataSource = new MatTableDataSource(this.roleTypeList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (data, attribute) => data[attribute];
    });
  }

  GetRoleTypes(){
    this._roleTypeService.GetRoleTypesForDropdown(this.financialYearId, this.departmentId).subscribe((res: GenericType[]) => {
      this.roleTypeList = [];
      res.forEach(element => {
        this.roleTypeList.push({ label: element.Name, value: element.Id });
      });
    }, (error) => {
      this._snackBar.open(error.error.text, 'x', {
        duration: 3000, 
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });

  }
  

  GetDepartments(){
    this._rolesService.getDepartmentList().subscribe((res: Department[]) => {
      this.departmentList = [];
      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });
    }, (error) => {
      this._snackBar.open(error.error.text, 'x', {
        duration: 3000, 
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });
  }

  GetFinancialYears(): void {
    this._masterDataService.GetFinancialYears().subscribe(
      (yearsdata: any[]) => {
        yearsdata.forEach((e: any) => {
          this.financialYearList.push({
            label: e.FinancialYearName,
            value: e.Id
          });
          // if (e.Id == this._selectedFinancialYearId)
          //   this.financialYearName = e.FinancialYearName;
        });        
        this.financialYearId = this.financialYearList[0].value;
        this.roleTypeForm.get("financialYearId").setValue(this.financialYearId);
        this.roleTypeForm.controls["financialYearId"].enable();
        this.GetGradeRoleTypes();
        this.GetRoleTypes();  
      },
      error => {
        this._snackBar.open("Failed to get Financial Year List", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
        // this.messageService.add({
        //   severity: "error",
        //   summary: "Error Message",
        //   detail: "Failed to get Financial Year List"
        // });
      }
    );
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.roleTypeList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  clear() {
    this.editMode = false;
    this.roleTypeForm.get('IsActive').patchValue(true);
    this.roleTypeForm.reset({
      IsActive: this.roleTypeForm.get('IsActive').value, 
      financialYearId: this.roleTypeForm.get('financialYearId').value, 
      GradeRoleTypeId: 0
    })

    setTimeout(() => this.formGroupDirective.resetForm({
      IsActive: this.roleTypeForm.get('IsActive').value, 
      financialYearId: this.roleTypeForm.get('financialYearId').value, 
      GradeRoleTypeId: 0,
    }), 0);
   //
   // this.roleTypeForm.controls.RoleTypeName.enable();
    this.buttonTitle = 'SAVE';
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.roleTypeForm.valid) {
      let gradeIds = "";
     let grade_data :any ;   
    this.roleTypeForm.get('GradeIds').setValue(this.roleTypeForm.value.GradeId.join(','));
      if (this.buttonTitle == 'SAVE') {        
     //   this.roleTypeForm.get('RoleTypeId').setValue(0);     
        this._roleTypeService.CreateRoleTypes(this.roleTypeForm.value).subscribe(response => {
          this.GetGradeRoleTypes();
          this.clear();
          this._snackBar.open('Role Type added successfully.', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        }, (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        });
      }
      else {
        this._roleTypeService.CreateRoleTypes(this.roleTypeForm.value).subscribe(response => {
          this.GetGradeRoleTypes();
          this.clear();
          this._snackBar.open('Role Type updated successfully.', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        }, (error) => {
          this._snackBar.open('Not updated successfully', 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        });
      }
    }
  }

  Delete(id) {
    this._roleTypeService.DeleteRoleTypes(id).subscribe(response => {
      this.GetGradeRoleTypes();
    }, error => {

    });
  }

  onEdit(data: Roletype) {
    this.departmentId = data.DepartmentId;
    this.GetRoleTypes();
    this.roleTypeForm.patchValue(data);
    this.buttonTitle = 'UPDATE';
    this.editMode = true;
   // this.roleTypeForm.controls.RoleTypeName.disable();
  }
}
