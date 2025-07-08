import { Component } from "@angular/core";
import { environment } from "../environments/environment";
import { Router } from "../../node_modules/@angular/router";
import { OAuthHelper } from "./services/OAuthHelper.service";
import { Subscription } from "../../node_modules/rxjs";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  title = "Associate Portal";
  isProd: boolean = false;
  constructor(private router: Router, private authHelper: OAuthHelper) {
    this.isProd = environment.production;
  }

  ngOnInit() { 
    if (this.isProd) {
    if (window.location.hash.includes("id_token")) {
      const id_token = window.location.hash;
      this.router.navigate(["/authLogin"], { queryParams: { id_token: id_token.replace('#id_token=', '') } });
      return;
    }

    if (window.location.href.includes("access_token")) {
      const access_token = window.location.hash;
      this.router.navigate(["/authLogin"], { queryParams: { access_token: access_token.split("#")[1]
      .split("&")[0]
      .replace("access_token=", "") } });
      return;
    }
      this.router.navigate(["/perform-auth-login"]);
    } else {
       this.router.navigate(["/login"]);
    }
  }
}
