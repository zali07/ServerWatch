CREATE PROCEDURE [dbo].[dsAgGetDatabaseProperties]
AS
SET NOCOUNT ON;

-- loading the configured properties
DECLARE @code AS nvarchar(128), @companyName AS nvarchar(128), @codFiscal nvarchar(25), @codFiscal2 nvarchar(25),
		@cont nvarchar(34);

SELECT @code = Value
FROM dbo.[Config]
WHERE Property = N'DatabaseCode';

SELECT @companyName = Value
FROM dbo.[Config]
WHERE Property = N'CompanyName';

SELECT @codFiscal = CodFiscal, @codFiscal2 = CodFiscal2, @cont = Cont
FROM dbo.GeneralData;

-- returning the properties
SELECT @code AS Code, '[$(AuthorizationDb)]' AS AuthorizationDb, @companyName AS CompanyName, @codFiscal AS CodFiscal,
	   @codFiscal2 AS CodFiscal2, @cont AS ContBanca;

-- retrieving the Silver configuration settings
SELECT Property, Value
FROM dbo.[Config]

-- done
RETURN
GO

GRANT EXECUTE ON [dbo].[dsAgGetDatabaseProperties] TO [SWRole] AS [dbo];
GO
