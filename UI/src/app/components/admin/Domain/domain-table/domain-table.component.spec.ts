import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { DomainTableComponent } from './domain-table.component';
import { DomainMasterService } from '../../services/domainmaster.service';
import { of } from 'rxjs';
import { AppPrimenNgModule } from '../../../shared/module/primeng.module';
import { ReactiveFormsModule } from '@angular/forms';


describe('DomainTableComponent', () => {
  let component: DomainTableComponent;
  let fixture: ComponentFixture<DomainTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports : [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule],
      declarations: [ DomainTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DomainTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should call getDomains method on initialization', () => {
    const cService = TestBed.get(DomainMasterService);
    spyOn(cService, 'getDomains').and.callThrough();
    component.ngOnInit();
    expect(cService.getDomains).toHaveBeenCalled();
  });
});
