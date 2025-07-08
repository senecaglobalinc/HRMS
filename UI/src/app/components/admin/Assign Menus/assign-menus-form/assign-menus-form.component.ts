import { Component, ViewChild, Injector, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { MenuRoles, Menus, GenericType } from '../../models/index';
import { CommonService } from "../../../../services/common.service";
import { MasterDataService } from '../../../../services/masterdata.service';
import { Message } from 'primeng/components/common/api';
import { SelectItem } from 'primeng/components/common/api';
import { Menus, MenuRoles } from '../../Models/menu-roles.model';
import { AssignmenustoroleService } from '../../services/assignmenustorole.service';
import {MessageService} from 'primeng/api';
declare var _: any;

@Component({
  selector: 'app-assign-menus-form',
  templateUrl: './assign-menus-form.component.html',
  styleUrls: ['./assign-menus-form.component.css'],
  providers: [MasterDataService, AssignmenustoroleService, CommonService, MessageService]
})
export class AssignMenusFormComponent implements OnInit {
    rolesList: SelectItem[] = [];
    RoleId: number;
    sourceMenus: Menus[] = [];
    targeMenus: Menus[] = [];
    finalTargetMenu: MenuRoles;
    isEdit: boolean;
    componentName: string;
    hidePanel: boolean = false;
    constructor(private actRoute: ActivatedRoute,
        private _router: Router, private messageService : MessageService, private _http: HttpClient, private masterDataService: MasterDataService, private commonService: CommonService, private assignMenusToRoleService: AssignmenustoroleService) {
    }

    ngOnInit() {
        this.getRolesList();
        this.cancel();
    }

    getRolesList(): void {
        this.masterDataService.GetRoles().subscribe((res: any[]) => {
            this.rolesList = [];
            this.rolesList.push({ label: 'Select Role', value: null });
            res.forEach(e => {
                this.rolesList.push({ label: e.Name, value: e.ID });
            });
        });
    }

    getRoleId(RoleId: number): void {
        this.RoleId=RoleId;
        if (RoleId == null || RoleId == undefined) {
           
        }
        else {
            this.hidePanel = true;
            this.getSourceMenusList(RoleId);
            this.getTargetMenusList(RoleId);
        }
    }

    getSourceMenusList(RoleId: number): void {
        this.assignMenusToRoleService.getSourceMenus(RoleId).subscribe((menus: Menus[]) => {
            this.sourceMenus = [];

            this.sourceMenus = menus;

        }, (error) => {
            if (error.error != undefined && error.error != "")
                this.commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                });
            //this.errorMessage = [];
           // this.errorMessage.push({ severity: 'error', summary: 'Failed get Source Menus!' });
           this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed get Source Menus!'});

        });
    }

    getTargetMenusList(RoleId: number): void {
        this.assignMenusToRoleService.getTargetMenus(RoleId).subscribe((menus: Menus[]) => {
            this.targeMenus = [];
            this.targeMenus = menus;
        }, (error) => {
            if (error.error != undefined && error.error != "")
                this.commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                });
           // this.errorMessage = [];
           // this.errorMessage.push({ severity: 'error', summary: 'Failed get Target Menus!' });
           this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed get Source Menus!'});

        });
    }

    saveTargetMenus(targeMenus: Menus[]): void {
        this.finalTargetMenu = new MenuRoles();
        let MenuList: Menus[] = [];
        targeMenus.forEach(menu => {
            MenuList.push({ MenuId: menu.MenuId, MenuName: "" })
        });
        this.finalTargetMenu.RoleId=this.RoleId;
        this.finalTargetMenu.MenuList=MenuList;
        this.assignMenusToRoleService.addTargetMenuRoles(this.finalTargetMenu).subscribe((response: boolean) => {
            if(response == true){
                this.getTargetMenusList(this.RoleId);
                this.messageService.add({severity:'success', summary: 'Success Message', detail:'Menu details saved'});
            }
          

        }, (error) => {
           this.messageService.add({severity:'error', summary: 'Failure Message', detail:error.error});
            
        });
    }
    cancel(): void {
        this.hidePanel = false;
        this.RoleId=null;
    }

}


// if(res == 1){
//     this._clientService.getClients();
//     if(this._clientService.editMode == false)
//       this.messageService.add({severity:'success', summary: 'Success Message', detail:'Client added'});
//     else if(this._clientService.editMode == true)
//       this.messageService.add({severity:'success', summary: 'Success Message', detail:'Client updated'});    
//       this.cancel();
//     }