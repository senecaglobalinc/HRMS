import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem, MessageService } from 'primeng/api';
import { KeyFunctionService } from '../../services/key-function.service';
import { KeyFunction } from '../../models/key-function.model';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-key-function-form',
  templateUrl: './key-function-form.component.html',
  styleUrls: ['./key-function-form.component.scss']
})
export class KeyFunctionFormComponent implements OnInit {

  isEdit : boolean;
  btnLabel : string = "";
  formSubmitted = false;
  addkeyFunction : FormGroup;
  keyFunction : KeyFunction;
  departmentList: SelectItem[];
  
  constructor(private _keyFunctionService : KeyFunctionService,private  messageService: MessageService) { }

  ngOnInit() {
    this.getDepartmentList();
    this.btnLabel = "SAVE";
    this.keyFunction = new KeyFunction();
    this.addkeyFunction = new FormGroup({
      DepartmentId : new FormControl(null,[
        Validators.required
      ]),
      SGRoleName : new FormControl(null,[
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
    });
    
    this._keyFunctionService.KeyFunctionEdit.subscribe(data => {
      if (this._keyFunctionService.editMode == true) {
        this.isEdit = this._keyFunctionService.editMode;
        this.addkeyFunction.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.cancel();
  }

  getDepartmentList() : void{
    this._keyFunctionService.getDepartmentList().subscribe((res : Department[]) => {
      this.departmentList = [];
      this.departmentList.push({ label: "Select Deparment", value: null });
      res.forEach(e => {
        this.departmentList.push({ label: e.DepartmentCode, value: e.DepartmentId });
      });
    },
    (error)=>{
      this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   
    });
  }

  addkeyFunctions() : void {  
    this.formSubmitted = true;
    var keyFunction = new KeyFunction();
    keyFunction.DepartmentId = this.addkeyFunction.value.DepartmentId;
    keyFunction.SGRoleName = this.addkeyFunction.value.SGRoleName;
    if(this._keyFunctionService.editMode == true){
      keyFunction.SGRoleID = this._keyFunctionService.KeyFunctionEdit.value.SGRoleID;
      keyFunction.DepartmentId = this._keyFunctionService.KeyFunctionEdit.value.DepartmentId;
    }
    if(this.addkeyFunction.valid == true){
      this._keyFunctionService.createKeyFunction(keyFunction).subscribe(res => {
        if(res != null){
          this._keyFunctionService.getKeyFunctions();
          if(this._keyFunctionService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Key function record added successfully.'});
          
          else
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Key function record updated successfully.'});
            this.cancel();
        }
      },
      (error)=>{
        this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   
        
      }
    );
      
    }
    else{
     // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
     // this.cancel();
    }
  }

  cancel() : void{
    this.formSubmitted = false;
    this.addkeyFunction.reset();
    this._keyFunctionService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }
}


