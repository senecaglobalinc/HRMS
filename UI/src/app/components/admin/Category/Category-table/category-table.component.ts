import { Component, OnInit } from '@angular/core';
import { CategorymasterService } from '../../services/categorymaster.service';
import {  CategoryData } from '../../models/categorymaster.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-category-table',
  templateUrl: './category-table.component.html',
  styleUrls: ['./category-table.component.scss']
})
export class CategoryTableComponent implements OnInit {

  CategoryList : CategoryData[];
  selectedCategory: CategoryData;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  cols = [
    { field: 'CategoryName', header: 'Category Name' },
    { field: 'ParentCategoryName', header: 'Parent Name' }
];
  constructor(private _categoryService : CategorymasterService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.CategoryList = [];
    this._categoryService.getCategory();

    this._categoryService.categoryData.subscribe((data) => {
      this.CategoryList = data;
    });
  }

  setEditObj(editObj) {
    this._categoryService.editMode = true;
    this._categoryService.editObj.next(editObj);
   }
  ngOnDestroy() {
    // this._designationsService.designationData.unsubscribe();
  }
}
