import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SelectItem, Message, MessageService } from 'primeng/components/common/api';
import { DropDownType } from '../../../../models/dropdowntype.model';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { CommonService } from "../../../../services/common.service";
import { MasterDataService } from '../../../../services/masterdata.service';
import { NotificationConfigurationService } from '../../services/notificationconfiguration.service';
import { NotificationTypeService } from '../../services/notificationtype.service';
import { NotificationConfiguration, Email, NotificationType } from '../../models/notificationconfiguration.model';

@Component({
    selector: 'app-notification-configuration-form',
    templateUrl: './notification-configuration-form.component.html',
    styleUrls: ['./notification-configuration-form.component.css'],
    providers: [MessageService]

})
export class NotificationConfigurationFormComponent implements OnInit {
    errorMessage: Message[] = [];
    notificationConfigurationDetails: NotificationConfiguration;
    notificationTypeList: SelectItem[] = [];
    notificationType: NotificationType[] = [];
    filteredEmailIds: Email[] = [];
    componentName: string;
    formSubmitted: boolean = false;
    buttonText: string = "Save";
    EmailFrom: string = "";
    notificationTypeID: number;
    categoryID: number;
    notificationDescripttion: string;
    categoriesList: DropDownType[] = [];
    constructor(private _http: HttpClient, private _actRoute: ActivatedRoute,
        private serviceObj: NotificationConfigurationService, private notificationTypeService: NotificationTypeService,
        private masterDataService: MasterDataService, private _commonService: CommonService, private messageService: MessageService) {
        this.notificationConfigurationDetails = new NotificationConfiguration();
        this.notificationConfigurationDetails.ToEmail = new Array<Email>();
        this.notificationConfigurationDetails.CCEmail = new Array<Email>();
        this.componentName = this._actRoute.routeConfig.component.name;
    }

    ngOnInit() {
        this.getCategories();
        this.notificationTypeList.splice(0, 0, { label: 'Select Notification   Type', value: 0 });

        this.serviceObj.notificationTypeData.subscribe(data => {
            // if (this.serviceObj.editMode == true) {

            // }
            this.notificationType = data;
        });
        this.clear();
    }
    getCategories(): void {
        this.categoriesList = [];
        this.categoriesList.splice(0, 0, { label: 'Select Category', value: 0 });
        this.masterDataService.GetCategories().subscribe((res: any[]) => {
            res.forEach(ele => {
                this.categoriesList.push({ label: ele.CategoryName, value: ele.CategoryMasterId });
            });
        })
        //   (error: any) => {
        //       if (error._body != undefined && error._body != "")
        //           this._commonService.LogError(this.componentName, error._body).then((data: any) => {
        //           });

        //       this.growlerrormessages("error", "Failed to get categories", "");
        //   };
    };
    //   growlerrormessages(severity: string, summary: string, detail: string): void {
    //       this.errorMessage = [];
    //       this.errorMessage.push({ severity: severity, summary: summary, detail: detail });
    //       event.preventDefault();
    //   }

