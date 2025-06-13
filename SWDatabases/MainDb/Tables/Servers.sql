CREATE TABLE [dbo].[Servers]
(
	[Id]			int IDENTITY(1,1)	NOT NULL PRIMARY KEY,
	[GUID]			nvarchar(36)		NOT NULL,
	[PublicKey]		nvarchar(max)		NOT NULL,
	[Partner]		nvarchar(255)		NULL,
	[Server]		nvarchar(255)		NULL,
	[Windows]		nvarchar(255)		NULL,
	[BackupRoot]	nvarchar(255)		NULL,
	[Flag]			int					NOT NULL DEFAULT (0),
)
GO

GRANT SELECT, INSERT ON [dbo].[Servers] TO [SWRole] AS [dbo];
GO

EXEC sp_addextendedproperty 
	@name = N'MS_Description', 
	@value = N'1 - Is approved, 2 - Deleted / OUS, 4 - Mirroring, 8 - Drive data, 16 - Backups',
	@level0type = N'Schema', @level0name = 'dbo', 
	@level1type = N'Table',  @level1name = 'Servers', 
	@level2type = N'Column', @level2name = 'Flag'
GO
