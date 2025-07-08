import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
//import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResourceAllocationDetails } from '../../../reports/models/resourcereportbyproject.model';

@Component({
  selector: 'app-allocated-resources-dialog',
  templateUrl: './allocated-resources-dialog.component.html',
  styleUrls: ['./allocated-resources-dialog.component.scss']
})
export class AllocatedResourcesDialogComponent implements OnInit {

  allocatedResourcesDataSource: MatTableDataSource<ResourceAllocationDetails>;
 // @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['AssociateCode','AssociateName','ClientBillingRoleName','AllocationPercentage','IsPrimaryProject','IsCriticalResource'];

  constructor(private dialogRef: MatDialogRef<AllocatedResourcesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any  ) { }

  ngOnInit(): void {
    this.allocatedResourcesDataSource = this.data.datasource;
   // this.allocatedResourcesDataSource.paginator = this.paginator;
    this.allocatedResourcesDataSource.sort = this.sort;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
