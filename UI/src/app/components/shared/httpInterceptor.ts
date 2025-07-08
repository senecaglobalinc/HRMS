
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

@Injectable()

export class HttpConfigInterceptor  implements HttpInterceptor {

  constructor(private _router : Router){}
  	intercept(request: HttpRequest<any>, next: HttpHandler):Observable<HttpEvent<any>> {
        if (sessionStorage["token"] != "null") {
            request = request.clone({
                setHeaders: {
                  Authorization: `Bearer ${sessionStorage["token"]}`
                }
              });
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
                        window.location.href = environmentInformation.environment.LogOutURL;
                  }
                  else
                    // this._router.navigate(['/login']);
                  
                    return EMPTY;
                 }
                else {
                    return throwError(error);
                }

	        })
	      )

    };
  
 
}
