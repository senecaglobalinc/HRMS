import { Observable } from 'rxjs/Observable';
import { FinancialYear } from '../models/financialyear.model';
export interface IFinancialYear {
    GetFinancialYearList(): Observable<FinancialYear[]>;
    GetCurrentFinancialYear(): Observable<number>;
    CreateFinancialYear(financialYear: FinancialYear): Observable<number>;
    UpdateFinancialYear(financialYear: FinancialYear): Observable<number>;
}