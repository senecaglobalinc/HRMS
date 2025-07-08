-- Table: public.UtilizationReport

-- DROP TABLE public."AssociateExitReport";

CREATE TABLE public."AssociateExitReport"
(
    "AssociateId" integer NOT NULL,
    "AssociateExitId" integer NOT NULL,
    "AssociateCode" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "AssociateName" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "Grade" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "Gender" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "Department" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "TechnologyGroup" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "Project" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ProgramManager" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ReportingManager" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "FinancialYear" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "Quarter" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ExitType" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ExitCause" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "RehireEligibility" boolean,
    "LegalExit" boolean,
    "ImpactOnClientDelivery" boolean,
    "Remarks" character varying(5000) COLLATE pg_catalog."default",
    "ServiceTenure" numeric(18,2) NOT NULL,
    "ServiceTenureWithSG" numeric(18,2) NOT NULL,
    "ServiceTenurePriorToSG" numeric(18,2) NOT NULL,
    "JoinDate" timestamp with time zone NOT NULL,
    "ExitDate" timestamp with time zone NOT NULL,
    "ResignedDate" timestamp with time zone,
    "ServiceTenureRange" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ServiceTenureWithSGRange" character varying(150) COLLATE pg_catalog."default" NOT NULL
)

TABLESPACE pg_default;