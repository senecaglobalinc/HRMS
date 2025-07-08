import { Grade } from "./grade.model";


export class Designation {
    DesignationId:number;
    DesignationCode:string;
    DesignationName:string;
    Grade : Grade;
    IsActive:boolean;
 
}

// designation page uses all designation, grade, AppreciateDropDownType classes fields
  
export class DesignationData {
    DesignationId:number;
    DesignationCode:string;
    DesignationName:string;
    IsActive:boolean;
    Grade:Grade;
    ID?:number;
    Name?:string;
    GradeCode: string;
    GradeId: number;
    GradeName: string
    }


