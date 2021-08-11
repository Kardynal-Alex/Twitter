import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule }   from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from "@auth0/angular-jwt";

import { SortPipe } from './pipes/sort.pipe';

import { AppComponent } from './app.component';


const appRoutes: Routes =[
  { path: '', component: AppComponent },
  { path: '**', component: AppComponent }
];

export function tokenGetter() {
  return localStorage.getItem("token");
}
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
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