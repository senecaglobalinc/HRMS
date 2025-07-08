import { Component, OnInit } from '@angular/core';
import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-associate-projects',
  templateUrl: './associate-projects.component.html',
  styleUrls: ['./associate-projects.component.scss']
})
export class AssociateProjectsComponent implements OnInit {

  constructor(private _associateExitDashboardService: ExitAnalysisService,private _router: Router) { }
  projects = []
  ngOnInit(): void {
    this.getProjects()
  }
  getProjects(){
    this.projects = this._associateExitDashboardService.GetAssociateProjects()
  }
  goToKtPlan(selectedProject){
    this._router.navigate(['/shared/KtForm/Associate/'+selectedProject.EmpCode+'/'+ selectedProject.projectId]);
  }

}
