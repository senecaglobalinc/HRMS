import { Component, Inject } from '@angular/core';
import { themeconfig } from 'themeconfig';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { KraDlgData } from '../Models/kra-dlg-data';
import { kradefindata } from '../krajson';

import {SelectionModel} from '@angular/cdk/collections';
import {MatTableDataSource} from '@angular/material';

@Component({
  selector: 'app-import-kra-dlg',
  templateUrl: './import-kra-dlg.component.html',
  styleUrls: ['./import-kra-dlg.component.scss']
})
export class ImportKraDlgComponent {
  leaderRole:boolean=true;
  themeappeareance = themeconfig.formfieldappearances;

  displayedColumns: string[] = ['select','kraaspect', 'metrics', 'ration'];
  dataSource = new MatTableDataSource<any>(kradefindata);
  selection = new SelectionModel<any>(true, []);

  constructor(
    public dialogRef: MatDialogRef<ImportKraDlgComponent>,
    @Inject(MAT_DIALOG_DATA) public data: KraDlgData) {}

  onNoClick(): void {
    this.dialogRef.close();
  }


   /** Whether the number of selected elements matches the total number of rows. */
   isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
  changeValue(e){
    if(e.value=="hr"){
      this.leaderRole=false;
    }
    else{
      this.leaderRole=true;
    }
  }

  
}
