import { Component } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private cookieService: CookieService, private _router: Router) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  loggedIn() {
    return this.cookieService.check('.AspNetCore.Cookies');
  }

  logout() {
    this.cookieService.delete('.AspNetCore.Cookies');
    this._router.navigate(['/']);
  }
}
