import { NotificationType, NotificationConfiguration } from "../components/admin/models/notificationconfiguration.model";

export interface INotificationConfiguration {
    GetNotificationCofigurationByNotificationType(NotificationTypeId: number,categoryId:number);
    SaveNotificationCofiguration(NotificationCofigurationDetails: NotificationConfiguration);
    UpdateNotificationCofiguration(NotificationCofigurationDetails: NotificationConfiguration);
    GetFromEmail();
}
export interface INotificationType {
    GetNotificationTypes();
    AddNotificationType(NotificationTypeDetails: NotificationType);
    UpdateNotificationType(NotificationTypeDetails: NotificationType);
    DeleteNotificationType(notificationTypeId: number,categoryId:number);
}