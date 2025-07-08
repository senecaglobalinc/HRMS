export class ReportsSearchFilter {
    public ProjectId: number;
    public AssociateId: number;
    public ExperienceId: number;
    public GradeId: number;
    public SkillId: number;
    public DesignationId: number;
    public CompetencyId: number;
    public ClientId: number;
    public ManagerId: number;
    public UtilizationId: number;
    public IsBillable: boolean;
    public IsCritical: boolean;
    public Utilization: number;
    public Experience: string;
}
export class ResourceReport {
    public IsBillable: boolean;
    public IsCritical: boolean;
    public Utilization: number;
    public Experience: string;
    public AssociateName: string;
    public ProjectName: string;
    public ProgramManager: string;
    public AssociateCode: string;
    public SkillCode: string;
}