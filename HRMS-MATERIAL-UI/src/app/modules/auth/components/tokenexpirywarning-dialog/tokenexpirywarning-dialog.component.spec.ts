import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TokenexpirywarningDialogComponent } from './tokenexpirywarning-dialog.component';

describe('TokenexpirywarningDialogComponent', () => {
  let component: TokenexpirywarningDialogComponent;
  let fixture: ComponentFixture<TokenexpirywarningDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TokenexpirywarningDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TokenexpirywarningDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
