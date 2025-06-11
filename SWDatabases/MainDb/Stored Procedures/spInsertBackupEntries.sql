CREATE PROCEDURE [dbo].[spInsertBackupEntries]
    @BackupEntries AS dbo.BackupEntryTableType READONLY
AS
BEGIN
    INSERT INTO BackupEntries(ServerGUID, DatabaseName, [Type], [Date], SizeGB, TS)
    SELECT ServerGUID, DatabaseName, [Type], [Date], SizeGB, TS
    FROM @BackupEntries;
END;

GRANT EXECUTE ON [dbo].[spInsertBackupEntries] TO [SWRole] AS [dbo];
GO