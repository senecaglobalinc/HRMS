import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KraComponent } from './components/kra/kra.component';
import { ApplicableroletypesComponent } from './components/applicableroletypes/applicableroletypes.component';
import { KraMeasurementTypeComponent } from './components/kra.measurement-type/kra.measurement-type.component';
import { AspectmasterComponent } from './components/aspectmaster/aspectmaster.component';
import { KraScaleMasterComponent } from './components/kra-scale-master/kra-scale-master.component';
import { DefineKRAComponent } from './components/define-kra/define-kra.component';
import { ReviewKRAComponent } from './components/review-kra/review-kra.component';
import { KraStatusComponent } from './components/kra-status/kra-status.component';
import { CeostatusComponent } from './components/ceostatus/ceostatus.component';
import { ViewKraComponent } from './components/view-kra/view-kra.component';
import { DownloadKraComponent } from './components/download-kra/download-kra.component';


const routes: Routes = [
    {
		path: '', component: KraComponent,
        children: [ 
            { path: "applicableroletypes", component: ApplicableroletypesComponent, data: { title: 'Applicable Role Types' }},
            { path: "krameasurementtype", component: KraMeasurementTypeComponent, data: { title: 'Measurement Types' }},
            { path: "aspectmaster", component: AspectmasterComponent, data: { title: 'Aspect' }},
            { path: "scaleMaster", component: KraScaleMasterComponent, data: { title: 'Scales' }},
            { path: "kradefinitions", component: DefineKRAComponent, data: { title: 'Define Key Result Areas' }},
            { path: "reviewkra", component: ReviewKRAComponent, data: { title: 'Review Key Result Areas' }},
            { path: "ceostatus", component: CeostatusComponent, data: { title: 'Status' }},
            { path: "status", component: KraStatusComponent, data: { title: 'Status' }},
            { path: 'krareview/:financialYearId/:departmentId', component: ReviewKRAComponent, data: { title: 'Review Key Result Areas' }},
            { path: 'kradefine/:financialYearId/:departmentId', component: DefineKRAComponent, data: { title: 'Define Key Result Areas' }},
            { path: "viewkra", component: ViewKraComponent, data: { title: 'Generate PDF' }},
            { path: "downloadkra", component: DownloadKraComponent, data: { title: 'Download KRA' }},
           ] 
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KraRoutingModule { }
