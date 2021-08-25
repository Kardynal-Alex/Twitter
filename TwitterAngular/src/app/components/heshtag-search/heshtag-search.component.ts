import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { favorite } from 'src/app/models/favorite';
import { like } from 'src/app/models/like';
import { twitterPost } from 'src/app/models/twitter-post';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { LikeService } from 'src/app/services/like.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-heshtag-search',
  templateUrl: './heshtag-search.component.html',
  styleUrls: ['./heshtag-search.component.css','../view-users-tweet/view-users-tweet.component.css']
})
export class HeshtagSearchComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private favoriteService:FavoriteService,
              private router:Router,
              private authService:AuthService,
              private likeService:LikeService) { }

  userTokenId:string;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.userTokenId=this.authService.getUserId();
    if(this.userTokenId){
      this.getFavoritesByTokenUserId(this.userTokenId);
      this.getLikesByTokenUserId(this.userTokenId);
    }
  }

  twitterPosts:twitterPost[];
  search:string="test";
  searchTweets(){
    if(this.search!=''){
      this.twitterPostService.searchTweetsByHeshTag(this.search).subscribe(response=>{
        this.twitterPosts=response;
        setTimeout(()=>
          this.renderfavoriteTweets()
        ,100);
        this.setLikesAfterSearch();
      });
    }else{
      this.twitterPosts=null;
    }
  }

  renderfavoriteTweets(){
    for(let favorite of this.favorites){
      var x=document.getElementById("save-"+favorite['twitterPostId']);
      if(x!=null)
        x.style.background="black";
    }
  }

  getFavoritesByTokenUserId(userId:string){
    this.favoriteService.getFavoritesByUserId(userId).subscribe(response=>{
      this.favorites=response;
    });
  }

  favorites:favorite[];
  changeIconSavedPost(twitterPostId:string){
    document.getElementById("save-"+twitterPostId).style.background==""?
      this.addToFavorite(twitterPostId):this.removeFromFavorite(twitterPostId);
  }

  addToFavorite(twitterPostId:string){
    const favorite:favorite={
      id:Guid.create().toString(),
      twitterPostId:twitterPostId,
      userId:this.userTokenId
    }
    this.favoriteService.addFavorite(favorite).subscribe(response=>{
      document.getElementById("save-"+twitterPostId).style.background="black";
      this.favorites.push(favorite);
    });
  }

  removeFromFavorite(twitterPostId:string){
    const index=this.favorites.findIndex(x=>x['twitterPostId']==twitterPostId);
    const favoriteId=this.favorites[index]['id'];
    this.favoriteService.deleteFavoriteById(favoriteId).subscribe(response=>{
      document.getElementById("save-"+twitterPostId).style.background="";
      this.favorites=this.favorites.filter(x=>x['id']!=favoriteId);
    });
  }

  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

  getLikesByTokenUserId(userId:string){
    this.likeService.getLikesByUserId(userId).subscribe(response=>{
      this.likes=response;
    });
  }

  setLikesAfterSearch(){
    setTimeout(()=>{
      for(let like of this.likes){
        var x=document.getElementById("like-"+like['twitterPostId']);
        if(x!=null)
          x.style.background="red";
      }
    },100);
  }

  likes:like[];
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
      this.likes.push(like);
    });
  }

  removeLikeTweet(tweet:twitterPost){
    tweet['like']=tweet['like']-1;
    const index=this.likes.findIndex(x=>x['twitterPostId']==tweet['id']);
    const likeId=this.likes[index]['id'];
    this.twitterPostService.updateOnlyTwitterPost(tweet).subscribe(response=>{});
    this.likeService.deleteLikeById(likeId).subscribe(response=>{
      document.getElementById("like-"+tweet['id']).style.background="";
      this.likes=this.likes.filter(x=>x['id']!=likeId);
    });
  }
}
