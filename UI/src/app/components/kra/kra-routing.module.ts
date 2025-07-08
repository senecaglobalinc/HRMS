import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KraaspectComponent } from './kraaspect/kraaspect.component';
import { KRAComponent } from './kra.component';
import { AspectmasterComponent } from './aspectmaster/aspectmaster.component';
import { KraScaleMasterComponent } from './kra-scale-master/kra-scale-master.component';
import { CloneKraComponent } from './clone-kra/clone-kra.component';
import { FinancialyearComponent } from './financialyear/financialyear.component';
import { CustomKraComponent } from './customKra/add-custom-kra/add-custom-kra.component';
import { CustomKrasOperations } from './customKra/view-and-modify-custom-kra/view-and-modify-custom-kra.component';
import { AssociateKRAsComponent } from './associate-kras/associate-kras.component';
import { KraMeasurementTypeComponent } from './kra.measurement-type/kra.measurement-type.component';
import { AddKraComponent } from './add-kra/add-kra.component';
import { AddKradefinitionComponent } from './add-kra/add-kradefinition.component';
import { MapassociateroleComponent } from './mapassociaterole/mapassociaterole.component';
import { ReviewKRAComponent } from './review-kra/review-kra.component';
import { KraStatusComponent } from './kra-status/kra-status.component';
import { DefineKRAComponent } from './define-kra/define-kra.component';
import { ApplicableroletypesComponent } from './applicableroletypes/applicableroletypes.component';
import { CeostatusComponent } from './ceostatus/ceostatus.component';
import { KraCommentsComponent } from './kra-comments/kra-comments.component';

const routes: Routes = [
    {
        path: "", component: KRAComponent,
        children: [
            { path: 'kraaspect', component: KraaspectComponent },
            { path: 'aspectmaster', component: AspectmasterComponent },
            { path: 'scaleMaster', component: KraScaleMasterComponent },
            { path: 'clonekra', component: CloneKraComponent },
            { path: 'financialyear', component: FinancialyearComponent },
            { path: 'customkras', component: CustomKraComponent },
            { path: 'reviewkra', component: ReviewKRAComponent },
            { path: 'krareview/:financialYearId/:departmentId',  component: ReviewKRAComponent  },
            { path: 'krastatus', component: KraStatusComponent },
            { path: 'ceostatus', component: CeostatusComponent },
            { path: 'definekra', component: DefineKRAComponent },
             { path: 'kradefine/:financialYearId/:departmentId',  component: DefineKRAComponent  },
            { path: 'applicableroletypes', component: ApplicableroletypesComponent },
            { path: 'commentmode', component: KraCommentsComponent },
            { path: 'view-associate-kras/:employeeId/:financialYearId/:projectId', component: CustomKrasOperations },
            { path: 'customkras/:employeeId/:financialYearId/:projectId', component: CustomKraComponent },
            { path: 'krainformation', component: AssociateKRAsComponent },
            { path: 'krameasurementtype', component: KraMeasurementTypeComponent },
            { path: 'scaleMaster', component: KraScaleMasterComponent },
            { path: 'kradefinitions', component: AddKraComponent },
            { path: 'addkradefinition/:type/:financialyear',  component:  AddKradefinitionComponent  },
            { path: 'kradefinitions/:financialYearId/:departmentId',  component:  AddKraComponent  },
            { path: 'mapassociaterole/:isNew', component: MapassociateroleComponent },
        ] 
    },
    // { path: 'mappingassociaterole/mapassociaterole', redirectTo: "mappingassociaterole/mapassociaterole/"},
    // {
    //     path: 'mappingassociaterole', component: KRAComponent,
    //     children: [
    //         { path: 'mapassociaterole/:isNew', component: MapassociateroleComponent },
    //     ]
    // },
];

@NgModule({
    imports:
        [RouterModule.forChild(routes)],

})
export class KraRoutingModule {


}
