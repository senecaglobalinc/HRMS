import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-associate-exit-review-screen',
  templateUrl: './associate-exit-review-screen.component.html',
  styleUrls: ['./associate-exit-review-screen.component.scss']
})
export class AssociateExitReviewScreenComponent implements OnInit {

  EmpId: number;
  roleName: string;
  constructor() { }

  ngOnInit(): void {
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
  }

}
