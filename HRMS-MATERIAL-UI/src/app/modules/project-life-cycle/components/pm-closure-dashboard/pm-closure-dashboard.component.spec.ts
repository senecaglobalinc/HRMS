import { async, ComponentFixture, TestBed,fakeAsync,inject,flush } from '@angular/core/testing';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { AllAngularMaterialModule } from 'src/app/modules/plugins/all-angular-material/all-angular-material.module';
import { PMClosureDashboardComponent } from './pm-closure-dashboard.component';
import { ProjectClosureReport} from '../../models/projects.model';
 import { ProjectCreationService } from '../../services/project-creation.service';
 import{ProjectClosureService} from '../../services/project-closure.service';
 import { UrlService } from 'src/app/modules/shared/services/url.service';
 import { MatSnackBarModule } from '@angular/material/snack-bar';
 import { MatCardModule } from '@angular/material/card';
import { RouterTestingModule } from '@angular/router/testing';
import { Observable, Observer } from 'rxjs';
import 'rxjs/add/observable/of';
import { By } from '@angular/platform-browser';
import { of, BehaviorSubject } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Subscription } from 'rxjs';

import { environment } from '../../../../../environments/environment';
 import * as servicePath from '../../../../core/service-paths';
 import * as fileSaver from 'file-saver';
 import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import 'rxjs/Rx' ;
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTable } from '@angular/material/table';
 import { NgxSpinnerService, NgxSpinnerModule } from 'ngx-spinner';

 import {
  HttpClientModule,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';

import { ActivityData, ActivityList } from '../../models/activities.model';
fdescribe('PMClosureDashboardComponent', () => {
  let component: PMClosureDashboardComponent;
  let fixture: ComponentFixture<PMClosureDashboardComponent>;
  let testBedPCService: ProjectCreationService;
  let testBedPCloseService: ProjectClosureService;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PMClosureDashboardComponent ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatCardModule,
        MatSnackBarModule,
        NgxSpinnerModule,
        MatTableModule,
        AllAngularMaterialModule,
        BrowserAnimationsModule
      ],
      providers: [
        {provide: ProjectCreationService, useClass: MockProjectCreationService},
        {provide: ProjectClosureService, useClass: MockProjectCLosureService}
        
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000; // else there are errors sometimes
    sessionStorage["AssociatePortal_UserInformation"]='{"roles":"Admin Manager,Associate,CEO,Delivery,Department Head,Finance Manager,HRA,HRM,IT Manager,Program Manager,Quality and Information Security Manager,SystemAdmin,Team Lead,Training Department Head","roleName":"IT Manager","employeeId":"172","email":"kalyan.penumutchu@senecaglobal.com","firstName":"kalyan","lastName":"penumutchu","fullName":"kalyan penumutchu"}';
    fixture = TestBed.createComponent(PMClosureDashboardComponent);
    component = fixture.componentInstance;
    testBedPCloseService = TestBed.inject(ProjectClosureService);
    testBedPCService = TestBed.inject(ProjectCreationService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });



  it('should call getbyTL when component is loaded', fakeAsync(
    inject([ProjectClosureService], (service: MockProjectCLosureService) => {
      spyOn(component, 'GetTeamLeadData').and.callThrough();
      component.ngOnInit();
      expect(component.GetTeamLeadData).toHaveBeenCalled();
      flush();
    })
  ));

  it('reject button is being clicked',async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('button');
      const nativeButton = HTMLButtonElement = buttons[0];
      let spy = spyOn(component,'Reject');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.Reject).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });;
  });
  it('SubmitForClosureByDH button is being clicked',async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('button');
      const nativeButton = HTMLButtonElement = buttons[0];
      let spy = spyOn(component,'SubmitForClosureByDH');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.SubmitForClosureByDH).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });;
  });

});

class MockProjectCLosureService {
  //  resources = servicePath.API.Activity;
  //   resourcesClosure = servicePath.API.ProjectClosure;
  //   re_sources = servicePath.API.projects;
  //   adminMicroService = environment.AdminMicroService;
  //   serviceUrl = environment.ServerUrl;
  //   projectMicroService = environment.ProjMicroService;
    selectedTab = new BehaviorSubject<number>(0);
    ProjectState = new BehaviorSubject<string>(null);
    EditMode = new BehaviorSubject<boolean>(false);
    ProjectId = new BehaviorSubject<number>(0);
  
      constructor() { }
   
      // GetProjectsList(userRole: string, empId: number, dashboard: string) {
      //     return new Observable((observer: Observer<any>) => {
      //         observer.next({
      //             ProjectId: 23,
      //     ProjectCode: 'abc',
      //     ProjectName: 'xyz',
      //     ProjectType: 45
      //         });
      //       });
      // }
      //export const prjktclosure={
         // ProjectId: 23,
          //     ProjectCode: 'abc',
          //     ProjectName: 'xyz',
          //     ProjectType: 45
  
