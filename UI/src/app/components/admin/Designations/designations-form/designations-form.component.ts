import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { DesignationsService } from '../../services/designations.service';
import {  DesignationData } from '../../models/designation.model';
import { SelectItem, MessageService } from 'primeng/components/common/api';
import { Grade } from '../../models/grade.model';

@Component({
  selector: 'app-designations-form',
  templateUrl: './designations-form.component.html',
  styleUrls: ['./designations-form.component.css'],
  providers: [MessageService]
})
export class DesignationsFormComponent implements OnInit {
  addDesignation :FormGroup;
  btnLabel = "";
  formSubmitted = false;
  GradeCodeMasterData : SelectItem[] = [];
  constructor(private _designationService : DesignationsService, private messageService : MessageService) { }

  ngOnInit() {
    this.GetGradesData();
    this.btnLabel = "Save";  
   
    this.addDesignation = new FormGroup({
      GradeId : new FormControl(null,[
        Validators.required,
      ]),
      DesignationCode : new FormControl(null,[
        Validators.required,
      ]),
      DesignationName : new FormControl(null,[
        Validators.required, 
      ])
    });
    this._designationService.editObj.subscribe((data)=>{
    
      if(this._designationService.editMode == true){
        this.btnLabel = "Update";
        this.addDesignation.patchValue(data);
      }
    });
    this.Reset();
  }

  GetGradesData(){
    this.GradeCodeMasterData.push({ label:"Select Grade", value: null });
    this._designationService.getGradesData().subscribe((res : Grade[])=>{
      res.forEach(element => {
        this.GradeCodeMasterData.push({ label: element.GradeCode, value: element.GradeId });
    });
    });
  }
  
  CreateDesignation() {  
    this.formSubmitted = true;
    var creatingObj = new DesignationData();
    creatingObj.DesignationCode = this.addDesignation.value.DesignationCode;
    creatingObj.DesignationName = this.addDesignation.value.DesignationName;
    creatingObj.GradeId = this.addDesignation.value.GradeId;
    if(this._designationService.editMode == true){
      creatingObj.DesignationId = this._designationService.editObj.value.DesignationId;
      creatingObj.IsActive = this._designationService.editObj.value.IsActive;     
    }
    if(this.addDesignation.valid == true) {
        if(this._designationService.editMode == false ){
        this._designationService.createDesignation(creatingObj).subscribe(
          response =>{
            if(response != null){
              this._designationService.getDesignation(); 
              if(this._designationService.editMode == false)    
                this.messageService.add({severity:'success', summary: 'Success message', detail:'Designation record added successfully.'});
                this.Reset();  
            }  
           },
           error=>{
            this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});
  
          });
          
        }
        else{
          this._designationService.editDesignation(creatingObj).subscribe(
            response =>{
              if(response != null){
                this._designationService.getDesignation();     
                this.messageService.add({severity:'success', summary: 'Success message', detail:'Designation record updated successfully.'}); 
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
  this.addDesignation.reset();
  this.btnLabel = "Save";
  this._designationService.editMode = false;
 }

 ngOnDestroy() {
  // this._designationService.editObj.unsubscribe();
}
}
