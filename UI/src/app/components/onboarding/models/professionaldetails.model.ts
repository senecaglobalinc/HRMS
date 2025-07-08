class ProfessionalDetail {
    Id:number;
    EmployeeId:number;
    ProgramType: number;
    Institution: string;
    Specialization: string;
    ValidFrom: number;
    ValidUpto: number;
}
class Certification extends ProfessionalDetail {
    SkillGroupId: number;
    SkillGroupName:string;
    SkillName:string;
    CertificationId: number;
}
class MemberShip extends ProfessionalDetail {
    ProgramTitle: string;
}
class Professional extends ProfessionalDetail {
    SkillGroupId: number;
    CertificationId: number;
    ProgramTitle: string;
}

export { Certification, MemberShip, Professional }
