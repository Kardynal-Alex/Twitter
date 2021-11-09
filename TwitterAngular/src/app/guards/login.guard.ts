import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {tokenAuth} from '../models/token-auth';
import {AuthService} from '../services/auth.service';
import {LocalStorageService} from '../services/local-storage.service';

@Injectable({providedIn: 'root'})
export class LoginGuard implements CanActivate {
    constructor(private authService : AuthService, private router : Router, private httpClient : HttpClient, private jwtHelper : JwtHelperService, private localStorage : LocalStorageService) {}

    canActivate() {
        const token = localStorage.getItem("token");
        if (token || token != undefined) {
            if (token && !this.jwtHelper.isTokenExpired(token)) 
                return true;
            

            const isRefreshSuccess = this.tryRefreshToken(token);
            if (! isRefreshSuccess) 
                this.router.navigate(['']);
            
            return isRefreshSuccess;
        } else {
            return false;
        }
    }

    tryRefreshToken(token : string): boolean {
        console.log("refresh");
        const apiUrl = 'https://localhost:44318/api/account/';
        const refreshToken: string = this.localStorage.get("refreshToken");
        if (!token || ! refreshToken) 
            return false;
        
        var tokenAuth: tokenAuth = {
            token: token.toString(),
            refreshToken: refreshToken.toString()
        }

        let isRefreshSuccess: boolean;
        try {
            this.httpClient.post(apiUrl + "refreshToken/", tokenAuth).subscribe(response => {
                const newToken = response['token'];
                const newRefreshToken = response['refreshToken'];
                this.localStorage.set("token", newToken);
                this.localStorage.set("refreshToken", newRefreshToken);
            });

            isRefreshSuccess = true;
        } catch (ex) {
            isRefreshSuccess = false;
            console.log("error");
        }
        return isRefreshSuccess;
    }

}
