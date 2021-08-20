import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { favorite } from '../models/favorite';

@Injectable({providedIn: 'root'})
export class FavoriteService {
    apiUrl = 'https://localhost:44318/api/favorite/';
    constructor(private httpClient:HttpClient) { }

    addFavorite(favorite:favorite){
        return this.httpClient.post(this.apiUrl+"addFavorite/",favorite);
    }

    deleteFavoriteById(id:string){
        return this.httpClient.delete(this.apiUrl+"deleteFavoriteById/"+id);
    }

    getFavoritesByUserId(id:string){
        return this.httpClient.get<favorite[]>(this.apiUrl+"getFavoritesByUserId/"+id);
    }

    deleteFavoriteByTwitterPostAndUserId(favorite:favorite){
        return this.httpClient.post(this.apiUrl+"deleteFavoriteByTwitterPostAndUserId/",favorite);
    }
}