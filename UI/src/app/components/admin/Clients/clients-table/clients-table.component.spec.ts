import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ClientsTableComponent } from './clients-table.component';
import { ClientService } from '../../services/client.service';
import { of } from 'rxjs';
import { AppPrimenNgModule } from '../../../shared/module/primeng.module';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

const MOCK_DATA  = [
  {
    ClientId:5,
    ClientCode:"ALPS",
    ClientName:"ALPS",
    ClientRegisterName:"ALPS",
    isActive : true
  }
];

// const MOCK_TABLE_DATA  = [
//   {
//     ClientCode:"ALPS",
//     ClientName:"ALPS",
//     ClientRegisterName:"ALPS"
//   }
// ];

class MockClientService{
  getClients(){
    return of(MOCK_DATA);
  }
}

fdescribe('ClientsTableComponent', () => {
  let component: ClientsTableComponent;
  let fixture: ComponentFixture<ClientsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,AppPrimenNgModule, ReactiveFormsModule],
      declarations: [ ClientsTableComponent ],
      providers :
      [
        {
          provide: [ClientService],
          useClass: MockClientService
        }
      ] 
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getClients method on initialization', () => {
    const cService = TestBed.get(ClientService);
    spyOn(cService, 'getClients').and.callThrough();
    component.ngOnInit();
    expect(cService.getClients).toHaveBeenCalled();
  });

  //it('should call editClients() method when clicks on edit', () => {
  //  const data = fixture.debugElement.query(By.css('data')).nativeElement;
  //  spyOn(component, 'editClients').and.callThrough();
  //  const buttonElement = fixture.debugElement.query(By.css('i.editCls')).nativeElement;
  //  console.log(buttonElement);
  //  buttonElement.click();
  //  fixture.detectChanges();
  //  expect(component.editClients).toHaveBeenCalled();
  //});

  
});
