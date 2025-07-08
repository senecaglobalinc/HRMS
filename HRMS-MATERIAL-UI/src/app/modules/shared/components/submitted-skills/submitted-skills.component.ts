import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SkillsService } from '../../../onboarding/services/skills.service';
import { SkillData } from '../../../master-layout/models/skills.model'
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';
import { EditSkillDialogComponent } from '../edit-skill-dialog/edit-skill-dialog.component';

export interface PeriodicElement {
  skillname: string;
  proficiency: string;
  experience: number;
  lastused: string;
}


@Component({
  selector: 'app-submitted-skills',
  templateUrl: './submitted-skills.component.html',
  styleUrls: ['./submitted-skills.component.scss']
})
export class SubmittedSkillsComponent implements OnInit {

  id: number;
  displayedColumns: string[] = [
    'SkillName',
    'Version',
    'Proficiency',
    'IsPrimary',
    'Experience',
    'LastUsed',
    'Remarks',
    'Edit',
  ];
  dataSource = new MatTableDataSource<SkillData>();
  skills: SkillData[];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  
  constructor(private skillService:SkillsService,
    private route: Router,
    private actRoute: ActivatedRoute,
    public dialog: MatDialog,
    private _snackBar: MatSnackBar) {
    this.actRoute.params.subscribe(params => { this.id = params.id; });
    this.getEmployeeSkillById();
  }

  ngOnInit(): void {
    this.getEmployeeSkillById();
  }
  getEmployeeSkillById(){
    this.skillService.getNewSkillsByEmployee(this.id).subscribe((res : SkillData[]) => {
      this.skills = res;
      this.dataSource = new MatTableDataSource(this.skills);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }
  editSkill(skillObj) {
    const dialogRef = this.dialog.open(EditSkillDialogComponent, {
      width: '500px',
      data: {
        skillData:skillObj
      },
      disableClose: true 
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getEmployeeSkillById();
    });
  }

  Approve() {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      width: '300px',
      data: {heading: 'Confirmation', message: 'Are you sure you want to Approve?'},
      disableClose: true 
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result==true){
        this.skillService.ApproveSkillRM(this.id).subscribe(res => {
          if(res == "Successfully Approved all the submitted skills"){
            this.skillService.updateEmpSkillDetailsByRM(this.id).subscribe(res => {
              this._snackBar.open('Skills Approved Succesfully', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top'
              });
              setTimeout(() => {
                this.route.navigate(['/shared/dashboard']);
              }, 1000);
            },
            (error) => {
              this._snackBar.open(error.error, 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top'
              });
            })
          }
          else {
            this._snackBar.open('Failed to Update', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top'
            });
          }
        },
        (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });
        })  
      }
    });
  }
}
