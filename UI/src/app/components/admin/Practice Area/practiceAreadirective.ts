import {Component, Input, ViewChild} from '@angular/core';
import { PracticeAreaTableComponent } from './practice-area-table/practice-area-table.component';

@Component({
    selector: 'app-practice-area',    
    template: '<app-practice-area-form></app-practice-area-form><app-practice-area-table></app-practice-area-table>',
     
})
export class PracticeAreaDirective {

}