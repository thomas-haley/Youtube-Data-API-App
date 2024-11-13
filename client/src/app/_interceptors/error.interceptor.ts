import { HttpEventType, HttpHandlerFn, HttpHeaders, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';
import { catchError, Observable, tap } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const router = inject(Router);
    const toaster = inject(ToastrService);

    return next(req).pipe(
        catchError(error => {
            if(error){
                switch (error.status) {
                    case 400:
                        if(error.error.errors){
                            const modalStateErrors = [];
                            for (const key in error.error.errors){
                                if(error.error.errors[key]){
                                    modalStateErrors.push(...error.error.errors[key])
                                }
                            }
                            for(var error of modalStateErrors){
                                toaster.error(error);
                            }
                        } else {
                            toaster.error(error.error, error.status);
                        }
                        
                        break;
                    case 401:
                        toaster.error("Unauthorized", error.status);
                        break;
                    case 404:
                        router.navigateByUrl("404");
                        break;
                    case 500:
                        const navigationExtras: NavigationExtras = {state: {error: error.error}}
                        router.navigateByUrl("/server-error", navigationExtras);
                        break;
                    default:
                        toaster.error("Something unexpected went wrong");
                        break;
                }
            }
            throw error;
        })
    )
}