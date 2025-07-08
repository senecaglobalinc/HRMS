import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
// import { HttpInterceptor } from './components/shared/httpInterceptor';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorHandler } from '@angular/core';
import { AppErrorHandler } from './error/app-error-handler';
import { AppPrimenNgModule } from './components/shared/module/primeng.module';
import { OAuthHelper } from './services/OAuthHelper.service';
import { BooleanToStringPipe } from './Pipes/BooleanToStringPipe';
import { HttpConfigInterceptor } from './components/shared/httpInterceptor';
import { GenericErrorsMsgsComponent } from './error/generic-errors-msgs/generic-errors-msgs.component';
import { TruncatePipe } from './Pipes/TruncatePipe';
import { SharedModule } from './components/shared/shared.module';
import { KradialogsComponent } from './components/kra/kradialogs/kradialogs.component';
import { allAnglrModules } from './plugins/all-angular-material.module';
import { ImportKraDlgComponent } from './components/kra/import-kra-dlg/import-kra-dlg.component';
import { AddKRAdlgComponent } from './components/kra/add-kradlg/add-kradlg.component';


@NgModule({
  declarations: [
    AppComponent,
    BooleanToStringPipe,
    TruncatePipe,
    GenericErrorsMsgsComponent,
    KradialogsComponent,
    ImportKraDlgComponent,
    AddKRAdlgComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    ReactiveFormsModule,
    AppPrimenNgModule,
    AppRoutingModule,
    SharedModule,
    allAnglrModules
  ],
  providers: [
    OAuthHelper,
    // { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true } 
  ],
  
  exports:[KradialogsComponent,ImportKraDlgComponent],
  entryComponents: [KradialogsComponent,ImportKraDlgComponent, AddKRAdlgComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }

