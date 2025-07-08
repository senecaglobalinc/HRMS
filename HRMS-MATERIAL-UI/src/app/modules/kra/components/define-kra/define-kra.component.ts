import { Component, OnInit, ViewChild } from "@angular/core";
import { FormControl } from "@angular/forms";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from "@angular/material/dialog";
import { ImportKraDlgComponent } from "../../components/import-kra-dlg/import-kra-dlg.component"
import { CloneKraDlgComponent } from "../../components/clone-kra-dlg/clone-kra-dlg.component";
import { SelectionModel } from "@angular/cdk/collections";
import { AddKRAdlgComponent } from "../../components/add-kradlg/add-kradlg.component";
import { ApplicableRoleTypeService } from "../../../kra/services/applicableroletype.service";
import { MasterDataService } from "../../../../core/services/masterdata.service";
import { DepartmentDetails } from "../../../master-layout/models/role.model";
import {
  SelectedKRAParameters
} from "../../../master-layout/models/kra.model";
import { RoleTypeService } from "../../../admin/services/role-type.service";
import {
  ApplicableRoleType,
  ApplicableRoleTypeData
} from "../../../kra/models/kraapplicableRoleType.model";

import { ActivatedRoute, Router } from "@angular/router";
import { DefineKraService } from "../../../kra/services/definekra.service";
import { DefineKRAData } from "../../../kra/models/definekra.model";
import { KraDefinitionService } from "../../../kra/services/kradefinition.service";
import * as servicePath from "../../../../core/service-paths";

