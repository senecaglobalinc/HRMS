import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { CategorymasterService } from '../../services/categorymaster.service';
import { CategoryData, Category } from '../../models/categorymaster.model';
import { SelectItem, MessageService } from 'primeng/components/common/api';
import { Parent } from '../../models/parent.model';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.scss'],
  providers: [MessageService]
})
export class CategoryFormComponent implements OnInit {
  addCategory: FormGroup;
  btnLabel = "";
  formSubmitted = false;
  ParentData: SelectItem[] = [];
  constructor(private _categoryservice: CategorymasterService,
    private messageService: MessageService) { }

  ngOnInit() {
    this.getParentData();
    this.addCategory = new FormGroup({
      ParentId: new FormControl(null),
      CategoryName: new FormControl(null, [
        Validators.required,
      ])

    });
    this._categoryservice.editObj.subscribe((data) => {
      if (this._categoryservice.editMode == true) {
        this.btnLabel = "Update";
        this.addCategory.patchValue(data);
      }
    });
    this.btnLabel = "Save";
    this.Reset();
  }

  getParentData() {
    this.ParentData = [];
    this.ParentData.push({ label: "Select Parent", value: null });
    this._categoryservice.getParentData().subscribe((res: Category[]) => {
      res.forEach(element => {
        this.ParentData.push({ label: element.CategoryName, value: element.CategoryMasterId });
      });
    });
  }

  CreateCategory() {
    this.formSubmitted = true;
    var creatingObj = new CategoryData();

    creatingObj.CategoryName = this.addCategory.value.CategoryName;

    if (this.addCategory.value.ParentId != undefined && this.addCategory.value.ParentId != null
      && this.addCategory.value.ParentId != '')
      creatingObj.ParentId = this.addCategory.value.ParentId;
    else
      creatingObj.ParentId = 0;

    if (this._categoryservice.editMode == true) {
      creatingObj.CategoryMasterId = this._categoryservice.editObj.value.CategoryMasterId;
    }
    if (this.addCategory.valid == true) {
      if (this._categoryservice.editMode == false) {
        this._categoryservice.createCategory(creatingObj).subscribe(
          response => {
            if (response != null) {
              this._categoryservice.getCategory();
              this.getParentData();
              if (this._categoryservice.editMode == false)
                this.messageService.add({ severity: 'success', summary: 'Success message', detail: 'Category record added successfully.' });
              this.Reset();
            }
            else
              this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add category.' });
          },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error });
          });

      }
      else {
        this._categoryservice.editCategory(creatingObj).subscribe(
          response => {
            if (response != null) {
              this._categoryservice.getCategory();
              this.getParentData();
              this.messageService.add({ severity: 'success', summary: 'Success message', detail: 'Category record updated successfully.' });
              this.Reset();
            }
          }, error => {
            this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error });

          }
        )
      }
    }
    else {
      // this.messageService.add({severity:'warn', summary: 'Warning message', detail:'Invalid data'});

    }

  }

  Reset() {
    this.formSubmitted = false;
    this.addCategory.reset();
    this._categoryservice.editMode = false;
    this.btnLabel = "Save";
  }

  ngOnDestroy() {
    // this._designationService.editObj.unsubscribe();
  }
}
