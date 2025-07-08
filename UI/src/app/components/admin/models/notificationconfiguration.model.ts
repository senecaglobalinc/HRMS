import { CategoryData } from "./categorymaster.model";

export class NotificationType {
    Id?: number;
    NotificationTypeId?: number;
    CategoryMasterId?:number;
    NotificationCode: string;
    NotificationDescription: string;
    CategoryName:string;
    CategoryMaster:CategoryData;
}

export class NotificationConfiguration extends NotificationType{
    EmailFrom: string;
    EmailTo: string; //passing array properties to string 
    EmailCC: string; //passing array properties to string 
    ToEmail?:Email[]; 
    CCEmail?:Email[];
    EmailSubject : string;
    EmailContent : string;
    NotificationType:NotificationType;
}

export class Email{
    EmailID:string
}
