using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    [Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(Modeler.Api.Persistence.ModelerDbContext))]
    [Migration("20260526203000_AddDaramadReviewDecisionRuntime")]
    public partial class AddDaramadReviewDecisionRuntime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @now datetime2 = SYSUTCDATETIME();

DECLARE @DaramadReviewStageId int = (SELECT Id FROM Stages WHERE StageKey=N'DaramadReviewStage');
DECLARE @DaramadKartabl int = (SELECT Id FROM Kartabls WHERE KartablKey=N'DaramadKartabl');
DECLARE @S_Open int = (SELECT Id FROM Scenarios WHERE ScenarioKey=N'OpenAdmAndAcceptWork');
DECLARE @S_Review int = (SELECT Id FROM Scenarios WHERE ScenarioKey=N'ReviewAdmByDaramadDecisionScenario');

IF @DaramadReviewStageId IS NULL
    THROW 51000, 'SeedDaramadAdmProcess must be applied before AddDaramadReviewDecisionRuntime. Missing Stage: DaramadReviewStage', 1;
IF @DaramadKartabl IS NULL
    THROW 51001, 'SeedDaramadAdmProcess must be applied before AddDaramadReviewDecisionRuntime. Missing Kartabl: DaramadKartabl', 1;

-- A real Scenario that contains the Daramad decision. This replaces the old visual/inferred decision node.
IF @S_Review IS NULL
BEGIN
    INSERT INTO Scenarios (ScenarioKey, TitleFa, Description, StageId, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        N'ReviewAdmByDaramadDecisionScenario',
        N'تصمیم درآمد روی پرونده',
        N'سناریوی تصمیم واقعی درآمد بعد از باز کردن/پذیرش فعالیت؛ گزینه‌ها شامل تایید، ابطال و انصراف از پذیرش هستند',
        @DaramadReviewStageId,
        N'Daramad',
        @now,
        @now
    );
END
SET @S_Review = (SELECT Id FROM Scenarios WHERE ScenarioKey=N'ReviewAdmByDaramadDecisionScenario');

IF NOT EXISTS (SELECT 1 FROM ScenarioKartabls WHERE ScenarioId=@S_Review AND KartablId=@DaramadKartabl)
    INSERT INTO ScenarioKartabls (ScenarioId, KartablId) VALUES (@S_Review, @DaramadKartabl);

DECLARE @C_InReview int = (SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsDarHaleResidegiDaramad');
IF @C_InReview IS NOT NULL AND NOT EXISTS (SELECT 1 FROM ScenarioPreconditions WHERE ScenarioId=@S_Review AND ConditionId=@C_InReview)
    INSERT INTO ScenarioPreconditions (ScenarioId, ConditionId) VALUES (@S_Review, @C_InReview);

-- The earlier seed created DaramadDecision under OpenAdmAndAcceptWork only for Mermaid metadata.
-- Remove that inferred/metadata decision so there is exactly one real DaramadDecision under the review scenario.
IF @S_Open IS NOT NULL
BEGIN
    DELETE dofc
    FROM DecisionOptionFactChanges dofc
    JOIN ScenarioDecisionOptions opt ON opt.Id = dofc.ScenarioDecisionOptionId
    JOIN ScenarioDecisions dec ON dec.Id = opt.ScenarioDecisionId
    WHERE dec.ScenarioId = @S_Open AND dec.DecisionKey = N'DaramadDecision';

    DELETE opt
    FROM ScenarioDecisionOptions opt
    JOIN ScenarioDecisions dec ON dec.Id = opt.ScenarioDecisionId
    WHERE dec.ScenarioId = @S_Open AND dec.DecisionKey = N'DaramadDecision';

    DELETE FROM ScenarioDecisions
    WHERE ScenarioId = @S_Open AND DecisionKey = N'DaramadDecision';
END

IF NOT EXISTS (SELECT 1 FROM ScenarioDecisions WHERE ScenarioId=@S_Review AND DecisionKey=N'DaramadDecision')
BEGIN
    INSERT INTO ScenarioDecisions (ScenarioId, DecisionKey, TitleFa, UiActionKey, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@S_Review, N'DaramadDecision', N'تصمیم درآمد روی پرونده', N'DaramadDecision', @now, @now);
END
DECLARE @DaramadDecisionId int = (SELECT Id FROM ScenarioDecisions WHERE ScenarioId=@S_Review AND DecisionKey=N'DaramadDecision');

DECLARE @A_Approve int = (SELECT Id FROM Actions WHERE ActionKey=N'ApproveAdmByDaramad');
DECLARE @A_Cancel int = (SELECT Id FROM Actions WHERE ActionKey=N'CancelAdmByDaramad');
DECLARE @A_Release int = (SELECT Id FROM Actions WHERE ActionKey=N'ReleaseAdmWorkByDaramad');

IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Approve')
    INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@DaramadDecisionId, N'Approve', N'تایید', NULL, CASE WHEN @A_Approve IS NULL THEN NULL ELSE N'[' + CAST(@A_Approve AS nvarchar(20)) + N']' END, @now, @now);
