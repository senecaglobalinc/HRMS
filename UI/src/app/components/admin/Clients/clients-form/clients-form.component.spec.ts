import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ClientsFormComponent } from './clients-form.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppPrimenNgModule } from '../../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { ClientService } from '../../services/client.service';
import { of } from 'rxjs';

describe('ClientsFormComponent', () => {
  let component: ClientsFormComponent;
  let fixture: ComponentFixture<ClientsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule],
      providers :
      [
        {
          provide: [ClientService],
          // useClass: MockClientService
        }
      ] ,
      declarations: [ ClientsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should call GetClients() method on initialization', () => {
    const cService = TestBed.get(ClientService);
    spyOn(cService, 'GetClients').and.callThrough();
    component.ngOnInit();
    expect(cService.GetClients).toHaveBeenCalled();
  });

  it('should be true when valid data is given', () => {
    component.addClient.controls['ClientCode'].setValue("C12345");
    component.addClient.controls['ClientName'].setValue("SenecaGlobal");
    component.addClient.controls['ClientRegisterName'].setValue(null);
    expect(component.addClient.valid).toBe(true);
  });

  it('should be invalid when invalid data is given', () => {
    component.addClient.controls['ClientCode'].setValue(null);
    component.addClient.controls['ClientName'].setValue(null);
    component.addClient.controls['ClientRegisterName'].setValue(null);
    expect(component.addClient.valid).toBe(false);
  });

  it('should display pattern error when Client Code pattern is wrong', () => {
    component.addClient.controls['ClientCode'].setValue("cntg");
    component.addClient.controls['ClientName'].setValue("SenecaGlobal");
    component.addClient.controls['ClientRegisterName'].setValue(null);
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".ClientCode")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual("Client Code should be in pattern 'c#####'");
  });

  it('should display error when Client Code is greater than 6 characters', () => {
    component.addClient.controls['ClientCode'].setValue("c122334575");
    component.addClient.controls['ClientName'].setValue("SenecaGlobal");
    component.addClient.controls['ClientRegisterName'].setValue(null);
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".ClientCode")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual("Only 6 characters are allowed.");
  });

  it('should display error when Client Code has continuous spaces', () => {
    component.addClient.controls['ClientCode'].setValue("    ");
    component.addClient.controls['ClientName'].setValue("SenecaGlobal");
    component.addClient.controls['ClientRegisterName'].setValue(null);
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".ClientCode")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual("Should not have continuous spaces.");
  });


  it('should display error message if data is not provided', () => {
    component.addClient.controls['ClientCode'].setValue(null);
    component.addClient.controls['ClientName'].setValue("SenecaGlobal");
    component.addClient.controls['ClientRegisterName'].setValue(null);
    const buttonElement =fixture.debugElement.query(By.css(".btn-custom")).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    const el = fixture.debugElement.query(By.css(".ClientCode")).nativeElement;
    expect(el).toBeTruthy();
    expect(el.innerText).toEqual(
      "Client Code Required"
    );
  });

  it('should call addClients() method on submit', () =>{
    spyOn(component, 'addClients');
    const buttonElement = fixture.debugElement.query(By.css('.submit')).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    expect(component.addClients).toHaveBeenCalledTimes(1);
  });

  it('should call cancel() method', () =>{
    spyOn(component, 'cancel');
    const buttonElement = fixture.debugElement.query(By.css('.cancel')).nativeElement;
    buttonElement.click();
    fixture.detectChanges();
    expect(component.cancel).toHaveBeenCalledTimes(1);
  });


});
