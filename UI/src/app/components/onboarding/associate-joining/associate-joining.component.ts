import { Component,Injector, OnInit ,Inject} from '@angular/core';
import { AssociatejoiningService } from '../services/associatejoining.service';
import { Associate } from '../models/associate.model';
import * as servicePath from '../../../service-paths';
import { Router } from '@angular/router';
import * as moment from 'moment';


@Component({
  selector: 'app-associate-joining',
  templateUrl: './associate-joining.component.html',
  styleUrls: ['./associate-joining.component.scss']
})
export class AssociateJoiningComponent implements OnInit {
  associateList : Associate[];  
  selectedRow : Associate;
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  constructor(private _associatejoingService : AssociatejoiningService,@Inject(Router) private _router: Router){
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this.getAssociatejoiningList();
  }
  cols = [
    {field : 'Name', header: 'Name' },
    {field : 'TechnologyName', header : 'Technology'},
    {field : 'DesignationName', header : 'Designation'},    
    {field : 'DepartmentCode', header : 'Department'},
    {field : 'JoinDate', header : 'Joining Date'},
    {field : 'Hradvisor', header : 'Advisor'},
    
];
  getAssociatejoiningList(){
     this._associatejoingService.getAssociates().subscribe((res: any) => { this.associateList = res;
        this.associateList.forEach((r:any) => {
            if(r.JoinDate != null)
                r.JoinDate= moment(r.JoinDate).format('YYYY-MM-DD');
            else r.JoinDate="";
            });
        });
            
  }

  editAssociatejoining(currentAssociate: Associate) {
    let subType = "profileupdate";
    currentAssociate.associateType = currentAssociate.EmployeeId != 0 ? "edit" : "new";
    this._router.navigate(['/associates/prospectivetoassociate/' + currentAssociate.associateType + '/' + currentAssociate.EmployeeId + '/' + subType]);
}

  ngOnDestroy() {
}
}
