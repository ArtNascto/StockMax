import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (
      localStorage.getItem('token') != null &&
      !this.isTokenExpired(localStorage.getItem('token') ?? '')
    ) {
      return true;
    } else {
      this.router.navigate(['/auth']);
      return false;
    }
  }
  isTokenExpired(token: string): boolean {
    if (token == '') return true;
    const decodedToken: any = jwtDecode(token);
    const currentTime = Date.now() / 7200; // Em segundos
    return decodedToken.exp < currentTime;
  }
}
