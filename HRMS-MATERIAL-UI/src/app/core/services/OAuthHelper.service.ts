import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
//import {environment} from '../../../environments/environment.qa'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../../modules/auth/models/user.model';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import {
  OidcClientNotification,
  OidcSecurityService,
  PublicConfiguration,
} from 'angular-auth-oidc-client';

@Injectable()
export class OAuthHelper {
  private _accessToken = '';
  private image;

  setAccessToken(_accessToken: any): any {
    this._accessToken = _accessToken;
    sessionStorage['token'] = this._accessToken;
  }

  public getImage() {
    return this.image;
  }
  public setImage(image: Blob) {
    this.image = image;
  }
  public loginData = new User();
  env = environment;
  constructor(
    private http: HttpClient,
    private route: Router,
    public oidcSecurityService: OidcSecurityService
  ) {}

  logout() {
    window.location.replace(window.location.origin);
  }

  public GetUserProfile() {
    return this.http.get('https://graph.microsoft.com/v1.0/me', {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + this._accessToken,
      }),
    });
  }

  public GetUserdetails(res) {
    const url = environment.ServerUrl + '/UserLogin';
    sessionStorage['mail'] = res.mail;
    sessionStorage['token'] = null;
    const data =
      'grant_type=password&username=' + res.mail + '&password=' + 'gfhks';
    let x = this.http.post(url, data);
    return x;
  }

  // public getUserPhoto() {
  //   sessionStorage['token'] = this._accessToken;
  //   return this.http.get(this.env.Image, { responseType: 'blob' });
  // }
}
