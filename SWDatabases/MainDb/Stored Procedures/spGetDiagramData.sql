CREATE PROCEDURE [dbo].[spGetDiagramData]
    @ServerGUID AS NVARCHAR(36),
    @Type       AS NVARCHAR(50),
    @StartDate  AS DATETIME,
    @EndDate    AS DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SET @EndDate = DATEADD(SECOND, -1, DATEADD(DAY, 1, CAST(@EndDate AS DATE)))

    IF @Type = 'DriversTemperature'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), TS, 120) AS [Label],
            MAX(ISNULL(Temperature, 0)) AS [Value],
            ISNULL(FriendlyName, 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE ServerGUID = @ServerGUID
          AND TS BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), [TS], 120), [FriendlyName]
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'DriversReadLatency'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), TS, 120) AS [Label],
            MAX(ISNULL(ReadLatencyMax, 0)) AS [Value],
            ISNULL(FriendlyName, 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE ServerGUID = @ServerGUID 
          AND TS BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), TS, 120), FriendlyName
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'DriversWriteLatency'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), TS, 120) AS [Label],
            MAX(ISNULL(WriteLatencyMax, 0)) AS [Value],
            ISNULL(FriendlyName, 'Unknown') AS [Category]
        FROM [dbo].[DriverEntries]
        WHERE ServerGUID = @ServerGUID
          AND TS BETWEEN @StartDate AND @EndDate
        GROUP BY CONVERT(nvarchar(10), TS, 120), FriendlyName
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'BackupsSizeGB'
    BEGIN
        SELECT
            CONVERT(nvarchar(10), TS, 120) AS [Label],
            MAX(TRY_CAST(SizeGB AS FLOAT)) AS [Value],
            ISNULL(DatabaseName, 'Unknown') AS [Category]
        FROM [dbo].[BackupEntries]
        WHERE ServerGUID = @ServerGUID
          AND TS BETWEEN @StartDate AND @EndDate
          AND ISNUMERIC(SizeGB) = 1
        GROUP BY CONVERT(nvarchar(10), TS, 120), [DatabaseName]
        ORDER BY [Label], [Category]
    END
    ELSE IF @Type = 'BackupsTS'
    BEGIN
        WITH FilteredBackups AS
        (
            SELECT *,
                CONVERT(nvarchar(10), [Date], 120) AS DayLabel,
                TRY_CAST([SizeGB] AS FLOAT) AS SizeValue,
                DATEADD(HOUR, 6, CAST(CONVERT(date, [Date]) AS DATETIME)) AS SixAM
            FROM [dbo].[BackupEntries]
            WHERE ServerGUID = @ServerGUID
                AND [Date] BETWEEN @StartDate AND @EndDate
                AND ISNUMERIC([SizeGB]) = 1
                AND TRY_CAST([SizeGB] AS FLOAT) > 0
        ),
        RankedBackups AS
        (
            SELECT *,
                ROW_NUMBER() OVER (PARTITION BY DayLabel, DatabaseName ORDER BY [Date] DESC) AS rn
            FROM FilteredBackups
        )
        SELECT
            DayLabel AS [Label],
            DATEDIFF(SECOND, SixAM, [Date]) AS [Value],
            ISNULL(DatabaseName, 'Unknown') AS [Category]
        FROM RankedBackups
        WHERE rn = 1 AND [Date] >= SixAM
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