    getNotificationtypes(categoryId: number): void {
        if (categoryId) {
            this.notificationTypeList = [];
            this.notificationConfigurationDetails.NotificationTypeId = 0;
            this.notificationTypeList.splice(0, 0, { label: 'Select Notification   Type', value: 0 });
            if (categoryId != 0) {
                this.serviceObj.GetNotificationTypes().subscribe((res: any[]) => {
                    res.forEach((notificationType: NotificationType) => {
                        if (notificationType.CategoryMasterId == categoryId) {
                            this.notificationTypeList.push({ label: notificationType.NotificationCode, value: notificationType.NotificationTypeId });
                        }
                    });
                    this.GetNotificationCofigurationByNotificationType(this.notificationConfigurationDetails.NotificationTypeId, categoryId);
                },
                    (error) => {
                        this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: error.error });

                    });
            }
        }
        else {
            this.clear();
        }
    }

    filterEmailIDs(event: any): void {
        let suggestionString = event.query;
        this.masterDataService.GetEmailIDsByString(suggestionString).subscribe((emailListResponse: string[]) => {
            this.filteredEmailIds = [];
            for (let i = 0; i < emailListResponse.length; i++) {
                this.filteredEmailIds.push({ EmailID: emailListResponse[i] })
            }
        },
            (error: any) => {
                if (error._body != undefined && error._body != "")
                    this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                    });
                this.errorMessage = [];
                this.errorMessage.push({ severity: 'error', summary: 'Failed to get Email suggestions List!' });
                this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: error.error });

            });
    }

    GetNotificationCofigurationByNotificationType(notificationTypeId: number, categoryMasterId: number): void {
        if (notificationTypeId != 0 && categoryMasterId != 0) {
            this.notificationTypeID = notificationTypeId;
            this.categoryID = categoryMasterId;
            this.serviceObj.GetNotificationCofigurationByNotificationType(notificationTypeId, categoryMasterId)
                .subscribe((notificationConfigurationResponse: NotificationConfiguration) => {
                    this.formSubmitted = false;
                    if (notificationConfigurationResponse != null) {
                        this.notificationConfigurationDetails = new NotificationConfiguration();
                        this.notificationConfigurationDetails.EmailFrom = this.getFromEmail();
                        this.notificationConfigurationDetails.CCEmail = new Array<Email>();
                        this.notificationConfigurationDetails.ToEmail = new Array<Email>();
                        if (notificationConfigurationResponse.EmailTo != "" && notificationConfigurationResponse.EmailTo != null) {
                            if (notificationConfigurationResponse && notificationConfigurationResponse.CategoryMasterId != null)
                                this.notificationConfigurationDetails.CategoryMasterId = notificationConfigurationResponse.CategoryMasterId;
                            else
                                this.notificationConfigurationDetails.CategoryMasterId = 0;

                            if (notificationConfigurationResponse && notificationConfigurationResponse.NotificationTypeId != null)
                                this.notificationConfigurationDetails.NotificationTypeId = notificationConfigurationResponse.NotificationTypeId;
                            else
                                this.notificationConfigurationDetails.NotificationTypeId = 0;
                            if (notificationConfigurationResponse && notificationConfigurationResponse.EmailSubject != "")
                                this.notificationConfigurationDetails.EmailSubject = notificationConfigurationResponse.EmailSubject;
                            else
                                this.notificationConfigurationDetails.EmailSubject = "";
                            if (notificationConfigurationResponse && notificationConfigurationResponse.NotificationType &&
                                notificationConfigurationResponse.NotificationType.NotificationDescription != undefined &&
                                notificationConfigurationResponse.NotificationType.NotificationDescription != null)
                                this.notificationConfigurationDetails.NotificationDescription =
                                    notificationConfigurationResponse.NotificationType.NotificationDescription;
                            else
                                this.notificationConfigurationDetails.NotificationDescription = "";
                            if (notificationConfigurationResponse && notificationConfigurationResponse.EmailContent != "") {
                                var plainText = notificationConfigurationResponse.EmailContent.replace(/<[^>]*>/g, '');
                                this.notificationConfigurationDetails.EmailContent = plainText;
                                // this.notificationConfigurationDetails.EmailContent = this._commonService.htmlDecode(notificationConfigurationResponse.EmailContent);
                            }
                            else
                                this.notificationConfigurationDetails.EmailContent = "";
                            if (notificationConfigurationResponse && notificationConfigurationResponse.EmailTo != "" && notificationConfigurationResponse.EmailTo != null) {
                                let emailTo = notificationConfigurationResponse.EmailTo.split(";");
                                if (emailTo.length == 1) {
                                    this.notificationConfigurationDetails.ToEmail.push({ EmailID: emailTo[0] });
                                }
                                else {
                                    for (let i = 0; i < emailTo.length; i++) {
                                        this.notificationConfigurationDetails.ToEmail.push({ EmailID: emailTo[i] });
                                    }
                                }
                            }
                            else {
                                this.notificationConfigurationDetails.ToEmail = []
                            }
                            if (notificationConfigurationResponse && notificationConfigurationResponse.EmailCC != "") {
                                let emailCC = notificationConfigurationResponse.EmailCC.split(";");
                                if (emailCC.length == 1) {
                                    this.notificationConfigurationDetails.CCEmail.push({ EmailID: emailCC[0] });
                                }
                                else {
                                    for (let i = 0; i < emailCC.length; i++) {
                                        this.notificationConfigurationDetails.CCEmail.push({ EmailID: emailCC[i] });
                                    }
                                }
                            }
                            else {
                                this.notificationConfigurationDetails.CCEmail = []
                            }
                            this.buttonText = "Update";

                        }
                        else {
                            this.notificationConfigurationDetails = {
                                NotificationTypeId: notificationTypeId,
                                NotificationType: null,
                                NotificationCode: "",
                                NotificationDescription: notificationConfigurationResponse.NotificationDescription,
                                EmailFrom: this.getFromEmail(),
                                EmailTo: "",
                                EmailCC: "",
                                ToEmail: new Array<Email>(),
                                CCEmail: new Array<Email>(),
                                EmailSubject: "",
                                EmailContent: "",
                                CategoryMasterId: categoryMasterId,
                                CategoryMaster: null,
                                CategoryName: ""
                            }
                            this.buttonText = "Save";
                        }
                    }
                    else {
                        this.notificationConfigurationDetails = {
                            NotificationTypeId: notificationTypeId,
                            NotificationType: null,
                            NotificationCode: "",
                            NotificationDescription: "",
                            EmailFrom: this.getFromEmail(),
                            EmailTo: "",
                            EmailCC: "",
                            ToEmail: new Array<Email>(),
                            CCEmail: new Array<Email>(),
                            EmailSubject: "",
                            EmailContent: "",
                            CategoryMasterId: categoryMasterId,
                            CategoryMaster: null,
                            CategoryName: ""
                        }
                        this.buttonText = "Save";
                    }
                })
            //   (error: any) => {
            //       if (error._body != undefined && error._body != "")
            //           this._commonService.LogError(this.componentName, error._body).then((data: any) => {
            //           });

            //       this.growlerrormessages("error", "Failed to get Notification Configuration", "");
            //   };
        }
        else {
            this.notificationConfigurationDetails = {
                NotificationTypeId: notificationTypeId,
                NotificationType: null,
                NotificationCode: "",
                NotificationDescription: "",
                EmailFrom: this.getFromEmail(),
                EmailTo: "",
                EmailCC: "",
                ToEmail: new Array<Email>(),
                CCEmail: new Array<Email>(),
                EmailSubject: "",
                EmailContent: "",
                CategoryMasterId: categoryMasterId,
                CategoryMaster: null,
                CategoryName: ""
            }
        }
    }

    getFromEmail(): string {
        this.serviceObj.GetFromEmail().subscribe((emailFrom: string) => {
            this.EmailFrom = emailFrom;
        });
        return this.EmailFrom
    }

    onSave(notificationConfigurationDetails: NotificationConfiguration): void {
        this.formSubmitted = true;
        if (notificationConfigurationDetails.CategoryMasterId == 0) return;
        if (notificationConfigurationDetails.NotificationTypeId == 0) return;
        if (notificationConfigurationDetails.EmailFrom == "" || notificationConfigurationDetails.EmailSubject == "" || notificationConfigurationDetails.EmailContent == "") return;
        if (notificationConfigurationDetails.EmailFrom == null || notificationConfigurationDetails.EmailSubject == null || notificationConfigurationDetails.EmailContent == null) return;
        if (notificationConfigurationDetails.ToEmail.length == 0) return;
        notificationConfigurationDetails.EmailContent = notificationConfigurationDetails.EmailContent
        // this._commonService.htmlEncode(notificationConfigurationDetails.EmailContent);
        notificationConfigurationDetails.EmailTo = "";
        notificationConfigurationDetails.ToEmail.map(function (email: Email) {
            return notificationConfigurationDetails.EmailTo += email.EmailID + ';'
        });
        notificationConfigurationDetails.EmailCC = "";
        notificationConfigurationDetails.CCEmail.map(function (email: Email) { return notificationConfigurationDetails.EmailCC += email.EmailID + ';' });
        notificationConfigurationDetails.EmailTo = notificationConfigurationDetails.EmailTo.substr(0, notificationConfigurationDetails.EmailTo.lastIndexOf(';'));
        notificationConfigurationDetails.EmailCC = notificationConfigurationDetails.EmailCC.substr(0, notificationConfigurationDetails.EmailCC.lastIndexOf(';'));
        if (this.buttonText == "Save") {
            this.serviceObj.SaveNotificationCofiguration(notificationConfigurationDetails).subscribe((response: boolean) => {
                //   this.growlerrormessages("success", "Notification Configuration Saved successfully", "");
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: "Notification configuration record added successfully." });
                this.clear();
            },
                //   ,
                (error: any) => {
                    if (error._body != undefined && error._body != "")
                        this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                        });

                    //this.growlerrormessages("error", "Failed to Save Notification Configuration", "");
                    this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: "Failed to add notification configuration." });

                });
        }
        else {
            this.serviceObj.UpdateNotificationCofiguration(notificationConfigurationDetails).subscribe((response: boolean) => {
                //   this.growlerrormessages("success", "Notification Configuration Updated successfully", "");
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: "Notification configuration record updated successfully." });

                this.clear();
            },
                (error: any) => {
                    if (error._body != undefined && error._body != "")
                        this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                        });
                    this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: "Failed to update notification configuration." });

                    //           this.growlerrormessages("error", "Failed to Update Notification Configuration", "");
                });
        }
    }

    clear() {
        this.formSubmitted = false;
        this.buttonText = "Save";
        this.notificationConfigurationDetails = new NotificationConfiguration();
        this.notificationTypeList = [];
        this.notificationTypeList.splice(0, 0, { label: 'Select notification type', value: 0 });
        this.notificationConfigurationDetails.ToEmail = new Array<Email>();
        this.notificationConfigurationDetails.CCEmail = new Array<Email>();
    }
}