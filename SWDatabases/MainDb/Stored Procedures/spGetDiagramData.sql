CREATE PROCEDURE [dbo].[spGetDiagramData]
    @ServerGUID AS NVARCHAR(36),
    @Type       AS NVARCHAR(50),
    @StartDate  AS DATETIME,
    @EndDate    AS DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'DriversTemperature'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), [TS], 120) AS [Label],
            MAX(ISNULL([Temperature], 0)) AS [Value],
            ISNULL([FriendlyName], 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE [ServerGUID] = @ServerGUID
          AND [TS] BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), [TS], 120), [FriendlyName]
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'DriversReadLatency'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), [TS], 120) AS [Label],
            MAX(ISNULL(ReadLatencyMax, 0)) AS [Value],
            ISNULL([FriendlyName], 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE [ServerGUID] = @ServerGUID 
          AND [TS] BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), [TS], 120), [FriendlyName]
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'DriversWriteLatency'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), [TS], 120) AS [Label],
            MAX(ISNULL(WriteLatencyMax, 0)) AS [Value],
            ISNULL([FriendlyName], 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE [ServerGUID] = @ServerGUID
          AND [TS] BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), [TS], 120), [FriendlyName]
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'BackupsSizeGB'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), [TS], 120) AS [Label],
            MAX(TRY_CAST([SizeGB] AS FLOAT)) AS [Value],
            ISNULL([DatabaseName], 'Unknown') AS [Category]
        FROM [dbo].[BackupEntries]
        WHERE [ServerGUID] = @ServerGUID
          AND [TS] BETWEEN @StartDate AND @EndDate
          AND ISNUMERIC([SizeGB]) = 1
        GROUP BY CONVERT(nvarchar(10), [TS], 120), [DatabaseName]
        ORDER BY [Label], [Category]
    END
    ELSE
    BEGIN
        RAISERROR('Unsupported type. Use a valid type that is indicated in the procedure.', 16, 1);
        RETURN;
    END
END
GO

GRANT EXECUTE ON [dbo].[spGetDiagramData] TO [SWRole] AS [dbo];
GO