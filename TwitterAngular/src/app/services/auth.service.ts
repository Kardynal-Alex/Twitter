import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { user } from '../models/user';
import { externalAuth } from '../models/external-auth';
import { facebook } from '../models/facebook';

@Injectable({providedIn: 'root'})
export class AuthService {
    apiUrl = 'https://localhost:44318/api/account/';
    constructor(private httpClient:HttpClient,
                private localStorage:LocalStorageService,
                private jwtHelper: JwtHelperService) { }

    
    facebookLogin(facebookLogin:facebook){
        return this.httpClient.post(this.apiUrl+"facebook/",facebookLogin);
    }

    googleLogin(googleLogin:externalAuth){
        return this.httpClient.post(this.apiUrl+"google/",googleLogin);
    }

    getUserById(id:string){
        return this.httpClient.get<user>(this.apiUrl+"getUserById/"+id);
    }

    searchUsersByNameAndSurname(search:string){
        return this.httpClient.get<user[]>(this.apiUrl+"SearchUsers/?search="+search);
    }

    logout() {
        this.localStorage.remove("token");
    }

    isAuthenticated() {
        const token=this.localStorage.get("token");
        if(token && !this.jwtHelper.isTokenExpired(token))
          return true;
        else 
          return false;
    }
    
    checkIfIsAdmin(){
        const token=this.localStorage.get("token");
        if(token)
            var payload=JSON.parse(window.atob(token.split('.')[1]));
        if(payload.role.toLowerCase()==="admin")
            return true;
        return false;
    }

    getUserId():string{
        const token=this.localStorage.get("token");
        if(token)
            var payload=JSON.parse(window.atob(token.split('.')[1]));
        return payload.id;
    }

    getUserEmail():string{
        const token=this.localStorage.get("token");
        if(token)
            var payload=JSON.parse(window.atob(token.split('.')[1]));
        return payload.email;
    }   
    
    getUserFromToken():user{
        const token=this.localStorage.get("token");
        if(token)
            var payload=JSON.parse(window.atob(token.split('.')[1]));
        const user:user={
            id:payload.id,
            name:payload.name,
            surname:payload.surname,
            role:payload.role,
            email:payload.email,
            password:"",
            profileImagePath:payload.profileimagepath
        }
        return user;
    }

    getProfileImagePath():string{
        const token=this.localStorage.get("token");
        if(token)
            var payload=JSON.parse(window.atob(token.split('.')[1]));
        return payload.profileImagePath;
    }
}