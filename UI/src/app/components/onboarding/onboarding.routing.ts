import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToolbarComponent } from '../shared/toolbar/toolbar.component';
import { ProspectiveAssociateComponent } from './prospective-associate/prospective-associate.component';
import { AssociateinformationComponent } from './associateinformation/associateinformation.component';
import { AssociateJoiningComponent } from './associate-joining/associate-joining.component';
import { ProspectiveToAssociateComponent } from './prospective-to-associate/prospective-to-associate.component';
import { ProjectassociateComponent } from './projectassociate/projectassociate.component';
import { AssosiateuploadComponent } from './assosiateupload/assosiateupload.component';
import { AddProspectiveAssosiateComponent } from './add-prospective-assosiate/add-prospective-assosiate.component';
import { PersonalDetailsComponent } from './personal-details/personal-details.component';
import { FamilyAssociateComponent } from './family-associate/family-associate.component';
 import { EditProspectiveAssociateComponent } from './edit-prospective-associate/edit-prospective-associate.component';
import { SkillsComponent } from './skills/skills.component';
import { OnboardingComponent } from './onboarding.component';
// import { EditProspectiveAssociateComponent } from './edit-prospective-associate/edit-prospective-associate.component';



const routes: Routes = [
  {
      path:"",
      component:OnboardingComponent,
      children:[
        {path:'prospectivetoassociate/:type/:id/:subtype', component: ProspectiveToAssociateComponent},
        {path:'prospectiveassociate',component:ProspectiveAssociateComponent},
        {path:'addprospectiveassociate',component:AddProspectiveAssosiateComponent},
        {path:'associateinformation',component:AssociateinformationComponent},
        {path:'associatejoining',component:AssociateJoiningComponent},
        {path:'projectassociate',component:ProjectassociateComponent},
        {path:'assosiateupload',component:AssosiateuploadComponent},
        { path :'personaldetails/:type/:id', component : PersonalDetailsComponent},
        {path:'associatejoining',component:AssociateJoiningComponent},
        {path:'family-associate/:id',component:FamilyAssociateComponent},
         {path:'editprospectiveassociate',component:EditProspectiveAssociateComponent},
        {path:'skills/:id',component:SkillsComponent},
        // {path:'edit-prospective-associate',component:EditProspectiveAssociateComponent}
         {path:'editprospectiveassociate/:id',component:EditProspectiveAssociateComponent}
      ]

  }
 
];

@NgModule({
  imports:
   [RouterModule.forChild(routes)]
})
export class OnBoardingRoutingModule { 


}
