import { GenericType } from 'src/app/models/dropdowntype.model';

export class KRADefinition
{
    KRAGroupId:number;
    KRAAspectId:number;
    KRAAspectName:string;
    StatusId:number;
    FinancialYearId:number;
    RoleName:string;    
    lstMetricAndTarget:MetricAndTarget[]=[]; 
}

export class MetricAndTarget {
    KRADefinitionId :number;
    KRAAspectId :number;
    Metric:string;
    KRAOperatorId:number;
    KRAMeasurementTypeId:number;
    KRAScaleMasterId?:number;
    KRAScaleDescription:string;
    KRATargetValue:number;
    KRATargetValueAsDate?:string;
    KRATargetText? :string;
    KRATargetPeriodId :number;
    Operator:string;
    KRAMeasurementType:string;
    ScaleLevel:string;
    KRATargetPeriod:string;
    TargetDescription:string;
}

export class KRAWorkFlowData {
    FinancialYearID :number;
    FromEmployeeID :number;
    DepartmentID :number;
    KRAGroupIDs:GenericType[]=[];
    Comments:string;
    StatusId:number;
}
export class KRASubmittedGroup {
    FinancialYearId :number;
    fromEmployeeId :number;   
    KRAGroupIds:number[]=[];
    RoleName:string;
}