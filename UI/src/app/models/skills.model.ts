import { SkillGroup } from "./associate-skills.model";
import { CompetencyArea } from "../components/admin/models/competencyarea.model";

export class SkillData{
    SkillId:number;
    SkillCode:string;
    SkillDescription:string;
    SkillGroupId:number;
    SkillGroup: SkillGroup;
    CompetencyArea : CompetencyArea;
    SkillGroupName: string;
    CompetencyAreaId:number;
    CompetencyAreaCode:string;
}