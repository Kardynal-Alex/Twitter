import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Guid} from 'guid-typescript';
import {friend} from 'src/app/models/friend';
import {user} from 'src/app/models/user';
import {AuthService} from 'src/app/services/auth.service';
import {FriendService} from 'src/app/services/friend.service';

@Component({selector: 'app-user-friends', templateUrl: './user-friends.component.html', styleUrls: ['./user-friends.component.css']})
export class UserFriendsComponent implements OnInit {

    constructor(private authService : AuthService, private activateRoute : ActivatedRoute, private friendService : FriendService, private router : Router) {}

    ngOnInit() {
        document.getElementById('router').className = "router1";
        this.userId = this.activateRoute.snapshot.params['id'];
        if (this.userId) 
            this.seeUserFriend(this.userId);
        
        this.tokenUserId = this.authService.getUserId();
        if (this.tokenUserId) 
            this.getFriendsByUserId(this.tokenUserId);
        
    }

    userId : string; // from query friend id
    users : user[];
    tokenUserId : string; // only native user can add friend from token
    seeUserFriend(userId : string) {
        this.authService.getUserFriendsByUserId(userId).subscribe(response => {
            this.users = response;
            setTimeout(() => {
                this.changeFriendButtonOposite();
            }, 100);
        });
    }

    friends : friend[]; // native user friends id is taken from token(native user can add friend)
    getFriendsByUserId(userId : string) {
        return this.friendService.getFriendsByUserId(userId).subscribe(response => {
            this.friends = response;
        });
    }

    changeFriendButtonOposite() {
        for (let friend of this.friends) {
            var x = document.getElementById('friend-' + friend['friendId']);
            if (x != null) 
                x.innerText = 'unfriend';
            
        }
    }

    changeFriend(friendId : string) {
        document.getElementById('friend-' + friendId).innerText == 'friend' ? this.addFriend(friendId) : this.removeFriend(friendId);
    }

    addFriend(friendId : string) {
        const friend: friend = {
            id: Guid.create().toString(),
            userId: this.tokenUserId,
            friendId: friendId
        }
        this.friendService.addFriend(friend).subscribe(response => {
            document.getElementById('friend-' + friendId).innerText = 'unfriend';
            this.friends.push(friend);
        });
    }

    removeFriend(friendId : string) {
        const index = this.friends.findIndex(x => x['friendId'] == friendId);
        const friendIdToDelete = this.friends[index]['id'];
        this.friendService.deleteFriendById(friendIdToDelete).subscribe(response => {
            document.getElementById('friend-' + friendId).innerText = 'friend';
            this.friends = this.friends.filter(x => x['id'] != friendIdToDelete);
        });
    }

    navigateToUserProfile(id : string) {
        this.router.navigate(['user-profile/' + id]);
    }

}
