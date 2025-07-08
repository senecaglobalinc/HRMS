import { Component, OnInit } from '@angular/core';
import { ProficiencyLevelService } from '../../services/proficiency-level.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ProficiencyLevel } from '../../models/proficiencyLevel.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-proficiency-level-form',
  templateUrl: './proficiency-level-form.component.html',
  styleUrls: ['./proficiency-level-form.component.css'],
  providers: [MessageService]
})
export class ProficiencyLevelFormComponent implements OnInit {
  btnLabel: string;
  valid = true;
  addProficiencyLevel: FormGroup;
  formSubmitted = false;
  constructor(private serviceObj: ProficiencyLevelService,private  messageService: MessageService) { }

  ngOnInit() {
    this.addProficiencyLevel = new FormGroup({
      "ProficiencyLevelCode": new FormControl(null, [Validators.required]),
      "ProficiencyLevelDescription": new FormControl(null, [Validators.required])
    });
    this.btnLabel = "Save";
    this.serviceObj.editObj.subscribe((data) => {
      if (this.serviceObj.editMode == true) {
        this.addProficiencyLevel.patchValue(data);
        this.btnLabel = "Update";
      }
    })
    this.Reset();
  }

  CreateProficiencyLevelData(): void {
    this.formSubmitted = true;
    var creatingObj = new ProficiencyLevel();
    if (this.addProficiencyLevel.valid == true) {
      creatingObj.ProficiencyLevelCode = this.addProficiencyLevel.value.ProficiencyLevelCode;
      creatingObj.ProficiencyLevelDescription = this.addProficiencyLevel.value.ProficiencyLevelDescription;
      if (this.serviceObj.editMode == true) {
        creatingObj.IsActive = this.serviceObj.editObj.value.IsActive;
        creatingObj.ProficiencyLevelId = this.serviceObj.editObj.value.ProficiencyLevelId;
      }
      this.serviceObj.createProficiencyLevelData(creatingObj)
        .subscribe(res => {
          if (res) {
            this.serviceObj.getProficiencyLevelData();
            if (this.serviceObj.editMode == false)
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Proficiency level record added  successfully.' });
            else
              this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Proficiency level record updated  successfully.' });
            this.Reset();
          }
          else {
            this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add proficiency level.' });
            // this.Reset();
          }
        },
      error=>{
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });

      });
    }
    else {
      //this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid data' });
      // this.reset();
    }

  }

  Validate(value): void {
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
  }
  Reset(): void {
    this.formSubmitted = false;
    this.addProficiencyLevel.reset();
    this.btnLabel = "Save";
    this.serviceObj.editMode = false;
  }
  ngOnDestroy() {
    // this.serviceObj.editObj.unsubscribe();
  }
}
