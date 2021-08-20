import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { favorite } from 'src/app/models/favorite';
import { twitterPost } from 'src/app/models/twitter-post';
import { AuthService } from 'src/app/services/auth.service';
import { FavoriteService } from 'src/app/services/favorite.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-view-users-tweet',
  templateUrl: './view-users-tweet.component.html',
  styleUrls: ['./view-users-tweet.component.css']
})
export class ViewUsersTweetComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private router:Router,
              private authService:AuthService,
              private favoriteService:FavoriteService) { }

  @Input() twitterPosts:twitterPost[];
  userTokenId:string;
  ngOnInit(){
    this.userTokenId=this.authService.getUserId();
    if(this.userTokenId)
      setTimeout(()=>
      this.getFavoritesByTokenUserId(this.userTokenId),500);
  }

  getFavoritesByTokenUserId(userId:string){
    this.favoriteService.getFavoritesByUserId(userId).subscribe(response=>{
      this.favorites=response;
      for(let favorite of response){
        var x=document.getElementById("save-"+favorite['twitterPostId']);
        if(x!=null)
          x.style.background="black";
      }
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
}
