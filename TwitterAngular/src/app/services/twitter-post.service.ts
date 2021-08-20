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

    deleteTwitterPost(twitterPost:twitterPost){
        return this.httpClient.post(this.apiUrl+"deleteTwitterPost/",twitterPost);
    }

    updateTwitterPostWithImages(twitterPost:twitterPost){
        return this.httpClient.put(this.apiUrl+"updateTwitterPost/",twitterPost);
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

    getTwitterPostByIdWithDetails(id:string){
        return this.httpClient.get<twitterPost>(this.apiUrl+"getTweetByIdWithDetails/"+id);
    }

    getTwitterpostByUserIdWithImagesAndUsers(id:string){
        return this.httpClient.get<twitterPost[]>(this.apiUrl+"getTweetByUserIdWithImagesAndUsers/"+id);
    }

    getFriendsTweetsByUserId(id:string){
        return this.httpClient.get<twitterPost[]>(this.apiUrl+"getFriendsTweetsByUserId/"+id);
    }

    getFavoriteUserTwitterPostsByUserId(id:string){
        return this.httpClient.get<twitterPost[]>(this.apiUrl+"getFavoriteTwitterPostsByUserId/"+id);
    }

    uploadPhoto(files){
        let fileToUpload=<File>files[0];
        let formData=new FormData();
        formData.append('file',fileToUpload,fileToUpload.name);
        return this.httpClient.post(this.uploadApiPhoto,formData, {reportProgress: true, observe: 'events'});
    }
}