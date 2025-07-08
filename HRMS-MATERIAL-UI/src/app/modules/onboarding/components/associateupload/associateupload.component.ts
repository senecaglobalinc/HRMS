import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Router, ActivatedRoute, } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UploadService } from '../../services/upload.service';
import * as servicePath from '../../../../core/service-paths';
//import { MessageService } from 'primeng/api';
import { environment } from '../../../../../environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormGroup, FormControl } from '@angular/forms';
//import { ConfirmationService } from 'primeng/api';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import {MatDialog} from '@angular/material/dialog';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-associateupload',
  templateUrl: './associateupload.component.html',
  styleUrls: ['./associateupload.component.scss']
})
export class AssociateuploadComponent implements OnInit {

  uploaddetails: any;
  dialogResponse:boolean;
    id: number;
    filedata: any;
    PAstatus: any;
    uploaddata: any[] = [];
    // private _resources: any;
    private _resources = servicePath.API.upload;
    // private _serverURL: string;
    private _serverURL = environment.EmployeeMicroService;
    private selectedUploadDetails: any;
    @ViewChild('messageToaster') messageToaster: any;
    @ViewChild('fileUpload') fileUpload: any;
    @ViewChild('uploadDialog') uploadDialog: any;
    @ViewChild('fileInput') fileInputVariable: any;
    @ViewChild('fileName') fileNameVariable: any;
    index: number;

    selectedFile: String = "";
    documentDesc: any = { file: "" };
    displayedColumns: string[] = [
      'Sno',
      'DocumentName',
      'Delete',
    ];
    fileGroup: FormGroup;

    constructor(
        @Inject(HttpClient) private _http: HttpClient, public dialog: MatDialog,
        @Inject(UploadService) private _service: UploadService,
        @Inject(Router) private _router: Router,
        private actRoute: ActivatedRoute,    private _snackBar: MatSnackBar,
        private spinner: NgxSpinnerService
    ) {

    }

    GetFileUploadData() {
        this._service.GetUploadData(this.id).subscribe((res: any) => {
            this.spinner.hide()
            this.uploaddata = res;
        },(error)=>{
            this.spinner.hide()
        });
    }
    documentUploadChange(fileURL: string) {
        this.selectedFile = fileURL.substr(12, fileURL.length);
    }

    ngOnInit() {
        this.spinner.show()
        this.actRoute.params.subscribe(params => { this.id = params['id']; });
        // this.id=218;
        this.GetFileUploadData();
        this.getPAStatus();
        this.fileGroup = new FormGroup(
            {
                SelectedFileShow: new FormControl(null),
                uploadselect: new FormControl(null)
            }
        );
    }

    onSave() {
        this.filedata = this.fileUpload.nativeElement;
        this.GetFileUploadData();
        if (this.filedata.files.length > 0) {
            for (var count = 0; count < this.uploaddata.length; count++) {
                if (this.uploaddata[count].FileName == this.filedata.files[0].name) {
                    this._snackBar.open(this.uploaddata[count].FileName + ' already exists !', 'x', {
                        duration: 1000,
                        panelClass:['error-alert'],
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                      });

                  //  this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: this.uploaddata[count].FileName + " already exists !" });
                    //  swal("", this.uploaddata[count].FileName + " already exists !", "warning");
                    return false;
                }
            }
            let formData: FormData = new FormData(),
                xhr: XMLHttpRequest = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) {
                    if (xhr.responseText === "true") {
                        // alert("Document uploaded successfully");
                        //    this.messageService.add({severity:'success', summary: 'success Message', detail:'Document uploaded successfully'});
                        // swal("", "Document uploaded successfully", "success");
                        return true;
                    }
                    else {
                        // alert("Some thing went wrong, please try again later");
                        // this.messageService.add({severity:'error', summary: 'error', detail:'Some thing went wrong, please try again later'});
                        // swal("", "Some thing went wrong, please try again later", "error");
                        return true;
                    }
                }
            }
            formData.append('UploadedFiles', this.filedata.files[0]);
            formData.append('FileName', this.filedata.files[0].name);
            formData.append('EmployeeId', this.id.toString());
            xhr.open('POST', this._serverURL + this._resources.save, false);
            // xhr.open('POST', this._serverURL + this._resources.save, false);
            if (sessionStorage["token"]) {
                xhr.setRequestHeader("Authorization", "bearer " + sessionStorage["token"]);
            }
            xhr.send(formData);

