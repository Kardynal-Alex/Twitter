import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { like } from '../models/like';


@Injectable({providedIn: 'root'})
export class LikeService {
    apiUrl = 'https://localhost:44318/api/like/';
    constructor(private httpClient:HttpClient) { }

    addLike(like:like){
        return this.httpClient.post(this.apiUrl+"addLike/",like);
    }

    deleteLikeById(id:string){
        return this.httpClient.delete(this.apiUrl+"deleteLikeById/"+id);
    }

    getLikesByUserId(id:string){
        return this.httpClient.get<like[]>(this.apiUrl+"getLikesByUserId/"+id);
    }

    getLikeByUserAndTwitterPostId(like:like){
        return this.httpClient.post(this.apiUrl+"getLikeByUserAndTwitterPostId/",like);
    }
}