import { Component, inject, OnInit, signal } from '@angular/core';
import { UserTasksBarComponent } from './user-tasks-bar/user-tasks-bar.component';
import { QueueTasksService } from '../../../_services/queue-tasks.service';
import { UserData } from '../../../_models/userData';
import { map } from 'rxjs';

@Component({
  selector: 'app-tasks-view',
  standalone: true,
  imports: [UserTasksBarComponent],
  templateUrl: './tasks-view.component.html',
  styleUrl: './tasks-view.component.css'
})
export class TasksViewComponent implements OnInit{
  queueService = inject(QueueTasksService);
  

  ngOnInit(){
    this.queueService.getUsersWithTasks();

  }

}
