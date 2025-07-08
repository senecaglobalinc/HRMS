export class KtForm {
        TransistionType: string;
        EmployeeId: number;
        Type: string;
        ProjectId: number;
        TransitionFrom: number;
        TransitionTo: number;
        StartDate: Date;
        EndDate:Date;
        KnowledgeTransferred: boolean;
        KnowledgeTransaferredRemarks:string;
        Others: string;
        Status: string;
        UpdateTransitionDetail: UpdateTransitionPlan[];
        TransitionNotRequired:boolean;
        Remarks:string;
        
}

export class KtFormWithSubStatus {
        TransistionType: string;
        EmployeeId: number;
        Type: string;
        ProjectId: number;
        TransitionFrom: number;
        TransitionTo: number;
        StartDate: Date;
        EndDate:Date;
        KnowledgeTransferred: boolean;
        KnowledgeTransaferredRemarks:string;
        Others: string;
        Status: string;
        UpdateTransitionDetail: UpdateTransitionPlan[];
        SubStatusCode: string;
        SubStatusDesc: string;
}

export class UpdateTransitionPlan{
        TransitionPlanDetailId: number;
        TransitionPlanId: number;
        ActivityId: number;
        StartDate: Date;
        EndDate: Date;
        Remarks: Date;
        Status: string;
        ActivityDescription: string;
}