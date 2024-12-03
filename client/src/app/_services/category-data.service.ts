import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { map, Observable } from 'rxjs';
import { CategoryData } from '../_models/categoryData';


@Injectable({
  providedIn: 'root'
})
export class CategoryDataService {

  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;


  getAllUserCategories(): Observable<CategoryData[]>
  {
    return this.http.get<CategoryData[]>(this.baseUrl + "users/categories/all", ).pipe(
        map(data => {
          return data.map(category => {
              return category
            }
          )
        }
      )
    )
  }
}
