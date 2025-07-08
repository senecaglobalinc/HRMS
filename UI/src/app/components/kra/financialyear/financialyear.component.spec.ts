// import { async, ComponentFixture, TestBed } from '@angular/core/testing';
// import { FinancialyearComponent } from './financialyear.component';
// import { HttpClientModule } from '@angular/common/http';
// import { ReactiveFormsModule } from '@angular/forms';
// import { AppPrimenNgModule } from '../../shared/module/primeng.module';
// import { FinancialYearService, MockFinancialService } from "../Services/financialyear.service";
// import { CommonService } from "../../../services/common.service";
// import { By } from '@angular/platform-browser';
// import { RouterTestingModule } from '@angular/router/testing';
// import { ActivatedRoute } from '@angular/router';

// fdescribe('FinancialyearComponent', () => {
//   let component: FinancialyearComponent;
//   let fixture: ComponentFixture<FinancialyearComponent>;

//   beforeEach(async(() => {
//     TestBed.configureTestingModule({
//       imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule,  RouterTestingModule],
//       providers: [ { provide :FinancialYearService ,useClass : MockFinancialService},
//                     { provide : CommonService, useValue:{} },
//         {
//           provide: ActivatedRoute,
//           useValue: {
//             routeConfig: {
//               component: {
//                 name: "Iron man"
//               }
//             }
//           }
//         } ],
//       declarations: [ FinancialyearComponent ]
//     })
//     .compileComponents();
//   }));

//   beforeEach(() => {
//     fixture = TestBed.createComponent(FinancialyearComponent);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });
// });
