import {Component, Input, ViewChild} from '@angular/core';
import { GradesFormComponent } from './grades-form/grades-form.component';

@Component({
    selector: 'app-grades',    
    template: '<div class= "content-wrapper"><app-grades-form></app-grades-form><app-grades-table ></app-grades-table></div>',
})
export class GradesDirective {
   
}