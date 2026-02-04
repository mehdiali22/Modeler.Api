using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain; 
using System.Reflection.Emit;

namespace Modeler.Api.Persistence;

public sealed class ModelerDbContext : DbContext
{
    public ModelerDbContext(DbContextOptions<ModelerDbContext> options) : base(options) { }

    public DbSet<DictionaryTerm> DictionaryTerms => Set<DictionaryTerm>();

    public DbSet<Artifact> Artifacts => Set<Artifact>();
    public DbSet<Fact> Facts => Set<Fact>();
    public DbSet<FactEnumValue> FactEnumValues => Set<FactEnumValue>();

    public DbSet<Condition> Conditions => Set<Condition>();
    public DbSet<ConditionFactUsed> ConditionFactUsed => Set<ConditionFactUsed>();

    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Domain.Actions> Actions => Set<Domain.Actions>();

    public DbSet<Process> Processes => Set<Process>();
    public DbSet<SubProcess> SubProcesses => Set<SubProcess>();
    public DbSet<Stage> Stages => Set<Stage>();

    public DbSet<Scenario> Scenarios => Set<Scenario>();
    public DbSet<ScenarioPrecondition> ScenarioPreconditions => Set<ScenarioPrecondition>();
    public DbSet<ScenarioInputArtifact> ScenarioInputArtifacts => Set<ScenarioInputArtifact>();
    public DbSet<ScenarioFactChange> ScenarioFactChanges => Set<ScenarioFactChange>();

    public DbSet<ScenarioDecision> ScenarioDecisions => Set<ScenarioDecision>();
    public DbSet<ScenarioDecisionOption> ScenarioDecisionOptions => Set<ScenarioDecisionOption>();
    public DbSet<DecisionOptionFactChange> DecisionOptionFactChanges => Set<DecisionOptionFactChange>();

    public DbSet<TriggerDefinition> Triggers => Set<TriggerDefinition>();
    public DbSet<EventDefinition> Events => Set<EventDefinition>();
    public DbSet<EventTriggerLink> EventTriggerLinks => Set<EventTriggerLink>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // composite keys
        mb.Entity<ConditionFactUsed>().HasKey(x => new { x.ConditionId, x.FactId });
        mb.Entity<ScenarioPrecondition>().HasKey(x => new { x.ScenarioId, x.ConditionId });

        // Identity PKs (int)
        mb.Entity<DictionaryTerm>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Artifact>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Fact>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<FactEnumValue>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Condition>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Actor>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Domain.Actions>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Process>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<SubProcess>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Stage>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<Scenario>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<ScenarioInputArtifact>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<ScenarioFactChange>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<ScenarioDecision>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<ScenarioDecisionOption>().Property(x => x.Id).ValueGeneratedOnAdd();
        mb.Entity<DecisionOptionFactChange>().Property(x => x.Id).ValueGeneratedOnAdd();

        // uniques
        mb.Entity<DictionaryTerm>().HasIndex(x => x.TermKey).IsUnique();

        mb.Entity<Artifact>().HasIndex(x => x.ArtifactKey).IsUnique();
        mb.Entity<Fact>().HasIndex(x => x.FactKey).IsUnique();
        mb.Entity<FactEnumValue>().HasIndex(x => new { x.FactId, x.EnumKey }).IsUnique();

        mb.Entity<Condition>().HasIndex(x => x.ConditionKey).IsUnique();

        mb.Entity<Actor>().HasIndex(x => x.ActorKey).IsUnique();
        mb.Entity<Domain.Actions>().HasIndex(x => x.ActionKey).IsUnique();

        mb.Entity<Process>().HasIndex(x => x.ProcessKey).IsUnique();
        mb.Entity<Stage>().HasIndex(x => new { x.ProcessId, x.StageKey }).IsUnique();
        mb.Entity<SubProcess>().HasIndex(x => new { x.ProcessId, x.SubProcessKey }).IsUnique();

