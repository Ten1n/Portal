USE [Portal]
GO
/****** Object:  StoredProcedure [dbo].[uiGetObjectsPage]    Script Date: 12/29/2012 17:01:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER procedure [dbo].[uiGetObjectsPage]
	@PageIndex int,
	@PageSize int,
	@OrderField nvarchar(4000),
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



