export class KraSetData {
    KRASetID: number;
    ID: number;
    KRARoleCategoryID: number;
    RoleID: number;
    //KRARoleID: number;
    FinancialYearID: number;
    KRARoleCategoryName: string;
    KRARoleName: string;
    FinancialYear: string;
    KRAAspectID:number
    KRAAspectName:string;
    KRAAspectMetric: string;
    KRAAspectTarget: string;
    FromEmployeeID: number;
    CreatedDate: string;
    FromEmployeeName: string;
    ToEmployeeName: string;
    Comments: string;
    DepartmentIDs: Array<number>;
    DepartmentID: number;
    RoleMasterID: number;
    RoleName: string;
    StatusID: number;
    DepartmentCode: string;
    StatusCode: string;
    ActedOn: string; 
}

export class AspectData {
    KRAAspectID: number;
    KRAAspectName: string;
    AspectName: string;
    CreatedDate: string;
    AspectId: number;
    IsMappedAspect: boolean = false;
    DepartmentId:number;   
}

export class KraAspectData extends AspectData {
    DepartmentId:number;    
    DepartmentName:string;
    KRAAspectID: number;
    KRAAspectName: string;
    lstAspectData:AspectData[];
    AspectId:number;
}

export class FinancialYear {
    Id: number;
    FinancialYearName : string;
}

export class SelectedKRAParameters {
    public FinancialYearName : string;
    public DepartmentName : string;
    public GradeName : string;
    public RoleTypeName : string;
}

export class OperatorData {
    OperatorId: number;
    OperatorValue: string;
}

export class TargetPeriodData {
    public TargetPeriodId: number;
    public TargetPeriodValue: string;
}
