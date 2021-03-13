import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { StatusCodes, getReasonPhrase } from 'http-status-codes';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error){
          switch (error.status){
            case StatusCodes.BAD_REQUEST: 
              if(error.error.errors){
                let errors = error.error.errors;
                const modalStateErrors = [];
                for(const key in errors){
                  modalStateErrors.push(errors[key]);
                }
                throw modalStateErrors.flat();
              } 
              else if (typeof(error.error) === "object"){
                this.toastr.error(getReasonPhrase(StatusCodes.BAD_REQUEST), error.status);
              }
              else{
                this.toastr.error(error.error, error.status);
              }
              break;
            case StatusCodes.UNAUTHORIZED:
              this.toastr.error(getReasonPhrase(StatusCodes.UNAUTHORIZED), error.status);
              break;
            case StatusCodes.NOT_FOUND:
              this.router.navigateByUrl('/not-found');
              break; 
            case StatusCodes.INTERNAL_SERVER_ERROR:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;    
            default:
              this.toastr.error("Something unexpected went wrong");
              break; 
          }
        }
        return throwError(error);
      }) 
    );
  }
}
