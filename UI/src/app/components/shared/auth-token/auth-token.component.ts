import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { MessageService } from "primeng/api";
import { User } from "../../../models/user.model";
import { OAuthHelper } from "../../../services/OAuthHelper.service";

@Component({
  selector: "app-auth-token",
  templateUrl: "./auth-token.component.html",
  styleUrls: ["./auth-token.component.scss"],
  providers: [MessageService]
})
export class AuthTokenComponent implements OnInit {
  public loginData = new User();
  isValidUser: boolean = false;
  inValidUser = false;

  constructor(
    private router: Router,
    private authService: OAuthHelper,
    private activatedRoute: ActivatedRoute
  ) {}

  access_token: any;
  id_token: any;

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.id_token = params["id_token"];
      this.access_token = params["access_token"];
    });

    if (this.id_token && !this.access_token) {
      this.authService.getAccessToken();
    }
    if (!this.access_token) {
    } else {
      this.authService.setAccessToken(this.access_token);
      this.authService.GetUserProfile().subscribe(response => {
        this.authService.GetUserdetails(response).subscribe(
          (data: any) => {
            this.loginData = new User();
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
              this.router.navigate(["/role"]);
            else
            this.router.navigate(["/shared/dashboard"]);
          },
          error => {
            this.router.navigate(["/errorPage"]);
          }
        );
      });
    }
  }
}
