import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { ClientbillingroleService } from "../../services/clientbillingrole.service";
import { CommonService } from "../../../../services/common.service";
import { Client } from "../../models/client.model";
import { MasterDataService } from "../../../../services/masterdata.service";
import { SelectItem, Message } from "primeng/components/common/api";
import {MessageService} from 'primeng/api';
@Component({
  selector: 'app-client-billing-roles-form',
  templateUrl: './client-billing-roles-form.component.html',
  styleUrls: ['./client-billing-roles-form.component.css'],
  providers: [ClientbillingroleService, CommonService, MasterDataService, MessageService]
})
export class ClientBillingRolesFormComponent implements OnInit {
  clientList: SelectItem[] = [];
  componentName: string;
  helpMsg = "Successfully created";
  btnLabel = "";
  displayDialog = false;
  editObj = null;
  createObj: any;
  constructor(
    private actRoute: ActivatedRoute,
    private serviceObj: ClientbillingroleService,
    private _commonService: CommonService,
    private  _ClientbillingroleService: ClientbillingroleService,
    private _masterDataService: MasterDataService,
    private  messageService: MessageService) 
    { this.componentName = this.actRoute.routeConfig.component.name; }
  addClientBilling: FormGroup

  ngOnInit() {
    this.serviceObj.editObj.subscribe((data) => {
      this.editObj = data;

      this.getClientList();
    });

    this.btnLabel = "CREATE";
    this.addClientBilling = new FormGroup({
      "client": new FormControl(null, [Validators.required]),
      "clientBillingRoleCode": new FormControl(null, [Validators.required]),
      "clientBillingRoleName": new FormControl(null, [Validators.required])
    });
  }

  toSetEditData(editObj) {
    this.btnLabel = "UPDATE";
  }
  toReset() {
    this.addClientBilling.reset();
  }
  getClientList(): void {
    this._masterDataService.GetClientList().subscribe((clientResponse: Client[]) => {
      clientResponse.forEach((clientResponse: Client) => {
        this.clientList.push({ label: clientResponse.ClientName, value: clientResponse.ClientId })
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
          });
      };
  }
  CreateClientBilling() {
    this.createObj.client = this.addClientBilling.value.client;
    this.createObj.clientBillingRoleCode = this.addClientBilling.value.clientBillingRoleCode;
    this.createObj.clientBillingRoleName = this.addClientBilling.value.clientBillingRoleName;
    if (this.addClientBilling.valid == true) {
      this.serviceObj
        .subscribe(res => {

          if (res == 0) {
            //this.serviceObj.toGetClientBillingData();            
            if (this.serviceObj.editMode == true)
              this.serviceObj.editMode = false;
            // this.displayDialog = true;
          }
        });
      this.toReset();
    }
    else {
      alert("invalid data");
    }
  }
}