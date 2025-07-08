import { Component, HostBinding, OnInit } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';
import { ThemechangerService } from './core/services/themechanger.service';
import { NavigationEnd, Router } from '@angular/router';
import { PageTitleService } from './core/services/page-title.service';
import { Title } from '@angular/platform-browser';
import { NavService } from './modules/master-layout/services/nav.service';
import { filter } from 'rxjs/operators';
import { UrlService } from './modules/shared/services/url.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs/internal/Observable';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  userData$: Observable<any>;
  constructor(private authService: AuthService, private urlService: UrlService, public oidcSecurityService: OidcSecurityService, public navService: NavService, public router: Router, public overlayContainer: OverlayContainer, private themechangeservice: ThemechangerService, public pageTitleService: PageTitleService, titleService: Title) {
    this.themechangeservice.currentTheme.subscribe(theme => {
      this.onSetTheme(theme)
    })
    this.router.events.subscribe((ev) => {
      if (ev instanceof NavigationEnd) {
        let title = this.getTitle(router.routerState, router.routerState.root).join('-');
        titleService.setTitle(title);
        this.pageTitleService.changePageTitle(title);

        this.navService.changeSearchBoxData('');
      }
    });

  }

  previousUrl: string = null;
  currentUrl: string = null;
  ngOnInit() {
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.previousUrl = this.currentUrl;
      this.currentUrl = event.url;
      this.urlService.setPreviousUrl(this.previousUrl);
    });
  }

  name = 'Angular 7';
  @HostBinding('class') componentCssClass;
  title = 'hrms';
  themeChanger: boolean = false;

  onSetTheme(theme) {

    if (theme == true) {
      this.overlayContainer.getContainerElement().classList.add('dark-theme');
      this.componentCssClass = 'dark-theme';
    } else {
      this.overlayContainer.getContainerElement().classList.add('theme-alternate');
      this.componentCssClass = 'theme-alternate';
    }

  }


  getTitle(state, parent) {

    var data = [];
    if (parent && parent.snapshot.data && parent.snapshot.data.title) {
      data.push(parent.snapshot.data.title);
    }

    if (state && parent) {
      data.push(... this.getTitle(state, state.firstChild(parent)));
    }
    return data;
  }

}
