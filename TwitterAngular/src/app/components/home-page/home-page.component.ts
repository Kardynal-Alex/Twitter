import { Component, Input, OnInit } from '@angular/core';
import { twitterPost } from 'src/app/models/twitter-post';
import { AuthService } from 'src/app/services/auth.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  constructor(private authService:AuthService,
              private twitterPostService:TwitterPostService) { }
  ngOnInit() {
    document.getElementById('router').className="router1";
    this.userId=this.authService.getUserId();
    if(this.userId)
      this.getFriendsTweetsByUserId(this.userId);
  }
  twitterPosts:twitterPost[];
  userId:string;

  getFriendsTweetsByUserId(userId:string){
    this.twitterPostService.getFriendsTweetsByUserId(userId).subscribe(response=>{
      this.twitterPosts=response;
    });
  }

}
