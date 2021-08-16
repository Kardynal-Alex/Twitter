import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { twitterPost } from 'src/app/models/twitter-post';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-view-tweet',
  templateUrl: './view-tweet.component.html',
  styleUrls: ['./view-tweet.component.css']
})
export class ViewTweetComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private activateRoute:ActivatedRoute) { }

  id:string;
  twitterPost:twitterPost;
  ngOnInit(){
    document.getElementById('router').className="router1";
    this.id=this.activateRoute.snapshot.params['id'];
    this.twitterPostService.getTwitterPostByIdWithDetails(this.id).subscribe(response=>{
      this.twitterPost=response;
    });
  }

  public createImgPath(serverPath: string){
    return this.twitterPostService.createImgPath(serverPath);
  }

}
