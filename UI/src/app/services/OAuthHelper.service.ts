import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { User } from "../models/user.model";
import { Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class OAuthHelper {
  private _accessToken = "";
  private image ;
  setAccessToken(_accessToken: any): any {
    this._accessToken = _accessToken;
    sessionStorage["token"] = this._accessToken;
  }

  public getImage(){
    return this.image;
  }
  public setImage(image : Blob){
    this.image = image;
  }
  public loginData = new User();
  env = environment;
  constructor(private http: HttpClient, private route: Router) {}

  login() {
    window.location.href =
      "https://login.microsoftonline.com/" +
      this.env.TENANT_ID +
      "/oauth2/authorize?response_type=id_token&client_id=" +
      this.env.CLIENT_ID +
      "&redirect_uri=" +
      encodeURIComponent(this.env.RedirectURL) +
      "&state=SomeState&nonce=SomeNonce";
  }
  logout() {
    sessionStorage.clear();
    if(this.env.production == true)
        window.location.href = this.env.LogOutURL;
}

  public GetUserProfile() {
    return this.http.get("https://graph.microsoft.com/v1.0/me", {
      headers: new HttpHeaders({
        Authorization: "Bearer " + this._accessToken
      })
    });
  }

  public getAccessToken() {
    window.location.href =
      "https://login.microsoftonline.com/" +
      this.env.TENANT_ID +
      "/oauth2/authorize?response_type=token&client_id=" +
      this.env.CLIENT_ID +
      "&resource=" +
      this.env.GRAPH_RESOURCE +
      "&redirect_uri=" +
      encodeURIComponent(this.env.RedirectURL) +
      "&prompt=none&state=SomeState&nonce=SomeNonce";
  }

  public GetUserdetails(res) {
    const url = environment.ServerUrl + "/UserLogin";
    sessionStorage["mail"] = res.mail;
    sessionStorage["token"] = null;
    const data =
      "grant_type=password&username=" + res.mail + "&password=" + "gfhks";
    return this.http.post(url, data);
  }

  public getUserPhoto(){
    sessionStorage['token'] = this._accessToken;
    return this.http.get(this.env.Image,{ responseType: "blob" });
  }
}
