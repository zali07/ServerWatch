CREATE TRIGGER [trg_SemaphoreOnDriverEntry]
ON dbo.DriverEntries
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    MERGE dbo.ServerSemaphores AS target
    USING (
        SELECT 
            i.ServerGUID,
            CASE 
                WHEN i.Temperature > 70 THEN 'SSD overheat'
                WHEN i.WearLevel > 90 THEN 'SSD near wear-out'
                WHEN i.ReadLatencyMax > 500 OR i.WriteLatencyMax > 500 THEN 'SSD high latency'
                WHEN i.HealthStatus IS NOT NULL AND i.HealthStatus <> 'Healthy' THEN 'SSD health status: ' + i.HealthStatus
                ELSE NULL
            END AS Problem,
            CASE 
                WHEN i.Temperature > 70 OR i.WearLevel > 90 THEN 2
                WHEN i.ReadLatencyMax > 500 OR i.WriteLatencyMax > 500 THEN 1
                WHEN i.HealthStatus IS NOT NULL AND i.HealthStatus <> 'Healthy' THEN 1
                ELSE 0
            END AS NewStatus
        FROM inserted i
    ) AS src
    ON target.ServerGUID = src.ServerGUID AND target.Component = 'Driver'
    WHEN MATCHED AND (target.Status <> src.NewStatus OR target.Message <> src.Problem)
        THEN UPDATE SET 
            target.Status = src.NewStatus,
            target.Message = ISNULL(src.Problem, 'OK'),
            target.UpdatedAt = GETDATE()
    WHEN NOT MATCHED
        THEN INSERT (ServerGUID, Component, Status, Message)
             VALUES (src.ServerGUID, 'Driver', src.NewStatus, ISNULL(src.Problem, 'OK'));
END
