--Script for removing the (HRAId) columns from AssociateExit table

--DB: Employee

--Table :AssociateExit

--Column :HRAId

Alter TABLE public."AssociateExit" DROP COLUMN "HRAId"