import {Component, DoCheck} from '@angular/core';
import {AuthService} from './services/auth.service';

@Component({selector: 'app-root', templateUrl: './app.component.html', styleUrls: ['./app.component.css']})
export class AppComponent implements DoCheck {
    title = 'TwitterAngular';
    isAuth : boolean = false;
    constructor(private authService : AuthService) {}

    ngDoCheck() {
        this.isAuth = this.authService.isAuthenticated();
    }

}
