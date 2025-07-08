import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  }   from '@angular/forms';
import { AdrRoutingModule } from './adr-routing.module';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
import { AdrcycleComponent } from './adrcycle/adrcycle.component';
import { AdrComponent } from './adr.component';
import { AdrOrganisationDevelopmentMasterComponent } from './adr-organisation-development-master/adr-organisation-development-master.component';
import { CommonModule } from '@angular/common';
import { AdrOrganisationValueMasterComponent } from './adr-organisation-value-master/adr-organisation-value-master.component';
import { AdrConfigurationComponent } from './adr-configuration/adr-configuration.component';
import { AdrSectionsComponent } from './adr-sections/adr-sections.component';
import { AdrAppreciateAssociateComponent } from './appreciations/adr-appreciate-associate/adr-appreciate-associate.component';
import { AdrAppreciationsofAssociateComponent } from './appreciations/adr-appreciationsof-associate/adr-appreciationsof-associate.component';
import { AdrKrasComponent } from './adr-kras/adr-kras.component';
import { AdrWorkplaceBehaviourComponent } from './adr-workplace-behaviour/adr-workplace-behaviour.component';
import { AdrOrganizationDevelopmentComponent } from './adr-organization-development/adr-organization-development.component';


@NgModule({

    imports: [ 
      AdrRoutingModule,
      AppPrimenNgModule,
      FormsModule,
      ReactiveFormsModule,
      CommonModule,
      
],
declarations: [
  AdrcycleComponent, 
  AdrComponent, 
  AdrOrganisationDevelopmentMasterComponent, 
  AdrOrganisationValueMasterComponent, 
  AdrConfigurationComponent, 
  AdrSectionsComponent, 
  AdrAppreciateAssociateComponent, 
  AdrAppreciationsofAssociateComponent,
  AdrKrasComponent,
  AdrWorkplaceBehaviourComponent,
  AdrOrganizationDevelopmentComponent,
],
 
exports: [ ]
})
export class AdrModule { }