import { Component, OnInit } from "@angular/core";
import { MasterDataService } from "../../../../services/masterdata.service";
import { SelectItem, MessageService } from "primeng/components/common/api";
import { AssignUserRolesService } from "../../services/assign-user-roles.service";
import { UserRoles } from "../../models/userRoles";
import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";
import { Subscription } from "rxjs";
import { Role } from "../../../../models/associate-skills.model";
import { RoleCompetencySkills, RoleData } from "../../../../models/role.model";

@Component({
  selector: "app-assign-user-roles-form",
  templateUrl: "./assign-user-roles-form.component.html",
  styleUrls: ["./assign-user-roles-form.component.css"],
  providers: [MessageService]

})
export class AssignUserRolesFormComponent implements OnInit {
   btnLabel = "Save";
   newRole: FormGroup;
   usersList: UserRoles[];
   userNamesList: SelectItem[] = [];
   rolesList: SelectItem[] = [];
   availableRoles: UserRoles[];
   selectedUserId: number;
   editObj: UserRoles;
   roleObj:Role;
  userRole : Subscription;
   selectedRole: number;
   addRoleToUser = false;
  formSubmitted = false;
  constructor(
    public masterService: MasterDataService,
    public assignUserRolesService: AssignUserRolesService,
    public messageService: MessageService
  ) {
    this.roleObj=new Role();
  }
  ngOnInit() {
    this.newRole = new FormGroup({
      IsActive: new FormControl(true, ),
      RoleId: new FormControl(null, [Validators.required])
    });
    this.GetUserNames();
    this.GetRolesList();
    this.userRole = this.assignUserRolesService.GetUserRole().subscribe(data=>{
      this.newRole.patchValue(data);

      if(data.UserId !=null)
        this.btnLabel = 'Update';
    });
   this.Reset();
  }
  GetUserNames() : void {
  this.userNamesList.push({ label: "Select User", value: null });
    this.assignUserRolesService.GetUserNames().subscribe((res: any) => {
      this.usersList = res;
    
      this.usersList.forEach(element => {
        this.userNamesList.push({
          label: element.UserName,
          value: element.UserId
        });
      });
    });
  }

 GetRolesList() : void{
    this.masterService.GetRoles().subscribe((res: any[]) => {
      this.rolesList = [];
      this.rolesList.push({ label: "Select Role", value: null });
      res.forEach(e => {
        this.rolesList.push({ label: e.Name, value: e.ID });
      });
    });
  }
 EditMode() : boolean {
  if(this.btnLabel == 'Update')
    return true;
  return false;
  }

 GetUserRoles() : void {
    if (this.selectedUserId != null) {
      this.assignUserRolesService.GetUserRoles(this.selectedUserId);
    }
  }

 AddRole() : void {
    this.addRoleToUser = true;
  }

 SaveOrUpdateUserRoles() : void {
  this.formSubmitted = true;
    if (this.selectedUserId != null) {
      if (this.newRole.value.RoleId != null) {
        this.GetUserById();
        this.editObj.RoleId = this.newRole.value.RoleId;
        this.editObj.IsActive = this.newRole.value.IsActive;
        this.roleObj.RoleId=this.newRole.value.RoleId;
        this.roleObj.RoleName=this.FilterRoles(this.editObj.RoleId);
        this.editObj.Role=this.roleObj;
        this.assignUserRolesService
          .SaveOrUpdateUserRoles(this.editObj)
          .subscribe((res: any) => {
            if(res != null){
            this.assignUserRolesService.GetUserRoles(this.editObj.UserId);
            if (this.btnLabel == 'Save')
              this.messageService.add({ severity: 'success', summary: 'Success message', detail: 'Role added' });
            else
              this.messageService.add({ severity: 'success', summary: 'Success message', detail: 'Role updated' });
              this.Reset();
            }
            else {
              this.messageService.add({ severity: 'error', summary: 'Server error', detail: 'Failed to add role ' });
            
            }
          },
        (error)=>{
          this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});

        });
        
      }
      else {
        this.messageService.add({ severity: 'warn', summary: 'Warning message', detail: 'Select role' });
      }
    }
    else{
      //this.messageService.add({ severity: 'warn', summary: 'Warning message', detail: 'Select associate' });

    }
      
    }

   CheckValue() : boolean{
      if (this.newRole.value.RoleId != null)
        return false;
      return true;

    }
   FilterRoles(roleId: number) : string {
    var obj = this.rolesList.find((x: SelectItem) => x.value === roleId);
    return obj.label;
  }

  // gets selected user object
 GetUserById() :void {
    var i;
    for (i = 0; i < this.usersList.length; i++) {
      if (this.usersList[i].UserId == this.selectedUserId) {
        this.editObj = this.usersList[i];
        return;
      }
    }
  }

 Reset() : void {
    this.formSubmitted = false;
    this.newRole.reset();
    this.newRole.get('IsActive').setValue(true);
    this.btnLabel = "Save";

  }
  ngOnDestroy() {
    this.userRole.unsubscribe();
    console.log("destroy called")
  }
}
