import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { favorite } from 'src/app/models/favorite';
import { twitterPost } from 'src/app/models/twitter-post';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-saved-posts',
  templateUrl: './saved-posts.component.html',
  styleUrls: ['./saved-posts.component.css']
})
export class SavedPostsComponent implements OnInit {

  constructor(private authService:AuthService,
              private favoriteService:FavoriteService,
              private twitterPostService:TwitterPostService,
              private router:Router) { }

  userTokenId:string;
  twitterPosts:twitterPost[];
  ngOnInit() {
    document.getElementById('router').className="router1";
    this.userTokenId=this.authService.getUserId();
    if(this.userTokenId)
      this.twitterPostService.getFavoriteUserTwitterPostsByUserId(this.userTokenId).subscribe(response=>{
        this.twitterPosts=response;
      });
  }

  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

  removeBookmark(tweetId:string){
    const favorite:favorite={
      id:Guid.create().toString(),
      twitterPostId:tweetId,
      userId:this.userTokenId
    }
    this.favoriteService.deleteFavoriteByTwitterPostAndUserId(favorite).subscribe(response=>{
      this.twitterPosts=this.twitterPosts.filter(x=>x['id']!=tweetId);
    });
  }

}
