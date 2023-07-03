CREATE TABLE [dbo].[Weather36Hour](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DatasetDescription] [nvarchar](50) NOT NULL,
	[LocationName] [nvarchar](50) NOT NULL,
	[ElementName] [nvarchar](50) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[ParameterName] [nvarchar](50) NULL,
	[ParameterValue] [nvarchar](50) NULL,
	[ParameterUnit] [nvarchar](50) NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Weather36Hour] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]