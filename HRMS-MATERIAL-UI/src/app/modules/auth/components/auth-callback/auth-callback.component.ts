import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.scss'],
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private authService: AuthService,  
    private route: Router,
    private router: ActivatedRoute,
    private spinner: NgxSpinnerService
  ) { }
 
  async ngOnInit() {
   
    let authCode;
    this.router.queryParams.subscribe(params => {
       authCode = params.auth_code
    })
    let tokenRequestObj ={
      authorization_code :null,
      client_id:null
    };
    tokenRequestObj.authorization_code = authCode;
    tokenRequestObj.client_id = environment.client_id;
    this.spinner.show();
    this.authService.getAcesstoken(tokenRequestObj).subscribe((res:any)=>{
      this.authService.setAccessToken(res.data['access_token']);
      this.GetLoggedUserEmail();
    },(error)=>{
      this.spinner.hide();
      this.route.navigate(['/errorPage']);
    })   
  }

  GetLoggedUserEmail()
  {
    this.authService.GetLoggedInUserEmail().subscribe((res)=>{
      sessionStorage['email'] = res;
      this.route.navigate(['/roles'])
    })
  }
}



