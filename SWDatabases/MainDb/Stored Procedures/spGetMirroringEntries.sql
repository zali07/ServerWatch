CREATE PROCEDURE [dbo].[spGetMirroringEntries]
    @ServerGUID AS NVARCHAR(36) = NULL
AS
BEGIN
    SELECT TOP 50 Id, ServerGUID, DatabaseName, Role, MirroringState, WitnessStatus,
        LogGenerationRate, UnsentLog, SendRate, UnrestoredLog, RecoveryRate,
        TransactionDelay, TransactionsPerSec, AverageDelay, TimeRecorded,
        TimeBehind, LocalTime, TS
    FROM dbo.MirroringEntries
    WHERE @ServerGUID IS NULL OR ServerGUID = @ServerGUID
    ORDER BY TS DESC
END
GO

GRANT EXECUTE ON [dbo].[spGetMirroringEntries] TO [SWRole] AS [dbo];
GO