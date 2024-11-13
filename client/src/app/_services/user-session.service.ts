import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { SessionFlags } from '../_models/sessionFlags';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private http = inject(HttpClient)
  toaster = inject(ToastrService);
  baseUrl = environment.apiUrl;
  sessionFlags = signal<SessionFlags | null>(null);

  getSessionFlags<SessionFlags>(){
    console.log("getting flags")
    return this.http.get<SessionFlags>(this.baseUrl + "session/flags").pipe(
      map(flags => {
        console.log(flags);
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

}
