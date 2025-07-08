
export class AssociateAllocation {
    AssociateAllocationId: number;
    TalentRequisitionId: number;
    DepartmentName: string;
    DepartmentId: number
    ProjectType: string;
    ProjectTypeId:number
    ProjectName: string;
    Status: string;
    NoOfPositions: number;
    EmployeeId: number;
    ProjectId: number;
    RoleMasterID: number;
    ReportingManagerId: number;
    RequisitionRoleDetailID: number;
    InternalBillingRoleId: number;
    ClientBillingRoleId: number;
    ClientBillingRoleName : string;
    StartDate : string;
    RoleName: string;
    Project: string;
    RoleRequisitionID: number;
    TotalNoOfPosition: number;
    EmployeeNameWithGrade: string;
    ResourceAvailability: number;
    AllocationPercentage: number;
    InternalBillingPercentage: number
    ClientBillingPercentage: number;
    isCritical: boolean = false;
    IsPrimary: boolean = false;
    Billable: boolean = false;
    EffectiveDate: string;
    NewEffectiveDate: string;
    MakePrimaryProjectId : number;
    AssociateName: string;
    Availability: number;
    NotifyAll: boolean = false;
    RoleMasterId : number;
    IsBillable : boolean = false;
    IsCritical : boolean = false;

}
// export class AssociateAllocation1 {
//     TalentRequisitionId: number;
//     EmployeeId: number;
//     ProjectId: number;
//     RoleId: number;
//     ReportingManagerId: number;
//     InternalBillingRoleId: number;
//     ClientBillingRoleId: number;
//     AllocationPercentage: number;
//     InternalBillingPercentage: number
//     ClientBillingPercentage: number;
//     //BillablePercentage: number;
//     isCritical: boolean;
//     IsPrimary: boolean;
//     Billable: boolean
//     EffectiveDate: string;
// }

export class ClientBillingRole {
    ClientBillingRoleId: number;
    ClientBillingRoleCode: string;
    ClientBillingRoleName: string;
    ClientId: number;
    ClientName:string; 
    StartDate: string;
    EndDate: string;
    NoOfPositions: number;
    AllocatedPositions: number;
    AvailablePositions: number;
    AllocationCount: number;
    ClientBillingPercentage : number;
    IsActive : boolean;
}
export class InternalBillingRole {
    InternalBillingRoleId: number;
    InternalBillingRoleCode: string;
    InternalBillingRoleName:string;
    IsActive:boolean;
}

export class ReportingManager {
    ReportingManagerId: number;
    ReportingManagerName: string;
}

export class RoleDetails {
    RoleMasterId : number;
    RoleName: string;
    NoOfBillablePositions: string;
    NoOfNonBillablePositions: string;
}

export class PercentageDropDown {
    AllocationPercentageId: number;
    Percentage: number;
}