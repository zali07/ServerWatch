CREATE TYPE [dbo].[MirroringEntryTableType] AS TABLE
(
    ServerGUID          nvarchar(36)    NOT NULL,
    DatabaseName        nvarchar(64)    NULL,
    Role                int             NULL,
    MirroringState      int             NULL,
    WitnessStatus       int             NULL,
    LogGenerationRate   int             NULL,
    UnsentLog           int             NULL,
    SendRate            int             NULL,
    UnrestoredLog       int             NULL,
    RecoveryRate        int             NULL,
    TransactionDelay    int             NULL,
    TransactionsPerSec  int             NULL,
    AverageDelay        int             NULL,
    TimeRecorded        smalldatetime   NULL,
    TimeBehind          smalldatetime   NULL,
    LocalTime           smalldatetime   NULL,
    TS                  smalldatetime   NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[MirroringEntryTableType] TO [SWRole] AS [dbo];
GO