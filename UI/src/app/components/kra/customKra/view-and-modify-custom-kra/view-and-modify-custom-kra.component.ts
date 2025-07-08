import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { CustomKRAService } from '../../Services/custom-kra.service';
import { KRAService } from '../../Services/kra.service';
import { ConfirmationService, Message, MessageService } from 'primeng/api';
import { MasterDataService } from 'src/app/services/masterdata.service';
import { CustomKras, OrganizationKras, AssociateKras } from 'src/app/models/associate-kras.model';
import { DropDownType } from 'src/app/models/dropdowntype.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-view-and-modify-custom-kra',
  templateUrl: './view-and-modify-custom-kra.component.html',
  styleUrls: ['./view-and-modify-custom-kra.component.scss'],
  providers: [CommonService, CustomKRAService, KRAService, ConfirmationService, MasterDataService]
})

export class CustomKrasOperations implements OnInit { 
  resources = servicePath.API.PagingConfigValue;
  componentName: string;
  employeeId: number;
  financialYearId: number;
  errorMessage: Message[] = [];
  organizationKras: Array<OrganizationKras>;
  customKras: Array<CustomKras>;
  financialYear: string;
  projectId: number = 0;
  display: boolean = false;
  kraAspectsList: DropDownType[] = [];
  kraSetForm: FormGroup;
  heading: string;
  customKraData: CustomKras;
  kraFormSubmitted: boolean = false;
  employeeName: string;
  public PageSize:number;
  public PageDropDown:number[]=[];

  constructor(
    private _activatedRoute: ActivatedRoute, 
    private _commonService: CommonService, 
    private _formBuilder: FormBuilder,
    private _customKraService: CustomKRAService, 
    private _router: Router, 
    private _kraService: KRAService, 
    private _masterDataService: MasterDataService,
    private messageService: MessageService,
    private _confirmationService: ConfirmationService) {
      this.componentName = this._activatedRoute.routeConfig.component.name;
      this.organizationKras = new Array<OrganizationKras>();
      this.customKras = new Array<CustomKras>();
      this.customKraData = new CustomKras();
      this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;  
  }
  ngOnInit() {
      this._activatedRoute.params.subscribe(params => { this.financialYearId = params['financialYearId'] });
      this._activatedRoute.params.subscribe(params => { this.employeeId = params['employeeId'] });
      this._activatedRoute.params.subscribe(params => { this.projectId = params['projectId'] });

      this.kraSetForm = this._formBuilder.group({
          aspect: ['', [Validators.required]],
          metric: ['', [Validators.required]],
          target: ['', [Validators.required]]
      });
      this.getKraAspects();
      this.getAssociateKrasList(this.financialYearId, this.employeeId);
      if (Number(this.financialYearId) > 0)
          this.getFinancialYear();
      if (Number(this.employeeId) > 0)
          this.getEmployeeNameById();
  }

  cols = [
    { field: 'KRAAspectName', header: 'Aspect' },
    { field: 'KRAAspectMetric', header: 'Metric' },
    { field: 'KRAAspectTarget', header: 'Target' }
  ];

  cols1 = [
    { field: 'KRAAspectName', header: 'Aspect' },
    { field: 'KRAAspectMetric', header: 'Metric' },
    { field: 'KRAAspectTarget', header: 'Target' }
  ];

  private getAssociateKrasList(financialYearId: number, employeeId: number) {
      if (financialYearId > 0 && employeeId > 0) {
        this._customKraService.GetOrganizationAndCustomKrasOfEmployee(employeeId, financialYearId).subscribe((result: AssociateKras) => {
            this.organizationKras = [];
            this.customKras = [];         
            if (result.OrganizationKRAs.length > 0) {
                result.OrganizationKRAs.forEach((kra: OrganizationKras) => {
                    this.organizationKras.push(kra);
                });
            }
            if (result.CustomKRAs.length > 0) {
                result.CustomKRAs.forEach((kra: CustomKras) => {
                    this.customKras.push(kra);
                });
            }
          },
              (error: any) => {
                this.messageService.add({ severity: 'error', summary: 'Failed to get Departments details.', detail: '' });
              });    
      }
  }

