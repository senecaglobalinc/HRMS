import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "../../../models/user.model";
import { SelectItem } from "primeng/components/common/api";
import { ProjectRoleService } from "../../../services/project-role.service";
import { OAuthHelper } from "../../../services/OAuthHelper.service";
import * as environmentInformation from "../../../../environments/environment";

@Component({
  selector: "app-choose-role",
  templateUrl: "./choose-role.component.html",
  styleUrls: ["./choose-role.component.css"],
  providers: [ProjectRoleService]
})
export class ChooseRoleComponent implements OnInit {
  public loginData: User;
  public roleName: String;
  display = true;
  roles: SelectItem[] = [];
  selectedRole: string;
  showRolesDialog = false;
  showRoleErrorMessage = false;
  access_token;
  loadHomePage = false;
  image ;
  fullName = "";
  isImagePresent = false;
  isProduction = environmentInformation.environment.production;
  constructor(private _router: Router,
    private _authService: OAuthHelper,) {
     this.access_token = sessionStorage['token']; 
    if (sessionStorage["AssociatePortal_UserInformation"]) {
      this.loginData = JSON.parse(
        sessionStorage["AssociatePortal_UserInformation"]
      );
      this.fullName = this.loginData.firstName +" "+this.loginData.lastName;
    }
    if(this.isProduction)
       this.getUserPhoto();
  }

  ngOnInit() {
    if (!this.loginData) {
      this._router.navigate(["/login"]);
      return false;
    }
    this.GetLoggedInUserRoles();
  }
  getUserPhoto(){
    this._authService.getUserPhoto().subscribe(res =>{
      this.image = res;
      this.createImageFromBlob(res);
    });
  }
  GetLoggedInUserRoles() {
    sessionStorage['token'] = this.access_token;
    const roles = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
      .roles;
    const role: any[] = roles.split(",");
    sessionStorage["Load"] = 0;
    role.forEach(element => {
      this.roles.push({ label: element, value: element });
    });
  }

  changeRole(roleName: string) {
    if (roleName != null && roleName != undefined) {
      sessionStorage["Load"] = 1;
      this.loginData.roleName = roleName;
      sessionStorage["AssociatePortal_UserInformation"] = JSON.stringify(
        this.loginData
      );
      this._router.navigate(["/shared/dashboard"]);
    } else {
      return;
    }
  }
  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.image = reader.result;
    }, false);
    if (image) {
      reader.readAsDataURL(image);
      this._authService.setImage(image);
    }
    this.isImagePresent = true;
  }
  logout() {
    this._authService.logout();
    if (this.isProduction === false)
      this._router.navigate(["/login"]);
    else this._router.navigate([""]);
  }
}
