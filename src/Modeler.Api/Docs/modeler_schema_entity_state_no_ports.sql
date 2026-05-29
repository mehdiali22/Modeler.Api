USE [master]
GO

/****** Object:  Database [Modeler05]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE DATABASE [Modeler05]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Modeler05', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Modeler05.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Modeler05_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Modeler05_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

ALTER DATABASE [Modeler05] SET COMPATIBILITY_LEVEL = 160
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Modeler05].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Modeler05] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [Modeler05] SET ANSI_NULLS OFF
GO

ALTER DATABASE [Modeler05] SET ANSI_PADDING OFF
GO

ALTER DATABASE [Modeler05] SET ANSI_WARNINGS OFF
GO

ALTER DATABASE [Modeler05] SET ARITHABORT OFF
GO

ALTER DATABASE [Modeler05] SET AUTO_CLOSE OFF
GO

ALTER DATABASE [Modeler05] SET AUTO_SHRINK OFF
GO

ALTER DATABASE [Modeler05] SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE [Modeler05] SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE [Modeler05] SET CURSOR_DEFAULT  GLOBAL
GO

ALTER DATABASE [Modeler05] SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE [Modeler05] SET NUMERIC_ROUNDABORT OFF
GO

ALTER DATABASE [Modeler05] SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE [Modeler05] SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE [Modeler05] SET  ENABLE_BROKER
GO

ALTER DATABASE [Modeler05] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO

ALTER DATABASE [Modeler05] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

ALTER DATABASE [Modeler05] SET TRUSTWORTHY OFF
GO

ALTER DATABASE [Modeler05] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE [Modeler05] SET PARAMETERIZATION SIMPLE
GO

ALTER DATABASE [Modeler05] SET READ_COMMITTED_SNAPSHOT ON
GO

ALTER DATABASE [Modeler05] SET HONOR_BROKER_PRIORITY OFF
GO

ALTER DATABASE [Modeler05] SET RECOVERY FULL
GO

ALTER DATABASE [Modeler05] SET  MULTI_USER
GO

ALTER DATABASE [Modeler05] SET PAGE_VERIFY CHECKSUM
GO

ALTER DATABASE [Modeler05] SET DB_CHAINING OFF
GO

ALTER DATABASE [Modeler05] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF )
GO

ALTER DATABASE [Modeler05] SET TARGET_RECOVERY_TIME = 60 SECONDS
GO

ALTER DATABASE [Modeler05] SET DELAYED_DURABILITY = DISABLED
GO

ALTER DATABASE [Modeler05] SET ACCELERATED_DATABASE_RECOVERY = OFF
GO

EXEC sys.sp_db_vardecimal_storage_format N'Modeler05', N'ON'
GO

ALTER DATABASE [Modeler05] SET QUERY_STORE = ON
GO

ALTER DATABASE [Modeler05] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

USE [Modeler05]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Actions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Actions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActionKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[TargetArtifactId] [int] NULL,
	[ExecutorKind] [int] NULL,
	[ExecutorActorId] [int] NULL,
	[Description] [nvarchar](2000) NULL,
	[DefaultParamsJson] [nvarchar](max) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Actors]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Actors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActorKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Kind] [int] NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Actors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Artifacts]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Artifacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[IsChildOfCase] [bit] NOT NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Artifacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ConditionFactUsed]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ConditionFactUsed](
	[ConditionId] [int] NOT NULL,
	[FactId] [int] NOT NULL,
 CONSTRAINT [PK_ConditionFactUsed] PRIMARY KEY CLUSTERED 
(
	[ConditionId] ASC,
	[FactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Conditions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Conditions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConditionKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Expression] [nvarchar](4000) NOT NULL,
	[FailMessage] [nvarchar](1000) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Conditions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DecisionOptionFactChanges]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DecisionOptionFactChanges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioDecisionOptionId] [int] NOT NULL,
	[FactId] [int] NOT NULL,
	[Op] [int] NOT NULL,
	[Value] [nvarchar](200) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_DecisionOptionFactChanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DictionaryTerms]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DictionaryTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TermKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DictionaryTerms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[FactEnumValues]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FactEnumValues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FactId] [int] NOT NULL,
	[EnumKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Value] [nvarchar](200) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FactEnumValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Facts]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Facts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArtifactId] [int] NOT NULL,
	[FactKey] [nvarchar](150) NOT NULL,
	[ValueType] [int] NOT NULL,
	[Meaning] [nvarchar](2000) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Facts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[KartablRoutingRules]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KartablRoutingRules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleKey] [nvarchar](150) NOT NULL,
	[OwnerSubdomain] [nvarchar](150) NULL,
	[Priority] [int] NOT NULL,
	[FromKartablId] [int] NULL,
	[TargetKartablId] [int] NOT NULL,
	[ConditionIdsJson] [nvarchar](max) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_KartablRoutingRules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Kartabls]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Kartabls](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KartablKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[OwnerSubdomain] [nvarchar](150) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Kartabls] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Processes]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Processes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[Order] [int] NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Processes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[ScenarioActions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioActions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioId] [int] NOT NULL,
	[ActionId] [int] NOT NULL,
	[ParamsJson] [nvarchar](max) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ScenarioActions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScenarioDecisionOptions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioDecisionOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioDecisionId] [int] NOT NULL,
	[OptionKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[ConditionIdsJson] [nvarchar](max) NULL,
	[ActionIdsJson] [nvarchar](max) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ScenarioDecisionOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScenarioDecisions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioDecisions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioId] [int] NOT NULL,
	[DecisionKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[UiActionKey] [nvarchar](150) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ScenarioDecisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScenarioFactChanges]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioFactChanges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioId] [int] NOT NULL,
	[FactId] [int] NOT NULL,
	[Op] [int] NOT NULL,
	[Value] [nvarchar](200) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_ScenarioFactChanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[ScenarioInputArtifacts]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioInputArtifacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioId] [int] NOT NULL,
	[ArtifactId] [int] NOT NULL,
	[RoleKey] [nvarchar](150) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ScenarioInputArtifacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScenarioKartabls]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioKartabls](
	[ScenarioId] [int] NOT NULL,
	[KartablId] [int] NOT NULL,
 CONSTRAINT [PK_ScenarioKartabls] PRIMARY KEY CLUSTERED 
(
	[ScenarioId] ASC,
	[KartablId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScenarioPreconditions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScenarioPreconditions](
	[ScenarioId] [int] NOT NULL,
	[ConditionId] [int] NOT NULL,
 CONSTRAINT [PK_ScenarioPreconditions] PRIMARY KEY CLUSTERED 
(
	[ScenarioId] ASC,
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Scenarios]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Scenarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[StageId] [int] NOT NULL,
	[OwnerSubdomain] [nvarchar](150) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Scenarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Stages]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Stages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessId] [int] NOT NULL,
	[StageKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[Order] [int] NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
	[SubProcessId] [int] NULL,
 CONSTRAINT [PK_Stages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SubProcesses]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SubProcesses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessId] [int] NOT NULL,
	[SubProcessKey] [nvarchar](150) NOT NULL,
	[TitleFa] [nvarchar](300) NULL,
	[Description] [nvarchar](2000) NULL,
	[Order] [int] NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_SubProcesses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[WorkItemActions]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkItemActions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkItemId] [int] NOT NULL,
	[ActionId] [int] NOT NULL,
	[Source] [nvarchar](30) NOT NULL,
	[SourceScenarioId] [int] NOT NULL,
	[SourceDecisionOptionId] [int] NULL,
	[ParamsJson] [nvarchar](max) NULL,
	[Status] [nvarchar](30) NOT NULL,
	[AttemptCount] [int] NOT NULL,
	[LastAttemptAtUtc] [datetime2](7) NULL,
	[CompletedAtUtc] [datetime2](7) NULL,
	[FailedAtUtc] [datetime2](7) NULL,
	[LastError] [nvarchar](max) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_WorkItemActions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[WorkItems]    Script Date: 5/27/2026 6:43:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkItemKey] [nvarchar](450) NOT NULL,
	[OwnerSubdomain] [nvarchar](450) NOT NULL,
	[ReferenceNo] [nvarchar](450) NULL,
	[CaseId] [nvarchar](450) NULL,
	[CurrentKartablId] [int] NULL,
	[FactsJson] [nvarchar](max) NULL,
	[CaseStatus] [nvarchar](450) NULL,
	[Title] [nvarchar](max) NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_WorkItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260517143452_AddWorkItemActionOutboxStatus', N'8.0.8')
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260521160000_AddStageSubProcess', N'8.0.8')
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260526170000_AddFactChangeSortOrder', N'8.0.8')
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260526182000_SeedDaramadAdmProcess', N'8.0.8')
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260526203000_AddDaramadReviewDecisionRuntime', N'8.0.8')
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260529123000_AddEntityStatesAndActionTransitions', N'8.0.8')
GO

GO

GO

/****** Object:  Table [dbo].[EntityStates]    Script Date: 2026-05-29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EntityStates](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ArtifactId] [int] NOT NULL,
    [StateKey] [nvarchar](150) NOT NULL,
    [TitleFa] [nvarchar](300) NULL,
    [ConditionJson] [nvarchar](max) NOT NULL,
    [Description] [nvarchar](1000) NULL,
    [CreatedAtUtc] [datetime2](7) NOT NULL,
    [UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_EntityStates] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EntityStates] ADD CONSTRAINT [DF_EntityStates_ConditionJson] DEFAULT (N'[]') FOR [ConditionJson]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_EntityStates_ArtifactId_StateKey] ON [dbo].[EntityStates]([ArtifactId] ASC,[StateKey] ASC)
GO

/****** Object:  Table [dbo].[ActionStateTransitions]    Script Date: 2026-05-29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActionStateTransitions](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ScenarioId] [int] NULL,
    [ActionId] [int] NOT NULL,
    [FromStateId] [int] NULL,
    [ToStateId] [int] NULL,
    [DecisionId] [int] NULL,
    [DecisionOptionId] [int] NULL,
    [LabelFa] [nvarchar](300) NULL,
    [SortOrder] [int] NOT NULL,
    [Description] [nvarchar](1000) NULL,
    [CreatedAtUtc] [datetime2](7) NOT NULL,
    [UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ActionStateTransitions] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ActionStateTransitions_ScenarioId] ON [dbo].[ActionStateTransitions]([ScenarioId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ActionStateTransitions_ActionId] ON [dbo].[ActionStateTransitions]([ActionId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ActionStateTransitions_FromStateId] ON [dbo].[ActionStateTransitions]([FromStateId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ActionStateTransitions_ToStateId] ON [dbo].[ActionStateTransitions]([ToStateId] ASC)
GO

SET IDENTITY_INSERT [dbo].[EntityStates] ON
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, 1, N'AdmOpenableForDaramadState', N'پرونده قابل پذیرش درآمد', N'[{"factKey":"Asnadm","op":"in","value":["DaryaftShode","OdatShodeBeDaramad"]},{"factKey":"IsAccepted","op":"=","value":false}]', N'پرونده در کارتابل درآمد قابل باز کردن است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 1, N'AdmClaimedState', N'پرونده پذیرش‌شده', N'[{"factKey":"Asnadm","op":"=","value":"DarHaleResidegiDaramad"},{"factKey":"IsAccepted","op":"=","value":true},{"factKey":"AcceptedByRole","op":"=","value":"Daramad"}]', N'پرونده توسط درآمد جاری پذیرش/claim شده است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, 1, N'AdmReadyForDaramadDecisionState', N'پرونده آماده تصمیم درآمد', N'[{"factKey":"Asnadm","op":"=","value":"DarHaleResidegiDaramad"},{"factKey":"IsAccepted","op":"=","value":true},{"factKey":"AcceptedByRole","op":"=","value":"Daramad"}]', N'پرونده پذیرش‌شده روی صفحه تصمیم درآمد نمایش داده شده و آماده انتخاب گزینه است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, 1, N'AdmApprovedByDaramadState', N'پرونده تاییدشده توسط درآمد', N'[{"factKey":"Asnadm","op":"=","value":"TaeidShodeTavasoteDaramad"}]', N'پرونده توسط درآمد تایید شده و آماده مسیردهی بعدی است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, 1, N'AdmCanceledByDaramadState', N'پرونده ابطال‌شده توسط درآمد', N'[{"factKey":"Asnadm","op":"=","value":"EbtalShodeTavasoteDaramad"},{"factKey":"IsAccepted","op":"=","value":false}]', N'پرونده توسط درآمد ابطال شده است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[EntityStates] ([Id], [ArtifactId], [StateKey], [TitleFa], [ConditionJson], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (6, 1, N'AdmReleasedByDaramadState', N'پرونده آزادشده از پذیرش درآمد', N'[{"factKey":"Asnadm","op":"=","value":"DaryaftShode"},{"factKey":"IsAccepted","op":"=","value":false},{"factKey":"AcceptedByUserId","op":"=","value":null}]', N'پذیرش/claim درآمد آزاد شده و پرونده دوباره قابل مشاهده است', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[EntityStates] OFF
GO

SET IDENTITY_INSERT [dbo].[ActionStateTransitions] ON
GO

INSERT [dbo].[ActionStateTransitions] ([Id], [ScenarioId], [ActionId], [FromStateId], [ToStateId], [DecisionId], [DecisionOptionId], [LabelFa], [SortOrder], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, 1, 4, 1, 2, NULL, NULL, N'پذیرش انجام فعالیت', 10, N'باز کردن پرونده و claim کردن برای درآمد جاری', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[ActionStateTransitions] ([Id], [ScenarioId], [ActionId], [FromStateId], [ToStateId], [DecisionId], [DecisionOptionId], [LabelFa], [SortOrder], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 5, 8, 2, 3, 2, NULL, N'مشاهده پرونده‌های پذیرش‌شده', 10, N'صفحه واقعی تصمیم درآمد؛ کاربر پرونده پذیرش‌شده را می‌بیند', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[ActionStateTransitions] ([Id], [ScenarioId], [ActionId], [FromStateId], [ToStateId], [DecisionId], [DecisionOptionId], [LabelFa], [SortOrder], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, 5, 5, 3, 4, 2, 4, N'تایید', 20, N'انتخاب تایید در تصمیم درآمد', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[ActionStateTransitions] ([Id], [ScenarioId], [ActionId], [FromStateId], [ToStateId], [DecisionId], [DecisionOptionId], [LabelFa], [SortOrder], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, 5, 6, 3, 5, 2, 5, N'ابطال', 30, N'انتخاب ابطال در تصمیم درآمد', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

INSERT [dbo].[ActionStateTransitions] ([Id], [ScenarioId], [ActionId], [FromStateId], [ToStateId], [DecisionId], [DecisionOptionId], [LabelFa], [SortOrder], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, 5, 7, 3, 6, 2, 6, N'انصراف از پذیرش', 40, N'انتخاب انصراف از پذیرش در تصمیم درآمد', CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T12:30:00.0000000' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[ActionStateTransitions] OFF
GO

SET IDENTITY_INSERT [dbo].[Actions] ON
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'ParseSepasXml', N'پارس XML دریافتی از سپاس', 1, 1, 2, N'پارس XML پرونده الکترونیک دریافت‌شده از سپاس', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, N'CalculateWarnings', N'محاسبه هشدارهای پرونده', 1, 1, 2, N'محاسبه هشدارهای سطحی، تاریخی، تیتک، فارماکوپه، استحقاق و هتلینگ', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, N'CalculateAutoDeductions', N'محاسبه کسور اتومات', 1, 1, 2, N'محاسبه کسور اتومات و کسر یارانه ارزی', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, N'OpenAdmAndAcceptWork', N'باز کردن پرونده و پذیرش انجام فعالیت', 1, 2, 1, N'باز کردن پرونده در کارتابل درآمد و قفل کردن آن برای همان کاربر درآمد', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, N'ApproveAdmByDaramad', N'تایید پرونده توسط درآمد', 1, 2, 1, N'تایید پرونده توسط نقش درآمد و ارسال به مرحله بعد', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (6, N'CancelAdmByDaramad', N'ابطال پرونده توسط درآمد', 1, 2, 1, N'ابطال پرونده توسط درآمد؛ پرونده باید در صورت نیاز مجدداً ارسال شود', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (7, N'ReleaseAdmWorkByDaramad', N'انصراف از پذیرش فعالیت', 1, 2, 1, N'برداشتن قفل پذیرش و قابل مشاهده شدن پرونده برای درآمدهای دیگر', N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actions] ([Id], [ActionKey], [TitleFa], [TargetArtifactId], [ExecutorKind], [ExecutorActorId], [Description], [DefaultParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (8, N'ShowAdmForDaramadDecision', N'مشاهده پرونده برای تصمیم درآمد', 1, 2, 1, N'نمایش پرونده پذیرش‌شده به کاربر درآمد برای انتخاب یکی از گزینه‌های تصمیم؛ این اکشن وضعیت پرونده را تغییر نمی‌دهد', N'{}', CAST(N'2026-05-29T10:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T10:30:00.0000000' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Actions] OFF
GO

SET IDENTITY_INSERT [dbo].[Actors] ON
GO

INSERT [dbo].[Actors] ([Id], [ActorKey], [TitleFa], [Kind], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'Daramad', N'درآمد', 2, N'نقش درآمد بیمارستان', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actors] ([Id], [ActorKey], [TitleFa], [Kind], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, N'RasaSystem', N'سامانه رسا', 1, N'نقش سیستمی سامانه رسا', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actors] ([Id], [ActorKey], [TitleFa], [Kind], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, N'TosiKonandeh', N'توزیع‌کننده', 2, N'نقش توزیع‌کننده مقوم', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Actors] ([Id], [ActorKey], [TitleFa], [Kind], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, N'Moghavem', N'مقوم', 2, N'نقش مقوم بیمه‌گر', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Actors] OFF
GO

SET IDENTITY_INSERT [dbo].[Artifacts] ON
GO

INSERT [dbo].[Artifacts] ([Id], [ArtifactKey], [TitleFa], [Description], [IsChildOfCase], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'Adm', N'پرونده الکترونیک پذیرش', N'پرونده الکترونیکی دریافت‌شده از سپاس شامل اطلاعات بستری، گلوبال، اورژانس تحت نظر یا بستری موقت', 0, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Artifacts] OFF
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (1, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (2, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (3, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (4, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (10, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (11, 1)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (5, 3)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (6, 3)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (7, 3)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (8, 33)
GO

INSERT [dbo].[ConditionFactUsed] ([ConditionId], [FactId]) VALUES (9, 33)
GO

SET IDENTITY_INSERT [dbo].[Conditions] ON
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'AdmIsDaryaftShode', N'پرونده دریافت‌شده است', N'Asnadm == "DaryaftShode"', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, N'AdmIsOdatShodeBeDaramad', N'پرونده عودت‌شده به درآمد است', N'Asnadm == "OdatShodeBeDaramad"', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, N'AdmCanBeOpenedByDaramad', N'پرونده برای باز شدن در درآمد آماده است', N'(Asnadm == "DaryaftShode" || Asnadm == "OdatShodeBeDaramad")', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, N'AdmIsDarHaleResidegiDaramad', N'پرونده در حال رسیدگی درآمد است', N'Asnadm == "DarHaleResidegiDaramad"', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, N'AdmIsAcceptedByNoOne', N'پرونده توسط هیچ درآمدی پذیرش نشده است', N'AcceptedByUserId == null', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (6, N'AdmIsAcceptedByCurrentUser', N'پرونده توسط همین کاربر درآمد پذیرش شده است', N'AcceptedByUserId == CurrentUserId', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (7, N'AdmVisibleInDaramadKartabl', N'پرونده برای کاربر درآمد قابل نمایش است', N'AcceptedByUserId == null || AcceptedByUserId == CurrentUserId', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (8, N'AdmHasMultipleMoghavem', N'بیمارستان چند مقومه است', N'HasMultipleMoghavem == true', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (9, N'AdmHasSingleMoghavem', N'بیمارستان تک مقومه است', N'HasMultipleMoghavem == false', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (10, N'AdmIsApprovedByDaramad', N'پرونده توسط درآمد تایید شده است', N'Asnadm == "TaeidShodeTavasoteDaramad"', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Conditions] ([Id], [ConditionKey], [TitleFa], [Expression], [FailMessage], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (11, N'AdmIsCanceledByDaramad', N'پرونده توسط درآمد ابطال شده است', N'Asnadm == "EbtalShodeTavasoteDaramad"', NULL, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Conditions] OFF
GO

SET IDENTITY_INSERT [dbo].[DecisionOptionFactChanges] ON
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (1, 4, 1, 1, N'TaeidShodeTavasoteDaramad', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 10)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (2, 5, 1, 1, N'EbtalShodeTavasoteDaramad', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 10)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (3, 5, 6, 1, N'false', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 20)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (4, 6, 1, 1, N'DaryaftShode', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 10)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (5, 6, 3, 2, NULL, CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 20)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (6, 6, 4, 2, NULL, CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 30)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (7, 6, 6, 1, N'false', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 40)
GO

INSERT [dbo].[DecisionOptionFactChanges] ([Id], [ScenarioDecisionOptionId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (8, 6, 5, 2, NULL, CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), 50)
GO

SET IDENTITY_INSERT [dbo].[DecisionOptionFactChanges] OFF
GO

SET IDENTITY_INSERT [dbo].[Facts] ON
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, 1, N'Asnadm', 1, N'وضعیت پرونده پذیرش', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 1, N'CurrentKartablId', 2, N'کارتابل فعلی', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, 1, N'AcceptedByUserId', 1, N'کاربر درآمد پذیرنده پرونده', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, 1, N'AcceptedByRole', 1, N'نقش پذیرنده پرونده', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, 1, N'AcceptedAt', 5, N'زمان پذیرش فعالیت', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (6, 1, N'IsAccepted', 4, N'آیا پرونده پذیرش فعالیت شده است', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (7, 1, N'AdmissionNo', 1, N'شماره پذیرش', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (8, 1, N'NationalCode', 1, N'کد ملی بیمار', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (9, 1, N'PatientName', 1, N'نام بیمار', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (10, 1, N'AdmType', 1, N'نوع پرونده', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (11, 1, N'AdmissionDate', 5, N'تاریخ پذیرش', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (12, 1, N'DischargeDate', 5, N'تاریخ ترخیص', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (13, 1, N'ReceivedDate', 5, N'تاریخ دریافت از سپاس', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (14, 1, N'HasWarning', 4, N'دارای هشدار', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (15, 1, N'HasLevelMismatchWarning', 4, N'هشدار مغایرت سطوح اطلاعاتی', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (16, 1, N'HasDateMismatchWarning', 4, N'هشدار مغایرت تاریخی', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (17, 1, N'HasTitekWarning', 4, N'هشدار تیتک', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (18, 1, N'HasPharmacopoeiaWarning', 4, N'هشدار فارماکوپه', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (19, 1, N'HasEligibilityWarning', 4, N'هشدار استحقاق', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (20, 1, N'HasHotelingWarning', 4, N'هشدار هتلینگ', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (21, 1, N'HasAutoDeduction', 4, N'دارای کسر اتومات', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (22, 1, N'TotalAmount', 3, N'مبلغ کل پرونده', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (23, 1, N'CoveredAmount', 3, N'مبلغ در تعهد', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (24, 1, N'GovernmentSubsidy', 3, N'یارانه دولت', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (25, 1, N'PatientShare', 3, N'سهم بیمار', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (26, 1, N'InsuranceShare', 3, N'سهم بیمه', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (27, 1, N'AutoDeductionAmount', 3, N'مبلغ کسر اتومات', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (28, 1, N'CurrencySubsidyAmount', 3, N'یارانه ارزی', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (29, 1, N'CurrencySubsidyDeduction', 3, N'کسر یارانه ارزی', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (30, 1, N'ApprovedAmount', 3, N'مبلغ مورد تایید', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (31, 1, N'NursingAmount', 3, N'مبلغ پرستاری', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (32, 1, N'PayableOrganizationShare', 3, N'سهم سازمان قابل پرداخت', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Facts] ([Id], [ArtifactId], [FactKey], [ValueType], [Meaning], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (33, 1, N'HasMultipleMoghavem', 4, N'بیمارستان چند مقومه است', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Facts] OFF
GO

SET IDENTITY_INSERT [dbo].[KartablRoutingRules] ON
GO

INSERT [dbo].[KartablRoutingRules] ([Id], [RuleKey], [OwnerSubdomain], [Priority], [FromKartablId], [TargetKartablId], [ConditionIdsJson], [TitleFa], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'Route_DaramadApproved_To_TosiKonandeh', N'Daramad', 10, 1, 2, N'[10,8]', N'تایید درآمد و چندمقومه؛ ارسال به توزیع‌کننده', N'اگر پرونده توسط درآمد تایید شده و بیمارستان چند مقومه باشد، پرونده به کارتابل توزیع‌کننده می‌رود', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[KartablRoutingRules] ([Id], [RuleKey], [OwnerSubdomain], [Priority], [FromKartablId], [TargetKartablId], [ConditionIdsJson], [TitleFa], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, N'Route_DaramadApproved_To_Moghavem', N'Daramad', 20, 1, 3, N'[10,9]', N'تایید درآمد و تک‌مقومه؛ ارسال به مقوم', N'اگر پرونده توسط درآمد تایید شده و بیمارستان تک مقومه باشد، پرونده مستقیم به کارتابل مقوم می‌رود', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[KartablRoutingRules] ([Id], [RuleKey], [OwnerSubdomain], [Priority], [FromKartablId], [TargetKartablId], [ConditionIdsJson], [TitleFa], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, N'Route_DaramadCanceled_To_AdmClosed', N'Daramad', 30, 1, 4, N'[11]', N'ابطال درآمد؛ خروج از چرخه', N'اگر درآمد پرونده را ابطال کند، پرونده از چرخه جاری خارج می‌شود', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[KartablRoutingRules] ([Id], [RuleKey], [OwnerSubdomain], [Priority], [FromKartablId], [TargetKartablId], [ConditionIdsJson], [TitleFa], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, N'Route_MoghavemReturned_To_DaramadKartabl', N'Moghavem', 40, 3, 1, N'[2]', N'عودت مقوم به کارتابل درآمد', N'اگر مقوم پرونده را به درآمد عودت دهد، پرونده دوباره در کارتابل درآمد قرار می‌گیرد', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[KartablRoutingRules] OFF
GO

SET IDENTITY_INSERT [dbo].[Kartabls] ON
GO

INSERT [dbo].[Kartabls] ([Id], [KartablKey], [TitleFa], [Description], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'DaramadKartabl', N'کارتابل درآمد', N'فقط پرونده‌هایی را نمایش می‌دهد که هیچ درآمدی پذیرش نکرده یا خود همین کاربر درآمد پذیرش کرده است', N'Daramad', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Kartabls] ([Id], [KartablKey], [TitleFa], [Description], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, N'TosiKonandehKartabl', N'کارتابل توزیع‌کننده', N'مقصد پرونده‌های تاییدشده درآمد برای بیمارستان چندمقومه', N'TosiKonandeh', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Kartabls] ([Id], [KartablKey], [TitleFa], [Description], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (3, N'MoghavemKartabl', N'کارتابل مقوم', N'مقصد پرونده‌های تاییدشده درآمد برای بیمارستان تک‌مقومه یا بعد از توزیع', N'Moghavem', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Kartabls] ([Id], [KartablKey], [TitleFa], [Description], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, N'AdmClosed', N'پرونده‌های ابطال‌شده / خارج از چرخه', N'پرونده‌هایی که توسط درآمد ابطال یا از چرخه جاری خارج شده‌اند', N'System', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Kartabls] OFF
GO

SET IDENTITY_INSERT [dbo].[Processes] ON
GO

INSERT [dbo].[Processes] ([Id], [ProcessKey], [TitleFa], [Description], [Order], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'RasaElectronicAdm', N'فرایند رسیدگی پرونده الکترونیک رسا', N'فرایند دریافت پرونده الکترونیک از سپاس، آماده‌سازی، بررسی درآمد، و ارسال پرونده به مرحله بعد یا ابطال', 10, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Processes] OFF
GO

SET IDENTITY_INSERT [dbo].[ScenarioActions] ON
GO

INSERT [dbo].[ScenarioActions] ([Id], [ScenarioId], [ActionId], [ParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, 1, 4, N'{}', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[ScenarioActions] ([Id], [ScenarioId], [ActionId], [ParamsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 5, 8, N'{}', CAST(N'2026-05-29T10:30:00.0000000' AS DateTime2), CAST(N'2026-05-29T10:30:00.0000000' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[ScenarioActions] OFF
GO

SET IDENTITY_INSERT [dbo].[ScenarioDecisionOptions] ON
GO

INSERT [dbo].[ScenarioDecisionOptions] ([Id], [ScenarioDecisionId], [OptionKey], [TitleFa], [ConditionIdsJson], [ActionIdsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (4, 2, N'Approve', N'تایید', NULL, N'[5]', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2))
GO

INSERT [dbo].[ScenarioDecisionOptions] ([Id], [ScenarioDecisionId], [OptionKey], [TitleFa], [ConditionIdsJson], [ActionIdsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, 2, N'Cancel', N'ابطال', NULL, N'[6]', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2))
GO

INSERT [dbo].[ScenarioDecisionOptions] ([Id], [ScenarioDecisionId], [OptionKey], [TitleFa], [ConditionIdsJson], [ActionIdsJson], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (6, 2, N'Release', N'انصراف از پذیرش', NULL, N'[7]', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[ScenarioDecisionOptions] OFF
GO

SET IDENTITY_INSERT [dbo].[ScenarioDecisions] ON
GO

INSERT [dbo].[ScenarioDecisions] ([Id], [ScenarioId], [DecisionKey], [TitleFa], [UiActionKey], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 5, N'DaramadDecision', N'تصمیم درآمد روی پرونده', N'DaramadDecision', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[ScenarioDecisions] OFF
GO

SET IDENTITY_INSERT [dbo].[ScenarioFactChanges] ON
GO

INSERT [dbo].[ScenarioFactChanges] ([Id], [ScenarioId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (1, 1, 1, 1, N'DarHaleResidegiDaramad', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 10)
GO

INSERT [dbo].[ScenarioFactChanges] ([Id], [ScenarioId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (2, 1, 3, 1, N'CurrentUserId', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 20)
GO

INSERT [dbo].[ScenarioFactChanges] ([Id], [ScenarioId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (3, 1, 4, 1, N'Daramad', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 30)
GO

INSERT [dbo].[ScenarioFactChanges] ([Id], [ScenarioId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (4, 1, 6, 1, N'true', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 40)
GO

INSERT [dbo].[ScenarioFactChanges] ([Id], [ScenarioId], [FactId], [Op], [Value], [CreatedAtUtc], [UpdatedAtUtc], [SortOrder]) VALUES (5, 1, 5, 1, N'Now', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 50)
GO

SET IDENTITY_INSERT [dbo].[ScenarioFactChanges] OFF
GO

INSERT [dbo].[ScenarioKartabls] ([ScenarioId], [KartablId]) VALUES (1, 1)
GO

INSERT [dbo].[ScenarioKartabls] ([ScenarioId], [KartablId]) VALUES (5, 1)
GO

INSERT [dbo].[ScenarioPreconditions] ([ScenarioId], [ConditionId]) VALUES (1, 3)
GO

INSERT [dbo].[ScenarioPreconditions] ([ScenarioId], [ConditionId]) VALUES (5, 4)
GO

SET IDENTITY_INSERT [dbo].[Scenarios] ON
GO

INSERT [dbo].[Scenarios] ([Id], [ScenarioKey], [TitleFa], [Description], [StageId], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, N'OpenAdmAndAcceptWork', N'باز کردن پرونده و پذیرش انجام فعالیت', N'باز کردن پرونده در کارتابل درآمد؛ پرونده برای همان کاربر درآمد قفل می‌شود و کارتابل تغییر نمی‌کند', 2, N'Daramad', CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[Scenarios] ([Id], [ScenarioKey], [TitleFa], [Description], [StageId], [OwnerSubdomain], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (5, N'ReviewAdmByDaramadDecisionScenario', N'تصمیم درآمد روی پرونده', N'سناریوی تصمیم واقعی درآمد بعد از باز کردن/پذیرش فعالیت؛ گزینه‌ها شامل تایید، ابطال و انصراف از پذیرش هستند', 2, N'Daramad', CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2), CAST(N'2026-05-27T08:15:07.3496761' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Scenarios] OFF
GO

SET IDENTITY_INSERT [dbo].[Stages] ON
GO

INSERT [dbo].[Stages] ([Id], [ProcessId], [StageKey], [TitleFa], [Description], [Order], [CreatedAtUtc], [UpdatedAtUtc], [SubProcessId]) VALUES (1, 1, N'AdmReceivedStage', N'دریافت و آماده‌سازی پرونده', N'پرونده از سپاس دریافت شده، XML پارس می‌شود و هشدارها/کسور اتومات محاسبه می‌شوند', 10, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 1)
GO

INSERT [dbo].[Stages] ([Id], [ProcessId], [StageKey], [TitleFa], [Description], [Order], [CreatedAtUtc], [UpdatedAtUtc], [SubProcessId]) VALUES (2, 1, N'DaramadReviewStage', N'رسیدگی درآمد بیمارستان', N'درآمد پرونده را در کارتابل درآمد مشاهده می‌کند، باز می‌کند، تایید/ابطال یا انصراف از پذیرش انجام می‌دهد', 20, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), 2)
GO

SET IDENTITY_INSERT [dbo].[Stages] OFF
GO

SET IDENTITY_INSERT [dbo].[SubProcesses] ON
GO

INSERT [dbo].[SubProcesses] ([Id], [ProcessId], [SubProcessKey], [TitleFa], [Description], [Order], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (1, 1, N'ReceiveAdmFromSepas', N'دریافت پرونده از سپاس', N'دریافت XML، پارس اطلاعات، محاسبه هشدارها و کسور اتومات', 10, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

INSERT [dbo].[SubProcesses] ([Id], [ProcessId], [SubProcessKey], [TitleFa], [Description], [Order], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (2, 1, N'DaramadHospitalReview', N'بررسی درآمد بیمارستان', N'مشاهده پرونده توسط درآمد، باز کردن پرونده/پذیرش فعالیت، تایید، ابطال یا انصراف از پذیرش', 20, CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2), CAST(N'2026-05-27T08:15:07.0315649' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[SubProcesses] OFF
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Actions_ActionKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Actions_ActionKey] ON [dbo].[Actions]
(
	[ActionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_Actions_ExecutorActorId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Actions_ExecutorActorId] ON [dbo].[Actions]
(
	[ExecutorActorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_Actions_TargetArtifactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Actions_TargetArtifactId] ON [dbo].[Actions]
(
	[TargetArtifactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Actors_ActorKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Actors_ActorKey] ON [dbo].[Actors]
(
	[ActorKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Artifacts_ArtifactKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Artifacts_ArtifactKey] ON [dbo].[Artifacts]
(
	[ArtifactKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ConditionFactUsed_FactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ConditionFactUsed_FactId] ON [dbo].[ConditionFactUsed]
(
	[FactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Conditions_ConditionKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Conditions_ConditionKey] ON [dbo].[Conditions]
(
	[ConditionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_DecisionOptionFactChanges_FactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_DecisionOptionFactChanges_FactId] ON [dbo].[DecisionOptionFactChanges]
(
	[FactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_DecisionOptionFactChanges_ScenarioDecisionOptionId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_DecisionOptionFactChanges_ScenarioDecisionOptionId] ON [dbo].[DecisionOptionFactChanges]
(
	[ScenarioDecisionOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_DictionaryTerms_TermKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_DictionaryTerms_TermKey] ON [dbo].[DictionaryTerms]
(
	[TermKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_FactEnumValues_FactId_EnumKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_FactEnumValues_FactId_EnumKey] ON [dbo].[FactEnumValues]
(
	[FactId] ASC,
	[EnumKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_Facts_ArtifactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Facts_ArtifactId] ON [dbo].[Facts]
(
	[ArtifactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Facts_FactKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Facts_FactKey] ON [dbo].[Facts]
(
	[FactKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_KartablRoutingRules_FromKartablId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_KartablRoutingRules_FromKartablId] ON [dbo].[KartablRoutingRules]
(
	[FromKartablId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_KartablRoutingRules_RuleKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_KartablRoutingRules_RuleKey] ON [dbo].[KartablRoutingRules]
(
	[RuleKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_KartablRoutingRules_TargetKartablId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_KartablRoutingRules_TargetKartablId] ON [dbo].[KartablRoutingRules]
(
	[TargetKartablId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Kartabls_KartablKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Kartabls_KartablKey] ON [dbo].[Kartabls]
(
	[KartablKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Processes_ProcessKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Processes_ProcessKey] ON [dbo].[Processes]
(
	[ProcessKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_ScenarioActions_ActionId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioActions_ActionId] ON [dbo].[ScenarioActions]
(
	[ActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioActions_ScenarioId_ActionId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ScenarioActions_ScenarioId_ActionId] ON [dbo].[ScenarioActions]
(
	[ScenarioId] ASC,
	[ActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_ScenarioDecisionOptions_ScenarioDecisionId_OptionKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ScenarioDecisionOptions_ScenarioDecisionId_OptionKey] ON [dbo].[ScenarioDecisionOptions]
(
	[ScenarioDecisionId] ASC,
	[OptionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_ScenarioDecisions_ScenarioId_DecisionKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ScenarioDecisions_ScenarioId_DecisionKey] ON [dbo].[ScenarioDecisions]
(
	[ScenarioId] ASC,
	[DecisionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioFactChanges_FactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioFactChanges_FactId] ON [dbo].[ScenarioFactChanges]
(
	[FactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioFactChanges_ScenarioId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioFactChanges_ScenarioId] ON [dbo].[ScenarioFactChanges]
(
	[ScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_ScenarioInputArtifacts_ArtifactId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioInputArtifacts_ArtifactId] ON [dbo].[ScenarioInputArtifacts]
(
	[ArtifactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioInputArtifacts_ScenarioId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioInputArtifacts_ScenarioId] ON [dbo].[ScenarioInputArtifacts]
(
	[ScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioKartabls_KartablId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioKartabls_KartablId] ON [dbo].[ScenarioKartabls]
(
	[KartablId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_ScenarioPreconditions_ConditionId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScenarioPreconditions_ConditionId] ON [dbo].[ScenarioPreconditions]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Scenarios_ScenarioKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Scenarios_ScenarioKey] ON [dbo].[Scenarios]
(
	[ScenarioKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_Scenarios_StageId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Scenarios_StageId] ON [dbo].[Scenarios]
(
	[StageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Stages_ProcessId_StageKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Stages_ProcessId_StageKey] ON [dbo].[Stages]
(
	[ProcessId] ASC,
	[StageKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_Stages_SubProcessId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Stages_SubProcessId] ON [dbo].[Stages]
(
	[SubProcessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_SubProcesses_ProcessId_SubProcessKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_SubProcesses_ProcessId_SubProcessKey] ON [dbo].[SubProcesses]
(
	[ProcessId] ASC,
	[SubProcessKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItemActions_ActionId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItemActions_ActionId] ON [dbo].[WorkItemActions]
(
	[ActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItemActions_Status]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItemActions_Status] ON [dbo].[WorkItemActions]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItemActions_Status_CreatedAtUtc]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItemActions_Status_CreatedAtUtc] ON [dbo].[WorkItemActions]
(
	[Status] ASC,
	[CreatedAtUtc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_WorkItemActions_WorkItemId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItemActions_WorkItemId] ON [dbo].[WorkItemActions]
(
	[WorkItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItems_CaseId]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItems_CaseId] ON [dbo].[WorkItems]
(
	[CaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItems_CurrentKartablId_OwnerSubdomain_CaseStatus_UpdatedAtUtc]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItems_CurrentKartablId_OwnerSubdomain_CaseStatus_UpdatedAtUtc] ON [dbo].[WorkItems]
(
	[CurrentKartablId] ASC,
	[OwnerSubdomain] ASC,
	[CaseStatus] ASC,
	[UpdatedAtUtc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItems_CurrentKartablId_ReferenceNo]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItems_CurrentKartablId_ReferenceNo] ON [dbo].[WorkItems]
(
	[CurrentKartablId] ASC,
	[ReferenceNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItems_ReferenceNo]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkItems_ReferenceNo] ON [dbo].[WorkItems]
(
	[ReferenceNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_WorkItems_WorkItemKey]    Script Date: 5/27/2026 6:43:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkItems_WorkItemKey] ON [dbo].[WorkItems]
(
	[WorkItemKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DecisionOptionFactChanges] ADD  DEFAULT ((0)) FOR [SortOrder]
GO

ALTER TABLE [dbo].[ScenarioFactChanges] ADD  DEFAULT ((0)) FOR [SortOrder]
GO

ALTER TABLE [dbo].[WorkItemActions] ADD  DEFAULT (N'Pending') FOR [Status]
GO

ALTER TABLE [dbo].[Actions]  WITH CHECK ADD  CONSTRAINT [FK_Actions_Actors_ExecutorActorId] FOREIGN KEY([ExecutorActorId])
REFERENCES [dbo].[Actors] ([Id])
GO

ALTER TABLE [dbo].[Actions] CHECK CONSTRAINT [FK_Actions_Actors_ExecutorActorId]
GO

ALTER TABLE [dbo].[Actions]  WITH CHECK ADD  CONSTRAINT [FK_Actions_Artifacts_TargetArtifactId] FOREIGN KEY([TargetArtifactId])
REFERENCES [dbo].[Artifacts] ([Id])
GO

ALTER TABLE [dbo].[Actions] CHECK CONSTRAINT [FK_Actions_Artifacts_TargetArtifactId]
GO

ALTER TABLE [dbo].[ConditionFactUsed]  WITH CHECK ADD  CONSTRAINT [FK_ConditionFactUsed_Conditions_ConditionId] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Conditions] ([Id])
GO

ALTER TABLE [dbo].[ConditionFactUsed] CHECK CONSTRAINT [FK_ConditionFactUsed_Conditions_ConditionId]
GO

ALTER TABLE [dbo].[ConditionFactUsed]  WITH CHECK ADD  CONSTRAINT [FK_ConditionFactUsed_Facts_FactId] FOREIGN KEY([FactId])
REFERENCES [dbo].[Facts] ([Id])
GO

ALTER TABLE [dbo].[ConditionFactUsed] CHECK CONSTRAINT [FK_ConditionFactUsed_Facts_FactId]
GO

ALTER TABLE [dbo].[DecisionOptionFactChanges]  WITH CHECK ADD  CONSTRAINT [FK_DecisionOptionFactChanges_Facts_FactId] FOREIGN KEY([FactId])
REFERENCES [dbo].[Facts] ([Id])
GO

ALTER TABLE [dbo].[DecisionOptionFactChanges] CHECK CONSTRAINT [FK_DecisionOptionFactChanges_Facts_FactId]
GO

ALTER TABLE [dbo].[DecisionOptionFactChanges]  WITH CHECK ADD  CONSTRAINT [FK_DecisionOptionFactChanges_ScenarioDecisionOptions_ScenarioDecisionOptionId] FOREIGN KEY([ScenarioDecisionOptionId])
REFERENCES [dbo].[ScenarioDecisionOptions] ([Id])
GO

ALTER TABLE [dbo].[DecisionOptionFactChanges] CHECK CONSTRAINT [FK_DecisionOptionFactChanges_ScenarioDecisionOptions_ScenarioDecisionOptionId]
GO

ALTER TABLE [dbo].[FactEnumValues]  WITH CHECK ADD  CONSTRAINT [FK_FactEnumValues_Facts_FactId] FOREIGN KEY([FactId])
REFERENCES [dbo].[Facts] ([Id])
GO

ALTER TABLE [dbo].[FactEnumValues] CHECK CONSTRAINT [FK_FactEnumValues_Facts_FactId]
GO

ALTER TABLE [dbo].[Facts]  WITH CHECK ADD  CONSTRAINT [FK_Facts_Artifacts_ArtifactId] FOREIGN KEY([ArtifactId])
REFERENCES [dbo].[Artifacts] ([Id])
GO

ALTER TABLE [dbo].[Facts] CHECK CONSTRAINT [FK_Facts_Artifacts_ArtifactId]
GO

ALTER TABLE [dbo].[KartablRoutingRules]  WITH CHECK ADD  CONSTRAINT [FK_KartablRoutingRules_Kartabls_FromKartablId] FOREIGN KEY([FromKartablId])
REFERENCES [dbo].[Kartabls] ([Id])
GO

ALTER TABLE [dbo].[KartablRoutingRules] CHECK CONSTRAINT [FK_KartablRoutingRules_Kartabls_FromKartablId]
GO

ALTER TABLE [dbo].[KartablRoutingRules]  WITH CHECK ADD  CONSTRAINT [FK_KartablRoutingRules_Kartabls_TargetKartablId] FOREIGN KEY([TargetKartablId])
REFERENCES [dbo].[Kartabls] ([Id])
GO

ALTER TABLE [dbo].[KartablRoutingRules] CHECK CONSTRAINT [FK_KartablRoutingRules_Kartabls_TargetKartablId]
GO

ALTER TABLE [dbo].[ScenarioActions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioActions_Actions_ActionId] FOREIGN KEY([ActionId])
REFERENCES [dbo].[Actions] ([Id])
GO

ALTER TABLE [dbo].[ScenarioActions] CHECK CONSTRAINT [FK_ScenarioActions_Actions_ActionId]
GO

ALTER TABLE [dbo].[ScenarioActions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioActions_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioActions] CHECK CONSTRAINT [FK_ScenarioActions_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[ScenarioDecisionOptions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioDecisionOptions_ScenarioDecisions_ScenarioDecisionId] FOREIGN KEY([ScenarioDecisionId])
REFERENCES [dbo].[ScenarioDecisions] ([Id])
GO

ALTER TABLE [dbo].[ScenarioDecisionOptions] CHECK CONSTRAINT [FK_ScenarioDecisionOptions_ScenarioDecisions_ScenarioDecisionId]
GO

ALTER TABLE [dbo].[ScenarioDecisions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioDecisions_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioDecisions] CHECK CONSTRAINT [FK_ScenarioDecisions_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[ScenarioFactChanges]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioFactChanges_Facts_FactId] FOREIGN KEY([FactId])
REFERENCES [dbo].[Facts] ([Id])
GO

ALTER TABLE [dbo].[ScenarioFactChanges] CHECK CONSTRAINT [FK_ScenarioFactChanges_Facts_FactId]
GO

ALTER TABLE [dbo].[ScenarioFactChanges]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioFactChanges_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioFactChanges] CHECK CONSTRAINT [FK_ScenarioFactChanges_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[ScenarioInputArtifacts]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioInputArtifacts_Artifacts_ArtifactId] FOREIGN KEY([ArtifactId])
REFERENCES [dbo].[Artifacts] ([Id])
GO

ALTER TABLE [dbo].[ScenarioInputArtifacts] CHECK CONSTRAINT [FK_ScenarioInputArtifacts_Artifacts_ArtifactId]
GO

ALTER TABLE [dbo].[ScenarioInputArtifacts]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioInputArtifacts_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioInputArtifacts] CHECK CONSTRAINT [FK_ScenarioInputArtifacts_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[ScenarioKartabls]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioKartabls_Kartabls_KartablId] FOREIGN KEY([KartablId])
REFERENCES [dbo].[Kartabls] ([Id])
GO

ALTER TABLE [dbo].[ScenarioKartabls] CHECK CONSTRAINT [FK_ScenarioKartabls_Kartabls_KartablId]
GO

ALTER TABLE [dbo].[ScenarioKartabls]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioKartabls_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioKartabls] CHECK CONSTRAINT [FK_ScenarioKartabls_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[ScenarioPreconditions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioPreconditions_Conditions_ConditionId] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Conditions] ([Id])
GO

ALTER TABLE [dbo].[ScenarioPreconditions] CHECK CONSTRAINT [FK_ScenarioPreconditions_Conditions_ConditionId]
GO

ALTER TABLE [dbo].[ScenarioPreconditions]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioPreconditions_Scenarios_ScenarioId] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[Scenarios] ([Id])
GO

ALTER TABLE [dbo].[ScenarioPreconditions] CHECK CONSTRAINT [FK_ScenarioPreconditions_Scenarios_ScenarioId]
GO

ALTER TABLE [dbo].[Scenarios]  WITH CHECK ADD  CONSTRAINT [FK_Scenarios_Stages_StageId] FOREIGN KEY([StageId])
REFERENCES [dbo].[Stages] ([Id])
GO

ALTER TABLE [dbo].[Scenarios] CHECK CONSTRAINT [FK_Scenarios_Stages_StageId]
GO

ALTER TABLE [dbo].[Stages]  WITH CHECK ADD  CONSTRAINT [FK_Stages_Processes_ProcessId] FOREIGN KEY([ProcessId])
REFERENCES [dbo].[Processes] ([Id])
GO

ALTER TABLE [dbo].[Stages] CHECK CONSTRAINT [FK_Stages_Processes_ProcessId]
GO

ALTER TABLE [dbo].[Stages]  WITH CHECK ADD  CONSTRAINT [FK_Stages_SubProcesses_SubProcessId] FOREIGN KEY([SubProcessId])
REFERENCES [dbo].[SubProcesses] ([Id])
GO

ALTER TABLE [dbo].[Stages] CHECK CONSTRAINT [FK_Stages_SubProcesses_SubProcessId]
GO

ALTER TABLE [dbo].[SubProcesses]  WITH CHECK ADD  CONSTRAINT [FK_SubProcesses_Processes_ProcessId] FOREIGN KEY([ProcessId])
REFERENCES [dbo].[Processes] ([Id])
GO

ALTER TABLE [dbo].[SubProcesses] CHECK CONSTRAINT [FK_SubProcesses_Processes_ProcessId]
GO

ALTER TABLE [dbo].[WorkItemActions]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemActions_Actions_ActionId] FOREIGN KEY([ActionId])
REFERENCES [dbo].[Actions] ([Id])
GO

ALTER TABLE [dbo].[WorkItemActions] CHECK CONSTRAINT [FK_WorkItemActions_Actions_ActionId]
GO

ALTER TABLE [dbo].[WorkItemActions]  WITH CHECK ADD  CONSTRAINT [FK_WorkItemActions_WorkItems_WorkItemId] FOREIGN KEY([WorkItemId])
REFERENCES [dbo].[WorkItems] ([Id])
GO

ALTER TABLE [dbo].[WorkItemActions] CHECK CONSTRAINT [FK_WorkItemActions_WorkItems_WorkItemId]
GO

ALTER TABLE [dbo].[WorkItems]  WITH CHECK ADD  CONSTRAINT [FK_WorkItems_Kartabls_CurrentKartablId] FOREIGN KEY([CurrentKartablId])
REFERENCES [dbo].[Kartabls] ([Id])
GO

ALTER TABLE [dbo].[WorkItems] CHECK CONSTRAINT [FK_WorkItems_Kartabls_CurrentKartablId]
GO

USE [master]
GO

ALTER DATABASE [Modeler05] SET  READ_WRITE
GO

USE [Modeler05]
GO
