export class AllocationDetails {
    lstBillableResources: ResourceAllocationDetails[];
    lstNonBillableCriticalResources: ResourceAllocationDetails[];
    lstNonBillableNonCriticalResources: ResourceAllocationDetails[];
    AllocationCount: AllocationCount;
}
export class AllocationCount {
    ProjectName: string
    ResourceCount: number
    BillableCount: number
    NonBillableCriticalCount: number
    NonBillableNonCriticalCount: number
}

export class ResourceAllocationDetails {
    AssociateCode: string
    AssociateName: string
    AllocationPercentage: number
    InternalBillingRoleName: string
    ClientBillingRoleName: string 
    ClientBillingRoleId:number
    IsPrimaryProject: string
    IsCriticalResource: string
    EffectiveDate: string;
}

export class ProjectDetails {
    ProjectId: number;
    ProjectCode: string;
    ProjectName: string;
    ProjectType:number;
}

