import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { SessionFlags } from '../_models/sessionFlags';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { UserVideoData } from '../_models/userVideoData';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private http = inject(HttpClient)
  toaster = inject(ToastrService);
  baseUrl = environment.apiUrl;
  sessionFlags = signal<SessionFlags | null>(null);
  allowUpload = signal<boolean>(false);
  dataUploaded = signal<boolean>(false);
  userVideoData = signal<UserVideoData[] | null>(null);
  getSessionFlags(){

    return this.http.get<SessionFlags>(this.baseUrl + "session/flags").pipe(
      map(sessionFlags => {
          if(sessionFlags){
            this.sessionFlags.set(sessionFlags);
          }
          
      })
    )
  }

  postWatchHistory(model: any){
    return this.http.post(this.baseUrl + "users/upload-watch-history", model).subscribe(
      {
        next: response => this.toaster.show("File uploaded"),
        error: error => this.toaster.error(error),
        complete: () => this.getSessionFlags()
      }
    ) 
  }


  getUserVideos(id: number){
    return this.http.get<UserVideoData[]>(this.baseUrl + "users/"+ id +"/videos").pipe(
      map(userVideoData => {
        if(userVideoData){
          this.userVideoData.set(userVideoData);
        }
      })
    )
  }

  

}
