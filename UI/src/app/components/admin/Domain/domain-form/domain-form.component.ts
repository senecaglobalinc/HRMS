import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DomainMasterService } from '../../services/domainmaster.service';
import { DomainMasterData } from '../../models/domainmasterdata.model';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-domain-form',
  templateUrl: './domain-form.component.html',
  styleUrls: ['./domain-form.component.css'],
  providers: [MessageService]
})

export class DomainFormComponent implements OnInit {
  addDomainName : FormGroup;
  btnLabel : string = "";
  displayDialog : boolean = false;
  formSubmitted = false;
 
  constructor(private _domainService : DomainMasterService, private messageService: MessageService) { 
  }
  
  ngOnInit() {
    this.addDomainName = new FormGroup({
      DomainName : new FormControl(null,[
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
    });
    this._domainService.domainEdit.subscribe(data => {
      if (this._domainService.editMode == true) {
        this.addDomainName.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.btnLabel = "SAVE";
    this.cancel();
  }

  addDomain() : void {
    this.formSubmitted = true;
    var domain = new DomainMasterData();
    domain.DomainName = this.addDomainName.value.DomainName;
    if(this._domainService.editMode == true){
      domain.DomainID = this._domainService.domainEdit.value.DomainID;
    }
    if(this.addDomainName.valid == true){
      this._domainService.createDomain(domain).subscribe(res => {    
        if(res){
          this._domainService.getDomains();
          if(this._domainService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Domain record added successfully.'});
          else
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Domain record updated successfully.'});
            this.cancel();
        }
        else{
          this.messageService.add({severity:'error', summary: 'Error message', detail:'Unable to add domain.'});   
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
    this.addDomainName.reset();
    this._domainService.editMode = false;
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

  ngOnDestroy() {
    // this._domainService.domainEdit.unsubscribe();
  }

}



