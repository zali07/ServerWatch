CREATE TABLE [dbo].[Config]
(
	[Property]     nvarchar(64)  NOT NULL PRIMARY KEY,
	[Value]        nvarchar(128) NULL,
	[Description]  nvarchar(255) NULL,
)
GO