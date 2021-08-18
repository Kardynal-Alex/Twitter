import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { user } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.component.html',
  styleUrls: ['./search-users.component.css']
})
export class SearchUsersComponent implements OnInit {

  constructor(private authService:AuthService,
              private router:Router) { }

  ngOnInit(){
    document.getElementById('router').className="router1";
  }

  search:string="kardynal";
  users:user[];
  searchUsers(){
    this.authService.searchUsersByNameAndSurname(this.search).subscribe(response=>{
      this.users=response;
    });
  }

  navigateToUserProfile(id:string){
    this.router.navigate(['user-profile/'+id]);
  }

}
