import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { UserVideoData } from '../_models/userVideoData';
import { map } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { VideoData } from '../_models/videoData';
import { TableHeader } from '../_models/tableHeader';

@Injectable({
  providedIn: 'root'
})
export class VideoDataService {
  possibleHeaders = [
    {name: "Date Watched", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-date", location: "watched"} as TableHeader,
    {name: "Video Thumbnail", visibleInHeader: false, hasHref: true, href: (model: any) => {return "https://www.youtube.com/watch?v=" + this.getNestedValue(model, "video.apI_Id")}, customHeaderCSSClass:"", customDataCSSClass:"table-video-image-lg", location: "video.thumbnail"} as TableHeader,
    {name: "Video Title", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all", location: "video.title"} as TableHeader,
    {name: "Channel Thumbnail", visibleInHeader: false, hasHref: true, href: (model: any) => {return "https://www.youtube.com/channel/" + this.getNestedValue(model, "video.channel.apI_Id")}, customHeaderCSSClass:"", customDataCSSClass:"table-channel-img-sm", location: "video.channel.thumbnail"} as TableHeader,
    {name: "Channel", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.channel.title"} as TableHeader
  ]


  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // userVideoData = signal<UserVideoData[] | null>(null);
  paginatedResult = signal<PaginatedResult<UserVideoData[]>  | null>(null);
  pageNumber = signal<number>(1);
  pageSize = signal<number>(5);

  defaultHeaders = [this.possibleHeaders[0], this.possibleHeaders[1], this.possibleHeaders[2], this.possibleHeaders[3], this.possibleHeaders[4]];
  tableHeaders = signal<TableHeader[]>(this.defaultHeaders);




  getUserVideos(id: number){
    let params = new HttpParams();

    params = params.append('pageNumber', this.pageNumber()).append('pageSize',  this.pageSize());
    


    return this.http.get<UserVideoData[]>(this.baseUrl + "users/"+ id +"/videos", {observe: "response", params}).subscribe({
      next: response => {
        this.paginatedResult.set({
          items: response.body as UserVideoData[],
          pagination: JSON.parse(response.headers.get("Pagination")!)
        })
      }
    })
  }


  getNestedValue(row: any, path: string)
  {
    const keys = path.split(".");
    if(this.paginatedResult()!.items != null){
      let value = row;
      for(let key of keys)
      {

        if(value) {
          value = value[key];
        } else {
          return null
        }
      }
      return value;
    }
    return null;
  }

}
