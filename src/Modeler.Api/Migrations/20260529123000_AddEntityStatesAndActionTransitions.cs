using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Modeler.Api.Persistence;

#nullable disable

namespace Modeler.Api.Migrations;

[DbContext(typeof(ModelerDbContext))]
[Migration("20260529123000_AddEntityStatesAndActionTransitions")]
public partial class AddEntityStatesAndActionTransitions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.EntityStates', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EntityStates](
        [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_EntityStates] PRIMARY KEY,
        [ArtifactId] int NOT NULL,
        [StateKey] nvarchar(150) NOT NULL,
        [TitleFa] nvarchar(300) NULL,
        [ConditionJson] nvarchar(max) NOT NULL CONSTRAINT [DF_EntityStates_ConditionJson] DEFAULT N'[]',
        [Description] nvarchar(1000) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NOT NULL
    );
    CREATE UNIQUE INDEX [IX_EntityStates_ArtifactId_StateKey] ON [dbo].[EntityStates]([ArtifactId],[StateKey]);
END;

IF OBJECT_ID(N'dbo.ActionStateTransitions', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ActionStateTransitions](
        [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ActionStateTransitions] PRIMARY KEY,
        [ScenarioId] int NULL,
        [ActionId] int NOT NULL,
        [FromStateId] int NULL,
        [ToStateId] int NULL,
        [DecisionId] int NULL,
        [DecisionOptionId] int NULL,
        [LabelFa] nvarchar(300) NULL,
        [SortOrder] int NOT NULL,
        [Description] nvarchar(1000) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NOT NULL
    );
    CREATE INDEX [IX_ActionStateTransitions_ScenarioId] ON [dbo].[ActionStateTransitions]([ScenarioId]);
    CREATE INDEX [IX_ActionStateTransitions_ActionId] ON [dbo].[ActionStateTransitions]([ActionId]);
    CREATE INDEX [IX_ActionStateTransitions_FromStateId] ON [dbo].[ActionStateTransitions]([FromStateId]);
    CREATE INDEX [IX_ActionStateTransitions_ToStateId] ON [dbo].[ActionStateTransitions]([ToStateId]);
END;
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.ActionStateTransitions', N'U') IS NOT NULL DROP TABLE [dbo].[ActionStateTransitions];
IF OBJECT_ID(N'dbo.EntityStates', N'U') IS NOT NULL DROP TABLE [dbo].[EntityStates];
");
    }
}
