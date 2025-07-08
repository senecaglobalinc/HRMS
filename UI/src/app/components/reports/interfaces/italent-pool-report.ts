import { TalentpoolDataCount } from '../models/talentpool.model';
import { EmployeeReportData } from '../models/employee.model';
import { Observable } from 'rxjs';

export interface ItalentPoolReport {
    GetTalentpoolResourceCount(): Observable<TalentpoolDataCount>

    GetEmployeesByTalentPoolProjectId(projectId: number): Observable<EmployeeReportData>
}
