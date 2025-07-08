import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule  }   from '@angular/forms';
import { OnBoardingRoutingModule } from './onboarding.routing';
import { FamilyAssociateComponent } from './family-associate/family-associate.component';
import { ProspectiveAssociateComponent } from './prospective-associate/prospective-associate.component';
import { AssociateinformationComponent } from './associateinformation/associateinformation.component';
import { AssociateJoiningComponent } from './associate-joining/associate-joining.component';
import { ProspectiveToAssociateComponent } from './prospective-to-associate/prospective-to-associate.component';
import { EducationComponent } from './education-associate/education-associate.component';
import { AssociateEmploymentComponent } from './associate-employment/associate-employment.component';
import { PersonalDetailsComponent } from './personal-details/personal-details.component';
import { AddProspectiveAssosiateComponent } from './add-prospective-assosiate/add-prospective-assosiate.component';
import { AssociateProfessionalComponent } from './associate-professional/associate-professional.component';
import { AssosiateuploadComponent } from './assosiateupload/assosiateupload.component';
import { ProjectassociateComponent } from './projectassociate/projectassociate.component';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
 import { EditProspectiveAssociateComponent } from './edit-prospective-associate/edit-prospective-associate.component';
import { SkillsComponent } from './skills/skills.component';
import { OnboardingComponent } from './onboarding.component';


@NgModule({
  imports: [
    CommonModule,OnBoardingRoutingModule,AppPrimenNgModule,
    FormsModule, ReactiveFormsModule
   
  ],
  declarations: [ProspectiveAssociateComponent, AssociateinformationComponent, OnboardingComponent, AssociateJoiningComponent,
    EducationComponent, ProspectiveToAssociateComponent, AssociateEmploymentComponent,PersonalDetailsComponent,
    AssosiateuploadComponent,FamilyAssociateComponent,EditProspectiveAssociateComponent,
     AddProspectiveAssosiateComponent, AssociateProfessionalComponent,SkillsComponent, ProjectassociateComponent],
  
  
  exports: []
})
export class OnBoardingModule { }
