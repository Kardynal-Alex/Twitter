import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { twitterPost } from '../models/twitter-post';

@Injectable({providedIn: 'root'})
export class TwitterPostService {
    apiUrl = 'https://localhost:44318/api/twitterpost/';
    uploadApiPhoto='https://localhost:44318/api/upload/';
    constructor(private httpClient:HttpClient) { }

    addTwitterPost(twitterPost:twitterPost){
        return this.httpClient.post(this.apiUrl+"addTwitterPost/",twitterPost);
    }

    createImgPath(serverPath: string){
        return `https://localhost:44318/${serverPath}`;
    }

    deletePhoto(path:string){
        return this.httpClient.delete(this.uploadApiPhoto+"?path="+path);
    }

    getTwitterPostByUserId(id:string){
        return this.httpClient.get<twitterPost[]>(this.apiUrl+"getUserTwitterPosts/"+id);
    }
}