import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule }   from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from "@auth0/angular-jwt";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SortPipe } from './pipes/sort.pipe';
import { LoginGuard } from './guards/login.guard';

import { AppComponent } from './app.component';
import { AuthPageComponent } from './components/auth-page/auth-page.component';
import { FacebookLoginComponent } from './components/facebook-login/facebook-login.component';
import { GoogleLoginComponent } from './components/google-login/google-login.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { MenuComponent } from './components/menu/menu.component';
import { MyPostsComponent } from './components/my-posts/my-posts.component';


const appRoutes: Routes =[
  { path: '', component: AuthPageComponent },
  { path: 'home', component: HomePageComponent, canActivate: [LoginGuard] },
  { path: 'myposts', component:MyPostsComponent, canActivate: [LoginGuard] },
  { path: '**', component: AppComponent }
];

export function tokenGetter() {
  return localStorage.getItem("token");
}
@NgModule({
  declarations: [
    AppComponent,
    AuthPageComponent,
    FacebookLoginComponent,
    GoogleLoginComponent,
    HomePageComponent,
    MenuComponent,
    MyPostsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),
    ToastrModule.forRoot({preventDuplicates:true}),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["localhost:4200"],
        blacklistedRoutes: []
      }
    })
  ],
  providers: [SortPipe],
  bootstrap: [AppComponent],
  schemas : [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
//ng serve --ssl --ssl-cert "D:\\Asp.Net(project)\\Twitter\\TwitterAngular\\localhost.crt" --ssl-key "D:\\Asp.Net(project)\\Twitter\\TwitterAngular\\localhost.key"