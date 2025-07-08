import { Component, OnInit, ViewChild } from '@angular/core';
import { themeconfig } from 'themeconfig';
import { commentkradefindata } from '../krajson';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar, MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { AddKRAdlgComponent } from '../add-kradlg/add-kradlg.component';
import {MatSidenav} from '@angular/material/sidenav';
import { Router } from '@angular/router';

@Component({
  selector: 'app-kra-comments',
  templateUrl: './kra-comments.component.html',
  styleUrls: ['./kra-comments.component.scss']
})
export class KraCommentsComponent implements OnInit {

  @ViewChild('sidenav') sidenav: MatSidenav;

  themeappeareance = themeconfig.formfieldappearances;
  displayedColumns: string[] = ['kraaspect', 'metrics', 'ration'];

  dataSource = new MatTableDataSource(commentkradefindata);
  commentkradefinData;

  importedKradisplayedColumns: string[] = ['select', 'kraaspect', 'metrics', 'ration'];
  selection = new SelectionModel<any>(true, []);

  addNewKRA: boolean = true;
  freezedKRA: boolean = false;
  importedKRA: boolean = false;
  newAddedRow: boolean = false;
  marksAsFinish: boolean = false;
  undoBtn: boolean = false;
  undoDiv: boolean = false;
  deletedTr: boolean = false;
  deletedKRAdiv: boolean = false;
  addedTR:boolean=false;
  dispAddedKRADiv:boolean=false;

  constructor(private router: Router,public dialog: MatDialog, public snackBar: MatSnackBar) { }

  ngOnInit() {
this.sidenav.toggle();
  }


  openAddKRA(e): void {
    if (e == 'add') {
      const dialogRef = this.dialog.open(AddKRAdlgComponent, {
        width: '80vw',
        data: { heading: 'Add KRA', btntext: 'Add' }
      });
      dialogRef.afterClosed().subscribe(result => {    
        this.commentkradefinData.push(result);
        // this.dataSource.push(result);
        // this.dataSource = this.kradefinData;
        this.dataSource = new MatTableDataSource(this.commentkradefinData);
        this.addedTR=true;
      });
    }
    else if (e == "edit") {
      const dialogRef = this.dialog.open(AddKRAdlgComponent, {
        width: '80vw',
        data: { heading: 'Edit KRA', btntext: 'Done' }
      });
      dialogRef.afterClosed().subscribe(result => {      
        this.marksAsFinish = result;
        this.undoBtn = result;
      });
    }
  }


  dispUndo() {
    this.undoDiv = true;

    setTimeout(() => {
      this.undoDiv = false;
      this.undoBtn = true;
      this.marksAsFinish = true;
    }, 3000);

  }
  dispDelKRA() {
    this.deletedKRAdiv = true;

    setTimeout(() => {
      this.deletedKRAdiv = false;
    }, 3000);

  }

  deleteKRA() {
    this.deletedTr = true;
  }
 
  dispAddedKRA() {
    this.dispAddedKRADiv=true;

    setTimeout(() => {
      this.dispAddedKRADiv = false;
    }, 3000);

  }

  closeSideNav() {
    this.sidenav.close();
  }
  

  exitCommentMode(){
    // sessionStorage.setItem('isApproved','true');
    this.router.navigate(["/kra/reviewkra"]);
  }

}
