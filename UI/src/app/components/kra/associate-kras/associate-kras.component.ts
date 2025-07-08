import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren, ViewChild, Input } from "@angular/core";
import { AssociatekraService } from "../Services/associatekra.service";
import { RolemasterService } from "../Services/rolemaster.service";
import { MasterDataService } from "../../../services/masterdata.service";
import {  SelectItem } from "primeng/components/common/api";
import * as servicePath from "../../../service-paths";
import {
  OrganizationKras,
  AssociateKras
} from "../../../models/associate-kras.model";
import { GenericType } from "../../../models/dropdowntype.model";
import { MessageService } from "primeng/api";
import { DepartmentDetails } from "src/app/models/role.model";
import { DepartmentType } from "../../shared/utility/enums";
import { AssociateRoleMappingData } from "src/app/models/associaterolemappingdata.model";
import { ProjectDetails } from "src/app/models/projects.model";
import { KRAService } from "../Services/kra.service";
import { CustomKRAService } from "../Services/custom-kra.service";
import { themeconfig } from "themeconfig";
import { krastutusdata,krastatusrelation } from "../krajson";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource, MatTable, MatSort } from "@angular/material";
 




@Component({
  selector: "app-associate-kras",
  templateUrl: "./associate-kras.component.html",
  styleUrls: ["./associate-kras.component.scss"],
  providers: [
    AssociatekraService,
    RolemasterService,
    MasterDataService,
    KRAService,
    CustomKRAService
  ],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})



export class AssociateKRAsComponent implements OnInit {

  
  @ViewChild('outerSort') sort: MatSort;
  @ViewChildren('innerSort') innerSort: QueryList<MatSort>;
  @ViewChildren('innerTables') innerTables: QueryList<MatTable<any>>;

 


  themeappeareance = themeconfig.formfieldappearances;


  displayedColumns: string[] = ['kraaspect', 'noofrloetypes', 'date','status','action'];
  dataSource = krastutusdata;
  
  kraStatusdataSource : MatTableDataSource<any>;

  kraStatusRelationClmns = ['roletypes', 'noofkras', 'status'];
  innerDisplayedColumns=['kraaspect', 'metrics', 'ration'];
 
  


  isExpansionDetailRow = (index, row) => row.hasOwnProperty('detailRow');

  @Input() singleChildRowDetail: boolean;




  resources = servicePath.API.PagingConfigValue;
  loggedinUserRole: string;
  loggedInEmployeeId: number;
  public PageSize: number;
  public PageDropDown: number[];
  public selectedEmployees: AssociateRoleMappingData[] = [];
  public overideExisting = 0;
  public KraHeading: string;
  public associateKRAList: AssociateKras;
  public currentfinancialYearId: number = 0;
  public financialYearId: number = 0;
  departmentHeadDepartmentId: number;
  public financialYearsList: SelectItem[] = [];
  public projectsList: SelectItem[] = [];
  public rolesList: SelectItem[] = [];
  public departmentList: SelectItem[] = [];
  public formSubmitted: boolean = false;
  public associateRoleMapping: AssociateRoleMappingData;
  public associatesList: AssociateRoleMappingData[] = [];
  private departmentType: number;
  public isNonDelivery: boolean = true;
  _selectedFinancialYearId: number;
  showProjectsDropDown: boolean = false;
  showDepartmentsDropDown: boolean = false;
  public associateKraView: boolean = false;
  public pdfStatus: boolean = false;
  public aspectsList = [];
  public rowspansList = [];

