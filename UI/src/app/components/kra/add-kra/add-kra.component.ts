import { Component, OnInit , Inject} from '@angular/core'; 
import { ActivatedRoute, Router } from "@angular/router";
import { DomSanitizer } from "@angular/platform-browser";
import { CommonService } from "../../../services/common.service";
import { MasterDataService } from "../../../services/masterdata.service";
import { DropDownType, GenericType } from "../../../models/dropdowntype.model";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Message, ConfirmationService } from "primeng/components/common/api";
import * as moment from "moment";
import { KRAService } from "../Services/kra.service";
import { KRAServiceForDepartmentHead } from "../Services/kra-departmenthead.service";
import { KRAGroup } from "../../../models/kragroup.model";
import { KRAComments } from "../../../models/kracomments.model";
import {  ProjectTypeData } from "../../admin/Models/projecttype.model";
import { KRAStatusCodes, DepartmentType } from "../../shared/utility/enums";
import { KRASubmittedGroup, KRAWorkFlowData } from "../../../models/kradefinition.model";
import { MessageService } from 'primeng/api';
import * as servicePath from "../../../service-paths";
import {themeconfig} from 'themeconfig';
import {kradefindata} from '../krajson';
import { MatDialog , MatDialogRef, MAT_DIALOG_DATA, MatSnackBar} from '@angular/material';
import { KradialogsComponent } from '../kradialogs/kradialogs.component';
import { ImportKraDlgComponent } from '../import-kra-dlg/import-kra-dlg.component';
import {SelectionModel} from '@angular/cdk/collections';
import {MatTableDataSource} from '@angular/material';
import { AddKRAdlgComponent } from '../add-kradlg/add-kradlg.component';
 




@Component({ 
    templateUrl: './add-kra.component.html',
    styleUrls: ['./add-kra.component.scss'],
  providers: [ KRAService, CommonService,
               ConfirmationService, MasterDataService, 
                KRAServiceForDepartmentHead, MessageService ]
  })


  

export class AddKraComponent implements OnInit {
  themeappeareance = themeconfig.formfieldappearances;


  displayedColumns: string[] = ['kraaspect', 'metrics', 'ration'];
  dataSource = kradefindata;

  importedKradisplayedColumns: string[] = ['select','kraaspect', 'metrics', 'ration'];
  selection = new SelectionModel<any>(true, []);


  addNewKRA:boolean=false;
  freezedKRA:boolean=true;
  importedKRA:boolean=false;


