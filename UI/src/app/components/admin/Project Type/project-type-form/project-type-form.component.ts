import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ProjectTypeService } from '../../services/project-type.service';
import { ProjectTypeData } from '../../Models/projecttype.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-project-type-form',
  templateUrl: './project-type-form.component.html',
  styleUrls: ['./project-type-form.component.css'],
  providers: [MessageService]
})
export class ProjectTypeFormComponent implements OnInit {
  valid = false;
  btnLabel = "";
  addProjectType : FormGroup;
  formSubmitted = false;
  constructor(private serviceObj : ProjectTypeService,private  messageService : MessageService) { }

  ngOnInit() {
    this.btnLabel = "Save";
    this.addProjectType = new FormGroup({
      "ProjectTypeCode" : new FormControl(null , [Validators.required]),
      "Description" : new FormControl(null , [Validators.required])
    });
    this.serviceObj.editObj.subscribe((data)=>{
      if(this.serviceObj.editMode == true)
      {
       this.addProjectType.patchValue(data);
       this.btnLabel = "Update";
      }
    })
    this.Reset();
  }
  CreateProjectType() : void{
    this.formSubmitted = true;
    var creatingObj = new ProjectTypeData();
    creatingObj.Description = this.addProjectType.value.Description;
    creatingObj.ProjectTypeCode = this.addProjectType.value.ProjectTypeCode;
    
    if(this.serviceObj.editMode == true){
      creatingObj.ProjectTypeId= this.serviceObj.editObj.value.ProjectTypeId;
    }
    if(this.addProjectType.valid == true)
    {
    this.serviceObj.createProjectType(creatingObj).subscribe(res => {
      if (res) {
      this.serviceObj.getProjectTypeData();
      if(this.serviceObj.editMode == true)
        this.messageService.add({severity:'success', summary: 'Success Message', detail:'Project type record added successfully.'});  
      else
        this.messageService.add({severity:'success', summary: 'Success Message', detail:'Project type record updated successfully.'});  
        this.Reset();
      }
      else
        this.messageService.add({severity:'error', summary: 'Error message', detail:'Unable to add project type.'});  

    },
  error=>{
    this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});  

  });   
  }
  else{
    //this.messageService.add({severity:'warn', summary: 'Warn message', detail:'Invalid data'});
  }
  }
  Validate(value) : void{
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
 }
Reset() : void{
  this.formSubmitted = false;
    this.addProjectType.reset();
    this.serviceObj.editMode = false;
    this.btnLabel = "Save";
  }

ngOnDestroy() {
    // this.serviceObj.editObj.unsubscribe();
  }
}
