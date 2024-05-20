import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (localStorage.getItem('token') != null) {
      const clonedReq = req.clone({
        headers: req.headers.set(
          'Authorization',
          'Bearer ' + localStorage.getItem('token')
        ),
      });
      return next.handle(clonedReq).pipe(
        tap(
          (res) => {},
          (err) => {
            if ([401, 403].includes(err.status)) {
              localStorage.removeItem('token');
              localStorage.removeItem('permissions');
              localStorage.removeItem('tenant');
              this.router.navigate(['/authentication/login']);
            }

            const error = err.error.message || err.statusText;
            return throwError(() => error);
          }
        )
      );
    } else {
      return next.handle(req.clone()).pipe(tap((res) => {}));
    }
  }
}
