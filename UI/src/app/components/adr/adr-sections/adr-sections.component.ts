import { Component, OnInit, ViewChild } from '@angular/core';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ADRSections } from '../Models/adr-sections.model';
import { AdrSectionsService } from '../Services/adr-sections.service';
import { DropDownType } from '../../../models/dropdowntype.model';
import { MasterDataService } from "../../../services/masterdata.service";

@Component({
  selector: 'app-adr-sections',
  templateUrl: './adr-sections.component.html',
  styleUrls: ['./adr-sections.component.scss'],
  providers: [MessageService, AdrSectionsService, MasterDataService]
})
export class AdrSectionsComponent implements OnInit {

  addADRSections: FormGroup;
  btnLabel: string = "";
  formSubmitted = false;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  adrSectionsList: ADRSections[] = [];
  selectedRow: ADRSections;
  adrSectionId: number = 0;
  adrMeasurementAreaList: DropDownType[] = [];
  departmentList: DropDownType[] = [];
  DepartmentId: number[] = [];
  deptDisable: boolean = false;
  @ViewChild("dt1") table: any;

  cols = [
    { field: 'DepartmentDescription', header: 'Department' },
    { field: 'ADRMeasurementAreaName', header: 'ADR Measurement Area' },
    { field: 'ADRSectionName', header: 'ADR Section Name' }
  ];

  constructor(private adrSectionsService: AdrSectionsService,
    private masterDataService: MasterDataService,
    private messageService: MessageService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.addADRSections = new FormGroup({
      ADRSectionId: new FormControl(null),
      DepartmentId: new FormControl(null),
      ADRMeasurementAreaId: new FormControl(null, [
        Validators.required
      ]),
      ADRSectionName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(1000)
      ]),
    });
    this.btnLabel = "Add";
    this.getADRMeasurementAreas();
    this.getDepartments();
    this.clear();
  }

  private getADRSections(): void {
    this.deptDisable = false;
    this.adrSectionsService.GetADRSections().subscribe((res: ADRSections[]) => {
      this.adrSectionsList = [];
      this.adrSectionsList = res;
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get ADR Sections.' });
      }
    );
  }

  private getADRMeasurementAreas(): void {
    this.adrSectionsService.GetADRMeasurementAreas().subscribe((res: ADRSections[]) => {
      this.adrMeasurementAreaList = [];
      this.adrMeasurementAreaList.push({ label: "Select ADRMeasurement Area", value: null });
      res.forEach((element: ADRSections) => {
        this.adrMeasurementAreaList.push({ label: element.ADRMeasurementAreaName, value: element.ADRMeasurementAreaId });
      });
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get ADR Meaurement Areas.' });
      }
    );
  }
  getDepartments() {
    this.masterDataService.GetDepartments().subscribe(
      (res: any[]) => {
        this.departmentList = [];
        // this.departmentList.push({ label: "Select Department", value: null });
        res.forEach(element => {
          this.departmentList.push({
            label: element.Description,
            value: element.DepartmentId
          });
        });
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get Departments details.',
          summary: 'Error Message'
        });
      }
    );
  }
  private duplicateSectionCheck(sectionName: string): boolean {
    if (this.adrSectionsList) {
      let duplicateSectionsList = this.adrSectionsList.filter(function (section: ADRSections) {
        return (
          section.ADRSectionName.toLowerCase() == sectionName.toLowerCase()
        );
      });
      if ((this.btnLabel == "Add" && duplicateSectionsList.length > 0) || (this.btnLabel == "Update" && duplicateSectionsList.length > 1)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ADR Section already exists.' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  public toAddorUpdateADRSection(): void {
    this.table.reset();
    this.formSubmitted = true;
    if (this.addADRSections.valid == true) {
      let duplicateSection = this.duplicateSectionCheck(this.addADRSections.value.ADRSectionName);
      if (!duplicateSection) return;
      if (this.adrSectionId <= 0) { //Add new Section
        this.addNewSection();
      }
      else
        this.updateSection();
    }
    else {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      //this.cancel();
    }
  }

  public editADRSection(section: ADRSections): void {
    this.btnLabel = "Update";
    this.deptDisable = true;
    this.adrSectionId = section.ADRSectionId;
    this.addADRSections.patchValue(section);
    this.addADRSections.controls['ADRMeasurementAreaId'].setValue(this.addADRSections.value.ADRMeasurementAreaId);
    this.addADRSections.controls['ADRSectionName'].setValue(this.addADRSections.value.ADRSectionName);
    this.addADRSections.controls['DepartmentId'].setValue(this.addADRSections.value.DepartmentId);
  }

  private addNewSection(): void {
    this.addADRSections.controls['DepartmentId'].setValue(this.addADRSections.value.DepartmentId.join(','));
    console.log(this.addADRSections.controls['DepartmentId']);
    this.adrSectionsService.CreateADRSections(this.addADRSections.value).subscribe(res => {
      if (res == 1) {
        this.adrSectionsService.GetADRSections();
        this.adrSectionsList = [];
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ADR Section Name added' });
        this.clear();
      }
      else if (res == -1)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ADR Section Name already exist' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add ADR Section Name' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  private updateSection(): void {
    this.adrSectionsService.UpdateADRSections(this.addADRSections.value).subscribe(res => {
      if (res == 1) {
        this.adrSectionsService.GetADRSections();
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ADR Section Name updated' });
        this.clear();
      }
      else if (res == -1)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ADR Section Name already exist' });
      else {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to update ADR Section Name' });
      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
  }

  public clear(): void {
    this.formSubmitted = false;
    this.addADRSections.reset();
    this.btnLabel = "Add";
    this.adrSectionId = 0;
    this.adrSectionsList = [];
    this.getADRSections();

  }


}
