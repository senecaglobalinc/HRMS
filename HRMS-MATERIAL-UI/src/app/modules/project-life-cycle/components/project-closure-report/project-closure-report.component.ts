import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, FormGroupDirective } from '@angular/forms';
import { editorConfig } from "../../../../core/angularEditorConfiguratioan";
import { MatStepper } from '@angular/material/stepper';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { ProjectClosureReport } from '../../models/projects.model';
import { ProjectCreationService } from '../../services/project-creation.service';
import { ProjectClosureService } from '../../services/project-closure.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { themeconfig } from 'src/themeconfig';
import { Subscription } from 'rxjs';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { environment } from '../../../../../environments/environment';
import * as servicePath from '../../../../core/service-paths';
import * as fileSaver from 'file-saver';
import 'rxjs/Rx';
import { MatTable } from '@angular/material/table';
import { UrlService } from 'src/app/modules/shared/services/url.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-project-closure-report',
  templateUrl: './project-closure-report.component.html',
  styleUrls: ['./project-closure-report.component.scss']
})
export class ProjectClosureReportComponent implements OnInit {

  projectId: number;
  projectIdSubscription: Subscription;
  type: string;
  FileType: string;
  fileclientdata: any;
  filedeliverydata: any;
  editorConfig = editorConfig;
  selectedFileclient: String = "";
  documentDescclient: any = { file: "" };
  selectedFiledelivery: String = "";
  documentDescdelivery: any = { file: "" };
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  fourthFormGroup: FormGroup;
  fifthFormGroup: FormGroup;
  sixthFormGroup: FormGroup;
  seventhFormGroup: FormGroup;
  //  eighthFormGroup: FormGroup;
  //  ninethFormGroup: FormGroup;
  formSave = false;
  activeIndex: number = 0;
  clientuploadeddata: any[] = [];
  deliveryuploadeddata: any[] = [];
  projectclosurereportdata: ProjectClosureReport[];
  clientInfo: any[] = [];
  deliveryInfo: any[] = [];
  projectclosuredataclient: ProjectClosureReport;
  projectclosuredatadelivery: ProjectClosureReport;
  clientdataSource = new MatTableDataSource<ProjectClosureReport>();
  deliverydataSource = new MatTableDataSource<ProjectClosureReport>();
  firstFormGroupvalid: boolean = false;
  secondFormGroupvalid: boolean = false;
  formSaveone: boolean = false;
  formSavetwo: boolean = false;
  private selectedUploadDetails: any;
  pageload: boolean = false;
  ifremarks: boolean = false;
  Remarkspm: FormGroup;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('fileUploadclient') fileUploadclient: any;
  @ViewChild('fileUploaddelivery') fileUploaddelivery: any;
  @ViewChild('deliverytable') table: MatTable<any>;
  @ViewChild('clienttable') tablee: MatTable<any>;
  @ViewChild('fileInputclient') fileInputVarclient: any;
  @ViewChild('fileInputdelivery') fileInputVardelivery: any;
  displayedColumns: string[] = [
    'Sno',
    'DocumentName',
    'Download',
    'Delete'
  ];




  previousUrl: string;
  showback: boolean = true;
  constructor(private _router: Router, private cdr: ChangeDetectorRef,
    private serviceObj: ProjectCreationService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private projectClosureService: ProjectClosureService,
    private urlService: UrlService, private spinner: NgxSpinnerService,
    private dialog?: MatDialog

  ) {

  }
  themeConfigInput = themeconfig.formfieldappearances;


  documentUploadChangeClient(fileURL: string) {
    this.selectedFileclient = fileURL.substr(12, fileURL.length);
  }
  documentUploadChangeDelivery(fileURL: string) {
    this.selectedFiledelivery = fileURL.substr(12, fileURL.length);
  }

