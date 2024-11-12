import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { SessionHomeComponent } from './session-home/session-home.component';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SessionHomeComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  accountService = inject(AccountService);
}
