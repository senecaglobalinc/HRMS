import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DomainFormComponent } from './domain-form.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppPrimenNgModule } from '../../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { DomainMasterService } from '../../services/domainmaster.service';

fdescribe('DomainFormComponent', () => {
  let component: DomainFormComponent;
  let fixture: ComponentFixture<DomainFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule],
      // providers: [   {
      //       provide: DomainMasterService,
      //       useClass: MockDomainMasterService
      //     } ],
      declarations: [ DomainFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DomainFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should be true when valid data is given', () => {
    component.addDomainName.controls['DomainName'].setValue("web Technology");
    expect(component.addDomainName.valid).toBe(true);
  });
  it('should be invalid when invalid data is given', () => {
    component.addDomainName.controls['DomainName'].setValue(null);
    expect(component.addDomainName.valid).toBe(false);
  });

  it('should display error message if data is not provided', () => {
    component.addDomainName.controls['DomainName'].setValue(null);
    const buttonElement =fixture.debugElement.query(By.css(".btn-custom")).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".DomainName")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual(
      "Domain Name Required"
    );
  });
  it('should display error message DomainName has Only characters allowed', () => {
    component.addDomainName.controls['DomainName'].setValue("7689543");
    const buttonElement =fixture.debugElement.query(By.css(".btn-custom")).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".DomainName")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual(
      "Only characters allowed"
    );
  });

  it('should display error message DomainName has Only 100 characters allowed', () => {
    component.addDomainName.controls['DomainName'].setValue("uigujghjgjhggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggdgfdhgfghjgjhghjgjhgjhgjhghjgjhghjgjhgjhgjhgjhgjhgjhghjghjgjhgjhghjklllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll");
    const buttonElement =fixture.debugElement.query(By.css(".btn-custom")).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".DomainName")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual(
      "Only 100 characters"
    );
  });

  it('should call addDomain() method', () =>{
    spyOn(component, 'addDomain');
    const buttonElement = fixture.debugElement.query(By.css('.submit')).nativeElement;
    buttonElement.click();
    expect(component.addDomain).toHaveBeenCalledTimes(1);
  });

  it('should call cancel() method', () =>{
    spyOn(component, 'cancel');
    const buttonElement = fixture.debugElement.query(By.css('.cancel')).nativeElement;
    buttonElement.click();
    expect(component.cancel).toHaveBeenCalledTimes(1);
  });

});
