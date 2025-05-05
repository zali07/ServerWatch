CREATE PROCEDURE [dbo].[spGetServer] 
	@GUID as nvarchar(32)
AS
BEGIN
	SET NOCOUNT ON;

	IF @GUID IS NULL
    BEGIN
        RAISERROR('Parameter @Id cannot be NULL.', 16, 1);
        RETURN;
    END

	SELECT [GUID], PublicKey, [Partner], [Server], [Windows], IsApproved, Flag
	FROM dbo.Servers
	WHERE [GUID] = @GUID

	RETURN
END
GO

GRANT EXECUTE ON [dbo].[spGetServer] TO [SWRole] AS [dbo];
GO
