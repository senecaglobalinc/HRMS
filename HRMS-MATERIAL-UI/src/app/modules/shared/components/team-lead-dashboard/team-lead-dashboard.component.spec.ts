import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { AllAngularMaterialModule } from 'src/app/modules/plugins/all-angular-material/all-angular-material.module';
import { ProjectCreationService } from 'src/app/modules/project-life-cycle/services/project-creation.service';
import { DeliveryHeadService } from '../../services/delivery-head.service';

import { TeamLeadDashboardComponent } from './team-lead-dashboard.component';

fdescribe('TeamLeadDashboardComponent', () => {
  let component: TeamLeadDashboardComponent;
  let fixture: ComponentFixture<TeamLeadDashboardComponent>;
  let testBedDHServe: DeliveryHeadService;
  let testBedPCService: ProjectCreationService;
  let testBedNavService: NavService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamLeadDashboardComponent, MatPaginator, MatSort, ],
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
        {provide: DeliveryHeadService, useClass: MockDeliveryHeadService},
        {provide: NavService, useClass: MockNavService}
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000; // else there are errors sometimes
    fixture = TestBed.createComponent(TeamLeadDashboardComponent);
    component = fixture.componentInstance;
    testBedDHServe = TestBed.inject(DeliveryHeadService);
    testBedPCService = TestBed.inject(ProjectCreationService);
    testBedNavService = TestBed.inject(NavService);

    sessionStorage["AssociatePortal_UserInformation"] = '{"roles":"Admin Manager,Associate,CEO,Delivery,Department Head,Finance Manager,HRA,HRM,IT Manager,Program Manager,Quality and Information Security Manager,SystemAdmin,Team Lead,Training Department Head","roleName":"IT Manager","employeeId":"172","email":"kalyan.penumutchu@senecaglobal.com","firstName":"kalyan","lastName":"penumutchu","fullName":"kalyan penumutchu"}';
    testBedDHServe.GetProjectsList(component.roleName,component.EmpId,component.dashboard)
     .subscribe( (res:ProjectsData[])=>{ 
       component.dataSource.data = res;
    });

    fixture.detectChanges();
  });

  it('component should create', () => {
    expect(component).toBeTruthy();
  });

  it('table is created',()=>{
    let tableRows = fixture.nativeElement.querySelectorAll('tr');
    expect(tableRows).toBeTruthy();
  });

  it('table has correct headers', async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let tableRows = fixture.nativeElement.querySelectorAll('tr');
      // Header row
      let headerRow = tableRows[0];
      expect(headerRow.cells[0].innerText.trim()).toBe('Project Name');
      expect(headerRow.cells[1].innerText.trim()).toBe('Project Manager');
      expect(headerRow.cells[2].innerText.trim()).toBe('Technology');
      expect(headerRow.cells[3].innerText.trim()).toBe('Client Name');
      expect(headerRow.cells[4].innerText.trim()).toBe('Start Date');
      expect(headerRow.cells[5].innerText.trim()).toBe('Status');
      expect(headerRow.cells[6].innerText.trim()).toBe('View');
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it('table has 2 rows', async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let tableRows = fixture.nativeElement.querySelectorAll('tr');
      expect(tableRows.length).toBe(3);
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });
  
  it('table has correct data', async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let tableRows = fixture.nativeElement.querySelectorAll('tr');
      // Rows
      let row1 = tableRows[1];
      expect(row1.cells[0].innerText.trim()).toBe('ProjectName1');
      expect(row1.cells[1].innerText.trim()).toBe('ManagerName1');
      expect(row1.cells[2].innerText.trim()).toBe('PracticeAreaCode1');
      expect(row1.cells[3].innerText.trim()).toBe('ClientName1');
      expect(row1.cells[4].innerText.trim()).toBe('12/12/2020');
      expect(row1.cells[5].innerText.trim()).toBe('ProjectState1');
      expect(row1.cells[6].innerText.trim()).toBe('Open');

      let row2 = tableRows[2];
      expect(row2.cells[0].innerText.trim()).toBe('ProjectName2');
      expect(row2.cells[1].innerText.trim()).toBe('ManagerName2');
      expect(row2.cells[2].innerText.trim()).toBe('PracticeAreaCode2');
      expect(row2.cells[3].innerText.trim()).toBe('ClientName2');
      expect(row2.cells[4].innerText.trim()).toBe('12/13/2020');
      expect(row2.cells[5].innerText.trim()).toBe('ProjectState2');
      expect(row2.cells[6].innerText.trim()).toBe('Open');
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it('table should have 2 buttons', async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('td button');
      expect(buttons.length).toBe(2);
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it('open button is being clicked',async(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('td button');
      const nativeButton = HTMLButtonElement = buttons[0];
      let spy = spyOn(component,'ViewProject');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.ViewProject).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });;
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
}
class MockDeliveryHeadService {
  constructor() { }
  GetProjectsList(userRole: string, empId: number, dashboard: string){
    return new Observable((observer: Observer<ProjectsData[]>) => {
      let dataSource: ProjectsData[];
      dataSource = [{
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
        UserRole : "Team Lead",
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
      },{
        ActualStartDate: "12/13/2020",
        ActualEndDate: "12/13/2020",
        PlannedEndDate: "12/13/2020",
        PlannedStartDate: "12/13/2020",
        CustomerId: 14,
        PracticeAreaId: 15,
        ReportingManagerId: 16,
        ManagerId: 17, //ProgramManagerId
        LeadId: 18,
        ReportingManager: {Id: 19, Name: "ReportingManagerName2"},
        ProgramManager: {Id: 20, Name: "ProgramManagerName2"},
        Lead: {Id: 21, Name: "LeadName2"},
        ProgramManagerName: "ProgramManagerName2",
        ReportingManagerName: "ReportingManagerName2",
        LeadName: "LeadName2",
        IsActive: false,
        EmpCode: 22,
        actualStartDate: null,
        actualEndDate: null,
        plannedEndDate: null,
        plannedStartDate: null,
        DepartmentId: 23,
        ClientId : "ClientId2",
        ProjectTypeId : "ProjectTypeId2",
        UserRole : "Team Lead",
        ManagerName: "ManagerName2",
        ProjectTypeCode : "ProjectTypeCode2",
        ProjectTypeDescription : "ProjectTypeDescription2",
        ClientName : "ClientName2",
        ClientShortName: "ClientShortName2",
        PracticeAreaCode: "PracticeAreaCode2",
        DepartmentCode: "DepartmentCode2",
        DomainId : 24,
        ProjectState : "ProjectState2",
        ProjectStateId : 25,
        DomainName : "DomainName2",
        Remarks: "Remarks2",
        ProjectId: 26,
        ProjectCode: "ProjectCode2",
        ProjectName: "ProjectName2",
        ProjectType: 27,
      }];
      observer.next(dataSource);
    });
  }
}

class MockNavService {
  constructor(){}
  public searchBoxData = new BehaviorSubject<any>('');
  currentSearchBoxData = this.searchBoxData.asObservable();
}