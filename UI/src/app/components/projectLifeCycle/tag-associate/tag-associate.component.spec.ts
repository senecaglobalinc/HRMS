import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TagAssociateComponent } from './tag-associate.component';
describe('TagAssociateComponent', () => {
  let component: TagAssociateComponent;
  let fixture: ComponentFixture<TagAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
