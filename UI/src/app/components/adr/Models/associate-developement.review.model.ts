export class ADRCycle {
    ADRCycleID?: number;
    ADRCycle?: string;
    IsActive?: boolean;
    checked?: boolean;
}
export class Appreciation extends ADRCycle {
    FinancialYearID?:number;
    FinancialYear?:string
    AppreciationDate?: string;
    ToEmployeeName?: string;
    ToEmployeeID?:number
    AssociateNames?: AppreciateDropDownType[];
    AppreciationTypeID?: number;
    AppreciationType?: string;
    SourceOfOriginId?: number;
    SourceOfOriginName?: string;
    AppreciationMessage?: string;
    FromEmployeeID?: number;
    FromEmployeeName?: string;
    ID?: number;
    Name : string;
}
 export class AppreciateDropDownType {
     Id?:number;
     Name?:string;
 }
