import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { twitterPost } from 'src/app/models/twitter-post';
import { user } from 'src/app/models/user';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-output-tweets',
  templateUrl: './output-tweets.component.html',
  styleUrls: ['./output-tweets.component.css']
})
export class OutputTweetsComponent implements OnInit {

  constructor(private twitterPostService:TwitterPostService,
              private toastrService:ToastrService,
              private router:Router) { }
  @Input() twitterPosts:twitterPost[];
  @Input() user:user;
  ngOnInit() {
    
  }
  image(){}
  deleteTwitterPost(tweet:twitterPost){
    if(confirm("Are you sure?")){
      this.twitterPostService.deleteTwitterPost(tweet).subscribe(response=>{
        this.twitterPosts=this.twitterPosts.filter(x=>x['id']!=tweet['id']);
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
}
