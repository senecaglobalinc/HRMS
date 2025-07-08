import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TalentManagementComponent } from './talentmanagement.component';
import { AssociateallocationComponent } from './associateallocation/associateallocation.component';
import { ProjectViewComponent } from './projects/projectview.component';
import { AddProjectComponent } from './projects/addproject.component';
import { AssociateReleaseComponent } from './associate-release/associate-release.component';
import { AllocationChangesComponent } from './allocation-changes/allocation-changes.component';


const routes: Routes = [
    {
        path: '', component: TalentManagementComponent,
        children: [
            { path: 'allocation', component: AssociateallocationComponent },
            { path: 'allocatefromtagassociate/:tagid/:empid', component: AssociateallocationComponent },
            // { path: 'allocatefromtagassociate/:tagid/:empid/:availableallocation', component: AssociateallocationComponent },
            { path: 'release', component: AssociateReleaseComponent },
            { path: 'projectsview', component: ProjectViewComponent },
            { path: 'addproject/:id', component: AddProjectComponent },
            { path: 'allocationchanges', component: AllocationChangesComponent },
                     
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
})

export class TalentManagementRoutingModule { }