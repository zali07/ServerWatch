CREATE TABLE [dbo].[Alerts]
(
	[Id]              int IDENTITY(1, 1) NOT NULL	PRIMARY KEY,
	[Key]             nvarchar(64)       NULL,
    [Title]           nvarchar(64)       NOT NULL,
    [Message]         nvarchar(255)      NOT NULL,
	[Info]            nvarchar(255)      NULL,
    [Date]            smalldatetime      NOT NULL,
    [ExpirationDate]  smalldatetime      NULL,
    [Type]            char(1)            NOT NULL,
	[AccessRight]     nvarchar(255)      NULL,
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
