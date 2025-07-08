import { NgModule } from '@angular/core';
import { AddKraComponent } from './add-kra/add-kra.component';
import { KraaspectComponent } from './kraaspect/kraaspect.component';
import { KraRoutingModule } from './kra-routing.module';
import { CommonModule } from '@angular/common';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { KraScaleMasterComponent } from './kra-scale-master/kra-scale-master.component';
import { AspectmasterComponent } from './aspectmaster/aspectmaster.component';
import { CloneKraComponent } from './clone-kra/clone-kra.component';
import { FinancialyearComponent } from './financialyear/financialyear.component';
import { CustomKraComponent } from './customKra/add-custom-kra/add-custom-kra.component';
import { CustomKrasOperations } from './customKra/view-and-modify-custom-kra/view-and-modify-custom-kra.component';
import { AssociateKRAsComponent } from './associate-kras/associate-kras.component';
import { KraMeasurementTypeComponent } from './kra.measurement-type/kra.measurement-type.component';
import { AddKradefinitionComponent } from './add-kra/add-kradefinition.component';
import { KRAComponent } from './kra.component';
import { MapassociateroleComponent } from './mapassociaterole/mapassociaterole.component';
import { AllAngularMaterialModule } from 'src/app/plugins/all-angular-material.module';
import { ReviewKRAComponent } from './review-kra/review-kra.component';
import { DefineKRAComponent } from './define-kra/define-kra.component';
import { KraStatusComponent } from './kra-status/kra-status.component';
import { KraCommentsComponent } from './kra-comments/kra-comments.component';
import { CeostatusComponent } from './ceostatus/ceostatus.component';
import { ApplicableroletypesComponent } from './applicableroletypes/applicableroletypes.component';





@NgModule({
  imports: [ 
    KraRoutingModule,
    CommonModule,
    AppPrimenNgModule,
    FormsModule, 
    ReactiveFormsModule,
    AllAngularMaterialModule 
  ],
  declarations: [
      KraaspectComponent,
      KraScaleMasterComponent,
      AspectmasterComponent,
      KraScaleMasterComponent,
      CloneKraComponent,
      FinancialyearComponent,
      CustomKraComponent,
      CustomKrasOperations,
      AssociateKRAsComponent,
      KraMeasurementTypeComponent,
      AddKraComponent,
      AddKradefinitionComponent,
      KRAComponent,
      MapassociateroleComponent,
      ReviewKRAComponent,
      DefineKRAComponent,
      KraStatusComponent,
      KraCommentsComponent,
      CeostatusComponent,
      ApplicableroletypesComponent,       
  ]
})
export class KraModule { }
 