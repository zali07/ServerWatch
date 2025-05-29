CREATE PROCEDURE [dbo].[spInsertDriverEntries]
    @DriverEntries dbo.DriverEntryTableType READONLY
AS
BEGIN
    INSERT INTO DriverEntries
    (
        ServerGUID, DeviceId, FriendlyName, SerialNumber, Model, MediaType, HealthStatus,SizeGB, 
        Temperature, TemperatureMax, PowerOnHours, WearLevel, ReadLatencyMax, WriteLatencyMax, TS
    )
    SELECT  
        ServerGUID, DeviceId, FriendlyName, SerialNumber, Model, MediaType, HealthStatus, SizeGB,
        Temperature, TemperatureMax, PowerOnHours, WearLevel, ReadLatencyMax, WriteLatencyMax, TS
    FROM @DriverEntries;
END;

GRANT EXECUTE ON [dbo].[spInsertDriverEntries] TO [SWRole] AS [dbo];
GO