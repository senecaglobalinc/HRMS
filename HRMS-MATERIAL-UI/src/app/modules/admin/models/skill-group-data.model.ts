import { CompetencyAreaData } from "./competency-area-data.model";

export class SkillGroupData{
    SkillGroupId:number;
    SkillGroupName:string;
    Description:string;
    CompetencyArea :CompetencyAreaData;
    CompetencyAreaId:number;
    CompetencyAreaCode:string;
}
