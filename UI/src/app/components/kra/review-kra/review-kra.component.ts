import { Component, OnInit } from '@angular/core';
import { themeconfig } from 'themeconfig';
import { kradefindata } from '../krajson';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar, MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { AddKRAdlgComponent } from '../add-kradlg/add-kradlg.component';
import { FormControl } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MasterDataService } from '../../../services/masterdata.service';
import { SelectItem, MessageService } from "primeng/components/common/api";
import { DepartmentDetails } from 'src/app/models/role.model';
import { Grade } from '../../admin/models/grade.model';
import { Roletype } from "../../admin/models/roletype.model";
import { FinancialYear, SelectedKRAParameters } from 'src/app/models/kra.model';
import { ApplicableRoleTypeService } from "src/app/components/kra/Services/applicableroletype.service";
import {ActivatedRoute } from '@angular/router';
import { DefineKraService } from "src/app/components/kra/Services/definekra.service";
import { DefineKRAData } from '../models/definekra.model';
import { ApplicableRoleType, ApplicableRoleTypeData } from '../../../models/kraApplicableRoleType.model';
import { environment } from 'src/environments/environment';
import { KraDefinitionService } from '../Services/kradefinition.service';

@Component({
  selector: "app-review-kra",
  templateUrl: "./review-kra.component.html",
  styleUrls: ["./review-kra.component.scss"]
})
export class ReviewKRAComponent implements OnInit {
  themeappeareance = themeconfig.formfieldappearances;
  displayedColumns: string[] = ["kraaspect", "metrics", "ration"];
  selectedParameters: SelectedKRAParameters;
  // dataSource = new MatTableDataSource(kradefindata);
  // kradefinData = kradefindata;
  //  dataSource = this.kradefinData;
  dataSource = null;
  importedKradisplayedColumns: string[] = [
    "select",
    "kraaspect",
    "metrics",
    "ration"
  ];
  selection = new SelectionModel<any>(true, []);
  markedAsFinish: boolean = false;
  approvedKRA: boolean = false;
  addNewKRA: boolean = false;
  freezedKRA: boolean = false;
  importedKRA: boolean = false;
  newAddedRow: boolean = false;
  marksAsFinish: boolean = false;
  reviewKradata: boolean = false;
  undoBtn: boolean = false;
  editKRA: boolean = false;
  undoDiv: boolean = false;
  deletedTr: boolean = false;
  deletedKRAdiv: boolean = false;
  addedTR: boolean = false;
  dispAddedKRADiv: boolean = false;
  private _selectedDepartmentId: string;
  _selectedFinancialYearId: string;
  departmentHeadDepartmentId: number;
  public financialYearList: SelectItem[] = [];
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] = [];
  public roleTypeList: SelectItem[] = [];
  reviewKRAForm: FormGroup;
  public financialYearId: number = 0;
  public departmentId: number = 0;
  public gradeId: number = 0;
  public roleTypeId: number = 0;
  // financialYearName: string;
  // selected: string;

  constructor(
    public dialog: MatDialog,
    public snackBar: MatSnackBar,
    private messageService: MessageService,
    private _masterDataService: MasterDataService,
    private _definekraService: DefineKraService,
    private fb: FormBuilder,
    private _appRoleTypeService: ApplicableRoleTypeService,
    private _kraDefinitionService: KraDefinitionService,
    private actRoute: ActivatedRoute
  ) {
    this.selectedParameters = new SelectedKRAParameters();
  }

  ngOnInit() {
    this.reviewKRAForm = this.fb.group({
      financialYearId: new FormControl(null),
      departmentId: new FormControl(null),
      gradeId: new FormControl(null),
      roleTypeId: new FormControl(null)
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
      this.departmentHeadDepartmentId = environment.departmentHeadDepartmentId;
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
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Financial Year List"
        });
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
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Department Details"
        });
      }
    );
  }

  private onchange(event) {
    this.financialYearId = event.value;
    if (this._selectedFinancialYearId) {
      this.getApplicableGradeRoleTypes();
    }
  }
  private ondepartmentchange(event) {
    this.departmentId = event.value;
    this.getApplicableGradeRoleTypes();
  }
  private getApplicableGradeRoleTypes() {
    //this.resetgraderoletypes();
    this.gradeList = [];
    this._appRoleTypeService
      .getApplicableRoleTypeById(
        this.financialYearId,
        this.departmentId,
        null,
        null
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            //this.addApplicableRole = false;
            //this.gradeList.push({ label: "Select Grade", value: null })
            res.forEach((element: any) => {
              this.gradeList.push({
                label: element.Grade,
                value: element.GradeId
              });
              this.gradeId = element.GradeId;
            });
            if (this.gradeId > 0) {
              this.reviewKRAForm.get("gradeId").setValue(this.gradeId);
              this.getApplicableGradeRoleTypesByGrade();
            }
          }
        },
        (error: any) => {
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: "Failed to get Role Type details."
          });
        }
      );
  }

  // private getappGradeRoleTypesByGrade(event) {
  //   this.gradeId = event.value;
  //   this.roleTypeId = 0;
  //   this.getApplicableGradeRoleTypesByGrade();
  // }

  private getApplicableGradeRoleTypesByGrade() {
    this.roleTypeList = [];
    this.selectedParameters.RoleTypeName = "";
    //this.roleTypeList.push({ label: "Select Role Type", value: null })
    this._appRoleTypeService
      .getApplicableRoleTypeById(
        this.financialYearId,
        this.departmentId,
        null,
        this.reviewKRAForm.value.gradeId
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((element: any) => {
              this.roleTypeList.push({
                label: element.RoleTypeName,
                value: element.RoleTypeId
              });
              this.roleTypeId = element.RoleTypeId;
            });
            if (this.roleTypeId > 0) {
              this.reviewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              this.showDefineKRAs("");
            }
          }
        },
        (error: any) => {
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: "Failed to get Role Type details."
          });
        }
      );
  }
  private EDITKRAS() {
    this.editKRA = this.addNewKRA = true;
  }

  private markAsFinish() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.reviewKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.reviewKRAForm.value.roleTypeId;
    creatingObj.status = "FinishedEditByHOD";
    this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
      (res: any[]) => {
        if (res) {
          this.markedAsFinish = true;
          this.editKRA = this.addNewKRA = this.marksAsFinish = false;
        } else {
          this.markedAsFinish = false;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to update."
        });
      }
    );
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
                this.approvedKRA = false;
                // this.kradefinData.push(result);
                // this.dataSource.push(result);
                // this.dataSource = this.kradefinData;
                // this.dataSource = new MatTableDataSource(this.kradefinData);
                this.showDefineKRAs("add");
              }, 10);
            }
          },
          (error: any) => {
            this.messageService.add({
              severity: "error",
              summary: "Error Message",
              detail: "Failed to get KRA details." + error.error
            });
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
            IsHOD: true,
            depId: this.departmentId,
            gradeId: this.reviewKRAForm.value.gradeId,
            roleId: this.reviewKRAForm.value.roleTypeId,
            definitionDetailsId: e
          }
        });
        dialogRef.afterClosed().subscribe((res: any) => {
          if (res == 13) {
            this.addNewKRA = false;
            this.addedTR = true;
            this.marksAsFinish = true;
            this.approvedKRA = false;
            this.showDefineKRAs("edit");
          }
        });
      }
    }
  }

  private ContinueEditing() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.reviewKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.reviewKRAForm.value.roleTypeId;
    creatingObj.status = "SentToHOD";
    this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
      (res: any[]) => {
        if (res) {
          this.marksAsFinish = this.addNewKRA = this.editKRA = false;
          this.showDefineKRAs("");
          // this.marksAsFinish = true;
          // this.markedAsFinish = false;
          // this.checkMarkAsFinished=false;
          //this.editKra = (this.selectedfromyear == this.currentyear  && this.currentyear<this.selectedtoyear)||(this.selectedfromyear>this.currentyear);
        } else {
          this.marksAsFinish = false;
          this.markedAsFinish = true;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to update."
        });
      }
    );
  }

  private ApproveKRA() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.reviewKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.reviewKRAForm.value.roleTypeId;
    creatingObj.status = "ApprovedbyHOD";
    this._appRoleTypeService.updateRoleTypeStatus(creatingObj).subscribe(
      (res: any[]) => {
        if (res) {
          this.approvedKRA = true;
          this.addNewKRA = this.editKRA = false;
        } else {
          this.markedAsFinish = false;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to update."
        });
      }
    );
  }
  private showDefineKRAs(mode: string) {
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
            //this.length = res.length;
            this.dataSource = res;
            // this.marksAsFinish = false;
            this.reviewKradata = true;

            // this.defineDataSource = this.dataSource.slice(0,this.pageSize);
            this.dataSource.forEach((data: DefineKRAData) => {
              this.approvedKRA = data.Status == "ApprovedbyHOD";
              this.markedAsFinish = data.Status == "FinishedEditByHOD";
              this.marksAsFinish = data.Status == "EditedByHOD";
              if (
                data.AspectName == "Client Delivery" &&
                data.previousration != null
              ) {
                this.undoBtn = true;
              } else this.undoBtn = false;
            });
            // this.freezedKRA = true;
            // if(this.markedAsFinish)  this.markasfinish=false;
            if (mode == "edit") {
              setTimeout(() => {
                this.messageService.add({
                  severity: "success",
                  summary: "Success Message",
                  detail: "KRA Updated Successfully"
                });
              }, 10);
            } else if (mode == "add") {
              setTimeout(() => {
                this.messageService.add({
                  severity: "success",
                  summary: "Success Message",
                  detail: "KRA Added Successfully"
                });
              }, 10);
            }
          }
        },
        (error: any) => {
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: error.error
          });
        }
      );
  }

  onDeleteKRA(definitionDetailId: number) {
    this._kraDefinitionService.DeleteKRAByHOD(definitionDetailId).subscribe(
      (res: any) => {
        if (res.IsSuccessful == true) {
          this.showDefineKRAs("refresh");
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "KRA Definition deleted successfully."
          });
        } else {
          if (res.Message != "") {
            this.messageService.add({
              severity: "error",
              summary: "Error Message",
              detail: "Failed to delete KRA Definition." + res.Message
            });
          } else {
            this.messageService.add({
              severity: "error",
              summary: "Error Message",
              detail: "Failed to delete KRA Definition." + res.Message
            });
          }
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to delete KRA Definition."
        });
      }
    );
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
}
