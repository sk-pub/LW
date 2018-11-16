import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CategoryService, Category } from '../services/catservice.service'

@Component({
  selector: 'app-fetch-category',
  templateUrl: './fetch-category.component.html',
  providers: [CategoryService]
})
export class FetchCategoryComponent {
  public categories: Category[];

  constructor(http: HttpClient, private _categoryService: CategoryService) {
    this.getCategories();
  }

  getCategories() {
    this._categoryService.getCategories().subscribe(
      data => this.categories = data
    );
  }

  delete(categoryId: string) {
    var ans = confirm("Do you want to delete category with Id: " + categoryId);
    if (ans) {
      this._categoryService.deleteCategory(categoryId).subscribe((data) => {
        this.getCategories();
      }, error => console.error(error))
    }
  }
}
