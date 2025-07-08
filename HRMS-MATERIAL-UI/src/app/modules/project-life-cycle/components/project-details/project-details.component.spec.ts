import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { AllAngularMaterialModule } from 'src/app/modules/plugins/all-angular-material/all-angular-material.module';
import { UrlService } from 'src/app/modules/shared/services/url.service';
import { ProjectCreationService } from '../../services/project-creation.service';
import { By } from "@angular/platform-browser";
import { ProjectDetailsComponent } from './project-details.component';

fdescribe('ProjectDetailsComponent', () => {
  let component: ProjectDetailsComponent;
  let fixture: ComponentFixture<ProjectDetailsComponent>;
  let testBedPCService: ProjectCreationService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDetailsComponent ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatCardModule,
        MatSnackBarModule,
        AllAngularMaterialModule,
        BrowserAnimationsModule
      ],
      providers: [
        {provide: ProjectCreationService, useClass: MockProjectCreationService},
        {provide: UrlService, useClass: MockUrlService}
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDetailsComponent);
    component = fixture.componentInstance;
    testBedPCService = TestBed.inject(ProjectCreationService);

    sessionStorage["AssociatePortal_UserInformation"] = '{"roles":"Admin Manager,Associate,CEO,Delivery,Department Head,Finance Manager,HRA,HRM,IT Manager,Program Manager,Quality and Information Security Manager,SystemAdmin,Team Lead,Training Department Head","roleName":"IT Manager","employeeId":"172","email":"kalyan.penumutchu@senecaglobal.com","firstName":"kalyan","lastName":"penumutchu","fullName":"kalyan penumutchu"}';
    testBedPCService.GetProjectDetailsbyID(component.projectId)
    .subscribe( (res:ProjectsData)=>{ 
      component.projectData = res;
   });
   component.pageload = true;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('The headers bold content is correct', ()=>{
    fixture.detectChanges();
    let boldItems = fixture.debugElement.queryAll(By.css('.font-weight-bold'));
    expect(boldItems.length).toBe(12);
    expect(boldItems[0].nativeElement.innerText).toBe('Project Details');
    expect(boldItems[1].nativeElement.innerText).toBe('Project Code');
    expect(boldItems[2].nativeElement.innerText).toBe('Project Name');
    expect(boldItems[3].nativeElement.innerText).toBe('Project Type');
    expect(boldItems[4].nativeElement.innerText).toBe('Technology Area');
    expect(boldItems[5].nativeElement.innerText).toBe('Client');
    expect(boldItems[6].nativeElement.innerText).toBe('Domain');
    expect(boldItems[7].nativeElement.innerText).toBe('Program Manager');
    expect(boldItems[8].nativeElement.innerText).toBe('Department');
    expect(boldItems[9].nativeElement.innerText).toBe('Project State');
    expect(boldItems[10].nativeElement.innerText).toBe('Start Date');
    expect(boldItems[11].nativeElement.innerText).toBe('End Date');
  });

  it('Content from get service is binded correctly', async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let boldItems = fixture.debugElement.queryAll(By.css('.col-md-7'));
      expect(boldItems.length).toBe(11);
      expect(boldItems[0].nativeElement.innerText).toBe(': ProjectCode1');
      expect(boldItems[1].nativeElement.innerText).toBe(': ProjectName1');
      expect(boldItems[2].nativeElement.innerText).toBe(': ProjectTypeDescription1');
      expect(boldItems[3].nativeElement.innerText).toBe(': PracticeAreaCode1');
      expect(boldItems[4].nativeElement.innerText).toBe(': ClientName1');
      expect(boldItems[5].nativeElement.innerText).toBe(': DomainName1');
      expect(boldItems[6].nativeElement.innerText).toBe(': ManagerName1');
      expect(boldItems[7].nativeElement.innerText).toBe(': DepartmentCode1');
      expect(boldItems[8].nativeElement.innerText).toBe(': ProjectState1');
      expect(boldItems[9].nativeElement.innerText).toBe(': Dec 12, 2020');
      expect(boldItems[10].nativeElement.innerText).toBe(': Dec 12, 2020');
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it('Back button is pressed',()=>{
    let nativeButton: HTMLButtonElement = fixture.nativeElement.querySelector('button');
    let spy = spyOn(component,'onBack');
    nativeButton.click();
    fixture.detectChanges();
    expect(component.onBack).toHaveBeenCalled();
  });

});

class MockProjectCreationService {
  constructor() { }
  GetProjectDetailsbyID(ProjectId: number){
    return new Observable((observer: Observer<ProjectsData>) => {
      let dataSource: ProjectsData;
      dataSource = {
        ActualStartDate: "12/12/2020",
        ActualEndDate: "12/12/2020",
        PlannedEndDate: "12/12/2020",
        PlannedStartDate: "12/12/2020",
        CustomerId: 1,
        PracticeAreaId: 2,
        ReportingManagerId: 3,
        ManagerId: 4, //ProgramManagerId
        LeadId: 5,
        ReportingManager: {Id: 5, Name: "ReportingManagerName1"},
        ProgramManager: {Id: 6, Name: "ProgramManagerName1"},
        Lead: {Id: 7, Name: "LeadName1"},
        ProgramManagerName: "ProgramManagerName1",
        ReportingManagerName: "ReportingManagerName1",
        LeadName: "LeadName1",
        IsActive: false,
        EmpCode: 8,
        actualStartDate: null,
        actualEndDate: null,
        plannedEndDate: null,
        plannedStartDate: null,
        DepartmentId: 9,
        ClientId : "ClientId1",
        ProjectTypeId : "ProjectTypeId1",
        UserRole : "IT Manager",
        ManagerName: "ManagerName1",
        ProjectTypeCode : "ProjectTypeCode1",
        ProjectTypeDescription : "ProjectTypeDescription1",
        ClientName : "ClientName1",
        ClientShortName: "ClientShortName1",
        PracticeAreaCode: "PracticeAreaCode1",
        DepartmentCode: "DepartmentCode1",
        DomainId : 10,
        ProjectState : "ProjectState1",
        ProjectStateId : 11,
        DomainName : "DomainName1",
        Remarks: "Remarks",
        ProjectId: 12,
        ProjectCode: "ProjectCode1",
        ProjectName: "ProjectName1",
        ProjectType: 13,
      };
      observer.next(dataSource);
    });
  }
  GetProjectId(){
    return new Observable();
  }
  GetProjectState(){
    return new Observable();
  }
}

class MockUrlService {
  constructor() {}
  private previousUrl: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public previousUrl$: Observable<string> = this.previousUrl.asObservable();
}