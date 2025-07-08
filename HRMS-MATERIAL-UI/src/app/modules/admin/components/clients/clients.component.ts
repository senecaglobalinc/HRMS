import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormGroupDirective,
} from '@angular/forms';
import { Client } from '../../models/client.model';
import { ClientService } from '../../services/client.service';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonService } from '../../../../core/services/common.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { themeconfig } from '../../../../../themeconfig';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss'],
})
export class ClientsComponent implements OnInit {
  addClient: FormGroup;
  btnLabel: string = '';
  isEdit: boolean = false;
  formSubmitted = false;
  clients: Subscription;
  clientsData: Client[];
  clientsList: Subscription;
  themeConfigInput = themeconfig.formfieldappearances;
  displayedColumns: string[] = [
    'ClientCode',
    'ClientName',
    'ClientRegisterName',
    'Edit',
  ];

  dataSource: MatTableDataSource<Client>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private _clientService: ClientService,
    private commonService: CommonService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this.clientsData = [];
    this.clientsList = this._clientService
      .GetClientsList()
      .subscribe((data: Client[]) => {
        this.clientsData = data;
        this.dataSource = new MatTableDataSource(this.clientsData);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit(): void {
    this.addClient = new FormGroup({
      ClientCode: new FormControl(null, [
        Validators.required,
        Validators.pattern('[cC][0-9]*'),
        Validators.maxLength(6),
      ]),
      ClientName: new FormControl(null, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50),
      ]),
      ClientRegisterName: new FormControl(null, [
        Validators.minLength(2),
        Validators.maxLength(150),
      ]),
      IsActive: new FormControl(null)
    });

    this.clients = this._clientService.GetClients().subscribe((data) => {
      if (this._clientService.editMode == true) {
        this.isEdit = this._clientService.editMode;
        this.addClient.patchValue(data);
        this.btnLabel = 'UPDATE';

      }
    });
    this.btnLabel = 'SAVE';
    this.cancel();
    this.getClients();

  }
  getClients(): void {
    this._clientService.getClients();
  }

  editClients(clientsData: Client): void {
    this._clientService.editMode = true;
    this._clientService.SetClientsData(clientsData);
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.clientsData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  cancel(): void {
    this.formSubmitted = false;
    this.addClient.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this._clientService.editMode = false;
    this.btnLabel = 'SAVE';
    this.addClient.controls['ClientCode'].enable();
    this.isEdit = false;
  }

  validate_clientCode(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) || (k > 47 && k < 58) || (k > 96 && k < 123) || k == 8 || k == 32
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
      clients.ClientCode = this.addClient.value.ClientCode;
    }
    if (this.addClient.valid == true) {
      this._clientService.createClients(clients).subscribe(
        (res: number) => {
          if (res) {
            this._clientService.getClients();
            if (this._clientService.editMode == false) {
              this._snackBar.open('Client record added successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            }
            else if (this._clientService.editMode == true) {
              this._snackBar.open('Client record updated successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            }
          }
        },
        (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
    }
  }
}
