import { Component, ViewChild } from '@angular/core';
import { CompetencyAreaFormComponent } from './competency-area-form/competency-area-form.component';
import { CompetencyAreaTableComponent } from './competency-area-table/competency-area-table.component';

@Component({
    selector: 'app-competency-area',
    template: `
    <div class= "content-wrapper">
        <app-competency-area-form ></app-competency-area-form>
        <app-competency-area-table></app-competency-area-table>
    </div>`

})
export class CompetencyAreaDirective {
   
}