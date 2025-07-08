import { GenericType } from "../../master-layout/models/dropdowntype.model";

export class ActivityList {
    ActivityId: number;
    Description: string;
    value: boolean;
    ActivityRemarks: string;
    Department: string;
    DepartmentId: number;
    ActivityType: string;
}

export class ActivityData {
    ProjectId: number;
    DepartmentId: number;
    Remarks: string;
    type: string;
    ActivityDetails: ActivityDetails[];
}

export class ActivityDetails{
    ActivityId: number;
    Value: string;
    value_bool: boolean;
    Remarks: string;
    createdBy: string;
    modifiedBy: string;
}