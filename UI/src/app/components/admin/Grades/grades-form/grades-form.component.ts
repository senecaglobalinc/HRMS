import { Component, OnInit } from "@angular/core";
import {MessageService} from 'primeng/api';
import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";
import { GradesService } from "../../services/grades.service";
import { Grade } from "../../models/grade.model";

@Component({
  selector: "app-grades-form",
  templateUrl: "./grades-form.component.html",
  styleUrls: ["./grades-form.component.css"],
  providers: [MessageService]
})
export class GradesFormComponent implements OnInit {
  btnLabel = "";
  valid = true;
  addGrades : FormGroup;
  formSubmitted = false;
  constructor(private serviceObj: GradesService, private messageService: MessageService) {}

  ngOnInit() {
    this.addGrades = new FormGroup({
      "GradeCode" : new FormControl(null, [Validators.required]),
      "GradeName" : new FormControl(null,[Validators.required])
    })
    this.serviceObj.editObj.subscribe(data => {
      if (this.serviceObj.editMode == true) {
        this.addGrades.patchValue(data);
        this.btnLabel = "Update";
      }
    });  
   this.btnLabel = "Save";
   this.Reset();
  }

  Reset() {
    this.formSubmitted = false;
  this.addGrades.reset();
  this.serviceObj.editMode = false;
  this.btnLabel = "Save"
  }

  CreateGrades() {
    this.formSubmitted = true;
    var creatingObj = new Grade();
    creatingObj.GradeCode = this.addGrades.value.GradeCode;
    creatingObj.GradeName = this.addGrades.value.GradeName;

    if(this.serviceObj.editMode == true){
      creatingObj.IsActive = this.serviceObj.editObj.value.IsActive;
      creatingObj.GradeId = this.serviceObj.editObj.value.GradeId;
      creatingObj.GradeCode = this.serviceObj.editObj.value.GradeCode;
    }
    if(this.addGrades.valid == true){
    this.serviceObj.createGrades(creatingObj)
    .subscribe(res => {
      if(res != null) {
         this.serviceObj.getGradesDetails();
         if(this.serviceObj.editMode == false)
           this.messageService.add({severity:'success', summary: 'Success Message', detail:'Grade record added successfully.'});   
        else
          this.messageService.add({severity:'success', summary: 'Success Message', detail:'Grade record updated successfully.'});
          this.Reset();   
      }    
    },
  error=>{
    this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   

  });
    
  }
  else{
    // this.messageService.add({severity:'warn', summary: 'Warning message', detail:'Invalid data'});
    // this.Reset();
  }
  }

  Validate(value){
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
 }
 ngOnDestroy() {
  // this.serviceObj.editObj.unsubscribe();
}
}
