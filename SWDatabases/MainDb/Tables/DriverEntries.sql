CREATE TABLE [dbo].[DriverEntries]
(
	[Id]					int IDENTITY(1,1)	NOT NULL PRIMARY KEY,
	[ServerGUID]			nvarchar(36)		NULL,
	[DeviceId]				bigint				NOT NULL,
	[FriendlyName]			nvarchar(max)		NOT NULL,
	[SerialNumber]			nvarchar(max)		NOT NULL,
	[Model]					nvarchar(max)		NOT NULL,
	[MediaType]				nvarchar(max)		NOT NULL,
	[HealthStatus]			nvarchar(max)		NOT NULL,
	[SizeGB]				nvarchar(max)		NOT NULL,
	[Temperature]			int					NULL,
	[TemperatureMax]		int					NULL,
	[PowerOnHours]			int					NULL,
	[WearLevel]				int					NULL,
	[ReadLatencyMax]		decimal(20, 0)		NOT NULL,
	[WriteLatencyMax]		decimal(20, 0)		NOT NULL,
	[TS]					smalldatetime		NOT NULL,
)
GO