
export class EmployeeDetails {
    EmployeeId: number;
    EmpName: string;
    ReportingManagerId: number;
    ReportingManager: string;
  }

export class NonDeliveryAssociates {
    Id: number;
    Name: string;
    DepartmentId: number;
}

export class updateAssociateRMDetais extends EmployeeDetails {
    // reportingManagerId: number;
    associates: EmployeeDetails[];
}