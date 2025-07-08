class ProfessionalDetail {
    id:number;
    employeeID:number;
    programType: number;
    institution: string;
    specialization: string;
    validFrom: number;
    validUpto: number;
}
class Certification extends ProfessionalDetail {
    skillGroupID: number;
    skillGroupName:string;
    SkillName:string;
    certificationID: number;
}
class MemberShip extends ProfessionalDetail {
    programTitle: string;
}
class Professional extends ProfessionalDetail {
    skillGroupID: number;
    certificationID: number;
    programTitle: string;
}

export { Certification, MemberShip, Professional }

