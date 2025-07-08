import { Component, Inject, OnInit } from '@angular/core';
import { themeconfig } from '../../../../../themeconfig';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig} from '@angular/material/dialog';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { KraCommentService } from 'src/app/modules/kra/services/kra-comment.service';
import { CommentModel } from 'src/app/modules/kra/models/comment.model';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition} from '@angular/material/snack-bar';

@Component({
  selector: 'app-kra-comment',
  templateUrl: './kra-comment.component.html',
  styleUrls: ['./kra-comment.component.scss'],
  providers: [KraCommentService]
})
export class KraCommentComponent {
  themeappeareance = themeconfig.formfieldappearances;
  addComment: FormGroup;
  finYearId: number;
  deptId: number;
  gradeId: number;
  roleId: number;

  constructor(
    private _snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<KraCommentComponent>,
    private _formBuilder: FormBuilder,
    private _kraCommentService: KraCommentService,
    @Inject(MAT_DIALOG_DATA) public data: KraCommentComponent) {
      
    }

  ngOnInit() {
    this.finYearId = this.data.finYearId;
      this.deptId = this.data.deptId;
      this.gradeId = this.data.gradeId;
      this.roleId = this.data.roleId;
      this.addComment = this._formBuilder.group({
        comment: ['', [Validators.required]]
  });
 }
 
 onNoClick(): void {    
  this.dialogRef.close(true);
}

 onAddClick(): void {  
  if (this.addComment.valid) {            
    let commentModel: CommentModel;
    commentModel = new CommentModel();

    commentModel.CommentText = this.addComment.value.comment;
    commentModel.Username = sessionStorage["mail"];
    commentModel.IsCEO = false;
    commentModel.FinancialYearId = this.finYearId;
    commentModel.DepartmentId = this.deptId;
    commentModel.GradeId = this.gradeId;
    commentModel.RoleTypeId = this.roleId;
       
      this._kraCommentService
        .CreateComment(commentModel)
        .subscribe(
          res => {
            this.dialogRef.close(13);
          },
          error => {
            this._snackBar.open(error.error, 'x', {
                      duration: 1000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
          }
        );  
        }
      }
}
