CREATE PROCEDURE [dbo].[spListAlerts]
AS
SET NOCOUNT ON;

DECLARE @userId uniqueidentifier;
SELECT @userId = UserId FROM dbo.svUser;

IF @userId IS NULL
BEGIN
	RETURN;
END

--retrieve the active alerts which have not yet been acknowledged by the user
SELECT a.Id, a.Title, a.[Message], a.[Date], a.ExpirationDate, a.[Type], a.AccessRight, aa.[Date] as AckDate
FROM dbo.Alerts a
    LEFT JOIN dbo.AlertsAck aa ON (a.Id = aa.AlertId AND aa.UserId = @userId)
WHERE a.Flag & 1 = 0 /* !Canceled */
    AND (a.ExpirationDate IS NULL OR a.ExpirationDate >= GETDATE())
    AND aa.UserId IS NULL
ORDER BY a.[Date] DESC, a.Id DESC;

RETURN
GO

GRANT EXECUTE ON [dbo].[spListAlerts] TO [SWRole] AS [dbo];
GO
