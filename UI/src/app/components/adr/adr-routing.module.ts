import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdrcycleComponent } from './adrcycle/adrcycle.component';
import { AdrComponent } from './adr.component';
import { AdrOrganisationDevelopmentMasterComponent } from './adr-organisation-development-master/adr-organisation-development-master.component';
import { AdrOrganisationValueMasterComponent } from './adr-organisation-value-master/adr-organisation-value-master.component';
import { AdrSectionsComponent } from './adr-sections/adr-sections.component';
import { AdrConfigurationComponent } from './adr-configuration/adr-configuration.component';
import { AdrAppreciateAssociateComponent } from './appreciations/adr-appreciate-associate/adr-appreciate-associate.component';
import { AdrAppreciationsofAssociateComponent } from './appreciations/adr-appreciationsof-associate/adr-appreciationsof-associate.component';
import { AdrKrasComponent } from './adr-kras/adr-kras.component';
import { AdrWorkplaceBehaviourComponent } from './adr-workplace-behaviour/adr-workplace-behaviour.component';
import { AdrOrganizationDevelopmentComponent } from './adr-organization-development/adr-organization-development.component';

const routes: Routes = [
    {
        path: '', component: AdrComponent,
        children: [
            {path:'adrcycle', component:AdrcycleComponent}, 
            {path:'appreciateassociate', component:AdrAppreciateAssociateComponent},
            {path:'appreciationsofassociate', component:AdrAppreciationsofAssociateComponent},
            {path:'adrorganisationdevelopmentmaster', component:AdrOrganisationDevelopmentMasterComponent},
            {path:'adrorganisationvaluemaster', component:AdrOrganisationValueMasterComponent},
            {path:'adrsections', component:AdrSectionsComponent},
            {path:'adrconfiguration', component:AdrConfigurationComponent},
            {path:'adrkras', component:AdrKrasComponent},
            {path:'adrWorkplaceBehaviour', component:AdrWorkplaceBehaviourComponent},   
            {path:'adrOrganizationdevelopmenty', component:AdrOrganizationDevelopmentComponent},   
        ]
    },
];

@NgModule({
    imports:
        [RouterModule.forChild(routes)],
})
export class AdrRoutingModule {
}
