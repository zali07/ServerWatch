CREATE TYPE [dbo].[DriverEntryTableType] AS TABLE
(
    ServerGUID      nvarchar(36)    NOT NULL,
    DeviceId        bigint          NULL,
    FriendlyName    nvarchar(255)   NULL,
    SerialNumber    nvarchar(255)   NULL,
    Model           nvarchar(255)   NULL,
    MediaType       nvarchar(255)   NULL,
    HealthStatus    nvarchar(255)   NULL,
    SizeGB          nvarchar(50)    NULL,
    Temperature     int             NULL,
    TemperatureMax  int             NULL,
    PowerOnHours    int             NULL,
    WearLevel       int             NULL,
    ReadLatencyMax  decimal(20, 0)  NULL,
    WriteLatencyMax decimal(20, 0)  NULL,
    TS              smalldatetime   NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[DriverEntryTableType] TO [SWRole] AS [dbo];
GO