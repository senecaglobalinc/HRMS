import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { Department } from "../../models/department.model";
import { GenericType } from "../../../../models/dropdowntype.model";
import { SelectItem, Message } from "primeng/components/common/api";
import { DepartmentService } from "../../services/department.service";
import { MasterDataService } from "../../../../services/masterdata.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { MessageService } from 'primeng/components/common/messageservice';


@Component({
  selector: 'app-departments-form',
  templateUrl: './departments-form.component.html',
  styleUrls: ['./departments-form.component.css'],
  providers: [MasterDataService, MessageService]
})
export class DepartmentsFormComponent implements OnInit {
  addDepartment: FormGroup;
  filteredAssociateIds: GenericType[] = [];
  formSubmitted: boolean = false;
  btnLabel = "";
  isEdit : boolean;
 editObj;
  constructor(
    private  _router: Router,
    private _departmentService: DepartmentService,
    private _masterDataService: MasterDataService,
    private messageService: MessageService
  ) {}

  ngOnInit() { 
    this.addDepartment = new FormGroup({
      DepartmentCode: new FormControl(null, [Validators.required]),
      Description: new FormControl(null, [Validators.required]),
      DepartmentHead : new FormControl(null),
      IsDelivery: new FormControl(false),
      
    });
      this._departmentService.departmentsEdit.subscribe((data) => {
        if(this._departmentService.editMode == true){
          this.btnLabel = "Update";
          this.isEdit = this._departmentService.editMode;
      
       if(this.isEdit == true)
        {
          this.addDepartment.patchValue({
            DepartmentCode : data.DepartmentCode,
            Description :  data.Description,
            DepartmentHead : {Id :data.DepartmentHeadId, Name :  data.DepartmentHeadName }
          });
        }
      } 
    });
    this.btnLabel = "Save";
    this.cancel();
  }

    CreateDepartment() {
    this.formSubmitted = true;
    var department = new Department();
    department.DepartmentCode = this.addDepartment.value.DepartmentCode;
    department.Description = this.addDepartment.value.Description;
    if(this.addDepartment.value.DepartmentHead  != null && this.addDepartment.value.DepartmentHead.Id != null)
      {
        department.DepartmentHeadId = this.addDepartment.value.DepartmentHead.Id;
        department.DepartmentHeadName = this.addDepartment.value.DepartmentHead.Name;
      }
    else
      {
        department.DepartmentHeadId = null;
        department.DepartmentHeadName = null;
      }
    
    
    if(this.addDepartment.value.IsDelivery == true){
      department.DepartmentTypeId = 1;
    } else {
      department.DepartmentTypeId = 2;
    }
    
    if(this._departmentService.editMode == true){
      department.DepartmentId = this._departmentService.departmentsEdit.value.DepartmentId;
      
    }
    if(this.addDepartment.valid == true){
      this._departmentService.CreateDepartment(department).subscribe((res:number) => {
          
          if(res){
            this._departmentService.getDepartmentDetails();
          if(this._departmentService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Department record added successfully.'});
          else if(this._departmentService.editMode == true)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Department record updated successfully.'});    
            this.cancel();
          }
        else{
          this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Department cannot be added.'});
        }
      },
      (error)=>{
        this.messageService.add({ severity: 'error', summary: 'Server error', detail: error.error});

      });
      
    }
    else{
      this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      // this.cancel();
    }
  }

  filteredAssociatesMultiple(event: any): void {
  //   if(this.addDepartment.value.DepartmentHeadId > 0){
  //     this.addDepartment.controls['DepartmentHeadId'].setValue(this.addDepartment.value.DepartmentHeadId);
  //  }
    let suggestionString = event.query;
    this._masterDataService.GetAllAssociateList().subscribe(
      (associateListResponse: GenericType[]) => {
        this.filteredAssociateIds=[];
        this.filteredAssociateIds = this.filteredAssociateId(
          suggestionString,
          associateListResponse
        );
      },
      (error: any) => {
      }
    );
  }

  filteredAssociateId(
    suggestionString: string,
    associateListResponse: GenericType[]
  ): GenericType[] {
    let filtered: GenericType[] = [];
    for (let i = 0; i < associateListResponse.length; i++) {
      let associateResponse = associateListResponse[i];
      if (
        associateResponse.Name
          .toLowerCase()
          .includes(suggestionString.toLowerCase()) == true
      ) {
        filtered.push(associateResponse);
      }
    }
    return filtered;
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

  cancel() {
    this.addDepartment.reset();
    this._departmentService.editMode = false;
    this.btnLabel = "Save";
    this.isEdit = false;
    this.formSubmitted = false;
  }

  ngOnDestroy() {
    // this._departmentService.departmentsEdit.complete();
  }
}
  
