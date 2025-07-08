
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { TalentPoolReportData } from '../models/talentpool.model';
import { AssociateProjectHistoryService } from '../services/associate-project-history.service';

@Component({
  selector: 'app-associate-project-history',
  templateUrl: './associate-project-history.component.html',
  styleUrls: ['./associate-project-history.component.scss']
})

export class AssociateProjectHistoryComponent implements OnInit {
  projectHistory :TalentPoolReportData[];
    userProjectHistory : Subscription;
    constructor (
      private associateProjectHistoryService : AssociateProjectHistoryService) {
   }

  ngOnInit() {
    this.projectHistory = [];
    this.userProjectHistory  = this.associateProjectHistoryService.userProjectHistory.subscribe(data=>{
       this.projectHistory = data;
    });
  }

  ngOnDestroy() {
    this.userProjectHistory.unsubscribe();
  }
  
}