ELSE
    UPDATE ScenarioDecisionOptions SET TitleFa=N'تایید', ActionIdsJson=CASE WHEN @A_Approve IS NULL THEN ActionIdsJson ELSE N'[' + CAST(@A_Approve AS nvarchar(20)) + N']' END, UpdatedAtUtc=@now WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Approve';

IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Cancel')
    INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@DaramadDecisionId, N'Cancel', N'ابطال', NULL, CASE WHEN @A_Cancel IS NULL THEN NULL ELSE N'[' + CAST(@A_Cancel AS nvarchar(20)) + N']' END, @now, @now);
ELSE
    UPDATE ScenarioDecisionOptions SET TitleFa=N'ابطال', ActionIdsJson=CASE WHEN @A_Cancel IS NULL THEN ActionIdsJson ELSE N'[' + CAST(@A_Cancel AS nvarchar(20)) + N']' END, UpdatedAtUtc=@now WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Cancel';

IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Release')
    INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@DaramadDecisionId, N'Release', N'انصراف از پذیرش', NULL, CASE WHEN @A_Release IS NULL THEN NULL ELSE N'[' + CAST(@A_Release AS nvarchar(20)) + N']' END, @now, @now);
ELSE
    UPDATE ScenarioDecisionOptions SET TitleFa=N'انصراف از پذیرش', ActionIdsJson=CASE WHEN @A_Release IS NULL THEN ActionIdsJson ELSE N'[' + CAST(@A_Release AS nvarchar(20)) + N']' END, UpdatedAtUtc=@now WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Release';

DECLARE @O_Approve int = (SELECT Id FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Approve');
DECLARE @O_Cancel int = (SELECT Id FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Cancel');
DECLARE @O_Release int = (SELECT Id FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Release');

DECLARE @F_Asnadm int = (SELECT Id FROM Facts WHERE FactKey=N'Asnadm');
DECLARE @F_IsAccepted int = (SELECT Id FROM Facts WHERE FactKey=N'IsAccepted');
DECLARE @F_AcceptedByUserId int = (SELECT Id FROM Facts WHERE FactKey=N'AcceptedByUserId');
DECLARE @F_AcceptedByRole int = (SELECT Id FROM Facts WHERE FactKey=N'AcceptedByRole');
DECLARE @F_AcceptedAt int = (SELECT Id FROM Facts WHERE FactKey=N'AcceptedAt');

-- Option fact changes. Op: Set=1, Unset=2.
IF @F_Asnadm IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Approve AND FactId=@F_Asnadm)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Approve, @F_Asnadm, 1, 10, N'TaeidShodeTavasoteDaramad', @now, @now);

IF @F_Asnadm IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Cancel AND FactId=@F_Asnadm)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Cancel, @F_Asnadm, 1, 10, N'EbtalShodeTavasoteDaramad', @now, @now);
IF @F_IsAccepted IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Cancel AND FactId=@F_IsAccepted)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Cancel, @F_IsAccepted, 1, 20, N'false', @now, @now);

IF @F_Asnadm IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Release AND FactId=@F_Asnadm)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Release, @F_Asnadm, 1, 10, N'DaryaftShode', @now, @now);
IF @F_AcceptedByUserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Release AND FactId=@F_AcceptedByUserId)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Release, @F_AcceptedByUserId, 2, 20, NULL, @now, @now);
IF @F_AcceptedByRole IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Release AND FactId=@F_AcceptedByRole)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Release, @F_AcceptedByRole, 2, 30, NULL, @now, @now);
IF @F_IsAccepted IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Release AND FactId=@F_IsAccepted)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Release, @F_IsAccepted, 1, 40, N'false', @now, @now);
IF @F_AcceptedAt IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DecisionOptionFactChanges WHERE ScenarioDecisionOptionId=@O_Release AND FactId=@F_AcceptedAt)
    INSERT INTO DecisionOptionFactChanges (ScenarioDecisionOptionId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@O_Release, @F_AcceptedAt, 2, 50, NULL, @now, @now);
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @S_Review int = (SELECT Id FROM Scenarios WHERE ScenarioKey=N'ReviewAdmByDaramadDecisionScenario');
IF @S_Review IS NOT NULL
BEGIN
    DELETE dofc
    FROM DecisionOptionFactChanges dofc
    JOIN ScenarioDecisionOptions opt ON opt.Id = dofc.ScenarioDecisionOptionId
    JOIN ScenarioDecisions dec ON dec.Id = opt.ScenarioDecisionId
    WHERE dec.ScenarioId = @S_Review;

    DELETE opt
    FROM ScenarioDecisionOptions opt
    JOIN ScenarioDecisions dec ON dec.Id = opt.ScenarioDecisionId
    WHERE dec.ScenarioId = @S_Review;

    DELETE FROM ScenarioDecisions WHERE ScenarioId = @S_Review;
    DELETE FROM ScenarioPreconditions WHERE ScenarioId = @S_Review;
    DELETE FROM ScenarioKartabls WHERE ScenarioId = @S_Review;
    DELETE FROM Scenarios WHERE Id = @S_Review;
END
");
        }
    }
}
