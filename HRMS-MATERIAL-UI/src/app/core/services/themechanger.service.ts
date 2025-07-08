import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemechangerService {

 
  private darkThemeVrbl = new BehaviorSubject(false);
  currentTheme = this.darkThemeVrbl.asObservable();

  constructor() { }

  darkTheme(theme: boolean) {
    this.darkThemeVrbl.next(theme)
  }

}
