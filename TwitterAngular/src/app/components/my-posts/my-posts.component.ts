import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { images } from 'src/app/models/images';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-my-posts',
  templateUrl: './my-posts.component.html',
  styleUrls: ['./my-posts.component.css']
})
export class MyPostsComponent implements OnInit {

  constructor(private authService:AuthService,
              private httpClient:HttpClient,
              private toastrService:ToastrService,
              private twitterPostService:TwitterPostService,
              private router:Router) { }

  numbersOfImages=Array.from(Array(4).keys());
  user:user;
  images:images={
    image1:'',image2:'',image3:'',image4:'',id:null
  };
  twitterPosts:twitterPost[];
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.user=this.authService.getUserFromToken();
    this.getTwitterPostByUserId();
  }

  getTwitterPostByUserId(){
    if(this.user['id'] && this.user['id']!=undefined)
    this.twitterPostService.getTwitterPostByUserId(this.user['id']).subscribe(response=>{
      this.twitterPosts=response;
    });
  }

  createTwitterPost(form:NgForm){
    const idGuid=Guid.create().toString();
    this.images['id']=idGuid;
    const twitterPost:twitterPost={
      id:idGuid,
      postText:form.value.postText,
      dateCreation:new Date(Date.now()),
      like:0,
      userId:this.user['id'],
      user:null,
      images:this.images,
      comments:null
    }
    this.twitterPostService.addTwitterPost(twitterPost).subscribe(response=>{
      this.toastrService.success("Post is added");
      form.resetForm();
      for(let i of this.numbersOfImages)
        this.images['image'+(i+1)]='';
        //this.twitterPosts.unshift(twitterPost);
        this.getTwitterPostByUserId();
    },error=>{
      this.toastrService.error("Something went wrong!");
    })
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

  response;
  uploadFiles(files, field, number){
    if(files.length === 0)
      return;
    this.twitterPostService.uploadPhoto(files).subscribe(event=>{
      if (event.type === HttpEventType.Response) {
            this.response=event.body;
            document.getElementById('but-'+number).style.display='none';
            this.images[field]=this.response['dbPath'];
            this.toastrService.success('Photo is uploaded!');
      }
    });
  }

  deletePhotoByPath(imagePath:string, field:string){
    if(imagePath!==''){
      this.twitterPostService.deletePhoto(imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.images[field]='';
      });
    }
  }

  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

}
