import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ChannelData } from '../_models/channelData';
import { map, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ChannelDataService {

  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;


  getAllUserChannels(): Observable<ChannelData[]>
  {
    return this.http.get<ChannelData[]>(this.baseUrl + "users/channels/all", ).pipe(
        map(data => {
          return data.map(channel => {
              return channel
            }
          )
        }
      )
    )
  }
}
