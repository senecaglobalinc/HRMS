import { Component, OnInit, ViewChild } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import * as servicePath from '../../../service-paths';
import { AspectMasterService } from "../Services/aspectmaster.service";
import { AspectData } from "../../../models/kra.model";
import { MessageService } from 'primeng/api';

// Start - This is just for Demo Purpose can be removed later if material data table is not required
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-aspectmaster',
  templateUrl: './aspectmaster.component.html',
  styleUrls: ['./aspectmaster.component.scss'],
  providers: [MessageService, AspectMasterService]
})
export class AspectmasterComponent implements OnInit {
  aspectForm: FormGroup;
  aspectName: string;
  // Start - This is just for Demo Purpose can be removed later if material data table is not required
  displayedColumns: string[] = ['AspectName', 'Edit'];
  dataSource;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  // End - This is just for Demo Purpose can be removed later if material data table is not required

  resources = servicePath.API.PagingConfigValue;

  private componentName: string;
  public PageSize: number;
  public PageDropDown: number[];
  public aspectData: AspectData;
  public kraAspectsList: AspectData[] = [];
  public aspectFormSubmitted: boolean = false;
  saveButton: string = "Add";
  private aspectDataBeforeEdit: AspectData;
  private kraAspectName: string;
  private valid: boolean = false;

