CREATE PROCEDURE [dbo].[spListServers] 
	@Filter as nvarchar(100) = NULL,
	@States as nvarchar(255) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CrLf as char(2) = char(13)+char(10)

	-- states of the server
	SET @States = 
		left( 
			'Meaning of abbreviations:'+@CrLf
			+'i - initialized'+@CrLf
			+'a - approved for data gathering'+@CrLf
			+'x - deleted / out of service'+@CrLf
			+'m - mirroring'+@CrLf
			+'d - drive data'+@CrLf
			+'b - backups'
		,255)

	-- return list with or without the filtering
	IF ISNULL(@Filter,'')=''
		BEGIN
			SELECT	[GUID], PublicKey, [Partner], [Server], [Windows], BackupRoot, Flag,
					[State] =
						 CASE WHEN Flag = 0 /*Initialized*/ then 'i' else '' END
						+CASE when (Flag & 1 /*Is approved*/) <> 0 then 'a' else '' END
						+CASE when (Flag & 2 /*Deleted / OUS*/) <> 0 then 'x' else '' END
						+CASE when (Flag & 4 /*Mirroring*/) <> 0 then 'm' else '' END
						+CASE when (Flag & 8 /*Drives*/) <> 0 then 'd' else '' END
						+CASE when (Flag & 16 /*Backups*/) <> 0 then 'b' else '' END
			FROM dbo.Servers
			ORDER BY [Partner] ASC, [Server] ASC
		END
	ELSE
		BEGIN
			SET @Filter = '%'+@Filter+'%'

			SELECT	[GUID], PublicKey, [Partner], [Server], [Windows], BackupRoot, Flag,
					[State] =
						 CASE WHEN Flag = 0 /*Initialized*/ then 'i' else '' END
						+CASE when (Flag & 1 /*Is approved*/) <> 0 then 'a' else '' END
						+CASE when (Flag & 2 /*Deleted / OUS*/) <> 0 then 'x' else '' END
						+CASE when (Flag & 4 /*Mirroring*/) <> 0 then 'm' else '' END
						+CASE when (Flag & 8 /*Drives*/) <> 0 then 'd' else '' END
						+CASE when (Flag & 16 /*Backups*/) <> 0 then 'b' else '' END
			FROM dbo.Servers
			WHERE ISNULL([GUID],'') like @Filter
				OR ISNULL(PublicKey,'') like @Filter
				OR ISNULL([Partner],'') like @Filter
				OR ISNULL([Server],'') like @Filter
				OR ISNULL([Windows],'') like @Filter
				OR ISNULL(Flag,'') like @Filter
			ORDER BY [Partner] ASC, [Server] ASC
		END

	RETURN
END
GO

GRANT EXECUTE ON [dbo].[spListServers] TO [SWRole] AS [dbo];
GO
