import { Component, Input, OnInit } from '@angular/core';
import { comment } from 'src/app/models/comment';
import { twitterPost } from 'src/app/models/twitter-post';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService) { }

  comments:comment[];
  ngOnInit(){
  }

  @Input() id:string;
  getCommentByTweetId(){

  }

}
