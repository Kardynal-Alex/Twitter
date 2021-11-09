import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {user} from 'src/app/models/user';
import {AuthService} from 'src/app/services/auth.service';

@Component({selector: 'app-instant-search', templateUrl: './instant-search.component.html', styleUrls: ['./instant-search.component.css']})
export class InstantSearchComponent implements OnInit {

    constructor(private authService : AuthService, private router : Router) {}

    ngOnInit() {}

    search : string = '';
    users : user[];
    instantSearch() {
        if (this.search != '') {
            this.authService.searchUsersByNameAndSurname(this.search).subscribe(response => {
                this.users = response;
            });
        } else {
            this.users = null;
        }
    }

    navigateToUserProfile(id : string) {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';
        this.router.navigate(['user-profile/' + id]);
    }

}
