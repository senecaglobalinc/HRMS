import { Observable } from 'rxjs/Observable';
import { CloneKRA } from '../models/clonekra.model';
export interface ICloneKRAs {
   CloneKRAs(cloneKRA: CloneKRA): Observable<number>
}