import { Component, inject, Input, signal } from '@angular/core';
import { UserSessionService } from '../../../_services/user-session.service';
import { AccountService } from '../../../_services/account.service';
import { VideoDataService } from '../../../_services/video-data.service';
import { UserVideoData } from '../../../_models/userVideoData';
import { TableHeader } from '../../../_models/tableHeader';
import { VideoListItemComponent } from "./video-list-item/video-list-item.component";
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [VideoListItemComponent, CommonModule],
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.css'
})
export class VideoListComponent {
  @Input() listType!: string;
  accountService = inject(AccountService);
  userSessionService = inject(UserSessionService);
  videoDataService = inject(VideoDataService);
  testSrc = signal<string | null>(null);
  pageNumber = 1;
  pageSize = 5;
  defaultHeaders = [
    {name: "Date Watched", visibleInHeader: true, location: "watched"} as TableHeader,
    {name: "Video Thumbnail", visibleInHeader: false, location: "video.thumbnail"} as TableHeader,
    {name: "Video Title", visibleInHeader: true, location: "video.title"} as TableHeader,
    {name: "Channel Thumbnail", visibleInHeader: false, location: "video.channel.thumbnail"} as TableHeader,
    {name: "Channel ", visibleInHeader: true, location: "video.channel.title"} as TableHeader
  ];
  tableHeaders = signal<TableHeader[] | null>(null);

  ngOnInit()
  {
    
    if(!this.videoDataService.paginatedResult()) this.listType == "user-videos" ? this.loadUserVideo() : null;
    if(!this.tableHeaders()) this.tableHeaders.set(this.defaultHeaders); //eventually get last used headers
  }

  loadUserVideo(){
    this.videoDataService.getUserVideos(this.userSessionService.sessionFlags()!.id, this.pageNumber, this.pageSize);
  }
  test2(){
    console.log("here");
  }

  stringifyHeaders(): string
  {
    var test = JSON.stringify(this.tableHeaders());
    return test;
  }

  getLastPathKey(path: string)
  {
    const keys = path.split(".");
    return keys[keys.length - 1];
  }

  getNestedValue(row: any, path: string)
  {
    const keys = path.split(".");
    if(this.videoDataService.paginatedResult()!.items != null){
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

  getThumbnailCSSClass(path: string){
    const keys = path.split(".");
    return keys.indexOf("channel") == -1 ? "video-thumbnail" : "channel-thumbnail";
  }

  test(){
    // var test = [];
    // test.find
    // console.log(this.videoDataService.paginatedResult().items![0].find(h => h.field == ""));
  }
}
