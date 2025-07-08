import {Component} from '@angular/core';


@Component({
    selector: 'app-clients',    
    template: `
    <div class= "content-wrapper">
    <app-clients-form></app-clients-form>
    <app-clients-table></app-clients-table>
    </div>`
})

export class ClientsDirective{
    
}
