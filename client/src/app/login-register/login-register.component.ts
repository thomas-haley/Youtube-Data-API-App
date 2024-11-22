import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login-register.component.html',
  styleUrl: './login-register.component.css'
})
export class LoginRegisterComponent {
  accountService = inject(AccountService)
  model: any = {};
  private router = inject(Router);
  private toaster = inject(ToastrService);
  login(){
    console.log(this.model)
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.router.navigateByUrl("dash");
      },
      error: error => this.toaster.error(error.error)
    })
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response)
      },
      error: error => this.toaster.error(error.error)
    })
  }
}
