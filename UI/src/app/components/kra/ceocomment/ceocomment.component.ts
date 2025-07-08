import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

export interface DialogData {
  comment: string;

}


@Component({
  selector: 'app-ceocomment',
  templateUrl: './ceocomment.component.html',
  styleUrls: ['./ceocomment.component.scss']
})


export class CeocommentComponent implements OnInit {

 
  constructor(
    public dialogRef: MatDialogRef<CeocommentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData){}

  ngOnInit() {
  }

  showDiv = {
    previous : false,
    current : true,
    next : false
  }
}
