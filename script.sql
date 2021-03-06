USE [MyShulWorld]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 8/12/2016 12:30:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [varchar](50) NULL,
	[Time] [varchar](50) NULL,
	[Date] [date] NULL,
	[EventTypeId] [int] NULL,
	[BasedOn] [int] NULL,
	[TimeDifference] [int] NULL,
 CONSTRAINT [PK_Events_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EventTypes]    Script Date: 8/12/2016 12:30:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Fixed] [bit] NULL,
	[FixedTime] [varchar](50) NULL,
	[BasedOn] [int] NULL,
	[TimeDifference] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[LastDayPopulated] [datetime] NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Exclusions]    Script Date: 8/12/2016 12:30:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Exclusions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Exclusion] [varchar](max) NOT NULL,
	[EventTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Exclusions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Restrictions]    Script Date: 8/12/2016 12:30:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Restrictions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Restriction] [varchar](max) NOT NULL,
	[EventTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Restrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Exclusions]  WITH CHECK ADD  CONSTRAINT [FK_Exclusions_EventTypes] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventTypes] ([ID])
GO
ALTER TABLE [dbo].[Exclusions] CHECK CONSTRAINT [FK_Exclusions_EventTypes]
GO
ALTER TABLE [dbo].[Restrictions]  WITH CHECK ADD  CONSTRAINT [FK_Restrictions_Restrictions] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventTypes] ([ID])
GO
ALTER TABLE [dbo].[Restrictions] CHECK CONSTRAINT [FK_Restrictions_Restrictions]
GO
