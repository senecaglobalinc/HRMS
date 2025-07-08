import { Component, OnInit } from '@angular/core';
import { MasterDataService } from 'src/app/services/masterdata.service';
import { MapAssociateRoleService } from '../Services/map-associate-role.service';
import { ConfirmationService, Message, SelectItem, MessageService } from 'primeng/api';
import { AssociateRoleMappingData } from 'src/app/models/associaterolemappingdata.model';
import { GenericType } from 'src/app/models/dropdowntype.model';
import { ProjectDetails } from 'src/app/models/projects.model';
import { DepartmentDetails } from 'src/app/models/role.model';
import { DepartmentType } from '../../shared/utility/enums';
import { ActivatedRoute } from '@angular/router';
import * as servicePath from '../../../service-paths';
import { KRAService } from '../Services/kra.service';
import { KraRoleData } from '../../../models/kraRoleData.model';

@Component({
  selector: 'app-mapassociaterole',
  templateUrl: './mapassociaterole.component.html',
  styleUrls: ['./mapassociaterole.component.scss'],
  providers: [MasterDataService, MapAssociateRoleService, ConfirmationService]
})

export class MapassociateroleComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  private componentName: string;
  private errorMessage: Message[] = [];
  public projectsList: SelectItem[] = [];
  public rolesList: SelectItem[] = [];
  public departmentList: SelectItem[] = [];
  public kraRoleList: SelectItem[] = [];
  public formSubmitted: boolean = false;
  public associateRoleMapping: AssociateRoleMappingData;
  // public selectedEmployees: AssociateRoleMappingData[] = [];
  public associatesList: AssociateRoleMappingData[] = [];
  private departmentType: number;
  public isNonDelivery: boolean = true;
  private isNew: boolean = true;
  public cols: any[];
  public PageSize: number;
  public PageDropDown: number[] = [];

  constructor(
    private _masterDataService: MasterDataService,
    private _activatedRoute: ActivatedRoute,
    private _mapAssociateRoleService: MapAssociateRoleService,
    private _confirmationService: ConfirmationService,
    private messageService: MessageService,
    public kraService: KRAService
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe(params => {
      if (params["isNew"]) {
        this.isNew = params["isNew"];
        this.componentName = this._activatedRoute.routeConfig.component.name;
        this.Clear();
        this.departmentType = DepartmentType.Delivery;
        this.departmentList = [];
        this.departmentList.push({ label: "Select Department", value: null });
        this.projectsList = [];
        this.projectsList.push({ label: "Select Project", value: null });
        this.kraRoleList = [];
        this.kraRoleList.push({ label: "Select KRA Role", value: null });
        this.rolesList = [];
        this.rolesList.push({ label: "Select KRA Group", value: null });
        this.getDepartments();
        this.dynamicColumnBinding();
        this.getCurrentFinancialYear();
      }
    });
  }

  private dynamicColumnBinding() {
    this.cols = [];
    if (String(this.isNew) == "true") {
      this.cols = [
        { field: "EmployeeName", header: "Associate Name" }
      ];
    } else {
      this.cols = [
        { field: "EmployeeName", header: "Associate Name" },
        { field: "RoleName", header: "Role Name" }
      ];
    }
  }

  private getDepartments(): void {
    this._masterDataService.GetDepartments().subscribe(
      (res: DepartmentDetails[]) => {
        res.forEach((element: DepartmentDetails) => {
          this.departmentList.push({
            label: element.Description,
            value: element.DepartmentId
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Departments details.' });
      }
    );
  }

  public getEmployeesByDepartmentId(departmentId: number): void {
    // this.selectedEmployees = new Array<AssociateRoleMappingData>();
    this.associatesList = new Array<AssociateRoleMappingData>();
    this.getRoles(departmentId);
    if (departmentId == this.departmentType) {
      this.getProjects();
      this.getKraRoles();
      this.getCurrentFinancialYear();
      this.isNonDelivery = false;
    } else {
      this.projectsList = [];
      this.associateRoleMapping.ProjectId = null;
      this.projectsList.push({ label: "Select Project", value: null });
      let projectId: number = null;
      // this.getEmployeesByDepartmentIdAndProjectId(departmentId, projectId);
      this.isNonDelivery = true;
    }
  }

  private getProjects(): void {
    this._masterDataService.GetProjectsList().subscribe(
      (res: ProjectDetails[]) => {
        this.projectsList = [];
        this.associateRoleMapping.ProjectId = null;
        this.projectsList.push({ label: "Select Project", value: null });
        res.forEach((element: ProjectDetails) => {
          this.projectsList.push({
            label: element.ProjectName,
            value: element.ProjectId
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Projects list.' });
      }
    );
  }

  private getKraRoles() {
    this._masterDataService.GetKraRoles().subscribe(
      (res: KraRoleData[]) => {
        this.projectsList = [];
        this.projectsList.push({ label: "Select Project", value: null });
        this.kraRoleList = [];
        this.associateRoleMapping.KRARoleId = null;
        this.kraRoleList.push({ label: "Select KRA Role", value: null });
        res.forEach((element: KraRoleData) => {
          this.kraRoleList.push({
            label: element.KRARoleName,
            value: element.KRARoleID
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get KRA Roles list.' });
      }
    );
    
  }

  public getEmployeesByDepartmentIdAndProjectId(
    departmentId: number,
    projectId: number
  ): void {
    if (departmentId != null) {
      this._mapAssociateRoleService.GetEmployeesByDepartmentIdAndProjectId(departmentId, projectId, this.isNew).subscribe(
        (res: AssociateRoleMappingData[]) => {
          this.associatesList = [];
          res.forEach((element: AssociateRoleMappingData) => {
            element.EmployeeName = element.AssociateName
          });
          this.associatesList = res;
          this.dynamicColumnBinding();
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Employees list.' });
        }
      );
    } else {
      this.associatesList = [];
    }
  }

  getEmployeesByKraRoleIdAndFinancialYearId(
    financialYearId: number,
    kraRoleId: number
  ): void {
    if (kraRoleId != null) {
      this._mapAssociateRoleService.GetEmployeesByKraRoleIdAndFinancialYearId(financialYearId, kraRoleId, this.isNew).subscribe(
        (res: AssociateRoleMappingData[]) => {
          this.associatesList = [];
          res.forEach((element: AssociateRoleMappingData) => {
            element.Id = element.EmployeeId
          });
          this.associatesList = res;
          this.dynamicColumnBinding();
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Employees list.' });
        }
      );
    } else {
      this.associatesList = [];
    }
  }


  private getRoles(departmentId: number): void {
    if (departmentId != null) {
      this._mapAssociateRoleService
        .GetKraRolesByDepartmentId(departmentId)
        .subscribe(
          (res: GenericType[]) => {
            this.rolesList = [];
            this.associateRoleMapping.KRAGroupId = null;
            this.rolesList.push({ label: "Select KRA", value: null });
            res.forEach((element: GenericType) => {
              this.rolesList.push({ label: element.Name, value: element.Id });
            });
          },
          (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Roles list Type.' });
          }
        );
    } else {
      this.rolesList = [];
      this.rolesList.push({ label: "Select KRA", value: null });
    }
  }
  public getCurrentFinancialYear() {
    this.kraService
      .getCurrentFinancialYear()
      .subscribe(
        (res) => {
          this.associateRoleMapping.FinancialYearID = res.Id;
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Roles list Type.' });
        }
      );
  }

  public submit(
    associateRoleMapping: AssociateRoleMappingData
    // , selectedEmployees: AssociateRoleMappingData[]
  ): void {
    this.formSubmitted = true;
    if (
      associateRoleMapping.DepartmentId == null ||
      associateRoleMapping.KRAGroupId == null
    )
      return;
      if (this.associatesList.length > 0) {
        associateRoleMapping.IDs = [];
        this.associatesList.forEach((employee: AssociateRoleMappingData) => {
          associateRoleMapping.IDs.push(employee.Id);
          // associateRoleMapping.IDs.push(employee.EmployeeId);
        });
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'No Associates to map the role.' });
        return;
      }

    // if (selectedEmployees.length > 0) {
    //   associateRoleMapping.IDs = [];
    //   selectedEmployees.forEach((employee: AssociateRoleMappingData) => {
    //     associateRoleMapping.IDs.push(employee.Id);
    //     associateRoleMapping.IDs.push(employee.EmployeeId);
    //   });
    // } else {
    //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'No Associates to map the role.' });
    //   return;
    // }
    if (String(this.isNew) == "true") {
      this.MapAssociateRoleComponent(associateRoleMapping);
    } else {
      this._confirmationService.confirm({
        message: "Are you sure? you want to map the Role to Associate?",
        header: "Role Updation Confirmation",
        key: "roleConfirmationDialog",
        icon: "fa fa-right",
        accept: () => {
          this.MapAssociateRoleComponent(associateRoleMapping);
        },
        reject: () => {
          return;
        }
      });
    }
    this.Clear();
  }

  private MapAssociateRoleComponent(
    associateRoleMapping: AssociateRoleMappingData
  ): void {
    this._mapAssociateRoleService
      .MapAssociateRole(associateRoleMapping)
      .subscribe(
        (res: number) => {
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate role mapped successfully.' });
          this.associatesList = [];
          // this.selectedEmployees = [];
          // this.getEmployeesByDepartmentIdAndProjectId(associateRoleMapping.DepartmentId, associateRoleMapping.ProjectId);
          this.getEmployeesByKraRoleIdAndFinancialYearId(associateRoleMapping.FinancialYearID, associateRoleMapping.KRARoleId);
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to map the role.' });
        }
      );
  }

  public Clear(): void {
    this.formSubmitted = false;
    this.isNonDelivery = true;
    this.associateRoleMapping = new AssociateRoleMappingData();
    this.associatesList = new Array<AssociateRoleMappingData>();
    // this.selectedEmployees = new Array<AssociateRoleMappingData>();
    this.rolesList = [];
    this.rolesList.push({ label: "Select KRA Group", value: null });
  }
}
