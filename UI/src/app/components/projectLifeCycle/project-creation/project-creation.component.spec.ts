import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ProjectCreationComponent } from './project-creation.component';
import { AppPrimenNgModule } from '../../shared/module/primeng.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ProjectCreationService } from '../services/project-creation.service';
import { MockProjectCreationService } from '../services/project-creation.service.spec';
import { RouterTestingModule } from '../../../../../node_modules/@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

fdescribe('ProjectCreationComponent', () => {
  let component: ProjectCreationComponent;
  let fixture: ComponentFixture<ProjectCreationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule,RouterTestingModule, HttpClientTestingModule],
      declarations: [ProjectCreationComponent],
      providers: [
        {
          provide: ProjectCreationService,
          useClass: MockProjectCreationService
        }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  fdescribe("Test form validations", () => {
    

    it("Project code should have error message when two hypens are given", () => {
      component.addProject.controls["ProjectCode"].setValue("--");
      fixture.detectChanges();
      const el = fixture.debugElement.query(By.css(".patternErr")).nativeElement;
      expect(el).toBeTruthy(); // to check if element is created
      expect(el.innerText).toEqual(
        // to check the error msg
        "No special characters are allowed expect one hyphen"
      );
    });

    it("Project code should have error message when continuous spaces are given", () => {
      component.addProject.controls["ProjectCode"].setValue("    ");
      fixture.detectChanges();
      const el = fixture.debugElement.query(By.css(".invalidErr")).nativeElement;
      expect(el).toBeTruthy();
      expect(el.innerText).toEqual(
        "Invalid Project Code."
      );
    });
    //requiredErr
    it(" Project type should have error message when value is not selected", () => {
      component.addProject.controls["ProjectTypeId"].setValue(null);
      let buttonEl = fixture.debugElement.query(By.css(".submitBtn")).nativeElement;
      buttonEl.click();
      fixture.detectChanges();
      const el = fixture.debugElement.query(By.css(".requiredErr")).nativeElement;
      expect(el).toBeTruthy();
      expect(el.innerText).toEqual(
        "Required Field."
      );
    });

    it("form should be invalid when mandatory fields are selected", () => {
      component.addProject.controls["ProjectCode"].setValue("SG123");
      component.addProject.controls["ProjectName"].setValue("SG");
      component.addProject.controls["DomainId"].setValue(1);
      component.addProject.controls["ProjectStateId"].setValue(null);
      component.addProject.controls["ClientId"].setValue(null);
      component.addProject.controls["ManagerId"].setValue(null);
      component.addProject.controls["PracticeAreaId"].setValue(null);
      component.addProject.controls["ActualStartDate"].setValue(null);
      component.addProject.controls["ActualEndDate"].setValue(null);
      component.addProject.controls["ProjectTypeId"].setValue(null);
      component.addProject.controls["DepartmentId"].setValue(1);

      let buttonEl = fixture.debugElement.query(By.css(".submitBtn")).nativeElement;
      buttonEl.click();
      fixture.detectChanges();

      expect(component.addProject.valid).toBeFalsy();

    });
  });

  fdescribe("test saveProject method", () => {

    it("should call saveProject method on button click", () => {
     // const projectcreationService = TestBed.get(ProjectCreationService);
      spyOn(component , "SaveProject");
      component.addProject.controls["ProjectCode"].setValue("SG123");
      component.addProject.controls["ProjectName"].setValue("SG");
      component.addProject.controls["DomainId"].setValue(1);
      component.addProject.controls["ProjectStateId"].setValue(1);
      component.addProject.controls["ClientId"].setValue(1);
      component.addProject.controls["ManagerId"].setValue(1);
      component.addProject.controls["PracticeAreaId"].setValue(1);
      component.addProject.controls["ActualStartDate"].setValue(new Date(28-5-2019));
      component.addProject.controls["ActualEndDate"].setValue(new Date(28-5-2019));
      component.addProject.controls["ProjectTypeId"].setValue(1);
      component.addProject.controls["DepartmentId"].setValue(1);
      let buttonEl = fixture.debugElement.query(By.css(".submitBtn")).nativeElement;
      buttonEl.click();
      expect(component.SaveProject).toHaveBeenCalled();
    })
  })


});