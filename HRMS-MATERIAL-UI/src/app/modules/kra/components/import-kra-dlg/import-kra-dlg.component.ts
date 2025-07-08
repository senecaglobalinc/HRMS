import { Component, OnInit, ViewChild, Inject } from "@angular/core";
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { KraDlgData } from "../../../../modules/kra/models/kra-dlg-data";
import { FormControl } from "@angular/forms";
import { FormBuilder, FormGroup } from "@angular/forms";
import { SelectionModel } from "@angular/cdk/collections";
import { AddKRAdlgComponent } from "../add-kradlg/add-kradlg.component";
import { ApplicableRoleTypeService } from "../../services/applicableroletype.service";
import { MasterDataService } from "../../../../core/services/masterdata.service"
import { DepartmentDetails } from "../../../master-layout/models/role.model";
import { Grade } from "../../../admin/models/grade.model";
import { Roletype } from "../../../kra/models/roletype.model";
import { element } from "protractor";
import { DefinitionModel } from "../../../kra/models/kradefinition.model"
import {
  FinancialYear,
  SelectedKRAParameters
} from "../../../master-layout/models/kra.model";
import { RoleTypeService } from "../../../admin/services/role-type.service"
import {
  ApplicableRoleType,
  ApplicableRoleTypeData
} from "../../../kra/models/kraapplicableRoleType.model"
import { ActivatedRoute, Router } from "@angular/router";
import { DefineKraService } from "../../../../modules/kra/services/definekra.service";
import { DefineKRAData } from "../../../kra/models/definekra.model";
import { KraDefinitionService } from "../../../kra/services/kradefinition.service";
import { tap } from "rxjs/operators";
import * as servicePath from "../../../../core/service-paths";
import { environment } from "../../../../../environments/environment";

import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from "@angular/material/snack-bar";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from "@angular/material/sort";
import { themeconfig } from "../../../../../themeconfig";
import { NavService } from "../../../master-layout/services/nav.service";

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-import-kra-dlg',
  templateUrl: './import-kra-dlg.component.html',
  styleUrls: ['./import-kra-dlg.component.scss']
})

export class ImportKraDlgComponent {
  leaderRole:boolean=false;
  themeappeareance = themeconfig.formfieldappearances;

