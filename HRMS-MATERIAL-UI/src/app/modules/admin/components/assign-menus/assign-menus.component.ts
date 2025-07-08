import { SelectionModel } from '@angular/cdk/collections';
import { Component, ComponentFactoryResolver, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { themeconfig } from 'src/themeconfig';
import { MenuRoles, Menus } from '../../models/menu-roles.model';
import { AssignmenustoroleService } from '../../services/assignmenustorole.service';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-assign-menus',
  templateUrl: './assign-menus.component.html',
  styleUrls: ['./assign-menus.component.scss']
})
export class AssignMenusComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;

  displayedColumnsAvailableMenu: string[] = ['select', 'availablemenus'];
  displayedColumnsSelectedMenu: string[] = ['select', 'selectedmenus'];

  dataSourceAvailableMenus: MatTableDataSource<Menus>;
  selectionAvailableMenus = new SelectionModel<Menus>(true, []);

  dataSourceSelectedMenus: MatTableDataSource<Menus>;
  selectionSelectedMenus = new SelectionModel<Menus>(true, []);
  changeInMenu: boolean = false;

  assignMenus: FormGroup;
  show: boolean = false;

  finalTargetMenu: MenuRoles;

  rolesList: SelectItem[];
  RoleId: number;
  availableMenus: Menus[] = [];
  selectedMenus: Menus[] = [];

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _snackBar: MatSnackBar, private _masterDataService: MasterDataService, private _assignMenusToRoleService: AssignmenustoroleService) { }

  ngOnInit(): void {
    this.getRolesList();

    this.assignMenus = new FormGroup({
      RoleId: new FormControl(null),
    });


  }

  getRolesList(): void {
    this._masterDataService.GetRoles().subscribe((res: any[]) => {
      this.rolesList = [];
      res.forEach(e => {
        this.rolesList.push({ label: e.Name, value: e.ID });
      });
    });
  }

  getRoleId(RoleId: number): void {
    this.show = true;

    this.RoleId = RoleId;
    if (RoleId == null || RoleId == undefined) {

    }
    else {
      this.getSourceMenusList(RoleId);
      this.getTargetMenusList(RoleId);
    }
  }

  getSourceMenusList(RoleId: number): void {
    this._assignMenusToRoleService.getSourceMenus(RoleId).subscribe((menus: Menus[]) => {
      this.availableMenus = [];
      this.availableMenus = menus;
      this.dataSourceAvailableMenus = new MatTableDataSource(this.availableMenus);

    }, (error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000, panelClass: ['error-alert'], horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });
  }

  getTargetMenusList(RoleId: number): void {
    this._assignMenusToRoleService.getTargetMenus(RoleId).subscribe((menus: Menus[]) => {
      this.selectedMenus = [];
      this.selectedMenus = menus;
      this.dataSourceSelectedMenus = new MatTableDataSource(this.selectedMenus);
    }, (error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000, panelClass: ['error-alert'], horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });
  }

  moveAvailabletoSelected(): void {

    this.selectionAvailableMenus.selected.forEach(item => {

      let index: number = this.availableMenus.findIndex(d => d === item);

      this.selectedMenus.push(this.availableMenus[index]);
      this.availableMenus.splice(index, 1);
      this.changeInMenu = true;

    });


    this.selectionAvailableMenus = new SelectionModel<Menus>(true, []);
    this.dataSourceAvailableMenus = new MatTableDataSource<Menus>(this.availableMenus);
    this.dataSourceSelectedMenus = new MatTableDataSource<Menus>(this.selectedMenus);

  }

  moveSelectedtoAvailable(): void {
    this.selectionSelectedMenus.selected.forEach(item => {
      let index: number = this.selectedMenus.findIndex(d => d === item);

      this.availableMenus.push(this.selectedMenus[index]);
      this.selectedMenus.splice(index, 1);
      this.changeInMenu = true;

    });

    this.selectionSelectedMenus = new SelectionModel<Menus>(true, []);
    this.dataSourceSelectedMenus = new MatTableDataSource<Menus>(this.selectedMenus);
    this.dataSourceAvailableMenus = new MatTableDataSource<Menus>(this.availableMenus);

  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelectedAvailable() {
    const numSelected = this.selectionAvailableMenus.selected.length;
    const numRows = this.dataSourceAvailableMenus.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggleAvailable() {
    this.isAllSelectedAvailable() ?
      this.selectionAvailableMenus.clear() :
      this.dataSourceAvailableMenus.data.forEach(row => this.selectionAvailableMenus.select(row));

  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelectedSelected() {
    const numSelected = this.selectionSelectedMenus.selected.length;
    const numRows = this.dataSourceSelectedMenus.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggleSelected() {
    this.isAllSelectedSelected() ?
      this.selectionSelectedMenus.clear() :
      this.dataSourceSelectedMenus.data.forEach(row => this.selectionSelectedMenus.select(row));

  }

  saveTargetMenus(): void {
    this.finalTargetMenu;
    this.finalTargetMenu = new MenuRoles();
    let MenuList: Menus[] = [];
    this.selectedMenus.forEach(menu => {
      MenuList.push({ MenuId: menu.MenuId, MenuName: "" })
    });
    this.finalTargetMenu.RoleId = this.RoleId;
    this.finalTargetMenu.MenuList = MenuList;

    this._assignMenusToRoleService.addTargetMenuRoles(this.finalTargetMenu).subscribe((response: boolean) => {
      if (response == true) {

        this.getTargetMenusList(this.RoleId);
        this.getSourceMenusList(this.RoleId);
        this._snackBar.open('Menu details saved successfully.', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
        this.changeInMenu=false;
      }
    }, error => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });

    });
  }


  cancel(): void {
    this.RoleId = null;
    this.show = false;
    this.assignMenus.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.changeInMenu = false;
  }

  applyFilterAvailableMenu(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSourceAvailableMenus.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceAvailableMenus = new MatTableDataSource(this.availableMenus);
    }
  }

  applyFilterSelectedMenu(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSourceSelectedMenus.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceSelectedMenus = new MatTableDataSource(this.selectedMenus);
    }

  }

}

