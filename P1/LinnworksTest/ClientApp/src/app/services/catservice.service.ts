import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class CategoryService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.myAppUrl = baseUrl;
  }

  getCategories() {
    return this._http.get<Category[]>(this.myAppUrl + 'api/Category/Index')
      .catch(this.errorHandler);
  }

  getCategoryById(id: string) {
    return this._http.get<Category>(this.myAppUrl + "api/Category/Details/" + id)
      .catch(this.errorHandler)
  }

  saveCategory(Category) {
    return this._http.post(this.myAppUrl + 'api/Category/Create', Category)
      .catch(this.errorHandler)
  }

  updateCategory(Category) {
    return this._http.put(this.myAppUrl + 'api/Category/Edit', Category)
      .catch(this.errorHandler);
  }

  deleteCategory(id) {
    return this._http.delete(this.myAppUrl + "api/Category/Delete/" + id)
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}

export interface Category {
  categoryId: string;
  categoryName: string;
  stock: number;
}
