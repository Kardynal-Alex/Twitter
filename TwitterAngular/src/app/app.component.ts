import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TwitterAngular';
  isAuth:boolean=false;
  constructor(private authService:AuthService){ }

  ngDoCheck(): void {
    this.isAuth=this.authService.isAuthenticated();
  }
}
