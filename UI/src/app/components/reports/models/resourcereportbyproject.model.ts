export class AllocationDetails {
    lstBillableResources: ResourceAllocationDetails[];
    lstNonBillableResources: ResourceAllocationDetails[];
    AllocationCount: AllocationCount;
}
export class AllocationCount {
    ProjectName: string
    ResourceCount: number
    BillableCount: number
    NonBillableCount: number
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
}

export class ProjectDetails {
    ProjectId: number;
    ProjectCode: string;
    ProjectName: string;
    ProjectType:number;
}

