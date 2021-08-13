import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor(private authService:AuthService) { }
  @Input() isAuth:boolean;
  ngOnInit() {
    this.isAuth=this.authService.isAuthenticated();
  }

}
