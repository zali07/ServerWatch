CREATE PROCEDURE [dbo].[spGetDriverEntries]
    @ServerGUID AS NVARCHAR(36) = NULL
AS
BEGIN
    SELECT TOP 50 Id, ServerGUID, DeviceId, FriendlyName, SerialNumber, Model, MediaType,
        HealthStatus, SizeGB, Temperature, TemperatureMax, PowerOnHours, WearLevel,
        ReadLatencyMax, WriteLatencyMax, TS
    FROM dbo.DriverEntries
    WHERE @ServerGUID IS NULL OR ServerGUID = @ServerGUID
    ORDER BY TS DESC
END
GO

GRANT EXECUTE ON [dbo].[spGetDriverEntries] TO [SWRole] AS [dbo];
GO