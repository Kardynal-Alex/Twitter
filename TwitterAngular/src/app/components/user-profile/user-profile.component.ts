import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private activateRoute:ActivatedRoute,
              private authService:AuthService) { }

  backgroungImage:string="Resources\\Images\\twitterImage.jpg";
  id:string;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.id=this.activateRoute.snapshot.params['id'];
    this.getTwitterPostByUserIdWithImagesAndUsers(this.id);
    this.getUserById(this.id);
  }

  twitterPosts:twitterPost[];
  getTwitterPostByUserIdWithImagesAndUsers(id:string){
    this.twitterPostService.getTwitterpostByUserIdWithImagesAndUsers(id).subscribe(response=>{
      this.twitterPosts=response;
    });
  }

  user:user;
  getUserById(id:string){
    this.authService.getUserById(id).subscribe(response=>{
      this.user=response;
    });
  }  

  createImgPath(path:string){
    return this.twitterPostService.createImgPath(path);
  }

}
