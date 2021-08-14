import { Component, Input, OnInit } from '@angular/core';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-output-tweets',
  templateUrl: './output-tweets.component.html',
  styleUrls: ['./output-tweets.component.css']
})
export class OutputTweetsComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService) { }
  @Input() twitterPosts:twitterPost[];
  @Input() user:user;
  ngOnInit() {
    
  }

 

  createImgPath(path:string){
    return this.twitterPostService.createImgPath(path);
  }
}
