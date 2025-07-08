import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { PracticeAreaService } from '../../services/practice-area.service';
import { PracticeArea } from '../../../../models/practicearea.model';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-practice-area-form',
  templateUrl: './practice-area-form.component.html',
  styleUrls: ['./practice-area-form.component.css'],
  providers: [MessageService]
})
export class PracticeAreaFormComponent implements OnInit {
  btnLabel : string = "";
  isEdit : boolean;
  addPracticeArea : FormGroup;
  formSubmitted = false;
  
  constructor(private _practiceAreaService : PracticeAreaService, private messageService: MessageService) {
   }

  ngOnInit() {
    this.addPracticeArea = new FormGroup({
      PracticeAreaCode : new FormControl(null,[
        Validators.required,
        // Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
      PracticeAreaDescription : new FormControl(null,[
        Validators.required,
        // Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
    });

    this._practiceAreaService.practiceAreaEdit.subscribe(data => {
      if (this._practiceAreaService.editMode == true) {
        this.isEdit = this._practiceAreaService.editMode;
        this.addPracticeArea.patchValue(data);
        this.btnLabel = "Update";
      }
    });
    this.btnLabel = "Save";
    this.cancel();
  }
  addpracticeAreas(): void {
    this.formSubmitted = true;
    var practiceArea = new PracticeArea();
    practiceArea.PracticeAreaCode = this.addPracticeArea.value.PracticeAreaCode;
    practiceArea.PracticeAreaDescription = this.addPracticeArea.value.PracticeAreaDescription;
    if (this._practiceAreaService.editMode == true) {
      practiceArea.PracticeAreaId = this._practiceAreaService.practiceAreaEdit.value.PracticeAreaId;
      practiceArea.IsActive = this._practiceAreaService.practiceAreaEdit.value.IsActive;
    }
    if (this.addPracticeArea.valid == true) {
      this._practiceAreaService.createPracticeAreas(practiceArea).subscribe((res: number) => {
        if (res != null) {
          this._practiceAreaService.getPracticeAreas();
          if (this._practiceAreaService.editMode == false)
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Practice area record added successfully.' });
          else
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Practice area record updated successfully.' });
          this.cancel();
        }
      },
        error => {
          this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: error.error });

        });

    }
    else {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      // this.cancel();
    }
  }

  cancel() : void {
    this.formSubmitted = false;
    this.addPracticeArea.reset();
    this._practiceAreaService.editMode = false;
    this.btnLabel = "Save";
    this.isEdit = false;
  }

  ngOnDestroy() {
    // this._practiceAreaService.practiceAreaEdit.complete();
  }
}



