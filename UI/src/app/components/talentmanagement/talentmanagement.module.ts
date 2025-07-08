
import { NgModule } from '@angular/core';
import { AssociateallocationComponent } from './associateallocation/associateallocation.component';
import { TalentManagementRoutingModule } from './talentmanagement-routing.module';
import { ProjectViewComponent } from './projects/projectview.component';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
import { AddProjectComponent } from './projects/addproject.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AssociateReleaseComponent } from './associate-release/associate-release.component';
import { TalentManagementComponent } from './talentmanagement.component';
import { AllocationChangesComponent } from './allocation-changes/allocation-changes.component';

@NgModule({
  declarations: [  
  AssociateallocationComponent, AssociateReleaseComponent, ProjectViewComponent,TalentManagementComponent, AddProjectComponent, AllocationChangesComponent],
  imports: [AppPrimenNgModule,FormsModule, ReactiveFormsModule, TalentManagementRoutingModule],
})
export class TalentManagementModule { }


