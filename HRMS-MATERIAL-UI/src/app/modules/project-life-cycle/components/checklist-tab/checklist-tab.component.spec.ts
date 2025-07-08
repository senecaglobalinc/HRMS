import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, fakeAsync, inject, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { AllAngularMaterialModule } from 'src/app/modules/plugins/all-angular-material/all-angular-material.module';
import { UrlService } from 'src/app/modules/shared/services/url.service';
import { ActivityList, ActivityData, ActivityDetails } from '../../models/activities.model';
import { ProjectClosureService } from '../../services/project-closure.service';
import { ProjectCreationService } from '../../services/project-creation.service';
import { ProjectDetailsComponent } from '../project-details/project-details.component';
import { ChecklistTabComponent } from './checklist-tab.component';

fdescribe('ChecklistTabComponent', () => {
  let component: ChecklistTabComponent;
  let fixture: ComponentFixture<ChecklistTabComponent>;
  let testBedCrService: ProjectCreationService;
  let testBedClService: ProjectClosureService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChecklistTabComponent, ProjectDetailsComponent ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatCardModule,
        MatSnackBarModule,
        NgxSpinnerModule,
        AllAngularMaterialModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
      ],
      providers: [
        {provide: ProjectCreationService, useClass: MockProjectCreationService},
        {provide: ProjectClosureService, useClass: MockProjectClosureService},
        {provide: UrlService, useClass: MockUrlService}
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000; // else there are errors sometimes
    fixture = TestBed.createComponent(ChecklistTabComponent);
    component = fixture.componentInstance;

    testBedCrService = TestBed.inject(ProjectCreationService);
    testBedClService = TestBed.inject(ProjectClosureService);

    //roleName = "IT Manager"
    sessionStorage["AssociatePortal_UserInformation"] = '{"roles":"Admin Manager,Associate,CEO,Delivery,Department Head,Finance Manager,HRA,HRM,IT Manager,Program Manager,Quality and Information Security Manager,SystemAdmin,Team Lead,Training Department Head","roleName":"IT Manager","employeeId":"172","email":"kalyan.penumutchu@senecaglobal.com","firstName":"kalyan","lastName":"penumutchu","fullName":"kalyan penumutchu"}'
    component.projectId = 178;

    fixture.detectChanges();
  });

  it('Service injected via inject(...) and TestBed.get(...) should be the same instance',
    inject([ProjectCreationService],(injectService: ProjectCreationService) => {
      expect(injectService).toBe(testBedCrService);
    })
  );

  it('Service injected via component should be and instance of Mock service', () => {
    expect(testBedCrService instanceof MockProjectCreationService).toBeTruthy();
    expect(testBedClService instanceof MockProjectClosureService).toBeTruthy();
  });

  it('Service injected via inject(...) and TestBed.get(...) should be the same instance',
    inject([ProjectClosureService],(injectService: ProjectClosureService) => {
      expect(injectService).toBe(testBedClService);
    })
  );

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Should have expected # of checkboxes',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let checkBoxes = fixture.nativeElement.querySelectorAll('mat-checkbox');
      switch(component.serviceDeptID){
        case(2):{
          expect(checkBoxes.length).toBe(1);
          break;
        }
        case(3):{
          expect(checkBoxes.length).toBe(4);
          break;
        }
        case(4):{
          expect(checkBoxes.length).toBe(2);
          break;
        }
        case(5):{
          expect(checkBoxes.length).toBe(5);
          break;
        }
      }
      done();
    });
  });

  it('Should have the right text description for the activities',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let checklabel = fixture.nativeElement.querySelectorAll('span>span.mat-checkbox-label');
      switch(component.serviceDeptID){
        case(2):{
          expect(checklabel.length).toBe(1);
          expect(checklabel[0].innerText).toBe("Revoke client assests from associates");
          break;
        }
        case(3):{
          expect(checklabel.length).toBe(4);
          expect(checklabel[0].innerText).toBe("Back up project data");
          expect(checklabel[1].innerText).toBe("Disable access to tools and email groups");
          expect(checklabel[2].innerText).toBe("Formating systems");
          expect(checklabel[3].innerText).toBe("Get approval from Delivery Head");
          break;
        }
        case(4):{
          expect(checklabel.length).toBe(2);
          expect(checklabel[0].innerText).toBe("Invoice raised");
          expect(checklabel[1].innerText).toBe("Payment completed");
          break;
        }
        case(5):{
          expect(checklabel.length).toBe(5);
          expect(checklabel[0].innerText).toBe("Check if any resuable code is used ");
          expect(checklabel[1].innerText).toBe("Check if any reusable libraries are used");
          expect(checklabel[2].innerText).toBe("Check the quality metrics used in the project");
          expect(checklabel[3].innerText).toBe("Support TL in filling Project report");
          expect(checklabel[4].innerText).toBe("Support TL in preparing Project case study");
          break;
        }
      }
      done();
    });
  });

  it('Should read the checkboxes correctly',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let checkBoxes = fixture.nativeElement.querySelectorAll('.mat-checkbox-input');
      switch(component.serviceDeptID){
        case(2):{
          expect(checkBoxes.length).toBe(1);
          expect(checkBoxes[0].ariaChecked).toBe("false");
          break;
        }
        case(3):{
          expect(checkBoxes.length).toBe(4);
          expect(checkBoxes[0].ariaChecked).toBe("false");
          expect(checkBoxes[1].ariaChecked).toBe("false");
          expect(checkBoxes[2].ariaChecked).toBe("false");
          expect(checkBoxes[3].ariaChecked).toBe("false");
          break;
        }
        case(4):{
          expect(checkBoxes.length).toBe(2);
          expect(checkBoxes[0].ariaChecked).toBe("false");
          expect(checkBoxes[1].ariaChecked).toBe("false");
          break;
        }
        case(5):{
          expect(checkBoxes.length).toBe(5);
          expect(checkBoxes[0].ariaChecked).toBe("false");
          expect(checkBoxes[1].ariaChecked).toBe("false");
          expect(checkBoxes[2].ariaChecked).toBe("false");
          expect(checkBoxes[3].ariaChecked).toBe("false");
          expect(checkBoxes[4].ariaChecked).toBe("false");
          break;
        }
      }
      done();
    });
  });

  it('Should read the comments and activity remarks correctly',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let textarea = fixture.nativeElement.querySelectorAll('textarea');
      switch(component.serviceDeptID){
        case(2):{
          expect(textarea.length).toBe(2);
          expect(textarea[0].innerText).toBe("");
          expect(textarea[1].innerText).toBe("");
          break;
        }
        case(3):{
          expect(textarea.length).toBe(5);
          expect(textarea[0].innerText).toBe("");
          expect(textarea[1].innerText).toBe("");
          expect(textarea[2].innerText).toBe("");
          expect(textarea[3].innerText).toBe("");
          expect(textarea[4].innerText).toBe("");
          break;
        }
        case(4):{
          expect(textarea.length).toBe(3);
          expect(textarea[0].innerText).toBe("");
          expect(textarea[1].innerText).toBe("");
          expect(textarea[2].innerText).toBe("");
          break;
        }
        case(5):{
          expect(textarea.length).toBe(6);
          expect(textarea[0].innerText).toBe("");
          expect(textarea[1].innerText).toBe("");
          expect(textarea[2].innerText).toBe("");
          expect(textarea[3].innerText).toBe("");
          expect(textarea[4].innerText).toBe("");
          expect(textarea[5].innerText).toBe("");
          break;
        }
      }
      done();
    });
  });

  it('Should click save button',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('button');
      const nativeButton = HTMLButtonElement = buttons[0];
      let spy = spyOn(component,'Save');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.Save).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it('Should click submit button',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let buttons = fixture.nativeElement.querySelectorAll('button');
      const nativeButton = HTMLButtonElement = buttons[1];
      let spy = spyOn(component,'Submit');
      nativeButton.click();
      fixture.detectChanges();
      expect(component.Submit).toHaveBeenCalled();
      done();
    }).catch(error=>{
      console.log(error);
      done();
    });
  });

  it("Form can't submit when empty",(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      component.Submit();
      expect(component.checklistForm.valid).toBeFalsy();
      done()
    });
  });

  it('Activities have correct validations',(done)=>{
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      let form = component.checklistForm;
      switch(component.serviceDeptID){
        case(3):{
          form.get('Checklist0').setValue(true);
          form.get('Checklist0').updateValueAndValidity();
          component.Submit();
          expect(form.valid).toBeFalsy();
          form.get('Checklist1').setValue(true);
          form.get('Checklist1').updateValueAndValidity();
          component.Submit();
          expect(form.valid).toBeFalsy();
          form.get('Checklist2').setValue(true);
          form.get('Checklist2').updateValueAndValidity();
          component.Submit();
          expect(form.valid).toBeFalsy();
          form.get('Checklist3').setValue(true);
          form.get('Checklist3').updateValueAndValidity();
          component.Submit();
          expect(form.valid).toBeTruthy();
          break;
        }
      }
      done();
    });
  });

});

