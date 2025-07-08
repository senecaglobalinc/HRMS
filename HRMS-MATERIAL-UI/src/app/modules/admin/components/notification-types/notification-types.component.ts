import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NotificationTypeService } from '../../services/notificationtype.service';
import { NotificationType } from '../../models/notificationconfiguration.model';
import { DropDownType } from '../../../master-layout/models/dropdowntype.model';
import { CommonService } from "../../../../core/services/common.service";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from 'src/themeconfig';
import * as servicePath from '../../../../core/service-paths';



@Component({
  selector: 'app-notification-types',
  templateUrl: './notification-types.component.html',
  styleUrls: ['./notification-types.component.scss']
})
export class NotificationTypesComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  helpMsg = "Successfully created";
  btnLabel = "";
  valid = true;
  notificationTypeList: NotificationType[] = [];
  notificationTypeId: number;
  componentName: string;
  notificationTypeData: NotificationType;
  isEdit: boolean = false;
  formSubmitted: boolean;
  display: boolean = false;
  duplicateheckNotificationType: number;
  categoriesList: DropDownType[] = [];
  addNotificationType: FormGroup;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  abc: DropDownType[] = [];


  dataSource: MatTableDataSource<NotificationType>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['CategoryName', 'NotificationCode', 'NotificationDescription', 'Edit'];


  constructor(private _commonService: CommonService,
    private serviceObj: NotificationTypeService,
    private fb: FormBuilder, private _snackBar: MatSnackBar,
    public navService: NavService,
    private masterDataService: MasterDataService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;


    this.serviceObj.notificationTypeData.subscribe((data) => {
      this.notificationTypeList = data;
      this.dataSource = new MatTableDataSource(this.notificationTypeList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  ngOnInit() {
    this.getNotificationTypes();
    this.getCategories();


    this.addNotificationType = this.fb.group({
      // Id: ['', []],
      NotificationTypeId: ['', []],
      NotificationCode: ['', [Validators.required]],
      NotificationDescription: ['', [Validators.required]],
      CategoryMasterId: ['', [Validators.required]],

    });

    this.serviceObj.editObj.subscribe(data => {
      if (this.serviceObj.editMode == true) {
        this.isEdit = this.serviceObj.editMode;
        this.addNotificationType.patchValue(data);
        this.addNotificationType.value.notificationTypeId = data.NotificationTypeId;
        // this.addNotificationType.value.Id = data.Id;
        this.btnLabel = "Update";
      }
    });
    this.btnLabel = "Save";

    this.serviceObj.GetNotificationTypes();
    this.serviceObj.notificationTypeData.subscribe((data) => {
      data.forEach(function (value) {
        if (value.CategoryMaster != null)
          value.CategoryName = value.CategoryMaster.CategoryName;
        else
          value.CategoryName = '';
      });

    })
    this.reset();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.notificationTypeList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }
  reset() {
    this.formSubmitted = false;
    this.addNotificationType.reset();
    this.serviceObj.editMode = false;
    this.btnLabel = "Save";
    this.isEdit = false;
  }
  GetNotificationList() {
    this.serviceObj.GetNotificationTypes();

  }
  editnotificationtype(editObj) {
    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(editObj);
  }

  getNotificationTypes(): void {
    this.serviceObj.GetNotificationTypes();
  }

  getCategories(): void {
    this.categoriesList = [];
    this.masterDataService.GetCategories().subscribe((res: any[]) => {
      res.forEach(ele => {
        this.categoriesList.push({ label: ele.CategoryName, value: ele.CategoryMasterId });
      });
    }),
      this.abc = this.categoriesList;
    (error: any) => {
      if (error._body != undefined && error._body != "")
        this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        });
    };
  }

  onSubmit(): void {
    this.formSubmitted = true;
    if (this.addNotificationType.value.NotificationDescription == null) {
      this.addNotificationType.value.NotificationDescription = "";
    }
    // if (this.addNotificationType.value.NotificationTypeId == null) {
    //   this.addNotificationType.value.NotificationTypeId = 0;
    // }
    console.log(this.addNotificationType.value)
    if (this.addNotificationType.valid) {
      if (this.isEdit == false) {
        this.serviceObj.AddNotificationType(this.addNotificationType.value).subscribe((response: number) => {
          if (response) {
            this._snackBar.open('Notification type record added successfully.', '', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.getNotificationTypes();
            this.display = false;
            this.reset();
            this.serviceObj.editMode = false;
          }
        },
          error => {

            this._snackBar.open(error.error, '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
          });
      }
      else {
        this.serviceObj.UpdateNotificationType(this.addNotificationType.value).subscribe((response: number) => {
          if (response) {
            this._snackBar.open('Updated notification.', '', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.getNotificationTypes();
            this.reset();
            this.display = false;
            this.serviceObj.editMode = false;
          }
        },
          error => {
            this._snackBar.open(error.error, '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
          });
      }
    }
  }
  search(query: string) {
    let result = this.select(query)
    this.abc = result;
  }

  select(query: string): DropDownType[] {
    ``
    let result: DropDownType[] = [];
    for (let category of this.categoriesList) {
      if (category.label.toLowerCase().indexOf(query) > -1) {
        result.push(category)
      }
    }
    return result
  }
}