import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren, ViewChild, Input } from "@angular/core";
import { themeconfig } from "themeconfig";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource, MatTable, MatSort } from "@angular/material";
import { krastutusdata } from "../krajson";
import { FormBuilder, FormGroup } from '@angular/forms';
import { MasterDataService } from '../../../services/masterdata.service';
import { MessageService, SelectItem } from "primeng/api";
import { FinancialYear } from "src/app/models/kra.model";
import { KraStatusService } from "src/app/components/kra/Services/krastatus.service";
import { KRAStatusData } from '../models/krastatus.model';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from "@angular/router";
import { DefineKRAData } from '../models/definekra.model';

@Component({
  selector: 'app-kra-status',
  templateUrl: './kra-status.component.html',
  styleUrls: ['./kra-status.component.scss']
})
export class KraStatusComponent implements OnInit {


  @ViewChild("fromFinancialYear") fromFinancialYear: any
  @ViewChild('outerSort') sort: MatSort;
  @ViewChildren('innerSort') innerSort: QueryList<MatSort>;
  @ViewChildren('innerTables') innerTables: QueryList<MatTable<any>>;

  themeappeareance = themeconfig.formfieldappearances;

  displayedColumns: string[] = ['departmentname', 'noofroletypes', 'date', 'status', 'action'];
  // dataSource = krastutusdata;
  dataSource = null;
  financialYearId:number;
  departmentId:number;
  sendtoCEO:boolean;
   _departmentHeadDepartmentId= environment.departmentHeadDepartmentId;

  kraStatusdataSource: MatTableDataSource<any>;

  kraStatusRelationClmns = ['roletypes', 'noofkras', 'status'];
  innerDisplayedColumns = ['kraaspect', 'metrics', 'ration'];

  public financialYearList: SelectItem[] = [];
  KRAStatusForm: FormGroup;
  constructor(private _masterDataService: MasterDataService, private fb: FormBuilder,
    private messageService: MessageService, private _kraStatusService: KraStatusService, 
    private _router: Router) {
  }

  ngOnInit() {
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
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Financial Year List' });
    }
    );
  }

  private onchange(event) {    
    if(this._departmentHeadDepartmentId > 0)
    this.getHODKRAStatuses(); 
    else this.getHRKRAStatuses();
  }

  private getHRKRAStatuses() {   
    this._kraStatusService
      .getHRKRAStatuses(this.KRAStatusForm.value.Id)
      .subscribe(
        (res: KRAStatusData[]) => {
          if (res) {
            this.dataSource = res;
          this.dataSource.forEach((data: DefineKRAData) => {                  
            if(data.SendtoCEOStatus=="SEND")
           {
             this.sendtoCEO=true;
             //data.Status="KRAs Recived from Head HR";      
           }                 
          });
          } else this.dataSource = null;
        },
        error => {
          this.dataSource = null;
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: error.error
          });
        }
      );
  }

 private getHODKRAStatuses() {  
    this._kraStatusService.getHODKRAStatuses(this.KRAStatusForm.value.Id,this._departmentHeadDepartmentId)
      .subscribe(
      (res: KRAStatusData[]) => {
        if (res) {
          this.dataSource = res;
          this.dataSource.forEach((data: DefineKRAData) => {                  
            if(data.Status=="KRA Recived from Head HR")
           {
             data.Status="KRAs Recived from Head HR";      
           }                 
          });
        }
        else this.dataSource = null;
      },
      (error) => {
        this.dataSource = null;
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: error.error
        });
      });
  }

    public SendToCEO(data: KRAStatusData): void {
    this._kraStatusService.SendToCEO
      (this.KRAStatusForm.value.Id).subscribe(res => {
        this.sendtoCEO=false;
        this.getHRKRAStatuses();
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: 'Notification Sent to CEO Successfully'
        });
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: error.error
        });
      });
  }


  public SendToHOD(data: KRAStatusData): void {
    this._kraStatusService.UpdateKRAStatus
      (this.KRAStatusForm.value.Id, data.DepartmentId).subscribe(res => {
        this.getHRKRAStatuses();
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: 'Notification Sent to HOD Successfully'
        });
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: error.error
        });
      });
  }

  public StartHODReview(data: KRAStatusData): void { 
        this._router.navigate(['/kra/krareview/' + this.KRAStatusForm.value.Id + '/' + data.DepartmentId]);
  }

   public StartHRReview(data: KRAStatusData): void { 
        this._router.navigate(['/kra/kradefine/' + this.KRAStatusForm.value.Id + '/' + data.DepartmentId]);
  }

     public SendToHR(data: KRAStatusData): void {
    this._kraStatusService.UpdateKRAStatus
      (this.KRAStatusForm.value.Id, data.DepartmentId,false).subscribe(res => {
        this.getHODKRAStatuses();
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: 'Notification Sent to HR Successfully'
        });
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: error.error
        });
      });
  }
}
