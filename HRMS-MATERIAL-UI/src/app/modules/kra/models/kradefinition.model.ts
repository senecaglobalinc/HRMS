//import { GenericType } from 'src/app/models/dropdowntype.model';

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
    //KRAGroupIDs:GenericType[]=[]; commented on 201120
    KRAGroupIDs:  Array<number>  
    Comments:string;
    StatusId:number;
}
export class KRASubmittedGroup {
    FinancialYearId :number;
    fromEmployeeId :number;   
    KRAGroupIds:number[]=[];
    RoleName:string;
}

export class DefinitionModel {
    definitionId : string;
    financialYearId:number;
    // gradeRoleTypeId : number;
    roleTypeId:number;
    scaleId : number;
    aspectId: number;
    metric: string;
    operatorId: number;
    measurementTypeId: number;
    targetValue: string;
    targetPeriodId: number;
    isActive: boolean;
    currentUser:string;   
    createdBy:string;
    modifiedBy:string; 
    definitionTransactionId:number;
}