  constructor(
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _aspectMasterService: AspectMasterService,
    private messageService: MessageService,
    private fb: FormBuilder
  ) {

    this.componentName = this._activatedRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.aspectForm = this.fb.group({
      AspectId: [0],
      AspectName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z-&,\s()]*$/), Validators.maxLength(70)]]
    });
    this.clear();
    this.getAspectMasterList();
  }

  cols = [
    { field: 'AspectName', header: 'Aspect Name' },
    { field: 'IsMappedAspect', header: 'IsMappedAspect' }
  ];

  private getAspectMasterList(): void {
    this._aspectMasterService.GetAspectMasterList().subscribe((aspectListResponse: AspectData[]) => {
      this.kraAspectsList = [];
      this.kraAspectsList = aspectListResponse;
      this.dataSource = aspectListResponse;
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Aspects List.' });
      }
    );
  }

  private duplicateAspectCheck(aspectName: string): boolean {
    if (this.kraAspectsList) {
      let duplicateAspectList = this.kraAspectsList.filter(function (aspect: AspectData) {
        return (
          aspect.AspectName.toLowerCase() == aspectName.toLowerCase()
        );
      });
      if ((this.saveButton == "Add" && duplicateAspectList.length > 0) || (this.saveButton == "Update" && duplicateAspectList.length > 1)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Aspect Already exists.' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  private dataChangeCheck(aspect: AspectData): boolean {
    if (aspect && this.aspectDataBeforeEdit) {
      if ((this.aspectDataBeforeEdit.AspectId == aspect.AspectId) && (this.aspectDataBeforeEdit.AspectName == aspect.AspectName)) {

        return false;
      }
      else {
        return true;
      }
    }
  }
  public submitForm(): void {
    //this.aspectFormSubmitted = true;
    if (this.aspectForm.valid) {
      this.aspectForm.value.AspectName.trim().replace(/  +/g, ' ');
      let position: number = this.aspectForm.value.AspectName.indexOf('-', 0);
      let position1: number = this.aspectForm.value.AspectName.indexOf('&', 0);
      let position2: number = this.aspectForm.value.AspectName.indexOf(',', 0)
      if (this.aspectForm.value.AspectName.charAt(position) == this.aspectForm.value.AspectName.charAt(position + 1) || this.aspectForm.value.AspectName.charAt(position1) == this.aspectForm.value.AspectName.charAt(position1 + 1) || this.aspectForm.value.AspectName.charAt(position2) == this.aspectForm.value.AspectName.charAt(position2 + 1)) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning Message',
          detail: 'Nothing to Save..'
        });
        return;
      }
      //this.valid = false;
      if (this.aspectForm.value.AspectName.trim() == "") {this.aspectForm.reset(); return;}
      let duplicateAspect = this.duplicateAspectCheck(this.aspectForm.value.AspectName);
      if (!duplicateAspect) return;
    } else return;
    if (this.aspectForm.value.AspectId == 0) {
      this._aspectMasterService.CreateAspect(this.aspectForm.value.AspectName).subscribe((createResponse: boolean) => {
        if (createResponse) {
          this.messageService.add({
            severity: 'success',
            summary: 'Success Message',
            detail: 'Aspect Created successfully'
          });
          this.getAspectMasterList();
          this.clear();
        }
        // else if (createResponse == -1) {          
        //   this.messageService.add({
        //     severity: 'warn',
        //     summary: 'Warning Message',
        //     detail: 'Aspect already exists!.'
        //   });
        // }
        else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error Message',
            detail: 'Failed to Create Aspect.'
          });
        }
      },
        error => {
          //if (error._body != undefined && error._body != "")
          this.messageService.add({
            severity: 'error',
            summary: 'Error Message',
            detail: 'Failed to Create Aspect.'
          });
        }
      );
    }
    else {
      let dataChange = this.dataChangeCheck(this.aspectForm.value);
      if (!dataChange) return;
      this._aspectMasterService.UpdateAspect(this.aspectForm.value).subscribe((updateResponse: boolean) => {
        if (updateResponse) {
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Aspect Updated successfully' });
          this.getAspectMasterList();
          this.clear();
        }
        // else if (updateResponse == -1) {
        //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Aspect already exists!.' });
        // }
        // else if (updateResponse == -14) {
        //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'This Aspect is mapped to KRAs, You cannot update' });
        // }
        // else if (updateResponse == -15) {
        //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid aspect name length'
        //   });
        // }
        // else if (updateResponse == -16) {
        //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Enter valid Aspect'
        //   });
        // }
        else {
          this.messageService.add({
            severity: 'error', summary: 'Error Message', detail: 'Failed to Update Aspect.'
          });
        }

      },
        error => {
          //if (error._body != undefined && error._body != "")
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Update Aspect.' });
        }
      );
    }
  }

  private editAspect(aspect: AspectData): void {
    this.saveButton = "Update";
    this.aspectDataBeforeEdit = new AspectData();
    this.aspectDataBeforeEdit = aspect;
    this.aspectData = aspect;
    this.kraAspectName = "";
    this.kraAspectName = aspect.KRAAspectName;
    this.aspectForm.patchValue(aspect);
    //this.getAspectMasterList();
  }

  // private deleteAspect(aspect: AspectData): void {
  //   this._confirmationService.confirm({
  //     message: "Are you sure, you want to delete?",
  //     header: "KRA Scale Delete",
  //     key: "deleteAspectConfirmation",
  //     icon: "fa fa-trash",
  //     accept: () => {
  //       this._aspectMasterService.DeleteAspect(aspect.AspectId).subscribe((deleteResponse: number) => {
  //         if (deleteResponse == 1) {
  //           this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Aspected Deleted successfully.' });
  //           this.clear();
  //         }
  //         else {
  //           this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'This Aspect is mapped to KRA's, You cannot delete.' });
  //         }
  //       },
  //         error => {
  //           this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Delete Aspect.' });
  //         }
  //       );
  //     },
  //     reject: () => {
  //       return;
  //     }
  //   });

  // }

  public clear(): void {
    this.aspectData = new AspectData();
    this.aspectData = {
      AspectId: 0,
      AspectName: "",
      KRAAspectName: "",
      KRAAspectID: null,
      CreatedDate: null,
      IsMappedAspect: false,
      DepartmentId: null
    };
    this.aspectForm.patchValue({AspectName: null, AspectId: 0 });
    this.aspectForm.controls['AspectName'].setErrors(null);
    this.valid = false;
    this.saveButton = "Add";
    //this.getAspectMasterList();
    this.aspectFormSubmitted = false;
  }

  public validateAspectName(aspectData: AspectData) {
    let numberRegex = /^[a-zA-Z-&,\s()]*$/;
    this.valid = numberRegex.test(aspectData.KRAAspectName);
    if (!this.valid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning Message',
        detail: 'Enter valid Aspect'
      });
      return;
    }
  }

  public omit_special_char(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 38 ||
      k == 44 ||
      k == 45 ||
      k == 40 ||
      k == 41
    );
  }
}

