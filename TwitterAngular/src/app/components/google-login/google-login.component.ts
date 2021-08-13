import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { externalAuth } from 'src/app/models/external-auth';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-google-login',
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.css']
})
export class GoogleLoginComponent implements OnInit {
  @ViewChild('loginRef', {static: true }) loginElement: ElementRef;
  auth2:any;
  constructor(private toastrService:ToastrService,
              private authService:AuthService,
              private localStorage:LocalStorageService,
              private router:Router) { }

  ngOnInit(){
    this.googleInitialize();
  }

  
  googleInitialize() {
    window['googleSDKLoaded'] = () => {
      window['gapi'].load('auth2', () => {
        this.auth2 = window['gapi'].auth2.init({
          client_id: '514920176630-8216lvu3dj91rsie16s39k1bbhsj2tg3.apps.googleusercontent.com',
          cookie_policy: 'single_host_origin',
          scope: 'profile email'
        });
        this.prepareLogin();
      });
    }
    (function(d, s, id){
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) {return;}
      js = d.createElement(s); js.id = id;
      js.src = "https://apis.google.com/js/platform.js?onload=googleSDKLoaded";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'google-jssdk'));
  }

  prepareLogin() {
    this.auth2.attachClickHandler(this.loginElement.nativeElement, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        const externalAuth: externalAuth = {
          provider: "GOOGLE",
          idToken: googleUser.getAuthResponse().id_token
        }
        this.authService.googleLogin(externalAuth).subscribe(response=>{
          this.router.navigate(['/home']);
          document.getElementById('auth').style.display="none";
          this.toastrService.success("Login successfully.");
          const token=(<any>response).token;
          this.localStorage.set("token", JSON.stringify(token));
          document.getElementById('router').className="router1";
        },error=>{
          this.toastrService.error("error");
        })
        //console.log('Token || ' + googleUser.getAuthResponse().id_token);
        //console.log('Email: ' , profile.getEmail());
        //console.log("profile", googleUser.getAuthResponse());
      }, (error) => {
        alert(JSON.stringify(error, undefined, 2));
      });
  }

}
