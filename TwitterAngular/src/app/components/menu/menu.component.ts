import {Component, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from 'src/app/services/auth.service';

@Component({selector: 'app-menu', templateUrl: './menu.component.html', styleUrls: ['./menu.component.css']})
export class MenuComponent implements OnInit {

    constructor(private authService : AuthService, private router : Router) {}
    @Input()isAuth : boolean;
    id : string;
    ngOnInit() {
        this.isAuth = this.authService.isAuthenticated();
        this.id = this.authService.getUserId();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['']);
    }

}
