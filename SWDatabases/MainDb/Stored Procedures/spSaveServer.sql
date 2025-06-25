CREATE PROCEDURE [dbo].[spSaveServer]
	@GUID				as nvarchar(36) OUTPUT,
	@server				as dbo.ServerType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- the name of the currently logged on user is
	DECLARE @utilizator as nvarchar(64)
	SELECT @utilizator = UserLogin
	FROM dbo.svUser s;
	IF @utilizator IS NULL
	BEGIN
		RAISERROR ('Error 5: Access denied.', 16, 1)
		RETURN
	END

	IF (SELECT COUNT(*) FROM @server) <> 1
	BEGIN
		RAISERROR ('The server list must contain exactly one record.', 16, 1)
		RETURN
	END

	SET @GUID = (SELECT TOP 1 [GUID] FROM @server); 

	BEGIN TRAN
		-- saving the changes made to the server (update)
		UPDATE s
		SET [Partner] = x.[Partner], [Server] = x.[Server], [Windows] = x.[Windows],
			BackupRoot = x.BackupRoot, Flag = x.Flag
		FROM dbo.Servers s
			INNER JOIN @server x ON (s.[GUID] = x.[GUID]);
		IF @@ERROR <> 0 BEGIN ROLLBACK RETURN END;

	COMMIT

	RETURN

END
GO

GRANT EXECUTE ON [dbo].[spSaveServer] TO [SWRole] AS [dbo];
GO