import {
  MatSnackBar
} from "@angular/material/snack-bar";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from "@angular/material/sort";
import { themeconfig } from "../../../../../themeconfig";
import { MatSidenav } from '@angular/material/sidenav';
import { CommentModel } from "../../../kra/models/comment.model";
import { KraCommentService } from "../../../kra/services/kra-comment.service";
import { NavService } from "../../../master-layout/services/nav.service";
import { ConfirmDialogComponent } from "../../../project-life-cycle/components/confirm-dialog/confirm-dialog.component";
import { flatten } from "@angular/compiler";
import { NgxSpinnerService } from 'ngx-spinner';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: "app-define-kra",
  templateUrl: "./define-kra.component.html",
  styleUrls: ["./define-kra.component.scss"]
})
export class DefineKRAComponent implements OnInit {
  @ViewChild('sidenav') sidenav: MatSidenav;
  selectedParameters: SelectedKRAParameters;
  themeappeareance = themeconfig.formfieldappearances;
  //displayedColumns: string[] = ['kraaspect', 'metrics', 'ration'];
  //dataSource;
  roletypedata: any;
  //importedKradisplayedColumns: string[] = ['select', 'kraaspect', 'metrics', 'ration'];
  selection = new SelectionModel<any>(true, []);
  addNewKRA: boolean = false;
  acceptedbyCEO: boolean = false;
  approvedKRA: boolean = false;
  importedKRA: boolean = false;
  roletypeStatus: string = "";
  showCommentsDialog: boolean = false;
  username: string;
  statusId: number;
  gridMessage: string = "No records found";
  defineKRAForm: FormGroup;
  commentForm: FormGroup;
  commentText: string;
  private _selectedDepartmentId: string;
  _selectedFinancialYearId: string;
  departmentHeadDepartmentId: number;
  public financialYearId: number = 0;
  public departmentId: number = 0;
  public gradeId: number = 0;
  public roleTypeId: number = 0;
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] = [];
  public roleTypeList: SelectItem[] = [];
  public financialYearList: SelectItem[] = [];
  addApplicableRole = false;
  addKra = false;
  undoBtn: boolean = false;
  editKRA: boolean = false;
  undoDiv: boolean = false;
  deletedTr: boolean = false;
  deletedKRAdiv: boolean = false;
  addedTR: boolean = false;
  dispAddedKRADiv: boolean = false;
  markasfinish = false;
  markedAsFinish = false;
  editByHRStatus = false;
  checkMarkAsFinished = false;
  approvedByHOD: boolean = false;
  sendtoHOD = false;
  currentyear = new Date().getFullYear();
  selectedfromyear: number;
  selectedtoyear: number;
  dispTargetDiv: boolean = false;
  dispMetricDiv: boolean = false;
  //defineDataSource = [];
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20, 30];
  addNewKraButton : boolean = false
  freezeKRAs = false
  dataSource = new MatTableDataSource<DefineKRAData>();
  gradeNames:string;

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;
  @ViewChild(MatSort, { static: true })
  sort: MatSort;
  // End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;

  constructor(
    public dialog: MatDialog,
    private _snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    //private messageService: MessageService,
    private _roleTypeService: RoleTypeService,
    private fb: FormBuilder,
    private _appRoleTypeService: ApplicableRoleTypeService,
    private _router: Router,
    private _kraDefinitionService: KraDefinitionService,
    private _definekraService: DefineKraService,
    private _kraCommentService: KraCommentService,
    private actRoute: ActivatedRoute,
    private spinner: NgxSpinnerService
  ) {
    this.selectedParameters = new SelectedKRAParameters();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.username = sessionStorage["mail"];
  }

  cols = [
    { field: "kraaspect", header: "ASPECT" },
    { field: "metrics", header: "METRICS" },
    { field: "ration", header: "TARGET" }
  ];
  displayedColumns: string[] = [
    "kraaspect",
    "metrics",
    "ration",
    "Edit",
    "Delete"
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
    this.defineKRAForm = this.fb.group({
      financialYearId: [null],
      departmentId: [null],
      gradeId: [null],
      roleTypeId: [null]
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
    this.getDepartments();
  }

  openComments() {
    this.sidenav.toggle();
    this.getComments();
  }

  closeSideNav() {
    this.sidenav.close();
  }

  saveComment(): void {
    if (this.defineKRAForm.valid && this.commentForm.valid) {
      let commentModel: CommentModel;
      commentModel = new CommentModel();

      commentModel.CommentText = this.commentForm.value.comment;
      commentModel.Username = this.username;
      commentModel.IsCEO = false;
      commentModel.FinancialYearId = this.financialYearId;
      commentModel.DepartmentId = this.departmentId;
      commentModel.GradeId = this.defineKRAForm.value.gradeId;
      commentModel.RoleTypeId = this.defineKRAForm.value.roleTypeId;

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
      this.defineKRAForm.value.gradeId,
      this.defineKRAForm.value.roleTypeId, false).subscribe(
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

  getFinancialYears(): void {
    this._masterDataService.GetFinancialYears().subscribe(
      (yearsdata: any[]) => {
        yearsdata.forEach((e: any) => {
          this.financialYearList.push({
            label: e.FinancialYearName,
            value: e.Id
          });
        });
        if (this._selectedFinancialYearId) {
          this.defineKRAForm
            .get("financialYearId")
            .setValue(this.financialYearId);
          this.defineKRAForm.controls["financialYearId"].disable();
        } else this.defineKRAForm.controls["financialYearId"].enable();
      },
      error => {
        this._snackBar.open("Failed to get Financial Year List", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
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
        if (this._selectedDepartmentId) {
          this.defineKRAForm.get("departmentId").setValue(this.departmentId);
          this.defineKRAForm.controls["departmentId"].disable();
          this.getRoleTypes()
        } else this.defineKRAForm.controls["departmentId"].enable();
      },
      error => {
        this._snackBar.open("Failed to get Department Details", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  onchange(event) {
    let target = event.source.selected._element.nativeElement;
    this.selectedfromyear = parseInt(target.innerText.split("-")[0].trim());
    this.selectedtoyear = parseInt(target.innerText.split("-")[1].trim());
    this.financialYearId = event.value;
    this.defineKRAForm.get("departmentId").setValue(null);
    this.defineKRAForm.get("gradeId").setValue(null);
    this.defineKRAForm.get("roleTypeId").setValue(null);
    //For currentyear checking
    this.editKRA =
      (this.selectedfromyear == this.currentyear &&
        this.currentyear < this.selectedtoyear) ||
      this.selectedfromyear > this.currentyear;
    this.approvedKRA = this.addKra = this.addNewKRA = this.markasfinish = this.markedAsFinish = this.sendtoHOD = this.acceptedbyCEO = false;
    this.addNewKraButton = false;
    this.freezeKRAs = false
    this.dataSource = null;
    this.selectedParameters.DepartmentName = "";
    // if (this._selectedFinancialYearId) {
    //   this.getApplicableGradeRoleTypes();
    // }
  }
  ondepartmentchange(event) {
    this.freezeKRAs=false;
    this.gradeNames="";
    this.setDefaultValues();
    this.departmentId = event.value;
    this.defineKRAForm.get("roleTypeId").setValue(null);
    this.getRoleTypes();
  }
  onRoleTypeChange() {
    this.getGradesForSelectedRole();
    this.showDefineKRAs("u");
  }

  getApplicableGradeRoleTypes() {
    this.resetgraderoletypes();
    this.dataSource = null;
    this.gradeList = [];
    this._appRoleTypeService
      .getGradesByDepartment(        
        this.departmentId,        
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            this.addApplicableRole = false;
            this.gradeList.push({ label: "Select Grade", value: null });
            res.forEach((element: any) => {
              this.gradeList.push({
                label: element.Name,
                value: element.Id
              });
              if (res.length === 1){
                this.gradeId = element.Id;
                this.defineKRAForm.get("gradeId").setValue(this.gradeId);
                // this.getApplicableGradeRoleTypesByGrade();
              }
            });
            this.dataSource = null;
            this.approvedKRA = this.addKra = this.addNewKRA = this.sendtoHOD = this.markedAsFinish = this.markasfinish = false;
            this.selectedParameters.GradeName = "";
            this.selectedParameters.RoleTypeName = "";
          } else {
            this.addApplicableRole = true;
            this.addNewKRA = this.addKra = this.approvedKRA = this.sendtoHOD = this.markedAsFinish = this.markasfinish = false;
            this.addNewKraButton = false
            this.dataSource = null;
            this.resetgraderoletypes();
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
  setDefaultValues() {
    this.markedAsFinish = this.checkMarkAsFinished = this.addNewKRA = false;
    this.markasfinish = this.sendtoHOD = this.editKRA = this.addKra = false;
    this.addApplicableRole = this.approvedKRA = this.acceptedbyCEO = this.editByHRStatus =this.approvedByHOD= false;
  }
  showDefineKRAs(mode: string) {
    this.spinner.show();
    this.addNewKRA =  true;
    this.freezeKRAs=false;
    this.addNewKraButton = false
    this.setDefaultValues();
    if(this.departmentId!==null&&this.defineKRAForm.value.roleTypeId!==null){
    this._definekraService
      .getKRADefinitionsById(
        this.financialYearId,
        this.departmentId,
        this.defineKRAForm.value.gradeId,
        this.defineKRAForm.value.roleTypeId
      )
      .subscribe(
        (res: DefineKRAData[]) => { 
          this.spinner.hide();
          if (res != null) {  
            this.length = res.length;            
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            res.forEach((data: DefineKRAData) => {
              this.roletypeStatus = data.Status;
              if (data.Status == "ApprovedByCEO") {
                this.acceptedbyCEO = true;
                this.approvedKRA = true;
                this.addNewKraButton = false;
                this.freezeKRAs = true
              }             
              if (data.Status == "ApprovedbyHOD") {
                this.approvedByHOD = true;
                this.approvedKRA = true;
                this.addNewKraButton = false;
                this.freezeKRAs = true
              }
              else if(data.Status == "EditedByHOD" || data.Status == "SentToOpHead" || data.Status == "SendToCEO" ||data.Status == "SentToAssociates"  || data.Status == "SentToHOD"){
                this.addNewKraButton = false
                this.approvedKRA = true;
                this.freezeKRAs = true;
              }
              else if(data.Status == "Draft" || data.Status == ""){
                this.addNewKraButton = true
              }
              // else if (data.Status == "FD" || data.Status == "FinishedEditByHR") this.markedAsFinish = this.checkMarkAsFinished = true;
              // else if (data.Status == "Draft") this.addKra = this.editKRA = this.markasfinish = (this.selectedfromyear == this.currentyear && this.currentyear < this.selectedtoyear) || this.selectedfromyear > this.currentyear;
              // else if (data.Status == "SentToHOD") this.sendtoHOD = true;
              // else if (data.Status == "EditByHR") this.addKra = this.editKRA = this.markasfinish = this.editByHRStatus = true;
            });
            
            if (this.markedAsFinish) this.markasfinish = false;
            if (mode == "edit") {
              setTimeout(() => {
                this._snackBar.open("KRA Updated Successfully", "", {
                  duration: 1000,
                  panelClass: ["success-alert"],
                  horizontalPosition: "right",
                  verticalPosition: "top"
                });
              }, 10);
            } else if (mode == "add") {
              setTimeout(() => {
                this._snackBar.open("KRA Added Successfully", "", {
                  duration: 1000,
                  panelClass: ["success-alert"],
                  horizontalPosition: "right",
                  verticalPosition: "top"
                });
              }, 10);
            }
            else if (mode == "import") {
              setTimeout(() => {
                this._snackBar.open("KRAs Imported Successfully", "", {
                  duration: 1000,
                  panelClass: ["success-alert"],
                  horizontalPosition: "right",
                  verticalPosition: "top"
                });
              }, 10);
            }
            else if (mode == "copy") {
              setTimeout(() => {
                this._snackBar.open("KRAs Copied Successfully", "", {
                  duration: 1000,
                  panelClass: ["success-alert"],
                  horizontalPosition: "right",
                  verticalPosition: "top"
                });
              }, 10);
            }
            
          }
          else{    
            this.length = 0       
            this.addNewKRA = true;
            this.addNewKraButton = true;
            this.dataSource = null;
          }
        },
        (error: any) => {  
          this.spinner.hide();
          this.length=0;
          if(error.error.text==="KRAs not found from Definition table!"){
            this.addNewKraButton = false;
            this.addNewKRA = true;
            this.dataSource = null;
            this._snackBar.open(error.error.text, "", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
          else if(error.error.text == ""){
            this.addNewKraButton = true;
          }
         else if (
            ((error.error == "No KRAs defined!" || error.error.text == "No KRAs defined!") &&
              (this.selectedfromyear == this.currentyear &&
                this.currentyear < this.selectedtoyear)) ||
            this.selectedfromyear > this.currentyear
          ) {
            this.addNewKRA = true;
            this.dataSource = null;
          } else if ((error.error == "No KRAs defined!" || error.error == "No records found!"
          || error.error =="No KRAs found!")||
          (error.error.text == "No KRAs defined!" || error.error.text == "No records found!"
          || error.error.text =="No KRAs found!")) {
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
    }else{
      this.spinner.hide();
    }
  }

  // getApplicableGradeRoleTypesByGrade() {
  //   this.roleTypeList = [];
  //   this.dataSource = null;
  //   this.defineKRAForm.get("roleTypeId").setValue(null);
  //   this.addNewKRA = this.markasfinish = this.approvedKRA = this.markedAsFinish = this.sendtoHOD = this.markedAsFinish = this.markasfinish = false;
  //   this.selectedParameters.RoleTypeName = "";
  //   this.roleTypeList.push({ label: "Select Role Type", value: null });
  //   if(this.defineKRAForm.value.gradeId!==null&&this.departmentId!==null){
  //   this._appRoleTypeService
  //     .getRoleTypesByGrade(        
  //       this.departmentId,        
  //       this.defineKRAForm.value.gradeId
  //     )
  //     .subscribe(
  //       (res: any[]) => {
  //         if (res) {
  //           this.addApplicableRole = false;
  //           res.forEach((element: any) => {
  //             this.roleTypeList.push({
  //               label: element.Name,
  //               value: element.Id
  //             });
  //             if(res.length === 1) {
  //               this.roleTypeId = element.Id;
  //               this.defineKRAForm.get("roleTypeId").setValue(this.roleTypeId);
  //             }
  //           });
  //           if(res.length > 0){
  //             this.showDefineKRAs("");
  //           }
  //         } else {
  //           this.addApplicableRole = true;
  //           this.approvedKRA = this.addNewKRA = this.addKra = this.markasfinish = this.markedAsFinish = false;
  //           this.addNewKraButton = false
  //           this.dataSource = null;
  //           this.resetgraderoletypes();
  //         }
  //       },
  //       (error: any) => {
  //         this._snackBar.open("Failed to get Role Type details.", "", {
  //           duration: 1000,
  //           horizontalPosition: "right",
  //           verticalPosition: "top"
  //         });
  //       }
  //     );
  //   }
  // }

  addAppRoleType() {
    setTimeout(() => {
      this._router.navigate(["/kra/applicableroletypes"]);
    }, 500);
  }

  dispUndo() {
    this.undoDiv = true;

    setTimeout(() => {
      this.undoDiv = false;
      this.undoBtn = true;
      //this.marksAsFinish = true;
    }, 3000);
  }
  dispDelKRA() {
    this.deletedKRAdiv = true;

    setTimeout(() => {
      this.deletedKRAdiv = false;
    }, 3000);
  }

  deleteKRA() {
    this.deletedTr = true;
  }

  dispAddedKRA() {
    this.dispAddedKRADiv = true;

    setTimeout(() => {
      this.dispAddedKRADiv = false;
    }, 3000);
  }

  ContinueEditing() {   
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.defineKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.defineKRAForm.value.roleTypeId;
    if (this.roletypeStatus == "FinishedEditByHR")
      creatingObj.status = "EditByHR";
    else creatingObj.status = "Draft";
    // this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
    //   (res: any[]) => {
    //     if (res) {
    //       this.showDefineKRAs("refresh");
    //       this.markasfinish = true;
    //       this.markedAsFinish = false;
    //       this.checkMarkAsFinished = false;
    //       this.editKRA =
    //         (this.selectedfromyear == this.currentyear &&
    //           this.currentyear < this.selectedtoyear) ||
    //         this.selectedfromyear > this.currentyear;
    //     } else {
    //       this.markasfinish = false;
    //       this.markedAsFinish = true;
    //     }
    //   },
    //   (error: any) => {
    //     this._snackBar.open("Failed to update.", "", {
    //       duration: 1000,
    //       horizontalPosition: "right",
    //       verticalPosition: "top"
    //     });
    //   }
    // );
  }

  markAsFinish() {
    // var creatingObj = new ApplicableRoleType();
    // creatingObj.financialYearId = this.financialYearId;
    // creatingObj.departmentId = this.departmentId;
    // creatingObj.gradeId = this.defineKRAForm.value.gradeId;
    // creatingObj.roleTypeId = this.defineKRAForm.value.roleTypeId;    
    // if (this.roletypeStatus == "EditByHR")
    //   creatingObj.status = "FinishedEditByHR";
    // else creatingObj.status = "FD";
    this._kraDefinitionService.UpdateKRAStatus(this.financialYearId, this.defineKRAForm.value.roleTypeId, "FD").subscribe(
      (res: any[]) => {
        if (res) {
          this.showDefineKRAs("refresh");         
        } else {
          this.markedAsFinish = false;
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  // onPageChanged(e) {
  //     let firstCut = e.pageIndex * e.pageSize;
  //     let secondCut = firstCut + e.pageSize;
  //     this.defineDataSource = this.dataSource.slice(firstCut, secondCut);
  //   }

  public closeComments(): void {
    this.showCommentsDialog = false;
  }

  resetgraderoletypes() {
    this.roleTypeList = [];
    this.gradeList = [];
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    this.gradeList.push({ label: "Select Grade", value: null });
  }

  // changeValue(selectedValue){
  //   if(selectedValue.value==4){
  //     this.openDialog('450px','','Do you have any new departments, grades or role types to be defined?',['No','Yes']);
  //   }
  // }

  changeValue(selectedValue) {
    this.resetgraderoletypes();
  }
  //commented 2011
  // openDialog(dlgwidth, heading, bodydata, footerbtns): void {
  //   const dialogRef = this.dialog.open(KradialogsComponent, {
  //     width: dlgwidth,
  //     data: { dlgHeadingData: heading, dlgBodyData: bodydata, dlgFooterBtns: footerbtns }
  //   });
  //   dialogRef.afterClosed().subscribe(result => {
  //     this.addNewKRA = result;
  //     this.freezedKRA = !result;
  //   });
  // }
 

  openImportKRA(): void {
    if (this.defineKRAForm.valid) {     
        const dialogRef = this.dialog.open(ImportKraDlgComponent, {
          width: "80vw",
          data: {           
            finYearId: this.financialYearId,
            depId: this.departmentId,
            gId: this.defineKRAForm.value.gradeId,
            rId: this.defineKRAForm.value.roleTypeId
          }
        });
        dialogRef.afterClosed().subscribe(
          (res: any) => {         
              if (res == 13) {
              setTimeout(() => {
                this.addKra = false;
                this.showDefineKRAs("import");
              }, 10);
            }
          },
          (error: any) => {
            this._snackBar.open(
              "Failed to get KRA details." + error.error,
              "x",
              {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              }
            );
          }
        );      
    }
  }

  cloneKRA(): void {
    if (this.defineKRAForm.valid) {     
        const dialogRef = this.dialog.open(CloneKraDlgComponent, {
          width: "80vw",
          data: {           
            finYearId: this.financialYearId,
            depId: this.departmentId,
            gId: this.defineKRAForm.value.gradeId,
            rId: this.defineKRAForm.value.roleTypeId
          }
        });
        dialogRef.afterClosed().subscribe(
          (res: any) => {         
              if (res == 13) {
              setTimeout(() => {
                this.addKra = false;
                this.showDefineKRAs("copy");
              }, 10);
            }
          },
          (error: any) => {
            this._snackBar.open(
              "Failed to get KRA details." + error.error,
              "x",
              {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              }
            );
          }
        );      
    }
  }


  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000
    });
  }

  openAddKRA(e): void {
    if (this.defineKRAForm.valid) {
      if (e == "add") {
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: "80vw",
          data: {
            heading: "Add KRA",
            btntext: "Add",
            finYearId: this.financialYearId,
            depId: this.departmentId,
            gradeId: this.defineKRAForm.value.gradeId,
            roleId: this.defineKRAForm.value.roleTypeId,
            editMode: false,
            definitionDetailsId:null,
            definitionDetails: null
          }
        });
        dialogRef.afterClosed().subscribe(
          (res: any) => {
            if (res == 13) {
              setTimeout(() => {
                this.addKra = false;
                this.showDefineKRAs("add");
              }, 10);
            }
          },
          (error: any) => {
            this._snackBar.open(
              "Failed to get KRA details." + error.error,
              "x",
              {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              }
            );
          }
        );
      } else {
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: "80vw",
          data: {
            heading: "Edit KRA",
            btntext: "Update",
            finYearId: this.financialYearId,
            editMode: true,
            depId: this.departmentId,
            gradeId: this.defineKRAForm.value.gradeId,
            roleId: this.defineKRAForm.value.roleTypeId,
            definitionDetailsId:e
          }
        });
        dialogRef.afterClosed().subscribe((res: any) => {
          if (res == 13) {
            this.addKra = false;
            this.showDefineKRAs("edit");
          }
        });
      }
    }
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

  onDeleteKRA(definitionDetailId: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: { message: 'Are you sure you want to delete?' },

    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._kraDefinitionService.DeleteKRA(definitionDetailId).subscribe(
          (res: any) => {
            if (res == true) {
              this.showDefineKRAs("refresh");
              this._snackBar.open("KRA Definition deleted successfully.", "", {
                duration: 1000,
                panelClass: ["success-alert"],
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            } else {
              this._snackBar.open("Failed to delete KRA Definition.", "", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            }
          },
          (error: any) => {
            this._snackBar.open("Failed to delete KRA Definition.", "", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        );
      }
    })
  }

  AcceptTargetValue(e): void {
    this._kraDefinitionService.AcceptTargetValue(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  RejectTargetValue(e): void {
    this._kraDefinitionService.RejectTargetValue(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  AcceptMetricValue(e): void {
    this._kraDefinitionService.AcceptMetricValue(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  RejectMetricValue(e): void {  
    this._kraDefinitionService.RejectMetricValue(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  AcceptDeletedKRAByHOD(e): void { 
    this._kraDefinitionService
      .AcceptDeletedKRAByHOD(e, this.username)
      .subscribe(
        (res: any) => {
          if (res.IsSuccessful == true) {
            this.showDefineKRAs("refresh");
            this._snackBar.open("KRA  updated successfully.", "", {
              duration: 1000,
              panelClass: ["success-alert"],
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            if (res.Message != "") {
              this._snackBar.open("Failed to update KRA." + res.Message, "x", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            } else {
              this._snackBar.open("Failed to update KRA." + res.Message, "x", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            }
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to update KRA.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
  }

  RejectDeletedKRAByHOD(e): void {   
    this._kraDefinitionService
      .RejectDeletedKRAByHOD(e, this.username)
      .subscribe(
        (res: any) => {
          if (res.IsSuccessful == true) {
            this.showDefineKRAs("refresh");
            this._snackBar.open("KRA  updated successfully.", "", {
              duration: 1000,
              panelClass: ["success-alert"],
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            if (res.Message != "") {
              this._snackBar.open("Failed to update KRA." + res.Message, "x", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            } else {
              this._snackBar.open("Failed to update KRA." + res.Message, "x", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            }
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to update KRA.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
  }

  AcceptAddedKRAByHOD(e): void {   
    this._kraDefinitionService.AcceptAddedKRAByHOD(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  RejectAddedKRAByHOD(e): void {    
    this._kraDefinitionService.RejectAddedKRAByHOD(e, this.username).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this._snackBar.open("KRA  updated successfully.", "", {
            duration: 1000,
            panelClass: ["success-alert"],
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        } else {
          if (res.Message != "") {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          } else {
            this._snackBar.open("Failed to update KRA." + res.Message, "x", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top"
            });
          }
        }
      },
      (error: any) => {
        this._snackBar.open("Failed to update KRA.", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  getRoleTypes(){
    this.roleTypeList = [];
    this.dataSource = null;
    this.defineKRAForm.get("roleTypeId").setValue(null);
    this.addNewKRA = this.markasfinish = this.approvedKRA = this.markedAsFinish = this.sendtoHOD = this.markedAsFinish = this.markasfinish = false;
    this.selectedParameters.RoleTypeName = "";
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    if(this.departmentId!==null){
    this._appRoleTypeService
      .getRoleTypesByFinancialYearAndDepartment(this.defineKRAForm.value.financialYearId,this.departmentId)
      .subscribe(
        (res: any[]) => {
          if (res) {
            this.addApplicableRole = false;
            res.forEach((element: any) => {
              this.roleTypeList.push({
                label: element.Name,
                value: element.Id
              });
              if(res.length === 1) {
                this.roleTypeId = element.Id;
                this.defineKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              }
            });
            if(res.length > 0){
              this.showDefineKRAs("");
            }
          } else {
            this.addApplicableRole = true;
            this.approvedKRA = this.addNewKRA = this.addKra = this.markasfinish = this.markedAsFinish = false;
            this.addNewKraButton = false
            this.dataSource = null;
            this.resetgraderoletypes();
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
    }
  }

  getGradesForSelectedRole(){
    this.gradeNames = "";
    
      this._appRoleTypeService
        .getGradesForSelectedRole(this.defineKRAForm.value.financialYearId,this.departmentId,this.defineKRAForm.value.roleTypeId)
        .subscribe(
          (res: any[]) => {
            if (res.length > 0) {
              this.gradeNames = res[0].GradeName
              console.log(this.gradeNames)
            } else {
              this.gradeNames="";
            }
          },
          (error: any) => {
            this.gradeNames="";
          }
        );
      

  }
}