  isdisabled: boolean = false;
  loggedinUserRole: string;
  resources = servicePath.API.PagingConfigValue;
  componentName: string;
  roleId: number;
  financialYearId: number = 0;
  departmentHeadDepartmentId: number;
  financialYearName: string;
  kraGroupList: KRAGroup[] = [];
  kraGroupForm: FormGroup;
  kraForm: FormGroup;
  kraGroupData: KRAGroup;
  kraGroupFormSubmitted: boolean = false;
  kraFormSubmitted: boolean = false;
  departmentList: DropDownType[] = [];
  rolecategoryList: DropDownType[] = [];
  projectTypeList: DropDownType[] = [];
  dhDepartmentList: DropDownType[] = [];
  display: boolean = false;
  selectedkraGroup: KRAGroup[] = [];
  kraSetForm: FormGroup;
  heading: string;
  statusCode: string;
  financialYearsList: DropDownType[] = [];
  errorMessage: Message[] = [];
  private _selectedDepartmentId: number;
  _selectedFinancialYearId: number;
  private employeeId: number;
  showDHControls: boolean = false;
  showHRMControls: boolean = false;
  showHRHeadControls: boolean = false;
  showKRAGroupButton: boolean = false;
  IsDeliveryDept: boolean = false;
  kraComments: string;
  commentsDispaly: boolean = false;
  titleForComments: string = "";
  commentsList: KRAComments[] = [];
  showCommentsDialog: boolean = false;
  statusId: number;
  gridMessage: string = "No records found";
  // statusList:DropDownType[]=[];
  private PageSize: number;
  departmentHeadId : number;
  private PageDropDown: number[] = [];
  private selectedKRAGroup: KRAGroup;
  public kratitleplaceholder = "";
  constructor(
    private _kraService: KRAService,
    private masterDataService: MasterDataService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _confirmationService: ConfirmationService,
    private _kraServiceForDH: KRAServiceForDepartmentHead,
    private sanitizer: DomSanitizer,
    private messageService: MessageService,
    //private _roleStatusService: RoleStatusService
    public dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.componentName = this.actRoute.routeConfig.component.name;
    this.kraGroupList = new Array<KRAGroup>();
    this.kraGroupData = new KRAGroup();
    this.selectedkraGroup = new Array<KRAGroup>();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.loggedinUserRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.actRoute.params.subscribe(params => { this._selectedDepartmentId = params["departmentId"];  });
    this.actRoute.params.subscribe(params => { this._selectedFinancialYearId = params["financialYearId"];  });
    if (this._selectedDepartmentId) {
      this.departmentHeadDepartmentId = this._selectedDepartmentId;
    }
    if (this._selectedFinancialYearId) {
      this.financialYearId = this._selectedFinancialYearId;
    }
    if (this.financialYearId == 0) {
      this.getCurrentFinancialYear();
    }
    if (this.loggedinUserRole.indexOf("Department Head") != -1) {
      this.getDepartmentHeadDepartments();
      this.showDHControls = true;
      this.showHRMControls = false;
      this.showHRHeadControls = false;
    }
     else if (this.loggedinUserRole.indexOf("HRM") != -1) {
      this.getDepartments();
      this.showDHControls = false;
      this.showHRMControls = true;
      this.showHRHeadControls = false;
    }
    else if (this.loggedinUserRole.indexOf("HR Head") != -1) {
      this.getDepartments();
      this.showDHControls = false;
      this.showHRMControls = false;
      this.showHRHeadControls = true;
    }
    this.getFinancialYears();
    // this.getStatuses(this.loggedinUserRole,CategoryMaster.KRA);
    this.kraForm = this._formBuilder.group({
      ddlCurrentYear: ["", [Validators.required]],
      DHdepartment: [""]
 
    });
    this.kraSetForm = this._formBuilder.group({
      aspect: ["", [Validators.required]],
      metric: ["", [Validators.required]],
      target: ["", [Validators.required]]
    });
    this.kraGroupForm = this._formBuilder.group({
      department: [null, [Validators.required]],
      rolecategory: [null, [Validators.required]],
      projecttype: [null],
      kraTitle: ["", [Validators.required, Validators.pattern('^[a-zA-Z0-9,&/_ -–]*$')]]
    });
  }
    cols = [
      { field: 'KRAGroupId', header: 'KRA Group Id', display: 'none'},
      { field: 'StatusId', header: 'Status Id', display: 'none'},
      { field: 'DepartmentName', header: 'Department Name', display: 'none' },
      { field: 'RoleCategoryName', header: 'Role Type', display: 'table-cell' },
      { field: 'KRATitle', header: 'Role Type Instance', display: 'table-cell' },
      { field: 'StatusDescription', header: 'Status', display: 'table-cell' },
      ];
  // getStatuses(roleName:string,categoryId:number){
  //   this.statusList = [];       
  //   this.statusList.push({ label: "All Statuses", value: null });
  //   this._roleStatusService.GetStatusByRole(roleName,categoryId).subscribe((res: GenericType[]) => {        
  //       res.forEach(element => {
  //          this.statusList.push({ label: element.Name, value: element.Id });
  //       });        
  //     },
  //     (error: any) => {
  //       if (error._body != undefined && error._body != "")
  //         this._commonService
  //           .LogError(this.componentName, error._body)
  //           .then((data: any) => {});
  //       this.growlerrormessage(
  //         "error",
  //         "Failed to get Status details.",
  //         ""
  //       );
  //     }
  //   );
  // }

  checkValue(kraData){
    if (this.loggedinUserRole.indexOf("HRM") != -1 && (kraData.StatusId == KRAStatusCodes.SubmittedForDepartmentHeadReview || kraData.StatusId == KRAStatusCodes.SubmittedForHRHeadReview || kraData.StatusId == KRAStatusCodes.SendBackForDepartmentHeadReview || kraData.StatusId == KRAStatusCodes.Approved)) {
      return true;
    }
    else if (this.loggedinUserRole.indexOf("Department Head") != -1) {
      if (kraData.StatusId == KRAStatusCodes.Draft || kraData.StatusId == KRAStatusCodes.Approved ||kraData.StatusId == KRAStatusCodes.SendBackForHRMReview || kraData.StatusId == KRAStatusCodes.SubmittedForHRHeadReview)
      return true;
    }
    else if (this.loggedinUserRole.indexOf("HR Head") != -1 && (kraData.StatusId != KRAStatusCodes.SubmittedForHRHeadReview)) {
      return true;
    }
    else
      return false;
  }
  getDepartments() {
    this.masterDataService.GetDepartments().subscribe(
      (res: any[]) => {
        this.departmentList = [];
        this.dhDepartmentList = [];
        this.departmentList.push({ label: "Select Department", value: null });
        res.forEach(element => {
          this.departmentList.push({
            label: element.Description,
            value: element.DepartmentId
          });
        });
        this.dhDepartmentList = this.departmentList;
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

  private getDepartmentHeadDepartments(): void {
    this.dhDepartmentList = [];
    this.departmentList = [];
    this.dhDepartmentList.push({ label: "Select Department", value: null });
    if (this.employeeId > 0) {
      this._kraServiceForDH.GetDepartmentsById(this.employeeId).subscribe(
        (data: GenericType[]) => {
          data.forEach(element => {
            this.dhDepartmentList.push({ label: element.Name, value: element.Id  });
          });
          if( this.dhDepartmentList.length == 2 && this.loggedinUserRole.indexOf("Department Head") != -1 ) {
            this.departmentHeadDepartmentId = this.dhDepartmentList[1].value;
            this.getKRAGroups();
          }
          this.departmentList = this.dhDepartmentList;
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
  }

  public getCurrentFinancialYear(): void {
    this._kraService.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.financialYearId = yearsdata.Id;
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get  current financial years!', 
          summary: 'Error Message'
        });
      }
    );
  }

  getProjectTypes() {
    this.masterDataService.GetProjectTypes().subscribe(
      (res: ProjectTypeData[]) => {
        this.projectTypeList = [];
        this.projectTypeList.push({
          label: "Select Project Type",
          value: null
        });
        res.forEach(element => {
          this.projectTypeList.push({
            label: element.ProjectTypeCode,
            value: element.ProjectTypeId
          });
        });
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get Project Types.', 
          summary: 'Error Message'
        });
      }
    );
  }

