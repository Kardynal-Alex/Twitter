import { Component, OnInit } from '@angular/core';
import { twitterPost } from 'src/app/models/twitter-post';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-heshtag-search',
  templateUrl: './heshtag-search.component.html',
  styleUrls: ['./heshtag-search.component.css']
})
export class HeshtagSearchComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService) { }

  ngOnInit(){
    document.getElementById('router').className="router1";
  }

  twitterPosts:twitterPost[];
  search:string="test";
  searchTweets(){
    if(this.search!=''){
      this.twitterPostService.searchTweetsByHeshTag(this.search).subscribe(response=>{
        this.twitterPosts=response;
      });
    }else{
      this.twitterPosts=null;
    }
  }
}