  getFinancialYear() {
      this._kraService.GetFinancialYear(this.financialYearId).subscribe((res: string) => {
          if (res != '')
              this.financialYear = res;
      },
          (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Failed to get financial year.', detail: '' });
          }
      );
  }

  getEmployeeNameById() {
      this._masterDataService.GetEmployeeNameByEmployeeId(this.employeeId).subscribe((res: string) => {
          if (res != '')
              this.employeeName = res;
      },
          (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Failed to get employee name.', detail: '' });
          }
      );
  }

  private getKraAspects(): void {
      // this._kraService.getKraAspects().subscribe((aspectsData: KraAspectData[]) => {
      //     this.kraAspectsList = [];
      //     this.kraAspectsList.push({ label: 'Select KRA Aspect', value: null });
      //     if (aspectsData.length > 0) {
      //         aspectsData.forEach((e: KraAspectData) => {
      //             this.kraAspectsList.push({ label: e.KRAAspectName, value: e.KRAAspectID });
      //         });
      //     }
      // }, (error: any) => {
      //     this.messageService.add({ severity: 'error', summary: 'Failed to get KRA aspects.', detail: '' });
      // });
  }
  private editCustomKrasOfEmployee(customKra: CustomKras) {
      this.display = true;
      this.customKraData.KRAAspectID = customKra.KRAAspectID;
      this.customKraData.KRAAspectMetric = customKra.KRAAspectMetric;
      this.customKraData.KRAAspectTarget = customKra.KRAAspectTarget;
      this.customKraData.CustomKRAID = customKra.CustomKRAID;
      this.kraFormSubmitted = false;
  }

  public update() {
      this.kraFormSubmitted = true;
      if (this.kraSetForm.valid) {
          if (this.customKraData.KRAAspectMetric.trim().length <= 0 || this.customKraData.KRAAspectTarget.trim().length <= 0) {
              this.messageService.add({ severity: 'warn', summary: 'Please provide valid Metric and/or Target details.', detail: '' });
              return;
          }
          else {
              this.customKraData.KRAAspectMetric = this.customKraData.KRAAspectMetric.trim();
              this._customKraService.EditCustomKras(this.customKraData).subscribe((data: any) => {
                  this.messageService.add({ severity: 'success', summary: 'Custom KRA data updated successfully.', detail: '' });
                  this.cancel();
                  this.getAssociateKrasList(this.financialYearId, this.employeeId);
              }, (error) => {
                  this.messageService.add({ severity: 'error', summary: 'Failed to update Custom KRA data.', detail: '' });
              });
          }
      }
  }

  public cancel(): void {
      this.display = false;
      this.kraSetForm.reset();
      this.kraFormSubmitted = false;
  }

  private deleteCustomKrasOfEmployee(customKra: CustomKras) {
      this._confirmationService.confirm({
          message: 'Selected Metric and Target will be deleted',
          header: 'Delete Confirmation',
          key: 'customKraMetric',
          icon: 'fa fa-trash',
          accept: () => {
              this.delete(customKra.CustomKRAID)
          },
          reject: () => {
          }
      });
  }

  private delete(customKraId: number) {
      if (customKraId > 0) {
          this._customKraService.DeleteCustomKra(customKraId).subscribe((data: any) => {
              this.messageService.add({ severity: 'success', summary: 'Custom KRA data deleted successfully.', detail: '' });
              this.getAssociateKrasList(this.financialYearId, this.employeeId);
          }, (error) => {
              if (error._body != undefined && error._body != "")
                  this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => { });
              this.messageService.add({ severity: 'error', summary: 'Failed to delete Custom KRA data.', detail: '' });
          });
      }
      else 
      this.messageService.add({ severity: 'error', summary: 'Failed to delete Custom KRA data.', detail: '' });
  }

  public onBackButtonClick() {
      this._router.navigate(['kra/customkras/' + this.employeeId + '/' + this.financialYearId + '/' + this.projectId]);
  }
}
