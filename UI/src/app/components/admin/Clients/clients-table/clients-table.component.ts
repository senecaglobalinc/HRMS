import { Component, OnInit } from "@angular/core";
import { Client } from "../../models/client.model";
import { ClientService } from "../../services/client.service";
import * as servicePath from "../../../../service-paths";
import { Subscription } from "rxjs";

@Component({
  selector: "app-clients-table",
  templateUrl: "./clients-table.component.html",
  styleUrls: ["./clients-table.component.css"]
})
export class ClientsTableComponent implements OnInit {
  clientsData: Client[];
  clientsList: Subscription;
  selectedRow: Client;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  cols = [
    { field: "ClientCode", header: "Client Code" },
    { field: "ClientName", header: "Client Name" },
    { field: "ClientRegisterName", header: "Client Register Name" }
  ];
  constructor(private _clientService: ClientService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.getClients();
    this.clientsData = [];
    this.clientsList = this._clientService
      .GetClientsList()
      .subscribe((data: Client[]) => {
        this.clientsData = data;
      });
  }

  getClients(): void {
    this._clientService.getClients();
  }

  editClients(clientsData: Client): void {
    this._clientService.editMode = true;
    this._clientService.SetClientsData(clientsData);
  }
print(){
    let popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
  <html>
  
  <body onload="window.print();window.close()">
    dklgkfdgklfdgkldfgjlkdf
  </body>
</html>`
      );
      popupWin.document.close();
  }
  ngOnDestroy() {
    this._clientService.SetClientsData(null);
    this.clientsList.unsubscribe();
    this.clientsData = [];
  }
}
