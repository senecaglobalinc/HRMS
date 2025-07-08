import { Component, OnInit, ViewChild } from '@angular/core';
//import { kradefindata } from '../../../../modules/kra/krajson';
import { MatDialog } from '@angular/material/dialog';
import { SelectionModel } from '@angular/cdk/collections';
import { AddKRAdlgComponent } from '../add-kradlg/add-kradlg.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormControl } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MasterDataService } from '../../../../core/services/masterdata.service';
//import { SelectItem, MessageService } from "primeng/components/common/api";
import { DepartmentDetails } from '../../../../modules/master-layout/models/role.model';
import { SelectedKRAParameters } from '../../../../modules/master-layout/models/kra.model';
import { ApplicableRoleTypeService } from "../../../../modules/kra/services/applicableroletype.service";
import { ActivatedRoute } from '@angular/router';
import { DefineKraService } from "../../../../modules/kra/services/definekra.service";
import { DefineKRAData } from '../../models/definekra.model';

import { KraDefinitionService } from '../../../../modules/kra/services/kradefinition.service';
import * as servicePath from '../../../../core/service-paths';
import { KraWorkFlow } from '../../models/kra-work-flow.model';

import {
  MatSnackBar,
} from '@angular/material/snack-bar';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { MatSidenav } from '@angular/material/sidenav';
import { CommentModel } from '../../models/comment.model';
import { KraCommentService } from '../../../../modules/kra/services/kra-comment.service';
import { ConfirmationDialogComponent } from '../../../../confirmation-dialog/confirmation-dialog.component'
import { ApplicableRoleType } from '../../models/kraapplicableRoleType.model';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: "app-review-kra",
  templateUrl: "./review-kra.component.html",
  styleUrls: ["./review-kra.component.scss"]
})
export class ReviewKRAComponent implements OnInit {
  @ViewChild('sidenav') sidenav: MatSidenav;

