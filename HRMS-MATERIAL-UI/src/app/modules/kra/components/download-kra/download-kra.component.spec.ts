import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadKraComponent } from './download-kra.component';

describe('DownloadKraComponent', () => {
  let component: DownloadKraComponent;
  let fixture: ComponentFixture<DownloadKraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadKraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadKraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
