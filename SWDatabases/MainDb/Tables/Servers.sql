CREATE TABLE [dbo].[Servers]
(
	[Id]			int IDENTITY(1,1)	NOT NULL PRIMARY KEY,
	[GUID]			nvarchar(max)		NOT NULL,
	[PublicKey]		nvarchar(max)		NOT NULL,
	[Partner]		nvarchar(max)		NULL,
	[Server]		nvarchar(max)		NULL,
	[Windows]		nvarchar(max)		NULL,
	[IsApproved]	bit					NOT NULL DEFAULT (0),
	[Flag]			int					NOT NULL DEFAULT (0),
)
