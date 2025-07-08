import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormBuilder, FormGroup, FormGroupDirective, FormControl } from '@angular/forms';
import { PracticeAreaService } from '../../services/practice-area.service';
import { PracticeArea } from '../../models/practicearea.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ErrorStateMatcher } from '@angular/material/core';
import { themeconfig } from 'src/themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import * as servicePath from '../../../../core/service-paths';
import { map, startWith } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { EmployeeStatusService } from '../../services/employeestatus.service';

@Component({
  selector: 'app-practice-area',
  templateUrl: './practice-area.component.html',
  styleUrls: ['./practice-area.component.scss'],
  providers: [EmployeeStatusService],
})
export class PracticeAreaComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  btnLabel: string = "";
  isEdit: boolean;
  addPracticeArea: FormGroup;
  formSubmitted = false;
  practiceAreaList: PracticeArea[];
  selectedRow: PracticeArea;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  usersList = [];
  filteredOptionsName: Observable<any>;
  controlName: string;

  dataSource: MatTableDataSource<PracticeArea>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['PracticeAreaCode', 'PracticeAreaDescription','PracticeAreaHeadName', 'Edit'];

  constructor(private _practiceAreaService: PracticeAreaService,
    private fb: FormBuilder, private _snackBar: MatSnackBar,
    private service: EmployeeStatusService,
    public navService: NavService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;


    this._practiceAreaService.practiceAreaList.subscribe((data) => {
      this.practiceAreaList = data;
      this.dataSource = new MatTableDataSource(this.practiceAreaList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.practiceAreaList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }
  ngOnInit() {
    this.addPracticeArea = this.fb.group({
      PracticeAreaDescription: ['', [Validators.required, Validators.maxLength(100)]],
      PracticeAreaCode: ['', [Validators.required, Validators.maxLength(100)]],
      PracticeAreaHeadId: ['', [Validators.required]]

    });

    this._practiceAreaService.practiceAreaEdit.subscribe(data => {
      if (this._practiceAreaService.editMode == true) {
        this.isEdit = this._practiceAreaService.editMode;
        this.addPracticeArea.patchValue({
          'PracticeAreaDescription': data.PracticeAreaDescription,
          'PracticeAreaCode': data.PracticeAreaCode,
          'PracticeAreaHeadId':{
            label: data.PracticeAreaHeadName,
            value: data.PracticeAreaHeadId,
          }
        });
        this.btnLabel = "Update";
      }
    });
    this._practiceAreaService.practiceAreaList.subscribe((data) => {
      this.practiceAreaList = data;
    });
    this.getPracticeAreas();
    this.getUsersList();

    this.btnLabel = "Save";
    this.Reset();
  }

  addpracticeAreas(): void {
    this.formSubmitted = true;
    var practiceArea = new PracticeArea();
    practiceArea.PracticeAreaCode = this.addPracticeArea.value.PracticeAreaCode;
    practiceArea.PracticeAreaDescription = this.addPracticeArea.value.PracticeAreaDescription;
    practiceArea.PracticeAreaCode.trim();
    practiceArea.PracticeAreaDescription.trim();
    practiceArea.PracticeAreaHeadId = this.addPracticeArea.value.PracticeAreaHeadId?.value;
    if (this._practiceAreaService.editMode == true) {
      practiceArea.PracticeAreaId = this._practiceAreaService.practiceAreaEdit.value.PracticeAreaId;
      practiceArea.IsActive = this._practiceAreaService.practiceAreaEdit.value.IsActive;
    }
    if (this.addPracticeArea.valid == true) {
      this._practiceAreaService.createPracticeAreas(practiceArea).subscribe((res: number) => {
        if (res != null) {
          this._practiceAreaService.getPracticeAreas();
          if (this._practiceAreaService.editMode == false) {
            this._snackBar.open('Practice area record added successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
          }

          else {
            this._snackBar.open('Practice area record updated successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
          }

          this.Reset();
        }
      },
        error => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() =>
            this.formGroupDirective.resetForm(), 0)


        });

    }

  }
  getPracticeAreas(): void {
    this._practiceAreaService.getPracticeAreas();
  }

  editPracticeArea(practiceAreaEdit): void {
    this._practiceAreaService.editMode = true;
    this._practiceAreaService.practiceAreaEdit.next(practiceAreaEdit);
    this.addPracticeArea.patchValue({
      'PracticeAreaDescription': practiceAreaEdit.PracticeAreaDescription,
      'PracticeAreaCode': practiceAreaEdit.PracticeAreaCode,
      'PracticeAreaHeadId':{
        label: practiceAreaEdit.PracticeAreaHeadName,
        value: practiceAreaEdit.PracticeAreaHeadId,
      }
    });
   
  }
  Reset(): void {
    this.formSubmitted = false;
    this.addPracticeArea.reset();
    this._practiceAreaService.editMode = false;
    this.btnLabel = "Save";
    this.isEdit = false;
  }

  getUsersList() {
    this.service.GetAssociateNames().subscribe((res: any[]) => {
      // tslint:disable-next-line:prefer-const
      let dataList: any[] = res;
      this.usersList = [];
      dataList.forEach((e) => {
        this.usersList.push({ label: e.EmpName, value: e.EmpId });
      });
      this.filteredOptionsName = this.addPracticeArea
        .get('PracticeAreaHeadId')
        .valueChanges.pipe(
          startWith(''),
          map((value) => this._filterName(value))
        );
    });
  }

  private _filterName(value): any {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else {
      if (value !== null) {
        filterValue = value.label.toLowerCase();
      } else {
        return this.usersList;
      }
    }

    return this.usersList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }

  clearInput(event : any, fieldName: string){
    if (fieldName == 'PracticeAreaHeadId'){
      event.stopPropagation();
      this.addPracticeArea.get('PracticeAreaHeadId').reset();
    }
  }

  getFormControl(value:any){
    this.controlName = value;
  }
  
  displayFn(user: any) {
    return user ? user.label : '';
  }
}