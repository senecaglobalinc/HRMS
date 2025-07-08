import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { AssociateallocationComponent } from './Components/associateallocation/associateallocation.component';
import { AssociateReleaseComponent } from './Components/associate-release/associate-release.component';
import { AllocationChangesComponent } from './Components/allocation-changes/allocation-changes.component';
import { FutureAllocationsComponent } from './Components/future-allocations/future-allocations.component';

const routes: Routes = [

  {
    path: '', component: HomeComponent, children: [
      { path: "allocation", component: AssociateallocationComponent, data: { title: 'Associate Allocation' } },
      { path: "release", component: AssociateReleaseComponent, data: { title: 'Associate Release' } },
      { path: "allocationchanges", component: AllocationChangesComponent, data: { title: 'Allocation Changes' } },
      { path: "future-allocations", component: FutureAllocationsComponent, data: {title: 'Future Allocation'} }
    
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TalentmangRoutingModule { }
