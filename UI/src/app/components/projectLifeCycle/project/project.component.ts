import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProjectCreationService } from '../services/project-creation.service';
import { Subscription } from 'rxjs';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss'],
  providers: [MessageService]
})
export class ProjectComponent implements OnInit {
  SelectedTab = 0;
  isNewProject = true;
  canSubmit = false;
  selectedTabSubscription: Subscription;
  projectIdSubscription: Subscription;
  subscriptionProjectState: Subscription;
  projectId = 0;
  EmpId : number;
  currentRole = "";
  btnLabel = "";
  canShow = true;
  dashboard : string;
  projectState: string = "";
  constructor(private actRoute: ActivatedRoute,private router: Router, private ProjectCreationServiceObj: ProjectCreationService, private messageService: MessageService) { }

  ngOnInit() {
    this.ProjectCreationServiceObj.SetProjectState(null);
    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.currentRole = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.subscriptionProjectState = this.ProjectCreationServiceObj.GetProjectState().subscribe((state: string) => {
      this.projectState = state;
      if(this.projectState != null){
        if (this.currentRole == "Program Manager" && this.projectState == "Drafted") {
          this.canSubmit = true;
          this.btnLabel = "Submit For Approval";
        }
        else {
          if(this.currentRole == "Department Head" && this.dashboard != "DHDashboard")
            this.btnLabel = "Save";
          else
            this.canShow = false;
        }
      }
    });
    
   
    
    this.projectIdSubscription = this.ProjectCreationServiceObj.GetProjectId().subscribe(data => {
      if (data === 0) {
        this.isNewProject = true;
      }
      else {
        this.projectId = data;
        this.isNewProject = false;
      }
    });
    this.selectedTabSubscription = this.ProjectCreationServiceObj.GetSelectedTab().subscribe((data) => {
      // subscribe to observable to get selected tab by user or by system
      this.SelectedTab = data;
    })
  }
  setSelectedTab(event) {
    this.ProjectCreationServiceObj.SetSeletedTab(event.index);
  }

  SubmitProject() {
    this.ProjectCreationServiceObj.submitForApproval(this.projectId , this.currentRole, this.EmpId ).subscribe(res => {
      if (res > 0) {
        if( this.canSubmit == true )
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Succesfully submitted the project' })
        else
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Project created succesfully' })

        
        setTimeout(() => 
                  {
                    this.router.navigate(['project/dashboard']);
                  }, 1000);
          
      }
      else {
        if( this.canSubmit == true )
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to submitted the project' });
        else
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to create the project' });

      }
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });

      })
  }

  onBack() {
    if(this.dashboard == "DHDashboard" || this.dashboard == "PMDashboard"  )
      this.router.navigate(['shared/dashboard'])  
    else
      this.router.navigate(['project/dashboard'])
  }

  ngOnDestroy() {
    this.ProjectCreationServiceObj.SetSeletedTab(0);
    this.selectedTabSubscription.unsubscribe();
    this.projectIdSubscription.unsubscribe();
    this.subscriptionProjectState.unsubscribe();
  }

}
