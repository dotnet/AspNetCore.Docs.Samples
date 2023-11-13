import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";

// this will intercept all http requests and redirect to signin if the user is not authenticated and
// is trying to access a protected route
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(tap({
      next: (event: HttpEvent<any>) => {
        if (event instanceof HttpErrorResponse) {
          if (event.status !== 401 || (event.url && event.url.indexOf("manage/info") >= 0)) {
            return;
          }
          this.router.navigate(['signin']);
        }
      }
    }));
  }
}
