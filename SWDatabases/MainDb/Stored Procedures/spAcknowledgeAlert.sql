CREATE PROCEDURE [dbo].[spAcknowledgeAlert]
    @alertId int
AS
SET NOCOUNT ON;

DECLARE @userId uniqueidentifier;
SELECT @userId = UserId FROM dbo.svUser;

IF @userId IS NULL
BEGIN
	RAISERROR ('Error 5: Access denied.', 16, 1)
	RETURN
END

-- if not yet acknowledged by the user
IF NOT EXISTS ( SELECT AlertId
				FROM dbo.AlertsAck 
				WHERE AlertId = @alertId AND UserId = @userId )
BEGIN
	-- acknowledge it
	INSERT INTO dbo.AlertsAck(AlertId, UserId, [Date])
	SELECT @alertId, @userId, GETDATE();
END

RETURN
GO

GRANT EXECUTE ON [dbo].[spAcknowledgeAlert] TO [SWRole] AS [dbo];
GO
