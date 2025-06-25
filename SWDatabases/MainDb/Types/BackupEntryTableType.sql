CREATE TYPE [dbo].[BackupEntryTableType] AS TABLE
(
	ServerGUID		nvarchar(36)        NOT NULL,
	DatabaseName	nvarchar(255)		NULL,
	[Type]			nvarchar(8)         NULL, -- 'Daily' or 'Weekly'
	[Date]			datetime2(0)		NULL,
	SizeGB			nvarchar(255)       NULL,
	TS				smalldatetime       NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[BackupEntryTableType] TO [SWRole] AS [dbo];
GO