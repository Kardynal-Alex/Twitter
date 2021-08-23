import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { friend } from 'src/app/models/friend';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { FriendService } from 'src/app/services/friend.service';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.component.html',
  styleUrls: ['./search-users.component.css']
})
export class SearchUsersComponent implements OnInit{

  constructor(private authService:AuthService,
              private router:Router,
              private friendService:FriendService) { }

  userId:string;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.userId=this.authService.getUserId();
    if(this.userId)
      this.getFriendsByUserId(this.userId);
  }

  search:string="";
  users:user[];
  searchUsers(){
    if(this.search!=''){
      this.authService.searchUsersByNameAndSurname(this.search).subscribe(response=>{
        this.users=response;
        setTimeout(() => {
          this.changeFriendButtonOposite();
        },100);
      });
    }else{
      this.users=null;
    }
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

  friends:friend[];
  getFriendsByUserId(userId:string){
    return this.friendService.getFriendsByUserId(userId).subscribe(response=>{
      this.friends=response;
    });
  }

  changeFriendButtonOposite(){
    for(let friend of this.friends){
      var x=document.getElementById('friend-'+friend['friendId']);
      if(x!=null)
        x.innerText='unfriend';
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
      friendId:friendId
    }
    this.friendService.addFriend(friend).subscribe(response=>{
      document.getElementById('friend-'+friendId).innerText='unfriend';
      this.friends.push(friend);
    });
  }

  removeFriend(friendId:string){
    const index=this.friends.findIndex(x=>x['friendId']==friendId);
    const friendIdToDelete=this.friends[index]['id'];
    this.friendService.deleteFriendById(friendIdToDelete).subscribe(response=>{
      document.getElementById('friend-'+friendId).innerText='friend';
      this.friends=this.friends.filter(x=>x['id']!=friendIdToDelete);
    });
  }

}
