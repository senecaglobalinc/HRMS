
import { DomainDataCount } from '../../reports/models/domainreport.model';
import { Observable } from 'rxjs/Observable';

export interface IDomainReport {
    GetDomainDataCount(): Observable<DomainDataCount[]>
 }
