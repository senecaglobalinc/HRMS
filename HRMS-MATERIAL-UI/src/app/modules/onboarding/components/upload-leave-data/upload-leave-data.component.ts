import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { environment } from '../../../../../environments/environment';
import * as servicePath from '../../../../core/service-paths';
import { RegularizationService } from '../../services/regularization.service';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-upload-leave-data',
  templateUrl: './upload-leave-data.component.html',
  styleUrls: ['./upload-leave-data.component.scss']
})


export class UploadLeaveDataComponent implements OnInit {
  progress : number;
  fileName='';
  uploadSub: Subscription;
 formData = new FormData(); 
 fileUploaded : boolean =false;

 serverUrl =environment.EmployeeMicroService;
 resourceUrl = servicePath.API.uploadLeaveData
 
  
  @ViewChild('fileUpload') fileUpload: any;

  constructor(private http : HttpClient,
    private _snackBar : MatSnackBar,
    private service : RegularizationService,
    private fileExporterService :  FileExporterService) { }

  ngOnInit(): void {
  }

  onFileSelected(event) {
    this.formData = new FormData();
    const file:File = event.target.files[0];

    if (file) {
        this.fileName = file.name;
        this.formData.append("file",file);
    }
}

uploadFile(){
  this.fileUploaded = true;
  const upload$ = this.http.post(this.serverUrl + this.resourceUrl.UploadLeaveData, this.formData
)
.pipe(
    finalize(() => this.reset())
);

this.uploadSub = upload$.subscribe(event => {
  this._snackBar.open(
    'File Uploaded Successfully',
    'x',
    {
      duration: 3000,
      panelClass : ['success-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    }
  );
},(error)=>{
  this._snackBar.open(
   error.error,
    'x',
    {
      duration: 3000,
      panelClass : ['error-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    }
  );
})
}

cancelUpload() {
  if(this.uploadSub){
    this.uploadSub.unsubscribe();
  }
  this.reset();
}

reset() {
  this.fileUploaded = false;
  this.fileName = null;
  this.uploadSub = null;
}

downloadTemplate(){
  this.service.getTemplateFile().subscribe((res:any)=>{

    const source = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + res.Content;

    const link = document.createElement("a");

    link.href = source;

    link.download = res.FileName;

    link.click();  

  })
}
}

