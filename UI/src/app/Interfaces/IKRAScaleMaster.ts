import { Observable } from 'rxjs/Observable';
import { KRAScaleMaster, KRAScaleDetails } from '../models/krascaleData.model';
export interface IKRAScaleMaster {
    GetKRAScale(): Observable<Array<KRAScaleMaster>>;
    GetKRADescriptionDetails(kraScaleMasterId:number):Observable<Array<KRAScaleDetails>>;
    CreateKRAScale(kraScaleMaster: KRAScaleMaster): Observable<number>;
    UpdateKRAScale(kraScaleMaster: KRAScaleMaster): Observable<number>;
    DeleteKRAScale(kraScaleMasterId: number);
}
