import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import * as servicePath from '../../../../core/service-paths';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from "@angular/forms";
import { KrascalemasterService } from "src/app/modules/kra/services/krascalemaster.service";
import { KRAScaleMaster, KRAScaleDetails } from 'src/app/modules/kra/models/krascaleData.model';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
// import { ConfirmationService } from 'primeng/components/common/confirmationservice';
//import { MessageService } from 'primeng/api';
//import { MatDialog, MatDialogConfig } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig } from '@angular/material/dialog';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';

import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';


@Component({
  selector: 'app-kra-scale-master',
  templateUrl: './kra-scale-master.component.html',
  styleUrls: ['./kra-scale-master.component.scss'],
  providers: [KrascalemasterService]
})
export class KraScaleMasterComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = ['ScaleTitle', 'MinimumScale', 'MaximumScale', 'View', 'Edit', 'Delete'];
  //dataSource:any;
  dataSource = new MatTableDataSource<KRAScaleMaster>();
  kraScaleForm: FormGroup;
  ScaleDetails: FormArray;
  @ViewChild('viewScale') scaleDialog: TemplateRef<any>;
  @ViewChild('addScale') addScale: TemplateRef<any>;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  private componentName: string;
  public PageSize: number;
  public PageDropDown: number[];


  public kraMasterScaleList: KRAScaleMaster[] = [];
  public kraMasterScale: KRAScaleMaster;

  private tempKRAScaleDetails: KRAScaleDetails[] = [];
  private scaleDesciptionList: GenericType[];
  public ScaleTitle: string = "Add KRA Scale";
  public saveButton: string = "Save";
  public kraScaleDisplay: boolean = false;
  public kraDescriptionView: boolean = false;
  public formSubmitted: boolean = false;
  public myForm: FormGroup;
  public descriptionView: string = "";
  public descriptionHide: boolean = false;
  private MinimumScaleValidation: boolean = true;
  private MaximumScaleValidation: boolean = true;
  public spaceValidation: boolean = false;
  public dialogResponse:boolean;
  public maximumScale:number;
  dialogText: string;


  constructor(
    private _activatedRoute: ActivatedRoute,
    private _kraScaleMasterService: KrascalemasterService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private _fb: FormBuilder,
    //private _confirmationService: ConfirmationService,
    private _masterDataService: MasterDataService,
    // private messageService: MessageService,
    private dialog: MatDialog) {
    this.componentName = this._activatedRoute.routeConfig.component.name;
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.getKRAScaleList();
    //this.getKRAScaleDescriptions();

    this.myForm = this._fb.group({
      ScaleTitle: ['', [Validators.required, Validators.pattern("^[a-zA-Z0-9-_ ]*$")]],
      MinimumScale: [null, [Validators.required]],
      MaximumScale: [null, [Validators.required]],
      ScaleDetails: this._fb.array([this.initDescription()])
    });
    this.resetForm();
  }
  cols = [
    { field: 'ScaleTitle', header: 'Scale Title' },
    { field: 'MinimumScale', header: 'Minimum Scale', type: "number" },
    { field: 'MaximumScale', header: 'Maximum Scale', type: "number" },

  ];
  columnsToDisplay: string[] = this.displayedColumns.slice();

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(
      this.displayedColumns,
      event.previousIndex,
      event.currentIndex
    );
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.dataSource.data);
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  initDescription() {
    return this._fb.group({
      ScaleDetailId: [null],
      ScaleValue: [null],
      ScaleDescription: [
        "",
        [Validators.required, Validators.pattern("^[a-zA-Z0-9_ ]*$")]
      ]
    });
  }

  // private getKRAScaleDescriptions(): void {
  //   this._masterDataService.getKRAScaleValues()
  //     .subscribe((desciptionResponse: GenericType[]) => {
  //       this.scaleDesciptionList = [];
  //       this.scaleDesciptionList = desciptionResponse;
  //     });

  // }
  public getStyles(type: string) {
    if (type == 'number') {
      return { 'text-align': 'right', 'width': '120px' };
    }
  }
  getKRAScaleList(): void {
    this._kraScaleMasterService.GetKRAScale().subscribe(
      (scaleResponse: KRAScaleMaster[]) => {
        this.kraMasterScaleList = [];
        this.kraMasterScaleList = scaleResponse;
        //this.dataSource = scaleResponse;
        this.dataSource = new MatTableDataSource(this.kraMasterScaleList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      (error: any) => {
        if (error._body != undefined && error._body != "") {}
        this._snackBar.open('Failed to get KRA Scale List', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackbar // this.messageService.add({
        //   severity: 'error',
        //   summary: 'Error Message',
        //   detail: 'Failed to get KRA Scale List'
        // });
      }
    );
  }

  public onAddKRAScale(): void {
    this.descriptionHide = false;
    this.saveButton = "Save";
    this.ScaleTitle = "Add KRA Scale";
    this.resetForm();
    this.kraScaleDisplay = true;
  }

  onMinimunScaleChange(
    MinimumScale: number,
    MaximumScale: number
  ): void {
    if (MinimumScale != null && MinimumScale != 1) {
      this._snackBar.open('Minimum Scale should start from 1', '', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      //snackbar // this.messageService.add({
      //   severity: 'warn',
      //   summary: 'Warning Message',
      //   detail: 'Minimum Scale should start from 1'
      // });
      this.MinimumScaleValidation = false;
      return;
    } else {
      this.MinimumScaleValidation = true;
    }
    if (MinimumScale != null && MaximumScale != null) {
      this.onMaximumScaleChange(this.myForm.value);
    }
  }

  onMaximumScaleChange(data: any): void {
    this.formSubmitted = false;
    let MinimumScale = data.MinimumScale, MaximumScale = data.MaximumScale
    if(MaximumScale==null){
      this._snackBar.open('Maximum Scale should be greater than or equals to Minimum Scale', '', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return;
    }
    if (MinimumScale != null && MaximumScale != null) {
      if (!this.MinimumScaleValidation) return;
      if (MinimumScale > MaximumScale) {
        this._snackBar.open('Maximum Scale should be greater than or equals to Minimum Scale', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackbar // this.messageService.add({
        //   severity: 'warn',
        //   summary: 'Warning Message',
        //   detail: 'Maximum Scale should be greater than or equals to Minimum Scale'
        // });

        this.MaximumScaleValidation = false;
        return;
      } else if (MaximumScale > 30) {
        this._snackBar.open('Maximum Scale should not be greater than 30', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        //snackbar // this.messageService.add({
        //   severity: 'warn',
        //   summary: 'Warning Message',
        //   detail: 'Maximum Scale should not be greater than 30'
        // });
        this.MaximumScaleValidation = false;
        return;
      } else if(this.maximumScale != MaximumScale){
        this.maximumScale = MaximumScale
        this.MaximumScaleValidation = true;
        this.myForm.controls["ScaleDetails"] = new FormArray([]);
        const control = <FormArray>this.myForm.controls["ScaleDetails"];
        (control: FormArray) => {
          while (control.length !== 0) {
            control.controls = [];
            this.descriptionHide = false;
          }
        };
        for (let i = MinimumScale; i <= MaximumScale; i++) {
          control.push(this.initDescription());
        }
        this.descriptionHide = true;
      }
    }
  }

  public saveKRAScale(kraMasterScale: KRAScaleMaster): void {
    this.formSubmitted = true;
    kraMasterScale.ScaleTitle = this.myForm.controls.ScaleTitle.value;
    kraMasterScale.MaximumScale = this.myForm.controls.MaximumScale.value;
    kraMasterScale.ScaleID = this.kraMasterScale.ScaleID;
    if (kraMasterScale.ScaleTitle) {
      kraMasterScale.ScaleTitle = kraMasterScale.ScaleTitle.trim().replace(/  +/g, ' ');
      if (kraMasterScale.ScaleTitle == '') {
        this.spaceValidation = true;
        // when we are displaying error mesage no need for toaster --( removed by hema suggested by sravanthi.)
        return;
      } else {
        this.spaceValidation = false;
      }
    } else {
      this.spaceValidation = false;
    }
    if (
      kraMasterScale.ScaleTitle == "" ||
      kraMasterScale.MinimumScale == null ||
      kraMasterScale.MaximumScale == null ||
      this.myForm.controls["ScaleTitle"].value == null
    )
      return;
    if (!this.MinimumScaleValidation) {
      this.onMinimunScaleChange(
        kraMasterScale.MinimumScale,
        kraMasterScale.MaximumScale
      );
      return;
    }
    if (!this.MaximumScaleValidation) {
      this.onMaximumScaleChange(this.myForm.value);
      return;
    }
    let scaleDescription: any[] = [];
    let numberRegex = /^[a-zA-Z0-9_\s]*$/;
    var desc = '';;
    scaleDescription = this.myForm.controls["ScaleDetails"].value;
    if (scaleDescription.length == 0) {
      return;
    }
    else {
      let description: any[] = [];

      for (let i = 0; i < scaleDescription.length; i++) {

        if (scaleDescription[i].ScaleDescription)
          scaleDescription[i].ScaleDescription = scaleDescription[i].ScaleDescription.trim().replace(/  +/g, ' ');
        let descriptionValidation = numberRegex.test(
          scaleDescription[i].ScaleDescription
        );

        description[i] = (scaleDescription[i].ScaleDescription == "" || !descriptionValidation || scaleDescription[i].ScaleDescription == null)

        if (description[i] == true) {
          desc = 'false';
        }
        else desc = 'true';

      }

      if (desc == 'true') {
        kraMasterScale.ScaleDetails = scaleDescription.map(
          (scaleDescription, index) => {
            if (this.tempKRAScaleDetails.length > 0) {
              return {
                ScaleDescription: scaleDescription.ScaleDescription,
                ScaleValue: this.tempKRAScaleDetails[index].ScaleValue,
                ScaleDetailId: this.tempKRAScaleDetails[index]
                  .ScaleDetailId
              };
            } else {
              return {
                ScaleDescription: scaleDescription.ScaleDescription,
                ScaleValue: index + 1
              };
            }
          }
        );
      } else {
        this._snackBar.open('Enter Valid input', '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        // this.messageService.add({
        //   severity: 'warn',
        //   summary: 'Warning Message',
        //   detail: 'Enter Valid input'
        // });
        return;
      }
    }
    if (this.saveButton == "Save") {
      // for (
      //   let i = kraMasterScale.MinimumScale;
      //   i <= kraMasterScale.MaximumScale;
      //   i++
      // )
      //   if (kraMasterScale.ScaleDetails.length > 0)
      //     kraMasterScale.ScaleDetails[i - 1].KRAScale = i;
      this._kraScaleMasterService.CreateKRAScale(kraMasterScale).subscribe(
        (response: number) => {
          if (response == 1) {
            this.descriptionHide = false;
            this.dialog.closeAll();
            this._snackBar.open('KRA Scale saved Successfully', '', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //snackbar // this.messageService.add({
            //   severity: 'success',
            //   summary: 'Success Message',
            //   detail: 'KRA Scale saved Successfully'
            // });
            this.getKRAScaleList();
            //this.getKRAScaleDescriptions();
          } else if (response == -1) {
            this._snackBar.open('Either Scale Title or Descriptions may be duplicate', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //snackbar // this.messageService.add({
            //   severity: 'warn',
            //   summary: 'Warning Message',
            //   detail: 'Either Scale Title or Descriptions may be duplicate'
            // });
          } else if (response == -13) {
            this._snackBar.open('Enter valid input', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //snackbar // this.messageService.add({
            //   severity: 'warn',
            //   summary: 'Warning Message',
            //   detail: 'Enter valid input'
            // });
          } else {
            this._snackBar.open('Failed to Save KRA Scale', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //snackbar // this.messageService.add({
            //   severity: 'error',
            //   summary: 'Error Message',
            //   detail: 'Failed to Save KRA Scale'
            // });
          }
        },
        (error: any) => {
          if (error._body != undefined && error._body != "") { }
          this._snackBar.open('Failed to Save KRA Scale', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          //snackbar // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error Message',
          //   detail: 'Failed to Save KRA Scale'
          // });
        }
      );
    } else {
      this._kraScaleMasterService.UpdateKRAScale(kraMasterScale)
        .subscribe(
          (response: number) => {
            if (response == 1) {
              this.kraScaleDisplay = false;
              this.dialog.closeAll();
              this._snackBar.open('KRA Scale updated Successfully', '', {
                duration: 1000,
                panelClass: ['success-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              //snackbar // this.messageService.add({
              //   severity: 'success',
              //   summary: 'Success Message',
              //   detail: 'KRA Scale updated Successfully'
              // });
              this.getKRAScaleList();
              //this.getKRAScaleDescriptions();
            } else if (response == -1) {
              this._snackBar.open('Either Scale Title or Descriptions may be duplicate', '', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              //snackbar // this.messageService.add({
              //   severity: 'warn',
              //   summary: 'Warning Message',
              //   detail: 'Either Scale Title or Descriptions may be duplicate'
              // });
            }
            else if (response == -13) {
              this._snackBar.open('Enter valid input', '', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              //snackbar  // this.messageService.add({
              //   severity: 'warn',
              //   summary: 'Warning Message',
              //   detail: 'Enter valid input'
              // });
            } else {
              this._snackBar.open('Failed to Update KRA Scale', '', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              //snackbar // this.messageService.add({
              //   severity: 'error',
              //   summary: 'Error Message',
              //   detail: 'Failed to Update KRA Scale'
              // });
            }
          },
          (error: any) => {
            if (error._body != undefined && error._body != "") { }
            this._snackBar.open('Failed to Update KRA Scale', '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            //snackbar // this.messageService.add({
            //   severity: 'error',
            //   summary: 'Error Message',
            //   detail: 'Failed to Update KRA Scale'
            // });
          }
        );
    }
  }

  onEditKRAScaleDescription(kraScaleMaster: KRAScaleMaster): void {
    this.myForm.patchValue({ ScaleTitle: kraScaleMaster.ScaleTitle, MinimumScale: kraScaleMaster.MinimumScale, MaximumScale: kraScaleMaster.MaximumScale });
    this.onMaximumScaleChange(kraScaleMaster);
    this.openAddKRAScale('edit');
    //this.descriptionHide = true;
    //this.formSubmitted = false;
    this.saveButton = "Update";
    this.ScaleTitle = "Update KRA Scale Descriptions";
    this._kraScaleMasterService
      .GetKRADescriptionDetails(kraScaleMaster.ScaleID)
      .subscribe(
        (DescriptionResponse: KRAScaleDetails[]) => {
          this.kraMasterScale = kraScaleMaster;
          this.kraMasterScale.ScaleDetails = DescriptionResponse;
          this.tempKRAScaleDetails = [];
          this.tempKRAScaleDetails = DescriptionResponse;
          DescriptionResponse.forEach(
            (response: KRAScaleDetails, index: number) => {
              const fbScaleDescriptions = this.myForm.get(
                "ScaleDetails"
              ) as FormArray;
              fbScaleDescriptions.at(index).setValue({
                ScaleDetailId: response.ScaleDetailId,
                ScaleValue: response.ScaleValue,
                ScaleDescription: response.ScaleDescription
              });
            }
          );
        },
        (error: any) => {
          if (error._body != undefined && error._body != "") { }
          this._snackBar.open('Failed to get KRA Scale Description', '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error Message',
          //   detail: 'Failed to get KRA Scale Description'
          // });
        }
      );
  }

  onDeleteKRAScale(kraScaleMasterID: number): void {
    // method to open delete dialog
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      data: { heading: 'Confirmation', message: "Are you sure, you want to delete?" }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse == true){
        this._kraScaleMasterService.DeleteKRAScale(kraScaleMasterID).subscribe(
            (scaleResponse: any) => {
              if (scaleResponse.Item1 == true) {
                this._snackBar.open('KRA Scale deleted Successfully', '', {
                  duration: 1000,
                  panelClass: ['success-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.getKRAScaleList();
              }
              else {
                this._snackBar.open(scaleResponse.Item2, 'x', {
                  duration: 1000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }
            },
            (error: any) => {
              if (error._body != undefined && error._body != "")
              this._snackBar.open('Failed to delete KRA Scale', '' ,{
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            });
          }
          else {
          }
        });
      }  

onViewKRAScaleDescription(kraScaleMaster: KRAScaleMaster): void {
  this.kraDescriptionView = true;
  let descriptionList: GenericType[] = [];
  this.ScaleTitle = "View KRA Scale Descriptions";
  if(this.scaleDesciptionList.length > 0 && kraScaleMaster.ScaleID) {

  for (let i = 0; i < this.scaleDesciptionList.length; i++) {
    let scaleDesciption = this.scaleDesciptionList[i];

    if (scaleDesciption.Id == kraScaleMaster.ScaleID) {
      descriptionList.push(scaleDesciption);
    }
  }
  if (descriptionList.length > 0) {
    this.descriptionView = descriptionList[0].Name;
  } else {
    this.descriptionView = "No Records";
  }

}
  }

  public resetForm(): void {
    this.maximumScale=0
  this.spaceValidation = false;
  this.formSubmitted = false;
  if(this.saveButton == "Save") {
  this.myForm.patchValue({ ScaleTitle: '', MaximumScale: null });
  this.descriptionHide = false;
  this.kraMasterScale = new KRAScaleMaster();
  this.kraMasterScale = {
    ScaleID: 0,
    MinimumScale: 1,
    MaximumScale: null,
    ScaleTitle: "",
    ScaleDetails: new Array<KRAScaleDetails>()
  };
  this.MaximumScaleValidation = true;
  this.MinimumScaleValidation = true;
} else {
  this.kraMasterScale.ScaleDetails = [];
  this.myForm.controls["ScaleDetails"].reset();
}
  }

  public cancelDialog(): void {
  this.kraScaleDisplay = false;
}

openAddKRAScale(val: string) {
  if (val == 'add') {
    this.maximumScale=0;
    this.descriptionHide = false;
    this.saveButton = "Save";
    this.myForm.reset();
    this.myForm.controls["ScaleDetails"].reset();
    this.myForm.patchValue({ MinimumScale: 1 });
    this.myForm.controls.ScaleTitle.enable();
    this.myForm.controls.MaximumScale.enable();
  }
  else {
    this.descriptionHide = true;
    this.myForm.controls.ScaleTitle.disable();
    this.myForm.controls.MaximumScale.disable();
  }
  const dialogConfig = new MatDialogConfig();
  dialogConfig.restoreFocus = false;
  dialogConfig.disableClose = true;
  dialogConfig.autoFocus = false;
  dialogConfig.role = 'dialog';
  this.dialog.open(this.addScale, dialogConfig);
}

viewDialog(kraScaleMasterID: number) {
  this._kraScaleMasterService.GetKRADescriptionDetails(kraScaleMasterID).subscribe(data => {
    let d = data.map((v, i) => v.ScaleValue + '-' + v.ScaleDescription);
    this.dialogText = d.join(', ');
  }, err => {
  });
  const dialogConfig = new MatDialogConfig();
  dialogConfig.restoreFocus = false;
  dialogConfig.disableClose = true;
  dialogConfig.autoFocus = false;
  dialogConfig.role = 'dialog';
  this.dialog.open(this.scaleDialog, dialogConfig);
}
}
