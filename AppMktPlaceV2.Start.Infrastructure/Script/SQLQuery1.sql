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

CREATE PROCEDURE TradePaginated
    @TradeId UNIQUEIDENTIFIER,
    @ClientSector VARCHAR(50),
    @ClientRisk VARCHAR(50),
    @PageNumber INT,
    @RowspPage INT
AS
BEGIN
    SELECT [TradeId], [Value], [ClientSector], [ClientRisk], [DateRegistered], [DateUpdated]
    FROM [TradeBank].[dbo].[Trade] [td]
    WHERE
                ([td].[TradeId]					=		  		@TradeId 						OR	@TradeId 				IS NULL)
    AND         ([td].[ClientSector]	        LIKE 	    '%' +@ClientSector+ '%'		        OR	@ClientSector		    IS NULL)
    AND         ([td].[ClientRisk]		        LIKE 	    '%' +@ClientRisk+ '%'		        OR	@ClientRisk  		    IS NULL)
    ORDER BY    [DateRegistered]
    OFFSET      (@PageNumber - 1) * @RowspPage ROWS
    FETCH NEXT  @RowspPage ROWS ONLY;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertTrade
    @TradeId UNIQUEIDENTIFIER,
    @Value INT,
    @ClientSector VARCHAR(50),
    @ClientRisk VARCHAR(50),
    @DateRegistered DATETIME
AS
BEGIN
    INSERT INTO [TradeBank].[dbo].[Trade]
    ([TradeId], [Value], [ClientSector], [ClientRisk], [DateRegistered])
    VALUES (@TradeId, @Value, @ClientSector, @ClientRisk, @DateRegistered);
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UpdateTrade
    @TradeId UNIQUEIDENTIFIER,
    @Value INT,
    @ClientSector VARCHAR(50),
    @ClientRisk VARCHAR(50),
    @DateUpdated DATETIME
AS
BEGIN
    UPDATE [TradeBank].[dbo].[Trade]
    SET [Value] = @Value,
        [ClientSector] = @ClientSector,
        [ClientRisk] = @ClientRisk,
        [DateUpdated] = @DateUpdated
    WHERE [TradeId] = @TradeId;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE DeleteTrade
    @TradeId UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM [TradeBank].[dbo].[Trade]
    WHERE [TradeId] = @TradeId;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO