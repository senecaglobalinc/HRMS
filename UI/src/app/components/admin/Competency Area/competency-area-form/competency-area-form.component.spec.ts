import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyAreaFormComponent } from './competency-area-form.component';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from "@angular/common/http";
import { APP_BASE_HREF } from '@angular/common';
import { AppPrimenNgModule } from '../../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { CompetencyAreaService } from '../../services/competency-area.service';
import { MockCompetencyAreaService } from '../../services/competency-area.service.spec';
import { HttpClientTestingModule } from '@angular/common/http/testing';

fdescribe('CompetencyAreaFormComponent', () => {
  let component: CompetencyAreaFormComponent;
  let fixture: ComponentFixture<CompetencyAreaFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientTestingModule],
      // providers: [  {
      //   provide: CompetencyAreaService,
      //   useClass: MockCompetencyAreaService
      // }],

      declarations: [CompetencyAreaFormComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyAreaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("form should be invalid when invalid data is given", () => {
    component.addCompetencyArea.controls["CompetencyAreaCode"].setValue(null);
    component.addCompetencyArea.controls["CompetencyAreaDescription"].setValue(null);
    fixture.detectChanges();
    expect(component.addCompetencyArea.valid).toBe(false);
  });

  it("form should be valid when valid data is given", () => {
    component.addCompetencyArea.controls["CompetencyAreaCode"].setValue("technology");
    component.addCompetencyArea.controls["CompetencyAreaDescription"].setValue("technology");
    fixture.detectChanges();
    expect(component.addCompetencyArea.valid).toBe(true);
  });

  it("form should have error message when invalid data is given", () => {
    component.addCompetencyArea.controls["CompetencyAreaCode"].setValue(null);
    component.addCompetencyArea.controls["CompetencyAreaDescription"].setValue("technology");

    const buttonElement = fixture.debugElement.query(By.css(".btn-custom")).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".CompetencyAreaCode")).nativeElement;

    expect(el).toBeTruthy(); // to check if element is created
    expect(el.innerText).toEqual(
      // to check the error msg
      "Competency Area Code Required"
    );

  });



});