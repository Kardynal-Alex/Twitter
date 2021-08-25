import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { like } from 'src/app/models/like';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { LikeService } from 'src/app/services/like.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-output-tweets',
  templateUrl: './output-tweets.component.html',
  styleUrls: ['./output-tweets.component.css']
})
export class OutputTweetsComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private toastrService:ToastrService,
              private router:Router,
              private httpClient: HttpClient,
              private likeService:LikeService) { }
  @Input() twitterPosts:twitterPost[];
  @Input() user:user;
  ngOnInit() {
    if(this.user['id'])
      this.getLikesByTokenUserId(this.user['id']);
  }
  image(){}
  deleteTwitterPost(tweet:twitterPost){
    if(confirm("Are you sure?")){
      this.twitterPostService.deleteTwitterPost(tweet).subscribe(response=>{
        this.twitterPosts=this.twitterPosts.filter(x=>x['id']!=tweet['id']);
        this.setLikesAfterUpdates();
        this.toastrService.success("Post is deleted!");
      },error=>{
        this.toastrService.error("Error!");
      });
    }
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

  togle3DotsForm(id:string){
    var x = document.getElementById('myForm-'+id);
    if(x.style.display=="none")
      x.style.display="block";
    else
      x.style.display="none";
  }
  
  createImgPath(path:string){
    return this.twitterPostService.createImgPath(path);
  }
 
  numbersOfImages=Array.from(Array(4).keys());
  @ViewChild('readOnlyTemplate', {static: false}) readOnlyTemplate: TemplateRef<any>|undefined;
  @ViewChild('editTemplate', {static: false}) editTemplate: TemplateRef<any>|undefined;
  editTwitterPost:twitterPost;
  editTwitterPostMethod(twitterPost:twitterPost){
    this.editTwitterPost=twitterPost;
  }
  
  getTwitterPostByUserId(){
    if(this.user['id'] && this.user['id']!=undefined)
    this.twitterPostService.getTwitterPostByUserId(this.user['id']).subscribe(response=>{
      this.twitterPosts=response;
    });
  }

  updateTwitterPost(){
    this.twitterPostService.updateTwitterPostWithImages(this.editTwitterPost).subscribe(response=>{
      this.editTwitterPost=null;
      this.getTwitterPostByUserId();
      this.setLikesAfterUpdates();
      this.toastrService.success("Successfully updated");
    },error=>{
      this.toastrService.error("Error!");
    })
  }

  loadTemplate(twitterPost: twitterPost) {
    if (this.editTwitterPost && this.editTwitterPost['id'] === twitterPost['id']) {
        return this.editTemplate;
    } else {
        return this.readOnlyTemplate;
    }
  }

  response;
  uploadFilesForEdit(files, field, number){
    if(files.length === 0)
      return;
    this.twitterPostService.uploadPhoto(files).subscribe(event=>{
      if (event.type === HttpEventType.Response) {
            this.response=event.body;
            document.getElementById('editbut-'+number).style.display='none';
            this.editTwitterPost['images'][field]=this.response['dbPath'];
            this.toastrService.success('Photo is uploaded!');
      }
    });
  }

  deletePhotoByPathForEdit(imagePath:string, field:string){
    if(imagePath!==''){
      this.twitterPostService.deletePhoto(imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.editTwitterPost['images'][field]='';
      });
    }
  } 

  public isEmojiPickerVisible: boolean;
  public addEmoji(event) {
    this.editTwitterPost['postText'] = `${this.editTwitterPost['postText']}${event.emoji.native}`;
    this.isEmojiPickerVisible = false;
  }


  getLikesByTokenUserId(userId:string){
    this.likeService.getLikesByUserId(userId).subscribe(response=>{
      this.likes=response;
      setTimeout(()=>{
        for(let like of response){
          var x=document.getElementById("like-"+like['twitterPostId']);
          if(x!=null)
            x.style.background="red";
        }
      },300);
    });
  }

  setLikesAfterUpdates(){
    setTimeout(()=>{
      for(let like of this.likes){
        var x=document.getElementById("like-"+like['twitterPostId']);
        if(x!=null)
          x.style.background="red";
      }
    },100);
  }

  likes:like[];
  changeHeartIconTweet(tweet:twitterPost){
    document.getElementById("like-"+tweet['id']).style.background==""?
      this.likeTweet(tweet):this.removeLikeTweet(tweet);
  }

  likeTweet(tweet:twitterPost){
    const like:like={
      id:Guid.create().toString(),
      twitterPostId:tweet['id'],
      userId:this.user['id']
    }
    tweet['like']=tweet['like']+1;
    this.twitterPostService.updateOnlyTwitterPost(tweet).subscribe(response=>{});
    this.likeService.addLike(like).subscribe(response=>{
      document.getElementById("like-"+tweet['id']).style.background="red";
      this.likes.push(like);
    });
  }

  removeLikeTweet(tweet:twitterPost){
    tweet['like']=tweet['like']-1;
    const index=this.likes.findIndex(x=>x['twitterPostId']==tweet['id']);
    const likeId=this.likes[index]['id'];
    this.twitterPostService.updateOnlyTwitterPost(tweet).subscribe(response=>{});
    this.likeService.deleteLikeById(likeId).subscribe(response=>{
      document.getElementById("like-"+tweet['id']).style.background="";
      this.likes=this.likes.filter(x=>x['id']!=likeId);
    });
  }

}
