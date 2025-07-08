import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpecialityService } from '../../services/speciality.service';
import { Speciality } from '../../models/speciality.model';
import {MessageService} from 'primeng/api';
@Component({
  selector: 'app-speciality-form',
  templateUrl: './speciality-form.component.html',
  styleUrls: ['./speciality-form.component.scss']
})

export class SpecialityFormComponent implements OnInit {  
  addSpecialityName : FormGroup;
  btnLabel : string = "";
  displayDialog : boolean = false;
  formSubmitted = false;
 
  constructor(private _specialityService : SpecialityService, private messageService: MessageService) { 
  }
  
  ngOnInit() {
    this.addSpecialityName = new FormGroup({
      SuffixName : new FormControl(null,[
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
    });
    this._specialityService.specialityEdit.subscribe(data => {
      if (this._specialityService.editMode == true) {
        this.addSpecialityName.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();
  }

  addSpeciality() : void {
    this.formSubmitted = true;
    var speciality = new Speciality();
    speciality.SuffixName = this.addSpecialityName.value.SuffixName;
    if(this._specialityService.editMode == true){
      speciality.SuffixID = this._specialityService.specialityEdit.value.SuffixID;
    }
    if(this.addSpecialityName.valid == true){
      this._specialityService.createSpeciality(speciality).subscribe(res => {    
        if(res){
          this._specialityService.getSpecialities();
          if(this._specialityService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Speciality record added successfully.'});
          else
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Speciality record updated successfully.'});
            this.cancel();
        }
        else{
          this.messageService.add({severity:'error', summary: 'Error message', detail:'Unable to add speciality.'});   
          this.cancel();  
        }
      },
    error=>{
      this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   

    });
      
    }
    else{
     // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      //this.cancel();
    }  
  }
  
  cancel() : void {
    this.formSubmitted = false;
    this.addSpecialityName.reset();
    this._specialityService.editMode = false;
    this.btnLabel = "SAVE";
  }

  omit_special_char(event: any) {
    let k: number;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (        
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 38 ||
      k == 44 ||
      k == 45
    );
  }
}
