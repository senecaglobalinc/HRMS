ALTER TABLE public."AssociateExitActivity"
ALTER COLUMN "AssociateExitId" DROP NOT NULL;

ALTER TABLE public."AssociateExitActivity"
ADD COLUMN "AssociateAbscondId" integer;

ALTER TABLE public."AssociateExitActivity" 
ADD CONSTRAINT "FK_AssociateExitActivity_AssociateAbscond_AssociateAbscondId"
FOREIGN KEY ("AssociateAbscondId") REFERENCES public."AssociateAbscond" ("AssociateAbscondId") MATCH SIMPLE
ON UPDATE NO ACTION
ON DELETE CASCADE;