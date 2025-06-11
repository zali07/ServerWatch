CREATE TABLE [dbo].[BackupEntries]
(
	[Id]            int IDENTITY(1,1)   NOT NULL PRIMARY KEY,
    [ServerGUID]    nvarchar(36)        NOT NULL,
    [DatabaseName]  nvarchar(255)       NULL,
    [Type]          nvarchar(8)         NULL, -- 'Daily' or 'Weekly'
    [Date]          smalldatetime       NULL,
    [SizeGB]        nvarchar(255)       NULL,
    [TS]            smalldatetime       NULL
)
GO