import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  token: string;
  errors: string[];
  isRequesting: boolean;

  constructor(private http: HttpClient, private router: Router) { }

  login({ value, valid }: { value: { token: string }, valid: boolean }) {
    this.isRequesting = true;
    if (valid) {
      this.http.post('/api/auth/login', { token: value.token })
        .subscribe(
          result => {
            if (result) {
              this.router.navigate(['/fetch-category']);
            }
          },
          error => {
            let keys = Object.keys(error.error);
            let values = [];
            for (let prop of keys) {
              values.push(error.error[prop]);
            }
            this.errors = values;
          }
        )
        .add(() => this.isRequesting = false);
    }
  }
}