  toastMessageClient() {
    this._snackBar.open('Please select a PDF/Word file', 'x', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  toastMessageDelivery() {
    this._snackBar.open('Please select a Excel file', 'x', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  ngOnInit(): void {
    this.spinner.show();
    this.urlService.previousUrl$.subscribe((previousUrl: string) => {
      this.previousUrl = previousUrl;
    });
    this.projectIdSubscription = this.serviceObj.GetProjectId().subscribe(data => {
      this.projectId = data;
    });

    this.GetProjectClosureReport(this.projectId);

    this.firstFormGroup = new FormGroup({

      SelectedFileShowclient: new FormControl(null),
      uploadselectclient: new FormControl(null, [Validators.required])


    });
    this.secondFormGroup = new FormGroup({

      SelectedFileShowdelivery: new FormControl(null),
      uploadselectdelivery: new FormControl(null, [Validators.required])
    });
    this.thirdFormGroup = new FormGroup({
      ValueDelivered: new FormControl(null, [Validators.required]),
    });

    this.fourthFormGroup = new FormGroup({
      ManagementChallenges: new FormControl(null, [Validators.required]),
      TechnologyChallenges: new FormControl(null, [Validators.required]),
      EngineeringChallenges: new FormControl(null, [Validators.required]),


    });
    this.fifthFormGroup = new FormGroup({
      BestPractices: new FormControl(null, [Validators.required]),
      LessonsLearned: new FormControl(null, [Validators.required]),
      ReusableArtifacts: new FormControl(null, [Validators.required]),
      ProcessImprovements: new FormControl(null, [Validators.required]),

    });
    this.sixthFormGroup = new FormGroup({
      Awards: new FormControl(null, [Validators.required]),
      NewTechnicalSkills: new FormControl(null, [Validators.required]),
      NewTools: new FormControl(null, [Validators.required]),

    });

    this.seventhFormGroup = new FormGroup({
      Remarks: new FormControl(null, [Validators.required]),
    });

    this.Remarkspm = new FormGroup({
      Remarksfrompm: new FormControl(null)
    });

    this.GetClientFeedbackUpload();
    this.GetDeliveryUpload();

  }
  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  saveClientFeedback() {
    this.fileclientdata = this.fileUploadclient.nativeElement;
    this.GetClientFeedbackUpload();
    let _resources = servicePath.API.projects;
    let _serverURL = environment.ProjMicroService;
    let formData: FormData = new FormData(),

      xhr: XMLHttpRequest = new XMLHttpRequest();
    formData.append('UploadedFiles', this.fileclientdata.files[0]);
    formData.append('ClientFeedbackFile', this.fileclientdata.files[0].name);

    for (var count = 0; count < this.clientInfo.length; count++) {
      let str: string = this.fileclientdata.files[count].name;
      let l = this.fileclientdata.files[count].name.length;
      //console.log(str.substring(l-3,l+1));
      let extension = str.substring(l - 3, l + 1);
      if (!(extension == 'pdf' || extension == 'txt' || extension == 'rtf' || extension == 'docx' || extension == 'doc')) {
        this.toastMessageClient();
        return;
      }

      if (this.clientInfo[count] == this.fileclientdata.files[0].name) {
        this._snackBar.open(this.clientInfo[count] + ' already exists !', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
    }

    formData.append('ProjectId', this.projectId.toString());
    formData.append('FileType', 'ClientFeedback');
    xhr.open('POST', _serverURL + _resources.savefiles, false);
    if (sessionStorage["token"]) {
      xhr.setRequestHeader("Authorization", "bearer " + sessionStorage["token"]);
    }
    xhr.send(formData);

    this.GetClientFeedbackUpload();
    this.fileInputVarclient.nativeElement.value = "";

  }
  saveDeliveryPerformance() {
    this.GetDeliveryUpload();
    this.filedeliverydata = this.fileUploaddelivery.nativeElement;
    let _resources = servicePath.API.projects;
    let _serverURL = environment.ProjMicroService;
    let formData: FormData = new FormData(),
      xhr: XMLHttpRequest = new XMLHttpRequest();
    formData.append('UploadedFiles', this.filedeliverydata.files[0]);
    formData.append('DeliveryPerformanceFile', this.filedeliverydata.files[0].name);
    let str: string = this.filedeliverydata.files[0].name;
    let l = this.filedeliverydata.files[0].name.length;
    //console.log(str.substring(l-4,l+1));
    let extension = str.substring(l - 4, l + 1);
    if (!(extension == 'xlsx' || extension == 'xls')) {
      this.toastMessageDelivery();
      return;
    }
    formData.append('ProjectId', this.projectId.toString());
    formData.append('FileType', 'DeliveryPerformance');
    xhr.open('POST', _serverURL + _resources.savefiles, false);
    if (sessionStorage["token"]) {
      xhr.setRequestHeader("Authorization", "bearer " + sessionStorage["token"]);
    }
    xhr.send(formData);

    this.GetDeliveryUpload();
    this.fileInputVardelivery.nativeElement.value = "";

  }
  GetClientFeedbackUpload() {
    this.projectClosureService.GetClosureReportByProjectID(this.projectId).subscribe((res: ProjectClosureReport) => {
      this.clientInfo[0] = res[0].ClientFeedbackFile;
      this.projectclosuredataclient = res[0];
      if (res[0].ClientFeedbackFile == '') {
        this.clientInfo = [];
      }
      this.clientdataSource = new MatTableDataSource(this.clientInfo);
      if (this.clientdataSource.data.length != 0) {
        this.firstFormGroupvalid = true;
        this.formSaveone = false;
      }
    });
    this.cdr.detectChanges();
  }
  GetDeliveryUpload() {
    this.projectClosureService.GetClosureReportByProjectID(this.projectId).subscribe((res: ProjectClosureReport) => {
      this.deliveryInfo[0] = res[0].DeliveryPerformanceFile;
      this.projectclosuredatadelivery = res[0];
      if (res[0].DeliveryPerformanceFile == '') {
        this.deliveryInfo = [];
      }

      this.deliverydataSource = new MatTableDataSource(this.deliveryInfo);
      if (this.deliverydataSource.data.length != 0) {
        this.secondFormGroupvalid = true;
        this.formSavetwo = false;
      }
    });
    this.cdr.detectChanges();
  }
  downloadclient() {

    this.FileType = "ClientFeedback";

    this.projectClosureService.DownloadProjectClosureFileUpload(this.FileType, this.projectId).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/pdf' });

      if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob);
        return;
      }
      const url = window.URL.createObjectURL(blob);
      fileSaver.saveAs(blob, this.projectclosuredataclient.ClientFeedbackFile);
    }), error => console.log('Error downloading the file'),
      () => console.info('File downloaded successfully');
  }
  downloaddelivery() {
    this.FileType = "DeliveryPerformance";

    this.projectClosureService.DownloadProjectClosureFileUpload(this.FileType, this.projectId).subscribe(response => {
      let blob: any = new Blob([response], { type: 'application/vnd.ms.excel' });
      if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob);
        return;
      }
      const url = window.URL.createObjectURL(blob);
      fileSaver.saveAs(blob, this.projectclosuredatadelivery.DeliveryPerformanceFile);
    }), error => console.log('Error downloading the file'),
      () => console.info('File downloaded successfully');
  }
  deleteclient(id: number, type: string) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: { message: 'Are you sure you want to delete?' },

    });

    dialogRef.afterClosed().subscribe(result => {

      if (result == true) {
        this.projectClosureService.Delete(id, type).toPromise().then((data1: any) => {

          this.GetClientFeedbackUpload();
          this.tablee.renderRows();
          this._snackBar.open('File Deleted', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.firstFormGroupvalid = false;
        }
        ).catch((error) => {
          this._snackBar.open('Some thing went wrong, please try again later', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          })
        });

      }

    });

  }
  onDeleteclient() {

    this.FileType = "ClientFeedback";
    this.deleteclient(this.projectId, this.FileType);
  }

  deletedelivery(id: number, type: string) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: { message: 'Are you sure you want to delete?' },

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.projectClosureService.Delete(id, type).toPromise().then((data1: any) => {

          this.GetDeliveryUpload();
          this.table.renderRows();
          this._snackBar.open('File Deleted', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.secondFormGroupvalid = false;
        }
        ).catch((error) => {
          this._snackBar.open('Some thing went wrong, please try again later', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          })
        });
      }
    });
  }

  onDeletedelivery() {
    this.FileType = "DeliveryPerformance";
    this.deletedelivery(this.projectId, this.FileType);
  }

  onSave() {

    this.type = 'update';

    var createObj = new ProjectClosureReport();
    createObj.ProjectId = this.projectId;
    createObj.type = this.type;
    createObj.ValueDelivered = this.thirdFormGroup.value.ValueDelivered;
    createObj.ManagementChallenges = this.fourthFormGroup.value.ManagementChallenges;
    createObj.TechnologyChallenges = this.fourthFormGroup.value.TechnologyChallenges;
    createObj.EngineeringChallenges = this.fourthFormGroup.value.EngineeringChallenges;
    createObj.BestPractices = this.fifthFormGroup.value.BestPractices;
    createObj.LessonsLearned = this.fifthFormGroup.value.LessonsLearned;
    createObj.ReusableArtifacts = this.fifthFormGroup.value.ReusableArtifacts;
    createObj.ProcessImprovements = this.fifthFormGroup.value.ProcessImprovements;
    createObj.Awards = this.sixthFormGroup.value.Awards;
    createObj.NewTechnicalSkills = this.sixthFormGroup.value.NewTechnicalSkills;
    createObj.NewTools = this.sixthFormGroup.value.NewTools;
    createObj.Remarks = this.seventhFormGroup.value.Remarks;

    this.projectClosureService.UpdateProjectClosureReport(createObj).subscribe(
      (res: any) => {

        if (res) {

          this._snackBar.open('Data Saved Successfully', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.GetProjectClosureReport(this.projectId);
        }
      },
      error => {
        this._snackBar.open('Failed to Save Data', 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  GetProjectClosureReport(projectId: number) {

    this.projectClosureService.GetClosureReportByProjectID(this.projectId).toPromise().then((res: ProjectClosureReport) => {
      if (res[0].RejectRemarks == '' || res[0].RejectRemarks == undefined || res[0].RejectRemarks == null) {
        this.thirdFormGroup.controls.ValueDelivered.patchValue(res[0].ValueDelivered);
        this.fourthFormGroup.controls.ManagementChallenges.patchValue(res[0].ManagementChallenges);
        this.fourthFormGroup.controls.TechnologyChallenges.patchValue(res[0].TechnologyChallenges);
        this.fourthFormGroup.controls.EngineeringChallenges.patchValue(res[0].EngineeringChallenges);
        this.fifthFormGroup.controls.BestPractices.patchValue(res[0].BestPractices);
        this.fifthFormGroup.controls.LessonsLearned.patchValue(res[0].LessonsLearned);

        this.fifthFormGroup.controls.ReusableArtifacts.patchValue(res[0].ReusableArtifacts);

        this.fifthFormGroup.controls.ProcessImprovements.patchValue(res[0].ProcessImprovements);
        this.sixthFormGroup.controls.Awards.patchValue(res[0].Awards);
        this.sixthFormGroup.controls.NewTechnicalSkills.patchValue(res[0].NewTechnicalSkills);
        this.sixthFormGroup.controls.NewTools.patchValue(res[0].NewTools);
        this.seventhFormGroup.controls.Remarks.patchValue(res[0].Remarks);
        this.pageload = true;
        this.spinner.hide();
        this.ifremarks = false;
      }
      else {
        this.thirdFormGroup.controls.ValueDelivered.patchValue(res[0].ValueDelivered);
        this.fourthFormGroup.controls.ManagementChallenges.patchValue(res[0].ManagementChallenges);
        this.fourthFormGroup.controls.TechnologyChallenges.patchValue(res[0].TechnologyChallenges);
        this.fourthFormGroup.controls.EngineeringChallenges.patchValue(res[0].EngineeringChallenges);
        this.fifthFormGroup.controls.BestPractices.patchValue(res[0].BestPractices);
        this.fifthFormGroup.controls.LessonsLearned.patchValue(res[0].LessonsLearned);

        this.fifthFormGroup.controls.ReusableArtifacts.patchValue(res[0].ReusableArtifacts);

        this.fifthFormGroup.controls.ProcessImprovements.patchValue(res[0].ProcessImprovements);
        this.sixthFormGroup.controls.Awards.patchValue(res[0].Awards);
        this.sixthFormGroup.controls.NewTechnicalSkills.patchValue(res[0].NewTechnicalSkills);
        this.sixthFormGroup.controls.NewTools.patchValue(res[0].NewTools);
        this.seventhFormGroup.controls.Remarks.patchValue(res[0].Remarks);
        this.Remarkspm.controls.Remarksfrompm.patchValue(res[0].RejectRemarks);
        this.pageload = true;
        this.spinner.hide();
        this.ifremarks = true;

      }
    }).catch(error => {
      this.spinner.hide();
      this.pageload = true;
      // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
      //   duration: 2000,
      //   horizontalPosition: 'right',
      //   verticalPosition: 'top',
      // });
    });


  }

  stepChanged(event) {

    event.previouslySelectedStep.interacted = false;
    this.activeIndex = event.selectedIndex;
    //this.formSave=false;
    //event.selectedStep.interacted=false;
  }
  onSelect(event) {

    this.activeIndex = event.selectedIndex;
    // this.formSave=false;
  }
  onSubmit() {
    this.formSave = true;
    this.type = 'submit';
    if (this.firstFormGroupvalid == false) {
      this.formSaveone = true;
    }
    if (this.secondFormGroupvalid == false) {
      this.formSavetwo = true;
    }

    var createObj = new ProjectClosureReport();
    createObj.ProjectId = this.projectId;
    createObj.type = this.type;
    createObj.ValueDelivered = this.thirdFormGroup.value.ValueDelivered;
    createObj.ManagementChallenges = this.fourthFormGroup.value.ManagementChallenges;
    createObj.TechnologyChallenges = this.fourthFormGroup.value.TechnologyChallenges;
    createObj.EngineeringChallenges = this.fourthFormGroup.value.EngineeringChallenges;
    createObj.BestPractices = this.fifthFormGroup.value.BestPractices;
    createObj.LessonsLearned = this.fifthFormGroup.value.LessonsLearned;
    createObj.ReusableArtifacts = this.fifthFormGroup.value.ReusableArtifacts;
    createObj.ProcessImprovements = this.fifthFormGroup.value.ProcessImprovements;
    createObj.Awards = this.sixthFormGroup.value.Awards;
    createObj.NewTechnicalSkills = this.sixthFormGroup.value.NewTechnicalSkills;
    createObj.NewTools = this.sixthFormGroup.value.NewTools;
    createObj.Remarks = this.seventhFormGroup.value.Remarks;
    if (this.firstFormGroupvalid && this.secondFormGroupvalid && this.thirdFormGroup.valid && this.fourthFormGroup.valid && this.fifthFormGroup.valid && this.sixthFormGroup.valid && this.seventhFormGroup.valid) {
      this.projectClosureService.UpdateProjectClosureReport(createObj).subscribe(
        (res: any) => {


          if (res) {

            this._snackBar.open('Project Closure Report Submitted Successfully', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            }).afterDismissed().toPromise().then(() => {
              this.onBack();
            });
            this.Reset();
          }
          else {
            this._snackBar.open('Failed to Submit Data', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

          }

        },
        error => {
          this._snackBar.open('error.error', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
    }
    else {
      this._snackBar.open('Fill the details in all the tabs', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });

    }
  }
  onBack() {
    this._router.navigate([this.previousUrl]);
    this.showback = false;
  }
  // ngOnDestroy() {
  //   this.projectIdSubscription.unsubscribe();
  // }
  Reset() {
    this.formSave = false;
    this.firstFormGroup.reset();
    this.secondFormGroup.reset();
    this.thirdFormGroup.reset();
    this.fourthFormGroup.reset();
    this.fifthFormGroup.reset();
    this.sixthFormGroup.reset();
    this.seventhFormGroup.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
  }
}
