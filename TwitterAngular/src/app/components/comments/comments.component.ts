import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { comment } from 'src/app/models/comment';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { CommentService } from 'src/app/services/comment.service';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(private toastrService:ToastrService,
              private commentService:CommentService,
              private authService:AuthService,
              private twitterPostService:TwitterPostService) { }

  ngOnInit(){
    this.user=this.authService.getUserFromToken();
  }

  @Input() comments:comment[];
  @Input() id:string;
  @Input() twitterPost:twitterPost;
  user:user;
  getCommentByTweetId(){
    this.commentService.getCommentsByTwitterPostId(this.id).subscribe(response=>{
      this.comments=response;
    })
  }

  addComment(form:NgForm){
    if(this.textArea.length>=2){
      const comment:comment={
        id:Guid.create().toString(),
        author:this.user['name']+" "+this.user['surname'],
        text:this.textArea,
        dateCreation:new Date(Date.now()),
        twitterPostId:this.id,
        userId:this.user['id'],
        profileImagePath:this.user['profileImagePath']
      }
      this.commentService.addComment(comment).subscribe(response=>{
        this.twitterPost['nComments']+=1;
        this.twitterPostService.updateOnlyTwitterPost(this.twitterPost).subscribe();
        this.getCommentByTweetId();
        this.toastrService.success("Comment is replied");
        form.resetForm();
      },error=>{
        this.toastrService.error("Something went wrong!");
      });
    }
    else{
      this.toastrService.info("Min 2 symbols is required");
    }
  }

  deleteComment(commentId:string){
    this.commentService.deleteCommentById(commentId).subscribe(response=>{
      this.comments=this.comments.filter(x=>x['id']!=commentId);
      this.twitterPost['nComments']-=1;
      this.twitterPostService.updateOnlyTwitterPost(this.twitterPost).subscribe();
    });
  }

  public textArea: string = '';
  public isEmojiPickerVisible: boolean;
  public addEmoji(event) {
     this.textArea = `${this.textArea}${event.emoji.native}`;
     this.isEmojiPickerVisible = false;
  }

}
