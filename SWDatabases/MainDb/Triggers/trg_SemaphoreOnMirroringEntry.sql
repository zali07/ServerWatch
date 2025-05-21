CREATE TRIGGER [trg_SemaphoreOnMirroringEntry]
ON dbo.MirroringEntries
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    MERGE dbo.ServerSemaphores AS target
    USING (
        SELECT 
            i.ServerGUID,
            MAX(
                CASE 
                    WHEN i.MirroringState NOT IN (4, 2) THEN 'Mirroring state not healthy: ' + CAST(i.MirroringState AS NVARCHAR)
                    WHEN i.UnsentLog > 10000 THEN 'High unsent log backlog: ' + CAST(i.UnsentLog AS NVARCHAR)
                    WHEN i.AverageDelay > 5000 THEN 'High delay: ' + CAST(i.AverageDelay AS NVARCHAR)
                    ELSE NULL
                END
            ) AS Problem,
            MAX(
                CASE 
                    WHEN i.MirroringState NOT IN (4, 2) THEN 2
                    WHEN i.UnsentLog > 10000 OR i.AverageDelay > 5000 THEN 1
                    ELSE 0
                END
            ) AS NewStatus
        FROM inserted i
        GROUP BY i.ServerGUID
    ) AS src
    ON target.ServerGUID = src.ServerGUID AND target.Component = 'Mirroring'
    WHEN MATCHED AND (target.Status <> src.NewStatus OR target.Message <> src.Problem)
        THEN UPDATE SET 
            target.Status = src.NewStatus,
            target.Message = ISNULL(src.Problem, 'OK'),
            target.UpdatedAt = GETDATE()
    WHEN NOT MATCHED
        THEN INSERT (ServerGUID, Component, Status, Message)
             VALUES (src.ServerGUID, 'Mirroring', src.NewStatus, ISNULL(src.Problem, 'OK'));
END
