import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, signal, Signal, WritableSignal, OnInit, ChangeDetectionStrategy, EventEmitter, Output } from '@angular/core';
import {MatDatepickerInputEvent, MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { ChannelData } from '../../../../_models/channelData';
import { CategoryData } from '../../../../_models/categoryData';
import { ChannelDataService } from '../../../../_services/channel-data.service';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatSelectModule} from '@angular/material/select';
import { Observable } from 'rxjs';
import {map, startWith} from 'rxjs/operators';
import {AsyncPipe} from '@angular/common';
import { CategoryDataService } from '../../../../_services/category-data.service';
import { VideoDataService } from '../../../../_services/video-data.service';
@Component({
  selector: 'app-filters',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAutocompleteModule,
    MatSelectModule,
    ReactiveFormsModule
  ],
  
  templateUrl: './filters.component.html',
  styleUrl: './filters.component.css',
  animations: [
    trigger('slideInOut', [
      state("true",
        style({
          transform: 'scaleY(100%)'
        })
      ),
      state("false",
        style({
          height: "0px",
          margin: 'auto',
          transform: 'scaleY(0%)'
        })
      ),
      transition('false <=> true', [animate('300ms ease-in-out')]),
    ])
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FiltersComponent implements OnInit {
  @Input() showFilters!: () => boolean;
  @Output() shouldUpdateTableContents: EventEmitter<any> = new EventEmitter();

  videoDataService = inject(VideoDataService);
  channelService = inject(ChannelDataService);
  categoryService = inject(CategoryDataService);
  filterChannels = signal<ChannelData[]|null>(null);
  filterCategories = signal<CategoryData[]|null>(null);

  channelAutoController = new FormControl<string|ChannelData>('');
  categoryAutoController = new FormControl<string|CategoryData>('');

  acFilteredChannels!: Observable<ChannelData[]>;
  acFilteredCategories!: Observable<CategoryData[]>;
  testString?: any;


  watchedStart?: string;
  watchedEnd?: string;

  publishedStart?: string;
  publishedEnd?: string;



  viewOpts = [">", "<","=",">=","<="];
  selectedViewOpt = "=";
  views?: string;
  ngOnInit()
  {
    this.channelService.getAllUserChannels().subscribe(data => this.filterChannels.set(data));
    this.acFilteredChannels = this.channelAutoController.valueChanges.pipe(
      startWith(''),
      map(value => {
        const name = typeof value === 'string' ? value : value?.title;
        return name ? this._channelFilter(name as string) : this.filterChannels()!.slice();
      })
    )
    this.categoryService.getAllUserCategories().subscribe(data => this.filterCategories.set(data));
    
    this.acFilteredCategories = this.categoryAutoController.valueChanges.pipe(
      startWith(''),
      map(value => {
        const name = typeof value === 'string' ? value : value?.id.toString();
        return name ? this._categoryFilter(name as string) : this.filterCategories()!.slice();
      })
    )

    this.channelAutoController.valueChanges.subscribe(
      (data) => {
        
        if(this.channelAutoController.valid && typeof this.channelAutoController.value == 'object') {
          this.setFilter("Channel", (this.channelAutoController.value as ChannelData).id.toString());
        } else {
          this.setFilter("Channel", null);
        }
        this.shouldUpdateTableContents.emit();
      }
    );

    this.categoryAutoController.valueChanges.subscribe(
      (data) => {
        if(typeof this.categoryAutoController.value == 'object') {
          this.setFilter("Category", (this.categoryAutoController.value as CategoryData).id.toString());
        } else {
          this.setFilter("Category", null);
        }
        this.shouldUpdateTableContents.emit();
      }
    );
  }

  channelDisplayFn(channel: ChannelData): string
  {
    return channel.title;
  }

  private _channelFilter(value: string): ChannelData[] {
    
    const filterValue = value.toLowerCase();

    return this.filterChannels()!.filter(channel => channel.title.toLowerCase().includes(filterValue));
  }





  categoryDisplayFn(category: CategoryData): string
  {
    return category?.id?.toString() ?? "";
  }

  private _categoryFilter(value: string): CategoryData[] {
    
    const filterValue = value.toLowerCase();

    return this.filterCategories()!.filter(cat => cat.id.toString().includes(filterValue));
  }


  setDateFilter(type:string, value: string | null)
  {
    if(value != null){
      let parsedTime = new Date(value)
      
      this.videoDataService.updateFilter(type, parsedTime.toLocaleDateString("en-US", {
        year: "numeric",
        month: "long",
        day: "numeric"
      }));
    } else {
      this.videoDataService.removeFilter(type)
    }
    this.shouldUpdateTableContents.emit()
  }

  setFilter(type: string, value: string | null)
  {
    if(value != null)
    {
      this.videoDataService.updateFilter(type, value);
    } else {
      this.videoDataService.removeFilter(type)
    }
  }

  setViewFilter()
  {
    if(this.views != null && this.views?.length >= 3)
    {
      this.videoDataService.updateFilter("Views", this.selectedViewOpt + "_" + this.views);
    } else {
      this.videoDataService.removeFilter("Views")
    }
    this.shouldUpdateTableContents.emit();
  }
}
