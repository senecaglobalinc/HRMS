import {Component, OnInit, Input, ChangeDetectorRef, ViewChild, ElementRef} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatStepper } from '@angular/material/stepper';
import { StepperSelectionEvent } from '@angular/cdk/stepper';


@Component({
  selector: 'app-prospective-to-associate',
  templateUrl: './prospective-to-associate.component.html',
  styleUrls: ['./prospective-to-associate.component.scss']
})
export class ProspectiveToAssociateComponent implements OnInit {

  items: any[];
  private subType: string;
  activeIndex: number = 0;
  public Type: string;

  @ViewChild('step1') step1;
  @ViewChild('step8') step8;
  isLinear = false;


  constructor(private _formBuilder: FormBuilder,private _router: Router, private actRoute: ActivatedRoute, private cdr: ChangeDetectorRef) {}

  ngOnInit() {

    this.actRoute.params.subscribe(params => { this.Type = params['type']; });
    this.actRoute.params.subscribe(params => { this.subType = params['subtype']; });
    if(this.Type === 'new')
    {
      this.isLinear = true;
    }
    else{
      this.isLinear = false;
    }

  }
  ngAfterViewChecked(){
    this.cdr.detectChanges();
 }
  stepChanged(event) {
    if (this.Type === 'edit'){
    event.previouslySelectedStep.interacted = false;
    this.activeIndex = event.selectedIndex;

    }
    if(event.selectedIndex === 7){
      this.step8.getPAStatus();
    }
  }
  onSelect(event) {
    if (this.Type === 'new') {
        this.activeIndex = 0;
        event.selectedStep._editable = 'false';
        return;
    }
    this.activeIndex = event.selectedIndex;
}


onAssociateSave(data: any) {
    this.Type = 'edit';
    this.isLinear = false;
}
onBack() {
    if (this.subType == "profile") this._router.navigate(['/associates/prospectiveassociate']);
    else if (this.subType == "profileupdate") this._router.navigate(['/associates/associatejoining']);
    else if (this.subType == "list") this._router.navigate(['/associates/associateinformation']);
    else if (this.subType == "EPU") this._router.navigate(["/shared/dashboard"]);
    else this._router.navigate(["/associates/prospectiveassociate/list"]);
}

goBack(stepper: MatStepper){
  stepper.previous();
}

goForward(stepper: MatStepper){
  stepper.next();
}
}