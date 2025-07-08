
import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse
} from '@angular/common/http'
import { catchError,tap} from 'rxjs/operators';
import { Router } from '@angular/router';
import { Observable, EMPTY, throwError } from 'rxjs';
import * as environmentInformation from '../../../environments/environment';
import { AuthService } from '../../core/services/auth.service'

@Injectable()

export class HttpConfigInterceptor  implements HttpInterceptor {

  UserInfo:string;
  constructor(private _router : Router, private authService:AuthService){}
  	intercept(request: HttpRequest<any>, next: HttpHandler):Observable<HttpEvent<any>> {
      if (!this.authService.loggedIn()) {
        this._router.navigate(['/login']);
      }
      this.UserInfo=sessionStorage["AssociatePortal_UserInformation"];
        if ( sessionStorage["token"] != null) {
            request = request.clone({
                setHeaders: {
                  Authorization: `Bearer ${sessionStorage["token"]}`,
                    UserName:this.UserInfo!=undefined|| this.UserInfo!=null? JSON.parse(this.UserInfo).email:''
                }
              });
        }
        else {
          this._router.navigate(['/login']);
        }

	    return next.handle(request)
	    .pipe(
	        tap(event => {
	          if (event instanceof HttpResponse) {

	          }
	        }, error => {

                if (error.status == 401) {
                  sessionStorage.clear();
                  if(environmentInformation.environment.production == true){
                    this._router.navigate(['/login']);
                  }
                  else{
                    this._router.navigate(['/login']);
                  }

                    return EMPTY;
                 }
                else {
                    return throwError(error);
                }

	        })
	      )

    };


}
