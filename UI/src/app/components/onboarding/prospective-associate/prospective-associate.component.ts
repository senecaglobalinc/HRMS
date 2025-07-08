import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProspectiveAssosiateService } from '../services/prospective-assosiate.service';
import * as servicePath from '../../../service-paths';
import * as moment from 'moment';
import { Associate } from '../models/associate.model';
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-prospective-associate',
  templateUrl: './prospective-associate.component.html',
  styleUrls: ['./prospective-associate.component.scss'],
  providers: [ProspectiveAssosiateService]
})
export class ProspectiveAssociateComponent implements OnInit {
//   prosAssociatesList: any[];selectedTRs;
items: MenuItem[];

    home: MenuItem;
prosAssociatesList: any[];
  private resources = servicePath.API.PagingConfigValue;
   PageSize: number;
   PageDropDown: number[] = [];

    constructor(
      private _ProspectiveAssosiateService: ProspectiveAssosiateService,  private _router: Router
  )
 {
      this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
    }

    ngOnInit() {
        this.getprosAssociatesDetails();
        
    }
        cols = [
            { field: "EmpName", header: "Name" },
            { field: "Designation", header: "Designation" },
            { field: "Department", header: "Department" },
            { field: "JoiningDate", header: "Joining Date" },
            { field: "HRAdvisorName", header: "Advisor" }
          ];
        
  

    getprosAssociatesDetails() {
        this._ProspectiveAssosiateService.list().subscribe((res: any) => { this.prosAssociatesList = res; 
             this.prosAssociatesList.forEach((r:any) => {
                 
                 r.joiningDate= moment(r.joiningDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            });
        });
    }

    onEdit(selectedData: any) {        
            this._router.navigate(['/associates/editprospectiveassociate/' + selectedData.Id]);
    }

    onAdd() {
       this._router.navigate(['/associates/addprospectiveassociate']);
    }
    onConfirm(currentAssociate: Associate) {
        let subType = "profile";
        currentAssociate.associateType = currentAssociate.EmpId != 0 ? "edit" : "new";
        currentAssociate.Id = currentAssociate.EmpId == 0 ? currentAssociate.Id : currentAssociate.EmpId;
        this._router.navigate(['/associates/prospectivetoassociate/' + currentAssociate.associateType + '/' + currentAssociate.Id + '/' + subType]);
    }

}