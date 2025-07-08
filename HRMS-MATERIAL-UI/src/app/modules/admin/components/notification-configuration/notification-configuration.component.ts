import { Component, OnInit, ElementRef, ViewChild, Input } from "@angular/core";
import { themeconfig } from "../../../../../themeconfig";
import { ActivatedRoute } from "@angular/router";
import { DropDownType } from "../../../../modules/master-layout/models/dropdowntype.model";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";
import {
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
  FormGroupDirective,
  NgForm
} from "@angular/forms";
import { CommonService } from "../../../../core/services/common.service";
import { MasterDataService } from "../../../../core/services/masterdata.service";
import { NotificationConfigurationService } from "../../services/notificationconfiguration.service";
import { NotificationTypeService } from "../../services/notificationtype.service";
import {
  NotificationConfiguration,
  Email,
  NotificationType
} from "../../models/notificationconfiguration.model";
import { COMMA, ENTER } from "@angular/cdk/keycodes";
import { MatChipInputEvent, MatChipList } from "@angular/material/chips";
import { MatChipsModule } from "@angular/material/chips";
import { map, startWith } from "rxjs/operators";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from "@angular/material/snack-bar";
import { Observable } from "rxjs/internal/Observable";
import { editorConfig } from "../../../../core/angularEditorConfiguratioan";
import { element } from "protractor";

@Component({
  selector: "app-notification-configuration",
  templateUrl: "./notification-configuration.component.html",
  styleUrls: ["./notification-configuration.component.scss"]
})
export class NotificationConfigurationComponent implements OnInit {
  editorConfig = editorConfig;
  erroroccured: boolean;
  themeAppearence = themeconfig.formfieldappearances;
  notificationTypeList = [];
  emailid;
  filteredOptions: Observable<string[]>;
  notificationConfigurationDetails: NotificationConfiguration;
  filteredOptionsnotification: Observable<string[]>;
  notificationType: NotificationType[] = [];
  filteredEmailIds: string[];
  filteredCCEmailIds: string[];
  componentName: string;
  formSubmitted = false;
  buttonText = "Save";
  isEmail: boolean = false
  EmailFrom = "";
  isCategory: boolean = false;
  notificationTypeID: number;
  categoryID: number;
  notificationDescripttion: string;
  categoriesList = [];
  NotificationConfig: FormGroup;
  visible = true;
  loading: boolean;
  selectable = true;
  removable = true;
  addOnBlur = false;
  notificationTypeDescriptionList:any[];
  horizontalPosition: MatSnackBarHorizontalPosition = "right";
  verticalPosition: MatSnackBarVerticalPosition = "top";
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  @ViewChild("EmailInput") EmailInput: ElementRef<HTMLInputElement>;
  @ViewChild("EmailCCInput") EmailCCInput: ElementRef<HTMLInputElement>;
  @ViewChild("chipListEmailTo") chipListEmailTo: MatChipList;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  constructor(
    private _http: HttpClient,
    private _actRoute: ActivatedRoute,
    private serviceObj: NotificationConfigurationService,
    private notificationTypeService: NotificationTypeService,
    private masterDataService: MasterDataService,
    private _commonService: CommonService,
    private snackBar: MatSnackBar
  ) {
    this.notificationConfigurationDetails = new NotificationConfiguration();
    this.notificationConfigurationDetails.ToEmail = new Array<Email>();
    this.notificationConfigurationDetails.CCEmail = new Array<Email>();
    this.componentName = this._actRoute.routeConfig.component.name;
    this.getCategories();
  }

