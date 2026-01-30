using Modeler.Api.Domain;
using Modeler.Api.Dtos;

namespace Modeler.Api.Mappers;

public static class Map
{
    public static DictionaryTermDto ToDto(DictionaryTerm e) => new()
    {
        Id = e.Id,
        TermKey = e.TermKey,
        TitleFa = e.TitleFa,
        Description = e.Description
    };

    public static DictionaryTerm ToEntity(DictionaryTermDto d) => new()
    {
        Id = d.Id,
        TermKey = d.TermKey,
        TitleFa = d.TitleFa,
        Description = d.Description
    };

    public static ArtifactDto ToDto(Artifact e) => new()
    {
        Id = e.Id,
        ArtifactKey = e.ArtifactKey,
        TitleFa = e.TitleFa,
        Description = e.Description,
        IsChildOfCase = e.IsChildOfCase
    };

    public static Artifact ToEntity(ArtifactDto d) => new()
    {
        Id = d.Id,
        ArtifactKey = d.ArtifactKey,
        TitleFa = d.TitleFa,
        Description = d.Description,
        IsChildOfCase = d.IsChildOfCase
    };

    public static FactDto ToDto(Fact e) => new()
    {
        Id = e.Id,
        ArtifactId = e.ArtifactId,
        FactKey = e.FactKey,
        ValueType = e.ValueType,
        Meaning = e.Meaning
    };

    public static Fact ToEntity(FactDto d) => new()
    {
        Id = d.Id,
        ArtifactId = d.ArtifactId,
        FactKey = d.FactKey,
        ValueType = d.ValueType,
        Meaning = d.Meaning
    };

    public static FactEnumValueDto ToDto(FactEnumValue e) => new()
    {
        Id = e.Id,
        FactId = e.FactId,
        EnumKey = e.EnumKey,
        TitleFa = e.TitleFa,
        Value = e.Value
    };

    public static FactEnumValue ToEntity(FactEnumValueDto d) => new()
    {
        Id = d.Id,
        FactId = d.FactId,
        EnumKey = d.EnumKey,
        TitleFa = d.TitleFa,
        Value = d.Value
    };

    public static ConditionDto ToDto(Condition e) => new()
    {
        Id = e.Id,
        ConditionKey = e.ConditionKey,
        TitleFa = e.TitleFa,
        Expression = e.Expression,
        FailMessage = e.FailMessage
    };

    public static Condition ToEntity(ConditionDto d) => new()
    {
        Id = d.Id,
        ConditionKey = d.ConditionKey,
        TitleFa = d.TitleFa,
        Expression = d.Expression,
        FailMessage = d.FailMessage
    };

    public static ActorDto ToDto(Actor e) => new()
    {
        Id = e.Id,
        ActorKey = e.ActorKey,
        TitleFa = e.TitleFa,
        Kind = e.Kind,
        Description = e.Description
    };

    public static Actor ToEntity(ActorDto d) => new()
    {
        Id = d.Id,
        ActorKey = d.ActorKey,
        TitleFa = d.TitleFa,
        Kind = d.Kind,
        Description = d.Description
    };

    public static ActionCatalogDto ToDto(ActionCatalog e) => new()
    {
        Id = e.Id,
        ActionKey = e.ActionKey,
        TitleFa = e.TitleFa,
        TargetArtifactId = e.TargetArtifactId,
        ExecutorKind = e.ExecutorKind,
        ExecutorActorId = e.ExecutorActorId,
        Description = e.Description,
        DefaultParamsJson = e.DefaultParamsJson
    };

    public static ActionCatalog ToEntity(ActionCatalogDto d) => new()
    {
        Id = d.Id,
        ActionKey = d.ActionKey,
        TitleFa = d.TitleFa,
        TargetArtifactId = d.TargetArtifactId,
        ExecutorKind = d.ExecutorKind,
        ExecutorActorId = d.ExecutorActorId,
        Description = d.Description,
        DefaultParamsJson = d.DefaultParamsJson
    };

    public static ProcessDto ToDto(Process e) => new()
    {
        Id = e.Id,
        ProcessKey = e.ProcessKey,
        TitleFa = e.TitleFa,
        Description = e.Description,
        Order = e.Order
    };

    public static Process ToEntity(ProcessDto d) => new()
    {
        Id = d.Id,
        ProcessKey = d.ProcessKey,
        TitleFa = d.TitleFa,
        Description = d.Description,
        Order = d.Order
    };

    public static SubProcessDto ToDto(SubProcess e) => new()
    {
        Id = e.Id,
        ProcessId = e.ProcessId,
        SubProcessKey = e.SubProcessKey,
        TitleFa = e.TitleFa,
        Description = e.Description,
        Order = e.Order
    };

