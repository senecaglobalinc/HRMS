import {Component, ViewChild, ElementRef, ViewEncapsulation, AfterViewInit, HostBinding,  ChangeDetectorRef,
  OnDestroy,   OnInit} from '@angular/core';
import {VERSION} from '@angular/material/core';
import { NavService } from '../../services/nav.service';
import { navitems } from '../../menulist';
import { themeconfig } from '../../../../../themeconfig';
import { OverlayContainer } from '@angular/cdk/overlay';


import { MediaMatcher } from "@angular/cdk/layout";
import { MenuService } from "../../../master-layout/services/menu.service";
import { Router } from "@angular/router";
// import { MessageService } from "primeng/api";
import * as environmentInformation from "../../../../../environments/environment";
import { OAuthHelper } from "../../../../core/services/OAuthHelper.service";
// import { take } from "../../../../../node_modules/rxjs-compat/operator/take";

@Component({
  selector: 'app-master-layout',
  templateUrl: './master-layout.component.html',
  styleUrls: ['./master-layout.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MasterLayoutComponent implements AfterViewInit {

  roles = '';
  isImagePresent = false;
  menuQuery: MediaQueryList;
  private _menuQueryListener: () => void;
  menuDetails: any[];
  roleName: string;
  isExpanded = true;
  showSubmenu: boolean = false;
  isShowing = false;
  showSubSubMenu: boolean = false;
  events: string[] = [];
  opened: boolean = true;
  fullName: string;
  hideSwitchRole = false;
  image ;
  isProduction = environmentInformation.environment.production;
  access_Token = "";
  currentYear=new Date().getFullYear();
  mouseenter() {
    if (!this.isExpanded) {
      this.isShowing = true;
    }
  }

  mouseleave() {
    if (!this.isExpanded) {
      this.isShowing = false;
    }
  }




  screenWidth: number;
  @ViewChild('appDrawer') appDrawer: ElementRef;
  version = VERSION;
  navItems = navitems;
  appearance= themeconfig.formfieldappearances;

  constructor(public navService: NavService, public overlayContainer: OverlayContainer, changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private _authService: OAuthHelper,
    private _menuService: MenuService,
    private _router: Router) {
    // set screenWidth on page load
  this.screenWidth = window.innerWidth;
  window.onresize = () => {
    // set screenWidth on screen size change
    this.screenWidth = window.innerWidth;
    
  };

  this.access_Token = sessionStorage['token'];
    this.menuQuery = media.matchMedia("(max-width: 600px)");
    this._menuQueryListener = () => changeDetectorRef.detectChanges();
    this.menuQuery.addListener(this._menuQueryListener);
    //if(this.isProduction){
      this.image = this._authService.getImage();
      this.createImageFromBlob(this.image);
    //}
     

  }

  ngAfterViewInit() {
    this.navService.appDrawer = this.appDrawer;


    this.roles = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roles;
    const role: any[] = this.roles.split(",");

    if (role.length > 1) {
      this.hideSwitchRole = true;
    }
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    if (this.roles.length == 1) this.hideSwitchRole = true;
    this.fullName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).fullName;
    if (this.roleName != "" && this.roleName != undefined) {
      sessionStorage['token'] = this.access_Token;
      this._menuService.getMenuDetails(this.roleName).subscribe(
        (res: any) => {
          this.menuDetails = res;
          this._router.navigate(["/shared/dashboard"]);
        },
        error => {}
      );
    }

  }


  
  ngOnDestroy(): void {
    this.menuQuery.removeListener(this._menuQueryListener);
  }

 

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.image = reader.result;
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
    this.isImagePresent = true;
  }
  logout() {
    this._authService.logout();
    if (this.isProduction === false)
      this._router.navigate(["/login"]);
    else this._router.navigate([""]);
  }


}
