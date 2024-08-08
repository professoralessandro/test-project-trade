CREATE DATABASE TradeBank
GO

USE TradeBank

-- -----------------------------------------------------
-- [dbo].[Trade]
-- -----------------------------------------------------
IF OBJECT_ID('[dbo].[Trade]') IS NULL
BEGIN
	CREATE TABLE [dbo].[Trade] (
	    [TradeId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
		[Value] INT NOT NULL,
	    [ClientSector] [varchar](50) NOT NULL,
		[ClientRisK] [varchar](50) NOT NULL,
		[DateRegistered] [datetime] NOT NULL,
		[DateUpdated] [datetime] NULL
    )
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO