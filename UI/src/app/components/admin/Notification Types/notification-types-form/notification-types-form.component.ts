import { Component, OnInit } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NotificationTypeService } from '../../services/notificationtype.service';
import { NotificationType } from '../../models/notificationconfiguration.model';
import { DropDownType } from '../../../../models/dropdowntype.model';
import { CommonService } from "../../../../services/common.service";
import { MasterDataService } from '../../../../services/masterdata.service';
import { MessageService } from 'primeng/api';
// import { Message } from 'primeng/components/common/api';


@Component({
  selector: 'app-notification-types-form',
  templateUrl: './notification-types-form.component.html',
  styleUrls: ['./notification-types-form.component.css'],
  providers: [MasterDataService, MessageService]
})
export class NotificationTypesFormComponent implements OnInit {
  helpMsg = "Successfully created";
  btnLabel = "";
  valid = true;
  // errorMessage: Message[] = [];
  notificationTypeList: NotificationType[] = [];
  notificationTypeId: number;
  componentName: string;
  notificationTypeData: NotificationType;
  isEdit: boolean = false;
  formSubmitted: boolean;
  display: boolean = false;
  // isEdit: boolean = false;
  // deletedisplay: boolean = false;
  duplicateheckNotificationType: number;
  categoriesList: DropDownType[] = [];
  //   categoryId:number;
  constructor(private _commonService: CommonService, private messageService: MessageService,
    private serviceObj: NotificationTypeService, private masterDataService: MasterDataService) { }
  addNotificationType: FormGroup;

  ngOnInit() {
    this.getNotificationTypes();
    this.getCategories();
    this.addNotificationType = new FormGroup({
      Id: new FormControl(null),
      NotificationTypeId: new FormControl(null),
      NotificationCode: new FormControl(null, [
        Validators.required,
      ]),
      NotificationDescription: new FormControl(null, [
      ]),
      CategoryMasterId: new FormControl(null, [
        Validators.required,
      ]),
    });

    this.serviceObj.editObj.subscribe(data => {
      if (this.serviceObj.editMode == true) {
        this.isEdit = this.serviceObj.editMode;
        this.addNotificationType.patchValue(data);
        this.addNotificationType.value.notificationTypeId = data.NotificationTypeId;
        this.addNotificationType.value.Id = data.Id;
        this.btnLabel = "Update";
      }
    });
    this.btnLabel = "Save";
    this.reset();
  }

  reset() {
    this.formSubmitted = false;
    this.addNotificationType.reset();
    this.serviceObj.editMode = false;
    this.btnLabel = "Save";
    this.isEdit = false;
  }


  getNotificationTypes(): void {
    this.serviceObj.GetNotificationTypes()
    //   .subscribe((notificationConfigurationTypeResponse: NotificationType[]) => {
    //       this.notificationTypeList = notificationConfigurationTypeResponse;   
    //   }),
    //       (error: any) => {
    //           if (error._body != undefined && error._body != "")
    //               this._commonService.LogError(this.componentName, error._body).then((data: any) => {
    //               });

    //       };
  }

  getCategories(): void {
    this.categoriesList = [];
    this.categoriesList.push({ label: "Select Category", value: null })
    this.masterDataService.GetCategories().subscribe((res: any[]) => {
      res.forEach(ele => {
        this.categoriesList.push({ label: ele.CategoryName, value: ele.CategoryMasterId });
      });
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
          });
      };
  }

  // showDialog(): void {
  //   this.form.reset();
  //   this.formSubmitted = false;
  //   this.isEdit = false;
  //   this.display = true;
  // }

  onSubmit(): void {
    this.formSubmitted = true;
    if (this.addNotificationType.value.NotificationDescription == null) {
      this.addNotificationType.value.NotificationDescription = "";
    }
    if (this.addNotificationType.value.NotificationTypeId == null) {
      this.addNotificationType.value.NotificationTypeId = 0;
    }
    if (this.addNotificationType.valid) {
      if (this.isEdit == false) {
        this.serviceObj.AddNotificationType(this.addNotificationType.value).subscribe((response: number) => {
          if (response) {
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Notification type record added successfully.' });
            this.getNotificationTypes();
            this.display = false;
            this.reset();
            this.serviceObj.editMode = false;
          }
        },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: error.error });
          });
      }
      else {
        this.serviceObj.UpdateNotificationType(this.addNotificationType.value).subscribe((response: number) => {
          if (response) {
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Notification type record updated successfully.' });
            this.getNotificationTypes();
            this.reset();
            this.display = false;
            this.serviceObj.editMode = false;
          }
        },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: error.error });
          });
      }
    }
  }
}