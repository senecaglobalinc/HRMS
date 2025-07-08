import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signout-dialog',
  templateUrl: './signout-dialog.component.html',
  styleUrls: ['./signout-dialog.component.scss']
})
export class SignoutDialogComponent {

  constructor(
    private dialogRef: MatDialogRef<SignoutDialogComponent>,
    private router: Router
  ) { }

  onCancel(): void {
    localStorage.clear();
    sessionStorage.clear();
    this.router.navigate(['/login']);   
    this.dialogRef.close(); 
  }

  
  onClose(): void {
    this.dialogRef.close(); 
  }

}
