import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { GenericType } from "../../../models/dropdowntype.model";
import { DepartmentDetails } from "../../../models/role.model";
import { KRAGroup } from "../../../models/kragroup.model";
import { CloneKRA } from "../../../models/clonekra.model";
import { CloneKRAService } from "../Services/clonekra.service";
import { KRAService } from "../Services/kra.service";
import { MasterDataService } from "../../../services/masterdata.service";
import { MessageService } from 'primeng/api';
import { SelectItem, Message, ConfirmationService} from "primeng/components/common/api";

  @Component({
    selector: 'app-clone-kra',
    templateUrl: './clone-kra.component.html',
    styleUrls: ['./clone-kra.component.scss'],
    providers: [ ConfirmationService, MessageService]
    })

export class CloneKraComponent implements OnInit {
  private errorMessage: Message[] = [];
  private componentName: string;
  public cloneKRA: CloneKRA;
  public financialYearsList: SelectItem[] = [];
  private departmentList: SelectItem[] = [];
  public cloneTypeList: SelectItem[] = [];
  private kraGroupList: KRAGroup[] = [];
  private groupList: SelectItem[] = [];
  public cloneSubmitted: boolean = false;
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _confirmationService: ConfirmationService,
    private _masterDataService: MasterDataService,
    private _kRAService: KRAService,
     private _cloneKRAService: CloneKRAService,
    private messageService: MessageService
  ) {
    this.componentName = this._activatedRoute.routeConfig.component.name;
    this.clear();
  }

  ngOnInit() {
    this.getFinancialYearList();
    this.getCloneType();
  }

  private getFinancialYearList(): void {
    this._masterDataService.GetFinancialYears().subscribe(
      (yearsdata: GenericType[]) => {
        this.financialYearsList = [];
        this.financialYearsList.push({
          label: "Select Financial Year",
          value: 0
        });
        yearsdata.forEach((year: GenericType) => {
          this.financialYearsList.push({ label: year.Name, value: year.Id });
        });
      },
      (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get financial years.' });
      }
    );
  }

  private getFinancialYear(financialYearId:number):string{
    if(this.financialYearsList && financialYearId){
      let currentYearList:SelectItem[] = this.financialYearsList.filter(x=>x.value == financialYearId);
      if(currentYearList.length>0){
        return currentYearList[0].label;
      }
    }
  }

  public onChangeToYear(cloneKRA: CloneKRA): boolean {
    if (
      cloneKRA &&
      cloneKRA.FromFinancialYearId &&
      cloneKRA.ToFinancialYearId
    ) {
      let FromFinancialYear:string = this.getFinancialYear(cloneKRA.FromFinancialYearId);
      let ToFinancialYear:string = this.getFinancialYear(cloneKRA.ToFinancialYearId);
      if (parseInt(FromFinancialYear.slice(0, 4)) >= parseInt(ToFinancialYear.slice(0,4))) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'From Year must not be greater than or equals to To Year.' });
        return false;
      } else {
        return true;
      }
    }
  } 

  private getCloneType(): void {
    this.cloneTypeList.push({ label: "Select Clone Type", value: 0 });
    this.cloneTypeList.push({ label: "Clone by Financial Year", value: 1 });
    this.cloneTypeList.push({ label: "Clone by Department", value: 2 });
    this.cloneTypeList.push({
      label: "Clone by Department and Groups",
      value: 3
    });
  }

  public onChangeCloneType(cloneKRA: CloneKRA): void {
    this.cloneSubmitted = false;
    this.cloneKRA.DepartmentId = 0;
    this.cloneKRA.DepartmentIds = [];
    if (
      cloneKRA &&
      cloneKRA.CloneType &&
      (cloneKRA.CloneType == 2 || cloneKRA.CloneType == 3)
    ) {
      this.getDepartment(cloneKRA);
    }
  }

  private getDepartment(cloneKRA: CloneKRA): void {
    this._masterDataService.GetDepartments().subscribe(
      (res: DepartmentDetails[]) => {
        this.departmentList = [];
        if (cloneKRA.CloneType == 3) {
          this.departmentList.push({
            label: "Select Department",
            value: 0
          });
        }
        res.forEach(element => {
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

  private onChangeDepartment(cloneKRA: CloneKRA): void {
    if (
      cloneKRA &&
      cloneKRA.CloneType &&
      cloneKRA.CloneType == 3 &&
      cloneKRA.FromFinancialYearId != 0
    ) {
      if (cloneKRA.DepartmentId != 0) {
        this.cloneKRA.GroupIds = [];
        this.getGroupsByDepartment(
          cloneKRA.FromFinancialYearId,
          cloneKRA.DepartmentId
        );
      } else {
        this.cloneKRA.GroupIds = [];
        this.groupList = [];
      }
    } else if (
      cloneKRA &&
      cloneKRA.CloneType &&
      cloneKRA.CloneType == 3 &&
      cloneKRA.FromFinancialYearId == 0
    ) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'We should select from Year to get department and groups' }); 
      return;
    } else {
      return;
    }
  }

  private getGroupsByDepartment(
    financialYearId: number,
    departmentId: number
  ): void {
    this._kRAService.getKRAGroupsByFinancialYear(financialYearId).subscribe(
      (kraGroupdata: KRAGroup[]) => {
        this.kraGroupList = kraGroupdata.filter(
          i => i.DepartmentId == departmentId
        );
        this.groupList = [];
        this.kraGroupList.forEach(element => {
          this.groupList.push({
            label: element.KRATitle,
            value: element.KRAGroupId
          });
        });
      },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get KRA Groups.' });
      }
    );
  }

  public cloneKRAs(cloneKRA: CloneKRA) {
    this.cloneSubmitted = true;
    let isValid = this.onChangeToYear(cloneKRA);
    if (!isValid) return;
    if (
      cloneKRA.FromFinancialYearId == 0 ||
      cloneKRA.ToFinancialYearId == 0 ||
      cloneKRA.CloneType == 0
    )
      return;

    if (cloneKRA.CloneType == 2 && cloneKRA.DepartmentIds.length == 0) return;
    if (
      cloneKRA.CloneType == 3 &&
      (cloneKRA.DepartmentId == 0 || cloneKRA.GroupIds.length == 0)
    )
      return;
    if (cloneKRA.DepartmentId != 0)
      cloneKRA.DepartmentIds.push(cloneKRA.DepartmentId);
    this._confirmationService.confirm({
      message: "Are you sure, you want to clone?",
      header: "KRA Clone",
      key: "cloneConfirmation",
      icon: "fa fa-exclamation-circle",
      accept: () => {
        this._cloneKRAService.CloneKRAs(cloneKRA).subscribe(
          (res: number) => {
            switch (res) {
              case 0:
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Clone KRAs.' });
                break;
              case 1:
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'KRAs Cloned Successfully.' });
                break;
              case -5:
                this.messageService.add({ severity: 'info', summary: 'Information Message', detail: 'No approved KRAs to Clone.' });
                break;
              case -8:
                this.messageService.add({ severity: 'info', summary: 'Information Message', detail: 'KRAs are already Cloned.' });
                break;
              default:
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Clone KRAs.' });
                break;
            }
            this.clear();
          },
          (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to clone KRAs.' });
          }
        );
      },
      reject: () => {
        return;
      }
    });
  }

  public clear(): void {
    this.cloneSubmitted = false;
    this.cloneKRA = new CloneKRA();
    this.cloneKRA = {
      FromFinancialYearId: 0,
      ToFinancialYearId: 0,
      DepartmentId: 0,
      DepartmentIds: Array<number>(),
      GroupIds: Array<number>(),
      CloneType: 0
    };
  }
}
