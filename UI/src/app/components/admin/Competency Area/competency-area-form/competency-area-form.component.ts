import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { CompetencyArea } from "../../models/competencyarea.model";
import { CompetencyAreaService } from "../../services/competency-area.service";
import {MessageService} from 'primeng/api';

@Component({
  selector: "app-competency-area-form",
  templateUrl: "./competency-area-form.component.html",
  styleUrls: ["./competency-area-form.component.css"],
  providers: [MessageService]
})
export class CompetencyAreaFormComponent implements OnInit {
  formSubmitted = false;
  helpMsg = "Successfully created";
  btnLabel = "";
  displayDialog = false;
  constructor(private serviceObj: CompetencyAreaService, private messageService: MessageService) {}
  addCompetencyArea: FormGroup;

  ngOnInit() {
    this.addCompetencyArea = new FormGroup({
      CompetencyAreaCode: new FormControl(null, [Validators.required]),
      CompetencyAreaDescription: new FormControl(null, [Validators.required])
    });
    this.serviceObj.editObj.subscribe(data => {
      if (this.serviceObj.editMode == true) {
        this.addCompetencyArea.patchValue(data);
        this.btnLabel = "Update";
      }
    });
    this.btnLabel = "Save";
    this.Reset();
  }

  Reset() {
    this.formSubmitted = false;
    this.addCompetencyArea.reset();
    this.serviceObj.editMode = false;
    this.btnLabel = "Save";
  }

  CreateCompetencyArea() {
    this.formSubmitted = true;
    var createObj = new CompetencyArea();
    createObj.CompetencyAreaCode = this.addCompetencyArea.value.CompetencyAreaCode;
    createObj.CompetencyAreaDescription = this.addCompetencyArea.value.CompetencyAreaDescription;
    if(this.serviceObj.editMode == true){
      createObj.CompetencyAreaId = this.serviceObj.editObj.value.CompetencyAreaId;
      createObj.IsActive = this.serviceObj.editObj.value.IsActive; 
    }
    if (this.addCompetencyArea.valid == true) {
      this.serviceObj
        .CreateCompetencyArea(createObj)
        .subscribe(res => {
          if (res) {
            this.serviceObj.GetCompetencyAreaData();
            if (this.serviceObj.editMode == false)
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Competency area record added successfully.' });
            else
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Competency area record updated successfully.' });
            this.Reset();
          }
        },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });

          });
    }
  }

  SpaceNotAllowed(e: any) {
    if (e.which === 32 && !e.currentTarget.value) e.preventDefault();
  }

  ngOnDestroy() {
    // this.serviceObj.editObj.unsubscribe();
  }

}
