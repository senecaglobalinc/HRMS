import {Component, Input, ViewChild} from '@angular/core';
import { DesignationsTableComponent } from './designations-table/designations-table.component';

@Component({
    selector: 'app-designations',    
    template: '<app-designations-form></app-designations-form><app-designations-table></app-designations-table>',
})
export class DesignationsDirective {
  
  
}