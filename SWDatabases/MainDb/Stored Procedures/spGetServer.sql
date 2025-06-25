CREATE PROCEDURE [dbo].[spGetServer] 
	@GUID as nvarchar(36)
AS
BEGIN
	SET NOCOUNT ON;

	IF @GUID IS NULL
    BEGIN
        RAISERROR('Parameter @GUID cannot be NULL.', 16, 1);
        RETURN;
    END

	SELECT  [GUID], PublicKey, [Partner], [Server], [Windows], BackupRoot, Flag,
			[State] =
				 CASE WHEN Flag = 0 /*Initialized*/ then 'i' else '' END
				+CASE when (Flag & 1 /*Is approved*/) <> 0 then 'a' else '' END
				+CASE when (Flag & 2 /*Deleted / OUS*/) <> 0 then 'x' else '' END
				+CASE when (Flag & 4 /*Mirroring*/) <> 0 then 'm' else '' END
				+CASE when (Flag & 8 /*Drives*/) <> 0 then 'd' else '' END
				+CASE when (Flag & 16 /*Backups*/) <> 0 then 'b' else '' END
	FROM dbo.Servers
	WHERE [GUID] = @GUID

	RETURN
END
GO

GRANT EXECUTE ON [dbo].[spGetServer] TO [SWRole] AS [dbo];
GO
