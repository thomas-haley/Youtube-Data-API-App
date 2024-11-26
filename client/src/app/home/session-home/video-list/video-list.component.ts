import { Component, inject, Input, signal } from '@angular/core';
import { UserSessionService } from '../../../_services/user-session.service';
import { AccountService } from '../../../_services/account.service';
import { VideoDataService } from '../../../_services/video-data.service';
import { UserVideoData } from '../../../_models/userVideoData';
import { TableHeader } from '../../../_models/tableHeader';
import { VideoListItemComponent } from "./video-list-item/video-list-item.component";
import { CommonModule } from '@angular/common';
import { PaginationComponent } from './pagination/pagination.component';
import { EditableTableHeaderComponent } from "./editable-table-header/editable-table-header.component";
@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [PaginationComponent, CommonModule, EditableTableHeaderComponent],
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.css'
})


export class VideoListComponent {
  @Input() listType!: string;
  accountService = inject(AccountService);
  userSessionService = inject(UserSessionService);
  videoDataService = inject(VideoDataService);
  allowHeaderEdit = signal<boolean>(false);

  ngOnInit()
  {
    if(!this.videoDataService.paginatedResult()) this.listType == "user-videos" ? this.loadUserVideo() : null;
    if(!this.videoDataService.tableHeaders()) this.videoDataService.tableHeaders.set(this.videoDataService.defaultHeaders); //eventually get last used headers
  }

  loadUserVideo(){
    this.videoDataService.getUserVideos(this.userSessionService.sessionFlags()!.id);
  }

  stringifyHeaders(): string
  {
    var test = JSON.stringify(this.videoDataService.tableHeaders());
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

  toggleHeaderEditFlag()
  {
    this.allowHeaderEdit.set(!this.allowHeaderEdit());
  }

  parseTopic(topicURL: string)
  {
    console.log(topicURL);
    var splitTopic = topicURL.split("/");
    console.log(splitTopic);
    var topic = splitTopic[splitTopic.length - 1];
    console.log(topic);
    topic = topic.replace("_", " ")
    return topic;
  }

}
