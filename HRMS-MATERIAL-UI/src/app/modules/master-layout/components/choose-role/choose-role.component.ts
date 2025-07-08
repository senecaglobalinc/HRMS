import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "../../models/user.model";
// import { SelectItem } from "primeng/components/common/api";
import { ProjectRoleService } from "../../services/project-role.service";
import { OAuthHelper } from "../../../../core/services/OAuthHelper.service";
import * as environmentInformation from "../../../../../environments/environment";
import { AuthService } from "src/app/core/services/auth.service";
import { RoleService } from "src/app/modules/admin/services/role.service";
import { NgxSpinnerService } from "ngx-spinner";
import { MenuService } from "src/app/core/services/menu.service";
import { environment } from "../../../../../environments/environment";
import { SignoutDialogComponent } from "../signout-dialog/signout-dialog.component";
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: 'app-choose-role',
  templateUrl: './choose-role.component.html',
  styleUrls: ['./choose-role.component.scss'],
  providers: [ProjectRoleService]
})
export class ChooseRoleComponent implements OnInit {
  public loginData: User = new User();
  public roleName: String;
  display = true;
  roles: any[] = [];
  selectedRole: string;
  showRolesDialog = false;
  showRoleErrorMessage = false;
  access_token;
  loadHomePage = false;
  image;
  fullName = "";
  isImagePresent = false;
  isProduction = environmentInformation.environment.production;
  rolesAndDepartments: any;
   constructor(private _router: Router,
    private _authService: OAuthHelper,
    private authService: AuthService,
    private _rolesService: RoleService,
    private spinner: NgxSpinnerService,
    private _menuService: MenuService,
     public dialog: MatDialog,
  ) {
    this.access_token = sessionStorage['token'];
    // if (sessionStorage["AssociatePortal_UserInformation"]) {
    //   this.loginData = JSON.parse(
    //     sessionStorage["AssociatePortal_UserInformation"]
    //   );
    //   this.fullName = this.loginData.firstName +" "+this.loginData.lastName;
    // }
    // if(this.isProduction)
    //    this.getUserPhoto();
  }

  ngOnInit() {
    // if (!this.loginData) {
    //   this._router.navigate(["/login"]);
    //   return false;
    // }
    this.GetLoggedInUserRoles();
  }

  // getUserPhoto() {
  //   this._authService.getUserPhoto().subscribe(res => {
  //     this.image = res;
  //     this.createImageFromBlob(res);
  //   });
  // }
  GetLoggedInUserRoles() {
    sessionStorage['token'] = this.access_token;
    // const roles = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
    //   .roles;
    // const role: any[] = roles.split(",");
    // sessionStorage["Load"] = 0;
    // role.forEach(element => {
    //   this.roles.push({ label: element, value: element });
    // });

    let email = sessionStorage['email'];
    // if(email ==''){
    //   email = (sessionStorage['loggedEmail']);
    // }
    this.spinner.show();
    this.getRolesandDepartments();
    this._menuService.GetUserDetailsByUserName(email).subscribe(
      (data: any) => {
        this.loginData.roles = data.roles;
        this.loginData.roleName = data.roles;
        this.loginData.employeeId = data.EmployeeId;
        this.loginData.employeeCode = data.EmployeeCode;
        this.loginData.email = data.username;
        this.loginData.departmentId = data.EmployeeDepartmentId;
        this.loginData.allowedWfoInHrms = data.allowedWfoInHrms;
        var uname = data.username.split('@');
        if (uname.length > 1 || uname[0].indexOf('.') > 0) {
          this.loginData.firstName = uname[0].split('.')[0];
          this.loginData.lastName = uname[0].split('.')[1];
          if (this.loginData.lastName == undefined) {
            this.loginData.lastName = '';
          }
          this.loginData.fullName =
            this.loginData.firstName + ' ' + this.loginData.lastName;
        }
        sessionStorage['AssociatePortal_UserInformation'] = JSON.stringify(
          this.loginData
        );
        let role = this.loginData.roles.split(',');
        sessionStorage['roleLength'] = role.length;
        sessionStorage["Load"] = 0;
        if(role.length > 1){
          role.forEach(element => {
            if(element != ''){
              this.roles.push({ label: element, value: element });
            }
          });
        }
        else{
          this.changeRole(this.loginData.roles)
        }
        this.spinner.hide();
      },
      (error) => {
        this.spinner.hide();
        this._router.navigate(['/errorPage']);
      }
    );


  }

  changeRole(roleName: string) {
    if (roleName != null && roleName != undefined) {
      sessionStorage["Load"] = 1;
      this.loginData.roleName = roleName;
      sessionStorage["AssociatePortal_UserInformation"] = JSON.stringify(
        this.loginData
      );
      this.rolesAndDepartments = JSON.parse(
        sessionStorage.getItem('RolesAndDepartments')
      )
      this.rolesAndDepartments.forEach(element => {
        if (element.RoleName === roleName) {
          const deptObj = {
            'DepartmentId': element.DepartmentId,
            'DepartmentName': element.DepartmentCode
          }
          sessionStorage.setItem('Department', JSON.stringify(deptObj));
        }
      });
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



  getRolesandDepartments() {
    this._rolesService.GetRolesAndDepartments().subscribe(res => {
      this.rolesAndDepartments = res;
      sessionStorage['RolesAndDepartments'] = JSON.stringify(
        this.rolesAndDepartments
      );
    })
  }

  logout(){
    this.dialog.open(SignoutDialogComponent, {
         height:"200px",
         width:"300px",
         disableClose: true
       });

      }
}
