USE [Portal]
GO
/****** Object:  Table [dbo].[AbilityUser]    Script Date: 12/01/2008 10:55:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbilityUser](
	[PersonID] [int] NOT NULL,
	[AbilityID] [int] NOT NULL,
 CONSTRAINT [PK_AbilityUser] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC,
	[AbilityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AbilityUser]  WITH CHECK ADD  CONSTRAINT [FK_AbilityUser_Ability] FOREIGN KEY([AbilityID])
REFERENCES [dbo].[Ability] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbilityUser] CHECK CONSTRAINT [FK_AbilityUser_Ability]
GO
ALTER TABLE [dbo].[AbilityUser]  WITH CHECK ADD  CONSTRAINT [FK_AbilityUser_Users] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbilityUser] CHECK CONSTRAINT [FK_AbilityUser_Users]