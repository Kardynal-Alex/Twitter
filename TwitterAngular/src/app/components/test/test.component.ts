import { HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { images } from 'src/app/models/images';
import { TwitterPostService } from 'src/app/services/twitter-post.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  httpClient: any;

  constructor(private toastrService:ToastrService,
              private twitterService:TwitterPostService) { }

  ngOnInit(): void {
    document.getElementById('router').className="router1";
  }

  images:images={
    image1:'',image2:'',image3:'',image4:'',id:null
  };
  createLot(form:NgForm){
  }

  response;
  upload(files, field,number){
    
    this.twitterService.uploadPhoto(files).
    subscribe(event=>
    {
      if (event.type === HttpEventType.Response) {
            this.response=event.body; 
            console.log(this.response['dbPath']);
            //document.getElementById('but-'+number).style.display='none';
            this.images[field]=this.response['dbPath'];
            console.log("path",this.response['dbPath']);
            this.toastrService.success('Photo is uploaded!');
      }
    });
  }
  deletePhoto(imagePath:string,field:string){
    if(imagePath!==''){
      this.twitterService.deletePhoto(imagePath).subscribe(response=>{
        this.toastrService.success("Photo is deleted");
        this.images[field]='';
        //document.getElementById('but-'+number).style.display='block';
      });
    }
  }

  numbers=Array.from(Array(4).keys());

  public createImgPath(serverPath: string){
    return this.twitterService.createImgPath(serverPath);
  }

  
  
}
