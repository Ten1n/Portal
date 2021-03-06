USE [Portal]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_News_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[News]'))
ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_Users]
GO
/****** Object:  Table [dbo].[News]    Script Date: 11/20/2007 16:28:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[News]') AND type in (N'U'))
DROP TABLE [dbo].[News]

GO
/****** Object:  Table [dbo].[News]    Script Date: 12/05/2007 14:26:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Text] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[AuthorID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ExpireTime] [datetime] NOT NULL,
	[OfficeID] [int] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [Portal]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[Users] ([ID])


GO
/****** Object:  Table [dbo].[AllowTags]    Script Date: 11/20/2007 16:28:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllowTags]') AND type in (N'U'))
DROP TABLE [dbo].[AllowTags]



GO
/****** Object:  Table [dbo].[AllowTags]    Script Date: 11/08/2007 17:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllowTags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[tagName] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
 CONSTRAINT [PK_AllowTags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


GO
/****** Object:  StoredProcedure [dbo].[uiGetListPage]    Script Date: 11/20/2007 16:29:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uiGetListPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uiGetListPage]

GO
/****** Object:  StoredProcedure [dbo].[uiGetListPage]    Script Date: 11/20/2007 16:29:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create  PROCEDURE [dbo].[uiGetListPage]
   @PageIndex 	INT,	
   @PageSize 	INT,
   @OrderField 	NVARCHAR(64),
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

GO
/****** Object:  StoredProcedure [dbo].[uiGetObjectsPage]    Script Date: 11/20/2007 16:30:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uiGetObjectsPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uiGetObjectsPage]

USE [Portal]
GO
/****** Object:  StoredProcedure [dbo].[uiGetObjectsPage]    Script Date: 11/20/2007 16:30:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[uiGetObjectsPage]
	@PageIndex int,
	@PageSize int,
	@OrderField nvarchar(64),
	@IsOrderASC bit,
	@Query nvarchar(4000)
as
begin
	if @OrderField = ''
		set @OrderField = 'ID'
	
	-- Вычислить общее количество записей
	declare @countQuery nvarchar(4000)
	declare @totalCount int
	set @countQuery = N'with T as (' + @Query + ') select @TotalCountOut = count(1) from T'
	exec sp_executesql @countQuery, N'@TotalCountOut int output', @TotalCountOut = @totalCount output

	exec uiGetListPage @PageIndex, @PageSize, @OrderField, @IsOrderASC, @TotalCount, @Query
	return @TotalCount
end



