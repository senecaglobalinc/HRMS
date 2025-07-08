import { Observable } from 'rxjs/Observable';
import { Appreciation, ADRCycle } from '../Models/associate-developement.review.model';
import { GenericType } from 'src/app/models/dropdowntype.model';

export interface IAssociateAppreciation {
    GetSentAppreciationsList(employeeID:number): Observable<Appreciation[]>;
    GetReceiveAppreciationsList(employeeID:number): Observable<Appreciation[]>;
    GetADRCycleList():Observable<ADRCycle[]>;
    GetAppreciationTypeList(): Observable<GenericType[]>;
    SendAnAppreciation(appreciation: Appreciation): Observable<boolean>;
    UpdateAnAppreciation(appreciation: Appreciation): Observable<boolean>
    DeleteAnAppreciation(rowIdForDelete: number): Observable<boolean>;
}