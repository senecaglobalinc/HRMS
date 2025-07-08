import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { OnboardingRoutingModule } from './onboarding-routing.module';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { AddProspectiveAssociateComponent } from './components/add-prospective-associate/add-prospective-associate.component';
import { AssociateEmploymentComponent } from './components/associate-employment/associate-employment.component';
import { AssociateJoiningComponent } from './components/associate-joining/associate-joining.component';
import { AssociateProfessionalComponent } from './components/associate-professional/associate-professional.component';
import { AssociateinformationComponent } from './components/associateinformation/associateinformation.component';
import { AssociateuploadComponent } from './components/associateupload/associateupload.component';
import { EditProspectiveAssociateComponent } from './components/edit-prospective-associate/edit-prospective-associate.component';
import { EducationAssociateComponent } from './components/education-associate/education-associate.component';
import { FamilyAssociateComponent } from './components/family-associate/family-associate.component';
import { PersonalDetailsComponent } from './components/personal-details/personal-details.component';
import { ProjectassociateComponent } from './components/projectassociate/projectassociate.component';
import { ProspectiveAssociateComponent } from './components/prospective-associate/prospective-associate.component';
import { ProspectiveToAssociateComponent } from './components/prospective-to-associate/prospective-to-associate.component';
import { SkillsComponent } from './components/skills/skills.component';
import {OnboardingComponent} from './components/onboarding/onboarding.component';
import {MasterLayoutModule} from '../master-layout/master-layout.module'
import { ValidationsDirectiveModule } from 'src/app/validations.directive';
import { AssociateSkillsComponent } from './components/associate-skills/associate-skills.component';
import { DropDownSuggestionDirectiveModule } from 'src/app/drop-down-suggestion.directive';
import { RequirematchDirectiveModule } from 'src/app/requirematch.directive';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SendEmailComponent } from './components/send-email/send-email.component';
import { WelcomeAssociateEmailComponent } from './components/welcome-associate-email/welcome-associate-email.component';
import { AssociateSkillDialogComponent } from './components/associate-skill-dialog/associate-skill-dialog.component';
import { ChangeRmForNonDeliveryComponent } from './components/change-rm-for-non-delivery/change-rm-for-non-delivery.component';
import { AssignRmToNonDeliveryComponent } from './components/assign-rm-to-non-delivery/assign-rm-to-non-delivery.component';
import {MatSortModule} from '@angular/material/sort';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { AssociateParkingComponent } from './components/associate-parking/associate-parking.component';
import { BiometricAttendanceReportComponent } from './components/biometric-attendance-report/biometric-attendance-report.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import { MatTableExporterModule } from 'mat-table-exporter';
import { BiometricAttendanceDialogComponent } from './components/biometric-attendance-dialog/biometric-attendance-dialog.component';
import { ManagerBiometricAttendanceReportComponent } from './components/manager-biometric-attendance-report/manager-biometric-attendance-report.component';
import { ParkingSlotReportComponent } from './parking-slot-report/parking-slot-report.component';
import { WfhAttendanceComponent } from './components/wfh-attendance/wfh-attendance.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BiometricAttendanceFoundDialogComponent } from './components/biometric-attendance-found-dialog/biometric-attendance-found-dialog.component';
import { RegulariationComponent } from './components/regulariation/regulariation.component';

import { ViewRegularizationAppliedAssociatesComponent } from './components/view-regularization-applied-associates/view-regularization-applied-associates.component';
import { ViewRegularizationAppliedDaysComponent } from './components/view-regularization-applied-days/view-regularization-applied-days.component';
import { ViewBiometricAttendanceComponent } from './components/view-biometric-attendance/view-biometric-attendance.component';
import {NgxMaterialTimepickerModule} from 'ngx-material-timepicker';
import { UploadLeaveDataComponent } from './components/upload-leave-data/upload-leave-data.component';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { AttendanceMusterReportComponent } from './components/attendance-muster-report/attendance-muster-report.component'

FullCalendarModule.registerPlugins([ 
  dayGridPlugin  
]);
@NgModule({
  // tslint:disable-next-line:max-line-length
  declarations: [OnboardingComponent, AddProspectiveAssociateComponent, AssociateEmploymentComponent, AssociateJoiningComponent, AssociateProfessionalComponent, 
    AssociateinformationComponent, AssociateuploadComponent, 
    EditProspectiveAssociateComponent, EducationAssociateComponent, 
    FamilyAssociateComponent, PersonalDetailsComponent, ProjectassociateComponent, 
    ProspectiveAssociateComponent, ProspectiveToAssociateComponent, 
    SkillsComponent, AssociateSkillsComponent,SendEmailComponent, 
    WelcomeAssociateEmailComponent, AssociateSkillDialogComponent, 
    ChangeRmForNonDeliveryComponent, AssignRmToNonDeliveryComponent, 
    AssociateParkingComponent, BiometricAttendanceReportComponent, 
    BiometricAttendanceDialogComponent, ManagerBiometricAttendanceReportComponent,
    ParkingSlotReportComponent, WfhAttendanceComponent, BiometricAttendanceFoundDialogComponent, 
    RegulariationComponent, ViewRegularizationAppliedAssociatesComponent, 
    ViewRegularizationAppliedDaysComponent,ViewBiometricAttendanceComponent, UploadLeaveDataComponent, AttendanceMusterReportComponent],
  
  imports: [
    CommonModule,
    OnboardingRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AllAngularMaterialModule,
    MasterLayoutModule,
    ValidationsDirectiveModule,
    DropDownSuggestionDirectiveModule,
    RequirematchDirectiveModule,
    NgxSpinnerModule,
    MatSortModule,
    NgxMatSelectSearchModule,
    FullCalendarModule,  
    MatTableExporterModule,
    FlexLayoutModule,
    NgxMaterialTimepickerModule,
    MatProgressBarModule
  
 
    
  ],
  exports : [ViewRegularizationAppliedAssociatesComponent],
  providers:[DatePipe],
  entryComponents:[AssociateSkillDialogComponent]
})
export class OnboardingModule { }
