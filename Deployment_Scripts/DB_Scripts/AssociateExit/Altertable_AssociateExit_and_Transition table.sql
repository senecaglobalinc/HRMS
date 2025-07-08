--Add ProjectId,AssociateAllocationId columns to AssociateExit table
Alter table public."AssociateExit" Add columns "ProjectId" INTEGER NULL;
Alter table public."AssociateExit" Add columns "AssociateAllocationId" INTEGER NULL;

-- Delete ProjectId from Transition table
ALTER table "TransitionPlan" drop COLUMN "ProjectId"


