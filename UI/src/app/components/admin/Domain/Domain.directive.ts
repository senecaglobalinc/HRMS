import {Component, Input, ViewChild, Output} from '@angular/core';
import { EventEmitter } from 'protractor';

import { DomainTableComponent } from './domain-table/domain-table.component';
import { DomainMasterData } from '../models/domainmasterdata.model';


@Component({
    selector: 'app-domain',    
    template: '<app-domain-form></app-domain-form><app-domain-table></app-domain-table>',
})
export class DomainDirective {

}