import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { ProgressSpinnerModule } from 'primeng/components/progressspinner/progressspinner';
import { PasswordModule } from 'primeng/components/password/password';
import { ButtonModule } from 'primeng/components/button/button';
import { DialogModule } from 'primeng/components/dialog/dialog';
import { TableModule } from 'primeng/table';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/components/dropdown/dropdown';
import { GrowlModule } from 'primeng/components/growl/growl';
import { EditorModule } from 'primeng/components/editor/editor';
import { AutoCompleteModule } from 'primeng/components/autocomplete/autocomplete';
import { CalendarModule } from 'primeng/components/calendar/calendar';
import { PickListModule } from 'primeng/components/picklist/picklist';
import { ToastModule } from 'primeng/components/toast/toast';
import { MessageModule } from 'primeng/components/message/message';
import { InputSwitchModule } from 'primeng/components/inputswitch/inputswitch';
import { TabViewModule } from 'primeng/components/tabview/tabview';
import { PaginatorModule } from 'primeng/components/paginator/paginator';
import { KeyFilterModule } from 'primeng/components/keyfilter/keyfilter';
import { SpinnerModule } from 'primeng/components/spinner/spinner';
import { ConfirmDialogModule } from 'primeng/components/confirmdialog/confirmdialog';
import {RadioButtonModule} from 'primeng/radiobutton';
import {CheckboxModule} from 'primeng/checkbox';
import {MultiSelectModule} from 'primeng/multiselect';
import {ChartModule} from 'primeng/chart';
import {PanelModule} from 'primeng/panel';
import {FieldsetModule} from 'primeng/components/fieldset/fieldset';
import {AccordionModule} from 'primeng/components/accordion/accordion';
import {TabMenuModule} from 'primeng/components/tabmenu/tabmenu';
import {OverlayPanelModule} from 'primeng/overlaypanel';
import { ProgressBarModule, StepsModule, BreadcrumbModule } from 'primeng/primeng';
import {CardModule} from 'primeng/components/card/card';

const PrimeNgModules = [
    CommonModule,OverlayPanelModule, PasswordModule, InputTextModule, ButtonModule, TabViewModule, ConfirmDialogModule, CheckboxModule,
    DialogModule, TableModule, ProgressSpinnerModule, InputTextareaModule,
    DropdownModule, GrowlModule, EditorModule, AutoCompleteModule, ConfirmDialogModule,
    RadioButtonModule, CalendarModule, PickListModule, ToastModule, PaginatorModule, MessageModule,
     InputSwitchModule, KeyFilterModule, SpinnerModule, MultiSelectModule, ChartModule, PanelModule, TabMenuModule,
     FieldsetModule, AccordionModule, StepsModule, BreadcrumbModule, ProgressBarModule, CardModule,
     OverlayPanelModule ];
     
@NgModule({
    imports: PrimeNgModules,
    exports: PrimeNgModules,
    declarations: []
})
export class AppPrimenNgModule {


}
