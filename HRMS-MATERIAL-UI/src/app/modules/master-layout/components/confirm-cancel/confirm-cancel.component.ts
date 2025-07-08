import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Router }from "@angular/router";   

interface RouteTo {
  route: string;
}

@Component({
  selector: 'app-confirm-cancel',
  templateUrl: './confirm-cancel.component.html',
  styleUrls: ['./confirm-cancel.component.scss']
})
export class ConfirmCancelComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ConfirmCancelComponent>,
    @Inject(MAT_DIALOG_DATA) public route: RouteTo,private _router: Router) { }
    navigateTo:any
    onCancel(): void {
      this._router.navigate([this.navigateTo]);
      this.dialogRef.close(); 
    }

  ngOnInit(): void {
    this.navigateTo = this.route.route 
  }

}
