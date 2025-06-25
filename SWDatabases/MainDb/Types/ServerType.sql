CREATE TYPE [dbo].[ServerType] AS TABLE
(
	[GUID]				nvarchar(36)    NOT NULL PRIMARY KEY,
	[PublicKey]			nvarchar(max)	NULL,
	[Partner]			nvarchar(255)	NULL,
	[Server]			nvarchar(255)	NULL,
	[Windows]			nvarchar(255)	NULL,
	[BackupRoot]		nvarchar(255)	NULL,
	[Flag]				int				NOT NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[ServerType] TO [SWRole] AS [dbo];
GO