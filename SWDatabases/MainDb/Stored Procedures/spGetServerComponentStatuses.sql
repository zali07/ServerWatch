CREATE PROCEDURE [dbo].[spGetServerComponentStatuses]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.GUID,
        s.Server AS ServerName,
        sc.Component,
        CASE 
            WHEN sc.Component = 'Mirroring' AND (s.Flag & 4) = 0 THEN NULL
            WHEN sc.Component = 'Driver' AND (s.Flag & 8) = 0 THEN NULL
            WHEN sc.Component = 'Backup' AND (s.Flag & 16) = 0 THEN NULL
            ELSE
                CASE ISNULL(ss.Status, 0)
                    WHEN 2 THEN 'Critical'
                    WHEN 1 THEN 'Warning'
                    WHEN 0 THEN 'OK'
                    ELSE NULL
                END
        END AS Status
    FROM dbo.Servers s
    CROSS JOIN (
                  SELECT 'Mirroring' AS Component
        UNION ALL SELECT 'Driver'
        UNION ALL SELECT 'Backup'
    ) sc
    LEFT JOIN (
        SELECT 
            ServerGUID, 
            Component,
            MAX(Status) AS Status
        FROM dbo.ServerSemaphores
        GROUP BY ServerGUID, Component
    ) ss ON s.GUID = ss.ServerGUID AND sc.Component = ss.Component
    WHERE (s.Flag & 1) = 1 /* IsApproved */ AND (s.Flag & 2) = 0 /* Deleted/OUS */
END

GRANT EXECUTE ON [dbo].[spGetServerComponentStatuses] TO [SWRole] AS [dbo];
GO