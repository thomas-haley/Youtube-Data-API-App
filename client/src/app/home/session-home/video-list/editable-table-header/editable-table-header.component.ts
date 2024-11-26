import { Component, effect, inject, Input, signal } from '@angular/core';
import { VideoDataService } from '../../../../_services/video-data.service';
import { TableHeader } from '../../../../_models/tableHeader';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-editable-table-header',
  standalone: true,
  imports: [],
  templateUrl: './editable-table-header.component.html',
  styleUrl: './editable-table-header.component.css'
})
export class EditableTableHeaderComponent {
  @Input() headerIndexString!: string;
  toaster = inject(ToastrService);
  videoService = inject(VideoDataService);
  headerIndex = signal<number | null>(null);
  headerData = signal<TableHeader | null>(null);

  //Required to have headers update when a different one is destroyed
  headerUpdateEffect = effect(
    () => {
      this.fetchHeaderData();
    },
    {
      allowSignalWrites: true
    }
  )

  ngOnInit()
  {
    this.fetchHeaderData()
  }

  isSameHeader(possibleHeader: TableHeader): boolean
  {
    this.headerUpdateEffect;
    return possibleHeader == this.headerData();
  }

  fetchHeaderData()
  {
    console.log("fetching header data");
    this.headerIndex.set(Number.parseInt(this.headerIndexString));
    this.headerData.set(this.videoService.tableHeaders()![this.headerIndex()!]);
  }

  updateHeaderColumn(event: any)
  {
    var selectedHeaderIndex = Number.parseInt(event.target.value)
    var selectedHeader = this.videoService.possibleHeaders[selectedHeaderIndex];

    if(selectedHeader == null) return;
    var currentHeaderClone = this.videoService.tableHeaders();
    currentHeaderClone![this.headerIndex()!] = selectedHeader;
    this.videoService.tableHeaders.set(currentHeaderClone);
    // console.log(`Should update header #${this.headerIndex()} from ${this.headerData()?.name} to ${selectedHeader.name}`);


  }

  removeHeaderColumnWrapper(index: number)
  {
    this.videoService.removeHeaderColumn(this.headerIndex()!)
    this.fetchHeaderData();
  }

  debug(){
    console.log(this.headerData());
  }


  moveHeaderLeft()
  {
    if(this.headerIndex() == 0)
    {
      this.toaster.show("Can't move column any more left");
      return;
    }
    this.videoService.moveHeaderFromTo(this.headerIndex()!, this.headerIndex()! - 1);

  }

  moveHeaderRight()
  {
    if(this.headerIndex()! + 1 == this.videoService.tableHeaders()!.length)
      {
        this.toaster.show("Can't move column any more right");
        return;
      }
      this.videoService.moveHeaderFromTo(this.headerIndex()!, this.headerIndex()! + 1);
  }
  
}
