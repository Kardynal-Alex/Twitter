import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { twitterPost } from 'src/app/models/twitter-post';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-view-users-tweet',
  templateUrl: './view-users-tweet.component.html',
  styleUrls: ['./view-users-tweet.component.css']
})
export class ViewUsersTweetComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private router:Router) { }

  @Input() twitterPosts:twitterPost[];
  ngOnInit(){
  }


  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }
}
