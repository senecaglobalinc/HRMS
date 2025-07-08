ALTER TABLE "Definition"
RENAME COLUMN "GradeRoleTypeId" TO "RoleTypeId"

ALTER TABLE "DefinitionTransaction"
RENAME COLUMN "GradeRoleTypeId" TO "RoleTypeId"

ALTER TABLE "KRAWorkFlow"
RENAME COLUMN "GradeRoleTypeId" TO "RoleTypeId"