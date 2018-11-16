import { Component } from '@angular/core';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [NavMenuComponent],
})

export class HomeComponent {
  constructor(private _navMenu: NavMenuComponent) { }

  loggedIn() {
    return this._navMenu.loggedIn();
  }
}