        mb.Entity<Scenario>().HasIndex(x => x.ScenarioKey).IsUnique();
        mb.Entity<ScenarioDecision>().HasIndex(x => new { x.ScenarioId, x.DecisionKey }).IsUnique();
        mb.Entity<ScenarioDecisionOption>().HasIndex(x => new { x.ScenarioDecisionId, x.OptionKey }).IsUnique();

        // relationships (Restrict to avoid multi cascade paths)
        mb.Entity<Fact>()
            .HasOne<Artifact>()
            .WithMany()
            .HasForeignKey(x => x.ArtifactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<FactEnumValue>()
            .HasOne<Fact>()
            .WithMany()
            .HasForeignKey(x => x.FactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ConditionFactUsed>()
            .HasOne<Condition>()
            .WithMany()
            .HasForeignKey(x => x.ConditionId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ConditionFactUsed>()
            .HasOne<Fact>()
            .WithMany()
            .HasForeignKey(x => x.FactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Domain.Actions>()
            .HasOne<Artifact>()
            .WithMany()
            .HasForeignKey(x => x.TargetArtifactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Domain.Actions>()
            .HasOne<Actor>()
            .WithMany()
            .HasForeignKey(x => x.ExecutorActorId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Stage>()
            .HasOne<Process>()
            .WithMany()
            .HasForeignKey(x => x.ProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<SubProcess>()
            .HasOne<Process>()
            .WithMany()
            .HasForeignKey(x => x.ProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Scenario>()
            .HasOne<Stage>()
            .WithMany()
            .HasForeignKey(x => x.StageId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioPrecondition>()
            .HasOne<Scenario>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioPrecondition>()
            .HasOne<Condition>()
            .WithMany()
            .HasForeignKey(x => x.ConditionId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioInputArtifact>()
            .HasOne<Scenario>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioInputArtifact>()
            .HasOne<Artifact>()
            .WithMany()
            .HasForeignKey(x => x.ArtifactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioFactChange>()
            .HasOne<Scenario>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioFactChange>()
            .HasOne<Fact>()
            .WithMany()
            .HasForeignKey(x => x.FactId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioDecision>()
            .HasOne<Scenario>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ScenarioDecisionOption>()
            .HasOne<ScenarioDecision>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioDecisionId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<DecisionOptionFactChange>()
            .HasOne<ScenarioDecisionOption>()
            .WithMany()
            .HasForeignKey(x => x.ScenarioDecisionOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<DecisionOptionFactChange>()
            .HasOne<Fact>()
            .WithMany()
            .HasForeignKey(x => x.FactId)
            .OnDelete(DeleteBehavior.Restrict);


        mb.Entity<TriggerDefinition>()
        .HasIndex(x => x.TriggerKey)
        .IsUnique();

        mb.Entity<EventDefinition>()
            .HasIndex(x => x.EventKey)
            .IsUnique();

        mb.Entity<EventTriggerLink>()
            .HasIndex(x => new { x.EventId, x.TriggerId })
            .IsUnique();

        mb.Entity<EventTriggerLink>()
            .HasOne(x => x.Event)
            .WithMany()
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<EventTriggerLink>()
            .HasOne(x => x.Trigger)
            .WithMany()
            .HasForeignKey(x => x.TriggerId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Scenario>()
        .HasOne(x => x.Trigger)
        .WithMany()
        .HasForeignKey(x => x.TriggerId)
        .OnDelete(DeleteBehavior.Restrict);

    }

    public override int SaveChanges()
    {
        TouchTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TouchTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void TouchTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var e in ChangeTracker.Entries<BaseEntity>())
        {
            if (e.State == EntityState.Added)
            {
                e.Entity.CreatedAtUtc = now;
                e.Entity.UpdatedAtUtc = now;
            }
            else if (e.State == EntityState.Modified)
            {
                e.Entity.UpdatedAtUtc = now;
            }
        }
    }
}