  constructor(
    private associateKraService: AssociatekraService,
    private _kraService: KRAService,
    private masterDataService: MasterDataService,
    private _customKraService: CustomKRAService,
    private messageService: MessageService,private cd: ChangeDetectorRef
  ) {
    this.associateKRAList = new AssociateKras();
    this.associateKRAList.OrganizationKRAs = new Array<OrganizationKras>();
    // this.associateKRAList.CustomKRAs = new Array<CustomKras>();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {

    // this.kraStatusdataSource.data = [];
    this.kraStatusdataSource = new MatTableDataSource(krastatusrelation);

    this.loggedinUserRole = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.loggedInEmployeeId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.Clear();
    this.departmentType = DepartmentType.Delivery;
    this.getFinancialYears();
    this.getCurrentFinancialYear();
    this.departmentList = [];
    this.departmentList.push({ label: "Select Department", value: null });
    this.projectsList = [];
    this.projectsList.push({ label: "Select Project", value: null });

    if (
      this.loggedinUserRole.indexOf("HRM") != -1 ||
      this.loggedinUserRole.indexOf("HR Head") != -1
    ) {
      this.showProjectsDropDown = true;
      this.showDepartmentsDropDown = true;
      this.getDepartments();
    } else if (this.loggedinUserRole.indexOf("Delivery Head") != -1) {
      this.showProjectsDropDown = true;
      this.showDepartmentsDropDown = false;
      this.isNonDelivery = false;
      this.getProjects();
    } else if (this.loggedinUserRole.indexOf("Program Manager") != -1) {
      this.showProjectsDropDown = true;
      this.showDepartmentsDropDown = false;
      this.isNonDelivery = false;
      this.getEmployeesByDepartmentId(
        this.departmentType,
        this.financialYearId
      );
    } else if (this.loggedinUserRole.indexOf("Associate") != -1) {
      this.showProjectsDropDown = false;
      this.showDepartmentsDropDown = false;
    }
  }

  cols = [
    { field: "AssociateName", header: "Associate Name" },
    { field: "RoleName", header: "Role" },
    { field: "KRAGroupId", header: "View KRA" }
  ];

  cols1 = [
    { field: "KRAAspectName", header: "KRAAspect" },
    { field: "KRAAspectMetric", header: "Metric" },
    { field: "KRAAspectTarget", header: "Target" },
    { field: "AspectCount", header: "Aspect Count" }
  ];

