CREATE TYPE [dbo].[ServerType] AS TABLE
(
	[GUID]				nvarchar(36)    NOT NULL PRIMARY KEY,
	[PublicKey]			nvarchar(max)	NULL,
	[Partner]			nvarchar(max)	NULL,
	[Server]			nvarchar(max)	NULL,
	[Windows]			nvarchar(max)	NULL,
	[Flag]				int				NOT NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[ServerType] TO [SWRole] AS [dbo];
GO