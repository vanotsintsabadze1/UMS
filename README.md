### Notice
The project uses **automatic migration**, therefore you don't need to migrate manually - It only works in **development** mode only.

The connection string in the `appsettings.json` has database options already written and Entity Framework will automatically create a database with those credentials.

**However**, If you do require a script for migration or a terminal command, they're written down as well.

---

1. [Technologies Used](#technologies-used)  
2. [NuGet Packages](#nuget-packages)  
3. [Patterns](#patterns)  
4. [Entity Relationship Diagram (ERD)](#entity-relationship-diagram-erd)  
5. [Layer Dependency Graph](#layer-dependency-graph)  
6. [Migration Scripts](#migration-scripts)
7. [Release Flow](#release-flow)

---

## Technologies Used
- ASP.NET 8 Web API
- Microsoft SQL Server
- Entity Framework Core

## NuGet Packages
- MediatR
- FluentValidation
- Mapster
- Serilog
- Asp.Versioning
- Microsoft.Extensions.Localization

## Patterns
- Mediator Pattern (Complements file-level CQRS)
- Repository Pattern
- Unit of Work Pattern
- Result Pattern

## Entity Relationship Diagram (ERD)
![ERD](https://i.imgur.com/bcTw3iC.png)

## Layer Dependency Graph
![LDG](https://i.imgur.com/4zmh7TF.png)

## Migration Scripts
SQL Script:
```sql
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Cities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Firstname] nvarchar(50) NOT NULL,
    [Lastname] nvarchar(50) NOT NULL,
    [Gender] nvarchar(max) NOT NULL,
    [SocialNumber] nvarchar(11) NOT NULL,
    [DateOfBirth] date NOT NULL,
    [CityId] int NOT NULL,
    [ImageUri] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_PhoneNumber_Length] CHECK (LEN(SocialNumber) = 11),
    CONSTRAINT [CHK_User_Age_18] CHECK (DATEDIFF(YEAR, DateOfBirth, GETDATE()) >= 18),
    CONSTRAINT [CHK_User_Name] CHECK ((Firstname NOT LIKE '%[^a-zA-Z]%' AND Lastname NOT LIKE '%[^a-zA-Z]%')),
    CONSTRAINT [FK_Users_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PhoneNumbers] (
    [UserId] int NOT NULL,
    [Number] nvarchar(50) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PhoneNumbers] PRIMARY KEY ([UserId], [Number]),
    CONSTRAINT [FK_PhoneNumbers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserRelationships] (
    [UserId] int NOT NULL,
    [RelatedUserId] int NOT NULL,
    [RelationshipType] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserRelationships] PRIMARY KEY ([UserId], [RelatedUserId]),
    CONSTRAINT [FK_UserRelationships_Users_RelatedUserId] FOREIGN KEY ([RelatedUserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_UserRelationships_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE INDEX [IX_PhoneNumbers_Number] ON [PhoneNumbers] ([Number]);
GO

CREATE INDEX [IX_UserRelationships_RelatedUserId] ON [UserRelationships] ([RelatedUserId]);
GO

CREATE UNIQUE INDEX [IX_Users_CityId] ON [Users] ([CityId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250312193648_Initial', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250313144423_RenamePhoneNumberToPlural', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ImageUri');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] ALTER COLUMN [ImageUri] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250313230946_MakeImageUriNullableForUsers', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_Users_CityId] ON [Users];
GO

CREATE INDEX [IX_Users_CityId] ON [Users] ([CityId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250314000848_FixRelationshipBetweenUsersAndCities', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250315163609_AddRelatedUsersAndRelatedByUsersNavigationProperties', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250315203238_MakeUserRelationshipNavigationPropertiesNotRequired', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Cities]'))
    SET IDENTITY_INSERT [Cities] ON;
INSERT INTO [Cities] ([Id], [Name])
VALUES (1, N'Tbilisi'),
(2, N'Batumi'),
(3, N'Kutaisi');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Cities]'))
    SET IDENTITY_INSERT [Cities] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250316170042_SeedCities', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE INDEX [IX_Users_Firstname_Lastname] ON [Users] ([Firstname], [Lastname]);
GO

CREATE UNIQUE INDEX [IX_Users_SocialNumber] ON [Users] ([SocialNumber]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250316195446_IndexFirstnameLastnameAndSocialNumber', N'8.0.0');
GO

COMMIT;
GO
```

**OR**
`dotnet ef database update --project UMS.API`

**OR**
`Update-Database` if you have a package manager console.

### Release Flow
During the process of working on this application, GitFlow technique was used to facilitate branches in a managed manner. (That's why there's so many commits as well, since I also focused on micro-commits)
