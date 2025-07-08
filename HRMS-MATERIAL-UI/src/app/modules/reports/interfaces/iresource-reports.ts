import {  ReportsFilterData, UtilizationReportFilterData } from '../models/reportsfilter.model';
import { AllocationDetails } from '../models/resourcereportbyproject.model';
import { Observable } from 'rxjs';
import { ReportsData } from '../models/reportsdata.model';
export interface IResourceReports {
    ResourceReportByFilters(resourceFilter: UtilizationReportFilterData): Observable<ReportsData[]>
    GetResourceReportByProjectId(projectId: number): Observable<AllocationDetails>
    GetUtilizationReportsByTechnology(resourceFilter: ReportsFilterData): Observable<ReportsFilterData>
    GetUtilizationReportsByMonth(resourceFilter: ReportsFilterData): Observable<ReportsFilterData>


}
