import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RouterModule } from '@angular/router';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpConfigInterceptor } from '../master-layout/httpInterceptor';
import { OAuthHelper } from '../../core/services/OAuthHelper.service';
import {
  AuthModule,
  LogLevel,
  OidcConfigService,
} from 'angular-auth-oidc-client';
import { APP_INITIALIZER } from '@angular/core';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import{environment} from '../../../environments/environment';
//import{environment} from '../../../environments/environment.qa';

@NgModule({
  declarations: [LoginComponent, AuthCallbackComponent],
  imports: [
    CommonModule,
    RouterModule,
    AllAngularMaterialModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    AuthModule.forRoot(),
  ],
  providers: [
    OAuthHelper,

    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService],
      multi: true,
    },
  ],
})
export class AuthenticationModule {}
// export function configureAuth(oidcConfigService: OidcConfigService) {
//   debugger;
//   return () =>
//     oidcConfigService.withConfig({
     
//       stsServer: environment.IDENTITY_SERVER_URL,
//       redirectUrl:environment.IDENTITY_SERVER_REDIRECT_URL,
//      // postLogoutRedirectUri: environment.postLogoutRedirectUri,
//       clientId: environment.clientId,
//       scope: environment.scope,
//       responseType: 'id_token token',
//     });
// }
