import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { friend } from 'src/app/models/friend';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { FriendService } from 'src/app/services/friend.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, AfterViewInit {

  constructor(private twitterPostService:TwitterPostService,
              private activateRoute:ActivatedRoute,
              private authService:AuthService,
              private friendService:FriendService) { }

  backgroungImage:string="Resources\\Images\\twitterImage.jpg";
  friendId:string;
  userId:string;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.friendId=this.activateRoute.snapshot.params['id'];
    this.getUserById(this.friendId);
    this.getTwitterPostByUserIdWithImagesAndUsers(this.friendId);
    this.userId=this.authService.getUserId();
  }

  ngAfterViewInit(){
    this.getFriendByUserAndFriendId();
  }

  twitterPosts:twitterPost[];
  getTwitterPostByUserIdWithImagesAndUsers(id:string){
    this.twitterPostService.getTwitterpostByUserIdWithImagesAndUsers(id).subscribe(response=>{
      this.twitterPosts=response;
    });
  }

  user:user;
  getUserById(id:string){
    this.authService.getUserById(id).subscribe(response=>{
      this.user=response;
    });
  }  

  friend:friend=null;
  getFriendByUserAndFriendId(){
    if(this.userId && this.friendId){
      const friend:friend={
        id:Guid.create().toString(),
        userId:this.userId,
        friendId:this.friendId
      };
      this.friendService.getFriendByUserAndFriendId(friend).subscribe(response=>{
        this.friend=<friend>response;
        if(this.friend!=null)
          document.getElementById('friend-'+this.friendId).innerText="unfriend";
      })
    }
  }

  changeFriend(friendId:string){
    document.getElementById('friend-'+friendId).innerText=='friend'?
        this.addFriend(friendId):this.removeFriend(friendId);
  }

  addFriend(friendId:string){
    const friend:friend={
      id:Guid.create().toString(),
      userId:this.userId,
      friendId:this.friendId
    }
    this.friendService.addFriend(friend).subscribe(response=>{
      document.getElementById('friend-'+friendId).innerText="unfriend";
      this.friend=friend;
    });
  }

  removeFriend(friendId:string){
    this.friendService.deleteFriendById(this.friend['id']).subscribe(response=>{
      document.getElementById('friend-'+friendId).innerText="friend";
      this.friend=null;
    });
  }

  createImgPath(path:string){
    return this.twitterPostService.createImgPath(path);
  }

  openEditProfileForm(){
    document.getElementById("editProfile").style.display="block";
  }

}
