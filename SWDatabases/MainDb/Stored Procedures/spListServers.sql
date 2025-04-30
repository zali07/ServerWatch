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
			'Semnificatia prescurtarilor:'+@CrLf
			+'i - initializat'+@CrLf
			+'a - approved for data gathering'+@CrLf
			+'x - deleted / out of service'+@CrLf
			+'m - mirroring'+@CrLf
			+'d - drive data'+@CrLf
			+'b - backups'
		,255)

	-- return list with or without the filtering
	IF ISNULL(@Filter,'')=''
		BEGIN
			SELECT [GUID], PublicKey, [Partner], [Server], [Windows], IsApproved, Flag
			FROM dbo.Servers
			ORDER BY [Partner] ASC, [Server] ASC
		END
	ELSE
		BEGIN
			SET @Filter = '%'+@Filter+'%'

			SELECT [GUID], PublicKey, [Partner], [Server], [Windows], IsApproved, Flag
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
