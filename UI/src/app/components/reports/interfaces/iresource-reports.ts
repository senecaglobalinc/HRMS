import { UtilizationReportFilterData,  ReportsFilterData } from '../models/reportsfilter.model';
import { ReportsData } from '../models/reportsdata.model';

import { AllocationDetails } from '../models/resourcereportbyproject.model';
import { Observable } from 'rxjs';
export interface IResourceReports {
    ResourceReportByFilters(resourceFilter: UtilizationReportFilterData): Observable<ReportsData[]>
    GetResourceReportByProjectId(projectId: number): Observable<AllocationDetails>
    GetUtilizationReportsByTechnology(resourceFilter: ReportsFilterData): Observable<ReportsFilterData>
    GetUtilizationReportsByMonth(resourceFilter: ReportsFilterData): Observable<ReportsFilterData>


}
