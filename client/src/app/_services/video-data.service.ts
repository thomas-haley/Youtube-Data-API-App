import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { UserVideoData } from '../_models/userVideoData';
import { map } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { VideoData } from '../_models/videoData';

@Injectable({
  providedIn: 'root'
})
export class VideoDataService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // userVideoData = signal<UserVideoData[] | null>(null);
  paginatedResult = signal<PaginatedResult<UserVideoData[]>  | null>(null);


  getUserVideos(id: number, pageNumber: number, pageSize?: number){
    let params = new HttpParams();

    if(pageNumber && pageSize){
      params = params.append('pageNumber',pageNumber).append('pageSize', pageSize);
    }


    return this.http.get<UserVideoData[]>(this.baseUrl + "users/"+ id +"/videos", {observe: "response", params}).subscribe({
      next: response => {
        this.paginatedResult.set({
          items: response.body as UserVideoData[],
          pagination: JSON.parse(response.headers.get("Pagination")!)
        })
      }
    })
  }


}
