import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { SessionFlags } from '../_models/sessionFlags';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private http = inject(HttpClient)
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

}
