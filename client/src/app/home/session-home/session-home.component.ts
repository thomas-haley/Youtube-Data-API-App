import { Component, inject } from '@angular/core';
import { UserSessionService } from '../../_services/user-session.service';
import { FileUploadComponent } from "./file-upload/file-upload.component";
import { VideoListComponent } from "./video-list/video-list.component";

@Component({
  selector: 'app-session-home',
  standalone: true,
  imports: [FileUploadComponent, VideoListComponent],
  templateUrl: './session-home.component.html',
  styleUrl: './session-home.component.css'
})
export class SessionHomeComponent {
  sessionService = inject(UserSessionService);
  constructor(){
    this.sessionService.getSessionFlags().subscribe();
  }
}
