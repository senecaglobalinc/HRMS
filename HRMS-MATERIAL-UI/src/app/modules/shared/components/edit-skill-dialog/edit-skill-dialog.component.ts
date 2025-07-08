import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { themeconfig } from 'src/themeconfig';
import { SkillsService } from 'src/app/modules/onboarding/services/skills.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-edit-skill-dialog',
  templateUrl: './edit-skill-dialog.component.html',
  styleUrls: ['./edit-skill-dialog.component.scss']
})
export class EditSkillDialogComponent implements OnInit {

  proficiencyLevels: SelectItem[] = [];
  totalProficiencyLevelsData;
  editProficiency: FormGroup;
  skillObj: any;
  themeConfigInput = themeconfig.formfieldappearances;
  constructor(private masterService: MasterDataService,
    private route: Router,
    private dialogRef: MatDialogRef<EditSkillDialogComponent>,
    private skillservice: SkillsService,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder) {
      this.skillObj = this.data['skillData'];
  }

  ngOnInit(): void {
    this.GetProficiencyList();
    this.editProficiency = new FormGroup({
      ProficiencyLevelId: new FormControl(this.skillObj['ProficiencyIDByRM']),
      Remarks: new FormControl(this.skillObj['Remarks'])
    });
  }
  GetProficiencyList() {
    this.masterService.GetProficiencyLevels().subscribe(res => {
      this.proficiencyLevels = [];
      this.totalProficiencyLevelsData = [];
      this.totalProficiencyLevelsData = res;
      this.totalProficiencyLevelsData = res;
      // this.indexOfBasic = this.totalProficiencyLevelsData.findIndex((x: any) => x.ProficiencyLevelCode === 'Basic');
      for (let i = 0; i < res.length; i++) {
        this.proficiencyLevels.push({
          label: res[i]["ProficiencyLevelCode"],
          value: res[i]["ProficiencyLevelId"]
        });
      }
    });
  }
  
  Update(){
    let obj = {
      "reportingManagerRating":this.editProficiency.value.ProficiencyLevelId,
      "remarks":this.editProficiency.value.Remarks,
      "employeeId":this.skillObj['EmployeeId'],
      "EmployeeSkillId":this.skillObj['Id']
    };
    this.skillservice.UpdateEmpSkillProficienyByRM(obj).subscribe(res => {
      this._snackBar.open('Skill Updated Successfully', 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    },
    (error)=>{
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    })
    this.close();
    this.route.navigate(['/shared/submitted-skills/'+this.skillObj['EmployeeId']]);

  }
  close() {
    this.dialogRef.close();
  }
}
