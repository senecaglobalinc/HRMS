import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import * as servicePath from 'src/app/core/service-paths';
import { MatDialog } from '@angular/material/dialog';
import { jwtDecode } from "jwt-decode";
import { Router } from '@angular/router';
import { TokenexpirywarningDialogComponent } from 'src/app/modules/auth/components/tokenexpirywarning-dialog/tokenexpirywarning-dialog.component';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  Url = environment.Url;
  private resourceURL = servicePath.API.Role;
  private tokenExpiryWarningShown = false;

  constructor(private http: HttpClient, public dialog: MatDialog, private router: Router) {}

  getAcesstoken(tokenRequestObj) {
    return this.http.post(this.Url, tokenRequestObj);
  }

  loggedIn(): boolean {
    const token = sessionStorage.getItem('token');
    
    if (!token) {
      return false;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      const expirationTime = decodedToken.exp * 1000;
      const currentTime = new Date().getTime();

      if (expirationTime < currentTime) {
        this.logout();
        return false;
      }

      const timeUntilExpiration = expirationTime - currentTime;
      if (timeUntilExpiration <= 5 * 60 * 1000 && !this.tokenExpiryWarningShown) {
        this.scheduleTokenExpiryWarning(expirationTime, currentTime);
      }

      return true;
    } catch (error) {
      this.logout();
      return false;
    }
  }

  private scheduleTokenExpiryWarning(expirationTime: number, currentTime: number): void {
    const fiveMinutesBefore = expirationTime - (5 * 60 * 1000);
    const timeUntilWarning = fiveMinutesBefore - currentTime;
    if (timeUntilWarning <= 0 || this.tokenExpiryWarningShown) {
      return; 
    }

    (window as any).tokenExpiryWarningTimer = setTimeout(() => {
      this.showTokenExpiryWarning();
      this.tokenExpiryWarningShown = true;  
    }, timeUntilWarning);
  }

  private showTokenExpiryWarning(): void {
    this.dialog.open(TokenexpirywarningDialogComponent, {
      height: "200px",
      width: "300px",
      disableClose: true
    });
  }

  
  setAccessToken(_accessToken: any): void {
    sessionStorage.setItem("token", _accessToken);
    this.tokenExpiryWarningShown = false;  
  }

  public GetLoggedInUserEmail() {
    let url = environment.AdminMicroService + this.resourceURL.GetLoggedInUserEmail;
    return this.http.get(url, { responseType: "text" });
  }

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
