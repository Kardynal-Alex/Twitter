import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { friend } from '../models/friend';

@Injectable({providedIn: 'root'})
export class FriendService {
    apiUrl = 'https://localhost:44318/api/friend/';
    constructor(private httpClient:HttpClient) { }

    addFriend(friend:friend){
        return this.httpClient.post(this.apiUrl+"addFriend/",friend);
    }

    deleteFriendById(id:string){
        return this.httpClient.delete(this.apiUrl+"deleteFriendById/"+id);
    }

    getFriendById(id:string){
        return this.httpClient.get<friend>(this.apiUrl+"getFriendById/"+id);
    }

    getFriendByUserAndFriendId(friend:friend){
        return this.httpClient.post(this.apiUrl+"getFriendByUserAndFriendId/",friend);
    }

    getFriendsByUserId(id:string){
        return this.httpClient.get<friend[]>(this.apiUrl+"getFriendsByUserId/"+id);
    }
}