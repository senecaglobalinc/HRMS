import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrSectionsComponent } from './adr-sections.component';

describe('AdrSectionsComponent', () => {
  let component: AdrSectionsComponent;
  let fixture: ComponentFixture<AdrSectionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrSectionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrSectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
