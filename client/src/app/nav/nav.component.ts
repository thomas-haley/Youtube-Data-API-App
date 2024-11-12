import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
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
    console.log("here")
    this.accountService.test();
  }

  logout(){
    this.accountService.logout();
  }
}
