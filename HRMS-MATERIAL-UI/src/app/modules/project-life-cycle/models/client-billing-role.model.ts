export class ClientBillingRoleDetails {
    ClientBillingRoleId: number;
    // ClientBillingRoleCode: string;
    ClientBillingRoleName: string;
    ProjectId: number;
    ProjectName: string;
    NoOfPositions: number;
    StartDate: string;
    EndDate: string;
    AllocatedPositions: number;
    AvailablePositions: number;
    ClientBillingPercentage : number;
    Percentage : number;
    AllocationCount : number;
    IsActive : boolean;
}
export class ClientBillingRole {
    ClientBillingRoleId: number;
    ClientBillingRoleCode: string;
    ClientBillingRoleName: string;
    ClientId: number;
    ClientName:string; 
   
}