  // displayedColumns: string[] = ['select','kraaspect', 'metrics', 'ration'];
  //dataSource = new MatTableDataSource<any>(kradefindata);
  selection = new SelectionModel<any>(true, []);
  financialYearId: number = 0;
  selectedApplicableRoleTypeId: number;
  finYearId: number;
  depId: number;
  gId: number;
  rId: number;
  departmentId: number = 0;
  gradeId: number = 0;
  roleTypeId: number = 0;
  departmentList: SelectItem[] = [];
  gradeList: SelectItem[] = [];
  roleTypeList: SelectItem[] = [];
  financialYearList: SelectItem[] = [];
  importKRAForm: FormGroup; 
  selectedfromyear: number;
  selectedtoyear: number;
    length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  currentyear = new Date().getFullYear();
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20, 30];
  definitionDetailsIds = [];  
  
 list = [];
  dataSource = new MatTableDataSource<DefineKRAData>();

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;
  @ViewChild(MatSort, { static: true })
  sort: MatSort;
  // End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;

  constructor( public dialog: MatDialog,
    private _snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    //private messageService: MessageService,
    private _roleTypeService: RoleTypeService,
    private fb: FormBuilder,
    private _appRoleTypeService: ApplicableRoleTypeService,
    private _router: Router,
    private _kraDefinitionService: KraDefinitionService,
    private _definekraService: DefineKraService,
    public dialogRef: MatDialogRef<ImportKraDlgComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ImportKraDlgComponent) {}

      cols = [
    { field: "kraaspect", header: "ASPECT" },
    { field: "metrics", header: "METRICS" },
    { field: "ration", header: "TARGET" }
  ];
  displayedColumns: string[] = ["select",
    "kraaspect",
    "metrics",
    "ration",   
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

  onNoClick(): void {
    this.dialogRef.close();
  }


ngOnInit() {
    this.importKRAForm = this.fb.group({
          financialYearId: [null],
          departmentId: [null],
          gradeId: [null],
          roleTypeId: [null]
    });  
    this.finYearId = this.data.finYearId;
    this.depId = this.data.depId;
    this.gId = this.data.gId;
    this.rId = this.data.rId;  
    this.getFinancialYears();
    this.getDepartments();
    this.getSelectedApplicableRoleTypeId();
}

  getApplicableGradeRoleTypesByGrade() {
    this.roleTypeList = [];
    this.dataSource = null;  
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    // this._appRoleTypeService
    //   .getApplicableRoleTypeById(
    //     this.financialYearId,
    //     this.departmentId,
    //     null,
    //     this.importKRAForm.value.gradeId
    //   )
    //   .subscribe(
    //     (res: any[]) => {
    //       if (res) {         
    //         res.forEach((element: any) => {
    //           this.roleTypeList.push({
    //             label: element.RoleTypeName,
    //             value: element.RoleTypeId
    //           });
    //           this.roleTypeId = element.RoleTypeId;
    //         });
    //         if (this.roleTypeId > 0) {
    //           this.importKRAForm.get("roleTypeId").setValue(this.roleTypeId);
    //           this.showDefineKRAs("");
    //         }
    //       } 
    //     },
    //     (error: any) => {
    //       this._snackBar.open("Failed to get Role Type details.", "", {
    //         duration: 1000,
    //         horizontalPosition: "right",
    //         verticalPosition: "top"
    //       });
    //     }
    //   );
  }
  getFinancialYears(): void {
    this._masterDataService.GetFinancialYears().subscribe(
      (yearsdata: any[]) => {
        yearsdata.forEach((e: any) => {       
          this.selectedfromyear = parseInt(e.FinancialYearName.split("-")[0].trim());
           if(this.selectedfromyear < this.currentyear)
           {
          this.financialYearList.push({
            label: e.FinancialYearName,
            value: e.Id
          });
           }
          // if (e.Id == this._selectedFinancialYearId)
          //   this.financialYearName = e.FinancialYearName;
        });
        this.importKRAForm.controls["financialYearId"].enable();
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

  getDepartments(): void {
    this._masterDataService.GetDepartmentsMasterData().subscribe(
      (res: DepartmentDetails[]) => {
        this.departmentList = [];
        this.departmentList.push({ label: "Select Department", value: null });
        res.forEach((e: any) => {
          this.departmentList.push({
            label: e.Description,
            value: e.DepartmentId
          });
        });
      this.importKRAForm.controls["departmentId"].enable();
      },
      error => {
        this._snackBar.open("Failed to get Department Details", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
        // this.messageService.add({
        //   severity: "error",
        //   summary: "Error Message",
        //   detail: "Failed to get Department Details"
        // });
      }
    );
  }

  onchange(event) {
    let target = event.source.selected._element.nativeElement;
    this.selectedfromyear = parseInt(target.innerText.split("-")[0].trim());
    this.selectedtoyear = parseInt(target.innerText.split("-")[1].trim());
    this.financialYearId = event.value;
    this.importKRAForm.get("departmentId").setValue(null);
    this.importKRAForm.get("gradeId").setValue(null);
    this.importKRAForm.get("roleTypeId").setValue(null);   
    this.dataSource = null;     
  }
  ondepartmentchange(event) {
    // this.setDefaultValues();
    this.departmentId = event.value;
    this.getApplicableGradeRoleTypes();
  }
  onRoleTypeChange() {
    this.showDefineKRAs("u");
  }

  getApplicableGradeRoleTypes() {   
    this.gradeList = [];
    // this._appRoleTypeService
    //   .getApplicableRoleTypeById(
    //     this.financialYearId,
    //     this.departmentId,
    //     null,
    //     null
    //   )
    //   .subscribe(
    //     (res: any[]) => {
    //       if (res) {         
    //         this.gradeList.push({ label: "Select Grade", value: null });
    //         res.forEach((element: any) => {
    //           this.gradeList.push({
    //             label: element.Grade,
    //             value: element.GradeId
    //           });
    //           this.gradeId = element.GradeId;
    //         });
    //         this.dataSource = null;          
    //         if (this.gradeId > 0) {
    //           this.importKRAForm.get("gradeId").setValue(this.gradeId);
    //           this.getApplicableGradeRoleTypesByGrade();
    //         }
    //       } 
    //     },
    //     (error: any) => {
    //       this._snackBar.open("Failed to get Role Type details.", "", {
    //         duration: 1000,
    //         horizontalPosition: "right",
    //         verticalPosition: "top"
    //       });
    //       // this.messageService.add({
    //       //   severity: 'error',
    //       //   summary: 'Error Message',
    //       //   detail: 'Failed to get Role Type details.'
    //       // });
    //     }
    //   );
  }


    private getSelectedApplicableRoleTypeId(): void {
    var selectedRoleId: number = this.rId;
    // this._appRoleTypeService.getApplicableRoleTypeById(this.finYearId,
    //   this.depId, null, this.gId)
    //   .subscribe(
    //     (res: any[]) => {
    //       if (res) {
    //         res.forEach((element: any) => {
    //            if (element.RoleTypeId == selectedRoleId) {               
    //              this.selectedApplicableRoleTypeId = element.ApplicableRoleTypeId;
    //            }
    //         })
    //       }
    //     }
    //   );
  }

// Import()
// {
// var selectedrowslength=this.selection.selected.length;
//  for (var i = 0; i < selectedrowslength; i++) {
//    this.definitionDetailsIds.push(this.selection.selected[i].DefinitionDetailsId);  
// } 
// }

  Import(){     
    var length=this.selection.selected.length;
    if(length==0)
    {
       this._snackBar.open("Please select a records to import the KRA's.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
          return;
    }
      for (var i = 0; i < length; i++) {
      this.definitionDetailsIds.push(this.selection.selected[i].DefinitionDetailsId); } 
    if (this.definitionDetailsIds.length >0) {     
      let definitionModel: DefinitionModel;
      definitionModel = new DefinitionModel();
      definitionModel.roleTypeId = this.roleTypeId;
      definitionModel.currentUser=sessionStorage["mail"];  
    //  definitionModel.definitionDetailsIds= this.definitionDetailsIds;
      this._kraDefinitionService
          .ImportKRA(definitionModel)
          .subscribe(
            res => {
              this.dialogRef.close(13);
            },
            error => {
              // this.messageService.add({
              //   severity: "error",
              //   summary: "Error Message",
              //   detail: error.error
              // });
            }
          );     
    }
  }

    showDefineKRAs(mode: string) {  
    this._definekraService
      .getKRADefinitionsById(
        this.financialYearId,
        this.departmentId,
        this.importKRAForm.value.gradeId,
        this.importKRAForm.value.roleTypeId
      )
      .subscribe(
        (res: DefineKRAData[]) => {
          if (res) {
            this.length = res.length;
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;                      
          }
        },
        (error: any) => {
          if (
            (error.error.text == "No KRAs defined!" &&
              (this.selectedfromyear == this.currentyear &&
                this.currentyear < this.selectedtoyear)) ||
            this.selectedfromyear > this.currentyear
          ) {
            // this.addNewKRA = true;
            this.dataSource = null;
          } else if (error.error.text == "No KRAs defined!" || error.error.text == "No records found!") {
            this._snackBar.open("No Records Found!", "", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open(
              "Failed to get Define KRA details." + error.error,
              "x",
              {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              }
            );
          }
        }
      );
  }

   /** Whether the number of selected elements matches the total number of rows. */
   isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  // /** The label for the checkbox on the passed row */
  // checkboxLabel(row?: any): string {
  //   if (!row) {
  //     return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
  //   }
  //   return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  // }
  changeValue(e){
    if(e.value=="hr"){
      this.leaderRole=false;
    }
    else{
      this.leaderRole=true;
    }
  }

  
}
