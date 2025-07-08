import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AssociateAllocationService } from '../../services/associate-allocation.service';

@Component({
  selector: 'app-future-allocations-grid',
  templateUrl: './future-allocations-grid.component.html',
  styleUrls: ['./future-allocations-grid.component.scss']
})
export class FutureAllocationsGridComponent implements OnInit, OnChanges {

  @Input() SelectedEmpId: number;
  @Input() updateGrid: Boolean = false;
  futuremarkedprojectObj: any;
  

  constructor(private _allocationservice: AssociateAllocationService,
    private snackBar: MatSnackBar) {
   }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.updateGrid){
      this.GetAssociateFutureProjectByEmpId();
    }
    if(this.SelectedEmpId != undefined && this.SelectedEmpId !=0 && this.SelectedEmpId != null){
      this.GetAssociateFutureProjectByEmpId();
    }
  }
  ngOnInit(): void {
  }
  
  GetAssociateFutureProjectByEmpId(){
    this.futuremarkedprojectObj = [];
    this._allocationservice.GetAssociateFutureProjectByEmpId(this.SelectedEmpId).subscribe(res => {
      if(res){
        this.futuremarkedprojectObj = res;
      }
    })

  }

}
