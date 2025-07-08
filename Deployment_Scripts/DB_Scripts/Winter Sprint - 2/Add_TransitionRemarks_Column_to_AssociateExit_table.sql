--Add Column
  --DB: Employee
   --Table AssociateExit
     --Column TransitionRemarks

Alter table "AssociateExit" add column "TransitionRemarks" Character varying(1000);

--Alter Column
 --DB: Employee
  --Table AssociateExit
   --Column TransitionRequired
Alter table "AssociateExit" alter column "TransitionRequired" SET DEFAULT 'true';