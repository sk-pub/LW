import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CategoryService, Category } from '../services/catservice.service'

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  providers: [CategoryService]
})
export class AddCategoryComponent implements OnInit {
  categoryForm: FormGroup;
  title: string = "Create";
  categoryId: string;
  errorMessage: any;

  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _categoryService: CategoryService, private _router: Router) {
    if (this._avRoute.snapshot.params["id"]) {
      this.categoryId = this._avRoute.snapshot.params["id"];
    }

    this.categoryForm = this._fb.group({
      categoryId: '00000000-0000-0000-0000-000000000000',
      categoryName: ['', [Validators.required]]
    })
  }

  ngOnInit() {
    if (this.categoryId) {
      this.title = "Edit";
      this._categoryService.getCategoryById(this.categoryId)
        .subscribe(resp =>
          this.categoryForm.setValue(resp)
          , error => this.errorMessage = error);
    }
  }

  save() {
    if (!this.categoryForm.valid) {
      return;
    }

    if (this.title == "Create") {
      this._categoryService.saveCategory(this.categoryForm.value)
        .subscribe((data) => {
          this._router.navigate(['/fetch-category']);
        }, error => this.errorMessage = error)
    }
    else if (this.title == "Edit") {
      this._categoryService.updateCategory(this.categoryForm.value)
        .subscribe((data) => {
          this._router.navigate(['/fetch-category']);
        }, error => this.errorMessage = error)
    }
  }

  cancel() {
    this._router.navigate(['/fetch-category']);
  }

  get categoryName() { return this.categoryForm.get('categoryName'); }
}  
