import {
  Component,
  OnInit,
  HostBinding,
  ChangeDetectorRef,
} from '@angular/core';
import { NavService } from '../../services/nav.service';
import { ThemechangerService } from 'src/app/core/services/themechanger.service';
import { MediaMatcher } from '@angular/cdk/layout';
 import { OAuthHelper } from 'src/app/core/services/OAuthHelper.service';
import { MenuService } from '../../services/menu.service';
import { Router } from '@angular/router';

import * as environmentInformation from '../../../../../environments/environment';
import { themeconfig } from 'src/themeconfig';
import { PageTitleService } from 'src/app/core/services/page-title.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ProfileDialogComponent } from '../profile-dialog/profile-dialog.component';
import { environment } from '../../../../../environments/environment';
import { SignoutDialogComponent } from '../signout-dialog/signout-dialog.component';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss'],
})
export class TopNavComponent implements OnInit {
  pageTitle: string = '';
  darktheme: boolean = false;
  dispLogoDiv: boolean = false;
  searchField: string = '';
  themeConfigInput = themeconfig.formfieldappearances;
  associateFullName:string;

  isProduction = environmentInformation.environment.production;
  public role : Number;


  constructor(
    public navService: NavService,
    private themechangeservice: ThemechangerService,
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private _authService: OAuthHelper,
    private _menuService: MenuService,
    private _router: Router,
    public pageTitleService: PageTitleService,
    private authService : AuthService,
    public dialog: MatDialog,
    

  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      if (responseData == '') {
        this.searchField = responseData;
      }
    });

    window.onresize = () => {
      // set screenWidth on screen size change
      this.navService.isSideNaveOpened.subscribe((resp) => {
        resp;
        if (resp && window.innerWidth > 840) {
          this.dispLogoDiv = false;
        } else if (!resp && window.innerWidth > 840) {
          this.dispLogoDiv = true;
        } else {
          this.dispLogoDiv = true;
        }
      });
    };

    this.navService.isSideNaveOpened.subscribe((resp) => {
      resp;
      if (resp && window.innerWidth > 840) {
        this.dispLogoDiv = false;
      } else if (!resp && window.innerWidth > 840) {
        this.dispLogoDiv = true;
      } else {
        this.dispLogoDiv = true;
      }
    });

    this.pageTitleService.currentPageTitle.subscribe(
      (respPageTitle) => (this.pageTitle = respPageTitle)
    );
  }

  ngOnInit() {
    if (sessionStorage.getItem('AssociatePortal_UserInformation') != null) {
      this.associateFullName = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).fullName;
      this.associateFullName = this.associateFullName.toUpperCase();
    }
    this.themechangeservice.currentTheme.subscribe((theme) => {
      this.darktheme = theme;
    });

    this.role = Number(JSON.parse(sessionStorage['roleLength']));
    
  }
  onSetTheme(e) {
    if (e.checked === true) {
      this.themechangeservice.darkTheme(true);
    } else {
      this.themechangeservice.darkTheme(false);
    }
  }

  switchRole() {
    this._router.navigate(['/roles']);
  }

  
  logout(){
    this.dialog.open(SignoutDialogComponent, {
      height:"200px",
      width:"300px",
      disableClose: true
    });
  }

  applyFilter($event) {
    this.navService.changeSearchBoxData($event);
  }
  clearField() {
    this.searchField = '';
    this.navService.changeSearchBoxData('');
  }

  openProfileDialog(){
    this.dialog.open(ProfileDialogComponent, {
      height:"300px",
      width:"600px",
      disableClose: true
    });
  }
}