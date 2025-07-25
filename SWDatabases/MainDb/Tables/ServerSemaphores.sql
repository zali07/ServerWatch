﻿CREATE TABLE [dbo].[ServerSemaphores] (
    [Id]            INT             IDENTITY(1,1) PRIMARY KEY,
    [ServerGUID]    NVARCHAR(36)    NOT NULL,
    [Component]     NVARCHAR(32)    NOT NULL, -- 'Driver', 'Mirroring', 'Backup'
    [Status]        INT             NOT NULL, -- 0 = OK, 1 = Warning, 2 = Error
    [Message]       NVARCHAR(255)   NULL,
    [UpdatedAt]     SMALLDATETIME   NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_ServerSemaphores UNIQUE (ServerGUID, Component)
);