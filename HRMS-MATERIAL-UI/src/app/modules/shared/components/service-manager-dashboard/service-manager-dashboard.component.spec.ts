import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { ProjectCreationService } from 'src/app/modules/project-life-cycle/services/project-creation.service';
import { DeliveryHeadService } from '../../services/delivery-head.service';
import { MatCardModule } from '@angular/material/card';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ServiceManagerDashboardComponent } from './service-manager-dashboard.component';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { GenericType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { AllAngularMaterialModule } from '../../../plugins/all-angular-material/all-angular-material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { By } from "@angular/platform-browser";
import {Location} from "@angular/common";
import { UrlService } from '../../services/url.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

fdescribe('ServiceManagerDashboardComponent', () => {
  let component: ServiceManagerDashboardComponent;
  let fixture: ComponentFixture<ServiceManagerDashboardComponent>;
  let testBedDHServe: DeliveryHeadService;
  let testBedPCService: ProjectCreationService;
  let testBedNavService: NavService;

  beforeEach(async(() => {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000; // else there are errors sometimes
    TestBed.configureTestingModule({
      declarations: [ ServiceManagerDashboardComponent, MatPaginator, MatSort, ],
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
    
    fixture = TestBed.createComponent(ServiceManagerDashboardComponent);
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

  it('Service injected via inject(...) and tested.get(...) should be the same instance',inject(
    [ProjectCreationService],
    (injectService: ProjectCreationService)=>{
      expect(injectService).toBe(testBedPCService);
    }
  ));

  it('component should create', () => {
    fixture.detectChanges();
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
      // Header rows
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
      expect(row1.cells[4].innerText.trim()).toBe('ActualStartDate1');
      expect(row1.cells[5].innerText.trim()).toBe('ProjectState1');
      expect(row1.cells[6].innerText.trim()).toBe('Open');

      let row2 = tableRows[2];
      expect(row2.cells[0].innerText.trim()).toBe('ProjectName2');
      expect(row2.cells[1].innerText.trim()).toBe('ManagerName2');
      expect(row2.cells[2].innerText.trim()).toBe('PracticeAreaCode2');
      expect(row2.cells[3].innerText.trim()).toBe('ClientName2');
      expect(row2.cells[4].innerText.trim()).toBe('ActualStartDate2');
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
      let spy = spyOn(component,'ViewChecklist');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.ViewChecklist).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

});

class MockProjectCreationService {
  constructor() { }
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
        ActualStartDate: "ActualStartDate1",
        ActualEndDate: "ActualEndDate1",
        PlannedEndDate: "PlannedEndDate1",
        PlannedStartDate: "PlannedStartDate1",
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
      },{
        ActualStartDate: "ActualStartDate2",
        ActualEndDate: "ActualEndDate2",
        PlannedEndDate: "PlannedEndDate2",
        PlannedStartDate: "PlannedStartDate2",
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
        UserRole : "IT Manager",
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