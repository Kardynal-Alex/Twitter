import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { comment } from '../models/comment';

@Injectable({providedIn: 'root'})
export class CommentService {
    apiUrl = 'https://localhost:44318/api/comment/';
    constructor(private httpClient:HttpClient) { }

    addComment(comment:comment){
        return this.httpClient.post(this.apiUrl+"addComment/",comment);
    }

    deleteCommentById(id:string){
        return this.httpClient.delete(this.apiUrl+"deleteCommentById/"+id);
    }

    getCommentsByTwitterPostId(id:string){
        return this.httpClient.get<comment[]>(this.apiUrl+"getCommentsByTweetId/"+id);
    }
}