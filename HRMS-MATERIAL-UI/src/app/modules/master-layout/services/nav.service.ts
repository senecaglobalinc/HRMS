import {EventEmitter, Injectable} from '@angular/core';
import {Event, NavigationEnd, Router} from '@angular/router';
import {BehaviorSubject, Subject} from 'rxjs';

@Injectable()
export class NavService {
  public appDrawer: any;
  public currentUrl = new BehaviorSubject<string>(undefined);
  public isSideNaveOpened = new BehaviorSubject<boolean>(true);

  // public searchBoxData = new BehaviorSubject<any>('');
  public searchBoxData = new Subject<any>();
  currentSearchBoxData = this.searchBoxData.asObservable();

  constructor(private router: Router) {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl.next(event.urlAfterRedirects);
      }
    });
  }

  public closeNav() {
    this.appDrawer.close();
    this.isSideNaveOpened.next(false);
  }

  public openNav() {
    this.appDrawer.open();
  }
  public toggle() {
    this.appDrawer.toggle();
    if(this.appDrawer._opened){
      this.isSideNaveOpened.next(true);
    }
    else{
      this.isSideNaveOpened.next(false);
    }
  }

  isSideNaveOpend(e:boolean) {
    this.isSideNaveOpened.next(e)
  }

  changeSearchBoxData(searchData:any) {
    this.searchBoxData.next(searchData);
  }

}
