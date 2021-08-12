import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.css']
})
export class AuthPageComponent implements OnInit {

  constructor(private authService:AuthService,
              private twitterPost:TwitterPostService) { }

  ngOnInit() {
  }

  twitterImage:string="Resources\\Images\\twitterImage.jpg";
  createImgPath(path:string){
    return this.twitterPost.createImgPath(path);
  }

}
