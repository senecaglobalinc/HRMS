import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Location } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CheckUsersGuard implements CanActivate {
  constructor(private route: Router,  private location: Location){}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {  
    //   let role =  JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;  
    //     if(role == "Program Manager" || role == "Department Head" || role == "HRA" || role == "HRM")
    //       return true;
    //   //this.location.back();
    //  else{
    //   this.route.navigate(['']);
    //   return false;
    //  }
      return true;
  }
}
