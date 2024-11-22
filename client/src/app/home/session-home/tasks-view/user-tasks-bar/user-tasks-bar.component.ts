import { Component, inject, Input, signal } from '@angular/core';
import { UserData } from '../../../../_models/userData';
import { QueueTasksService } from '../../../../_services/queue-tasks.service';
import { UserQueueData } from '../../../../_models/userQueueData';
import { TaskSummaryCardComponent } from '../task-summary-card/task-summary-card.component';

@Component({
  selector: 'app-user-tasks-bar',
  standalone: true,
  imports: [TaskSummaryCardComponent],
  templateUrl: './user-tasks-bar.component.html',
  styleUrl: './user-tasks-bar.component.css'
})
export class UserTasksBarComponent {
  @Input() userID!: number;
  @Input() userName!: string;
  queueService = inject(QueueTasksService);

  userTaskData = signal<UserQueueData[]|null>(null);

  ngOnInit(){
    this.queueService.getTasksForUser(this.userID).subscribe(data => this.userTaskData.set(data));
    
  }


  test(){
    console.log(this.userTaskData());
  }

  updateUserTasks(){
    this.queueService.getTasksForUser(this.userID).subscribe(data => this.userTaskData.set(data));
  }


  startAllTasks(){
    this.queueService.startUserTasks(this.userID);
  }

  cancelAllTasks(){
    this.queueService.deleteUserTasks(this.userID);
  }
}
