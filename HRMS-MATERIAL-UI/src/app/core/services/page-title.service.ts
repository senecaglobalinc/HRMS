import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PageTitleService {
  private pageTitle = new BehaviorSubject('default message');
  currentPageTitle = this.pageTitle.asObservable();

  constructor() {}

  changePageTitle(pagTitleData: string) {
    this.pageTitle.next(pagTitleData);
  }
}
