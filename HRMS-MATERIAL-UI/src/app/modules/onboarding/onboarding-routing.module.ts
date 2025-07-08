import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProspectiveAssociateComponent } from './components/prospective-associate/prospective-associate.component';
import { AssociateinformationComponent } from '../onboarding/components/associateinformation/associateinformation.component';
import { AssociateJoiningComponent } from '../onboarding/components/associate-joining/associate-joining.component';
import { ProspectiveToAssociateComponent } from '../onboarding/components/prospective-to-associate/prospective-to-associate.component';
import { ProjectassociateComponent } from '../onboarding/components/projectassociate/projectassociate.component';
import { AssociateuploadComponent } from '../onboarding/components/associateupload/associateupload.component';
import { AddProspectiveAssociateComponent } from '../onboarding/components/add-prospective-associate/add-prospective-associate.component';
import { PersonalDetailsComponent } from '../onboarding/components/personal-details/personal-details.component';
import { FamilyAssociateComponent } from '../onboarding/components/family-associate/family-associate.component';
// tslint:disable-next-line:max-line-length
import { EditProspectiveAssociateComponent } from '../onboarding/components/edit-prospective-associate/edit-prospective-associate.component';
import { SkillsComponent } from '../onboarding/components/skills/skills.component';
import { OnboardingComponent } from '../onboarding/components/onboarding/onboarding.component';
import { AssociateSkillsComponent } from './components/associate-skills/associate-skills.component';
import { WelcomeAssociateEmailComponent } from './components/welcome-associate-email/welcome-associate-email.component';
import { ChangeRmForNonDeliveryComponent } from './components/change-rm-for-non-delivery/change-rm-for-non-delivery.component';
import { AssociateParkingComponent } from './components/associate-parking/associate-parking.component';
import { BiometricAttendanceReportComponent } from './components/biometric-attendance-report/biometric-attendance-report.component';
import { ManagerAttendanceReportComponent } from '../reports/components/manager-attendance-report/manager-attendance-report.component';
import { ManagerBiometricAttendanceReportComponent } from './components/manager-biometric-attendance-report/manager-biometric-attendance-report.component';
import { ParkingSlotReportComponent } from './parking-slot-report/parking-slot-report.component';
import { WfhAttendanceComponent } from './components/wfh-attendance/wfh-attendance.component';
import { RegulariationComponent } from './components/regulariation/regulariation.component';
import { ViewRegularizationAppliedDaysComponent } from './components/view-regularization-applied-days/view-regularization-applied-days.component';
import { ViewBiometricAttendanceComponent } from './components/view-biometric-attendance/view-biometric-attendance.component';
import { UploadLeaveDataComponent } from './components/upload-leave-data/upload-leave-data.component';
import { AttendanceMusterReportComponent } from './components/attendance-muster-report/attendance-muster-report.component';

const routes: Routes = [
  {
      path: '',
      component: OnboardingComponent,
      children: [
        {path: 'prospectivetoassociate/:type/:id/:subtype', component: ProspectiveToAssociateComponent, data: { title: 'Profile Update' }},
        {path: 'prospectiveassociate', component: ProspectiveAssociateComponent, data: { title: 'Prospective Associates' }},
        {path: 'addprospectiveassociate', component: AddProspectiveAssociateComponent, data: { title: 'Prospective Associate' }},
        {path: 'associateinformation', component: AssociateinformationComponent, data: { title: 'Associate Information' }},
        {path: 'associatejoining', component: AssociateJoiningComponent, data: { title: 'Associate Joining' }},
        {path: 'projectassociate', component: ProjectassociateComponent},
        {path: 'assosiateupload', component: AssociateuploadComponent},
        { path : 'personaldetails/:type/:id', component : PersonalDetailsComponent},
        {path: 'associatejoining', component: AssociateJoiningComponent},
        {path: 'family-associate/:id', component: FamilyAssociateComponent},
         {path: 'editprospectiveassociate', component: EditProspectiveAssociateComponent, data: { title: 'Edit Prospective Associate' }},
        {path: 'skills/:id', component: SkillsComponent},
        {path: 'skill', component: AssociateSkillsComponent, data:{title: 'Skills'}},
        // {path:'edit-prospective-associate',component:EditProspectiveAssociateComponent}
         {path: 'editprospectiveassociate/:id', component: EditProspectiveAssociateComponent, data: { title: 'Edit Prospective Associate' }},
         {path: 'sendwelcomeemail', component: WelcomeAssociateEmailComponent, data: { title: 'Send Email' }},
         {path: 'changeRM', component: ChangeRmForNonDeliveryComponent, data: { title: 'Assign RM to Service Departments' }},
         {path: 'bookparkingslot', component: AssociateParkingComponent, data: { title: 'Parking Slots'}},
         {path: 'biometricattendance', component: BiometricAttendanceReportComponent, data: { title: 'Attendance Report'}},
         {path: 'biometricattendancereport',component:ManagerBiometricAttendanceReportComponent,data: { title: 'Attendance Report'}},
         {path: 'parkingslotreport', component: ParkingSlotReportComponent, data: { title: 'Parking Slots Report'}},
         {path: 'workfromhomeSignIn', component: WfhAttendanceComponent, data: { title: 'Work From Home'}},
         {path: 'regularization', component: RegulariationComponent, data: { title: 'Apply Regularization'}},
         {path: 'viewDays/:id/:associateName', component :ViewRegularizationAppliedDaysComponent, data : {title : 'View Regularizations'}},
         {path: 'viewAttendance', component: ViewBiometricAttendanceComponent, data: { title: 'Attendance'}},
         {path: 'associateleave', component : UploadLeaveDataComponent, data : {title:'Upload Leave  Data'}},
         {path: 'attendancemusterreport', component : AttendanceMusterReportComponent, data : {title:'Attendance Muster'}}

      ]}

];

@NgModule({
  imports:
   [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class OnboardingRoutingModule { }
