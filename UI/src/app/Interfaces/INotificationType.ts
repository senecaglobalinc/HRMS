import { Observable } from "rxjs";
import { NotificationType  } from '../components/admin/models/notificationconfiguration.model';
export interface INotificationType {
    GetNotificationTypes(): Observable<NotificationType[]>;
    AddNotificationType(NotificationTypeDetails: NotificationType): Observable<number>;
    UpdateNotificationType(NotificationTypeDetails: NotificationType): Observable<number>;
    DeleteNotificationType(notificationTypeId: number,categoryId:number): Observable<number>;
}