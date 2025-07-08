import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Router, ActivatedRoute, } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UploadService } from '../services/upload.service';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { environment } from '../../../../environments/environment';
import { ConfirmationService } from 'primeng/api';
@Component({
    selector: 'app-assosiateupload',
    templateUrl: './assosiateupload.component.html',
    styleUrls: ['./assosiateupload.component.scss'],
    providers: [UploadService, MessageService, ConfirmationService]
})
export class AssosiateuploadComponent implements OnInit {

    uploaddetails: any;
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

    constructor(
        @Inject(HttpClient) private _http: HttpClient,
        private messageService: MessageService,
        private _dialogservice: ConfirmationService,
        @Inject(UploadService) private _service: UploadService,
        @Inject(Router) private _router: Router,
        private actRoute: ActivatedRoute) {

    }

    GetFileUploadData() {
        this._service.GetUploadData(this.id).subscribe((res: any) => {
            this.uploaddata = res;
        });
    }
    documentUploadChange(fileURL: string) {
        this.selectedFile = fileURL.substr(12, fileURL.length);
    }

    ngOnInit() {
        this.actRoute.params.subscribe(params => { this.id = params['id']; });
        this.GetFileUploadData();
        this.getPAStatus();
    }

    onSave() {
        this.filedata = this.fileUpload.nativeElement;
        this.GetFileUploadData();
        if (this.filedata.files.length > 0) {
            for (var count = 0; count < this.uploaddata.length; count++) {
                if (this.uploaddata[count].FileName == this.filedata.files[0].name) {
                    this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: this.uploaddata[count].FileName + " already exists !" });
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
            this.messageService.add({ severity: 'warn', summary: 'warning message', detail: 'Please select file to upload' });
            // swal("", "Please select file to upload", "warning");
            return false;
        }

    }
    callMessageService() {
        this.messageService.add({ severity: 'error', summary: 'error', detail: 'Some thing went wrong, please try again later' });
    }
    OpenConfirmationDialog() {   // method to open dialog
        this._dialogservice.confirm({
            message: 'Are you sure, you want to delete this?',
            accept: () => {
                this.delete(this.id, this.selectedUploadDetails.Id);
            },
            reject: () => {
                this.onCancel();

            }
        });
    }


    delete(id: number, selectedUploadDetails: number) {
        this._service.Delete(id, selectedUploadDetails).subscribe((data1: any) => {
            this.GetFileUploadData();
        }, (error) => {
            this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
        });
        //this.uploadDialog.nativeElement.close();
    }


    onDelete(selectedData: any) {
        this.OpenConfirmationDialog();
        this.selectedUploadDetails = selectedData;
    }


    onCancel() {
        //this.uploadDialog.nativeElement.close();
    }
    getPAStatus() {
        this._service.GetPAstatus(this.id).subscribe(data => {
            this.PAstatus = data;
        }, (error) => {
            this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
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

        this._service.SubmitForApproval(this.id).subscribe((res) => {
            this.getPAStatus();
            this.messageService.add({ severity: 'success', summary: 'success message', detail: 'Employee profile submitted successfully' });
        }, (error) => {
            this.messageService.add({ severity: 'error', summary: 'Failure message', detail: 'Something went wrong.\nPlease try after some time' });
            // swal("", "Something went wrong.\nPlease try after some time", "error");
        });

    }


}


