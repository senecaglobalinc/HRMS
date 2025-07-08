import { Component, OnInit, Inject,EventEmitter, Output, ViewChild  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';import { AssociateAllocation, PercentageDropDown, RoleDetails } from '../../../master-layout/models/associateallocation.model';
import * as moment from 'moment';
import { AssociateAllocationService } from '../../services/associate-allocation.service';
import { DropDownType, GenericType } from '../../../master-layout/models/dropdowntype.model';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { ClientBillingRole, InternalBillingRole } from '../../../master-layout/models/associateallocation.model'
import { TemporaryAllocationReleaseService } from '../../services/temporary-allocation-release.service';
import { themeconfig } from 'src/themeconfig';
import { CommonService } from 'src/app/core/services/common.service';
import { InternalClient } from 'src/app/modules/master-layout/utility/enums';
import { FormGroup } from '@angular/forms';
import { MasterDataService } from 'src/app/modules/master-layout/services/masterdata.service';
import { TileStyler } from '@angular/material/grid-list/tile-styler';
import { AssignManagerToProjectService } from 'src/app/modules/project-life-cycle/services/assign-manager-to-project.service';
import { TagAssociateService } from 'src/app/modules/project-life-cycle/services/tag-associate.service';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-dialog-cbr',
  templateUrl: './dialog-cbr.component.html',
  styleUrls: ['./dialog-cbr.component.scss']
})


export class DialogCBRComponent implements OnInit {
  formsubmitted: boolean ;
  allocationHistory: AssociateAllocation[] = [];
  
  AllocationOptions: PercentageDropDown[] = [];
  
  displaySelectProject: boolean = false;
  
  select: boolean;
  clientBillingRoleData: MatTableDataSource<ClientBillingRole[]>;
  isProjectSelected: boolean;
  isAllSelected: boolean;
  selectedProjectId: number;  
  cbrList: any;
  cbrListAvailablePostions : any;
  checked : boolean;
  isApplyButton : boolean = false;
  // favoriteSeason: string;
  private selectedCBRInfo: any;

// @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;


  displayedColumns: string[] = ['checked','ClientBillingRoleName','ClientBillingPercentage',
  'NoOfPositions','StartDate','AllocationCount'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('table') table: MatTable<any>;
  @Output() messageEvent = new EventEmitter<any>();


  constructor(
    public dialogRef: MatDialogRef<DialogCBRComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private masterDataService: MasterDataService,
    private _service: AssociateAllocationService,
    private tagAssociateService: TagAssociateService,
    private _commonService: CommonService,
    private _tempAllocationRelease: TemporaryAllocationReleaseService,
    private snackBar: MatSnackBar,
    private _assignManagerservice: AssignManagerToProjectService) {
      this.isProjectSelected = this.data.isProjectSelected;
      this.isAllSelected = this.data.isAllSelected;
      this.selectedProjectId = this.data.selectedProjectId;  
    }

  ngOnInit(){
    this.clientBillingRoleData  = new MatTableDataSource(this.data.cbrList);
      this.clientBillingRoleData.paginator = this.paginator;
      this.clientBillingRoleData.sort = this.sort;
      if (this.data.cbrList.length > 0){
          this.isApplyButton = false;
        if (this.data.cbrList.length == 1 && this.data.cbrList[0].AvailablePositions == 0)
          this.isApplyButton = true; 
        
        else if (this.data.cbrList.length > 1){
          this.cbrListAvailablePostions = this.data.cbrList.filter(item => item.AvailablePositions == 0)
          if (this.cbrListAvailablePostions.length == this.data.cbrList.length)
            this.isApplyButton = true;
          else
            this.isApplyButton = false;  
        }  

      }
      else{
        this.isApplyButton = true;
      }
  }

  /// changeevent row
  onNoClick(): void {
    this.dialogRef.close();
  }
 
  selectedCBR() {
    if(this.selectedCBRInfo != undefined || this.selectedCBRInfo != null)
      this.dialogRef.close(this.selectedCBRInfo);
    else{
      this.snackBar.open('Please select Client Billing Role', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }  
  }
  close() {
    this.displaySelectProject = false;
  }
  clientbillingRoleSelected(clientbillingroleinfo: any){
    this.selectedCBRInfo = clientbillingroleinfo;
  }
}