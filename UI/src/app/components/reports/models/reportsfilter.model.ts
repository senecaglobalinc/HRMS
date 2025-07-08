import { ReportsData } from './reportsdata.model';
export class PagingData {
    public RowsPerPage: number;
    public PageNumber: number;
}
export class UtilizationReportFilterData  extends PagingData{
    public EmployeeId: number;
    public ProjectId: number;
    public GradeId: number;
    public DesignationId: number;
    public ClientId: number;
    public AllocationPercentageId: number;
    public ProgramManagerId: number;
    public ExperienceId: number;
    public ExperienceRange: string;
    public Experience: number;
    public IsBillable: number;
    public IsCritical: number;
    public isExportToExcel: boolean
    public PracticeAreaId: number;
}

export class FinanceReportFilterData  extends PagingData{
    public FromDate: Date;
    public ToDate: Date;
    public ProjectId: number;
}

export class utilizationReportByMonthFilterData  extends PagingData{
    public FromMonth: number;
    public ToMonth: number;
    public Year: number;
    public isExportToExcel: boolean
} 
export class ReportsFilterData {
    public financeReportFilterData: FinanceReportFilterData;
    public utilizationReportFilterData: UtilizationReportFilterData;
    public utilizationReportByMonthFilterData: utilizationReportByMonthFilterData;
    public reportsData: Array<ReportsData>;
    public TotalCount: number;
}