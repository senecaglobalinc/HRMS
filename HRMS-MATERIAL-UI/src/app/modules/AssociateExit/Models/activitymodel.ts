export class ActivityList {
    ActivityId: number;
    Description: string;
    value: boolean;
    ActivityRemarks: string;
    Department: string;
    DepartmentId: number;
    ActivityType: string;
    IsRequired: boolean;
}

export class ActivityData {
    // ProjectId: number;
    EmployeeId: number;
    DepartmentId: number;
    Remarks: string;
    type: string;
    ActivityDetails: ActivityDetails[];
}
export class ActivityDataByDepartment {
    // ProjectId: number;
    EmployeeId: number;
    DepartmentId: number;
    Remarks: string;
    type: string;
    StatusId: number;
    StatusCode: string;
    ActivityDetails: ActivityDetails[];
}
export class ActivityDetails{
    ActivityId: number;
    Value: string;
    ActivityValue:string;
    value_bool: boolean;
    Remarks: string;
    createdBy: string;
    modifiedBy: string;
    ActivityType: string;
    Description: string;
}