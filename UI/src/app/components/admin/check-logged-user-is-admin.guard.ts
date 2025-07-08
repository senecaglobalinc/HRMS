import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Location } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CheckLoggedUserIsAdminGuard implements CanActivate {
  constructor(private route: Router,  private location: Location){}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {  
      // let role =  JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;     
      //   if(role == "SystemAdmin")
      //     return true;
      //   else{
      //       this.route.navigate(['/shared/accessdenied']);
      //       return false;
      //      }
      return true;
  }
}
