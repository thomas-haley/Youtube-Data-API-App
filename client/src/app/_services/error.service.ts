import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private https = inject(HttpClient);
  baseUrl = environment.apiUrl;


  get400Error()
  {
    this.https.get(this.baseUrl + "error/400").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }
  getValidationError(){
    this.https.post(this.baseUrl + "account/register", {}).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401Error()
  {
    this.https.get(this.baseUrl + "error/401").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  getAuthError()
  {
    this.https.get(this.baseUrl + "error/auth").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get404Error()
  {
    this.https.get(this.baseUrl + "error/404").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error()
  {
    this.https.get(this.baseUrl + "error/500").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  queueTest(){
    this.https.get(this.baseUrl + "queue/add").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

}
