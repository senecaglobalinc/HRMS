import {Component, Inject} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { KraDlgData } from '../Models/kra-dlg-data';

@Component({
  selector: 'app-kradialogs',
  templateUrl: './kradialogs.component.html',
  styleUrls: ['./kradialogs.component.scss']
})
export class KradialogsComponent {

  constructor(
    public dialogRef: MatDialogRef<KradialogsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: KraDlgData) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

}
