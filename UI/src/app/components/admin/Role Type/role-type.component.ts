import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GradesService } from '../services/grades.service';
import { RoleTypeService } from '../services/role-type.service';
import { MessageService } from 'primeng/api';
import { Roletype } from '../models/roletype.model';

@Component({
  selector: 'app-role-type',
  templateUrl: './role-type.component.html',
  styleUrls: ['./role-type.component.scss'],
  providers: [MessageService]
})
export class RoleTypeComponent implements OnInit {
  buttonTitle: string = 'Save';
  roleTypeForm: FormGroup;
  gradeList: any;
  dataSource: any;
  editMode : boolean = false;
  displayedColumns: string[] = ['Grade', 'RoleType', 'Description', 'Edit'];
  constructor(private fb: FormBuilder, private _gradeService: GradesService,
    private _roleTypeService: RoleTypeService,
    private _message: MessageService) { }

  ngOnInit() {
    this.clear();
    this.GetGrades();
    this.GetRoleTypes();
  }

  GetGrades() {
    this._gradeService.getGradesDetails();
    this._gradeService.GradesData.subscribe((data) => {
      this.gradeList = data;
    });
  }
  GetRoleTypes() {
    this._roleTypeService.GetRoleTypes().subscribe((data) => {
      this.dataSource = data;
    });
  }
  onSubmit() {
    if (this.roleTypeForm.valid) {
      if (this.buttonTitle == 'Save') {
        this.roleTypeForm.value.RoleTypeName.trim();
        this._roleTypeService.CreateRoleTypes(this.roleTypeForm.value).subscribe(response => {
          this.GetRoleTypes();
          this.clear();
          this.Message('success', 'Success Message', 'Role Type added successfully.');
        }, err => {
          this.Message('error', 'Error Message', err);
        });
      }
      else {
        this._roleTypeService.UpdateRoleTypes(this.roleTypeForm.value).subscribe(response => {
          this.GetRoleTypes();
          this.clear();
          this.Message('success', 'Success Message', 'Role Type updated successfully.');
        }, err => {
          this.Message('error', 'Error Message', err);
        });
      }
    }
  }
  onEdit(data: Roletype) {
    this.roleTypeForm.patchValue(data);
    this.buttonTitle = 'Update';
    this.editMode = true;
    this.roleTypeForm.controls.RoleTypeName.disable();
  }

  Delete(id) {
    this._roleTypeService.DeleteRoleTypes(id).subscribe(response => {
      this.GetRoleTypes();
    }, err => {

    });
  }
  Message(status: string, msg: string, detail: any) {
    if (status == "success") {
      this._message.add({ severity: status, summary: msg, detail: detail });
    }
    else {
      this._message.add({ severity: status, summary: msg, detail: detail.error });
    }
  }
  clear() {
    this.editMode = false;
    this.roleTypeForm = this.fb.group({
      RoleTypeId: [0],
      GradeId: [0, Validators.required],
      GradeRoleTypeId: [0],
      RoleTypeName: ['', [Validators.required, Validators.maxLength(30), Validators.pattern('^[a-zA-Z \-\']+')]],
      RoleTypeDescription: ['', [Validators.required, Validators.maxLength(500)]],
      IsActive: [true]
    });
    this.roleTypeForm.controls.RoleTypeName.enable();
    this.buttonTitle = 'Save';
  }
}