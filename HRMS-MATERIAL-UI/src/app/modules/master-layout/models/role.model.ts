import { GenericType } from './dropdowntype.model';
import { Department } from '../../../modules/admin/models/department.model';

// import { GenericType } from '../models/index'

export class DepartmentDetails {
  DepartmentId: number;
  DepartmentCode?: string;
  Description?: string;
}

export class RoleData extends DepartmentDetails {
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
}
export class RoleSufixPrefix {
  Roles?: SGRole[];
  Prefix?: SGRolePrefix[];
  Suffix?: SGRoleSuffix[];
}

export class SGRole {
  SGRoleID: number;
  SGRoleName: string;
}
export class SGRolePrefix {
  PrefixID: number;
  PrefixName: string;
}
export class SGRoleSuffix {
  SuffixID: number;
  SuffixName: string;
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

//  DeltedSkillIds:
