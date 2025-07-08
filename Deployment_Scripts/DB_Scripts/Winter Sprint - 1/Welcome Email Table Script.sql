-- Table: public.WelcomeEmail

-- DROP TABLE IF EXISTS public."WelcomeEmail";

CREATE TABLE IF NOT EXISTS public."WelcomeEmail"
(
    "Id" uuid NOT NULL,
    "EmployeeId" integer NOT NULL,
    "IsWelcome" boolean,
    "IsActive" boolean,
    "CreatedBy" character varying COLLATE pg_catalog."default",
    "CreatedDate" timestamp with time zone,
    "ModifiedBy" character varying COLLATE pg_catalog."default",
    "ModifiedDate" timestamp with time zone,
    CONSTRAINT "WelcomeEmail_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

