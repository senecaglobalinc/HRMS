import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DepartmentTypeService } from '../../services/department-type.service';
import { DepartmentTypeData } from '../../models/department-type.model';
import {MessageService} from 'primeng/api';


@Component({
  selector: 'app-department-type-form',
  templateUrl: './department-type-form.component.html',
  styleUrls: ['./department-type-form.component.scss'],
  providers: [MessageService]
})
export class DepartmentTypeFormComponent implements OnInit {
  addDepartmentType :FormGroup;
  btnLabel = "";
  formSubmitted = false;
 
  constructor(private _departmenttypeservice : DepartmentTypeService,
     private messageService : MessageService) { }

  ngOnInit() {
    
    this.btnLabel = "Save";  
   
    this.addDepartmentType = new FormGroup({
      DepartmentTypeDescription : new FormControl(null,[
        Validators.required,
      ])
    ,
     
    });
    this._departmenttypeservice.departmentEdit.subscribe((data)=>{
    
      if(this._departmenttypeservice.editMode == true){
        this.btnLabel = "Update";
        this.addDepartmentType.patchValue(data);
      }
    });
    this.Reset();
  }

  
  createDepartmentType() {  
    this.formSubmitted = true;
    var departmenttype = new DepartmentTypeData();
    departmenttype.DepartmentTypeDescription = this.addDepartmentType.value.DepartmentTypeDescription;
 
    if(this._departmenttypeservice.editMode == true){
      departmenttype.DepartmentTypeId = this._departmenttypeservice.departmentEdit.value.DepartmentTypeId;
          
    }
    if(this.addDepartmentType.valid == true) {
        if(this._departmenttypeservice.editMode == false ){
        this._departmenttypeservice.createDepartmentType(departmenttype).subscribe(
          response =>{
            if(response){
              this._departmenttypeservice.getDepartmentType(); 
              if(this._departmenttypeservice.editMode == false)    
                this.messageService.add({severity:'success', summary: 'Success message', detail:'Department type record added successfully.'});
                this.Reset();  
            }
            else
             this.messageService.add({severity:'error', summary: 'Error message', detail:'unable to add department type.'});  
           },
           error=>{
            this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});
  
          });
          
        }
        else{
          this._departmenttypeservice.editDepartmentType(departmenttype).subscribe(
            response =>{
              if(response){
                this._departmenttypeservice.getDepartmentType();     
                this.messageService.add({severity:'success', summary: 'Success message', detail:'Department type record updated successfully.'}); 
                this.Reset(); 
              }
             }, error=>{
              this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});
    
            }
            )
          }
        }
      

    else{
     // this.messageService.add({severity:'warn', summary: 'Warning message', detail:'Invalid data'});

    }
    
}

Reset(){
  this.formSubmitted = false;
  this.addDepartmentType.reset();
  this.btnLabel = "Save";
  this._departmenttypeservice.editMode = false;
 }

 ngOnDestroy() {
  // this._designationService.editObj.unsubscribe();
}
}