  getRoleCategories() {
    this.masterDataService.GetRoleCategories().subscribe(
      (res: GenericType[]) => {
        this.rolecategoryList = [];
        this.rolecategoryList.push({
          label: "Select Role Type",
          value: null
        });
        res.forEach(element => {
          this.rolecategoryList.push({
            label: element.Name,
            value: element.Id
          });
        });
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get Role Categories.', 
          summary: 'Error Message'
        });
      }
    );
  }

  private getFinancialYears(): void {
    this.masterDataService.GetFinancialYears().subscribe(
      (yearsdata: GenericType[]) => {
        this.financialYearsList = [];
        this.financialYearsList.push({
          label: "Select Financial Year",
          value: null
        });
        yearsdata.forEach((e: GenericType) => {
          this.financialYearsList.push({ label: e.Name, value: e.Id });
        });
        if (this.departmentHeadDepartmentId > 0 && this.financialYearId > 0)
          this.getKRAGroups();
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get financial years.', 
          summary: 'Error Message'
        });
      }
    );
  }
  public getKRAGroups(): void {
    this.kraGroupList = [];
    this.selectedkraGroup = [];
    let checkStatus: string;
    if (this.financialYearId != null && this.departmentHeadDepartmentId != null) {
      this.financialYearName = this.financialYearsList.filter(i => i.value == this.financialYearId)[0].label;
      this._kraService.getKRAGroups(this.financialYearId).subscribe((kraGroupdata: KRAGroup[]) => {
        this.kraGroupList = kraGroupdata;
        console.log(this.kraGroupList);
        this.kraGroupList = this.kraGroupList.filter(i => i.DepartmentId == this.departmentHeadDepartmentId);
        if (this.loggedinUserRole.indexOf("HRM") != -1) {
          this.kraGroupList = this.kraGroupList.filter(i => i.DepartmentId == this.departmentHeadDepartmentId);
          this.showKRAGroupButton = true;
        if(this.kraGroupList.length <= 0)
        this.gridMessage = "No records found";
        else
        this.gridMessage = "";
        }
        else if (this.loggedinUserRole.indexOf("Department Head") != -1) {
          this.kraGroupList = this.kraGroupList.filter(i =>
            i.DepartmentId == this.departmentHeadDepartmentId && i.StatusId != KRAStatusCodes.Draft
          );
          this.showKRAGroupButton = false;
        if(this.kraGroupList.length <= 0)
        this.gridMessage = "No records found";
        else
        this.gridMessage = "";
        }
        else if (this.loggedinUserRole.indexOf("HR Head") != -1) {
          this.kraGroupList = this.kraGroupList.filter(i => i.StatusId == KRAStatusCodes.SubmittedForHRHeadReview || i.StatusId == KRAStatusCodes.SendBackForDepartmentHeadReview || i.StatusId == KRAStatusCodes.Approved);
          this.showKRAGroupButton = false;
        if(this.kraGroupList.length <= 0)
        this.gridMessage = "No records found";
        else
        this.gridMessage = "";
        }
      },
      (error: any) => {
        this.messageService.add({
          severity: 'error',
          detail: 'Failed to get KRA Groups.', 
          summary: 'Error Message'
        });
      }
      );
    }
  }

  // Double click is debricated in ng-prime so we using eye icon.
  // onGroupRowSelect(event) {
  //   // console.log(this.selectedkraGroup);
  //   console.log("kraGroupList");
  //   // console.log(selectedkraGroup);
  //   console.log("cols");
  //   console.log(event);
  //   // let statusId = selectedkraGroup.StatusId;
  //   // if ((this.loggedinUserRole.indexOf("HRM") != -1 && (statusId == KRAStatusCodes.Draft || statusId == KRAStatusCodes.SendBackForHRMReview)) || (this.loggedinUserRole.indexOf("Department Head") != -1 && (statusId == KRAStatusCodes.SubmittedForDepartmentHeadReview || statusId == KRAStatusCodes.SendBackForDepartmentHeadReview)) || (this.loggedinUserRole.indexOf("HR Head") != -1 && (statusId == KRAStatusCodes.SubmittedForHRHeadReview))) {
  //   //   this.addKraDefinition(selectedkraGroup);
  //   // } else {
  //   //   this.viewKraDefinition(selectedkraGroup);
  //   // }
  // }
  private  onGroupRowSelect(selectedData:  KRAGroup):  void  {
    let  statusId  =  selectedData.StatusId;
    if  ((this.loggedinUserRole.indexOf("HRM") != -1  &&  (statusId  ==  KRAStatusCodes.Draft  ||  statusId  ==  KRAStatusCodes.SendBackForHRMReview))  ||  (this.loggedinUserRole.indexOf("Department Head")  !=  -1  &&  (statusId  ==  KRAStatusCodes.SubmittedForDepartmentHeadReview  ||  statusId  ==  KRAStatusCodes.SendBackForDepartmentHeadReview))  ||  (this.loggedinUserRole.indexOf("HR Head")  !=  -1  &&  (statusId  ==  KRAStatusCodes.SubmittedForHRHeadReview))) {
      this.addKraDefinition(selectedData);
    }  else  {
      this.viewKraDefinition(selectedData);
    }
  } 


  private viewKraDefinition(selectedKraGroup: KRAGroup) {
    selectedKraGroup.FinancialYearId=this.financialYearId;
    selectedKraGroup.DepartmentId=this.departmentHeadDepartmentId;
    sessionStorage["KraGroupDetails"] = JSON.stringify(selectedKraGroup);
    this._router.navigate(["/kra/addkradefinition/view/" + this.financialYearName]);
  }

  private addKraDefinition(selectedKraGroup: KRAGroup) {
    selectedKraGroup.FinancialYearId=this.financialYearId;
    selectedKraGroup.DepartmentId=this.departmentHeadDepartmentId;
    sessionStorage["KraGroupDetails"] = JSON.stringify(selectedKraGroup);
    this._router.navigate(["/kra/addkradefinition/add/" + this.financialYearName]);
  }

  private deletekraGroup(selectedKraGroup: KRAGroup) {
    this._confirmationService.confirm({
      message:
        "All the KRA aspects for the current Group will be deleted. Do you want to proceed?",
      header: "KRA Group Confirmation",
      key: "kraGroupConfirmation",
      icon: "fa fa-trash",
      accept: () => {
        this.deleteKRAGroup(selectedKraGroup);
        this.getKRAGroups();
        this.kraGroupFormSubmitted = false;
      },
      reject: () => { }
    });
  }

  private deleteKRAGroup(selectedKraGroup: KRAGroup) {
    let financialYearId = this.financialYearId;
    let objKRAsubmittedGroup: KRASubmittedGroup = new KRASubmittedGroup();
    objKRAsubmittedGroup.FinancialYearId = this.financialYearId;
    objKRAsubmittedGroup.RoleName = this.loggedinUserRole;
    objKRAsubmittedGroup.fromEmployeeId = this.employeeId;
    objKRAsubmittedGroup.KRAGroupIds = [];
    objKRAsubmittedGroup.KRAGroupIds.push(selectedKraGroup.KRAGroupId);
    this._kraService.deleteKRAGroup(objKRAsubmittedGroup).subscribe((res: number) => {
      if (res == 1) {

        this.messageService.add({ severity: 'success', detail: 'KRA Group deleted successfully.', summary: '' }); 
        this.getKRAGroups();
      }
      else
      this.messageService.add({ severity: 'error', detail: 'Unauthorize access to delete KRA Group.', summary: '' }); 
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        detail: 'Failed to delete KRA Group', 
        summary: 'Error Message'
      });
    });
  }

  private addKraGroup(): void {
    this.kraFormSubmitted = true;
    if (
      this.financialYearId != null &&
      this.departmentHeadDepartmentId != null
    ) {
      this.getProjectTypes();
      this.getRoleCategories();
      this.heading = "Add KRA Group - (" + this.financialYearName + ")";
      this.kraGroupData = new KRAGroup();
      this.kraGroupData.DepartmentId = this.departmentHeadDepartmentId;
      this.kraGroupForm.controls['department'].setValue(this.departmentHeadDepartmentId);
      this.kraGroupData.FinancialYearId = this.financialYearId;
      this.display = true;
      if (this.kraGroupData.DepartmentId == DepartmentType.Delivery) {
        this.IsDeliveryDept = true;
        this.kratitleplaceholder = "RoleType - ProjectType";
      }
      else {
        this.IsDeliveryDept = false;
        this.kraGroupData.ProjectTypeId = null;
        this.kraGroupData.KRATitle = "";
        this.kratitleplaceholder = "RoleType";
      }
      this.kraGroupFormSubmitted = false;
      this.kraGroupForm.controls["rolecategory"].reset();
      this.kraGroupForm.controls["projecttype"].reset();
    }
  }

  // private filterForStatus(kraGroupList: KRAGroup[]): boolean {
  //   let kraGroup: KRAGroup[] = _.filter(kraGroupList, function(
  //     kraGroupOfList: KRAGroup
  //   ) {
  //     return (kraGroupOfList.StatusId == KRAStatusCodes.Draft);
  //   });
  //   if (kraGroup.length > 0) {
  //     return true;
  //   } else return false;
  // }
  
  private CallService(byWhom: string) {

    this.kraFormSubmitted = true;
    this.titleForComments = "";
    if (
      this.financialYearId != null &&
      this.departmentHeadDepartmentId != null
    ) {
      if (this.kraGroupList.length > 0) {      
        if (this.selectedkraGroup.length > 0) {
          let objkrawrorkflow = new KRAWorkFlowData();
          objkrawrorkflow.FinancialYearID = this.financialYearId;
          objkrawrorkflow.FromEmployeeID = this.employeeId;
          objkrawrorkflow.DepartmentID = this.departmentHeadDepartmentId;
          this.selectedkraGroup.filter(i => {
            objkrawrorkflow.KRAGroupIDs.push({
              Id: i.KRAGroupId,
              Name: i.KRATitle
            });
          });
          let groupid: number;
          let kratitle: string = "";
          objkrawrorkflow.KRAGroupIDs.forEach(i => {
            this.selectedkraGroup.filter(j => {
              if (j.KRAGroupId == i.Id && j.KRACount == 0)
                kratitle = j.KRATitle + "," + kratitle;
              if (j.KRAGroupId == i.Id)
                this.titleForComments =
                  j.KRATitle + ", " + this.titleForComments;
            });
          });
          kratitle = kratitle.slice(0, -1);
          this.titleForComments =
            "Comments for " +
            this.titleForComments.substr(
              0,
              this.titleForComments.lastIndexOf(",")
            );
          if (byWhom == "HRM" && kratitle == "")
            this.SendForDepartmentApproval(objkrawrorkflow);
          else if (byWhom == "SendBacktoHRM" && kratitle == "")
            this.SendBacktoHRM(objkrawrorkflow);
          else if (byWhom == "SendtoHRHead" && kratitle == "")
            this.SendtoHRHead(objkrawrorkflow);
          else if (byWhom == "SendBacktoDH" && kratitle == "")
            this.SendBacktoDepartmentHead(objkrawrorkflow);
          else if (byWhom == "HRHead" && kratitle == "")
            this.Accept(objkrawrorkflow);
          else
            this.messageService.add({ severity: 'error', detail: 'No KRAs are defined for ' + kratitle + ' group(s)', summary: 'Error Message' }); 
        } else {
          this.messageService.add({ severity: 'error', detail: 'Select atleast one KRA Group.', summary: 'Error Message' }); 
        }
      } else {
        this.messageService.add({ severity: 'error', detail: 'No KRA Groups are available.', summary: 'Error Message' }); 

      }
    }
  }
  private SendForDepartmentApproval(objkrawrorkflow: KRAWorkFlowData) {
    this._kraService.sendForDepartmentApproval(objkrawrorkflow).subscribe(
      (data: any) => {
        switch (data) {
          case 0:
          this.messageService.add({ severity: 'error', detail: 'Failed to submit KRA Group for approval', summary: 'Error Message' });
            break;
          case 1:
          this.messageService.add({ severity: 'success',
           detail: 'KRA groups are submitted for department head approval.', summary: 'Success Message' }); 
            this.commentsDispaly = false;
            this.getKRAGroups();
            break;
          case 2:
          this.messageService.add({ severity: 'error',
           detail: 'No department head for the selected department', summary: 'Error Message' }); 
            break;
          case 3:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the department head', summary: 'Error Message' });             
            break;
          case 5:
          this.messageService.add({ severity: 'error',
           detail: 'Some KRA Groups already submitted for action.', summary: 'Error Message' }); 
            break;
          case -3:
          this.messageService.add({ severity: 'error',
           detail: 'KRA groups are already submitted for approval.', summary: 'Error Message' });
           break;
          default:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to submit KRA Group for approval', summary: 'Error Message' });
            break;
        }
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Failed to submit KRA Group for approval', 
            summary: 'Error Message'
          });
        }
      }
    );
  }

  private Accept(objkrawrorkflow: KRAWorkFlowData) {
    this._kraService.approveKRA(objkrawrorkflow).subscribe(
      (data: any) => {
        switch (data) {
          case 0:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to approve KRA Groups', summary: 'Error Message' });
            break;
          case 1:
          this.messageService.add({ severity: 'success',
           detail: 'KRA groups are approved.', summary: 'Success Message' });          
            this.getKRAGroups();
            break;
          case 2:
          this.messageService.add({ severity: 'error',
           detail: 'No HR Head for the selected department.', summary: 'Error Message' });
            break;
          case 3:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the HR Head.', summary: 'Error Message' });
            break;
          case -4:
          this.messageService.add({ severity: 'warn',
           detail: 'KRA groups are already approved.', summary: 'Warn Message' });
            break;
          case 5:
          this.messageService.add({ severity: 'error',
           detail: 'Some KRA Groups already submitted for action.', summary: 'Error Message' });
            break;
          default:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to approve KRA Groups.', summary: 'Error Message' });
            break;
        }
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Failed to approve KRA Groups.', 
            summary: 'Error Message'
          });
        }
      }
    );
  }
  private SendBacktoHRM(objkrawrorkflow: KRAWorkFlowData) {
    this._kraService.SendBackForHRMReview(objkrawrorkflow).subscribe(
      (data: any) => {
        switch (data) {
          case 0:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to send back to HRM.', summary: 'Error Message' });
            break;
          case 1:
          this.messageService.add({ severity: 'success',
           detail: 'Successfully sent back to HRM.', summary: 'Success Message' });
            this.commentsDispaly = false;
            this.getKRAGroups();
            break;
          case 2:
          this.messageService.add({ severity: 'error',
           detail: 'No department head for the selected department', summary: 'Error Message' });
            break;
          case 3:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the HRM.', summary: 'Error Message' });
            break;
          case -4:
          this.messageService.add({ severity: 'warn',
           detail: 'KRA groups are already approved.', summary: 'Warn Message' });
            break;
          case 5:
          this.messageService.add({ severity: 'error',
           detail: 'Some KRA Groups already submitted for action.', summary: 'Error Message' });
            break;
          default:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the HRM', summary: 'Error Message' });
            break;
        }
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Failed to approve KRA Groups.', 
            summary: 'Error Message'
          });
        }
      }
    );
  }
  private SendBacktoDepartmentHead(objkrawrorkflow: KRAWorkFlowData) {
    this._kraService.SendBacktoDepartmentHead(objkrawrorkflow).subscribe(
      (data: any) => {
        switch (data) {
          case 0:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to send back to Department Head.', summary: 'Error Message' });
            break;
          case 1:
          this.messageService.add({ severity: 'success',
           detail: 'Successfully sent back to Department Head', summary: 'Error Message' });
            this.commentsDispaly = false;
            this.getKRAGroups();
            break;
          case 2:
          this.messageService.add({ severity: 'error',
           detail: 'No department head for the selected department.', summary: 'Error Message' });
            break;
          case 3:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the department head.', summary: 'Error Message' });
            break;
          case -4:
          this.messageService.add({ severity: 'warn',
           detail: 'KRA groups are already approved.', summary: 'Warn Message' });
            break;
          case 5:
          this.messageService.add({ severity: 'error',
           detail: 'Some KRA Groups already submitted for action.', summary: 'Error Message' });
            break;
          default:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the department head.', summary: 'Error Message' });
            break;
        }
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Unable to submit to the department head.', 
            summary: 'Error Message'
          });
        }
      }
    );
  }
  private SendtoHRHead(objkrawrorkflow: KRAWorkFlowData) {
    this._kraService.SendtoHRHeadReview(objkrawrorkflow).subscribe(
      (data: any) => {
        switch (data) {
          case 0:
          this.messageService.add({ severity: 'error',
           detail: 'Failed to send to HR Head.', summary: 'Error Message' });
            break;
          case 1:
          this.messageService.add({
            severity: 'success',
            detail: 'Successfully sent to HR Head',
            summary: 'Success Message'
            }); 
            this.commentsDispaly = false;
            this.getKRAGroups();
            break;
          case 2:
          this.messageService.add({ severity: 'error',
           detail: 'No HR head for the selected department', summary: 'Error Message' });
            break;
          case 3:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to HR head', summary: 'Error Message' });
            break;
          case -4:
          this.messageService.add({ severity: 'warn',
           detail: 'KRA groups are already approved.', summary: 'Error Message' });
            break;
          case 5:
          this.messageService.add({ severity: 'error',
           detail: 'Some KRA Groups already submitted for action.', summary: 'Error Message' });
            break;
          default:
          this.messageService.add({ severity: 'error',
           detail: 'Unable to submit to the HR head.', summary: 'Error Message' });
            break;
        }
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Unable to submit to the HR head.', 
            summary: 'Error Message'
          });
        }
      }
    );
  }
  public setKRATitle() {    
    let kraTitle: string = "";
    if (this.kraGroupData.RoleCategoryId != null) {
      kraTitle = this.rolecategoryList.filter(
        i => i.value == this.kraGroupData.RoleCategoryId
      )[0].label;
    }
    if (this.kraGroupData.ProjectTypeId != null) {
      if (kraTitle != "")
        kraTitle =
          kraTitle +
          " - " +
          this.projectTypeList.filter(
            i => i.value == this.kraGroupData.ProjectTypeId
          )[0].label;
      else
        kraTitle = this.projectTypeList.filter(
          i => i.value == this.kraGroupData.ProjectTypeId
        )[0].label;
    }
    this.kraGroupData.KRATitle = kraTitle.trim();
  }
  public saveKRAGroup(): void {
    this.kraGroupFormSubmitted = true;
    if (this.IsDeliveryDept && this.kraGroupData.ProjectTypeId == null) return;
    if (this.kraGroupForm.valid && this.kraGroupData.KRATitle.trim().length == 0) {
      this.errorMessage = [];
      this.errorMessage.push({ severity: "warn", detail: "Please give valid Role Type Instance!" });
    } else this.createKraGroup();
  }
  public cancelKRAGroup(): void {
    this.display = false;
    this.kraGroupForm.reset();
    this.kraGroupFormSubmitted = false;
  }

  // private kraGroupConfirmation() {
  //   let kraduplicategrouplist:any[]=[];    
  //   if (this.kraGroupForm.valid) {
  //     if (this.kraGroupList.length > 0) {
  //       this.kraGroupList.forEach((i: KRAGroup) => {
  //         if (i.DepartmentId == this.kraGroupData.DepartmentId && i.RoleCategoryId == this.kraGroupData.RoleCategoryId && i.ProjectTypeId == this.kraGroupData.ProjectTypeId) {
  //           kraduplicategrouplist.push({DepartmentId:i.DepartmentId,RoleCategoryId:i.RoleCategoryId,ProjectTypeId:i.ProjectTypeId})
  //         }
  //       });
  //       if(kraduplicategrouplist.length>0){
  //         this._confirmationService.confirm({
  //           message: "KRA group already exits.Are you sure, you want to create this?",
  //           header: "KRA Group Create Confirmation",
  //           key: "kraGroupConfirmation",
  //           icon: "fa fa-trash",
  //           accept: () => {
  //             this.createKraGroup();
  //             this.kraGroupFormSubmitted = false;
  //             return;
  //           },
  //           reject: () => { }
  //         });
  //       }
  //       else
  //       {
  //         this.createKraGroup();
  //       }
  //     }     
  //     else {
  //       this.createKraGroup();
  //     }
  //   }
  // }
  private createKraGroup(): void {
    if (this.kraGroupForm.valid) {
      this._kraService.createKRAGroup(this.kraGroupData).subscribe(
        (data: any) => {
          if (data > 0)
          this.messageService.add({ severity: 'success',
           detail: 'KRA Group added successfully', summary: 'Success Message' });
          else if (data == -1)
          this.messageService.add({ severity: 'error',
           detail: 'KRA Group already exists.', summary: 'Error Message' });
          this.getKRAGroups();
          this.cancelKRAGroup();
        },
        error => {
          (error: any) => {
            this.messageService.add({
              severity: 'error',
              detail: 'Failed to add KRA Group.', 
              summary: 'Error Message'
            });
          }
        }
      );
    }
  }


  public SaveComents() {
    if (this.selectedKRAGroup) {
      let objkrawrorkflow = new KRAWorkFlowData();
      objkrawrorkflow.FinancialYearID = this.financialYearId;
      objkrawrorkflow.FromEmployeeID = this.employeeId;
      objkrawrorkflow.DepartmentID = this.departmentHeadDepartmentId;
      objkrawrorkflow.StatusId = this.selectedKRAGroup.StatusId;
      objkrawrorkflow.KRAGroupIDs.push({
        Id: this.selectedKRAGroup.KRAGroupId,
        Name: this.selectedKRAGroup.KRATitle
      });
      if (this.kraComments) {
        if (this.kraComments.length > 0) {
          objkrawrorkflow.Comments =  this.kraComments.trim();
          this._kraService.addKRAComments(objkrawrorkflow).subscribe(
            (response: number) => {
              if (response == 1) {
                this.commentsDispaly = false;
                this.messageService.add({ severity: 'success',
                 detail: 'Comments saved Successfully', summary: 'Success Message' });
              } else if (response == -11) {
                this.messageService.add({ severity: 'warn',
                 detail: 'Nothing to Save', summary: 'Warn Message' });
              } else if (response == -12) {
                this.messageService.add({ severity: 'warn',
                 detail: 'Cannot provide comments for Approved KRA.', summary: 'Warn Message' });
              } else {
                this.messageService.add({ severity: 'error',
                 detail: 'Failed to Save Comments.', summary: 'Error Message' });
              }
            },
            error => {
              (error: any) => {
                this.messageService.add({
                  severity: 'error',
                  detail: 'Failed to add Comments', 
                  summary: 'Error Message'
                });
              }
              if (error._body != undefined && error._body != "")
              this.messageService.add({ severity: 'error',
                 detail: 'Failed to add Comments', summary: 'Error Message' });
            }
          );
        }
        else
        this.messageService.add({ severity: 'warn',
                 detail: 'Nothing to Save.', summary: 'Warn Message' });
      }
    }
  }

  public CancelComments() {
    this.commentsDispaly = false;
    this.kraComments = null;
  }

  private transformSanitizer(style: string) {
    let appreciateString = this.sanitizer.bypassSecurityTrustHtml(style);
    return appreciateString;
  }

  private addComment(selectedData: KRAGroup): void {
    if (selectedData.StatusId == KRAStatusCodes.Approved) {
      this.messageService.add({ severity: 'warn',detail: 'Cannot provide comments for Approved KRA.', summary: 'Warn Message' });
      return;
    } else {
      this.commentsDispaly = true;
      this.kraComments = null;
      if (selectedData && selectedData.KRATitle) {
        this.selectedKRAGroup = new KRAGroup();
        this.selectedKRAGroup = selectedData;
        this.titleForComments = "Comments for " + selectedData.KRATitle;
      }
    }
  }
  private showComments(financialYearId: number, kraGroupId: number) {
    this.showCommentsDialog = true;
    this._kraService.getKRAComments(financialYearId, kraGroupId).subscribe(
      (data: KRAComments[]) => {
        this.commentsList = data;
        console.log(this.commentsList);
        this.commentsList.forEach((data: KRAComments) => {
          data.CommentedDate = moment(data.CommentedDate).format("MM-DD-YYYY");
          data.Comments = this._commonService.htmlDecode(data.Comments);
        });
      },
      error => {
        (error: any) => {
          this.messageService.add({
            severity: 'error',
            detail: 'Failed to add KRA Group', 
            summary: 'Error Message'
          });
        }
        if (error._body != undefined && error._body != "")
        this.messageService.add({ severity: 'error',
                 detail: 'Failed to add KRA Group.', summary: 'Error Message' });
      }
    );
  }

 
  //Table check box and header events
  onRowSelect(event: any) {
    if (this.loggedinUserRole.indexOf("HRM") != -1 && (event.data.StatusId == KRAStatusCodes.SubmittedForDepartmentHeadReview || event.data.StatusId == KRAStatusCodes.SubmittedForHRHeadReview || event.data.StatusId == KRAStatusCodes.SendBackForDepartmentHeadReview || event.data.StatusId == KRAStatusCodes.Approved)) {
      this.selectedkraGroup.splice(this.findIndex(event.data.KRAGroupId, this.selectedkraGroup), 1);
    }
    else if (this.loggedinUserRole.indexOf("Department Head") != -1) {
      if (event.data.StatusId == KRAStatusCodes.Draft || event.data.StatusId == KRAStatusCodes.Approved || event.data.StatusId == KRAStatusCodes.SendBackForHRMReview || event.data.StatusId == KRAStatusCodes.SubmittedForHRHeadReview)
        this.selectedkraGroup.splice(this.findIndex(event.data.KRAGroupId, this.selectedkraGroup), 1);
    }
    else if (this.loggedinUserRole.indexOf("HR Head") != -1 && (event.data.StatusId != KRAStatusCodes.SubmittedForHRHeadReview)) {
      this.selectedkraGroup.splice(this.findIndex(event.data.KRAGroupId, this.selectedkraGroup), 1);
    }
  }
  findIndex(kraGroupId: number, updateresultArray: any[]) {
    let index = -1;
    for (let i = 0; i < updateresultArray.length; i++) {
      if (kraGroupId === updateresultArray[i].KRAGroupId) {
        index = i;
        break;
      }
    }
    return index;
  }
  onHeaderCheckboxToggle(event: any) {
    if (event.checked) {
      let groupsTodelete: any[] = [];
      if (this.loggedinUserRole.indexOf("HRM") != -1) {
        groupsTodelete = this.selectedkraGroup.filter(i => i.StatusId != KRAStatusCodes.Draft && i.StatusId != KRAStatusCodes.SendBackForHRMReview);
        for (let i = 0; i < groupsTodelete.length; i++) {
          let index: number = this.findIndex(groupsTodelete[i].KRAGroupId, this.selectedkraGroup);
          this.selectedkraGroup.splice(index, 1);
        }
      }
      else if (this.loggedinUserRole.indexOf("Department Head") != -1) {
        groupsTodelete = this.selectedkraGroup.filter(i => i.StatusId != KRAStatusCodes.SubmittedForDepartmentHeadReview && i.StatusId != KRAStatusCodes.SendBackForDepartmentHeadReview);
        for (let i = 0; i < groupsTodelete.length; i++) {
          let index: number = this.findIndex(groupsTodelete[i].KRAGroupId, this.selectedkraGroup);
          this.selectedkraGroup.splice(index, 1);
        }
      }
      else if (this.loggedinUserRole.indexOf("HR Head") != -1) {
        groupsTodelete = this.selectedkraGroup.filter(i => i.StatusId != KRAStatusCodes.SubmittedForHRHeadReview);
        for (let i = 0; i < groupsTodelete.length; i++) {
          let index: number = this.findIndex(groupsTodelete[i].KRAGroupId, this.selectedkraGroup);
          this.selectedkraGroup.splice(index, 1);
        }
      }
    }
  }

  public closeComments(): void {
    this.showCommentsDialog = false;
  }


  changeValue(selectedValue){
    if(selectedValue.value=="2019-2020"){
      this.openDialog('450px','','Do you have any new departments, grades or role types to be defined?',['No','Yes']);
    }
  }

  openDialog(dlgwidth,heading,bodydata,footerbtns): void {
    const dialogRef = this.dialog.open(KradialogsComponent, {
      width: dlgwidth,
      data: {dlgHeadingData: heading, dlgBodyData: bodydata,dlgFooterBtns: footerbtns}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.addNewKRA=result;
      this.freezedKRA=!result;
    });
  }

  openImportKRA(): void {
    const dialogRef = this.dialog.open(ImportKraDlgComponent, {
      data:"Import Kra Data"
    });
    dialogRef.afterClosed().subscribe(result => {
      this.importedKRA=result;
      this.addNewKRA=!result;
      this.freezedKRA=!result;
      this.openSnackBar("KRA's imported successfully!", "");
    });
  }


  
   /** Whether the number of selected elements matches the total number of rows. */
   isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
 


  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000,
    });
  }



}