  private getFinancialYears(): void {
    this.masterDataService.GetFinancialYears().subscribe(
      (yearsdata: GenericType[]) => {
        this.financialYearsList = [];
        this.financialYearsList.push({
          label: "Select Financial Year",
          value: null
        });
        this.rolesList = [];
        this.rolesList.push({ label: "Select Role", value: null });
        yearsdata.forEach((e: GenericType) => {
          this.financialYearsList.push({ label: e.Name, value: e.Id });
        });
      },
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Finacial Year List"
        });
      }
    );
  }

  // generatePDFforSelectedAssociates(){
  //   this.pdfStatus = true;
  //   this.associateKraService.GenerateKRAPdfSelectedAllAssociates(this.selectedEmployees).pipe(take(1)).subscribe(res =>{
  //     this.pdfStatus = false;
  //     this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Successfully generated PDFs.' });
  //   },
  //   (error: any) => {
  //     this.pdfStatus = false;
  //     this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to generate PDFs.' });
  //     }
  //   );
  // }

  public canDisableCheckbox(empInfo) {
    if (empInfo.KRAGroupId == null) {
      return true;
    }
    false;
  }

  public getCurrentFinancialYear(): void {
    this._kraService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.currentfinancialYearId = yearsdata.Id;
          this.financialYearId = yearsdata.Id;
          if (this.loggedinUserRole.indexOf("Associate") != -1) {
            this.getAssociateKRAs(this.loggedInEmployeeId, ' ', this.currentfinancialYearId);
          }
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          detail: "Failed to get current financial year!",
          summary: "Error Message"
        });
      }
    );
  }

  public generatePDFforAllAssociates() {
    this.pdfStatus = true;
    if(this.overideExisting != 1){
      this.overideExisting = 0;
    }
    this.associateKraService.generatePDFforAllAssociates(this.overideExisting).subscribe(
      (res: boolean) => {
        this.pdfStatus = false;
        if (res == true)
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "Successfully generated PDFs."
          });
        if (res == false)
          this.messageService.add({
            severity: "success",
            detail: "PDF's are already created for all associates"
          });
      },
      (error: any) => {
        this.pdfStatus = false;
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to generate PDFs."
        });
      }
    );
  }

  public generatePDF(
    selectedEmployees: AssociateRoleMappingData[],
    FinancialYearId: number
  ): void {
    if (FinancialYearId != this.currentfinancialYearId) {
      this.messageService.add({
        severity: "warn",
        summary: "Warning Message",
        detail: "You can generate PDFs only for the current financial year."
      });
    }
    // this.associateKraService.generatePDF().subscribe(( res : boolean)=>{
    //   this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Successfully generated PDFs.' });
    //   this.selectedEmployees = [];
    // },
    // (error: any) => {
    //   this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to generate PDFs.' });
    //   }
    // );
  }

  private getDepartments(): void {
    this.masterDataService.GetDepartments().subscribe(
      (res: DepartmentDetails[]) => {
        res.forEach((element: DepartmentDetails) => {
          this.departmentList.push({
            label: element.Description,
            value: element.DepartmentId
          });
        });
      },
      (error: any) => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Departments details."
        });
      }
    );
  }

  private getProjects(): void {
    this.masterDataService.GetProjectsList().subscribe(
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
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to get Projects list."
        });
      }
    );
  }

  private getProjectsByEmployeeId(): void {
    if (Number(this.loggedInEmployeeId) > 0) {
      this._customKraService
        .GetProjectsByProgramManagerId(this.loggedInEmployeeId)
        .subscribe(
          (result: GenericType[]) => {
            this.projectsList = [];
            this.projectsList.push({ label: "Select Project", value: null });
            if (result.length > 0) {
              result.forEach((element: GenericType) => {
                this.projectsList.push({
                  label: element.Name,
                  value: element.Id
                });
              });
            }
          },
          (error: any) => {
            this.messageService.add({
              severity: "error",
              summary: "Failed to get projects.",
              detail: ""
            });
          }
        );
    } else {
      this.projectsList = [];
      this.projectsList.push({ label: "Select Project", value: null });
    }
  }

  public getAssociateKRAs(
    EmployeeId: number,
    AssociateName: string,
    FinancialYearId: number
  ): void {
    this.selectedEmployees = [];
    this.aspectsList = [];
    this.rowspansList = [];
    this.KraHeading = " ";
    this.associateKRAList.OrganizationKRAs = [];
    if (FinancialYearId == null) {
      this.associateKRAList.OrganizationKRAs = [];
      // this.associateKRAList.CustomKRAs = [];
      return;
    }
    if (this.loggedinUserRole.indexOf("Associate") != -1) {
      EmployeeId = this.loggedInEmployeeId;
      this.associateKraView = false;
    } else {
      this.KraHeading = AssociateName + "'s KRAs";
      this.associateKraView = true;
    }
    this.associateKraService
      .GetAssociateKRAs(EmployeeId, FinancialYearId)
      .subscribe(
        (kraResponse: AssociateKras) => {
          this.associateKRAList.OrganizationKRAs = kraResponse.OrganizationKRAs;
          // this.associateKRAList.CustomKRAs = kraResponse.CustomKRAs;

          // Logic to calculate the RowSpan and Number of records for each KRA Aspect
          this.rowspansList = [0];
          for (
            let i = 0;
            i < this.associateKRAList.OrganizationKRAs.length;
            i++
          ) {
            var KRAAspect = this.associateKRAList.OrganizationKRAs[i]
              .KRAAspectName;
            if (
              i > 0 &&
              this.associateKRAList.OrganizationKRAs[i].KRAAspectName !=
                this.associateKRAList.OrganizationKRAs[i - 1].KRAAspectName
            ) {
              this.rowspansList.push(i);
            }
            if (
              this.associateKRAList.OrganizationKRAs[i].KRAMeasurementType ==
              "Percentage"
            ) {
              var KRAAspectTarget =
                this.associateKRAList.OrganizationKRAs[i].Operator +
                " " +
                this.associateKRAList.OrganizationKRAs[i].KRATargetValue +
                "% (" +
                this.associateKRAList.OrganizationKRAs[i].KRATargetPeriod +
                ")";
            } else {
              var KRAAspectTarget =
                this.associateKRAList.OrganizationKRAs[i].Operator +
                " " +
                this.associateKRAList.OrganizationKRAs[i].KRATargetValue +
                " (" +
                this.associateKRAList.OrganizationKRAs[i].KRATargetPeriod +
                ")";
            }
            this.aspectsList.push({
              KRAAspectName: this.associateKRAList.OrganizationKRAs[i]
                .KRAAspectName,
              KRAAspectMetric: this.associateKRAList.OrganizationKRAs[i]
                .KRAAspectMetric,
              KRAAspectTarget: KRAAspectTarget,
              AspectCount: this.associateKRAList.OrganizationKRAs.filter(
                obj => obj.KRAAspectName === KRAAspect
              ).length
            });
          }
        },
        error => {
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: "Failed to get Associate KRAs List"
          });
        }
      );
  }

  public downloadKraAspect(): void {
    this.messageService.add({
      severity: "success",
      summary: "Success Message",
      detail: "Successfully downloaded the KRA Aspects."
    });
  }

  public getEmployeesByDepartmentId(
    departmentId: number,
    financialYearId: number
  ): void {
    this.selectedEmployees = [];
    if (departmentId == null) {
      this.isNonDelivery = true;
      this.associatesList = [];
      return;
    }
    this.associatesList = new Array<AssociateRoleMappingData>();
    if (departmentId == this.departmentType) {
      if (
        this.loggedinUserRole.indexOf("HRM") != -1 ||
        this.loggedinUserRole.indexOf("HR Head") != -1
      ) {
        this.getProjects();
      } else {
        this.getProjectsByEmployeeId();
      }
      this.isNonDelivery = false;
    } else {
      this.projectsList = [];
      this.projectsList.push({ label: "Select Project", value: null });
      this.associateRoleMapping.ProjectId = null;
      let projectId: number = null;
      this.getEmployeesByDepartmentIdAndProjectId(
        departmentId,
        projectId,
        financialYearId
      );
      this.isNonDelivery = true;
    }
  }

  public getEmployeesByDepartmentIdAndProjectId(
    departmentId: number,
    projectId: number,
    financialYearId: number
  ): void {
    this.formSubmitted = true;
    this.selectedEmployees = [];
    if (
      this.loggedinUserRole.indexOf("Program Manager") != -1 ||
      this.loggedinUserRole.indexOf("Delivery Head") != -1
    )
      departmentId = this.departmentType;
    if (financialYearId == null) {
      this.associatesList = [];
      return;
    }
    if (departmentId == null) {
      this.associatesList = [];
      return;
    }
    if (departmentId == this.departmentType && projectId == null) {
      this.associatesList = [];
      return;
    }
    this.associateKraService
      .GetEmployeesByDepartmentIdAndProjectId(
        departmentId,
        projectId,
        financialYearId
      )
      .subscribe(
        (res: AssociateRoleMappingData[]) => {
          this.associatesList = [];
          this.associatesList = res;
        },
        (error: any) => {
          this.messageService.add({
            severity: "error",
            summary: "Error Message",
            detail: "Failed to get Employees list."
          });
        }
      );
  }

  public Clear(): void {
    this.formSubmitted = false;
    this.financialYearId = null;
    this.selectedEmployees = new Array<AssociateRoleMappingData>();
    if (this.loggedinUserRole == "HRM" || this.loggedinUserRole == "HR Head")
      this.isNonDelivery = true;
    else this.isNonDelivery = false;
    this.associateRoleMapping = new AssociateRoleMappingData();
    this.associatesList = new Array<AssociateRoleMappingData>();
  }

  public close(): void {
    this.associateKraView = false;
  }
}