    public static SubProcess ToEntity(SubProcessDto d) => new()
    {
        Id = d.Id,
        ProcessId = d.ProcessId,
        SubProcessKey = d.SubProcessKey,
        TitleFa = d.TitleFa,
        Description = d.Description,
        Order = d.Order
    };

    public static StageDto ToDto(Stage e) => new()
    {
        Id = e.Id,
        ProcessId = e.ProcessId,
        StageKey = e.StageKey,
        TitleFa = e.TitleFa,
        Description = e.Description,
        Order = e.Order
    };

    public static Stage ToEntity(StageDto d) => new()
    {
        Id = d.Id,
        ProcessId = d.ProcessId,
        StageKey = d.StageKey,
        TitleFa = d.TitleFa,
        Description = d.Description,
        Order = d.Order
    };

    public static ScenarioDto ToDto(Scenario e) => new()
    {
        Id = e.Id,
        ScenarioKey = e.ScenarioKey,
        TitleFa = e.TitleFa,
        Description = e.Description,
        StageId = e.StageId,
        OwnerSubdomain = e.OwnerSubdomain
    };

    public static Scenario ToEntity(ScenarioDto d) => new()
    {
        Id = d.Id,
        ScenarioKey = d.ScenarioKey,
        TitleFa = d.TitleFa,
        Description = d.Description,
        StageId = d.StageId,
        OwnerSubdomain = d.OwnerSubdomain
    };

    public static ScenarioInputArtifactDto ToDto(ScenarioInputArtifact e) => new()
    {
        Id = e.Id,
        ScenarioId = e.ScenarioId,
        ArtifactId = e.ArtifactId,
        RoleKey = e.RoleKey
    };

    public static ScenarioInputArtifact ToEntity(ScenarioInputArtifactDto d) => new()
    {
        Id = d.Id,
        ScenarioId = d.ScenarioId,
        ArtifactId = d.ArtifactId,
        RoleKey = d.RoleKey
    };

    public static ScenarioFactChangeDto ToDto(ScenarioFactChange e) => new()
    {
        Id = e.Id,
        ScenarioId = e.ScenarioId,
        FactId = e.FactId,
        Op = e.Op,
        Value = e.Value
    };

    public static ScenarioFactChange ToEntity(ScenarioFactChangeDto d) => new()
    {
        Id = d.Id,
        ScenarioId = d.ScenarioId,
        FactId = d.FactId,
        Op = d.Op,
        Value = d.Value
    };

    public static ScenarioDecisionDto ToDto(ScenarioDecision e) => new()
    {
        Id = e.Id,
        ScenarioId = e.ScenarioId,
        DecisionKey = e.DecisionKey,
        TitleFa = e.TitleFa,
        UiActionKey = e.UiActionKey
    };

    public static ScenarioDecision ToEntity(ScenarioDecisionDto d) => new()
    {
        Id = d.Id,
        ScenarioId = d.ScenarioId,
        DecisionKey = d.DecisionKey,
        TitleFa = d.TitleFa,
        UiActionKey = d.UiActionKey
    };

    public static ScenarioDecisionOptionDto ToDto(ScenarioDecisionOption e) => new()
    {
        Id = e.Id,
        ScenarioDecisionId = e.ScenarioDecisionId,
        OptionKey = e.OptionKey,
        TitleFa = e.TitleFa,
        ConditionIdsJson = e.ConditionIdsJson,
        ActionIdsJson = e.ActionIdsJson,
        ProducedEventIdsJson = e.ProducedEventIdsJson
    };

    public static ScenarioDecisionOption ToEntity(ScenarioDecisionOptionDto d) => new()
    {
        Id = d.Id,
        ScenarioDecisionId = d.ScenarioDecisionId,
        OptionKey = d.OptionKey,
        TitleFa = d.TitleFa,
        ConditionIdsJson = d.ConditionIdsJson,
        ActionIdsJson = d.ActionIdsJson,
        ProducedEventIdsJson = d.ProducedEventIdsJson
    };

    public static DecisionOptionFactChangeDto ToDto(DecisionOptionFactChange e) => new()
    {
        Id = e.Id,
        ScenarioDecisionOptionId = e.ScenarioDecisionOptionId,
        FactId = e.FactId,
        Op = e.Op,
        Value = e.Value
    };

    public static DecisionOptionFactChange ToEntity(DecisionOptionFactChangeDto d) => new()
    {
        Id = d.Id,
        ScenarioDecisionOptionId = d.ScenarioDecisionOptionId,
        FactId = d.FactId,
        Op = d.Op,
        Value = d.Value
    };
}
