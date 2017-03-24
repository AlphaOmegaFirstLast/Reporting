USE [Test]
GO

/****** Object:  Table [dbo].[Employees]    Script Date: 24/03/2017 9:02:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](10) NULL,
	[BirthDate] [datetime2](7) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Experience] [nvarchar](max) NULL,
	[GraduationDate] [datetime2](7) NOT NULL,
	[Level] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Salary] [int] NOT NULL,
	[WebSite] [nvarchar](max) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Departments_DepartmentId]
GO


