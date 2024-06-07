CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Books" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Books" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Author" TEXT NULL,
    "IsReserved" INTEGER NOT NULL,
    "ReservationComment" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL
);

CREATE TABLE "ReservationHistory" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ReservationHistory" PRIMARY KEY AUTOINCREMENT,
    "Comment" TEXT NOT NULL,
    "EventDate" TEXT NOT NULL,
    "Event" INTEGER NOT NULL,
    "BookId" INTEGER NOT NULL,
    CONSTRAINT "FK_ReservationHistory_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ReservationHistory_BookId" ON "ReservationHistory" ("BookId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240607082216_testmigrations', '8.0.0');

COMMIT;

