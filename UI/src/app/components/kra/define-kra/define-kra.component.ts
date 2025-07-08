import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { themeconfig } from 'themeconfig';
import { kradefindata } from '../krajson';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { SelectItem, MessageService } from "primeng/components/common/api";
import { KradialogsComponent } from '../kradialogs/kradialogs.component';
import { ImportKraDlgComponent } from '../import-kra-dlg/import-kra-dlg.component';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material';
import { AddKRAdlgComponent } from '../add-kradlg/add-kradlg.component';
import { GenericType } from "../../../models/dropdowntype.model";
import { ApplicableRoleTypeService } from "src/app/components/kra/Services/applicableroletype.service";
import { MasterDataService } from '../../../services/masterdata.service';
import { DepartmentDetails } from 'src/app/models/role.model';
import { Grade } from '../../admin/models/grade.model';
import { Roletype } from "../../admin/models/roletype.model";
import { element } from 'protractor';
import { FinancialYear, SelectedKRAParameters } from 'src/app/models/kra.model';
import { RoleTypeService } from '../../admin/services/role-type.service';
import { ApplicableRoleType, ApplicableRoleTypeData } from '../../../models/kraApplicableRoleType.model';
import { ActivatedRoute, Router } from "@angular/router";
import { DefineKraService } from "src/app/components/kra/Services/definekra.service";
import { DefineKRAData } from '../models/definekra.model';
import { KraDefinitionService } from '../Services/kradefinition.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { tap } from 'rxjs/operators';
import * as servicePath from '../../../service-paths';
//import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-define-kra',
  templateUrl: './define-kra.component.html',
  styleUrls: ['./define-kra.component.scss']
})

export class DefineKRAComponent implements OnInit {
  selectedParameters: SelectedKRAParameters;
  themeappeareance = themeconfig.formfieldappearances;
  displayedColumns: string[] = ['kraaspect', 'metrics', 'ration'];
  dataSource;
  roletypedata: any;
  importedKradisplayedColumns: string[] = ['select', 'kraaspect', 'metrics', 'ration'];
  selection = new SelectionModel<any>(true, []);
  addNewKRA: boolean = false;
  freezedKRA: boolean = false;
  importedKRA: boolean = false;
  showCommentsDialog: boolean = false;
  statusId: number;
  gridMessage: string = "No records found";
  defineKRAForm: FormGroup;
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
  undoDiv=false;
  editKra = false;
  markasfinish = false;
  markedAsFinish=false;
  checkMarkAsFinished=false;
  sendtoHOD=false;
  currentyear = new Date().getFullYear();
  selectedfromyear: number;
  selectedtoyear: number;
  defineDataSource = [];
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20,30];
  @ViewChild(MatPaginator) paginator: MatPaginator;
// End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;

  constructor(public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    private messageService: MessageService,
    private _roleTypeService: RoleTypeService, private fb: FormBuilder,
    private _appRoleTypeService: ApplicableRoleTypeService, private _router: Router,
    private _kraDefinitionService: KraDefinitionService,
    private _definekraService: DefineKraService,  private actRoute: ActivatedRoute)
    {
      this.selectedParameters = new SelectedKRAParameters();  
       this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
    }
  ngOnInit() 
  {
      this.defineKRAForm = this.fb.group({
      financialYearId: [null],
      departmentId: [null],
      gradeId: [null],
      roleTypeId: [null],
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
          this.defineKRAForm
            .get("financialYearId")
            .setValue(this.financialYearId);
          this.defineKRAForm.controls["financialYearId"].disable();
        } else this.defineKRAForm.controls["financialYearId"].enable();
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
          this.defineKRAForm.get("departmentId").setValue(this.departmentId);
          this.defineKRAForm.controls["departmentId"].disable();
          this.getApplicableGradeRoleTypes();
        } else this.defineKRAForm.controls["departmentId"].enable();
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
   let target = event.source.selected._element.nativeElement;
   this.selectedfromyear = parseInt(target.innerText.split('-')[0].trim());
   this.selectedtoyear = parseInt(target.innerText.split('-')[1].trim());  
   this.financialYearId=event.value;   

   //For currentyear checking
   this.editKra = (this.selectedfromyear == this.currentyear  && this.currentyear<this.selectedtoyear)||(this.selectedfromyear>this.currentyear);
   this.freezedKRA =  this.addKra=this.addNewKRA=this.markasfinish=this.markedAsFinish=this.sendtoHOD= false;   
   this.dataSource = null;
   this.selectedParameters.DepartmentName = '';
  if (this._selectedFinancialYearId) {
    this.getApplicableGradeRoleTypes();}
  }
   private ondepartmentchange(event) {
   this.departmentId=event.value;
    this.getApplicableGradeRoleTypes();
  }
  private getApplicableGradeRoleTypes() {    
    this.resetgraderoletypes();   
    this.gradeList = [];  
    this._appRoleTypeService.getApplicableRoleTypeById(this.financialYearId,
      this.departmentId, null,null)
      .subscribe(
      (res: any[]) => {
        if (res) {       
          this.addApplicableRole = false;   
          this.gradeList.push({ label: "Select Grade", value: null })      
          res.forEach((element: any) => {            
            this.gradeList.push({
              label: element.Grade,
              value: element.GradeId
            });           
             this.gradeId = element.GradeId;
          });     
          this.dataSource = null;
          this.freezedKRA =this.addKra=this.addNewKRA=this.sendtoHOD= this.markedAsFinish=this.markasfinish=false;          
          this.selectedParameters.GradeName = '';
          this.selectedParameters.RoleTypeName = '';
          if (this.gradeId > 0)
          {
            this.defineKRAForm.get("gradeId").setValue(this.gradeId);
            this.getApplicableGradeRoleTypesByGrade();
          }
        }
        else {
          this.addApplicableRole = true;        
          this.addNewKRA =this.addKra =  this.freezedKRA=this.sendtoHOD=
          this.markedAsFinish=this.markasfinish= false;        
          this.dataSource = null;
          this.resetgraderoletypes();
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get Role Type details.'
        });
      }
      );
  }
