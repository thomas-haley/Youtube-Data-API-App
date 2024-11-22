import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  const toaster = inject(ToastrService);
  toaster.clear();
  if(accountService.currentUser()){
    let clone = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.currentUser()?.token}`
      }
    });
    return next(clone);
  }
  return next(req);
};
