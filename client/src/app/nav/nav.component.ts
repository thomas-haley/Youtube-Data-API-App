import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ErrorService } from '../_services/error.service';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  errorService = inject(ErrorService);
  private router = inject(Router);
  model: any = {};
  login(){
    this.accountService.loginMode.set(true);
    this.router.navigateByUrl("login-register")
  }

  register(){
    this.accountService.loginMode.set(false);
    this.router.navigateByUrl("login-register")
  }

  test(){
    this.errorService.queueTest();
  }

  logout(){
    this.accountService.logout();
  }
}
