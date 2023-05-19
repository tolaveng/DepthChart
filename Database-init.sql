USE master
GO
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DataBase')
BEGIN
  CREATE DATABASE [ChartDepth_Db]


END
GO
	USE [ChartDepth_Db]
GO
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

CREATE TABLE [Players] (
    [Number] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [TeamId] int NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Number])
);
GO

CREATE TABLE [Positions] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Positions] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Sports] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Sports] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Teams] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [SportId] int NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Charts] (
    [Id] uniqueidentifier NOT NULL,
    [PlayerNumber] int NOT NULL,
    [Group] nvarchar(max) NULL,
    [PositionId] nvarchar(450) NULL,
    [Depth] int NOT NULL,
    CONSTRAINT [PK_Charts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Charts_Players_PlayerNumber] FOREIGN KEY ([PlayerNumber]) REFERENCES [Players] ([Number]) ON DELETE CASCADE,
    CONSTRAINT [FK_Charts_Positions_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [Positions] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Number', N'Name', N'TeamId') AND [object_id] = OBJECT_ID(N'[Players]'))
    SET IDENTITY_INSERT [Players] ON;
INSERT INTO [Players] ([Number], [Name], [TeamId])
VALUES (1, N'Test Player', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Number', N'Name', N'TeamId') AND [object_id] = OBJECT_ID(N'[Players]'))
    SET IDENTITY_INSERT [Players] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Positions]'))
    SET IDENTITY_INSERT [Positions] ON;
INSERT INTO [Positions] ([Id], [Name])
VALUES (N'OLB', N'OLB');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Positions]'))
    SET IDENTITY_INSERT [Positions] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Sports]'))
    SET IDENTITY_INSERT [Sports] ON;
INSERT INTO [Sports] ([Id], [Name])
VALUES (1, N'NFL');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Sports]'))
    SET IDENTITY_INSERT [Sports] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'SportId') AND [object_id] = OBJECT_ID(N'[Teams]'))
    SET IDENTITY_INSERT [Teams] ON;
INSERT INTO [Teams] ([Id], [Name], [SportId])
VALUES (1, N'Tampa Bay Buccaneers', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'SportId') AND [object_id] = OBJECT_ID(N'[Teams]'))
    SET IDENTITY_INSERT [Teams] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Depth', N'Group', N'PlayerNumber', N'PositionId') AND [object_id] = OBJECT_ID(N'[Charts]'))
    SET IDENTITY_INSERT [Charts] ON;
INSERT INTO [Charts] ([Id], [Depth], [Group], [PlayerNumber], [PositionId])
VALUES ('0323dab0-4be8-4db0-a28e-79f41138c3ea', 0, N'Offense', 1, N'OLB');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Depth', N'Group', N'PlayerNumber', N'PositionId') AND [object_id] = OBJECT_ID(N'[Charts]'))
    SET IDENTITY_INSERT [Charts] OFF;
GO

CREATE INDEX [IX_Charts_PlayerNumber] ON [Charts] ([PlayerNumber]);
GO

CREATE INDEX [IX_Charts_PositionId] ON [Charts] ([PositionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230519064656_InitDataTables', N'5.0.17');
GO

COMMIT;
GO

