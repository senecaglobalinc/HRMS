-- Table: public.TrainingMode

CREATE TABLE IF NOT EXISTS public."TrainingMode"
(
    "TrainingModeId" integer NOT NULL,
    "TrainingModeCode" character varying(256) COLLATE pg_catalog."default",
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "TrainingMode_pkey" PRIMARY KEY ("TrainingModeId")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."TrainingMode"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.TrainingMode_TrainingModeId_seq

CREATE SEQUENCE IF NOT EXISTS public."TrainingMode_TrainingModeId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "TrainingMode"."TrainingModeId";

ALTER SEQUENCE public."TrainingMode_TrainingModeId_seq"
    OWNER TO hrmsdev;	

ALTER TABLE ONLY public."TrainingMode"
    ALTER COLUMN "TrainingModeId" SET DEFAULT nextval('"TrainingMode_TrainingModeId_seq"'::regclass);


-- Table: public.WorkplaceBehaviorRating

CREATE TABLE IF NOT EXISTS public."WorkplaceBehaviorRating"
(
    "WorkplaceBehaviorRatingId" integer NOT NULL,
    "WorkplaceBehaviorRatingCode" character varying(256) COLLATE pg_catalog."default",
	"WorkplaceBehaviorRatingCodeDescription" character varying(256) COLLATE pg_catalog."default",
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "WorkplaceBehaviorRating_pkey" PRIMARY KEY ("WorkplaceBehaviorRatingId")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."WorkplaceBehaviorRating"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.WorkplaceBehaviorRating_WorkplaceBehaviorRatingId_seq

CREATE SEQUENCE IF NOT EXISTS public."WorkplaceBehaviorRating_WorkplaceBehaviorRatingId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "WorkplaceBehaviorRating"."WorkplaceBehaviorRatingId";

ALTER SEQUENCE public."WorkplaceBehaviorRating_WorkplaceBehaviorRatingId_seq"
    OWNER TO hrmsdev;

ALTER TABLE ONLY public."WorkplaceBehaviorRating"
    ALTER COLUMN "WorkplaceBehaviorRatingId" SET DEFAULT nextval('"WorkplaceBehaviorRating_WorkplaceBehaviorRatingId_seq"'::regclass);


-- Table: public.ProjectRole

CREATE TABLE IF NOT EXISTS public."ProjectRole"
(
    "ProjectRoleId" integer NOT NULL,
    "ProjectId" integer,
	"RoleId" integer,
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "ProjectRole_pkey" PRIMARY KEY ("ProjectRoleId"),
    CONSTRAINT "ProjectRole_ProjectId_fkey" FOREIGN KEY ("ProjectId")
        REFERENCES public."Projects" ("ProjectId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."ProjectRole"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.ProjectRole_ProjectRoleId_seq

CREATE SEQUENCE IF NOT EXISTS public."ProjectRole_ProjectRoleId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "ProjectRole"."ProjectRoleId";

ALTER SEQUENCE public."ProjectRole_ProjectRoleId_seq"
    OWNER TO hrmsdev;	
	
ALTER TABLE ONLY public."ProjectRole"
    ALTER COLUMN "ProjectRoleId" SET DEFAULT nextval('"ProjectRole_ProjectRoleId_seq"'::regclass);


-- Table: public.ProjectSkillsRequired

CREATE TABLE IF NOT EXISTS public."ProjectSkillsRequired"
(
    "ProjectSkillsRequiredId" integer NOT NULL,
    "ProjectRoleId" integer,
	"SkillId" integer,
	"ProficiencyId" integer,
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "ProjectSkillsRequired_pkey" PRIMARY KEY ("ProjectSkillsRequiredId"),
	CONSTRAINT "ProjectSkillsRequired_ProjectRoleId_fkey" FOREIGN KEY ("ProjectRoleId")
        REFERENCES public."ProjectRole" ("ProjectRoleId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."ProjectSkillsRequired"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.ProjectSkillsRequired_ProjectSkillsRequiredId_seq

CREATE SEQUENCE IF NOT EXISTS public."ProjectSkillsRequired_ProjectSkillsRequiredId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "ProjectSkillsRequired"."ProjectSkillsRequiredId";

ALTER SEQUENCE public."ProjectSkillsRequired_ProjectSkillsRequiredId_seq"
    OWNER TO hrmsdev;
	
ALTER TABLE ONLY public."ProjectSkillsRequired"
    ALTER COLUMN "ProjectSkillsRequiredId" SET DEFAULT nextval('"ProjectSkillsRequired_ProjectSkillsRequiredId_seq"'::regclass);


-- Table: public.ProjectRoleAssociateMapping

CREATE TABLE IF NOT EXISTS public."ProjectRoleAssociateMapping"
(
    "ProjectRoleAssociateMappingId" integer NOT NULL,
	"ProjectId" integer,
    "ProjectRoleId" integer,	
	"AssociateId" integer,
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "ProjectRoleAssociateMapping_pkey" PRIMARY KEY ("ProjectRoleAssociateMappingId"),
	CONSTRAINT "ProjectRoleAssociateMapping_ProjectId_fkey" FOREIGN KEY ("ProjectId")
        REFERENCES public."Projects" ("ProjectId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "ProjectRoleAssociateMapping_ProjectRoleId_fkey" FOREIGN KEY ("ProjectRoleId")
        REFERENCES public."ProjectRole" ("ProjectRoleId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."ProjectRoleAssociateMapping"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.ProjectRoleAssociateMapping_ProjectRoleAssociateMappingId_seq

CREATE SEQUENCE IF NOT EXISTS public."ProjectRoleAssociateMapping_ProjectRoleAssociateMappingId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "ProjectRoleAssociateMapping"."ProjectRoleAssociateMappingId";

ALTER SEQUENCE public."ProjectRoleAssociateMapping_ProjectRoleAssociateMappingId_seq"
    OWNER TO hrmsdev;
	
ALTER TABLE ONLY public."ProjectRoleAssociateMapping"
    ALTER COLUMN "ProjectRoleAssociateMappingId" SET DEFAULT nextval('"ProjectRoleAssociateMapping_ProjectRoleAssociateMappingId_seq"'::regclass);


-- Table: public.ProjectTrainingPlan

CREATE TABLE IF NOT EXISTS public."ProjectTrainingPlan"
(
    "ProjectTrainingPlanId" integer NOT NULL,
	"ProjectId" integer,
    "AssociateId" integer,
	"SkillId" integer,
	"ProjectTrainingPlanned" character varying(256) COLLATE pg_catalog."default",
	"TrainingFromDate" timestamp(6) without time zone,
	"TrainingToDate" timestamp(6) without time zone,
	"FinancialYearId" integer,
	"CycleId" integer,
	"TrainingModeId" integer,
	"IsTrainingCompleted" boolean,
	"SkillAssessedBy" integer,
	"SkillAssessmentDate" timestamp(6) without time zone,
	"SkillApplied" boolean,
	"ProficiencyLevelAchieved" integer,
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "ProjectTrainingPlan_pkey" PRIMARY KEY ("ProjectTrainingPlanId"),
	CONSTRAINT "ProjectTrainingPlan_TrainingModeId_fkey" FOREIGN KEY ("TrainingModeId")
        REFERENCES public."TrainingMode" ("TrainingModeId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "ProjectTrainingPlan_ProjectId_fkey" FOREIGN KEY ("ProjectId")
        REFERENCES public."Projects" ("ProjectId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."ProjectTrainingPlan"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.ProjectTrainingPlan_ProjectTrainingPlanId_seq

CREATE SEQUENCE IF NOT EXISTS public."ProjectTrainingPlan_ProjectTrainingPlanId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "ProjectTrainingPlan"."ProjectTrainingPlanId";

ALTER SEQUENCE public."ProjectTrainingPlan_ProjectTrainingPlanId_seq"
    OWNER TO hrmsdev;

ALTER TABLE ONLY public."ProjectTrainingPlan"
    ALTER COLUMN "ProjectTrainingPlanId" SET DEFAULT nextval('"ProjectTrainingPlan_ProjectTrainingPlanId_seq"'::regclass);


-- Table: public.WorkplaceBehavior

CREATE TABLE IF NOT EXISTS public."WorkplaceBehavior"
(
    "WorkplaceBehaviorId" integer NOT NULL,
    "ProjectId" integer,
	"WorkplaceBehaviorOrgValueName" character varying(256) COLLATE pg_catalog."default",
	"WorkplaceBehaviorRequirementDescription" character varying(256) COLLATE pg_catalog."default",
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "WorkplaceBehavior_pkey" PRIMARY KEY ("WorkplaceBehaviorId")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."WorkplaceBehavior"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.WorkplaceBehavior_WorkplaceBehaviorId_seq

CREATE SEQUENCE IF NOT EXISTS public."WorkplaceBehavior_WorkplaceBehaviorId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "WorkplaceBehavior"."WorkplaceBehaviorId";

ALTER SEQUENCE public."WorkplaceBehavior_WorkplaceBehaviorId_seq"
    OWNER TO hrmsdev;
	
ALTER TABLE ONLY public."WorkplaceBehavior"
    ALTER COLUMN "WorkplaceBehaviorId" SET DEFAULT nextval('"WorkplaceBehavior_WorkplaceBehaviorId_seq"'::regclass);


-- Table: public.WorkplaceBehaviorAssessment

CREATE TABLE IF NOT EXISTS public."WorkplaceBehaviorAssessment"
(
    "WorkplaceBehaviorAssessmentId" integer NOT NULL,
    "AssociateId" integer,
	"WorkplaceBehaviorId" integer,
	"WorkplaceBehaviorRatingId" integer,
	"IsActive" boolean,
    "CreatedBy" character varying(100) COLLATE pg_catalog."default",
    "CreatedDate" timestamp(6) without time zone,
    "ModifiedBy" character varying(100) COLLATE pg_catalog."default",
    "ModifiedDate" timestamp(6) without time zone,
    "SystemInfo" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "WorkplaceBehaviorAssessment_pkey" PRIMARY KEY ("WorkplaceBehaviorAssessmentId"),
	CONSTRAINT "WorkplaceBehaviorAssessment_WorkplaceBehaviorId_fkey" FOREIGN KEY ("WorkplaceBehaviorId")
        REFERENCES public."WorkplaceBehavior" ("WorkplaceBehaviorId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
	CONSTRAINT "WorkplaceBehaviorAssessment_WorkplaceBehaviorRatingId_fkey" FOREIGN KEY ("WorkplaceBehaviorRatingId")
        REFERENCES public."WorkplaceBehaviorRating" ("WorkplaceBehaviorRatingId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."WorkplaceBehaviorAssessment"
    OWNER to hrmsdev;
	
-- SEQUENCE: public.WorkplaceBehaviorAssessment_WorkplaceBehaviorAssessmentId_seq

CREATE SEQUENCE IF NOT EXISTS public."WorkplaceBehaviorAssessment_WorkplaceBehaviorAssessmentId_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1
    OWNED BY "WorkplaceBehaviorAssessment"."WorkplaceBehaviorAssessmentId";

ALTER SEQUENCE public."WorkplaceBehaviorAssessment_WorkplaceBehaviorAssessmentId_seq"
    OWNER TO hrmsdev;
	
ALTER TABLE ONLY public."WorkplaceBehaviorAssessment"
    ALTER COLUMN "WorkplaceBehaviorAssessmentId" SET DEFAULT nextval('"WorkplaceBehaviorAssessment_WorkplaceBehaviorAssessmentId_seq"'::regclass);
	

