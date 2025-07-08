import { Observable } from 'rxjs/Observable';
import { ADRCycle } from '../Models/adrcycle.model';

export interface IAssociateDevelopmentReviewCycle{
    GetADRCycle():  Observable<ADRCycle[]>;
    UpdateADRCycle(updatedADRCycledata:ADRCycle):Observable<any>;
}