import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { ClientService } from "../../services/client.service";
import { Client } from "../../models/client.model";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { CommonService } from "../../../../services/common.service";

@Component({
  selector: "app-clients-form",
  templateUrl: "./clients-form.component.html",
  styleUrls: ["./clients-form.component.css"],
  providers: [MessageService]
})
export class ClientsFormComponent implements OnInit {
  addClient: FormGroup;
  btnLabel: string = "";
  isEdit: boolean;
  formSubmitted = false;
  clients: Subscription;
  constructor(
    private _clientService: ClientService,
    private commonService: CommonService,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    this.addClient = new FormGroup({
      ClientCode: new FormControl(null, [
        Validators.required,
        Validators.pattern('[cC][0-9]*'),
        // Validators.pattern('^[a-zA-Z][a-zA-Z-&, ]*$'),
        this.commonService.unAllowednames_usingCustom,
        Validators.maxLength(6)
      ]),
      ClientName: new FormControl(null, [
        Validators.required,
        this.commonService.unAllowednames_usingCustom,
        // Validators.pattern('^[a-zA-Z][a-zA-Z-&, ]*$'),
        Validators.maxLength(50)
      ]),
      ClientRegisterName: new FormControl(null, [
        this.commonService.unAllowednames_usingCustom,
        // Validators.pattern('^[a-zA-Z][a-zA-Z-&, ]*$'),
        Validators.maxLength(150)
      ]),
      IsActive: new FormControl(null)
    });

    this.clients = this._clientService.GetClients().subscribe(data => {
      if (this._clientService.editMode == true) {
        this.isEdit = this._clientService.editMode;
        this.addClient.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();
  }

  addClients(): void {
    this.formSubmitted = true;
    if (this.addClient.value.IsActive != false) {
      this.addClient.value.IsActive = true;
    }
    var clients = new Client();
    clients.ClientCode = this.addClient.value.ClientCode;
    clients.ClientName = this.addClient.value.ClientName;
    clients.ClientRegisterName = this.addClient.value.ClientRegisterName;
    clients.IsActive = this.addClient.value.IsActive;
    if (this._clientService.editMode == true) {
      clients.ClientId = this._clientService.clients.value.ClientId;
    }
    if (this.addClient.valid == true) {
      this._clientService.createClients(clients).subscribe(
        (res: number) => {
          if (res) {
            this._clientService.getClients();
            if (this._clientService.editMode == false)
              this.messageService.add({
                severity: "success",
                summary: "Success Message",
                detail: "Client record added successfully."
              });
            else if (this._clientService.editMode == true)
              this.messageService.add({
                severity: "success",
                summary: "Success Message",
                detail: "Client record updated successfully."
              });
            this.cancel();
          }
        },
        error => {
            this.messageService.add({
              severity: "error",
              summary: "Error Message",
              detail: error.error
            });
        }
      );
    } 
  }

  cancel(): void {
    this.formSubmitted = false;
    this.addClient.reset();
    this._clientService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }

  validate_clientCode(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 47 && k < 58) ||
      (k > 96 && k < 123) ||
      k == 8
    );
  }


  validate_clientName(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 47 && k < 58) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32
    );
  }

  validate_clientRegisterName(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 47 && k < 58) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 44 ||
      k == 46
    );
  }

  ngOnDestroy() {
    this.clients.unsubscribe();
  }
}
