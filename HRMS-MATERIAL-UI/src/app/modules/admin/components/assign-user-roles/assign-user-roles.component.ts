import { Component, OnInit, ViewChild } from '@angular/core';
import { AssignUserRolesService } from "../../services/assign-user-roles.service";
import { UserRoles } from "../../models/userRoles";
import { FormGroup, FormControl, Validators, FormGroupDirective } from "@angular/forms";
import { Subscription, Observable } from "rxjs";
import { MasterDataService } from "../../../../modules/master-layout/services/masterdata.service";
import { Role } from "../../../../modules/master-layout/models/associate-skills.model";
import { RoleCompetencySkills, RoleData } from "../../../../modules/master-layout/models/role.model";
import { map, startWith } from 'rxjs/operators';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-assign-user-roles',
  templateUrl: './assign-user-roles.component.html',
  styleUrls: ['./assign-user-roles.component.scss']
})
export class AssignUserRolesComponent implements OnInit {

  isCheckedInit = false;
  btnLabel = "Save";
  newRole: FormGroup;
  usersList = [];
  userNamesList = [];
  rolesList = [];
  availableRoles: UserRoles[];
  selectedUserId: number;
  editObj: any;
  roleObj: Role;
  roleEditObj: any;
  userRole: Subscription;
  selectedRole: number;
  addRoleToUser = false;
  formSubmitted = false;
  filteredOptionsName: Observable<any>;
  filteredOptionsRole: Observable<any>;
  assignUserRolesData: UserRoles[];
  dataSource: MatTableDataSource<any>;
  userRolesList: Subscription;
  isRoleFound: UserRoles[];
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['RoleName', 'Edit'];
  searchValue:string;
  constructor(
    public masterService: MasterDataService,
    public assignUserRolesService: AssignUserRolesService, private _snackBar: MatSnackBar, public navService: NavService
  ) {
    this.roleObj = new Role();
    this.userRolesList = this.assignUserRolesService.GetRolesList().subscribe(data => {
      this.assignUserRolesData = data;
     // this.GetUserNames();
      this.GetRolesList();
      this.dataSource = new MatTableDataSource(this.assignUserRolesData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  ngOnInit(): void {
    this.newRole = new FormGroup({
      IsActive: new FormControl(true),
      associateName: new FormControl('', [Validators.required]),
      RoleId: new FormControl('', [Validators.required])

    });
    this.dataSource = new MatTableDataSource();
    this.userRole = this.assignUserRolesService.GetUserRole().subscribe(data => {
      this.newRole.patchValue(data);
      const roleobj = { label: data.RoleName, value: data.RoleId };
      this.newRole['value']['RoleId'] = roleobj;
      if (data.UserId != null)
        this.btnLabel = 'Update';
    });
    this.filteredOptionsName = this.newRole.controls.associateName.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );

    this.filteredOptionsRole = this.newRole.controls.RoleId.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterRole(value))
    );
    this.assignUserRolesData = [];
    this.userRolesList = this.assignUserRolesService.GetRolesList().subscribe(data => {
      this.assignUserRolesData = data;
      this.dataSource = new MatTableDataSource(this.assignUserRolesData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.Reset();
  }

  clearInput(event : any, fieldName: string){
    if (fieldName == 'associateName'){
      event.stopPropagation();
      this.newRole.get('associateName').reset();
      this.newRole.get('RoleId').reset();
      this.dataSource = new MatTableDataSource();
      this.dataSource.paginator = this.paginator;
      this.usersList = [];
      this.filteredOptionsName = this.newRole.controls.associateName.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterName(value))
      );
    }
    else if (fieldName == 'RoleId'){
      event.stopPropagation();
      this.newRole.get('RoleId').reset();
    }
  }


  SetEditObj(editObj): void {
    this.btnLabel = 'Update'
    this.newRole.controls['associateName'].setValue({
      label: editObj.UserName,
      value: editObj.UserId
    });
    this.assignUserRolesService.SetUserRole(editObj);
    this.newRole.controls['RoleId'].setValue({
      label: editObj.RoleName,
      value: editObj.RoleId
    });
    this.newRole.get('RoleId').disable();
    this.newRole.controls['IsActive'].setValue(editObj.IsActive);
    this.roleEditObj = { label: editObj.RoleName, value: editObj.RoleId };
    this.newRole['value']['RoleId'] = this.roleEditObj;
  }

  Reset(): void {
    this.newRole.get('RoleId').enable();
    this.formSubmitted = false;
    this.newRole.get('IsActive').patchValue(true);
    this.newRole.reset({
      IsActive: this.newRole.get('IsActive').value,
    });
    setTimeout(() => this.formGroupDirective.resetForm({
      IsActive: this.newRole.get('IsActive').value,
    }), 0);
    this.btnLabel = "Save";
    this.dataSource = new MatTableDataSource();
    this.dataSource.paginator = this.paginator;
    this.usersList = [];
  }

  ResetOnSaveAndUpdate() : void {
    this.newRole.get('RoleId').enable();
    this.formSubmitted = false;
    this.newRole.get('IsActive').patchValue(true);
    this.newRole.reset({
      IsActive: this.newRole.get('IsActive').value,
      associateName: this.newRole.get('associateName').value,
    });
    setTimeout(() => this.formGroupDirective.resetForm({
      IsActive: this.newRole.get('IsActive').value,
      associateName: this.newRole.get('associateName').value,
    }), 0);
    this.btnLabel = "Save";
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.assignUserRolesData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  GetUserById(): void {
    var i;
    for (i = 0; i < this.userNamesList.length; i++) {
      if (this.userNamesList[i].UserId == this.selectedUserId) {
        this.editObj = this.userNamesList[i];
        return;
      }
    }
  }


  SaveOrUpdateUserRoles(): void {
    this.formSubmitted = true;
    if (this.selectedUserId || this.newRole.valid == true) {
      if (this.roleEditObj || this.newRole.value.RoleId != undefined) {
        this.GetUserById();
        if (this.newRole.value.RoleId != undefined) {
          this.editObj.RoleId = this.newRole.value.RoleId.value;
          this.editObj.IsActive = this.newRole.value.IsActive;
          this.roleObj.RoleName = this.newRole.value.RoleId.label;
        }
        else {
          this.editObj.RoleId = this.roleEditObj.value;
          this.editObj.IsActive = this.newRole.value.IsActive;
          this.roleObj.RoleName = this.roleEditObj.label;
        }

        this.editObj.Role = this.roleObj;
        this.isRoleFound = this.assignUserRolesData.filter(role => role.RoleName == this.editObj.Role.RoleName);
        if (this.newRole.valid) {
          if(this.isRoleFound.length > 0 && this.btnLabel == 'Save'){
            this._snackBar.open('Role Already Exists', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.ResetOnSaveAndUpdate();
          }

          else{
            this.assignUserRolesService
              .SaveOrUpdateUserRoles(this.editObj)
              .subscribe((res: any) => {
                if (res) {
                  this.assignUserRolesService.GetUserRoles(this.editObj.UserId);
                  if (this.btnLabel == 'Save') {
                    this._snackBar.open('Role added successfully.', 'x', {
                      duration: 3000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                    this.ResetOnSaveAndUpdate();
                   }

                  else {
                    this._snackBar.open('Role updated successfully.', 'x', {
                      duration: 3000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                    this.ResetOnSaveAndUpdate();
                  }

                }
                else {

                  this._snackBar.open('Failed to add role', 'x', {
                    duration: 3000,
                    panelClass: ['error-alert'],
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                }
              },
                (error) => {
                  this._snackBar.open(error.error, 'x', {
                    duration: 3000,
                    panelClass: ['error-alert'],
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                });
          }

        }

      }

    }


  }
  isKeydownPressed(e){
    if(e.keyCode===17){
      return;
    }
    else if(e.keyCode===8){
      this.GetUserNames(e)
    }
  }

  isKeyPressed(e){
    if(e.keyCode!==8){
      this.GetUserNames(e)
    }
  }

  GetUserNames(e): void {
    // if(e.type==='paste'){
    //   let clipboardData = e.clipboardData;
    //   this.searchValue = clipboardData.getData('text');
    // }
   
    if(this.searchValue==undefined){
      this.searchValue = e.key
    }else{
      if(e.keyCode===8){
        this.searchValue = e.target.value.slice(0,-1)
      }else{
        this.searchValue = e.target.value+e.key;
      }
      
    }
  if(this.searchValue.length>=3){
    this.assignUserRolesService.GetUserNames(this.searchValue).subscribe((res: any) => {
      // this.userNamesList= [];
      console.log(res);
      this.userNamesList = res;
      this.usersList = [];
      this.userNamesList.forEach(element => {
        this.usersList.push({
          label: element.UserName,
          value: element.UserId
        });
        //console.log(this.usersList);
      });
      this.filteredOptionsName = this.newRole.controls.associateName.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterName(value))
      );
     //console.log(this.usersList);
    });
  }else{
    this.usersList = [];
    this.filteredOptionsName = this.newRole.controls.associateName.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );
  }
    
  }

  GetRolesList(): void {
    this.masterService.GetRoles().subscribe((res: any[]) => {
      this.rolesList = [];
      res.forEach(e => {
        this.rolesList.push({ label: e.Name, value: e.ID });
      });
    });
  }

  GetUserRoles(e): void {
    this.selectedUserId = this.newRole.get('associateName').value.value;
    this.assignUserRolesService.GetUserRoles(this.selectedUserId);
    // if (this.selectedUserId != null) {
    //   this.assignUserRolesService.GetUserRoles(this.selectedUserId);
    // }
  }


  EditMode(): boolean {
    if (this.btnLabel == 'Update')
      return true;
    return false;
  }

  CheckValue(): boolean {
    if (this.newRole.value.RoleId != null)
      return false;
    return true;

  }



  FilterRoles(roleId: number): string {

    var obj = this.rolesList.find((x) => x.value === roleId);
    return obj.label;
  }


  private _filterName(value: any) {
    let filterValue;
    if (value && !value.label && value.length>=3) {
      filterValue = value.toLowerCase();
      return this.usersList.filter((option) => {
        if (option) {
          return option.label.toLowerCase().includes(filterValue)
        }
      }
      );
    } else {
      return this.usersList;
    }
  }
  private _filterRole(value: any) {
    let filterValue;
    if (value && !value.label) {
      filterValue = value;
      return this.rolesList.filter((option) => {
        if (option) {
          return option.label.toLowerCase().includes(filterValue)
        }
      }
      );
    } else {
      return this.rolesList;
    }
  }


  displayFn(user: any) {
    return user && user ? user.label : '';
  }
  displayRolesFn(role: any) {
    return role && role ? role.label : '';
  }

  getFilteredOptionsName() {
    this.filteredOptionsName = this.newRole.controls.associateName.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );
  }


  getFilteredOptionsRole() {
    this.filteredOptionsRole = this.newRole.controls.RoleId.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterRole(value))
    );
  }




}
