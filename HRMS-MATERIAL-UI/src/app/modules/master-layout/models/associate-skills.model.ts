export interface AssociateSkill {
    // RoleId?: any;
    CompetencyAreaID?: any;
    PracticeAreaID?: any;
    skills: any[];
    skillList: any[];
    skillsVisible: boolean;
  }
  
  export interface CompetencyArea {
    CompetencyAreaId?: number;
    CompetencyAreaCode: string;
    CompetencyAreaDescription: string;
    IsActive?: boolean;
  }
  
  export interface PracticeArea {
    PracticeAreaId?: number;
    PracticeAreaCode: string;
    PracticeAreaDescription: string;
    // IsActive?: boolean;
  }
  
  export interface ProficiencyLevel {
    ProficiencyLevelId?: number;
    ProficiencyLevelCode: string;
    ProficiencyLevelDescription: string;
    IsActive?: boolean;
  }
  
  export class Role {
    RoleId: number;
    RoleName: string;
  }
  
  export class Skill {
    Id?: number;
    EmployeeId?: number;
    skillId?: number;
    SkillName: string;
    ProficiencyLevelId?: number;
    Experience?: number;
    LastUsed?: number;
    IsPrimary?: boolean;
    CompetencyAreaId?: number;
    SkillGroupId?: number;
    SkillId?: number;
    RoleId : number;
    StatusCode : string;
  }
  
  export class EmployeeSkillDetails{
    skillDetails : Array<Skill>;
    Id?: number;
    EmployeeId?: number;
    RoleId : number;
  }
  export class SkillGroup {
    SkillGroupId?: number;
    SkillGroupName: string;
    Description: string;
  }
  