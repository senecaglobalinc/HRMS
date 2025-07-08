import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SeniorityService } from '../../services/seniority.service';
import {MessageService} from 'primeng/api';
import { Seniority } from '../../models/seniority.model';

@Component({
  selector: 'app-seniority-form',
  templateUrl: './seniority-form.component.html',
  styleUrls: ['./seniority-form.component.scss']
})

export class SeniorityFormComponent implements OnInit {
  addSeniorityName : FormGroup;
  btnLabel : string = "";
  displayDialog : boolean = false;
  formSubmitted = false;
 
  constructor(private _seniorityService : SeniorityService, private messageService: MessageService) { 
  }
  
  ngOnInit() {
    this.addSeniorityName = new FormGroup({
      PrefixName : new FormControl(null,[
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
    });
    this._seniorityService.seniorityEdit.subscribe(data => {
      if (this._seniorityService.editMode == true) {
        this.addSeniorityName.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();
  }

  addSeniority() : void {
    this.formSubmitted = true;
    var seniority = new Seniority();
    seniority.PrefixName = this.addSeniorityName.value.PrefixName;
    if(this._seniorityService.editMode == true){
      seniority.PrefixID = this._seniorityService.seniorityEdit.value.PrefixID;
    }
    if(this.addSeniorityName.valid == true){
      this._seniorityService.createSeniority(seniority).subscribe(res => {    
        if(res){
          this._seniorityService.getSeniorities();
          if(this._seniorityService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Seniority record added successfully.'});
          else
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Seniority record updated successfully.'});
            this.cancel();
        }
        else{
          this.messageService.add({severity:'error', summary: 'Error message', detail:'Unable to add seniority.'});   
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
    this.addSeniorityName.reset();
    this._seniorityService.editMode = false;
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



