import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { UserData } from '../_models/userData';
import { environment } from '../../environments/environment.development';
import { map, Observable } from 'rxjs';
import { UserQueueData } from '../_models/userQueueData';

@Injectable({
  providedIn: 'root'
})
export class QueueTasksService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  usersWithTasks = signal<UserData[]|null>(null);
  getUsersWithTasks()
  {
    
    this.http.get<{id: number; username: string}[]>(this.baseUrl + "queue/user-tasks").pipe(
      map(data => {
        {
          return data.map((item) => {
            return {
            Id: item.id,
            Username: item.username
          }})
        }
      })
    ).subscribe(data => this.usersWithTasks.set(data));
  }

  getTasksForUser(userID: number): Observable<UserQueueData[]>
  { 
    return this.http.get<UserQueueData[]>(this.baseUrl + "queue/user-tasks/" + userID.toString()).pipe(
      map(data => {
        return data.map(item => {
          return item
        }
        )
      })
    )
  }



  startUserTasks(userID: number)
  {
    this.http.post(this.baseUrl + "queue/user-tasks/" + userID.toString() + "/start", "").subscribe({
      complete: () => this.getUsersWithTasks()
    });
  }

  deleteUserTasks(userID: number)
  {
    this.http.post(this.baseUrl + "queue/user-tasks/" + userID.toString() + "/cancel", "").subscribe({
      complete: () => this.getUsersWithTasks()
    });
  }

  startTask(taskID: number)
  {
    return this.http.post(this.baseUrl + "queue/tasks/" + taskID.toString() + "/start", "");
  }

  deleteTask(taskID: number)
  {
    return this.http.post(this.baseUrl + "queue/tasks/" + taskID.toString() + "/cancel", "");
  }
}
