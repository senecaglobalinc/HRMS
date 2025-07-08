import { SkillGroupData } from './skill-group-data.model';
import { CompetencyAreaData } from './competency-area-data.model';

export class SkillData {
    SkillId:number;
    SkillCode:string;
    SkillName:string;
    SkillDescription:string;
    SkillGroupId:number;
    SkillGroup: SkillGroupData;
    CompetencyArea : CompetencyAreaData;
    SkillGroupName: string;
    CompetencyAreaId:number;
    CompetencyAreaCode:string;
}

