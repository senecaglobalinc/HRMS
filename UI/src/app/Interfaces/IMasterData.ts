import { Observable } from "rxjs";
import { RequisitionRoleSkills } from "../models/talentrequisitiondata.model";
import { DepartmentDetails } from "../models/role.model";
import { ProjectDetails } from "../models/projects.model";
import { PracticeArea } from "../models/associate-skills.model";
import { Grade } from "../components/admin/models/grade.model";
import { Client } from "../components/admin/models/client.model";
import { Designation } from "../components/admin/models/designation.model";
import { PercentageDropDown } from "../components/talentmanagement/models/associateallocation.model";
import { GenericType } from "../models/dropdowntype.model";

export interface IMasterData {
  GetDepartments(): Observable<DepartmentDetails[]>;
  GetRoles(): Observable<any[]>;
  GetAllAssociateList(): Observable<GenericType[]>;
  GetRolesByDepartmentId(departmentId: number): Observable<Array<GenericType>>
  GetRolesByProjectId(ProjectId: number): any
  GetCompetencyAreas(): any
  GetSkillGroupsByCompetenctArea(competencyAreaID: number): any
  GetSkillsBySkillGroups(SkillGroupID: number): any
  GetProficiencyLevels(): any
  GetFinancialYears(): Observable<Array<GenericType>>
  GetEmailIDsByString(suggestionString: string): Observable<string[]>
  GetMasterSkillList(): Observable<RequisitionRoleSkills[]>
  GetProjectsList(): Observable<ProjectDetails[]>
  GetPractiseAreas(): Observable<PracticeArea[]>
  GetGradesDetails(): Observable<Grade[]>
  GetClientList(): Observable<Client[]>
  GetDesignationList(): Observable<Designation[]>
  GetAllocationPercentages(): Observable<PercentageDropDown[]>
  GetProgramManagers(): Observable<GenericType[]>
}
