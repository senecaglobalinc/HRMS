export class KRAGroup
{
    KRAGroupId:number;
    DepartmentId:number;
    RoleCategoryId:number;
    ProjectTypeId?:number;
    FinancialYearId:number;
    KRATitle:string;    
    StatusId:number;
    Description:string;
    RoleCategoryName:string;
    ProjectTypeCode:string;
    StatusCode:string;
    StatusDescription:string
    KRACount:number; // To validate definitions for groups
}