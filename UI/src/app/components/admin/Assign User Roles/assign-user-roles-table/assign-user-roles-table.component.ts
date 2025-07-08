import { Component, OnInit } from '@angular/core';
import { MasterDataService } from '../../../../services/masterdata.service';
import { AssignUserRolesService } from '../../services/assign-user-roles.service';
import { UserRoles } from '../../models/userRoles';
import * as servicePath from '../../../../service-paths';
import { Subscription } from '../../../../../../node_modules/rxjs';
@Component({
  selector: 'app-assign-user-roles-table',
  templateUrl: './assign-user-roles-table.component.html',
  styleUrls: ['./assign-user-roles-table.component.css']
})
export class AssignUserRolesTableComponent implements OnInit {
    assignUserRolesData :UserRoles[];
    selectedAssignUserRole : UserRoles;
    resources = servicePath.API.PagingConfigValue;
    PageSize: number;
    PageDropDown: number[] = [];
  userRolesList : Subscription;
  constructor(  private masterService : MasterDataService,    private assignUserRolesService : AssignUserRolesService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
   }
  ngOnInit() {
    this.assignUserRolesData = [];
    this.userRolesList  = this.assignUserRolesService.GetRolesList().subscribe(data=>{ 
       this.assignUserRolesData = data;
    });
  }
  cols = [
    { field : "RoleName", header : "Role Name"},
  ]
  
  
  SetEditObj(editObj) : void{
    this.assignUserRolesService.SetUserRole(editObj);
  }
  
  ngOnDestroy() {
    this.assignUserRolesService.SetUserRolesList(null);
     this.userRolesList.unsubscribe();
     this.assignUserRolesData = [];
  }

}
