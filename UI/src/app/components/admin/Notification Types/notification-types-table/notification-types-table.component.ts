import { Component, OnInit } from '@angular/core';
import { NotificationTypeService } from '../../services/notificationtype.service';
import { NotificationType} from '../../models/notificationconfiguration.model';
// import { DropDownType } from '../../../../models/dropdowntype.model';
// import { CommonService } from "../../../../services/common.service";
// import { MasterDataService } from '../../../../services/masterdata.service';
import * as servicePath from '../../../../service-paths';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification-types-table',
  templateUrl: './notification-types-table.component.html',
  styleUrls: ['./notification-types-table.component.css']
})
export class NotificationTypesTableComponent implements OnInit {
  recordsPerPage = 5;
  notificationTypeList : NotificationType[] = [] ;
  // selectedRow : NotificationType;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  cols = [
    { field: 'CategoryName', header: 'Category' },
    {field : 'NotificationCode', header : 'Notification Type Code'},
    {field : 'NotificationDescription', header : 'Notification Type Description'},
];
constructor(private _serviceObj : NotificationTypeService) {
  this.PageSize = this.resources.PageSize;
  this.PageDropDown = this.resources.PageDropDown;
 }

  ngOnInit() {
    this._serviceObj.GetNotificationTypes();
    this._serviceObj.notificationTypeData.subscribe((data) => {
      data.forEach(function (value) {
        if (value.CategoryMaster != null)
          value.CategoryName = value.CategoryMaster.CategoryName;
        else
          value.CategoryName = '';
      });


      this.notificationTypeList = data;
    })

  //.subscribe((response: NotificationType[]) => {this.notificationTypeList = response});
  }

// getNotificationTypes(): void {
//   this.serviceObj.GetNotificationTypes().subscribe((notificationConfigurationTypeResponse: NotificationType[]) => {
//       this.notificationTypeList = notificationConfigurationTypeResponse;
//   }),
//       (error: any) => {
//           if (error._body != undefined && error._body != "")
//               this._commonService.LogError(this.componentName, error._body).then((data: any) => {
//               });
//       };
// }

GetNotificationList(){
  this._serviceObj.GetNotificationTypes()
  //.subscribe((response : NotificationType[]) =>{ this.notificationTypeList = response});;

}
editnotificationtype(editObj){
  this._serviceObj.editMode = true;
  this._serviceObj.editObj.next(editObj);
 }


}
