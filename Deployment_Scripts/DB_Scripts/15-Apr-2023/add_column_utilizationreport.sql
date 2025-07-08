-- Add new column LastBillingDate to UtilizationReport
ALTER TABLE public."UtilizationReport" ADD COLUMN "LastBillingDate" timestamp with time zone;