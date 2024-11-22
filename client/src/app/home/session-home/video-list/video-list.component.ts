import { Component, inject, signal } from '@angular/core';
import { UserSessionService } from '../../../_services/user-session.service';
import { AccountService } from '../../../_services/account.service';


@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [],
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.css'
})
export class VideoListComponent {
  accountService = inject(AccountService);
  userSessionService = inject(UserSessionService);
  testSrc = signal<string | null>(null);
  ngOnInit()
  {
    this.userSessionService.getUserVideos(this.userSessionService.sessionFlags()!.id).subscribe();
  }

  test(){
    console.log(this.userSessionService.userVideoData());
    this.testSrc.set(this.userSessionService.userVideoData()![0].video.thumbnail);
  }
}
