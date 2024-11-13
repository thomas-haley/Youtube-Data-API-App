import { Component, inject } from '@angular/core';
import { UserSessionService } from '../../_services/user-session.service';

@Component({
  selector: 'app-session-home',
  standalone: true,
  imports: [],
  templateUrl: './session-home.component.html',
  styleUrl: './session-home.component.css'
})
export class SessionHomeComponent {
  sessionService = inject(UserSessionService);
  constructor(){
    this.sessionService.getSessionFlags().subscribe();
  }
}
