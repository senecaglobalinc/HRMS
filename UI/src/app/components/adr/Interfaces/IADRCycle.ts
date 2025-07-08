import { Observable } from 'rxjs/Observable';
import { ADRCycle } from '../Models/associate-developement.review.model';

export interface IADRCycle{
    GetADRCycle():  Observable<ADRCycle[]>;
    UpdateADRCycle(updatedADRCycledata:ADRCycle):Observable<any>;
}