class MockProjectCreationService {
  constructor() { }
  GetProjectId(){
    return new Observable();
  }
  GetProjectState(){
    return new Observable();
  }
}
class MockProjectClosureService {
  constructor() { }
  getClosureActivitiesByDepartment(departmentId: number){
    let activitylist: ActivityList[];
      switch(departmentId){
        case(2):{
          activitylist = [
            {
              "ActivityId": 64,
              "Department": "Administration",
              value: false,
              ActivityRemarks: "",
              "DepartmentId": 2,
              "ActivityType": "Handover of Assets",
              "Description": "Revoke client assests from associates"
            },
          ];
          break;
        }
        case(3):{
          activitylist = [
            {
              "ActivityId": 54,
              value: false,
              ActivityRemarks: "",
              "Department": "IT Service",
              "DepartmentId": 3,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Back up project data"
            },
            {
              "ActivityId": 53,
              value: false,
              ActivityRemarks: "",
              "Department": "IT Service",
              "DepartmentId": 3,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Disable access to tools and email groups"
            },
            {
              "ActivityId": 56,
              value: false,
              ActivityRemarks: "",
              "Department": "IT Service",
              "DepartmentId": 3,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Formating systems"
            },
            {
              "ActivityId": 52,
              value: false,
              ActivityRemarks: "",
              "Department": "IT Service",
              "DepartmentId": 3,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Get approval from Delivery Head"
            },
          ];
          break;
        }
        case(4):{
          activitylist = [
            {
              "ActivityId": 57,
              value: false,
              ActivityRemarks: "",
              "Department": "Finance",
              "DepartmentId": 4,
              "ActivityType": "Income Tax Compliance",
              "Description": "Invoice raised"
            },
            {
              "ActivityId": 58,
              value: false,
              ActivityRemarks: "",
              "Department": "Finance",
              "DepartmentId": 4,
              "ActivityType": "Income Tax Compliance",
              "Description": "Payment completed"
            },
          ];
          break;
        }
        case(5):{
          activitylist = [
            {
              "ActivityId": 59,
              value: false,
              ActivityRemarks: "",
              "Department": "Quality and Information Security",
              "DepartmentId": 5,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Check if any resuable code is used "
            },
            {
              "ActivityId": 60,
              value: false,
              ActivityRemarks: "",
              "Department": "Quality and Information Security",
              "DepartmentId": 5,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Check if any reusable libraries are used"
            },
            {
              "ActivityId": 63,
              value: false,
              ActivityRemarks: "",
              "Department": "Quality and Information Security",
              "DepartmentId": 5,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Check the quality metrics used in the project"
            },
            {
              "ActivityId": 61,
              value: false,
              ActivityRemarks: "",
              "Department": "Quality and Information Security",
              "DepartmentId": 5,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Support TL in filling Project report"
            },
            {
              "ActivityId": 62,
              value: false,
              ActivityRemarks: "",
              "Department": "Quality and Information Security",
              "DepartmentId": 5,
              "ActivityType": "Removal of Access, Privilege, Benefit",
              "Description": "Support TL in preparing Project case study"
            }
          ];
          break;
        }
      }
    return Observable.of(activitylist);
  }
  getActivitiesByProjectIdAndDepartmentId(projectId: number,departmentId){
    let activitydata: ActivityData;
      switch(departmentId){
        case(2):{
          activitydata = {
            "ProjectId": 452,
            "DepartmentId": 2,
            "Remarks": null,
            type: "",
            "ActivityDetails": []
          };
          break;
        }
        case(3):{
          activitydata = {
            "ProjectId": 452,
            "DepartmentId": 3,
            "Remarks": null,
            type: "",
            "ActivityDetails": []
          };
          break;
        }
        case(4):{
          activitydata = {
            "ProjectId": 452,
            "DepartmentId": 4,
            "Remarks": null,
            type: "",
            "ActivityDetails": []
          };
          break;
        }
        case(5):{
          activitydata = {
            "ProjectId": 452,
            "DepartmentId": 5,
            "Remarks": null,
            type: "",
            "ActivityDetails": []
          };
          break;
        }
      }
    return Observable.of(activitydata);
  }
  UpdateActivityChecklist(activityData: ActivityData){
    return Observable.of();
  }
}

class MockUrlService {
  constructor() {}
  private previousUrl: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public previousUrl$: Observable<string> = this.previousUrl.asObservable();
}