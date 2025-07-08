import { Department } from "./department.model";

export class RoleData{
    ID?: number;
    SGRoleID: number;
    RoleMasterID?: number;
    KRAGroupID: number;
    KRATitle: string;
    RoleName: string;
    RoleDescription: string;
    KeyResponsibilities: string;
    EducationQualification: string;
    PrefixID: number;
    SuffixID: number;
    dateOfJoining?: Date;
    RoleCompetencySkills?: RoleCompetencySkills[];
    DeltedSkillIds?: any[];
    IsActive?: boolean;
    SGRole: SGRole;
    SGRolePrefix: SGRolePrefix;
    SGRoleSuffix: SGRoleSuffix;
    Department: Department;
    DepartmentId: number;
}

export class SGRole{
    SGRoleID:number;
    SGRoleName:string;
}
export class RoleCompetencySkills {
    ID?: number;
    CompetencySkillsId?: number;
    CompetencyAreaId?: number;
    SkillGroupId?: number;
    SkillGroupName?: string;
    SkillId?: number;
    RoleMasterID?: number;
    IsPrimary: boolean;
    ProficiencyLevelId?: number;
    CompetencyAreaCode?: string;
    SkillName?: string;
    ProficiencyLevelCode: string;
}
export class SGRolePrefix{
    PrefixID:number;
    PrefixName:string;
}
export class SGRoleSuffix{
    SuffixID:number;
    SuffixName:string;
}