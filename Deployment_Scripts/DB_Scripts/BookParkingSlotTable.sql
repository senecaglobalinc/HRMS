-- Table: public.BookedParkingSlots

-- DROP TABLE IF EXISTS public."BookedParkingSlots";

CREATE TABLE IF NOT EXISTS public."BookedParkingSlots"
(
    "ID" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "EmailID" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "IsActive" boolean DEFAULT true,
    "BookedDate" character varying COLLATE pg_catalog."default" NOT NULL,
    "BookedTime" character varying COLLATE pg_catalog."default" NOT NULL,
    "ReleaseDate" character varying COLLATE pg_catalog."default",
    "ReleaseTime" character varying COLLATE pg_catalog."default",
    CONSTRAINT "BookedParkingSlots_pkey" PRIMARY KEY ("ID"),
    CONSTRAINT "IX_Unique_email_date" UNIQUE ("EmailID", "BookedDate")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."BookedParkingSlots"
    OWNER to hrmsdev;