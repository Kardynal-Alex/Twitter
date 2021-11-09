import {HttpEventType} from '@angular/common/http';
import {Component, Input, OnInit} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {user} from 'src/app/models/user';
import {AuthService} from 'src/app/services/auth.service';
import {TwitterPostService} from 'src/app/services/twitter-post.service';

@Component({selector: 'app-edit-profile', templateUrl: './edit-profile.component.html', styleUrls: ['./edit-profile.component.css']})
export class EditProfileComponent implements OnInit {

    constructor(private authService : AuthService, private toastrService : ToastrService, private twitterPostService : TwitterPostService) {}

    @Input()user : user;
    ngOnInit() { // document.getElementById("editProfile").style.display="block";
        this.oldImageProfilePath = this.user['profileImagePath'];
    }

    edituserProfile() {
        this.authService.updateUserProfile(this.user).subscribe(response => {
            document.getElementById("editProfile").style.display = "none";
            window.location.reload();
            this.toastrService.success("Profile is updated!");
        });
    }

    imageProfilePath : string = '';
    oldImageProfilePath : string = '';
    response;
    uploadProfileImage(files) {
        if (files.length === 0) 
            return;
        
        if (this.imageProfilePath != '') {
            this.deletePhotoByPath(this.imageProfilePath);
        }
        this.twitterPostService.uploadPhoto(files).subscribe(event => {
            if (event.type === HttpEventType.Response) {
                this.response = event.body;
                this.imageProfilePath = this.response['dbPath'];
                this.user['profileImagePath'] = "https://localhost:44318/" + this.response['dbPath'];
                this.toastrService.success('Photo is uploaded!');
            }
        });
    }

    deletePhotoByPath(imagePath : string) {
        if (imagePath !== '') {
            this.twitterPostService.deletePhoto(imagePath).subscribe(response => {
                this.user['profileImagePath'] = this.oldImageProfilePath;
                this.imageProfilePath = '';
            });
        }
    }

}
