import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { facebook } from 'src/app/models/facebook';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

declare var FB: any;

@Component({
  selector: 'app-facebook-login',
  templateUrl: './facebook-login.component.html',
  styleUrls: ['./facebook-login.component.css']
})
export class FacebookLoginComponent implements OnInit {

  constructor(private toastrService:ToastrService,
    private authService:AuthService,
    private localStorage:LocalStorageService) { }

  ngOnInit(){
    (window as any).fbAsyncInit = function() {
      FB.init({
        appId      : '539783017256590',
        cookie     : true,
        xfbml      : true,
        version    : 'v11.0'
      });
    FB.AppEvents.logPageView();
  };

  (function(d, s, id){
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) {return;}
      js = d.createElement(s); 
      js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
  }

  submitLogin(){
    FB.login((response:any)=>{
      if (response.authResponse)
      {
        const facebookLogin:facebook={
          accessToken:response.authResponse['accessToken']
        }
        this.authService.facebookLogin(facebookLogin).subscribe(response=>{
          this.toastrService.success("Login successfully.");
          const token=(<any>response).token;
          this.localStorage.set("token", JSON.stringify(token));
        },error=>{
          this.toastrService.error("error");
        });
      }
    });
  }

}
