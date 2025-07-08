import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren, ViewChild, Input } from "@angular/core";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { FinancialYear, SelectedKRAParameters } from 'src/app/modules/master-layout/models/kra.model';
import { KraStatusService } from "src/app/modules/kra/services/kraStatus.service";
import { KRAStatusData } from 'src/app/modules/kra/models/krastatus.model';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from "@angular/router";
import * as servicePath from '../../../../core/service-paths';
import { ConfirmationDialogComponent } from 'src/app/confirmation-dialog/confirmation-dialog.component'
import { NgxSpinnerService } from 'ngx-spinner';

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
import { DepartmentDetails } from "src/app/modules/master-layout/models/role.model";
import { MatDialog } from "@angular/material/dialog";
import { KraWorkFlow } from "../../models/kra-work-flow.model";
import { KraDefinitionService } from 'src/app/modules/kra/services/kradefinition.service';
import { MenuListItemComponent } from "src/app/modules/master-layout/components/menu-list-item/menu-list-item.component";

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-kra-status',
  templateUrl: './kra-status.component.html',
  styleUrls: ['./kra-status.component.scss']
})
export class KraStatusComponent implements OnInit {
  themeappeareance = themeconfig.formfieldappearances;
  financialYearId:number;
  departmentId:number;
  loggedInEmployeeId:number;
  loggedinUserRole:string;
  sendtoCEO:boolean;
   _departmentHeadDepartmentId:number; 
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20,30];
  dataSource = new MatTableDataSource<KRAStatusData>();  
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  resources = servicePath.API.PagingConfigValue;
  public financialYearList: SelectItem[] = [];
  KRAStatusForm: FormGroup;  
  sendToHodReqObj:any={};
  showSendToCEO:boolean=false;

  constructor(private _masterDataService: MasterDataService, private fb: FormBuilder,
    public dialog: MatDialog,
  private _kraStatusService: KraStatusService,  private _snackBar: MatSnackBar,
    private _router: Router, private spinner: NgxSpinnerService,
    private _kraDefinitionService:KraDefinitionService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;   
  }

   cols = [
    { field: 'departmentname', header: 'Department Name' },
    { field: 'noofroletypes', header: ' NO OF ROLE TYPES' },
    { field: 'status', header: 'STATUS' },  
     { field: 'action', header: 'ACTION' },  
  ];
  displayedColumns: string[] = [
    'departmentname',
    'noofroletypes',
    'status',
    'action',
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
     this.loggedInEmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
     this.loggedinUserRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;   
   if(this.loggedinUserRole.indexOf('CEO') != -1)
    this._router.navigate(['/kra/ceostatus/']);
     if(this.loggedinUserRole.indexOf('Department Head') != -1) this._departmentHeadDepartmentId =this.loggedInEmployeeId;
    this.KRAStatusForm = this.fb.group({
      Id: [null],
    });
    this.getFinancialYears();
    this.sendtoCEO=false;
  }
  
  private getFinancialYears(): void {
    this._masterDataService.GetFinancialYears().subscribe((yearsdata: any[]) => {
      this.financialYearList = [];
      this.financialYearList.push({ label: 'Select Financial Year', value: null });
      yearsdata.forEach((e: any) => {
        this.financialYearList.push({ label: e.FinancialYearName, value: e.Id });
      });
    }, (error: any) => {
          this._snackBar.open('Failed to get Financial Year details.', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });        
        }
    );
  }

   onchange() { 
    if(this.loggedinUserRole.indexOf('Department Head') != -1)
    {
    this._departmentHeadDepartmentId =this.loggedInEmployeeId;
    this.getHODKRAStatuses(); 
    }
    else if(this.loggedinUserRole.indexOf('Operation Head') != -1) this.getHRKRAStatuses();
  }

   getHRKRAStatuses() {   
    this.spinner.show();
    this._kraStatusService
      .getHRKRAStatuses(this.KRAStatusForm.value.Id)
      .subscribe(
        (res: KRAStatusData[]) => {
          this.spinner.hide();
          if (res) {          
            this.length = res.length;
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            this.showSendToCEO= (res.some((item)=>item.Status==='SentToOpHead'))&&
             !(res.some((item)=>item.Status==='SentToHOD')) &&
             !(res.some((item)=>item.Status==='ApprovedbyHOD')) 
             && !(res.some((item)=>item.Status==='EditedByHOD'))
            res.forEach((data: KRAStatusData) => {                  
              if(data.Status=="SentToOpHead")
             {
               data.Status="Sent To Operations Head";      
             }
             else if(data.Status == "SentToHOD"){
              data.Status="Sent To HOD";  
             }                 
             else if(data.Status == "ApprovedbyHOD"){
              data.Status="Approved By HOD";
             }
             else if(data.Status == "EditedByHOD"){
               data.Status="Edited By HOD";
             }
             else if(data.Status == "SendToCEO"){
               data.Status = "Sent To CEO"
             }
             else if(data.Status == "ApprovedByCEO"){
              data.Status="Approved By CEO";
             }
            });   
          } else this.dataSource = null;
        },
        error => {
          this.dataSource = null;
         (error: any) => {
        this._snackBar.open('Failed to Send.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });       
      }
        }
      );
  }

  getHODKRAStatuses() {     
    this._kraStatusService.getHODKRAStatuses(this.KRAStatusForm.value.Id,this._departmentHeadDepartmentId)
      .subscribe(
      (res: KRAStatusData[]) => {       
        if (res) {
          this.length = res.length;
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;  
          res.forEach((data: KRAStatusData) => {                  
            if(data.Status=="KRA Recived from Head HR")
           {
             data.Status="KRAs Recived from Head HR";      
           }                 
          });
        }      
      },
      (error) => {      
        this.dataSource = null;
        if(error.error.text!='')
        {
         this._snackBar.open(error.error.text, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else{
        this._snackBar.open(error.error, 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      });
  }

    SendToCEO(): void {
    this._kraStatusService.SendToCEO
      (this.KRAStatusForm.value.Id).subscribe(res => {
        this.sendtoCEO=false;
        this.getHRKRAStatuses();
        this._snackBar.open('Notification Sent to CEO Successfully.', '', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }); 
      },
      (error: any) => {
        this._snackBar.open('Failed to Send.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });       
      });
  }


  //  SendToHOD(data: KRAStatusData): void {    
  //   this._kraStatusService.UpdateKRAStatus
  //     (this.KRAStatusForm.value.Id, data.DepartmentId,true).subscribe(res => {
  //       this.getHRKRAStatuses();      
  //        this._snackBar.open('Notification Sent to HOD Successfully.', '', {
  //           duration: 1000,
  //           panelClass:['success-alert'],
  //           horizontalPosition: 'right',
  //           verticalPosition: 'top',
  //         }); 
  //     },
  //     (error: any) => {
  //       this._snackBar.open('Failed to Send.', '', {
  //         duration: 1000,
  //         horizontalPosition: 'right',
  //         verticalPosition: 'top',
  //       });       
  //     });
  // }

  StartHODReview(data: KRAStatusData): void { 
        this._router.navigate(['/kra/krareview/' + this.KRAStatusForm.value.Id + '/' + data.DepartmentId]);
  }

   StartHRReview(data: KRAStatusData): void { 
     this.EditByHR(data);       
  }

  //    SendToHR(data: KRAStatusData): void {    
  //   this._kraStatusService.UpdateKRAStatus
  //     (this.KRAStatusForm.value.Id, data.DepartmentId, false).subscribe(res => {
  //       this.getHODKRAStatuses();
  //       this._snackBar.open('Notification Sent to HR Successfully.', '', {
  //           duration: 1000,
  //           panelClass:['success-alert'],
  //           horizontalPosition: 'right',
  //           verticalPosition: 'top',
  //         }); 
  //     },
  //    (error: any) => {
  //       this._snackBar.open('Failed to Send.', '', {
  //         duration: 1000,
  //         horizontalPosition: 'right',
  //         verticalPosition: 'top',
  //       });       
  //     });
  // }

    EditByHR(data: KRAStatusData): void {     
    this._kraStatusService.EditByHR
      (this.KRAStatusForm.value.Id, data.DepartmentId).subscribe(res => {
         this._router.navigate(['/kra/kradefine/' + this.KRAStatusForm.value.Id + '/' + data.DepartmentId]);
      },
     (error: any) => {
        this._snackBar.open('Failed to Send.', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });       
      });
  }
  sendToHod(data: KRAStatusData){

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        message: data.IsEligilbeForReivew? 'Are you sure want to send?' : 'All the kras not defined are sure want to proceed' ,
        buttonText: {
          ok: 'Yes',
          cancel: 'No',
        },
      },
    });
    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
       this.sendToHodReqObj={
        financialYearId : this.KRAStatusForm.value.Id,
        departmentId : data.DepartmentId
       } 
        this._kraStatusService.UpdateKRAStatus
      (this.sendToHodReqObj,true).subscribe(res => {
        this.getHRKRAStatuses();      
         this._snackBar.open('Notification Sent to HOD Successfully.', '', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }); 
      },
      // (error: any) => {
      //   this._snackBar.open('Failed to Send.', '', {
      //     duration: 1000,
      //     horizontalPosition: 'right',
      //     verticalPosition: 'top',
      //   });       
      // }
      );
      }
    });

  }

  SendtoCEO(){
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
        approvalObj.financialYearId = this.KRAStatusForm.value.Id;
        // approvalObj.departmentId = this.departmentId;
        // approvalObj.gradeRoleTypeIds.push(this.reviewKRAForm.value.roleTypeId)
        this._kraDefinitionService.SendToCEO(approvalObj).subscribe((res) => {
         // this.sentToHR = true
         if(res){
           this.getHRKRAStatuses()
          this._snackBar.open("Notification Sent successfully.", '', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
         }
         
        },
          (error => {
            this._snackBar.open(error.error, '', {
              duration: 1000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }))
      }
    })
  }
}

