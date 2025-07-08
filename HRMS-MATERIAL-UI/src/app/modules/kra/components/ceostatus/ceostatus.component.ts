import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren, ViewChild, Input } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { animate, state, style, transition, trigger } from '@angular/animations';
// import { MatTableDataSource, MatTable, MatSort } from "@angular/material/
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import * as servicePath from '../../../../core/service-paths';
import { SelectedKRAParameters } from 'src/app/modules/master-layout/models/kra.model';
import { KraStatusService } from "src/app/modules/kra/services/kraStatus.service";
import { KRAStatusData } from 'src/app/modules/kra/models/krastatus.model';

import {
  MatSnackBar
} from '@angular/material/snack-bar';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSidenav } from '@angular/material/sidenav';
import { CommentModel } from 'src/app/modules/kra/models/comment.model';
import { KraCommentService } from 'src/app/modules/kra/services/kra-comment.service';

export interface DialogData {
  comment: string;
  
}


interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-ceostatus',
  templateUrl: './ceostatus.component.html',
  styleUrls: ['./ceostatus.component.scss'],
  // providers: [
  //   AssociatekraService,
  //   RolemasterService,
  //   MasterDataService,
  //   KRAService,
  //   CustomKRAService
  // ],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})


export class CeostatusComponent implements OnInit {
  // @ViewChild('outerSort') sort: MatSort;
  // @ViewChildren('innerSort') innerSort: QueryList<MatSort>;
  // @ViewChildren('innerTables') innerTables: QueryList<MatTable<any>>;
  @ViewChild('sidenav') sidenav: MatSidenav;
  selectedParameters : SelectedKRAParameters;
  comment: string;
  public financialYearList: SelectItem[] = [];
  undoDiv:boolean=false;
  undoDivs:boolean=true;
  imgdiv:boolean=false;
  undoDivss:boolean=false; 
  ceoStatusForm: FormGroup;
  // submitted = false;
  resources = servicePath.API.PagingConfigValue; 
  formGroup: FormGroup;
  commentForm: FormGroup;
  themeappeareance = themeconfig.formfieldappearances; 
  financialYearId:number;
  departmentId:number;
  deptId:number;
  commentText: string;
  username: string;
  sendtoCEO:boolean;   
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20,30];
  approveByCEO:boolean = false;
  CEODefinitions:KRAStatusData[]=[];


  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['departmentname', 'noofroletypes', 'status'];
  dataSource = new MatTableDataSource<KRAStatusData>(); 

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
      this.dataSource = new MatTableDataSource();
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  constructor( private _snackBar: MatSnackBar,
   public navService: NavService,
   private _masterDataService: MasterDataService ,
   private fb: FormBuilder,
   private _kraCommentService: KraCommentService,
   private _kraStatusService: KraStatusService,
    ) {      
    this.selectedParameters = new SelectedKRAParameters();  
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.username = sessionStorage["mail"];  
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
     }
 
  ngOnInit() {
    this.ceoStatusForm = new FormGroup({
      FinancialYearId: new FormControl(null, [Validators.required]),    
    });  
    this.commentForm = new FormGroup({
      comment: new FormControl('', [Validators.required])
    });
  this.getFinancialYears(); 
}

openComments(departmentId: number)
{
  this.deptId = departmentId;
  this.sidenav.toggle();
  this.getComments();
}

closeSideNav() {
  this.sidenav.close();
}

saveComment(): void{
  if (this.ceoStatusForm.valid && this.commentForm.valid) {
    let commentModel: CommentModel;
    commentModel = new CommentModel();

    commentModel.CommentText = this.commentForm.value.comment;
    commentModel.Username = this.username;
    commentModel.IsCEO = true;
    commentModel.FinancialYearId = this.ceoStatusForm.value.FinancialYearId;
    commentModel.DepartmentId = this.deptId;
    commentModel.GradeId = null;
    commentModel.RoleTypeId = null;
    commentModel.ApplicableRoleTypeId = null;

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
          this._snackBar.open('Failed to create comment.', '', {
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
    this.ceoStatusForm.value.FinancialYearId, this.deptId, 0, 0, true).subscribe(
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
        this._snackBar.open('Failed to get comments', 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
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

  getCEOKRAStatuses() { 
    this.CEODefinitions = []
    this.approveByCEO = false
    this._kraStatusService.getHRKRAStatuses(this.ceoStatusForm.value.FinancialYearId)
      .subscribe(
      (res: KRAStatusData[]) => {
        if (res) {  
          if(res.length > 0){
            // this.approveByCEO = true
            this.approveByCEO= (res.some((item)=>item.Status==='SendToCEO'))
          }
          res.forEach((data: KRAStatusData) => {
            // this.roletypeStatus = data.Status;
            if(data.Status == 'SendToCEO' || data.Status=='ApprovedByCEO'){
              if(data.Status == "ApprovedByCEO"){
                data.Status="Approved By CEO";
               }
               if(data.Status == "SendToCEO"){
                data.Status = "Sent To CEO"
              }
              this.CEODefinitions.push(data)
            }
          })
            this.dataSource = new MatTableDataSource(this.CEODefinitions);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;        
        }
        else this.dataSource = null;
      },
      (error) => {
        this.dataSource = null;
      });
  }

onclick() {
  // this.submitted = true;
  this.undoDiv=true;
  this.undoDivs=false;
  this.undoDivss=true;
    
 setTimeout(() => {
  this.undoDiv=false;
  this.undoDivs=true;
 
  
}, 24000);


}

onCancelclick() {
  // this.submitted = true;
  this.undoDiv=false;
  this.undoDivs=true;
  this.undoDivss=false;
    
 setTimeout(() => {
  this.undoDiv=true;
  this.undoDivs=false;
 
  
}, 24000);

}

onFinancialYearChange(event) {this.getCEOKRAStatuses();}

clickimg() { 
  this.imgdiv=true;  
  setTimeout(() => {
    this.imgdiv=false;
  }, 9000);
  setTimeout(() => {    
    this.undoDivss=false;
  }, 200);
  }

  approvedByCEO(){
    this._kraStatusService.approveByCEO(this.ceoStatusForm.value).subscribe((res)=>{
      this.getCEOKRAStatuses()
      this._snackBar.open('Approved successfully.', '', {
        duration: 1000,
        panelClass:['success-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });

    },
    (error)=>{
      this._snackBar.open('Failed in approving', '', {
        duration: 1000,
        panelClass:['danger-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    })
  }
}

