import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AspectmasterComponent } from './aspectmaster.component';


describe('AspectmasterComponent', () => {
  let component: AspectmasterComponent;
  let fixture: ComponentFixture<AspectmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AspectmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AspectmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});