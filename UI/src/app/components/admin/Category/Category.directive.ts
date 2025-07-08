import {Component, Input} from '@angular/core';

@Component({
    selector: 'app-categoryies',    
    template: `
    <div class= "content-wrapper">
        <app-category-form></app-category-form>
        <app-category-table></app-category-table>
    </div>`
})

export class CategoryDirective{

}
