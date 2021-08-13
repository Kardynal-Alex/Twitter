import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.css']
})
export class AuthPageComponent implements OnInit {

  constructor(private authService:AuthService,
              private twitterPost:TwitterPostService,
              private router:Router) { }
  isAuth:boolean=false;
  ngOnInit() {
    document.getElementById('router').className="router";
    this.isAuth=this.authService.isAuthenticated();
    if(this.isAuth)
      this.router.navigate(['/home']);
  }

  twitterImage:string="Resources\\Images\\twitterImage.jpg";
  createImgPath(path:string){
    return this.twitterPost.createImgPath(path);
  }

}
