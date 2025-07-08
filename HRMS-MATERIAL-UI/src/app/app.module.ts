import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AllAngularMaterialModule } from './modules/plugins/all-angular-material/all-angular-material.module';
import { AuthenticationModule } from './modules/auth/authentication.module';
import { NavService } from './modules/master-layout/services/nav.service';
import { MasterLayoutModule } from './modules/master-layout/master-layout.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AdminModule } from './modules/admin/admin.module';
import { KraModule } from './modules/kra/kra.module';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {
  HttpClientModule,
  HTTP_INTERCEPTORS,
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { HttpConfigInterceptor } from '../app/modules/master-layout/httpInterceptor';
import { OAuthHelper } from './core/services/OAuthHelper.service';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SharedModule } from './modules/shared/shared.module';
import { ProjectLifeCycleModule } from './modules/project-life-cycle/project-life-cycle.module';
import { TalentManagModule } from './modules/TalentMangment/talentmanagment.module';
import { AssociateexitModule} from './modules/AssociateExit/associateexit.module';
import { DialogCBRComponent } from './modules/TalentMangment/Components/dialog-cbr/dialog-cbr.component';
import { ReportsModule } from './modules/reports/reports.module';
import { UrlService } from './modules/shared/services/url.service';
import {
  AuthModule,
  LogLevel,
  OidcConfigService,
} from 'angular-auth-oidc-client';
import { Observable } from 'rxjs/internal/Observable';
import { ValidationsDirective, ValidationsDirectiveModule } from './validations.directive';
import { GradesValidationsDirective } from './grades-validations.directive';
import { PracticeAreaValidationsDirective } from './practice-area-validations.directive';
import { EmptySpacesValidationsDirective } from './empty-spaces-validations.directive';
import { ValidationImpSpecialCharsDirective } from './validation-imp-special-chars.directive';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { MatTableExporterModule } from 'mat-table-exporter';
import { MsalModule,MsalBroadcastService, MsalGuard, MsalRedirectComponent, MsalService } from '@azure/msal-angular';



@NgModule({
  declarations: [AppComponent, ConfirmationDialogComponent],
  imports: [
    BrowserModule,
    // RouterModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,

    AllAngularMaterialModule,
    MasterLayoutModule,
    FlexLayoutModule,
    AdminModule,
    KraModule,
    FormsModule,
    ReactiveFormsModule,
    AngularEditorModule,
    SharedModule,
    TalentManagModule,
    AssociateexitModule,
    ProjectLifeCycleModule,
    ReportsModule,
    AuthenticationModule,
    AuthModule.forRoot(),
    MatTableExporterModule
  ],
  exports: [RouterModule],
  providers: [NavService, OAuthHelper, UrlService,
    
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }, 
    

  ],

  entryComponents: [DialogCBRComponent],

  bootstrap: [AppComponent],
})
export class AppModule {}
