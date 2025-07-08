import { Component, OnInit } from '@angular/core';
import { Associate } from '../models/associate.model';
import * as servicePath from '../../../service-paths';
import { AssociateInformationService } from '../services/associateInformation.service';
import { Router, ActivatedRoute } from "@angular/router";
import * as moment from 'moment';

@Component({
  selector: 'app-associateinformation',
  templateUrl: './associateinformation.component.html',
  styleUrls: ['./associateinformation.component.scss']
})
export class AssociateinformationComponent implements  OnInit {
  private subType: string = "list";
   associateInfoList : Associate[];
   selectedRow : Associate;
   PageSize: number;
   PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  constructor(private _associateinfoService : AssociateInformationService,private _router: Router){
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this.getAssociateInformationList();
  }
  cols = [
    {field : 'EmpCode', header: 'Emp Code' },
    {field : 'EmpName', header : 'Emp Name'},
    {field : 'MobileNo', header : 'Mobile Number'},    
    {field : 'PersonalEmailAddress', header : 'Personal Email'},
    {field : 'BgvStatus', header : 'BGV Status'},
    
];
  getAssociateInformationList(){
    this._associateinfoService.getAssociates().subscribe((res: any) => { this.associateInfoList = res;
       this.associateInfoList.forEach((r:any) => {
           if(r.JoinDate != null)
               r.JoinDate= moment(r.JoinDate).format('YYYY-MM-DD');
           else r.JoinDate="";
           });
       });
   }

  editAssociatejoining(selectedData: any) {
    let currentID = selectedData.EmpId;
    selectedData.associateType = selectedData.EmpId != 0 ? "edit" : "new";
    this._router.navigate(['/associates/prospectivetoassociate/' + selectedData.associateType + '/' + currentID + '/' + this.subType]);
  }

  ngOnDestroy() {
}

}
