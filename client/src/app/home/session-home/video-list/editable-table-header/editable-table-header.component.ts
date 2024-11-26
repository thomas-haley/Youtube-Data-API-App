import { Component, inject, Input, signal } from '@angular/core';
import { VideoDataService } from '../../../../_services/video-data.service';
import { TableHeader } from '../../../../_models/tableHeader';

@Component({
  selector: 'app-editable-table-header',
  standalone: true,
  imports: [],
  templateUrl: './editable-table-header.component.html',
  styleUrl: './editable-table-header.component.css'
})
export class EditableTableHeaderComponent {
  @Input() headerIndexString!: string;
  videoService = inject(VideoDataService);
  headerIndex = signal<number | null>(null);
  headerData = signal<TableHeader | null>(null);

  ngOnInit()
  {
    this.headerIndex.set(Number.parseInt(this.headerIndexString));
    this.headerData.set(this.videoService.tableHeaders()[this.headerIndex()!]);
    console.log(this.headerData());
  }
  
}
