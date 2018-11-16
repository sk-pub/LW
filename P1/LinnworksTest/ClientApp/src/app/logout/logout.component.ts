import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})

export class LogoutComponent implements OnInit {
  constructor(private router: Router, private cookieService: CookieService) { }

  ngOnInit() {
    this.logout();
  }

  logout() {
    this.cookieService.delete('.AspNetCore.Cookies');
    this.router.navigate(['/login']);
  }
}
