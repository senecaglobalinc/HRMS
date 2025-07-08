import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { CommonService } from "../../../services/common.service";
import { Router, ActivatedRoute } from "@angular/router";
import { KraAspectService } from "../Services/kra-aspect.service";
import { KraAspectData } from "../Models/kra.model";
import { Message, SelectItem, MessageService } from "primeng/components/common/api";
import { MasterDataService } from "../../../services//masterdata.service";
import { AspectData } from "../../../models/kra.model";
import * as moment from 'moment';
import * as servicePath from '../../../service-paths';
import { AspectMasterService } from '../Services/aspectmaster.service';
import { DepartmentDetails } from '../../../models/role.model';
import { KraSetData } from '../../../models/kra.model';
//import { MessageService } from 'primeng/components/common/messageservice';
// import saveAs from "file-saver";



import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';



interface Department {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-kraaspect',
  templateUrl: './kraaspect.component.html',
  styleUrls: ['./kraaspect.component.scss']
  // ,
  // providers: [MessageService]

})
export class KraaspectComponent implements OnInit {

  displayedColumns: string[] = ['AspectName', 'CreatedDate', 'delete'];
  dataSource;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  colms: any[] = [];
  columnOptions: any[] = [];
  private componentName: string;
  public aspectFormSubmitted: boolean = false;
  private errorMessage: Message[] = [];
  public kraAspectsList: Array<AspectData>;
  private aspectName: string;
  private aspectId: number;
  public departmentList: SelectItem[] = [];
  public kraAspectData: KraAspectData;
  public buttonTitle: string;
  public PageSize: number;
  public PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  gridMessage: string = "No records found";
  public aspectsList: SelectItem[] = [];
  public aspectIds: number[] = [];
  selectedColumns: any[];
  addKraAspect: FormGroup;
  kraAspectForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _kraAspectService: KraAspectService,
    private _masterDataService: MasterDataService,
    private _aspectMasterService: AspectMasterService,
    private messageService: MessageService
  ) {
    this.componentName = this._activatedRoute.routeConfig.component.name;
    this.kraAspectsList = new Array<AspectData>();
    this.kraAspectData = new KraAspectData();
    this.kraAspectData.lstAspectData = new Array<AspectData>()
    this.buttonTitle = "Add";

    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.kraAspectForm = this.fb.group({
      departmentId: [null, [Validators.required]],
      aspectIds: ['', [Validators.required]]
    });
    this.cancel();
    this.getDepartments();
    this.getAspectsList();
  }

  cols = [
    { field: 'AspectName', header: 'Aspect Name' },
    { field: 'CreatedDate', header: 'Created Date' }
  ];

  private getDepartments(): void {
    this._masterDataService.GetDepartments().subscribe(
      (res: DepartmentDetails[]) => {
        this.departmentList = [];
        this.departmentList.push({ label: "Select Department", value: null });
        res.forEach((element: DepartmentDetails) => {
          this.departmentList.push({
            label: element.Description,
            value: element.DepartmentId
          });
        }); 
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get Departments details.'
        });
      }
    );
  }

  public deleteKraAspect(aspectData : AspectData ) : void{
    this._kraAspectService.DeleteKraAspect(aspectData).subscribe( res=>{
      if(res > 0){
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: 'Deleted Successfully'
        });
        this.getKraAspects(this.kraAspectData.DepartmentId);
      }
      else if (res == -14){
        this.messageService.add({
            severity: 'warn',
            summary: 'Warning Message',
          detail: 'Aspect is mapped to approved KRA'
        });
      }
    },
    (error)=>{
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: error.error
      });
    });
  }


  private saveAspect(): void {
    //this.aspectFormSubmitted = true;
    if (this.kraAspectForm.valid) {
      this.kraAspectData.lstAspectData = [];
      for (var i = 0; i < this.aspectIds.length; i++) {
        this.kraAspectData.lstAspectData.push({ AspectId: this.aspectIds[i], KRAAspectID: null, CreatedDate: null, KRAAspectName: null, AspectName:null, DepartmentId:null, IsMappedAspect:false})
      }
    }
    else {
      return;
    }
    this._kraAspectService.createKraAspect(this.kraAspectForm.value).subscribe(
      (data: number) => {
        if (data) {
          this.messageService.add({
            severity: 'success',
            summary: 'Success Message',
            detail: 'KRA Aspect created successfully'
          });
        }
        else if (data == -1) {
          this.messageService.add({
            severity: 'warn',
            summary: 'Warning Message',
            detail: 'Kra aspect already exist for this Department'
          });
        }
        else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error Message',
            detail: 'Failed to create kra aspect'
          });
        }
        //this.aspectFormSubmitted = false;
        this.getKraAspects(this.kraAspectForm.value.departmentId);
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Sorry! Failed to create kra aspect'
        });
      }
    );
  }

  public getKraAspects(departmentId: number): void {
    if (departmentId != null) {
      this._kraAspectService.getKraAspects(departmentId).subscribe(
        (aspectsData: AspectData[]) => {
          this.kraAspectsList = [];
          this.aspectIds = [];
          this.dataSource = aspectsData;
          this.kraAspectsList = aspectsData;
          if(this.kraAspectsList.length <= 0)
        this.gridMessage = "No records found";
        else
        this.gridMessage = "";
          this.kraAspectsList.forEach((kra: AspectData) => {
            kra.CreatedDate = moment(kra.CreatedDate).format("YYYY-MM-DD");
          });
          this.getAspectsList();
        },
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error Message',
            detail: 'Sorry! Failed to get kra aspects'
          });
        }
      );
    } else {
      this.kraAspectsList = [];
      this.getAspectsList();
    }
  }

  private editAspect(aspectId : number,departmentId: number, id: number, name: string,): void {
    this.buttonTitle = "Update";
    if (id && name && name != "") {
      this.kraAspectData = new KraAspectData();
      this.kraAspectData.KRAAspectID = id;
      this.kraAspectData.DepartmentId = departmentId;
      this.kraAspectData.AspectId = aspectId;
      this.aspectName = name;
      this.aspectIds = [];
      this.aspectsList.push({
        label: name,
        value: aspectId
      });
      this.aspectsList.map((item) => {
        if (item.value == aspectId)
          this.aspectIds.push(item.value)
      }
      );
      
    }
  }

  private updateAspect(): void {
    this.aspectFormSubmitted = true;
    
    if (this.aspectIds.length > 0) {
      this.kraAspectData.lstAspectData = [];
      for (var i = 0; i < this.aspectIds.length; i++) {
        this.kraAspectData.lstAspectData.push({ AspectId: this.aspectIds[i], KRAAspectID: null, CreatedDate: null, KRAAspectName: null, AspectName: null, DepartmentId: null, IsMappedAspect: false})
      }
    }
    else {
      return;
    }
    if (
      this.kraAspectData.KRAAspectID &&
      this.aspectName &&
      this.aspectName != ""
    ) {
      if (this.aspectName.trim().length == 0) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning Message',
          detail: 'Please give valid Aspect Name!'
        });

      } else {
        // this.kraAspectData.AspectId = this.aspectIds[0];
        // this.kraAspectData.KRAAspectName = this.aspectName.trim();
        this._kraAspectService.updateKraSet(this.kraAspectData).subscribe(
          (data: number) => {
            if (data == 1) {
              this.messageService.add({
                severity: 'success',
                summary: 'Success Message',
                detail: 'KRA Aspect updated successfully'
              });
            }
            else if (data == -1) {
              this.messageService.add({
                severity: 'warn',
                summary: 'Warning Message',
                detail: 'Kra aspect already exist for this Department'
              });
            }
            else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error Message',
                detail: 'Failed to update kra aspect'
              });

            }

            this.buttonTitle = "Add";
            this.aspectFormSubmitted = false;
            this.aspectName = "";
            this.getKraAspects(this.kraAspectData.DepartmentId);
          },
          (error: any) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error Message',
              detail: 'Sorry! Failed to update kra aspect'
            });
          }
        );
      }
    }
  }

  public cancel(): void {
    this.kraAspectsList = [];
    this.buttonTitle = "Add";
    this.aspectFormSubmitted = false;
    this.kraAspectData.DepartmentId = null;
    this.aspectIds = [];
  }

  private removeExistingAspects(aspectListResponse: AspectData[]): AspectData[] {
    let duplicateCheck: AspectData[] = [];
    if (this.kraAspectsList.length > 0 && aspectListResponse.length > 0) {
      duplicateCheck = aspectListResponse.filter((obj) => {
        return this.kraAspectsList.find((xyz) => xyz.AspectId === obj.AspectId) == null;
      });
    }
    return duplicateCheck;
  }

  private getAspectsList(): void {
    this._aspectMasterService.GetAspectMasterList().subscribe((aspectListResponse: AspectData[]) => {
      this.aspectsList = [];
      this.aspectIds = [];
      if (aspectListResponse.length == 0) return;
      if (this.kraAspectsList.length > 0) {
        let aspectListRes = this.removeExistingAspects(aspectListResponse);
        aspectListRes.forEach((element: AspectData) => {
          this.aspectsList.push({
            label: element.AspectName,
            value: element.AspectId
          });
        });
      }
      else {
        aspectListResponse.forEach((element: AspectData) => {
          this.aspectsList.push({
            label: element.AspectName,
            value: element.AspectId
          });
        });
      }
    },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: 'Failed to get Aspect List.'
        });
      }
    );
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }
  
}
