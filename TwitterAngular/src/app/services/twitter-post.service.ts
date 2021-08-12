import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class TwitterPostService {
    apiUrl = 'https://localhost:44318/api/twitterpost/';
    uploadApiPhoto='https://localhost:44318/api/upload/';
    constructor(private httpClient:HttpClient) { }



    createImgPath(serverPath: string){
        return `https://localhost:44318/${serverPath}`;
    }
}