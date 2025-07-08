import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CategoryData, Category } from '../models/categorymaster.model';
import { BehaviorSubject } from 'rxjs';
import * as servicePath from '../../../service-paths';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategorymasterService {

  _serviceUrl = environment.AdminMicroService;
  editMode = false;
  editObj = new BehaviorSubject<CategoryData>(new CategoryData());
  categoryData = new BehaviorSubject<CategoryData[]>([]);
  resources = servicePath.API.CategoryMaster;
  constructor(private httpClient : HttpClient ) { }
  
  getParentData(){
    return this.httpClient.get(this._serviceUrl + servicePath.API.CategoryMaster.getParentCategoies)
  }


  getCategory(){
     this.httpClient.get(this._serviceUrl +this.resources.list).subscribe((res:CategoryData[])=>{
       this.categoryData.next(res);
      });
  }

  createCategory(CategoryMaster : CategoryData){
    return this.httpClient.post(this._serviceUrl +this.resources.create , CategoryMaster)
  }

  editCategory( CategoryMaster :CategoryData){
    return this.httpClient.post(this._serviceUrl + this.resources.update , CategoryMaster)
  }
}

