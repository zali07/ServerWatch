CREATE PROCEDURE [dbo].[spInsertMirroringEntries]
    @MirroringEntries dbo.MirroringEntryTableType READONLY
AS
BEGIN
    INSERT INTO MirroringEntries ( ServerGUID, DatabaseName, [Role], MirroringState, WitnessStatus, LogGenerationRate,
        UnsentLog, SendRate, UnrestoredLog, RecoveryRate, TransactionDelay, TransactionsPerSec, AverageDelay,
        TimeRecorded, TimeBehind, LocalTime, TS)
    SELECT ServerGUID, DatabaseName, [Role], MirroringState, WitnessStatus, LogGenerationRate, 
        UnsentLog, SendRate, UnrestoredLog, RecoveryRate, TransactionDelay, TransactionsPerSec, AverageDelay,
        TimeRecorded, TimeBehind, LocalTime, TS
    FROM @MirroringEntries;
END;

GRANT EXECUTE ON [dbo].[spInsertMirroringEntries] TO [SWRole] AS [dbo];
GO