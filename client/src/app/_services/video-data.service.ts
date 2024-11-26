import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { UserVideoData } from '../_models/userVideoData';
import { from, map } from 'rxjs';
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
    {name: "Channel", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.channel.title"} as TableHeader,
    {name: "Published", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.published"} as TableHeader,
    {name: "Category Id", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.category.apI_Id"} as TableHeader,
    {name: "Duration", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.duration"} as TableHeader,
    {name: "Views", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.views"} as TableHeader,
    {name: "Data Fetched Date", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.dataFetched"} as TableHeader,
    {name: "Topics", visibleInHeader: true, customHeaderCSSClass:"", customDataCSSClass:"table-text-center-all",  location: "video.topics"} as TableHeader
  ]


  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // userVideoData = signal<UserVideoData[] | null>(null);
  paginatedResult = signal<PaginatedResult<UserVideoData[]>  | null>(null);
  pageNumber = signal<number>(1);
  pageSize = signal<number>(5);

  defaultHeaders = [this.possibleHeaders[0], this.possibleHeaders[1], this.possibleHeaders[2], this.possibleHeaders[3], this.possibleHeaders[4]];
  tableHeaders = signal<TableHeader[] | null>(this.defaultHeaders);




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


  addHeaderColumn()
  {
    if(this.tableHeaders() != null)
    {
      var currentHeaderClone = this.tableHeaders()!;
      //Always appends first default header to end of header list
      currentHeaderClone.push(this.defaultHeaders[0]);
      this.tableHeaders.set(currentHeaderClone);
    }
  }


  removeHeaderColumn(index: number)
  {
    if(this.tableHeaders() != null)
    {
      var currentHeaderClone = [...this.tableHeaders()!];
      if(index < 0 || index + 1 > currentHeaderClone.length) return;
      console.log(`deleting index ${index}`)
      console.log(currentHeaderClone);
      var removed = currentHeaderClone.splice(index, 1);
      console.log(`removed:`);
      console.log(removed)
      this.tableHeaders.set(null);
      this.tableHeaders.set(currentHeaderClone);



    }
    
  }


  moveHeaderFromTo(fromIndex: number, toIndex: number)
  {
    //fromIndex or toIndex out of bounds for tableHeaders()
    if(fromIndex < 0 || fromIndex + 1 > this.tableHeaders()!.length || toIndex < 0 || toIndex + 1 > this.tableHeaders()!.length) return;

    var clonedHeaders = [...this.tableHeaders()!];

    var elementToMove = clonedHeaders.splice(fromIndex, 1)[0];

    clonedHeaders.splice(toIndex, 0, elementToMove);

    this.tableHeaders.set(clonedHeaders);
  }
}
