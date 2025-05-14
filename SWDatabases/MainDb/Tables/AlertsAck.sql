CREATE TABLE [dbo].[AlertsAck]
(
	[AlertId]	int					NOT NULL   FOREIGN KEY REFERENCES [dbo].[Alerts]([Id]),
	[UserId]	uniqueidentifier	NOT NULL,
	[Date]		datetime			NOT NULL   DEFAULT (GETDATE()),
	PRIMARY KEY ([AlertId] ASC, [UserId] ASC)
)
GO