            this.GetFileUploadData();
            this.fileInputVariable.nativeElement.value = "";
            this.fileNameVariable.nativeElement.value = "";
            return false;
        }
        else {
          this._snackBar.open('Please select file to upload', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //this.messageService.add({ severity: 'warn', summary: 'warning message', detail: 'Please select file to upload' });
            // swal("", "Please select file to upload", "warning");
            return false;
        }

    }
    callMessageService() {
        this._snackBar.open('Some thing went wrong, please try again later', 'x', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });  
      //this.messageService.add({ severity: 'error', summary: 'error', detail: 'Some thing went wrong, please try again later' });
    }
   /* OpenConfirmationDialog() {   // method to open dialog
        this._dialogservice.confirm({
            message: 'Are you sure, you want to delete this?',
            accept: () => {
                this.delete(this.id, this.selectedUploadDetails.Id);
            },
            reject: () => {
                this.onCancel();

            }
        });
    }*/

    openDialog(Heading,Message,id, selectedUploadDetailsId): void {
        const dialogRef = this.dialog.open(CommonDialogComponent, {
          width: '300px',
          data: {heading: Heading, message: Message},
          disableClose: true 
        });
    
        dialogRef.afterClosed().subscribe(result => {
           this.dialogResponse = result;
           if(this.dialogResponse==true){
            this.delete(id, selectedUploadDetailsId);
            const dialogConf = this.dialog.open(MessageDialogComponent, {
                width: '300px',
                disableClose: true ,
                data: {heading: 'Confirmation', message: 'Uploaded document deleted successfully'}
              })
          }
        });
      }
    
    delete(id: number, selectedUploadDetails: number) {
        this._service.Delete(id, selectedUploadDetails).subscribe((data1: any) => {
            this.GetFileUploadData();
        }, (error) => {
            this._snackBar.open('Some thing went wrong, please try again later', 'x', {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
          // this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
        });
        //this.uploadDialog.nativeElement.close();
    }


    onDelete(selectedData: any) {
        this.selectedUploadDetails = selectedData;
        this.openDialog('Confirmation','Do you want to Delete ?',this.id, this.selectedUploadDetails.Id);
    }


    onCancel() {
        this.uploadDialog.nativeElement.close();
    }
    getPAStatus() {
        
        this._service.GetPAstatus(this.id).subscribe(data => {
            this.PAstatus = data;
        }, (error) => {
            this._snackBar.open('Some thing went wrong, please try again later', 'x', {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
          //this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
        });
    }

    // submit_approval() {
    //        var url=this._serverURL + this._resources.submitForApproval + this.id;
    //     this._http.post(url, {

    //     }).subscribe((res) => {
    //         this.getPAStatus();
    //         this.messageService.add({severity:'success', summary: 'success message', detail:'Employee profile submitted successfully'});
    //         // swal("", "Employee profile submitted successfully", "success");
    //     }, (error) => {
    //         this.messageService.add({severity:'error', summary: 'Failure message', detail:'Something went wrong.\nPlease try after some time'});
    //         // swal("", "Something went wrong.\nPlease try after some time", "error");
    //     });
    // }
    submit_approval() {
        this.spinner.show()
        this._service.SubmitForApproval(this.id).subscribe((res) => {
            this.spinner.hide();
            this.getPAStatus();
            this._snackBar.open('Employee data submitted successfully', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: 'success', summary: 'success message', detail: 'Employee profile submitted successfully' });
        }, (error) => {
            this.spinner.hide();
            this._snackBar.open('Some thing went wrong, please try again later', 'x', {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }); 
          // this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
            // swal("", "Something went wrong.\nPlease try after some time", "error");
        });

    }



}
