import { Component, OnInit , ViewChild,Inject} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AssociatejoiningService } from '../../services/associatejoining.service';
import { Associate } from '../../models/associate.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroupDirective} from '@angular/forms';
import * as moment from 'moment';
import { NavService } from '../../../master-layout/services/nav.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-associate-joining',
  templateUrl: './associate-joining.component.html',
  styleUrls: ['./associate-joining.component.scss']
})
export class AssociateJoiningComponent implements OnInit {

  associateList : Associate[];  
  selectedRow : Associate;

  displayedColumns: string[] = [
    'Name',
    'TechnologyName',
    'DesignationName',
    'DepartmentCode',
    'JoinDate',
    'Hradvisor',
    'Edit'
  ];

  dataSource: MatTableDataSource<Associate>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  subscription: any;

  constructor(private _associatejoingService : AssociatejoiningService,public navService: NavService,@Inject(Router) private _router: Router,private spinner: NgxSpinnerService){
    this.getAssociatejoiningList();
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
   }

  ngOnInit(): void {
    this.getAssociatejoiningList();
  }
  
  getAssociatejoiningList(){
    this.spinner.show();
    this._associatejoingService.getAssociates().subscribe((res: any) => { this.associateList = res;
      this.associateList.forEach((r:any) => {
          if(r.JoinDate != null)
              r.JoinDate= moment(r.JoinDate).format('YYYY-MM-DD');
          else r.JoinDate="";
          });
          
           this.dataSource = new MatTableDataSource(this.associateList );
           this.dataSource.paginator = this.paginator;
           this.dataSource.sort = this.sort;
           this.spinner.hide();
          this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
            if (typeof data[sortHeaderId] === 'string') {
              return data[sortHeaderId].toLocaleLowerCase();
            }
          
            return data[sortHeaderId];
          };
       });
           
 }

 editAssociatejoining(currentAssociate: Associate) {
   let subType = "profileupdate";
   currentAssociate.associateType = currentAssociate.EmployeeId != 0 ? "edit" : "new";
   this._router.navigate(['/associates/prospectivetoassociate/' + currentAssociate.associateType + '/' + currentAssociate.EmployeeId + '/' + subType]);
}

applyFilter(event: Event) {
  if (event) {
    const filterValue = (event.target as HTMLInputElement).value;
    if (filterValue){
      this.dataSource.filter = filterValue.trim().toLowerCase();
      // this.dataSource = new MatTableDataSource(this.dataSource.filteredData);
      }
      else{
      this.dataSource = new MatTableDataSource(this.associateList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };
      }
  } else {
    this.dataSource = new MatTableDataSource(this.associateList);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
      if (typeof data[sortHeaderId] === 'string') {
        return data[sortHeaderId].toLocaleLowerCase();
      }
    
      return data[sortHeaderId];
    };
  }
}

}
