USE [Portal]
GO
/****** Object:  StoredProcedure [dbo].[uiGetListPage]    Script Date: 12/29/2012 17:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER  PROCEDURE [dbo].[uiGetListPage]
   @PageIndex 	INT,	
   @PageSize 	INT,
   @OrderField 	NVARCHAR(4000),
   @IsOrderASC 	BIT,
   @TotalCount  INT,
   @Query 	NVARCHAR(4000)
AS
/*
	Возвращает результат запроса с учетом пейджинга
*/
SET XACT_ABORT ON

DECLARE @Order1 AS NVARCHAR(6)
DECLARE @Order2 AS NVARCHAR(6)

IF (@IsOrderASC = 1)
BEGIN
   SET @Order1 = ' ASC '
   SET @Order2 = ' DESC '
END
ELSE
BEGIN
   SET @Order1 = ' DESC '
   SET @Order2 = ' ASC '
END

DECLARE @Count INT
SET @Count = @TotalCount - @PageIndex * @PageSize

IF @Count < 0 SET @Count = 0

DECLARE @SQL AS NVARCHAR(4000)
SET @SQL = 
    'SELECT TOP ' + CAST(@PageSize AS NVARCHAR(16)) + ' * FROM
            (SELECT TOP ' + CAST(@Count AS NVARCHAR(16)) + ' * FROM (' + @Query + ') SO0 ORDER BY ' + @OrderField + @Order2 + ') SO1 ' +
    ' ORDER BY ' + @OrderField + @Order1

print @SQL 

EXEC (@SQL)

