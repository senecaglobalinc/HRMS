import { GenericType } from "./dropdowntype.model";



export class TalentRequisitionHistoryData {
    IsRoleDetailsExpand?: boolean;
    Authorized: string;
    TalentRequisitionId: number;
    TRCode: any;
    DepartmentName: string;
    ProjectType: string;
    ProjectId: number;
    ProjectName: string;
    RequestedDate: string;
    RoleMasterID: number;
    RoleName: string;
    EmployeeId: number
    EmployeeCode: string
    EmployeeName: string
    Status: string;
    StatusId: number;
    RequestedBy: string;
    DepartmentCode: string;
    RequisitionType: number;
    TRSelect: boolean;
    NoOfPositions?: number;
    Remarks: string;
    ClientName: string;
    DepartmentId: number;
    RequisitionTypeName: string;
    TalentRequisitionHistoryRoleDetails: TalentRequisitionHistoryRoleDetails[];
}

export interface TalentRequisitionHistoryRoleDetails {
    RequisitionRoleDetailID: number;
    RoleId: number;
    RoleName: string;
    NoOfBillablePositions?: number;
    NoOfNonBillablePositions?: number;
    TAPositions?: number;
    TotalPositions?: number;
    TaggedEmployees: number;
    isTAPositionsAdded: boolean;
}

export class employeeDetails {
    EmployeeCode: string;
    EmployeeId: number;
    EmployeeName: string;
    ProjectName: string;
    Comments: string;
    IsPrimary: boolean;
    IsBillable: boolean;
    IsCritical: boolean;
}

export interface ApproveOrRejectTRData {
    Type: string;
    TalentRequisitionIds: number[];
    Remarks: string;
}

export class DeliveryHeadTalentRequisitionDetails {
    RequisitionType: number;
    TalentRequisitionId: number;
    ApprovedByID?: GenericType[];
    FromEmployeeID: number;
    Comments?: string;
}

