import { SkillGroup } from "./associate-skills.model";
import { CompetencyArea } from "../../admin/models/competencyarea.model";

export class SkillData{
    SkillId:number;
    SkillCode:string;
    SkillDescription:string;
    SkillGroupId:number;
    SkillGroup: SkillGroup;
    Version : string;
    CompetencyArea : CompetencyArea;
    SkillGroupName: string;
    CompetencyAreaId:number;
    CompetencyAreaCode:string;
}

export class AddSkills{
    SkillId:number;
    SkillCode:string;
    SkillDescription:string;
    SkillGroupId:number;
    SkillGroup: SkillGroup;
    CompetencyArea : CompetencyArea;
    SkillGroupName: string;
    CompetencyAreaId:number;
    CompetencyAreaCode:string;
    ProficiencyLevel:string;
    Experience:number;
    LastUsed:string;
    IsPrimary:boolean;
    Id: number;
    IsAlreadyExist:boolean;
}

export class SkillHistory{
    Experience:number;
    LastUsed:string;
    SubmittedRating:string;
    RMRating:string;
    Status:string;
    Remarks:string;
    RequestedDate:Date;
    ApprovedDate:Date;
}