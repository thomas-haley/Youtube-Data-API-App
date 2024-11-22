import { Component, inject, Input, signal } from '@angular/core';
import { VideoDataService } from '../../../../_services/video-data.service';
import { UserVideoData } from '../../../../_models/userVideoData';
import { TableHeader } from '../../../../_models/tableHeader';

@Component({
  selector: 'app-video-list-item',
  standalone: true,
  imports: [],
  templateUrl: './video-list-item.component.html',
  styleUrl: './video-list-item.component.css'
})
export class VideoListItemComponent {

  @Input() stringIndex!: string;
  @Input() headersString!: string;
  videoDataService = inject(VideoDataService);
  userVideoData = signal<UserVideoData | null>(null);
  dataIndex = -1;
  tableHeaders = signal<TableHeader[] | null>(null);


  ngOnInit()
  {

    this.dataIndex = parseInt(this.stringIndex)
  
    this.tableHeaders.set(JSON.parse(this.headersString) as TableHeader[]);
    if(this.dataIndex != -1){
      this.userVideoData.set(this.videoDataService.paginatedResult()!.items![this.dataIndex] as UserVideoData);
    }
  }

  getLastPathKey(path: string)
  {
    const keys = path.split(".");
    return keys[keys.length - 1];
  }


}
