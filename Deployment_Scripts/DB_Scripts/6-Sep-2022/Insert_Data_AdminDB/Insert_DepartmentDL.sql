INSERT INTO public."DepartmentDL"("DepartmentId", "DLEmailAdress")
SELECT "DepartmentId",'india.admin@senecaglobal.com' FROM public."Department" WHERE "DepartmentCode"='Admin';

INSERT INTO public."DepartmentDL"("DepartmentId", "DLEmailAdress")
SELECT "DepartmentId",'india.it@senecaglobal.com' FROM public."Department" WHERE "DepartmentCode"='IT';
	
INSERT INTO public."DepartmentDL"("DepartmentId", "DLEmailAdress")
SELECT "DepartmentId",'india.finance@senecaglobal.com' FROM public."Department" WHERE "DepartmentCode"='FD';
	
INSERT INTO public."DepartmentDL"("DepartmentId", "DLEmailAdress")
SELECT "DepartmentId",'india.hr@senecaglobal.com' FROM public."Department" WHERE "DepartmentCode"='HR';	

INSERT INTO public."DepartmentDL"("DepartmentId", "DLEmailAdress")
SELECT "DepartmentId",'india.training@senecaglobal.com' FROM public."Department" WHERE "DepartmentCode"='Training Department';
