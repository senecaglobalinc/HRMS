import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";
import {
  SelectItem,
  MessageService
} from "primeng/api";
import { SowService } from "../services/sow.service";
import { SOW } from "../../admin/models/sow.model";
import { ProjectDetails, ProjectsData } from "../../../models/projects.model";
import * as moment from "moment";
import { ProjectCreationService } from "../services/project-creation.service";
import * as servicePath from "../../../service-paths";
import { ConfirmationService } from "primeng/components/common/confirmationservice";
import { CommonService } from "../../../services/common.service";
import { Subscription } from "rxjs";

@Component({
  selector: "app-sow",
  templateUrl: "./sow.component.html",
  styleUrls: ["./sow.component.scss"],
  providers: [MessageService, ConfirmationService]
})
export class SOWComponent implements OnInit {
  editMode = false;
  AddendumformSubmitted = false;
  btnLabel = "Add";
  CancelBtnLabelSOW = "Clear";
  CancelBtnLabelAddendum = "Clear"
  subscriptionProjectState: Subscription;
  projectIdSubscription: Subscription;
  formSubmitted = false;
  addSOW: FormGroup;
  addAddendum: FormGroup;
  projectsList: SelectItem[];
  projectDetails: ProjectDetails[] = [];
  projectId: any;
  SOWData: SOW[] = [];
  AddendumData: any[] = [];
  dashboard: string = "";
  disableAddendum: boolean = false;
  isSow = 1;
  sowlist: SelectItem[] = [];
  tempSOWList:any;
  addButton: string = "Add";
  state: string = "";
  currentRole = "";
  PageSize: number = 0;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  cols = [
    //sow table fields
    { field: "SOWId", header: "SOW ID" },
    { field: "SOWFileName", header: "SOW File Name" },
    { field: "SOWSignedDate", header: "Signed Date" }
  ];

  cols2 = [
    //Addendum table fields
    { field: "AddendumNo", header: "Addendum Number" },
    { field: "AddendumDate", header: "Addendum Date" },
    { field: "RecipientName", header: "Recipient Name" },
    { field: "Note", header: "Note" }
  ];

