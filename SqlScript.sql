CREATE DATABASE bankdb
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en-US'
    LC_CTYPE = 'en-US'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

---------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS public."Accounts"
(
    "Id" uuid NOT NULL,
    "AccountNumber" text COLLATE pg_catalog."default" NOT NULL,
    "OwnerName" text COLLATE pg_catalog."default" NOT NULL,
    "Balance" numeric NOT NULL,
    CONSTRAINT "PK_BankAccounts" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Accounts"
    OWNER to postgres;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_BankAccounts_AccountNumber"
    ON public."Accounts" USING btree
    ("AccountNumber" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;

---------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS public."Transactions"
(
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "Amount" numeric NOT NULL,
    "Type" text COLLATE pg_catalog."default" NOT NULL,
    "Note" text COLLATE pg_catalog."default",
    "RelatedAccountId" uuid,
    CONSTRAINT "PK_Transactions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Transactions_BankAccounts_AccountId" FOREIGN KEY ("AccountId")
        REFERENCES public."Accounts" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Transactions"
    OWNER to postgres;