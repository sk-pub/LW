import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchCategoryComponent } from './fetch-category/fetch-category.component';
import { ApiComponent } from './api/api.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';

import { CookieService } from 'ngx-cookie-service';
import { AddCategoryComponent } from './add-category/add-category.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchCategoryComponent,
    AddCategoryComponent,
    ApiComponent,
    LoginComponent,
    LogoutComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent },
      { path: 'logout', component: LogoutComponent },
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'fetch-category', component: FetchCategoryComponent },
      { path: 'add-category', component: AddCategoryComponent },
      { path: 'category/edit/:id', component: AddCategoryComponent },
      { path: 'api', component: ApiComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
