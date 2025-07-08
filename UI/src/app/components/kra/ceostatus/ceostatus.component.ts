import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren, ViewChild, Input } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { themeconfig } from "themeconfig";
import { ceostutusdata,krastatusrelation } from "../krajson";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource, MatTable, MatSort } from "@angular/material";
import {MatDialog,} from '@angular/material/dialog';


export interface DialogData {
  comment: string;
  
}
@Component({
  selector: 'app-ceostatus',
  templateUrl: './ceostatus.component.html',
  styleUrls: ['./ceostatus.component.scss'],
  // providers: [
  //   AssociatekraService,
  //   RolemasterService,
  //   MasterDataService,
  //   KRAService,
  //   CustomKRAService
  // ],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})





export class CeostatusComponent implements OnInit {
  @ViewChild('outerSort') sort: MatSort;
  @ViewChildren('innerSort') innerSort: QueryList<MatSort>;
  @ViewChildren('innerTables') innerTables: QueryList<MatTable<any>>;

  comment: string;


  undoDiv:boolean=false;
  undoDivs:boolean=true;
  imgdiv:boolean=false;
  undoDivss:boolean=false;
  
  // submitted = false;

  formGroup: FormGroup;
  themeappeareance = themeconfig.formfieldappearances;


  displayedColumns: string[] = ['departmentname', 'noofrloetypes', 'status','comments','action'];
  dataSource = ceostutusdata;
  
  kraStatusdataSource : MatTableDataSource<any>;

  kraStatusRelationClmns = ['roletypes', 'noofkras', 'status'];
  innerDisplayedColumns=['departmentname', 'metrics', 'ration'];
  constructor(public dialog: MatDialog, private formBuilder: FormBuilder) { }

  ngOnInit() {
  
  this.kraStatusdataSource = new MatTableDataSource(krastatusrelation);
}


// createForm() {
 
//   this.formGroup = this.formBuilder.group({
  
//   });
// }
onclick() {
  // this.submitted = true;
  this.undoDiv=true;
  this.undoDivs=false;
  this.undoDivss=true;
    
 setTimeout(() => {
  this.undoDiv=false;
  this.undoDivs=true;
 
  
}, 24000);


}
clickimg() {
  // this.submitted = true;
  // this.undoDiv=false;
  this.imgdiv=true;
  
  // this.addb=false;

  // this.imgdiv = !this.imgdiv;
  setTimeout(() => {
    this.imgdiv=false;
  }, 9000);
  setTimeout(() => {
    
    this.undoDivss=false;
  }, 200);
  }



}
