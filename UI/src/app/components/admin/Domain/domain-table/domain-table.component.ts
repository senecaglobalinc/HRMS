import { Component, OnInit} from '@angular/core';
import { DomainMasterData } from '../../models/domainmasterdata.model';
import { DomainMasterService } from '../../services/domainmaster.service';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-domain-table',
  templateUrl: './domain-table.component.html',
  styleUrls: ['./domain-table.component.css']
})

export class DomainTableComponent implements OnInit {
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  domainsList : DomainMasterData[];
  selectedRow : DomainMasterData;

  cols = [
    {field: 'DomainName', header: 'Domain Name' }
];

  constructor(private _domainService: DomainMasterService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this._domainService.domainsList.subscribe((data) => {
      this.domainsList = data;
    });
    this.getDomains();
  }

  getDomains() : void {
    this._domainService.getDomains();
  }

  editDomains(domainEdit) : void{
    this._domainService.editMode = true;
    this._domainService.domainEdit.next(domainEdit);
  }

  ngOnDestroy() {
    // this._domainService.domainsList.unsubscribe();
  }

}






