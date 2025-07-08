-- Table: public.UtilizationReport
DROP TABLE public."UtilizationReport";

CREATE TABLE public."UtilizationReport"
(
    "AssociateCode" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "AssociateName" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "DateOfJoining" timestamp with time zone NOT NULL,
    "EmploymentStartDate" timestamp with time zone,
    "LastWorkingDate" timestamp with time zone,
    "ProjectsWorked" character varying(1000) COLLATE pg_catalog."default",
    "TimeTakenForBillable" integer NOT NULL,
    "TotalWorkedDays" integer NOT NULL,
    "TotalBillingDays" integer NOT NULL,
    "TotalNonBillingDays" integer NOT NULL,
    "BillingDaysPercentage" numeric(18,2) NOT NULL,
    "ExperienceExcludingCareerBreak" numeric(18,2) NOT NULL,
    "CompetencyGroup" character varying(150),
    "Active" boolean
);

INSERT INTO public."MenuMaster"(
	"MenuId", "Title", "IsActive", "Path", "DisplayOrder", "ParentId", "CreatedBy", "CreatedDate", "SystemInfo", "Parameter", "NodeId", "Style")
	VALUES (91,  'Utilization Report',true, '/reports/utilizationreport', 10, 3, 'sa', NOW(), null, null, null, null);