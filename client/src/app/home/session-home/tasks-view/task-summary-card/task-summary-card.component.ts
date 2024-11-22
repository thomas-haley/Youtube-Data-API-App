import { Component, inject, Input, signal } from '@angular/core';
import { UserQueueData } from '../../../../_models/userQueueData';
import { QueueTasksService } from '../../../../_services/queue-tasks.service';

@Component({
  selector: 'app-task-summary-card',
  standalone: true,
  imports: [],
  templateUrl: './task-summary-card.component.html',
  styleUrl: './task-summary-card.component.css'
})
export class TaskSummaryCardComponent {
  queueService = inject(QueueTasksService);
  @Input() queueItem!: UserQueueData;
  @Input() updateTasksFunc: Function | null = null;
  idList = signal<string[] |null>(null);
  ngOnInit(){
    this.buildIDList();
  }

  buildIDList(){
    switch (this.queueItem.taskType){
      case ("videos/list"):
        this.idList.set(this.queueItem.videos)
        break;
      case("channels/list"):
      this.idList.set(this.queueItem.channels)
        break;
      default:
        break; 
    }
  }
  test(id: number){
    console.log(id)
  }

  startTask(){
    this.queueService.startTask(this.queueItem.id).subscribe(() => {
      if(this.updateTasksFunc != null){
        this.updateTasksFunc();
    }
    });
  }

  cancelTask(){
    this.queueService.deleteTask(this.queueItem.id).subscribe(() => {
      if(this.updateTasksFunc != null){
        this.updateTasksFunc();
    }
    });
  }
}
