import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProspectiveAssosiateService } from '../../services/prospective-assosiate.service';
import { NavService } from '../../../master-layout/services/nav.service';
import { Associate } from '../../models/associate.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { FormGroupDirective } from '@angular/forms';

import * as moment from 'moment';
import { themeconfig } from '../../../../../themeconfig';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-prospective-associate',
  templateUrl: './prospective-associate.component.html',
  styleUrls: ['./prospective-associate.component.scss']
})
export class ProspectiveAssociateComponent implements OnInit {
  prosAssociatesList: any[];

  displayedColumns: string[] = [
    'EmpName',
    'Designation',
    'Department',
    'JoiningDate',
    'HRAdvisorName',
    'Edit',
    'Confirm'
  ];

  dataSource: MatTableDataSource<Associate>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private _ProspectiveAssosiateService: ProspectiveAssosiateService, public navService: NavService, private _router: Router, private spinner: NgxSpinnerService) {
    this.getprosAssociatesDetails();
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit(): void {
    this.getprosAssociatesDetails();
  }
  getprosAssociatesDetails() {
    this.spinner.show();
    this._ProspectiveAssosiateService.list().subscribe((res: any) => {
      this.prosAssociatesList = res;
      this.prosAssociatesList.forEach((r: any) => {
        r.joiningDate = moment(r.joiningDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
      });

      this.dataSource = new MatTableDataSource(this.prosAssociatesList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.spinner.hide();
      // this.dataSource.sortingDataAccessor = (data, header) => data[header].toLowerCase();
      this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };

    });
  }

  onEdit(selectedData: any) {
    this._router.navigate(['/associates/editprospectiveassociate/' + selectedData.Id]);
  }

  onAdd() {
    this._router.navigate(['/associates/addprospectiveassociate']);
  }
  onConfirm(currentAssociate: Associate) {
    let subType = "profile";
    currentAssociate.associateType = currentAssociate.EmpId != 0 ? "edit" : "new";
    currentAssociate.Id = currentAssociate.EmpId == 0 ? currentAssociate.Id : currentAssociate.EmpId;
    this._router.navigate(['/associates/prospectivetoassociate/' + currentAssociate.associateType + '/' + currentAssociate.Id + '/' + subType]);
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.dataSource = new MatTableDataSource(this.prosAssociatesList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        // this.dataSource.sortingDataAccessor = (data, header) => data[header].toLowerCase();
        this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
          if (typeof data[sortHeaderId] === 'string') {
            return data[sortHeaderId].toLocaleLowerCase();
          }
        
          return data[sortHeaderId];
        };
      }
    } else {
      this.dataSource = new MatTableDataSource(this.prosAssociatesList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      // this.dataSource.sortingDataAccessor = (data, header) => data[header].toLowerCase();
      this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };
    }
  }
}
