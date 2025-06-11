CREATE PROCEDURE [dbo].[sp_CheckAlerts]
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now smalldatetime = GETDATE();

    -- Ensure required components exist per approved server
    INSERT INTO dbo.ServerSemaphores (ServerGUID, Component, Status, Message)
    SELECT 
        s.GUID, c.Component, 1, 'Monitoring not initialized for this component.'
    FROM dbo.Servers s
    CROSS APPLY (VALUES ('Driver'), ('Mirroring')) AS c(Component)
    LEFT JOIN dbo.ServerSemaphores ss 
        ON ss.ServerGUID = s.GUID AND ss.Component = c.Component
    WHERE (s.Flag & 1) = 1 /* IsApproved */ AND ss.Id IS NULL;

    -- Check for stale semaphores: data not updated in last 12 hours
    MERGE dbo.ServerSemaphores AS target
    USING (
        SELECT 
            s.GUID AS ServerGUID,
            ss.Component,
            1 AS StatusLevel,
            'No recent data for component "' + ss.Component + '" in past 12 hours.' AS Problem
        FROM dbo.Servers s
        JOIN dbo.ServerSemaphores ss ON ss.ServerGUID = s.GUID
        WHERE 
            (s.Flag & 1) = 1 /* IsApproved */
            AND ss.UpdatedAt < DATEADD(HOUR, -12, @Now)
            AND ss.Status <> 2 -- Avoid overwriting existing critical errors
    ) AS src
    ON target.ServerGUID = src.ServerGUID AND target.Component = src.Component
    WHEN MATCHED AND (target.Status <> src.StatusLevel OR target.Message <> src.Problem)
        THEN UPDATE SET 
            Status = src.StatusLevel, 
            Message = src.Problem, 
            UpdatedAt = @Now;
END
