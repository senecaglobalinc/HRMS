import { Observable } from 'rxjs/Observable';
import { AspectData } from  'src/app/modules/master-layout/models/kra.model';
export interface IAspectMaster {
   GetAspectMasterList(): Observable<AspectData[]>;
   CreateAspect(aspectName: string): Observable<boolean>;
   UpdateAspect(aspectData: AspectData): Observable<boolean>;
   DeleteAspect(aspectId: number): Observable<number>;
}