private setDefaultValues()
{
  this.markedAsFinish = this.checkMarkAsFinished=this.addNewKRA=false; 
  this.markasfinish = this.sendtoHOD = this.editKra =this.addKra = false;
   this.addApplicableRole = this.freezedKRA =  false;
}
  private showDefineKRAs(mode: string) {
    this.setDefaultValues();
    this._definekraService.getKRADefinitionsById(this.financialYearId,
      this.departmentId, this.defineKRAForm.value.gradeId,
      this.defineKRAForm.value.roleTypeId)
      .subscribe(
      (res: DefineKRAData[]) => {        
        if (res) {    
           this.length = res.length;     
          this.dataSource = res;
          this.defineDataSource = this.dataSource.slice(0,this.pageSize);
          this.dataSource.forEach((data: DefineKRAData) => {                  
            if(data.Status=="FD")
             this.markedAsFinish=this.checkMarkAsFinished=true;  
            else if (data.Status == "Draft")  
              this.addKra = this.editKra = this.markasfinish = (this.selectedfromyear == this.currentyear && this.currentyear < this.selectedtoyear) || (this.selectedfromyear > this.currentyear);
             else if (data.Status == "SentToHOD") this.sendtoHOD=true;              
             if (data.AspectName == "Client Delivery" && data.previousration != null) {
               this.undoDiv = true;
             } else this.undoDiv = false;        
          });
          this.freezedKRA = true;            
          if(this.markedAsFinish)  this.markasfinish=false;       
          if (mode == "edit") {
            setTimeout(() => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success Message',
                detail: 'KRA Updated Successfully'
              });
            }, 10);
          }
          else if (mode == "add") {
            setTimeout(() => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success Message',
                detail: 'KRA Added Successfully'
              });
            }, 10);
          }
        }       
      },
      (error: any) => {        
        if(error.statusText== "Not Found" && (this.selectedfromyear == this.currentyear  && this.currentyear<this.selectedtoyear)||(this.selectedfromyear>this.currentyear))
        {                  
          this.addNewKRA = true;        
          this.dataSource = null;       
        }
        else if(error.statusText== "Not Found")
        { 
          this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'No Records Found!.'
        });
        }
        else{
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get Define KRA details.'+error.error
        });
        }
      }
      );
  }

