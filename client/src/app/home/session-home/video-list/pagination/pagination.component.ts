import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { VideoDataService } from '../../../../_services/video-data.service';
@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Output() onPaginationDataUpdate: EventEmitter<any> = new EventEmitter();
  @Output() onTableSettingsClick: EventEmitter<any> = new EventEmitter();
  @Output() onFilterButtonClick: EventEmitter<any> = new EventEmitter();
  @Input() updateDataCallback!: Function;
  @Input() isEditingTable!: boolean;
  videoDataService = inject(VideoDataService);

  resultsPerPageOpts = [5, 10, 25, 50, 100];

  calcVideosDisplayCount(){
    var page = this.videoDataService.paginatedResult()!.pagination?.currentPage;
    var itemsPer = this.videoDataService.paginatedResult()!.pagination?.itemsPerPage;
    if(itemsPer && page != null){
      var start = ((page - 1) * itemsPer) + 1;
      var end = page * itemsPer;
      return start.toString() + " - " + end.toString() + " out of " + this.videoDataService.paginatedResult()!.pagination!.totalItems;
    }

    return "out of " + this.videoDataService.paginatedResult()!.pagination!.totalItems;
  }

  updateResultsPerPage(event: any)
  {
    var count = Number.parseInt(event.target.value);
    if(this.resultsPerPageOpts.indexOf(count) == -1){
      //Not in resultsPerPageOpts, reset to 5 because bad
      this.videoDataService.pageSize.set(5);
    } else {
      this.videoDataService.pageSize.set(count);
    }
    this.triggerPaginationUpdate();
  }

  jumpToPage(value: any)
  {
    var page = Number.parseInt(value.target.value);
    if(this.videoDataService.paginatedResult()!.pagination?.totalPages == null) return;
    if(page > this.videoDataService.paginatedResult()!.pagination?.totalPages! || page < 1)
    {
      //Reset to page 1 if outside of possible results
      this.videoDataService.pageNumber.set(1);
    } else
    {
      this.videoDataService.pageNumber.set(page);
    }
    this.triggerPaginationUpdate();
  }

  firstPage(){
    this.videoDataService.pageNumber.set(1);
    this.triggerPaginationUpdate();
  }

  lastPage(){
    if(this.videoDataService.paginatedResult()?.pagination?.totalPages != null){
      this.videoDataService.pageNumber.set(this.videoDataService.paginatedResult()?.pagination?.totalPages!);
    }
    this.triggerPaginationUpdate();
  }

  prevPage(){
    this.videoDataService.pageNumber.set(this.videoDataService.pageNumber() - 1);
    this.triggerPaginationUpdate();
  }

  nextPage(){
    this.videoDataService.pageNumber.set(this.videoDataService.pageNumber() + 1);
    this.triggerPaginationUpdate();
  }

  hasNext(): boolean
  {
    if(this.videoDataService.paginatedResult()?.pagination != null)
      {
      return this.videoDataService.paginatedResult()?.pagination?.currentPage! < this.videoDataService.paginatedResult()?.pagination?.totalPages!;
    }
    return false;
  } 

  hasPrev(): boolean
  {
    if(this.videoDataService.paginatedResult()?.pagination != null)
      {
      return this.videoDataService.paginatedResult()?.pagination?.currentPage! > 1;
    }
    return false;
  }

  isFirst(): boolean
  {
    if(this.videoDataService.paginatedResult()?.pagination != null)
      {
      return this.videoDataService.paginatedResult()?.pagination?.currentPage == 1;
    }
    return false;
  }

  isLast(): boolean
  {
    if(this.videoDataService.paginatedResult()?.pagination != null)
      {
      return this.videoDataService.paginatedResult()?.pagination?.currentPage == this.videoDataService.paginatedResult()?.pagination?.totalPages;
    }
    return false;
    
  }


  private triggerPaginationUpdate()
  {
    this.videoDataService.paginatedResult.set(null);
    this.onPaginationDataUpdate.emit();
  }

  public triggerOnTableSettingsClick(){
    this.onTableSettingsClick.emit();
  }

  public triggerOnFilterButtonClick()
  {
    this.onFilterButtonClick.emit();
  }
}
