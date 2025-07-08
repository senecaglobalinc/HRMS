import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { AllAngularMaterialModule } from 'src/app/modules/plugins/all-angular-material/all-angular-material.module';
import { UrlService } from 'src/app/modules/shared/services/url.service';
import { ProjectClosureService } from '../../services/project-closure.service';
import { ProjectCreationService } from '../../services/project-creation.service';

import { ProjectClosureReportComponent } from './project-closure-report.component';

describe('ProjectClosureReportComponent', () => {
  let component: ProjectClosureReportComponent;
  let fixture: ComponentFixture<ProjectClosureReportComponent>;
  let testBedCrService: ProjectCreationService;
  let testBedClService: ProjectClosureService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,ReactiveFormsModule, RouterTestingModule,MatCardModule,
        MatSnackBarModule,
        MatTableModule,
        AllAngularMaterialModule,
        BrowserAnimationsModule],
        declarations: [ProjectClosureReportComponent],
        providers: [
        {provide: ProjectCreationService, useClass: MockProjectCreationService},
        {provide: ProjectClosureService, useClass: MockProjectClosureService},
        {provide: UrlService, useClass: MockUrlService}
        ]
      })
    
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectClosureReportComponent);
    component = fixture.componentInstance;

    testBedCrService = TestBed.inject(ProjectCreationService);
    testBedClService = TestBed.inject(ProjectClosureService);
    //here
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
class MockProjectCreationService {
  constructor() { }
  GetProjectsList(userRole: string, empId: number, dashboard: string){
    return new Observable((observer: Observer<any>) => {
      observer.next({testdata: "test data"});
    });
  }
  SetProjectId(projectId: number) {
    return null;
  }
  GetProjectId() {
    return new Observable((observer: Observer<any>) => {
    observer.next({});
    });
  }
}
class MockProjectClosureService {
  constructor() { }
  GetProjectsList(userRole: string, empId: number, dashboard: string){
    return new Observable((observer: Observer<any>) => {
      observer.next({testdata: "test data"});
    });
  }
  SetProjectId(projectId: number) {
    return null;
  }
  GetClosureReportByProjectID(ProjectId: number) {
    return new Observable((observer: Observer<any>) => {
      observer.next({ProjectId:440,
        ProjectCode:"P252000",
        ProjectName:"TestingClosure01",
        PlannedStartDate:null,
        PlannedEndDate:null,
        ActualStartDate:"2020-08-05T05:30:00+05:30",
        ActualEndDate:"2020-12-25T00:00:00+05:30",
        ClientId:45,
        ClientName:"3 Step Solutions",
        PracticeAreaCode:" Technologies (MSFT)",
        ProjectTypeCode:null,
        ProjectTypeDescription:null,
        StatusCode:"ClosureInitiated",
        StatusId:20,
        ProjectStateId:20,
        ProjectState:"ClosureInitiated",
        ManagerName:"Sasfname LnameDat",
        ManagerId:152,
        PracticeAreaId:4,
        ProjectTypeId:0,
        DepartmentId:1,
        DepartmentCode:"Delivery",
        DomainId:0,
        DomainName:null,
        IsActive:true,
        ProgramManagerId:0,
        UserRole:null,
        ProgramManager:null,
        ReportingManagerId:null,
        EmployeeCode:null,
        EmployeeId:0,
        FirstName:null,
        LastName:null,
        PracticeArea:null});
      });
    }
}

class MockUrlService {
  constructor() {}
  private previousUrl: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public previousUrl$: Observable<string> = this.previousUrl.asObservable();
}
