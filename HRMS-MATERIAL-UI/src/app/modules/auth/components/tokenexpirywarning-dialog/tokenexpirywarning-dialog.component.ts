import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-tokenexpirywarning-dialog',
  templateUrl: './tokenexpirywarning-dialog.component.html',
  styleUrls: ['./tokenexpirywarning-dialog.component.scss']
})
export class TokenexpirywarningDialogComponent implements OnInit {

  constructor( private dialogRef: MatDialogRef<TokenexpirywarningDialogComponent>,) { }

  ngOnInit(): void {
  }


  onOk(): void {
    this.dialogRef.close(); 
  }
}
