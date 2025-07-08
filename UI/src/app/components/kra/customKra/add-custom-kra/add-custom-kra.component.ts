import { Component, OnInit } from '@angular/core';
import { Message, ConfirmationService} from "primeng/components/common/api";
import { MessageService } from 'primeng/api';
import { MasterDataService } from 'src/app/services/masterdata.service';
import { CustomKRAService } from '../../Services/custom-kra.service';
import { KRAService } from '../../Services/kra.service';
import { GenericType, DropDownType } from 'src/app/models/dropdowntype.model';
import { CustomKras } from 'src/app/models/associate-kras.model';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KraRoleData } from 'src/app/models/kraRoleData.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-add-custom-kra',
  templateUrl: './add-custom-kra.component.html',
  styleUrls: ['./add-custom-kra.component.scss'],
  providers: [MasterDataService, CustomKRAService, KRAService]
})

export class CustomKraComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  componentName: string;
  requestedToAddKra: boolean = false;
  formSubmitted: boolean = false;
  financialYearsList: DropDownType[] = [];
  projectsList: DropDownType[] = [];
  employeesList: DropDownType[] = [];
  selectedEmployees: DropDownType[] = [];
  kraAspectsList: DropDownType[] = [];
  errorMessage: Message[] = [];
  loggedInEmployeeId: number;
  financialYearId: number;
  projectId: number = 0;
  displayDialog: boolean = false;
  kraSetForm: FormGroup;
  kraFormSubmitted: boolean = false;
  kraRoleData: KraRoleData;
  customKras: CustomKras;
  heading: string;
  showProjectsDropDown: boolean = false;
  public PageSize:number;
  public PageDropDown:number[]=[];

  constructor(
    private _activatedRoute: ActivatedRoute, 
    private _router: Router,
    private _masterDataService: MasterDataService, 
    private _customKraService: CustomKRAService, 
    private messageService: MessageService,
    private _kraService: KRAService, 
    private _formBuilder: FormBuilder) {
      this.componentName = this._activatedRoute.routeConfig.component.name;
      this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
      this.loggedInEmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
      this.kraRoleData = new KraRoleData();
  }

  ngOnInit() {
      this._activatedRoute.params.subscribe(params => { this.financialYearId = params['financialYearId'] });
      this._activatedRoute.params.subscribe(params => { this.projectId = params['projectId'] });

      let roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
      if (roleName.indexOf("Program Manager") != -1) {
          this.showProjectsDropDown = true;
          this.getProjectsByEmployeeId();
      }
      else {
          this.showProjectsDropDown = false;
          this.getEmployeesByDepartment();
      }
      this.getFinancialYears();
      this.getKraAspects();
      this.kraSetForm = this._formBuilder.group({
          aspect: ['', [Validators.required]],
          metric: ['', [Validators.required]],
          target: ['', [Validators.required]]
      });
      if (Number(this.financialYearId) > 0 && Number(this.projectId) > 0)
          this.getEmployeesByProjectId(this.projectId);
  }

    cols = [
      { field: 'label', header: 'Associate Name' }
    ];

  private getFinancialYears(): void {
      this._masterDataService.GetFinancialYears().subscribe((yearsdata: GenericType[]) => {
          this.financialYearsList = [];
          this.financialYearsList.push({ label: 'Select Financial Year', value: null });
          yearsdata.forEach((element: GenericType) => {
              this.financialYearsList.push({ label: element.Name, value: element.Id });
          });
      }, (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Failed to get financial years.', detail: '' });
      });
  }

  private getProjectsByEmployeeId(): void {
      if (Number(this.loggedInEmployeeId) > 0) {
          this._customKraService.GetProjectsByProgramManagerId(this.loggedInEmployeeId).subscribe((result: GenericType[]) => {
              this.projectsList = [];
              this.projectsList.push({ label: 'Select Project', value: null });
              if (result.length > 0) {
                  result.forEach((element: GenericType) => {
                      this.projectsList.push({ label: element.Name, value: element.Id });
                  });
              }
          }, (error: any) => {
              this.messageService.add({ severity: 'error', summary: 'Failed to get projects.', detail: '' });
          });
      }
      else {
          this.projectsList = [];
          this.projectsList.push({ label: 'Select Project', value: null });
      }
  }

  private getEmployeesByProjectId(projectId: number): void {
      if (projectId > 0) {
          this._customKraService.GetEmployeesByProjectId(projectId).subscribe((result: GenericType[]) => {
              this.employeesList = [];
              if (result.length > 0) {
                  result.forEach((element: GenericType) => {
                      this.employeesList.push({ label: element.Name, value: element.Id });
                  });
              }
          }, (error: any) => {
              this.messageService.add({ severity: 'error', summary: 'Failed to get employees.', detail: '' });
          });
      }
      else {
        this.employeesList = [];
      }
  }

  private getEmployeesByDepartment(): void {
      if (Number(this.loggedInEmployeeId) > 0) {
          this._customKraService.GetEmployeesByDepartment(this.loggedInEmployeeId).subscribe((result: GenericType[]) => {
            this.employeesList = [];
              if (result.length > 0) {
                  result.forEach((element: GenericType) => {
                      this.employeesList.push({ label: element.Name, value: element.Id });
                  });
              }
          }, (error: any) => {
              this.messageService.add({ severity: 'error', summary: 'Failed to get employees.', detail: '' });
          });
      }
      else {
        this.employeesList = [];
      }
  }

  private getKraAspects(): void {
        // this._kraService.getKraAspects().subscribe((aspectsData: KraAspectData[]) => {
        //     this.kraAspectsList = [];
        //     this.kraAspectsList.push({ label: 'Select KRA Aspect', value: null });
        //     aspectsData.forEach((e: KraAspectData) => {
        //         this.kraAspectsList.push({ label: e.KRAAspectName, value: e.KRAAspectID });
        //     });
        // }, (error: any) => {
        // this.messageService.add({ severity: 'error', summary: 'Failed to get kra aspects.', detail: '' });
  }

  public addCustomKra(): void {
      this.requestedToAddKra = true;
      if (this.financialYearId > 0) {
          if (this.selectedEmployees.length <= 0) {
              this.messageService.add({ severity: 'info', summary: 'Please select at least one associate to add Custom KRA.', detail: '' });
              return;
          }
          else if (this.selectedEmployees.length > 0) {
              this.heading = "Add Custom KRA";
              this.displayDialog = true;
              this.kraFormSubmitted = false;
              this.kraSetForm.controls['aspect'].reset();
              this.kraSetForm.controls['metric'].reset();
              this.kraSetForm.controls['target'].reset();
          }
      }
  }

  public save(): void {
      this.kraFormSubmitted = true;
      if (this.kraSetForm.valid) {
          if (this.kraRoleData.KRAAspectMetric.trim().length <= 0 || this.kraRoleData.KRAAspectTarget.trim().length <= 0) {
              this.messageService.add({ severity: 'warn', summary: 'Please provide valid Metric and/or Target details.', detail: '' });
              return;
          }
          else {
              this.customKras = new CustomKras();
              this.customKras.EmployeeIDs = new Array<number>();
              this.selectedEmployees.forEach((employee: DropDownType) => {
                  this.customKras.EmployeeIDs.push(employee.value);
              });
              this.customKras.KRAAspectID = this.kraRoleData.KRAAspectID;
              this.customKras.KRAAspectMetric = this.kraRoleData.KRAAspectMetric.trim();
              this.customKras.KRAAspectTarget = this.kraRoleData.KRAAspectTarget.trim();
              this.customKras.FinancialYearID = this.financialYearId;
              this._customKraService.SaveCustomKras(this.customKras).subscribe((data: any) => {
                  this.selectedEmployees = [];
                  this.messageService.add({ severity: 'success', summary: 'Custom KRA data saved successfully.', detail: '' });
                  this.cancel();
              }, (error) => {
                  this.messageService.add({ severity: 'error', summary: 'Failed to save KRA data.', detail: '' });
              });
          }
      }
  }

  public cancel(): void {
      this.displayDialog = false;
      this.kraSetForm.reset();
      this.kraFormSubmitted = false;
  }

  private viewOrganizationAndCustomKrasOfEmployee(employeeId: number) {
      if (employeeId && this.financialYearId > 0)
          this._router.navigate(['kra/view-associate-kras/' + employeeId + '/' + this.financialYearId + '/' + this.projectId]);
  }

}

