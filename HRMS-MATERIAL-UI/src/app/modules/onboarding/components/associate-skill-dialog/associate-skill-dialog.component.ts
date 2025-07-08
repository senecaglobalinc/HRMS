import { Component, OnInit ,Inject} from '@angular/core';
import { MAT_DIALOG_DATA ,MatDialogRef, MatDialog} from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AddSkills } from 'src/app/modules/master-layout/models/skills.model';
@Component({
  selector: 'app-associate-skill-dialog',
  templateUrl: './associate-skill-dialog.component.html',
  styleUrls: ['./associate-skill-dialog.component.scss']
})
export class AssociateSkillDialogComponent implements OnInit {
  dataSource:MatTableDataSource<any>=new MatTableDataSource<any>() ;
  constructor(@Inject(MAT_DIALOG_DATA) public data: {data: any},
  private dialogRef:MatDialogRef<AssociateSkillDialogComponent>) { 
    this.dataSource.data = (this.data.data);
  }

  ngOnInit(): void {
    console.log(this.data.data);
    
  }
  displayedColumns: string[] = [
    'SkillName',
    'Proficiency'    
  ];
  onConfirmClick()
  {
    this.dialogRef.close("");
  }
   closeDialog(message:string){     this.dialogRef.close({event:message}); }

}
