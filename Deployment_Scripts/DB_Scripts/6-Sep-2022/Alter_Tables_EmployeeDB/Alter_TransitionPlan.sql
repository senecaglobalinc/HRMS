ALTER TABLE public."TransitionPlan"
ALTER COLUMN "TransitionFrom" TYPE integer USING "TransitionFrom"::integer;

ALTER TABLE public."TransitionPlan"
ALTER COLUMN "TransitionTo" TYPE integer USING "TransitionTo"::integer;
