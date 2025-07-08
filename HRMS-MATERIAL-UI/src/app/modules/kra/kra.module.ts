import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { KraRoutingModule } from './kra-routing.module';
import { KraComponent } from './components/kra/kra.component';
import { ApplicableroletypesComponent } from './components/applicableroletypes/applicableroletypes.component';
import { KraMeasurementTypeComponent } from './components/kra.measurement-type/kra.measurement-type.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { AspectmasterComponent } from './components/aspectmaster/aspectmaster.component';
import { KraScaleMasterComponent } from './components/kra-scale-master/kra-scale-master.component';
import { DefineKRAComponent }  from './components/define-kra/define-kra.component';
import { ReviewKRAComponent }  from './components/review-kra/review-kra.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { AddKRAdlgComponent }  from './components/add-kradlg/add-kradlg.component';
import { ImportKraDlgComponent }  from './components/import-kra-dlg/import-kra-dlg.component';
import { KraStatusComponent } from './components/kra-status/kra-status.component';
import { CeostatusComponent } from './components/ceostatus/ceostatus.component';
import { KraCommentComponent } from './components/kra-comment/kra-comment.component';
import { CloneKraDlgComponent } from './components/clone-kra-dlg/clone-kra-dlg.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ViewKraComponent } from './components/view-kra/view-kra.component';
import { NumberDirective  } from './directives/app-numbers-only.directives';
import { DownloadKraComponent } from './components/download-kra/download-kra.component';

@NgModule({
  declarations: [KraComponent,ApplicableroletypesComponent,KraMeasurementTypeComponent,AspectmasterComponent
  ,KraScaleMasterComponent,DefineKRAComponent,AddKRAdlgComponent,ImportKraDlgComponent,ReviewKRAComponent,KraStatusComponent
  ,CeostatusComponent, KraCommentComponent, CloneKraDlgComponent, ViewKraComponent, NumberDirective, DownloadKraComponent],
  imports: [
    CommonModule,
    AllAngularMaterialModule,
    KraRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AngularEditorModule,
    NgxSpinnerModule
  ]
})
export class KraModule { }
