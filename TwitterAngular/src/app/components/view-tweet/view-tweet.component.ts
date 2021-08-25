import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { like } from 'src/app/models/like';
import { twitterPost } from 'src/app/models/twitter-post';
import { AuthService } from 'src/app/services/auth.service';
import { LikeService } from 'src/app/services/like.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-view-tweet',
  templateUrl: './view-tweet.component.html',
  styleUrls: ['./view-tweet.component.css']
})
export class ViewTweetComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private activateRoute:ActivatedRoute,
              private authService:AuthService,
              private likeService:LikeService) { }

  id:string;
  twitterPost:twitterPost;
  userTokenId:string;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.id=this.activateRoute.snapshot.params['id'];
    this.twitterPostService.getTwitterPostByIdWithDetails(this.id).subscribe(response=>{
      this.twitterPost=response;
    });
    this.userTokenId=this.authService.getUserId();
    if(this.userTokenId)
      this.getLikeByUserAndTwitterPostId(this.id);
  }

  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

  getLikeByUserAndTwitterPostId(twitterPostId:string){
    const like:like={
      id:Guid.create().toString(),
      twitterPostId:twitterPostId,
      userId:this.userTokenId
    }
    this.likeService.getLikeByUserAndTwitterPostId(like).subscribe(response=>{
      this.like=<like>response;
      if(response!=null){
        setTimeout(()=>{
          var x=document.getElementById("like-"+twitterPostId);
          if(x!=null)
            x.style.background="red";
          },100);
      }
    })
  }

  like:like;
  changeHeartIconTweet(tweet:twitterPost){
    document.getElementById("like-"+tweet['id']).style.background==""?
      this.likeTweet(tweet):this.removeLikeTweet(tweet);
  }

  likeTweet(tweet:twitterPost){
    const like:like={
      id:Guid.create().toString(),
      twitterPostId:tweet['id'],
      userId:this.userTokenId
    }
    tweet['like']=tweet['like']+1;
    this.twitterPostService.updateOnlyTwitterPost(tweet).subscribe(response=>{});
    this.likeService.addLike(like).subscribe(response=>{
      document.getElementById("like-"+tweet['id']).style.background="red";
      this.like=like;
    });
  }

  removeLikeTweet(tweet:twitterPost){
    tweet['like']=tweet['like']-1;
    this.twitterPostService.updateOnlyTwitterPost(tweet).subscribe(response=>{});
    this.likeService.deleteLikeById(this.like['id']).subscribe(response=>{
      document.getElementById("like-"+tweet['id']).style.background="";
      this.like=null;
    });
  }

}
