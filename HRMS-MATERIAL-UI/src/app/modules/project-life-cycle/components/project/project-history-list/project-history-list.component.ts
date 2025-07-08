import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectsData } from '../../../models/projects.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { AssociateAllocationService } from 'src/app/modules/TalentMangment/services/associate-allocation.service';

@Component({
  selector: 'app-project-history-list',
  templateUrl: './project-history-list.component.html',
  styleUrls: ['./project-history-list.component.scss']
})
export class ProjectHistoryListComponent implements OnInit {

  dataSource = new MatTableDataSource<any>();
  ProjectsData : any;
  empId: number;
  empName: string;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns1: string[] = [
    'ProjectName',
    'ClientName',
    'EffectiveDate',
    'ReleaseDate',
    'IsBillable'
  ];
  constructor(public dialogRef: MatDialogRef<ProjectHistoryListComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{EmployeeId:number, EmployeeName: string}, public allocationService: AssociateAllocationService) { }

  ngOnInit(): void {
    this.empId = this.data.EmployeeId;
    this.empName = this.data.EmployeeName;
    this.allocationService.GetAllAllocationByEmployeeId(this.empId).subscribe(res => {
      this.ProjectsData = res['Items'];
      this.ProjectsData.forEach((record) => {            
        record.IsBillable = (record.IsBillable == true) ? 'Yes' : 'No'; 
      });
      this.dataSource =  new MatTableDataSource(this.ProjectsData);
    })
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