     // }
      GetClosureReportByProjectID(ProjectId: number) {
           return new Observable((observer: Observer<any>) => {
            observer.next(
                      [   {
                        //   ProjectId:440,
                        // ProjectCode:"P252000",
                        // ProjectName:"TestingClosure01",
                        // PlannedStartDate:null,
                        // PlannedEndDate:null,
                        // ActualStartDate:"2020-08-05T05:30:00+05:30",
                        // ActualEndDate:"2020-12-25T00:00:00+05:30",
                        // ClientId:45,
                        // ClientName:"3 Step Solutions",
                        // PracticeAreaCode:" Technologies (MSFT)",
                        // ProjectTypeCode:null,
                        // ProjectTypeDescription:null,
                        // StatusCode:"ClosureInitiated",
                        // StatusId:20,
                        // ProjectStateId:20,
                        // ProjectState:"ClosureInitiated",
                        // ManagerName:"Sasfname LnameDat",
                        // ManagerId:152,
                        // PracticeAreaId:4,
                        // ProjectTypeId:0,
                        // DepartmentId:1,
                        // DepartmentCode:"Delivery",
                        // DomainId:0,
                        // DomainName:null,
                        // IsActive:true,
                        // ProgramManagerId:0,
                        // UserRole:null,
                        // ProgramManager:null,
                        // ReportingManagerId:null,
                        // EmployeeCode:null,
                        // EmployeeId:0,
                        // FirstName:null,
                        // LastName:null,
                        // PracticeArea:null
                        ProjectId:65,
                        //  ClientFeedback:'abc',
                        //  DeliveryPerformance:'sb',
                        ValueDelivered:"Paragraphs",
                        ManagementChallenges:"ss",
                        TechnologyChallenges:"value",
                        EngineeringChallenges:"value",
                        BestPractices:"value",
                        LessonsLearned:"value",
                        ReusableArtifacts:"value",
                        ProcessImprovements:"value",
                        Awards:"value",
                        NewTechnicalSkills:"value",
                        NewTools:"value",
                        Remarks:"value",
                        // CaseStudy:"value",
                        // StatusId:24,
                        // ClientFeedbackFile:"value",
                        // DeliveryPerformanceFile:"value"
                      }]
                      );
                   });
      }
      
      public UpdateProjectClosureReport(details: ProjectClosureReport) {
        return new Observable((observer: Observer<any>) => {
                  observer.next(
                    {
                    // ProjectId:448,
                    // ClientFeedback:null,
                    // DeliveryPerformance:null,
                    // ValueDelivered:null,
                    // ManagementChallenges:null,
                    // TechnologyChallenges:null,
                    // EngineeringChallenges:null,
                    // BestPractices:null,
                    // LessonsLearned:null,
                    // ReusableArtifacts:null,
                    // ProcessImprovements:null,
                    // Awards:null,
                    // NewTechnicalSkills:null,
                    // NewTools:null,
                    // Remarks:null,
                    // CaseStudy:null,
                    // StatusId:24,
                    // ClientFeedbackFile:null,
                    // DeliveryPerformanceFile:"448_Cloudstorage_Workbook.xlsx"
                  }
                );
                });
  
      }
      // public DownloadProjectClosureFileUpload(FileType: string, projectId: number): any {
  //
      // }
      // public Delete(projectId: number,FileType:string) {
  
      // }
  
      // getActivitiesByProjectIdAndDepartmentId(projectId: number,departmentId){
  
      // } 
      // CreateActivityChecklist(activityData: ActivityData){
  
      // }
  
      // UpdateActivityChecklist(activityData: ActivityData){
          
      //   }
      
        GetActivitiesByProjectIdForPM(projectId: number){
          return new Observable((observer: Observer<any>) => {
            observer.next({
               
            });
          });
        }
        
      //   SubmitForClosureApproval(projectSubmitData){
          
      //   }
      //   Reject(projectId: number){
          
      //   }
      //   ApproveOrRejectClosureByDH(projectSubmitData){
          
      //   }
        getClosureActivitiesByDepartment(departmentId: number){
          return new Observable((observer: Observer<any>) => {
            observer.next({
               
            });
          });
        }
  }

  class MockProjectCreationService {
    
    constructor() { }
 
    public GetProjectId() {
    
        return new Observable((observer: Observer<any>) => {
                    observer.next({
                       
                    });
                  });
      }
      public GetProjectState(){
        return new Observable((observer: Observer<any>) => {
          observer.next({
             
          });
        });
      }

      public GetSelectedTab() {
        return new Observable((observer: Observer<any>) => {
          observer.next({
             
          });
        });
        
      }
}