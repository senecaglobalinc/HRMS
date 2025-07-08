import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit
} from "@angular/core";
import { MediaMatcher } from "@angular/cdk/layout";
import { MenuService } from "../../../services/menu.service";
import { Router } from "@angular/router";
import { MessageService } from "primeng/api";
import * as environmentInformation from "../../../../environments/environment";
import { OAuthHelper } from "../../../services/OAuthHelper.service";
import { take } from "../../../../../node_modules/rxjs-compat/operator/take";

@Component({
  selector: "app-toolbar",
  templateUrl: "./toolbar.component.html",
  styleUrls: ["./toolbar.component.css"],
  providers: [MessageService]
})
export class ToolbarComponent implements OnInit, OnDestroy {
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

  constructor(
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private _authService: OAuthHelper,
    private _menuService: MenuService,
    private _router: Router
  ) {
    this.access_Token = sessionStorage['token'];
    this.menuQuery = media.matchMedia("(max-width: 600px)");
    this._menuQueryListener = () => changeDetectorRef.detectChanges();
    this.menuQuery.addListener(this._menuQueryListener);
    if(this.isProduction){
      this.image = this._authService.getImage();
      this.createImageFromBlob(this.image);
    }
     
  }

  ngOnInit() {
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
          this._router.navigate(["shared/dashboard"]);
        },
        error => {}
      );
    }
  }

  ngOnDestroy(): void {
    this.menuQuery.removeListener(this._menuQueryListener);
  }

  switchRole() {
    this._router.navigate(["/role"]);
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
