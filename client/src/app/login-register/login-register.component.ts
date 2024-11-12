import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

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
  login(){
    console.log(this.model)
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.router.navigateByUrl("");
      },
      error: error => alert(error)
    })
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response)
      },
      error: error => alert(error)
    })
  }
}
