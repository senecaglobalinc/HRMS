
UPDATE public."Status" SET "StatusDescription"='Resignation Reviewed' WHERE "StatusId" =42 and "CategoryMasterId"=2 and "IsActive"=true;
UPDATE public."Status" SET "StatusCode"='MarkedForAbscond', "StatusDescription"='Abscond Marked by TL' WHERE "StatusId" =33 and "CategoryMasterId"=2 and "IsActive"=true;
UPDATE public."Status" SET "StatusCode"='AbscondAcknowledged', "StatusDescription"='Abscond Acknowledged by HRA' WHERE "StatusId" =34 and "CategoryMasterId"=2 and "IsActive"=true;
INSERT INTO public."Status"
("Id","StatusId", "StatusCode", "StatusDescription", "IsActive", "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", "SystemInfo", "CategoryMasterId")
VALUES 
(57,43, 'AbscondConfirmed', 'Abscond Confirmed by HRM', true, 'Anonymous', null, NOW(), null, null, 2);
INSERT INTO public."Status"
("Id","StatusId", "StatusCode", "StatusDescription", "IsActive", "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", "SystemInfo", "CategoryMasterId")
VALUES 
(58,44, 'AbscondDisproved', 'Abscond Disproved by HRM', true, 'Anonymous', null, NOW(), null, null, 2);
INSERT INTO public."Status"
("Id","StatusId", "StatusCode", "StatusDescription", "IsActive", "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", "SystemInfo", "CategoryMasterId")
VALUES 
(59,45, 'Absconded', 'Associate Absconded', true, 'Anonymous', null, NOW(), null, null, 2);
INSERT INTO public."Status"
("Id","StatusId", "StatusCode", "StatusDescription", "IsActive", "CreatedBy", "ModifiedBy", "CreatedDate", "ModifiedDate", "SystemInfo", "CategoryMasterId")
VALUES 
(60,46, 'Blacklisted', 'Associate Blacklisted', true, 'Anonymous', null, NOW(), null, null, 2);