import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { SessionFlags } from '../_models/sessionFlags';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private http = inject(HttpClient)
  baseUrl = environment.apiUrl;
  


  getSessionFlags(){
    return this.http.get<SessionFlags>(this.baseUrl + "users/getFlags")
  }

}
