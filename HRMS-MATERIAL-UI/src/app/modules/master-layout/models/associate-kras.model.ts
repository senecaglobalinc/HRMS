export class OrganizationKras {
    KRAAspectName: string;
    KRAAspectMetric: string;
    KRAAspectTarget: string;
    KRAMeasurementType: string;
    Operator: string;
    KRATargetValue: string;
    KRATargetPeriod: string;
}

export class CustomKras extends OrganizationKras {
    KRAAspectID: number;
    CustomKRAID: number;
    KRATypeID: number;
    FinancialYearID: number;
    EmployeeIDs: Array<number>;
}

export class AssociateKras {
    OrganizationKRAs: Array<OrganizationKras>;
    CustomKRAs: Array<CustomKras>;
} 