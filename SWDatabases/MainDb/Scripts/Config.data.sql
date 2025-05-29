-- =============================================
-- Initial data in dbo.Config
-- =============================================

DECLARE @config TABLE (
	[Property]     NVARCHAR (64)  NOT NULL PRIMARY KEY,
	[Value]        NVARCHAR (128) NULL,
	[Description]  NVARCHAR (250) NULL
);

INSERT INTO @config (Property, [Value], [Description])

/* Generic settings */

SELECT N'DatabaseCode', N'test_company', N'ID of database.'
UNION ALL
SELECT N'DatabaseVersion', N'1111', N'Current version of the database.'
UNION ALL
SELECT N'MinAppVersion', N'1111', N'Minimum SWTower version that is required to work with the database.'
UNION ALL
SELECT N'CompanyName', N'Test Company Ltd.', N'Company name which uses the application.'

/* Agent module */

;

UPDATE c SET Description = t.Description
FROM dbo.Config c
	INNER JOIN @config t ON (c.Property = t.Property);

INSERT INTO dbo.Config (Property, Value, Description)
SELECT t.Property, t.Value, t.Description
FROM @config t
	LEFT JOIN dbo.Config c ON (t.Property = c.Property)
WHERE c.Property IS NULL;
GO