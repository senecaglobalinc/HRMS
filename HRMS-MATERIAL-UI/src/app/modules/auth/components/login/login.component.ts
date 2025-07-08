import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { themeconfig } from 'src/themeconfig';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  email: string;
  password: string;
  authenticationStatus: string = 'Loading';
  authenticationUrl = environment.authUrl;
  themeConfigInput = themeconfig.formfieldappearances;
  production: boolean = environment.production;

  constructor(private route: Router, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    if (this.production) {
      this.checkForCallback();
    }

    this.loginForm = this.formBuilder.group({
      email: [
        'kalyan.penumutchu@senecaglobal.com',
        [Validators.required, Validators.email],
      ],
      password: [''],
    });
  }

  login() {
    window.location.href = this.authenticationUrl;
  }

  loginUser() {
    sessionStorage['email'] = this.loginForm.get('email').value;
    this.route.navigate(['/roles']);
  }

  checkForCallback() {
    if (window.location.href.includes('auth_code')) {
      const authCode = window.location.href.split('auth_code=')[1];
      this.route.navigate(['/auth-callback'], {
        queryParams: { auth_code: authCode },
      });
    }
  }
}
