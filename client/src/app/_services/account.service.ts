import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient)
  private router = inject(Router);
  baseUrl = environment.apiUrl

  currentUser = signal<User | null>(null)

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'auth/login', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  registerUser(model: any) {
    return this.http.post<User>(this.baseUrl + 'auth/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
          this.router.navigateByUrl('/contacts')
        }
      })
    )
  }

  deleteAccount(password: string) {
    return this.http.delete(this.baseUrl + 'user/delete-account/' + password);
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.router.navigateByUrl('/')
    this.currentUser.set(null);
  }
}
