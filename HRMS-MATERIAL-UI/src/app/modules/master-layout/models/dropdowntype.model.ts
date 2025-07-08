export class DropDownType {
    label:string;
    value:number;
  }
  export class SkillGroupDropDown{
    SkillGroupId:number;
    SkillGroupName:string;
  }
  export class SkillDropDown{
    SkillId:number;
    SkillName:string;
  }
  
  export class GenericType{
    Id:number;
    Name:string;
  }
  
  export class GenericModel extends GenericType{
    Gender:string;
    DepartmentId:number;
  }
  
  