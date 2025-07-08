import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { AssociateAllocationService } from '../../talentmanagement/services/associate-allocation.service';
import { AssociateAllocation } from '../../talentmanagement/models/associateallocation.model';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { TagAssociateList } from '../models/tag-associate.model';
import { TagAssociateService } from '../services/tag-associate.service';


@Component({
  selector: 'app-tag-associate',
  templateUrl: './tag-associate.component.html',
  styleUrls: ['./tag-associate.component.scss'],
  providers: [TagAssociateService, MessageService, ConfirmationService]
})
export class TagAssociateComponent implements OnInit {
  allocationHistory: AssociateAllocation[] = [];
  tagListDetails: TagAssociateList[] = [];
  private managerId: number = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
  rowGroupMetadata: any;
  availableAllocationPercentage: number = 0;
  hideAllocate: boolean = false;

  constructor(private tagAssociateService: TagAssociateService, private confirmationService: ConfirmationService,
    private _service: AssociateAllocationService, private messageService: MessageService, private router: Router) { }

  ngOnInit() {
    this.getTagListsByManagerId(this.managerId);
  }

  getTagListsByManagerId(managerId) {
    this.tagAssociateService.GetTagListDetailsByManagerId(managerId).subscribe((res: TagAssociateList[]) => {
      this.tagListDetails = res;
      this.updateRowGroupMetaData();
    },
      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to get the Tagged Associates' })
      }
    )
  }

  updateRowGroupMetaData() {
    this.rowGroupMetadata = {};
    if (this.tagListDetails) {
      for (let i = 0; i < this.tagListDetails.length; i++) {
        let rowData = this.tagListDetails[i];
        let tagListName = rowData.TagListName;
        if (i == 0) {
          this.rowGroupMetadata[tagListName] = { index: 0, size: 1 };
        }
        else {
          let previousRowData = this.tagListDetails[i - 1];
          let previousRowGroup = previousRowData.TagListName;
          if (tagListName === previousRowGroup)
            this.rowGroupMetadata[tagListName].size++;
          else
            this.rowGroupMetadata[tagListName] = { index: i, size: 1 };
        }
      }
    }
  }

  onDeleteClick(selectedTagList: TagAssociateList): void {
    // this.messageService.clear();
    // this.messageService.add({key: 'c', sticky: true, severity:'warn', summary:'Are you sure?', detail:'Confirm to proceed'});
    this.confirmationService.confirm({
      message: 'Do you want to delete the tagged Associate?',
      accept: () => {
        if (selectedTagList.Id > 0) {
          this.tagAssociateService.DeleteTagList(selectedTagList.Id).subscribe((res: number) => {
            if (res >= 1) {
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate deleted successfully from Tag list' })
              this.getTagListsByManagerId(this.managerId);
            }
            else {
              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete associate from Tag list.' })
            }
          },
            (error: any) => this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete associate from Tag list.' })
          );
        }
      },


      reject: () => { }
    });

  }

  deleteWholeTagList(selectedTagList: TagAssociateList){
    this.confirmationService.confirm({
      message: 'Do you want to delete whole tag list?',
      accept: () => {       
        if (selectedTagList.TagListName) {
          this.tagAssociateService.DeleteWholeTagList(selectedTagList.TagListName, this.managerId).subscribe((res: number) => {
            if (res >= 1) {
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Tag List deleted successfully.' })
              this.getTagListsByManagerId(this.managerId);
            }
            else {
              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete Tag list.' })
            }
          },
            (error: any) => this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete Tag list.' })
          );
        }
      },


      reject: () => { }
    });
  }

  gotoAllocation(selectedTagList: TagAssociateList) {
    this.router.navigate(['/talentmanagement/allocatefromtagassociate/' + selectedTagList.Id + '/' + selectedTagList.EmployeeId]);
  }

  BackToSkillSearch(): void {
    this.router.navigate(['/project/associateSkillSearch/']);
  }
}