  themeappeareance = themeconfig.formfieldappearances;
  selectedParameters: SelectedKRAParameters;
  selection = new SelectionModel<any>(true, []);
  markedAsFinish: boolean = false;
  approvedByHOD: boolean = false;
  acceptedbyCEO: boolean = false;
  addNewKRA: boolean = false;
  freezedKRA: boolean = false;
  importedKRA: boolean = false;
  newAddedRow: boolean = false;
  marksAsFinish: boolean = false;
  reviewKradata: boolean = false;
  undoBtn: boolean = false;
  undoBtnFMetric: boolean = false;
  username: string;
  editKRA: boolean = false;
  dispTargetDiv: boolean = false;
  dispMetricDiv: boolean = false;
  deletedTr: boolean = false;
  deletedKRAdiv: boolean = false;
  addedTR: boolean = false;
  dispAddedKRADiv: boolean = false;
  commentText: string;
  sentToHR:boolean=false;
  private _selectedDepartmentId: string;
  _selectedFinancialYearId: string;
  departmentHeadDepartmentId: number;
  public financialYearList: SelectItem[] = [];
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] = [];
  public roleTypeList: SelectItem[] = [];
  reviewKRAForm: FormGroup;
  commentForm: FormGroup;
  loggedinUserRole: string;
  public financialYearId: number = 0;
  public departmentId: number = 0;
  public gradeId: number = 0;
  public roleTypeId: number = 0;
  loggedInEmployeeId: number = 0;
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20, 30];
  isHODLoggedIn:boolean=false;
  HODDefinitions:DefineKRAData[]=[];
  isShowDetailData:boolean=false;
  newList:DefineKRAData[]=[];

  dataSource = new MatTableDataSource<DefineKRAData>();
  gradeNames:string

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  // End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;

  constructor(
    public dialog: MatDialog,
    public _snackBar: MatSnackBar,
    //private messageService: MessageService,
    private _masterDataService: MasterDataService,
    private _definekraService: DefineKraService,
    private fb: FormBuilder,
    private _appRoleTypeService: ApplicableRoleTypeService,
    private _kraDefinitionService: KraDefinitionService,
    private _kraCommentService: KraCommentService,
    private actRoute: ActivatedRoute,
    private spinner: NgxSpinnerService,
  ) {
    this.selectedParameters = new SelectedKRAParameters();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.username = sessionStorage["mail"];
    this.loggedInEmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.loggedinUserRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
  }
  cols = [
    { field: 'kraaspect', header: 'ASPECT' },
    { field: 'metrics', header: 'METRICS' },
    { field: 'ration', header: 'TARGET' },
  ];
  displayedColumns: string[] = [
    'kraaspect',
    'metrics',
    'ration',

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

  ngOnInit() {
    if(this.loggedinUserRole.indexOf('Operation Head')==-1){
      this.isHODLoggedIn = true;
        this.displayedColumns.push( 'Edit',
        'Delete')
    }else{
      this.displayedColumns.push('Count');
    }
    this.reviewKRAForm = this.fb.group({
      financialYearId: new FormControl(null),
      departmentId: new FormControl(null),
      gradeId: new FormControl(null),
      roleTypeId: new FormControl(null)
    });
    this.commentForm = new FormGroup({
      comment: new FormControl('', [Validators.required])
    });
    this.actRoute.params.subscribe(params => {
      this._selectedDepartmentId = params["departmentId"];
    });
    this.actRoute.params.subscribe(params => {
      this._selectedFinancialYearId = params["financialYearId"];
    });
    if (this._selectedDepartmentId) {
      this.departmentId = parseInt(this._selectedDepartmentId);
    } else {
      if (this.loggedinUserRole.indexOf("Department Head") != -1) {
        this.getUserDepartmentDetailsByEmployeeID();
      }
    }
    if (this._selectedFinancialYearId) {
      this.financialYearId = parseInt(this._selectedFinancialYearId);
    }
    this.financialYearList = [];
    this.financialYearList.push({
      label: "Select Financial Year",
      value: null
    });
    this.getFinancialYears();
    if (this.loggedinUserRole.indexOf("Operation Head") != -1 || (this.departmentId > 0 && this.loggedinUserRole.indexOf("Department Head") != -1)) {
      this.getDepartments();
    }
  }

  private getUserDepartmentDetailsByEmployeeID(): void {
    this._masterDataService.GetUserDepartmentDetailsByEmployeeID(this.loggedInEmployeeId)
      .subscribe(
        (res: DepartmentDetails[]) => {
          this.departmentList = [];
          this.departmentList.push({ label: "Select Department", value: null });
          res.forEach((e: any) => {
            this.departmentList.push({
              label: e.Description,
              value: e.DepartmentId
            });
          });
          if (this._selectedDepartmentId) {
            this.reviewKRAForm.get("departmentId").setValue(this.departmentId);
            this.reviewKRAForm.controls["departmentId"].disable();
            this.getApplicableGradeRoleTypes();
          } else this.reviewKRAForm.controls["departmentId"].enable();
        },
        error => {
          this._snackBar.open('Failed to get Department Details', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }

  private getFinancialYears(): void {
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
        if (this._selectedFinancialYearId) {
          this.reviewKRAForm
            .get("financialYearId")
            .setValue(this.financialYearId);
          this.reviewKRAForm.controls["financialYearId"].disable();
        } else this.reviewKRAForm.controls["financialYearId"].enable();
      },
      error => {
        this._snackBar.open('Failed to get Financial Year List', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        // this.messageService.add({
        //   severity: "error",
        //   summary: "Error Message",
        //   detail: "Failed to get Financial Year List"
        // });
      }
    );
  }

  private getDepartments(): void {
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
        if (this._selectedDepartmentId) {
          this.reviewKRAForm.get("departmentId").setValue(this.departmentId);
          this.reviewKRAForm.controls["departmentId"].disable();
          this.getApplicableGradeRoleTypes();
        } else this.reviewKRAForm.controls["departmentId"].enable();
      },
      error => {
        this._snackBar.open('Failed to get Department Details', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  onchange(event) {
    this.financialYearId = event.value;
    this.reviewKRAForm.get("departmentId").setValue(null);
    this.reviewKRAForm.get("gradeId").setValue(null);
    this.reviewKRAForm.get("roleTypeId").setValue(null);
    if (this._selectedFinancialYearId) {
      this.getApplicableGradeRoleTypes();
    }
  }
  ondepartmentchange(event) {
    this.gradeNames="";
    this.approvedByHOD=false;
    this.marksAsFinish = false
    this.sentToHR=false
    this.dataSource = null;
    this.departmentId = event.value;
    this.length=0;
    this.reviewKRAForm.get("gradeId").setValue(null);
    this.reviewKRAForm.get("roleTypeId").setValue(null);
    // this.getApplicableGradeRoleTypes();
    this.getRoleTypes();
  }
  getApplicableGradeRoleTypes() {
    this.resetgraderoletypes();
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
    //         //this.addApplicableRole = false;
    //         //this.gradeList.push({ label: "Select Grade", value: null })
    //         res.forEach((element: any) => {
    //           this.gradeList.push({
    //             label: element.Grade,
    //             value: element.GradeId
    //           });
    //           this.gradeId = element.GradeId;
    //         });
    //         if (this.gradeId > 0) {
    //           this.reviewKRAForm.get("gradeId").setValue(this.gradeId);
    //           this.getApplicableGradeRoleTypesByGrade();
    //         }
    //       }
    //     },
    //     (error: any) => {
    //       this._snackBar.open('Failed to get Role Type details.', '', {
    //         duration: 1000,
    //         horizontalPosition: 'right',
    //         verticalPosition: 'top',
    //       });
    //     }
    //   );
    this._appRoleTypeService
      .getGradesByDepartment(
        this.departmentId,
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            this.gradeList.push({ label: "Select Grade", value: null });
            res.forEach((element: any) => {
              this.gradeList.push({
                label: element.Name,
                value: element.Id
              });
              if (res.length === 1){
              this.gradeId = element.Id;
              this.reviewKRAForm.get("gradeId").setValue(this.gradeId);
              this.getApplicableGradeRoleTypesByGrade();
              }
            });
            // if (this.gradeId > 0) {
            //   if(this.gradeList.length===2){
            //     this.reviewKRAForm.get("gradeId").setValue(this.gradeId);
            //   }else{
            //     this.reviewKRAForm.get("gradeId").setValue(null);
            //   }
            //   if(res.length>0){
            //     this.getApplicableGradeRoleTypesByGrade();
            //   }
            // }
            if(res.length>0){
              // this.getApplicableGradeRoleTypesByGrade();
            }
            else{
              this.dataSource = null;
            this.resetgraderoletypes();
            }
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
          // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error Message',
          //   detail: 'Failed to get Role Type details.'
          // });
        }
      );
  }

  // private getappGradeRoleTypesByGrade(event) {
  //   this.gradeId = event.value;
  //   this.roleTypeId = 0;
  //   this.getApplicableGradeRoleTypesByGrade();
  // }

  getApplicableGradeRoleTypesByGrade() {
    this.approvedByHOD = false
    this.sentToHR=false
    this.length=0
    this.roleTypeList = [];
    this.selectedParameters.RoleTypeName = "";
    this.reviewKRAForm.get("roleTypeId").setValue(null);
    //this.roleTypeList.push({ label: "Select Role Type", value: null })
    // this._appRoleTypeService
    //   .getApplicableRoleTypeById(
    //     this.financialYearId,
    //     this.departmentId,
    //     null,
    //     this.reviewKRAForm.value.gradeId
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
    //           this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
    //           this.showDefineKRAs("");
    //         }
    //       }
    //     },
    //     (error: any) => {
    //       this._snackBar.open('Failed to get Role Type details.', '', {
    //         duration: 1000,
    //         horizontalPosition: 'right',
    //         verticalPosition: 'top',
    //       });
    //     }
    //   );
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    if(this.reviewKRAForm.value.gradeId!==null&&this.departmentId!==null){
    this._appRoleTypeService
      .getRoleTypesByGrade(
        this.departmentId,
        this.reviewKRAForm.value.gradeId
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((element: any) => {
              this.roleTypeList.push({
                label: element.Name,
                value: element.Id
              });
              if(res.length === 1) {
                this.roleTypeId = element.Id;
                this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              }
            });
            // if (this.roleTypeId > 0) {
            //   this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
            //   // this.showDefineKRAs("");
            //   this.getKrasByRoletypeFinancialYear()
            // }
            if (res.length > 0) {
              // this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              // this.showDefineKRAs("");
              this.getKrasByRoletypeFinancialYear()
            }
            else{
              this.dataSource = null;
            this.resetgraderoletypes();
            }
          }
          // this.spinner.hide();
        },
        (error: any) => {
          // this.spinner.hide();
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
    }
  }

  resetgraderoletypes(){
    this.roleTypeList = [];
    this.gradeList = [];
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    this.gradeList.push({ label: "Select Grade", value: null });
  }



  EDITKRAS() {
    this.editKRA = this.addNewKRA = this.marksAsFinish = true;
  }

  markAsFinish() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Are you sure to Proceed',
        buttonText: {
          ok: 'Yes',
          cancel: 'No',
        },
      },
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        var creatingObj = new KraWorkFlow();
        creatingObj.financialYearId = this.financialYearId;
        creatingObj.departmentId = this.departmentId;
        creatingObj.roleTypeIds.push(this.reviewKRAForm.value.roleTypeId)
        this._kraDefinitionService.EditedKRASByHOD(creatingObj).subscribe((res) => {
          this.addNewKRA = false
          this.getKrasByRoletypeFinancialYear()
          this._snackBar.open("updated successfully.", '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        },
          (error => {
            this.addNewKRA = true
            this._snackBar.open(error.error.message, '', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }))
      }
    })
    // this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
    //   (res: any[]) => {
    //     if (res) {
    //       this.markedAsFinish = true;
    //       this.editKRA = this.addNewKRA = this.marksAsFinish = false;
    //     } else {
    //       this.markedAsFinish = false;
    //     }
    //   },
    //   (error: any) => {
    //     this._snackBar.open('Failed to update.', '', {
    //       duration: 1000,
    //       horizontalPosition: 'right',
    //       verticalPosition: 'top',
    //     });
    //   }
    // );
  }
  openAddKRA(e): void {
    if (this.reviewKRAForm.valid) {
      if (e == "add") {
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: "80vw",
          data: {
            heading: "Add KRA",
            btntext: "Add",
            finYearId: this.financialYearId,
            depId: this.departmentId,
            gradeId: this.reviewKRAForm.value.gradeId,
            roleId: this.reviewKRAForm.value.roleTypeId,
            editMode: false,
            IsHOD: true,
            definitionDetailsId: null
          }
        });
        dialogRef.afterClosed().subscribe(
          (res: any) => {
            if (res == 13) {
              setTimeout(() => {
                this.addNewKRA = false;
                this.addedTR = true;
                this.marksAsFinish = true;
                this.approvedByHOD = this.acceptedbyCEO = false;
                // this.showDefineKRAs("add");
                this._snackBar.open('KRA Added Successfully',  '', {
                  duration: 1000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.getKrasByRoletypeFinancialYear()
              }, 10);
            }
          },
          (error: any) => {
            this._snackBar.open('Failed to get KRA details.' + error.error, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        );
      }
       else {
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: "80vw",
          data: {
            heading: "Edit KRA",
            btntext: "Update",
            finYearId: this.financialYearId,
            editMode: true,
            IsHOD: true,
            depId: this.departmentId,
            gradeId: this.reviewKRAForm.value.gradeId,
            roleId: this.reviewKRAForm.value.roleTypeId,
            definitionDetailsId: e.DefinitionId,
            definitionTransactionId : e.DefinitionTransactionId,
            definitionDetails:e
          }
        });
        dialogRef.afterClosed().subscribe((res: any) => {
          if (res == 13) {
            this.addNewKRA = false;
            this.addedTR = true;
            this.marksAsFinish = true;
            this.approvedByHOD = this.acceptedbyCEO = false;
            // this.showDefineKRAs("edit");
            this._snackBar.open('KRA Updated Successfully',  '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.getKrasByRoletypeFinancialYear()
          }
        });
      }
    }
  }

  SetPreviousTargetValues(e): void {
    e.IsActive=e.IsActive===false?false:true;
    e.financialYearId = this.reviewKRAForm.value.financialYearId
    e.roleTypeId = this.reviewKRAForm.value.roleTypeId
    this._kraDefinitionService.AcceptedByOperationHead(e).subscribe(
      (res: any) => {
        if(res==="KRA accepted successfully by operation head.")
        this._snackBar.open(res, '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.getKrasByRoletypeFinancialYear();

      },
      (error: any) => {
        this._snackBar.open('Failed to update KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  UnSetPreviousTargetValues(e): void {
    e.IsActive=e.IsActive===false?false:true;
    this._kraDefinitionService.RejectedByOperationHead(e).subscribe(
      (res: any) => {
        this._snackBar.open(res, '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.getKrasByRoletypeFinancialYear();

      },
      (error: any) => {
        this._snackBar.open('Failed to update KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  SetPreviousMetricValues(e): void {
    this._kraDefinitionService.SetPreviousMetricValues(e).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open('KRA Definition updated successfully.', '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open('Failed to update KRA Definition.' + res.Message, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          } else {
            this._snackBar.open('Failed to update KRA Definition.' + res.Message, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open('Failed to update KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  ADDKRAAgain(e): void {
    this._kraDefinitionService.AddKRAAgain(e).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open('KRA Definition updated successfully.', '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open('Failed to update KRA Definition.' + res.Message, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          } else {
            this._snackBar.open('Failed to update KRA Definition.' + res.Message, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open('Failed to update KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  ContinueEditing() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.reviewKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.reviewKRAForm.value.roleTypeId;
    creatingObj.status = "EditedByHOD";
    // this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
    //   (res: any[]) => {
    //     if (res) {
    //       this.marksAsFinish = this.addNewKRA = this.editKRA = false;
    //       this.showDefineKRAs("");
    //       // this.marksAsFinish = true;
    //       // this.markedAsFinish = false;
    //       // this.checkMarkAsFinished=false;
    //       //this.editKra = (this.selectedfromyear == this.currentyear  && this.currentyear<this.selectedtoyear)||(this.selectedfromyear>this.currentyear);
    //     } else {
    //       this.marksAsFinish = false;
    //       this.markedAsFinish = true;
    //     }
    //   },
    //   (error: any) => {
    //     this._snackBar.open('Failed to update.', '', {
    //       duration: 1000,
    //       horizontalPosition: 'right',
    //       verticalPosition: 'top',
    //     });
    //     // this.messageService.add({
    //     //   severity: "error",
    //     //   summary: "Error Message",
    //     //   detail: "Failed to update."
    //     // });
    //   }
    // );
  }
  openComments() {
    this.sidenav.toggle();
    this.getComments();
  }

  closeSideNav() {
    this.sidenav.close();
  }

  saveComment(): void {
    if (this.reviewKRAForm.valid && this.commentForm.valid) {
      let commentModel: CommentModel;
      commentModel = new CommentModel();

      commentModel.CommentText = this.commentForm.value.comment;
      commentModel.Username = this.username;
      commentModel.IsCEO = false;
      commentModel.FinancialYearId = this.financialYearId;
      commentModel.DepartmentId = this.departmentId;
      commentModel.GradeId = this.reviewKRAForm.value.gradeId;
      commentModel.RoleTypeId = this.reviewKRAForm.value.roleTypeId;

      this._kraCommentService
        .CreateComment(commentModel)
        .subscribe((createResponse: boolean) => {
          if (createResponse) {
            this.commentForm.get("comment").setValue("");
            this._snackBar.open('Comment created successfully', '', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            // this.commentForm.value.comment = "";
            // this.getComments();
          }
          else {
            this._snackBar.open('Failed to create Comment.', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        },
          error => {
            this._snackBar.open('Failed to Create comment.', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        );
        this.sidenav.close();
    }
  }

  getComments() {
    this._kraCommentService.GetComment(
      this.financialYearId,
      this.departmentId,
      this.reviewKRAForm.value.gradeId,
      this.reviewKRAForm.value.roleTypeId, false).subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((e) => {
              this.commentText = e.CommentText;
            });
          }
          else {
            this._snackBar.open('Failed to get comments', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        },
        (error: any) => {
          this._snackBar.open(error.error.text, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }

  ApproveKRA() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Are you sure to Proceed',
        buttonText: {
          ok: 'Yes',
          cancel: 'No',
        },
      },
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        let approvalObj: KraWorkFlow
        approvalObj = new KraWorkFlow()
        approvalObj.financialYearId = this.financialYearId;
        approvalObj.departmentId = this.departmentId;
        approvalObj.roleTypeIds.push(this.reviewKRAForm.value.roleTypeId)
        this._kraDefinitionService.ApproveKRASBYHOD(approvalObj).subscribe((res) => {
          // this.approvedByHOD = true;
          this.addNewKRA = false
          this.getKrasByRoletypeFinancialYear()
          this._snackBar.open('Approved Successfully', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.clearData()

        }, (error) => {
          this.approvedByHOD = false;
          this._snackBar.open('Failed to update', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        })
      }
    })
  }

  AcceptOriginal() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.reviewKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.reviewKRAForm.value.roleTypeId;
    creatingObj.status = "AcceptOriginal";
    // this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
    //   (res: any[]) => {
    //     if (res) {
    //       this.showDefineKRAs("");
    //       this.approvedByHOD = true;
    //       this.addNewKRA = this.editKRA = false;
    //     } else {
    //       this.markedAsFinish = false;
    //     }
    //   },
    //   (error: any) => {
    //     this._snackBar.open("Failed to update.", '', {
    //       duration: 1000,
    //       horizontalPosition: 'right',
    //       verticalPosition: 'top',
    //     });
    //     // this.messageService.add({
    //     //   severity: "error",
    //     //   summary: "Error Message",
    //     //   detail: "Failed to update."
    //     // });
    //   }
    // );
  }

  showDefineKRAs(mode: string) {
    if(this.reviewKRAForm.value.gradeId!==null&&this.departmentId!==null&&this.reviewKRAForm.value.roleTypeId!==null){
    this._definekraService
      .getKRADefinitionsById(
        this.financialYearId,
        this.departmentId,
        this.reviewKRAForm.value.gradeId,
        this.reviewKRAForm.value.roleTypeId,
        true
      )
      .subscribe(
        (res: DefineKRAData[]) => {
          if (res) {
            this.length = res.length;
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            this.reviewKradata = true;
            res.forEach((data: DefineKRAData) => {
              this.approvedByHOD = data.Status == "ApprovedbyHOD";
              if (data.Status == "ApprovedByCEO") {
                this.acceptedbyCEO = true;
              }
              this.markedAsFinish = data.Status == "FinishedEditByHOD";
              if (data.Status == "EditedByHOD") {
                this.EDITKRAS();
                this.marksAsFinish = true;
              }

            });
            // this.freezedKRA = true;
            // if(this.markedAsFinish)  this.markasfinish=false;
            if (mode == "edit") {
              setTimeout(() => {
                this._snackBar.open('KRA Updated Successfully', '', {
                  duration: 1000,
                  panelClass: ['success-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }, 10);
            } else if (mode == "add") {
              setTimeout(() => {
                this._snackBar.open('KRA Added Successfully', '', {
                  duration: 1000,
                  panelClass: ['success-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }, 10);
            }
          }
        },
        (error: any) => {
          this._snackBar.open(error.error.text, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
    }
  }

  onDeleteKRA(e) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Are you sure to delete KRA' ,
        buttonText: {
          ok: 'Yes',
          cancel: 'No',
        },
      },
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
    if (confirmed) {
    let deleteReqObj = {
      definitionId: e.DefinitionId,
      definitionTransactionId: e.DefinitionTransactionId
    }

    this._kraDefinitionService.DeleteKRAByHOD(deleteReqObj).subscribe(
      (res: any) => {
          // this.showDefineKRAs("refresh");
          this.marksAsFinish = true
          this.getKrasByRoletypeFinancialYear()
          this._snackBar.open("KRA Definition deleted successfully.", '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      },
      (error: any) => {
        this._snackBar.open('Failed to delete KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
    }
  })
  }


  dispTarget() {
    this.dispTargetDiv = true;
    setTimeout(() => {
      this.dispTargetDiv = false;
    }, 3000);
  }


  dispMetric() {
    this.dispMetricDiv = true;
    setTimeout(() => {
      this.dispMetricDiv = false;
    }, 3000);
  }

  dispDelKRA(details) {
   this.newList.forEach(element=>element.IsShowCountDetails=false);
    details.IsShowCountDetails=true;
    this.deletedKRAdiv = true;
    setTimeout(() => {
      this.deletedKRAdiv = false;
    }, 3000);
  }

  deleteKRA() {
    this.deletedTr = true;
  }


  DeleteKRA(definitionDetailId: number) {
    this._kraDefinitionService.DeleteByHODHardDelete(definitionDetailId).subscribe(
      (res: any) => {
        if (res == true) {
          // this.showDefineKRAs("refresh");
          this.getKrasByRoletypeFinancialYear()
          this._snackBar.open('KRA Definition deleted successfully.', '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        } else {
          this._snackBar.open('Failed to delete KRA Definition.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
      (error: any) => {
        this._snackBar.open('Failed to delete KRA Definition.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
  }

  dispAddedKRA() {
    this.dispAddedKRADiv = true;

    setTimeout(() => {
      this.dispAddedKRADiv = false;
    }, 3000);
  }

  getKrasByRoletypeFinancialYear(){
    if(this.isHODLoggedIn){
      this.approvedByHOD = false
      this.acceptedbyCEO = false
    this.length=0
    this.HODDefinitions=[];
    this.spinner.show();
    this._kraDefinitionService.getKrasByFinancialYearIdAndRoleTypeId(this.financialYearId,this.reviewKRAForm.value.roleTypeId).subscribe(
      (res: DefineKRAData[]) => {
        this.spinner.hide();
        this.addNewKRA = true;
        if (res) {
          // this.length = res.length;
          // this.dataSource = new MatTableDataSource(res);
          // this.dataSource.paginator = this.paginator;
          // this.dataSource.sort = this.sort;
          res.forEach((data: DefineKRAData) => {
            // this.roletypeStatus = data.Status;
            if(data.StatusId != 0){
              this.HODDefinitions.push(data)
            }
            if(data.IsUpdatedByHOD && data.StatusId == 1){
              this.marksAsFinish = true
            }
            if (data.Status == "ApprovedByCEO") {
              this.acceptedbyCEO = true;
              // this.approvedKRA = true;
            }
            if (data.StatusId > 1) {
              this.approvedByHOD = true;
              this.addNewKRA=false
            }
            if (data.StatusId == 4) {
              this.sentToHR=true
            }
          });
          this.length = this.HODDefinitions.length;
          if(this.length == 0){
            this.addNewKRA = false
          }
          this.dataSource = new MatTableDataSource(this.HODDefinitions);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
        else{
          this.dataSource = null;
        }

      },
      (error: any) => {
        this.spinner.hide();
        this.length=0;
        if(error.error.text==="KRAs not found from Definition table!"){
          this.addNewKRA = true;
          this.dataSource = null;
        }
      //  else if (
      //     ((error.error == "No KRAs defined!" || error.error.text == "No KRAs defined!") &&
      //       (this.selectedfromyear == this.currentyear &&
      //         this.currentyear < this.selectedtoyear)) ||
      //     this.selectedfromyear > this.currentyear
      //   ) {
      //     this.addNewKRA = true;
      //     this.dataSource = null;
      //   }
         else if ((error.error == "No KRAs defined!" || error.error == "No records found!"
        || error.error =="No KRAs found!")||
        (error.error.text == "No KRAs defined!" || error.error.text == "No records found!"
        || error.error.text =="No KRAs found!")) {
          this._snackBar.open("No Records Found!", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
          this.dataSource = null
        } else {
          this.dataSource=null
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
  } else{
      this._kraDefinitionService.getOperationHeadDefinitions(this.financialYearId,this.reviewKRAForm.value.roleTypeId).subscribe(
        (res: DefineKRAData[]) => {
          if (res != null) {
            this.newList=res;

            const uniquList=this.filterDuplicates(this.newList,'DefinitionId');
            this.length = res.length;
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            // res.forEach((data: DefineKRAData) => {
            //   // this.roletypeStatus = data.Status;
            //   if (data.Status == "ApprovedByCEO") {
            //     this.acceptedbyCEO = true;
            //     // this.approvedKRA = true;
            //   }
            //   if (data.Status == "ApprovedbyHOD") {
            //     this.approvedByHOD = true;
            //     // this.approvedKRA = true;
            //   }
            // });
          }
          else{
            this.dataSource = null;
          }
          this.addNewKRA = true;
        },
        (error: any) => {
          this.length=0;
          if(error.error.text==="KRAs not found from Definition table!"){
            this.addNewKRA = true;
            this.dataSource = null;
  }
        //  else if (
        //     ((error.error == "No KRAs defined!" || error.error.text == "No KRAs defined!") &&
        //       (this.selectedfromyear == this.currentyear &&
        //         this.currentyear < this.selectedtoyear)) ||
        //     this.selectedfromyear > this.currentyear
        //   ) {
        //     this.addNewKRA = true;
        //     this.dataSource = null;
        //   }
           else if ((error.error == "No KRAs defined!" || error.error == "No records found!"
          || error.error =="No KRAs found!")||
          (error.error.text == "No KRAs defined!" || error.error.text == "No records found!"
          || error.error.text =="No KRAs found!")) {
            this._snackBar.open("No Records Found!", "", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
            this.dataSource = null
          } else {
            this.dataSource=null
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
  }

  findDifferenceValues(item1: any, item2: any): number {
    let differenceCount:number=0;
    if(item1.MeasurementTypeId!==item2.MeasurementTypeId)
    differenceCount++;
    if(item1.Metric!==item2.Metric)
    differenceCount++;
    if(item1.OperatorId!==item2.OperatorId)
    differenceCount++;
    if(item1.TargetPeriodId!==item2.TargetPeriodId)
    differenceCount++;
    if(item1.TargetValue!==item2.TargetValue)
    differenceCount++;

    return differenceCount;
   }

  filterDuplicates(list,key){
    const reqObj={IsShowItem:true,OldMetric:''}
    list.forEach((item1,index1,arrayList)=>{
      Object.assign(item1,reqObj);
      list.forEach((item2,index2,arrayList)=>{
          if(index1!==index2){
              if(item1.DefinitionId!==null&&item2.DefinitionId!==null&&
                item1.DefinitionId===item2.DefinitionId){
                  if(item1.DefinitionTransactionId===0){
                     item1.IsShowItem=false;
                  }else{
                    if(item1.IsAccepted===false){
                       item1.TargetValue=item2.TargetValue;
                       item1.MeasurementType=item2.MeasurementType;
                       item1.Target=item2.Target;
                       item1.Metric=item2.Metric;
                    }else{
                      item1.OldMetric=item2.OldMetric;
                      item1.OldTargetValue=item2.TargetValue;
                      item1.OldMeasurementType=item2.MeasurementType;
                      item1.OldOperatorValue = item2.OperatorId;
                      item1.OldTargetPeriod = item2.TargetPeriodId;
                      item1.OldTarget = item2.Target;
                      item1.OldCount=this.findDifferenceValues(item1,item2);
                    }

                  }
                  if(item2.DefinitionTransactionId===0){
                    item2.IsShowItem=false;
                  }else{
                    if(item2.IsAccepted===false){
                      item2.TargetValue=item1.TargetValue;
                      item2.MeasurementType=item1.MeasurementType;
                      item2.Target=item1.Target;
                      item2.Metric=item1.Metric;
                   }else{
                    item2.OldMetric=item1.Metric;
                    item2.OldTargetValue=item1.TargetValue;
                    item2.OldMeasurementType=item1.MeasurementType;
                    item2.OldOperatorValue = item1.OperatorId;
                   item2.OldTargetPeriod = item1.TargetPeriodId;
                   item2.OldTarget = item1.Target;
                   item2.OldCount = this.findDifferenceValues(item2,item1);
                   }
                  }
              }
          }
      })
    })
    return list;
  }

  clearData(){
    this.addNewKRA=false
    this.editKRA=false
    this.marksAsFinish=false
  }
  sendToHR(){
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Are you sure to Proceed',
        buttonText: {
          ok: 'Yes',
          cancel: 'No',
        },
      },
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        let approvalObj: KraWorkFlow
        approvalObj = new KraWorkFlow()
        approvalObj.financialYearId = this.financialYearId;
        approvalObj.departmentId = this.departmentId;
        approvalObj.roleTypeIds.push(this.reviewKRAForm.value.roleTypeId)
        this._kraDefinitionService.SendToOperationHead(approvalObj).subscribe((res) => {
          this.sentToHR = true
          this._snackBar.open("Notification Sent successfully.", '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        },
          (error => {
            this._snackBar.open(error.error.message, '', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }))
      }
    })
  }
  getRoleTypes(){
    this.approvedByHOD = false
    this.sentToHR=false
    this.length=0
    this.roleTypeList = [];
    this.selectedParameters.RoleTypeName = "";
    this.reviewKRAForm.get("roleTypeId").setValue(null);
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    if(this.departmentId!==null){
    this._appRoleTypeService
      .getRoleTypesByFinancialYearAndDepartment(
        this.reviewKRAForm.value.financialYearId,this.departmentId
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((element: any) => {
              this.roleTypeList.push({
                label: element.Name,
                value: element.Id
              });
              if(res.length === 1) {
                this.roleTypeId = element.Id;
                this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
                this.getKrasByRoletypeFinancialYear()
              }
            });
            // if (this.roleTypeId > 0) {
            //   this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
            //   // this.showDefineKRAs("");
            //   this.getKrasByRoletypeFinancialYear()
            // }
            if (res.length > 0) {
              // this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              // this.showDefineKRAs("");
              // this.getKrasByRoletypeFinancialYear()
            }
            else{
              this.dataSource = null;
            this.resetgraderoletypes();
            }
          }
          // this.spinner.hide();
        },
        (error: any) => {
          // this.spinner.hide();
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
    }
  }

  getGradesForSelectedRole() {
    this.gradeNames = "";
    this._appRoleTypeService
      .getGradesForSelectedRole(this.reviewKRAForm.value.financialYearId, this.departmentId, this.reviewKRAForm.value.roleTypeId)
      .subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            this.gradeNames = res[0].GradeName
          } else {
            this.gradeNames = "";
          }
        },
        (error: any) => {
          this.gradeNames = "";
        }
      );
  }

  onRoleTypeChange(){
    this.getGradesForSelectedRole();
    this.getKrasByRoletypeFinancialYear();
  }

}
