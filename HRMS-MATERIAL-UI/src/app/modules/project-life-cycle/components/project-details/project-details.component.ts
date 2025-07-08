import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, NavigationEnd, Router, RoutesRecognized } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs-compat/operator/filter';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { ProjectCreationService } from 'src/app/modules/project-life-cycle/services/project-creation.service';
import { UrlService } from 'src/app/modules/shared/services/url.service';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss']
})
export class ProjectDetailsComponent implements OnInit {
  
  EmpId: number;
  fullName: string;
  roleName:string;
  dashboard: string;
  projectId: number;
  showback: boolean = true;
  pageload: boolean = false;

  projectIdSubscription: Subscription;
  projectData: ProjectsData;
  projectState: string;
  projectStateSubscription: Subscription;
  previousUrl: string;

  constructor(private actRoute: ActivatedRoute,
    private projectCreationService: ProjectCreationService,
    private _snackBar: MatSnackBar,
    private router: Router,
    private urlService: UrlService
    ) { }

  ngOnInit(): void {
    this.urlService.previousUrl$.subscribe((previousUrl: string) => {
      this.previousUrl = previousUrl;
    });
    this.EmpId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.fullName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).fullName;
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;

    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.projectStateSubscription = this.projectCreationService.GetProjectState().subscribe(data => {
      this.projectState = data;
    });
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
    });

    this.GetProjectDetails();
  }

  GetProjectDetails() {
    if (this.projectId > 0) {
      this.GetProjectByID(this.projectId);
    }
  }
  private GetProjectByID(currentProjectID: number): void {
    this.projectCreationService
      .GetProjectDetailsbyID(currentProjectID)
      .toPromise().then(
        (projectdata: ProjectsData) => {
          this.projectData = projectdata;
          this.pageload = true;
        }).catch(
        error => {
          this._snackBar.open("Error while getting the project details", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }

  onBack() {
    this.router.navigate([this.previousUrl]);
    this.showback = false;
  }

}
