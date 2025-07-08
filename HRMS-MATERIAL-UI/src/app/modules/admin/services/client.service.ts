import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment'
import {Client} from '../models/client.model';
import { BehaviorSubject } from "rxjs";
import * as servicePath from '../../../core/service-paths'

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  public editMode = false;

  _serviceUrl = environment.AdminMicroService;
  resources = servicePath.API.Clients;
  clients = new BehaviorSubject<Client>(new Client());
  clientsList = new BehaviorSubject<Client[]>([]);
  constructor(private httpClient: HttpClient) {}

  public getClients() {
    this.httpClient
      .get(this._serviceUrl + this.resources.list)
      .subscribe((res: Client[]) => {
        this.clientsList.next(res);
      });
  }

  public getAllClients() {
    return this.httpClient.get(this._serviceUrl + this.resources.list);
  }

  public createClients(client: Client) {
    if (this.editMode == false)
      return this.httpClient.post(
        this._serviceUrl + this.resources.create,
        client
      );
    else
      return this.httpClient.post(
        this._serviceUrl + this.resources.update,
        client
      );
  }

  public GetClientsList() {
    return this.clientsList.asObservable();
  }

  public GetClients() {
    return this.clients.asObservable();
  }

  // public GetEditMode(){
  //   return this.editMode.asObservable();
  // }

  // public SetEditMode(editMode){
  //   this.clients.next(editMode);
  //   this.GetEditMode();

  // }

  public SetClientsData(clients) {
    this.clients.next(clients);
    this.GetClients();
  }
}
