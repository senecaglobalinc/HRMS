import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import{TalentmangRoutingModule} from './talentmangment.routing'
import { AssociateallocationComponent } from './Components/associateallocation/associateallocation.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { HomeComponent } from './Components/home/home.component';
import { DialogCBRComponent } from './Components/dialog-cbr/dialog-cbr.component';
import { AssociateReleaseComponent } from './Components/associate-release/associate-release.component';
import { AllocationChangesComponent } from './Components/allocation-changes/allocation-changes.component';
import { RequirematchDirectiveModule } from '../../../../src/app/requirematch.directive';
import { DropDownSuggestionDirectiveModule } from 'src/app/drop-down-suggestion.directive';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FutureAllocationsComponent } from './Components/future-allocations/future-allocations.component';
import { FutureAllocationsGridComponent } from './Components/future-allocations-grid/future-allocations-grid.component';
// import { CBRTableComponent } from './Components/cbrtable/cbrtable.component';

@NgModule({
  declarations: [AssociateallocationComponent, HomeComponent, DialogCBRComponent, AssociateReleaseComponent, AllocationChangesComponent, FutureAllocationsComponent, FutureAllocationsGridComponent,   ],
 
  imports: [
    CommonModule,
    TalentmangRoutingModule,
    AllAngularMaterialModule,
    FormsModule,
    ReactiveFormsModule,
 DropDownSuggestionDirectiveModule,
 NgxSpinnerModule,
    RequirematchDirectiveModule  ]
})
export class TalentManagModule { }
