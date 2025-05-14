CREATE TABLE [dbo].[Alerts]
(
	[Id]              int IDENTITY(1, 1) NOT NULL	PRIMARY KEY,
	[Key]             nvarchar(32)       NULL,
    [Title]           nvarchar(50)       NOT NULL,
    [Message]         nvarchar(MAX)      NOT NULL,
	[Info]            nvarchar(MAX)      NULL,
    [Date]            smalldatetime      NOT NULL,
    [ExpirationDate]  smalldatetime      NULL,
    [Type]            char(1)            NOT NULL,
	[AccessRight]     nvarchar(MAX)      NULL,
    [Flag]            int                NOT NULL	DEFAULT (0),
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'W - Warning, E - Error, I - Info',
	@level0type=N'SCHEMA',@level0name=N'dbo',
	@level1type=N'TABLE', @level1name=N'Alerts',
	@level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - Canceled',
	@level0type=N'SCHEMA',@level0name=N'dbo',
	@level1type=N'TABLE', @level1name=N'Alerts',
	@level2type=N'COLUMN',@level2name=N'Flag'
GO
