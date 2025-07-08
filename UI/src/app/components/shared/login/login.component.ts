import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "../../../models/user.model";
import { MessageService } from "primeng/api";
import { OAuthHelper } from "../../../services/OAuthHelper.service";
@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
  providers: [MessageService]
})
export class LoginComponent implements OnInit {
  public _login: User;
  email: string;
  password: string;
  loading: boolean = false;
  errorMessage: string;
  public loginData = new User();

  constructor(
    private route: Router,
    private _service: OAuthHelper,
    private messageService: MessageService
  ) {
    this._login = new User();
  }

  ngOnInit() {
  }

  login() {
    if (this._login.email != null)
      this._service.GetUserdetails({ mail: this._login.email }).subscribe(
        (data: any) => {
          sessionStorage["token"] = data.access_token;
          this.loginData.roles = data.role;
          this.loginData.roleName = data.role;
          this.loginData.employeeId = data.employeeId;
          this.loginData.email = data.userName;
          var uname = data.userName.split("@");
          if (uname.length > 1 || uname[0].indexOf(".") > 0) {
            this.loginData.firstName = uname[0].split(".")[0];
            this.loginData.lastName = uname[0].split(".")[1];
            if (this.loginData.lastName == undefined) {
              this.loginData.lastName = "";
            }
            this.loginData.fullName =
              this.loginData.firstName + " " + this.loginData.lastName;
          }

          sessionStorage["AssociatePortal_UserInformation"] = JSON.stringify(
            this.loginData
          );
          let  roles = this.loginData.roles.split(",")
          if(roles.length > 1)
            this.route.navigate(["/role"]);
          else
          this.route.navigate(["/shared/dashboard"]);
        },
        error => {
           this.route.navigate(['/errorPage']);
        }
      );
    else
      this.messageService.add({
        severity: "warn",
        summary: "Warning Message",
        detail: "Email can't be empty"
      });
  }
}
