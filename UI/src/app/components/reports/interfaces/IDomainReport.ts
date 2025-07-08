
import { DomainDataCount } from '../../Reports/models/domainreport.model';
import { Observable } from 'rxjs/Observable';

export interface IDomainReport {
    GetDomainDataCount(): Observable<DomainDataCount[]>
 }