private getApplicableGradeRoleTypesByGrade() {   
    this.roleTypeList = [];
    this.dataSource = null;   
    this.addNewKRA =this.markasfinish= this.freezedKRA =this.markedAsFinish=this.sendtoHOD=
    this.markedAsFinish=this.markasfinish= false;
    this.selectedParameters.RoleTypeName = '';
    this.roleTypeList.push({ label: "Select Role Type", value: null })
    this._appRoleTypeService.getApplicableRoleTypeById(this.financialYearId,
      this.departmentId, null,this.defineKRAForm.value.gradeId)
      .subscribe(
      (res: any[]) => {
        if (res) {       
          this.addApplicableRole = false;         
          res.forEach((element: any) => {
            this.roleTypeList.push({
              label: element.RoleTypeName,
              value: element.RoleTypeId
            });    this.roleTypeId = element.RoleTypeId;      
          });
          if (this.roleTypeId > 0) {
              this.defineKRAForm.get("roleTypeId").setValue(this.roleTypeId);
              this.showDefineKRAs("");
            }
        }
        else {
          this.addApplicableRole = true;
          this.freezedKRA =this.addNewKRA= this.addKra=this.markasfinish=this.markedAsFinish= false;          
          this.dataSource = null;
          this.resetgraderoletypes();
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get Role Type details.'
        });
      }
      );
  }
  
  private addAppRoleType() {
    setTimeout(() => {
      this._router.navigate(['/kra/applicableroletypes']);
    }, 500);
  }  

  private ContinueEditing() {
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.defineKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.defineKRAForm.value.roleTypeId;
    creatingObj.status = "Draft";
    this._appRoleTypeService.updateRoleTypeStatus(creatingObj)
      .subscribe(
      (res: any[]) => {
        if (res) {
          this.markasfinish = true;
          this.markedAsFinish = false;
          this.checkMarkAsFinished=false;
          this.editKra = (this.selectedfromyear == this.currentyear  && this.currentyear<this.selectedtoyear)||(this.selectedfromyear>this.currentyear);
        }
        else {
          this.markasfinish = false;
          this.markedAsFinish = true;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to update.'
        });
      }
      );
  } 

  private markAsFinish() {  
    var creatingObj = new ApplicableRoleType();
    creatingObj.financialYearId = this.financialYearId;
    creatingObj.departmentId = this.departmentId;
    creatingObj.gradeId = this.defineKRAForm.value.gradeId;
    creatingObj.roleTypeId = this.defineKRAForm.value.roleTypeId;
    creatingObj.status = "FD";
    this._appRoleTypeService.updateRoleTypeStatus(creatingObj)
      .subscribe(
      (res: any[]) => {
        if (res) {         
          this.markedAsFinish = true;
          this.markasfinish = false;
          this.checkMarkAsFinished=true;
          this.editKra=false;
        }
        else {
          this.markedAsFinish = false;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to update.'
        });
      }
      );
  }  

onPageChanged(e) {
    let firstCut = e.pageIndex * e.pageSize;
    let secondCut = firstCut + e.pageSize;
    this.defineDataSource = this.dataSource.slice(firstCut, secondCut);
  }

  public closeComments(): void {
    this.showCommentsDialog = false;
  }

  resetgraderoletypes() {
    this.roleTypeList = [];
    this.gradeList = [];     
     this.roleTypeList.push({ label: "Select Role Type", value: null })
     this.gradeList.push({ label: "Select Grade", value: null })
  }

  // changeValue(selectedValue){
  //   if(selectedValue.value==4){
  //     this.openDialog('450px','','Do you have any new departments, grades or role types to be defined?',['No','Yes']);
  //   }
  // }

  changeValue(selectedValue) { this.resetgraderoletypes(); }

  openDialog(dlgwidth, heading, bodydata, footerbtns): void {
    const dialogRef = this.dialog.open(KradialogsComponent, {
      width: dlgwidth,
      data: { dlgHeadingData: heading, dlgBodyData: bodydata, dlgFooterBtns: footerbtns }
    });
    dialogRef.afterClosed().subscribe(result => {      
      this.addNewKRA = result;
      this.freezedKRA = !result;
    });
  }

  openImportKRA(): void {
    const dialogRef = this.dialog.open(ImportKraDlgComponent, {
      data: "Import Kra Data"
    });
    dialogRef.afterClosed().subscribe(result => {
      this.importedKRA = result;
      this.addNewKRA = !result;
      this.freezedKRA = !result;
      this.openSnackBar("KRA's imported successfully!", "");
    });
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000,
    });
  }

  openAddKRA(e): void {   
    if (this.defineKRAForm.valid) {
      if (e == 'add') {       
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: '80vw',
          data: { heading: 'Add KRA', btntext: 'Add', 
          finYearId: this.financialYearId, 
          depId: this.departmentId, 
          gradeId: this.defineKRAForm.value.gradeId, 
          roleId: this.defineKRAForm.value.roleTypeId,
          editMode:false,
          definitionDetailsId:null }
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
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get KRA details.'+error.error
        });
      }
      );
      }
      else {       
        const dialogRef = this.dialog.open(AddKRAdlgComponent, {
          width: '80vw',
          data: { heading: 'Edit KRA', btntext: 'Update' ,
          finYearId: this.financialYearId, 
          editMode:true,
          depId: this.departmentId, 
          gradeId: this.defineKRAForm.value.gradeId, 
          roleId: this.defineKRAForm.value.roleTypeId,
          definitionDetailsId:e},
        });
        dialogRef.afterClosed().subscribe( (res: any) => {         
          if (res == 13) {
            this.addKra = false;
            this.showDefineKRAs("edit");
          }
      });
      }
    }
  }

  onDeleteKRA(definitionDetailId: number) {
      this._kraDefinitionService.DeleteKRA(definitionDetailId).subscribe(
        (res: any) => {
            if (res == true) {
              this.showDefineKRAs("refresh");
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRA Definition deleted successfully.' });
            } else {
              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to delete KRA Definition.' });
            }            
        },
        (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to delete KRA Definition.' });
        });
  }
}