  ngOnInit() {
    this.NotificationConfig = new FormGroup({
      notificationCategoryControl: new FormControl("", [Validators.required]),
      notificationTypeControl: new FormControl("", [Validators.required]),
      notificationDescriptionControl: new FormControl(""),
      emailFromControl: new FormControl(""),
      subjectControl: new FormControl(null, [Validators.required]),
      emailToControl: new FormControl(Array[""], Validators.required),
      emailCCControl: new FormControl(Array[""]),
      emailContentControl: new FormControl(null, [Validators.required])
    });

    this.NotificationConfig.get("notificationDescriptionControl").disable();
    this.NotificationConfig.get("emailFromControl").disable();

    this.serviceObj.notificationTypeData.subscribe(data => {
      this.notificationType = data;
    });


  }
  private _filtercategory(value) {
    let filterValue;

    if (value && value.notificationCategoryControl) {
      filterValue = value.notificationCategoryControl.toString().toLowerCase();
      return this.categoriesList.filter(option =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.categoriesList;
    }
  }
  private _filternotification(value) {
    let filterValue;
    if (value && value.notificationTypeControl) {
      filterValue = value.notificationTypeControl.toString().toLowerCase();
      return this.notificationTypeList.filter(option => {
        return option.label.toLowerCase().includes(filterValue)
      }
      );
    } else {
      return this.notificationTypeList;
    }
  }

  displayFn(user: any) {
    return user ? user.label : "";
  }
  returnFn(user: any): number | undefined {
    return user ? user.value : undefined;
  }
  getCategories(): void {
    this.categoriesList = [];
    // this.categoriesList.splice(0, 0, { label: "Enter Category", value: null});
    this.masterDataService.GetCategories().subscribe((res: any[]) => {
      res.forEach(ele => {
        this.categoriesList.push({
          label: ele.CategoryName,
          value: ele.CategoryMasterId
        });
      });
      this.filteredOptions = this.NotificationConfig.valueChanges.pipe(
        startWith(""),
        map(value => this._filtercategory(value))
      );
    });
    
  }
  getNotificationtypes(event: any): void {
    this.chipListEmailTo.errorState = false;
    this.NotificationConfig.get("notificationTypeControl").reset();
    this.NotificationConfig.get("notificationDescriptionControl").reset();
    this.NotificationConfig.get("emailFromControl").reset();
    this.NotificationConfig.get("subjectControl").reset();
    this.NotificationConfig.get("emailToControl").reset();
    this.NotificationConfig.get("emailCCControl").reset();
    this.NotificationConfig.get("emailContentControl").reset();
    if (this.notificationConfigurationDetails.ToEmail.length !== 0) {
      this.chipListEmailTo.errorState = false;
    }

    this.notificationConfigurationDetails.ToEmail = [];
    this.chipListEmailTo.errorState = false;
    this.notificationConfigurationDetails.CCEmail = [];
    this.buttonText = "Save";
    // tslint:disable-next-line:prefer-const
    let categoryId = event.option.value.value;
    this.notificationConfigurationDetails.CategoryMasterId = categoryId;
    this.notificationTypeList = [];
    this.notificationTypeDescriptionList = [];
    // this.notificationConfigurationDetails.NotificationTypeId = 0;
    this.filteredOptionsnotification = this.NotificationConfig.valueChanges.pipe(
      startWith(""),
      map(value => this._filternotification(value))
    );
    if (categoryId == null)
      this.isCategory = true
    this.loading = true;
    this.serviceObj.GetNotificationTypes().subscribe(
      (res: any[]) => {
        res.forEach((notificationType: NotificationType) => {
          if (notificationType.CategoryMasterId === categoryId) {
            // tslint:disable-next-line:max-line-length
            this.notificationTypeList.push({
              label: notificationType.NotificationCode,
              value: notificationType.NotificationTypeId
            });
            this.notificationTypeDescriptionList.push({
              label: notificationType.NotificationDescription,
              value: notificationType.NotificationTypeId
            });
          }
        });
        this.loading = false
      },
      error => {
        this.snackBar.open(error.error, "x", {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition
        });
      }
    );
  }

  filterEmailIDs(event: any): void {
    // tslint:disable-next-line:prefer-const
    let suggestionString = event.target.value;
    if (suggestionString.length >= 3) {
      //
      this.masterDataService.GetEmailIDsByString().subscribe(
        (emailListResponse) => {
          this.filteredEmailIds = [];
          this.filteredEmailIds = emailListResponse.filter((option) =>
            option.toLowerCase().includes(suggestionString)
          );
          if (this.filteredEmailIds.length < 1) {
            this.isEmail = true
          }
          else {
            this.isEmail = false
          }
        },

        (error: any) => {
          if (error._body !== undefined && error._body !== "") {
          }
          this._commonService
            .LogError(this.componentName, error._body)
            .subscribe((data: any) => { });
          this.snackBar.open("Failed to get Email Suggestions", "x", {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition
          });
        }
      );
    }
  }

  filterCCEmailIDs(event: any): void {
    // tslint:disable-next-line:prefer-const
    let suggestionString = event.target.value;
    if (suggestionString.length >= 3) {
      //
      this.masterDataService.GetEmailIDsByString().subscribe(
        (emailListResponse) => {
          this.filteredCCEmailIds = [];
          this.filteredCCEmailIds = emailListResponse.filter((option) =>
            option.toLowerCase().includes(suggestionString)
          );
          if (this.filteredCCEmailIds.length < 1) {
            this.isEmail = true
          }
          else {
            this.isEmail = false
          }
        },

        (error: any) => {
          if (error._body !== undefined && error._body !== "") {
          }
          this._commonService
            .LogError(this.componentName, error._body)
            .subscribe((data: any) => { });
          this.snackBar.open("Failed to get Email Suggestions", "x", {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition
          });
        }
      );
    }
  }

  GetNotificationCofigurationByNotificationType(
    event: any,
    categoryMasterId: number
  ): void {
    this.chipListEmailTo.errorState = false;
    this.NotificationConfig.get("notificationDescriptionControl").reset();
    this.NotificationConfig.get("emailFromControl").reset();
    this.NotificationConfig.get("subjectControl").reset();
    this.NotificationConfig.get("emailToControl").reset();
    this.NotificationConfig.get("emailCCControl").reset();
    this.NotificationConfig.get("emailContentControl").reset();
    this.notificationConfigurationDetails.ToEmail = [];
    this.notificationConfigurationDetails.CCEmail = [];

    this.buttonText = "Save";
    let notificationTypeId;
    if (categoryMasterId !== null && event !== null) {
      notificationTypeId = event.option.value.value;
      this.notificationConfigurationDetails.NotificationTypeId = notificationTypeId;
    }
    //

    //
    if (notificationTypeId !== 0 && categoryMasterId !== 0) {
      this.notificationTypeID = notificationTypeId;
      this.categoryID = categoryMasterId;
      this.serviceObj
        .GetNotificationCofigurationByNotificationType(
          notificationTypeId,
          categoryMasterId
        )
        .subscribe(
          (notificationConfigurationResponse: NotificationConfiguration) => {
            this.formSubmitted = false;
            if (notificationConfigurationResponse != null) {
              this.notificationConfigurationDetails = new NotificationConfiguration();
              this.notificationConfigurationDetails.EmailFrom = this.getFromEmail();
              this.notificationConfigurationDetails.CCEmail = new Array<
                Email
              >();
              this.notificationConfigurationDetails.ToEmail = new Array<
                Email
              >();
              if (
                notificationConfigurationResponse.EmailTo !== "" &&
                notificationConfigurationResponse.EmailTo != null
              ) {
                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.CategoryMasterId != null
                ) {
                  this.notificationConfigurationDetails.CategoryMasterId =
                    notificationConfigurationResponse.CategoryMasterId;
                } else {
                  this.notificationConfigurationDetails.CategoryMasterId = 0;
                }

                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.NotificationTypeId != null
                ) {
                  this.notificationConfigurationDetails.NotificationTypeId =
                    notificationConfigurationResponse.NotificationTypeId;
                } else {
                  this.notificationConfigurationDetails.NotificationTypeId = 0;
                }
                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.EmailSubject !== ""
                ) {
                  this.notificationConfigurationDetails.EmailSubject =
                    notificationConfigurationResponse.EmailSubject;
                  this.NotificationConfig.get("subjectControl").patchValue(
                    this.notificationConfigurationDetails.EmailSubject
                  );
                } else {
                  this.notificationConfigurationDetails.EmailSubject = "";
                  this.NotificationConfig.get("subjectControl").patchValue(
                    this.notificationConfigurationDetails.EmailSubject
                  );
                }
                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.NotificationType &&
                  notificationConfigurationResponse.NotificationType
                    .NotificationDescription !== undefined &&
                  notificationConfigurationResponse.NotificationType
                    .NotificationDescription != null
                ) {
                  this.notificationConfigurationDetails.NotificationDescription =
                    notificationConfigurationResponse.NotificationType.NotificationDescription;
                  this.NotificationConfig.get(
                    "notificationDescriptionControl"
                  ).patchValue(
                    this.notificationConfigurationDetails
                      .NotificationDescription
                  );
                } else {
                  this.notificationConfigurationDetails.NotificationDescription =
                    "";
                  this.NotificationConfig.get(
                    "notificationDescriptionControl"
                  ).patchValue(
                    this.notificationConfigurationDetails
                      .NotificationDescription
                  );
                }
                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.EmailContent !== ""
                ) {

                  this.notificationConfigurationDetails.EmailContent =
                    notificationConfigurationResponse.EmailContent;
                  this.NotificationConfig.patchValue({
                    emailContentControl: this.notificationConfigurationDetails
                      .EmailContent
                  });

                } else {
                  this.notificationConfigurationDetails.EmailContent = "";
                  this.NotificationConfig.patchValue({
                    emailContentControl: this.notificationConfigurationDetails
                      .EmailContent
                  });
                }

                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.EmailTo !== "" &&
                  notificationConfigurationResponse.EmailTo != null
                ) {
                  //
                  let emailTo = notificationConfigurationResponse.EmailTo.split(
                    ";"
                  );
                  //
                  if (emailTo.length === 1) {
                    this.notificationConfigurationDetails.ToEmail.push({
                      EmailID: emailTo[0]
                    });
                    //
                    this.emailid = this.notificationConfigurationDetails.ToEmail[0].EmailID;
                  } else {
                    for (let i = 0; i < emailTo.length; i++) {
                      this.notificationConfigurationDetails.ToEmail.push({
                        EmailID: emailTo[i]
                      });
                    }
                  }
                  //
                } else {
                  this.notificationConfigurationDetails.ToEmail = [];
                }
                if (
                  notificationConfigurationResponse &&
                  notificationConfigurationResponse.EmailCC !== ""
                ) {
                  let emailCC = notificationConfigurationResponse.EmailCC.split(
                    ";"
                  );
                  if (emailCC.length === 1) {
                    this.notificationConfigurationDetails.CCEmail.push({
                      EmailID: emailCC[0]
                    });
                  } else {
                    for (let i = 0; i < emailCC.length; i++) {
                      this.notificationConfigurationDetails.CCEmail.push({
                        EmailID: emailCC[i]
                      });
                    }
                  }
                } else {
                  this.notificationConfigurationDetails.CCEmail = [];
                }
                this.buttonText = "Update";
              } else {
                this.notificationConfigurationDetails = {
                  NotificationTypeId: notificationTypeId,
                  NotificationType: null,
                  NotificationCode: "",
                  NotificationDescription:
                    notificationConfigurationResponse.NotificationDescription,
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
                };
                this.buttonText = "Save";
              }
            } else {
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
              };
              this.buttonText = "Save";
            }
          }
        );
    } else {
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
      };
    }
    if (this.NotificationConfig.get("notificationDescriptionControl").value == "" || this.NotificationConfig.get("notificationDescriptionControl").value == null || this.NotificationConfig.get("notificationDescriptionControl").value == undefined){
      let filteredNotificationTypes: any;
      filteredNotificationTypes = this.notificationTypeDescriptionList.filter(filteredNotificationTypes => filteredNotificationTypes.value == notificationTypeId)
      this.NotificationConfig.get("notificationDescriptionControl").patchValue(
        filteredNotificationTypes[0].label
      );

    }
  }

  getFromEmail(): string {
    this.serviceObj.GetFromEmail().subscribe((emailFrom: string) => {
      this.EmailFrom = emailFrom;
    });
    this.NotificationConfig.get("emailFromControl").patchValue(this.EmailFrom);
    return this.EmailFrom;
  }

  onSave(notificationConfigurationDetails: NotificationConfiguration): void {
    this.formSubmitted = true;
    this.notificationConfigurationDetails.EmailContent = this.NotificationConfig.get(
      "emailContentControl"
    ).value;
    this.notificationConfigurationDetails.EmailFrom = this.NotificationConfig.get(
      "emailFromControl"
    ).value;
    this.notificationConfigurationDetails.NotificationDescription = this.NotificationConfig.get(
      "notificationDescriptionControl"
    ).value;
    this.notificationConfigurationDetails.EmailSubject = this.NotificationConfig.get(
      "subjectControl"
    ).value;



    if (notificationConfigurationDetails.CategoryMasterId === 0) {
      return;
    }
    if (notificationConfigurationDetails.NotificationTypeId === 0) {
      return;
    }

    if (
      notificationConfigurationDetails.EmailFrom === "" ||
      notificationConfigurationDetails.EmailSubject === "" ||
      notificationConfigurationDetails.EmailContent === ""
    ) {
      return;
    }

    if (
      notificationConfigurationDetails.EmailFrom == null ||
      notificationConfigurationDetails.EmailSubject == null ||
      notificationConfigurationDetails.EmailContent == null
    ) {
      return;
    }
    if (notificationConfigurationDetails.ToEmail.length === 0) {
      this.chipListEmailTo.errorState = true;
      return;
    }
    notificationConfigurationDetails.EmailContent =
      notificationConfigurationDetails.EmailContent;

    notificationConfigurationDetails.EmailTo = "";

    notificationConfigurationDetails.ToEmail.map(function (email: Email) {
      return (notificationConfigurationDetails.EmailTo += email.EmailID + ";");
    });
    notificationConfigurationDetails.EmailCC = "";

    notificationConfigurationDetails.CCEmail.map(function (email: Email) {
      return (notificationConfigurationDetails.EmailCC += email.EmailID + ";");
    });

    notificationConfigurationDetails.EmailTo = notificationConfigurationDetails.EmailTo.substr(
      0,
      notificationConfigurationDetails.EmailTo.lastIndexOf(";")
    );

    notificationConfigurationDetails.EmailCC = notificationConfigurationDetails.EmailCC.substr(
      0,
      notificationConfigurationDetails.EmailCC.lastIndexOf(";")
    );

    if (this.buttonText === "Save") {
      //
      if (this.notificationConfigurationDetails.ToEmail.length === 0) {
        this.chipListEmailTo.errorState = true;
      } else {
        this.chipListEmailTo.errorState = false;
      }
      //

      this.serviceObj
        .SaveNotificationCofiguration(notificationConfigurationDetails)
        .subscribe(
          (response: boolean) => {
            this.snackBar.open(
              "Notification Configuration Record Added Successfully",
              "x",
              {
                duration: 3000,
                horizontalPosition: this.horizontalPosition,
                verticalPosition: this.verticalPosition
              }
            );
            this.clear();
          },

          (error: any) => {
            if (error._body !== undefined && error._body !== "") {
              this._commonService
                .LogError(this.componentName, error._body)
                .subscribe((data: any) => { });
            }

            this.snackBar.open(
              "Failed to Add Notification Configuration",
              "x",
              {
                duration: 3000,
                panelClass: ['error-alert'],
                horizontalPosition: this.horizontalPosition,
                verticalPosition: this.verticalPosition
              }
            );
          }
        );
    } else {
      if (this.notificationConfigurationDetails.ToEmail.length === 0) {
        this.chipListEmailTo.errorState = true;
      } else {
        this.chipListEmailTo.errorState = false;
      }
      //
      this.serviceObj
        .UpdateNotificationCofiguration(notificationConfigurationDetails)
        .subscribe(
          (response: boolean) => {
            this.snackBar.open(
              "Notification Configuration Updated Successfully",
              "x",
              {
                duration: 3000,
                horizontalPosition: this.horizontalPosition,
                verticalPosition: this.verticalPosition
              }
            );

            this.clear();
          },
          (error: any) => {
            if (error._body !== undefined && error._body !== "") {
              this._commonService
                .LogError(this.componentName, error._body)
                .subscribe((data: any) => { });
            }

            this.snackBar.open(
              "Failed to Update Notification Configuration",
              "x",
              {
                duration: 3000,
                panelClass: ['error-alert'],
                horizontalPosition: this.horizontalPosition,
                verticalPosition: this.verticalPosition
              }
            );
          }
        );
    }
  }
  EmailToSelected(event: any) {
    this.notificationConfigurationDetails.ToEmail.push({
      EmailID: event.option.value
    });
    this.EmailInput.nativeElement.value = "";
    if (this.notificationConfigurationDetails.ToEmail.length === 0) {
      this.chipListEmailTo.errorState = true;
    } else {
      this.chipListEmailTo.errorState = false;
    }
  }
  EmailCCSelected(event: any) {
    this.notificationConfigurationDetails.CCEmail.push({
      EmailID: event.option.value
    });
    this.EmailCCInput.nativeElement.value = "";
  }
  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our fruit
    if ((value || "").trim()) {
      this.notificationConfigurationDetails.CCEmail.push({
        EmailID: value.trim()
      });
    }

    // Reset the input value
    if (input) {
      input.value = "";
    }
  }

  remove(email: Email): void {
    const index = this.notificationConfigurationDetails.CCEmail.indexOf(email);

    if (index >= 0) {
      this.notificationConfigurationDetails.CCEmail.splice(index, 1);
    }
    
  }
  addEmailTo(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || "").trim()) {
      this.notificationConfigurationDetails.ToEmail.push({
        EmailID: value.trim()
      });
      if (this.notificationConfigurationDetails.ToEmail.length === 0) {
        this.chipListEmailTo.errorState = true;
      } else {
        this.chipListEmailTo.errorState = false;
      }
      //
    }

    if (input) {
      input.value = "";
    }
    if (this.notificationConfigurationDetails.ToEmail.length === 0) {
      this.chipListEmailTo.errorState = true;
    } else {
      this.chipListEmailTo.errorState = false;
    }
  }

  removeEmailTo(email: Email): void {
    const index = this.notificationConfigurationDetails.ToEmail.indexOf(email);

    if (index >= 0) {
      this.notificationConfigurationDetails.ToEmail.splice(index, 1);
    }

    //
    if (this.notificationConfigurationDetails.ToEmail.length === 0) {
      this.chipListEmailTo.errorState = true;
    } else {
      this.chipListEmailTo.errorState = false;
    }
    let suggestionString : string = "";
    this.masterDataService.GetEmailIDsByString().subscribe(
      (emailListResponse) => {
        this.filteredEmailIds = [];
        this.filteredEmailIds = emailListResponse.filter((option) =>
          option.toLowerCase().includes(suggestionString)
        );
        if (this.filteredEmailIds.length < 1) {
          this.isEmail = true
        }
        else {
          this.isEmail = false
        }
      },

      (error: any) => {
        if (error._body !== undefined && error._body !== "") {
        }
        this._commonService
          .LogError(this.componentName, error._body)
          .subscribe((data: any) => { });
        this.snackBar.open("Failed to get Email Suggestions", "x", {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition
        });
      }
    );

  }
  clearFieldCategory(event: any) {
    event.stopPropagation();
    this.NotificationConfig.controls.notificationCategoryControl.reset();
    
    this.clearFieldNotification(event);

    this.isCategory = true
  }
  clearFieldNotification(event:any) {
    event.stopPropagation();
    this.NotificationConfig.controls.notificationTypeControl.reset();

    event.stopPropagation();
    this.NotificationConfig.controls.notificationDescriptionControl.reset();
  }

  clear() {
    this.formSubmitted = false;
    this.buttonText = "Save";
    this.notificationConfigurationDetails = new NotificationConfiguration();
    this.notificationTypeList = [];

    this.NotificationConfig.controls.notificationDescriptionControl.reset();



    this.notificationConfigurationDetails.ToEmail = new Array<Email>();
    this.notificationConfigurationDetails.CCEmail = new Array<Email>();

    this.chipListEmailTo.errorState = false;
    this.NotificationConfig.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
  }
}
