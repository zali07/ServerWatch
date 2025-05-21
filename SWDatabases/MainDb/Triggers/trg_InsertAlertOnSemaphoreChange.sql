CREATE TRIGGER [trg_InsertAlertOnSemaphoreChange]
ON dbo.ServerSemaphores
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now smalldatetime = GETDATE();

    INSERT INTO dbo.Alerts ([Key], Title, Message, Date, Type)
    SELECT 
        i.ServerGUID + '-' + i.Component,
        'Alert: ' + ISNULL(s.Server, i.ServerGUID) + ' - ' + i.Component,
        'Component "' + i.Component + '" on server "' + ISNULL(s.Server, i.ServerGUID) + '" changed status to ' +
            CASE i.Status 
                WHEN 1 THEN 'Warning' 
                WHEN 2 THEN 'Error' 
                ELSE 'OK' 
            END + '. Detail: ' + ISNULL(i.Message, 'N/A'),
        @Now,
        CASE i.Status 
            WHEN 1 THEN 'W' 
            WHEN 2 THEN 'E' 
            ELSE 'I' 
        END
    FROM inserted i
        JOIN deleted d ON i.Id = d.Id
        LEFT JOIN dbo.Servers s ON i.ServerGUID = s.GUID
    WHERE 
        i.Status <> d.Status -- Only if status actually changed
        AND i.Status IN (1, 2); -- Only log if it's a warning or error

    UPDATE a
    SET a.Flag = 1 -- Cancelled
    FROM dbo.Alerts a
        JOIN inserted i ON a.[Key] = i.ServerGUID + '-' + i.Component
        JOIN deleted d ON i.Id = d.Id
    WHERE 
        i.Status = 0
        AND d.Status IN (1, 2) -- Previous was degraded
        AND a.Flag = 0
        AND a.Type IN ('W', 'E');
END