  constructor(
    private commonService: CommonService,
    private SowService: SowService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private projectCreationService: ProjectCreationService,
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.currentRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.CreateSowForm();
    this.CreateAddendumForm();
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
      if (this.projectId > 0) {
        this.GetSowByProjectId(this.projectId);
        this.GetProjectsList();
      }
    });

    this.btnLabel = "Add";
    this.GetProjectsList();
    this.disableAddendumOption();
  }

  disableAddendumOption(): void {
    //to disable Addendum radio button
    this.subscriptionProjectState = this.projectCreationService.GetProjectState().subscribe((state: string) => {
      this.state = state;
      if (this.state == "Drafted" || this.state == "SubmittedForApproval")
        this.disableAddendum = true;
      else if (this.state == "Created") {
        if (this.SOWData.length == 0) {
          this.disableAddendum = true;
        }
      }
      else
        this.disableAddendum = false;
    });
  }

  ChangeFormat(SowData): void {
    //to change date format
    let i;
    if (this.isSow == 1)
      for (i = 0; i < SowData.length; i++) {
        if (SowData[i].SOWSignedDate != null)
          SowData[i].SOWSignedDate = moment(SowData[i].SOWSignedDate).format(
            "MM/DD/YYYY"
          );
      }
    else
      for (i = 0; i < SowData.length; i++) {
        if (SowData[i].AddendumDate != null)
          SowData[i].AddendumDate = moment(SowData[i].AddendumDate).format(
            "MM/DD/YYYY"
          );
      }
  }

  GetAddendumsBySOWId(event): void {
    //to get SOW based on SOWID
    this.SowService.GetAddendumsBySOWId(event.value, this.projectId).subscribe(
      (res: any[]) => {
        this.AddendumData = res;
        this.ChangeFormat(this.AddendumData);
      }
    );
  }

  CreateAddendumForm() {
    //to initialize addendum form
    this.addAddendum = new FormGroup({
      Addendum: new FormControl(true),
      ProjectId: new FormControl(null),
      SOWId: new FormControl(null),
      AddendumNo: new FormControl(null, [
        Validators.required,
        //Validators.pattern("^[0-9]*$")
      ]),
      RecipientName: new FormControl(null, [
        this.commonService.unAllowednames_usingCustom,
        Validators.required,
        Validators.pattern("[a-zA-Z ]*$")
      ]),
      Note: new FormControl(null, [Validators.required, this.commonService.unAllowednames_usingCustom,], ),
      AddendumDate: new FormControl(null, [Validators.required]),
      Id: new FormControl(null, [Validators.required]),
      RoleName: new FormControl(null),
      AddendumId: new FormControl(null)
    });
    if (this.projectId > 0) {
      this.addAddendum.patchValue({
        ProjectId: this.projectId
      });
    }
  }

  CreateSowForm() {
    //to initialize SOW form
    this.addSOW = new FormGroup({
      SOW: new FormControl(true),
      ProjectId: new FormControl(null, [Validators.required]),
      SOWId: new FormControl(null, [Validators.required, this.commonService.unAllowednames_usingCustom,
      Validators.maxLength(50)]),
      SOWSignedDate: new FormControl(null, [Validators.required]),
      SOWFileName: new FormControl(null, [Validators.required, this.commonService.unAllowednames_usingCustom,
      Validators.maxLength(50)]),
      RoleName: new FormControl(null),
      Id: new FormControl(null)
    });
    if (this.projectId > 0) {
      this.addSOW.patchValue({
        ProjectId: this.projectId
      });
    }
    if(this.currentRole){
      this.addSOW.patchValue({
        RoleName : this.currentRole
      });
    }
  }

  // DefineCols(): void {
  //   if (this.isSow == 0) {
  //     this.cols = [
  //       { field: "SOWId", header: "SOW Id" },
  //       { field: "SOWFileName", header: "SOW File Name" },
  //       { field: "SOWSignedDate", header: "Signed Date" }
  //     ];
  //   } else {
  //   }
  // }


  EnableDisableAddendum() {
    //Enable Addendum when SOWs are >= 1
    if (this.state == "Created") {
      if (this.SOWData.length == 0)
        this.disableAddendum = true;
      else
        this.disableAddendum = false;
    }
  }

  GetProjectsList(): void {
    //to get the list of projects
    this.projectsList = [];
    //this.projectsList.push({ label: "Select Project ", value: null });
    this.projectCreationService.GetProjectDetailsbyID(this.projectId).subscribe(
      (res: ProjectsData) => {
        /*res.forEach(e => {
          this.projectsList.push({
            label: e.ProjectName,
            value: e.ProjectId
          });
          {*/
        this.projectsList.push({
          label: res.ProjectName,
          value: res.ProjectId
        });
        
        if (this.projectId > 0) {
          this.addSOW.patchValue({
            ProjectId: this.projectId
          });
          this.addAddendum.patchValue({
              ProjectId: this.projectId
          });
        }
      }
    ),
      error => {
      };
  }

  // canShow(){
  //   if(this.state == "Created" && this.currentRole !== 'Department Head'){
  //     return false;
  //   }
  //   else
  //     return true;
  // }

  GetSowByProjectId(currentProjectId: number) {
    //to get SOW based on ProjectID
    this.editMode = false;
    if (currentProjectId != null) {
      this.sowlist = [];
      this.tempSOWList = [];
      //this.sowlist.push({ label: "Select SOW", value: null });
      this.SowService.GetSowByProjectId(currentProjectId).subscribe(
        (response: any[]) => {
          this.SOWData = response;
          this.SOWData.forEach((d: any) => {
            this.sowlist.push({ label: d.SOWId, value: d.Id });
            this.tempSOWList.push({ label: d.SOWId, value: d.Id, sowSignedDate:d.SOWSignedDate });
            d.SOWSignedDate = moment(d.SOWSignedDate).format("YYYY-MM-DD");
          });
          this.ChangeFormat(this.SOWData);
          this.EnableDisableAddendum();
        }
      );
    }
  }

  canEnableSowId() {
    //to make SOWID editable or not
    if ((this.state == "Drafted" || this.state == "SubmittedForApproval") && this.currentRole == 'Department Head')
      return false;
    if ((this.state == "Drafted") && this.currentRole != 'Department Head')
      return false;
    return true;

  }
  EditSOW(SowObj): void {
    //to edit SOW details
    this.editMode = true;

    if (this.isSow == 1) {
      this.addButton = "Update";
      this.CancelBtnLabelSOW = "Cancel";
    }
    else {
      this.btnLabel = "Update";
      this.CancelBtnLabelAddendum = "Cancel";
    }
    if (this.isSow == 1) {
      // edit sow
      this.SowService.GetSowDetails(
        this.projectId,
        SowObj.Id,
        this.currentRole
      ).subscribe((res: any) => {
        this.PopulateSowForm(res);
      });
    } else {
      // edit addendum
      this.SowService.GetAddendumDetailsById(
        this.projectId,
        SowObj.AddendumId,
        this.currentRole
      ).subscribe(res => {
        this.PopulateAddendum(res);
      });
    }
  }


  deleteDialog(rowData) {
    // method to open delete dialog
    this.confirmationService.confirm({
      message: 'Do you want to delete SOW?',
      accept: () => {
        this.deleteSOW(rowData);
      },
      reject: () => {

      }
    });
  }

  deleteSOW(rowData) {
    //to delete SOW
    this.SowService.DeleteSow(rowData.Id).subscribe((res: any) => {
      if (res == true) {
        this.GetSowByProjectId(this.projectId);

        this.ResetSow();
        this.messageService.add({ severity: 'success', summary: 'Success message', detail: 'Deleted' });
      }
      else
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Deleting  failed' });
    });
  }


  PopulateAddendum(res) {
    //to populate addendum form
    this.addAddendum.patchValue({
      Addendum: true,
      ProjectId: res.ProjectId,
      SOWId: res.SOWId,
      AddendumNo: res.AddendumNo,
      RecipientName: res.RecipientName,
      Note: res.Note,
      AddendumDate:
        res.AddendumDate != null
          ? new Date(moment(res.AddendumDate).format("MM/DD/YYYY"))
          : null,
      Id: res.Id,
      AddendumId: res.AddendumId,
      RoleName: this.currentRole
    });
  }

  PopulateSowForm(res) {
    //to populate SOW form
    this.addSOW.patchValue({
      SOW: true,
      ProjectId: res.ProjectId,
      SOWId: res.SOWId,
      SOWSignedDate:
        res.SOWSignedDate != null
          ? new Date(moment(res.SOWSignedDate).format("MM/DD/YYYY"))
          : null,
      SOWFileName: res.SOWFileName,
      Id: res.Id,
      RoleName: this.currentRole
    });
  }

  GetSows(event) {
    //to get SOW's by Project Id
    this.GetSowByProjectId(event.value);
  }

  saveSOW() {
    //to save SOW
    let proId = this.addSOW.value.ProjectId;

    if(this.addSOW.value.Id == null){
      this.addSOW.value.Id = 0;
    }
    this.SowService.SaveSOW(this.addSOW.value).subscribe(res => {
      if (res == 1) {
        this.GetSowByProjectId(proId);
        this.EnableDisableAddendum();
        this.messageService.add({
          severity: "success",
          summary: "Success Message",
          detail: "Saved successfully"
        });
        this.ResetSow();
      }
      else if (res == -1) {
        // this.GetSowByProjectId(proId);
        // this.OpenConfirmationDialog(0);
        this.messageService.add({
          severity: "warn",
          summary: "Warning Message",
          detail: "SOW already exist"
        });

      }
      else if (res == 2627) {
        this.messageService.add({
          severity: "warn",
          summary: "Warning Message",
          detail: "SOW already exists"
        });
      }
      else {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Unable to Save SOW"
        });
      }
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Unable to save SOW"
        });
      };
    });
  }

  saveAddendum() {
    //to save Addendum
    this.addAddendum.value.RoleName = this.currentRole;
    this.addAddendum.value.ProjectId = this.projectId;
    this.addAddendum.value.SOWId = this.GetLabelOfSow(
      this.addAddendum.value.Id
    );
    if(this.addAddendum.value.AddendumId == null){
      this.addAddendum.value.AddendumId = 0;
    }
    let Id = this.addAddendum.value.Id;
    this.addAddendum.value.AddendumDate = moment(
      this.addAddendum.value.AddendumDate
    ).format("YYYY-MM-DD");
    if (this.addAddendum.valid == true) {
      this.SowService.CreateAddendum(this.addAddendum.value).subscribe(
        res => {
          if (res == true) {
            this.GetAddendumsBySOWId({ value: Id });
            this.messageService.add({
              severity: "success",
              summary: "Success Message",
              detail: "Saved successfully"
            });
            this.ResetAddendum();
          }
          error => {
            this.messageService.add({
              severity: "error",
              summary: "Error message",
              detail: "Unable to Add Addendum"
            });
          };
        }
      );
    }
  }

  updateSOW() {
    //to update SOW
    let proId = this.addSOW.value.ProjectId;
    this.addSOW.value.RoleName = this.currentRole;
    this.SowService.UpdateSOWDetails(
      this.addSOW.value
    ).subscribe(res => {
      if (res == 1) {
        this.GetSowByProjectId(proId);

        this.messageService.add({
          severity: "success",
          summary: "Success Message",
          detail: "Successfully Updated"
        });
        this.ResetSow();
      }
      else if (res == -1) {
        this.messageService.add({
          severity: "warn",
          summary: "Warning Message",
          detail: "Unable to update"
        });
      }
      else if (res == 2627) {
        this.messageService.add({
          severity: "warn",
          summary: "Warning Message",
          detail: "SOW already exists"
        });
      }
      else {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Unable to update SOW"
        });
      }
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Unable to Update"
        });
      };
    });
  }

  updateAddendum() {
    //to update Addendum
    let Id = this.addAddendum.value.Id;
    this.SowService.UpdateAddendumDetails(
      this.addAddendum.value
    ).subscribe(
      res => {
        if (res == true) {
          this.GetAddendumsBySOWId({ value: Id });
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "Updated successfully"
          });
        }
        if (res == false) {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Unable to add addendum server error"
          });
        }
        this.ResetAddendum();
      },

      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "error"
        });
      }
    );
  }

  AddAddendum() {
    //invokes when add button in clicked in Addendum form
    this.AddendumformSubmitted = true;
    if (this.isSow == 0 && this.editMode == false) {
      let selected_sow = this.tempSOWList.filter(q => q.value === this.addAddendum.value.Id);
      if(selected_sow.length > 0 && moment(selected_sow[0].sowSignedDate) > moment(this.addAddendum.value.AddendumDate)){
          this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Addendum Date cannot be lessthan SOW Signed Date."
        });
        return false;
      }
      this.saveAddendum();
    }
    else if (this.isSow == 0) {
      // edit is clicked
      let selected_sow = this.tempSOWList.filter(q => q.value === this.addAddendum.value.Id);
      if(selected_sow.length > 0 && moment(selected_sow[0].sowSignedDate) > moment(this.addAddendum.value.AddendumDate)){
          this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Addendum Date cannot be lessthan SOW Signed Date."
        });
        return false;
      }
      this.addAddendum.value.RoleName = this.currentRole;
      this.addAddendum.value.SOWId = this.GetLabelOfSow(this.addAddendum.value.Id);
      this.addAddendum.value.AddendumDate = moment(this.addAddendum.value.AddendumDate).format("YYYY-MM-DD");
      if (this.addAddendum.valid == true) {
        this.updateAddendum();
      }
    }
  }

  AddSOW(): void {
    //invokes when add button is clicked in SOW form
    this.formSubmitted = true;
    if (this.editMode == false) {  
      this.addSOW.value.SOWSignedDate = moment(
        this.addSOW.value.SOWSignedDate
      ).format("YYYY-MM-DD");
      if (this.addSOW.valid == true && this.isSow == 1) {
        this.saveSOW();
      }
    }
    else {
      // edit was clicked
      this.addSOW.value.SOWSignedDate = moment(
        this.addSOW.value.SOWSignedDate
      ).format("YYYY-MM-DD");
      if (this.addSOW.valid == true && this.isSow == 1) {
        this.updateSOW();
      }

    }
  }
  
  GetLabelOfSow(id: number): string {
    //to get SOWs
    var i, label;
    for (i = 0; i < this.sowlist.length; i++) {
      if (id == this.sowlist[i].value) {
        return this.sowlist[i].label;
      }
    }
    return null;
  }

  CreateAddendum() {
    //to create Addendum
    this.formSubmitted = true;
    if (this.addAddendum.valid == true) {
      let id = this.addAddendum.value.SOWId;
      this.SowService.CreateAddendum(this.addAddendum.value).subscribe(res => {
        if (res == true) {
          this.GetAddendumsBySOWId({ value: id });
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "Added successfully"
          });
        }

        error => {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Unable to add Addendum"
          });
        };
      });
      this.ResetAddendum();
    } else {
      this.messageService.add({
        severity: "warn",
        summary: "Error message",
        detail: "Invalid data"
      });
    }
  }
  // IsValidDate = function (fromDate: any) {
  //   //to validate Date
  //   if (Date.parse(fromDate)) return true;

  //   return false;
  // };

  ResetSow() {
    //to Reset Sow
    this.editMode = false;
    this.addButton = "Add";
    this.CancelBtnLabelSOW = "Clear";
    this.addSOW.get('SOWId').enable();
    this.CreateSowForm();
    this.formSubmitted = false;
  }
  ResetAddendum() {
    //to Reset Addendum
    this.CancelBtnLabelAddendum = "Clear";
    this.editMode = false;
    this.btnLabel = "Add";
    this.AddendumformSubmitted = false;
    this.CreateAddendumForm();
  }

  OpenConfirmation() {
    // method to open cancel dialog
    let msg;
    if (this.isSow == 1)
      msg = 'Do you want to ' + this.CancelBtnLabelSOW + ' ?';
    else
      msg = 'Do you want to ' + this.CancelBtnLabelAddendum + ' ?';
    this.confirmationService.confirm({

      message: msg,
      accept: () => {
        if (this.isSow == 1)
          this.ResetSow();
        else
          this.ResetAddendum();
      },
      reject: () => {

      }
    });
  }

  ngOnDestroy() {
    this.subscriptionProjectState.unsubscribe();
    this.projectIdSubscription.unsubscribe();
  }
}
