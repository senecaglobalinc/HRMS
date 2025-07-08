import { Component, OnInit } from "@angular/core";
import { OAuthHelper } from "../../../services/OAuthHelper.service";

@Component({
  selector: "app-perform-authlogin",
  templateUrl: "./perform-authlogin.component.html",
  styleUrls: ["./perform-authlogin.component.scss"]
})
export class PerformAuthloginComponent implements OnInit {
  constructor(private authHelper: OAuthHelper) {}

  ngOnInit() {
    this.authHelper.login();
  }
}
