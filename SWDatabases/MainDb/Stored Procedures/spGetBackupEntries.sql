CREATE PROCEDURE [dbo].[spGetBackupEntries]
    @ServerGUID AS NVARCHAR(36) = NULL
AS
BEGIN
    SELECT TOP 50 Id, ServerGUID, DatabaseName, Type, Date, SizeGB, TS
    FROM dbo.BackupEntries
    WHERE @ServerGUID IS NULL OR ServerGUID = @ServerGUID
    ORDER BY TS DESC
END
GO

GRANT EXECUTE ON [dbo].[spGetBackupEntries] TO [SWRole] AS [dbo];
GO