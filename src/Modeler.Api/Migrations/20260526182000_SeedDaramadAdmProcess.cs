using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modeler.Api.Migrations
{
    [Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(Modeler.Api.Persistence.ModelerDbContext))]
    [Migration("20260526182000_SeedDaramadAdmProcess")]
    public partial class SeedDaramadAdmProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @now datetime2 = SYSUTCDATETIME();

-- Process
IF NOT EXISTS (SELECT 1 FROM Processes WHERE ProcessKey = N'RasaElectronicAdm')
BEGIN
    INSERT INTO Processes (ProcessKey, TitleFa, Description, [Order], CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'RasaElectronicAdm', N'فرایند رسیدگی پرونده الکترونیک رسا', N'فرایند دریافت پرونده الکترونیک از سپاس، آماده‌سازی، بررسی درآمد، و ارسال پرونده به مرحله بعد یا ابطال', 10, @now, @now);
END
DECLARE @ProcessId int = (SELECT Id FROM Processes WHERE ProcessKey = N'RasaElectronicAdm');

-- SubProcesses
IF NOT EXISTS (SELECT 1 FROM SubProcesses WHERE ProcessId=@ProcessId AND SubProcessKey=N'ReceiveAdmFromSepas')
BEGIN
    INSERT INTO SubProcesses (ProcessId, SubProcessKey, TitleFa, Description, [Order], CreatedAtUtc, UpdatedAtUtc)
    VALUES (@ProcessId, N'ReceiveAdmFromSepas', N'دریافت پرونده از سپاس', N'دریافت XML، پارس اطلاعات، محاسبه هشدارها و کسور اتومات', 10, @now, @now);
END
IF NOT EXISTS (SELECT 1 FROM SubProcesses WHERE ProcessId=@ProcessId AND SubProcessKey=N'DaramadHospitalReview')
BEGIN
    INSERT INTO SubProcesses (ProcessId, SubProcessKey, TitleFa, Description, [Order], CreatedAtUtc, UpdatedAtUtc)
    VALUES (@ProcessId, N'DaramadHospitalReview', N'بررسی درآمد بیمارستان', N'مشاهده پرونده توسط درآمد، باز کردن پرونده/پذیرش فعالیت، تایید، ابطال یا انصراف از پذیرش', 20, @now, @now);
END
DECLARE @SubReceiveId int = (SELECT Id FROM SubProcesses WHERE ProcessId=@ProcessId AND SubProcessKey=N'ReceiveAdmFromSepas');
DECLARE @SubDaramadId int = (SELECT Id FROM SubProcesses WHERE ProcessId=@ProcessId AND SubProcessKey=N'DaramadHospitalReview');

-- Stages
IF NOT EXISTS (SELECT 1 FROM Stages WHERE ProcessId=@ProcessId AND StageKey=N'AdmReceivedStage')
BEGIN
    INSERT INTO Stages (ProcessId, SubProcessId, StageKey, TitleFa, Description, [Order], CreatedAtUtc, UpdatedAtUtc)
    VALUES (@ProcessId, @SubReceiveId, N'AdmReceivedStage', N'دریافت و آماده‌سازی پرونده', N'پرونده از سپاس دریافت شده، XML پارس می‌شود و هشدارها/کسور اتومات محاسبه می‌شوند', 10, @now, @now);
END
IF NOT EXISTS (SELECT 1 FROM Stages WHERE ProcessId=@ProcessId AND StageKey=N'DaramadReviewStage')
BEGIN
    INSERT INTO Stages (ProcessId, SubProcessId, StageKey, TitleFa, Description, [Order], CreatedAtUtc, UpdatedAtUtc)
    VALUES (@ProcessId, @SubDaramadId, N'DaramadReviewStage', N'رسیدگی درآمد بیمارستان', N'درآمد پرونده را در کارتابل درآمد مشاهده می‌کند، باز می‌کند، تایید/ابطال یا انصراف از پذیرش انجام می‌دهد', 20, @now, @now);
END
DECLARE @AdmReceivedStageId int = (SELECT Id FROM Stages WHERE ProcessId=@ProcessId AND StageKey=N'AdmReceivedStage');
DECLARE @DaramadReviewStageId int = (SELECT Id FROM Stages WHERE ProcessId=@ProcessId AND StageKey=N'DaramadReviewStage');

-- Artifact
IF NOT EXISTS (SELECT 1 FROM Artifacts WHERE ArtifactKey=N'Adm')
BEGIN
    INSERT INTO Artifacts (ArtifactKey, TitleFa, Description, IsChildOfCase, CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'Adm', N'پرونده الکترونیک پذیرش', N'پرونده الکترونیکی دریافت‌شده از سپاس شامل اطلاعات بستری، گلوبال، اورژانس تحت نظر یا بستری موقت', 0, @now, @now);
END
DECLARE @AdmArtifactId int = (SELECT Id FROM Artifacts WHERE ArtifactKey=N'Adm');

-- Facts helper: ValueType => String=1, Int=2, Decimal=3, Bool=4, DateTime=5
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'Asnadm') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'Asnadm', 1, N'وضعیت پرونده پذیرش', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'CurrentKartablId') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'CurrentKartablId', 2, N'کارتابل فعلی', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AcceptedByUserId') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AcceptedByUserId', 1, N'کاربر درآمد پذیرنده پرونده', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AcceptedByRole') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AcceptedByRole', 1, N'نقش پذیرنده پرونده', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AcceptedAt') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AcceptedAt', 5, N'زمان پذیرش فعالیت', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'IsAccepted') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'IsAccepted', 4, N'آیا پرونده پذیرش فعالیت شده است', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AdmissionNo') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AdmissionNo', 1, N'شماره پذیرش', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'NationalCode') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'NationalCode', 1, N'کد ملی بیمار', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'PatientName') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'PatientName', 1, N'نام بیمار', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AdmType') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AdmType', 1, N'نوع پرونده', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AdmissionDate') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AdmissionDate', 5, N'تاریخ پذیرش', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'DischargeDate') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'DischargeDate', 5, N'تاریخ ترخیص', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'ReceivedDate') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'ReceivedDate', 5, N'تاریخ دریافت از سپاس', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasWarning', 4, N'دارای هشدار', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasLevelMismatchWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasLevelMismatchWarning', 4, N'هشدار مغایرت سطوح اطلاعاتی', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasDateMismatchWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasDateMismatchWarning', 4, N'هشدار مغایرت تاریخی', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasTitekWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasTitekWarning', 4, N'هشدار تیتک', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasPharmacopoeiaWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasPharmacopoeiaWarning', 4, N'هشدار فارماکوپه', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasEligibilityWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasEligibilityWarning', 4, N'هشدار استحقاق', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasHotelingWarning') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasHotelingWarning', 4, N'هشدار هتلینگ', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasAutoDeduction') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasAutoDeduction', 4, N'دارای کسر اتومات', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'TotalAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'TotalAmount', 3, N'مبلغ کل پرونده', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'CoveredAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'CoveredAmount', 3, N'مبلغ در تعهد', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'GovernmentSubsidy') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'GovernmentSubsidy', 3, N'یارانه دولت', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'PatientShare') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'PatientShare', 3, N'سهم بیمار', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'InsuranceShare') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'InsuranceShare', 3, N'سهم بیمه', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'AutoDeductionAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'AutoDeductionAmount', 3, N'مبلغ کسر اتومات', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'CurrencySubsidyAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'CurrencySubsidyAmount', 3, N'یارانه ارزی', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'CurrencySubsidyDeduction') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'CurrencySubsidyDeduction', 3, N'کسر یارانه ارزی', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'ApprovedAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'ApprovedAmount', 3, N'مبلغ مورد تایید', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'NursingAmount') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'NursingAmount', 3, N'مبلغ پرستاری', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'PayableOrganizationShare') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'PayableOrganizationShare', 3, N'سهم سازمان قابل پرداخت', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Facts WHERE FactKey=N'HasMultipleMoghavem') INSERT INTO Facts (ArtifactId, FactKey, ValueType, Meaning, CreatedAtUtc, UpdatedAtUtc) VALUES (@AdmArtifactId, N'HasMultipleMoghavem', 4, N'بیمارستان چند مقومه است', @now, @now);

-- Roles are stored in the existing Actors table for now.
IF NOT EXISTS (SELECT 1 FROM Actors WHERE ActorKey=N'Daramad') INSERT INTO Actors (ActorKey, TitleFa, Kind, Description, CreatedAtUtc, UpdatedAtUtc) VALUES (N'Daramad', N'درآمد', 2, N'نقش درآمد بیمارستان', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actors WHERE ActorKey=N'RasaSystem') INSERT INTO Actors (ActorKey, TitleFa, Kind, Description, CreatedAtUtc, UpdatedAtUtc) VALUES (N'RasaSystem', N'سامانه رسا', 1, N'نقش سیستمی سامانه رسا', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actors WHERE ActorKey=N'TosiKonandeh') INSERT INTO Actors (ActorKey, TitleFa, Kind, Description, CreatedAtUtc, UpdatedAtUtc) VALUES (N'TosiKonandeh', N'توزیع‌کننده', 2, N'نقش توزیع‌کننده مقوم', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actors WHERE ActorKey=N'Moghavem') INSERT INTO Actors (ActorKey, TitleFa, Kind, Description, CreatedAtUtc, UpdatedAtUtc) VALUES (N'Moghavem', N'مقوم', 2, N'نقش مقوم بیمه‌گر', @now, @now);
DECLARE @RoleDaramad int = (SELECT Id FROM Actors WHERE ActorKey=N'Daramad');
DECLARE @RoleRasaSystem int = (SELECT Id FROM Actors WHERE ActorKey=N'RasaSystem');

-- Actions
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'ParseSepasXml') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'ParseSepasXml', N'پارس XML دریافتی از سپاس', @AdmArtifactId, 1, @RoleRasaSystem, N'پارس XML پرونده الکترونیک دریافت‌شده از سپاس', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'CalculateWarnings') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'CalculateWarnings', N'محاسبه هشدارهای پرونده', @AdmArtifactId, 1, @RoleRasaSystem, N'محاسبه هشدارهای سطحی، تاریخی، تیتک، فارماکوپه، استحقاق و هتلینگ', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'CalculateAutoDeductions') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'CalculateAutoDeductions', N'محاسبه کسور اتومات', @AdmArtifactId, 1, @RoleRasaSystem, N'محاسبه کسور اتومات و کسر یارانه ارزی', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'OpenAdmAndAcceptWork') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'OpenAdmAndAcceptWork', N'باز کردن پرونده و پذیرش انجام فعالیت', @AdmArtifactId, 2, @RoleDaramad, N'باز کردن پرونده در کارتابل درآمد و قفل کردن آن برای همان کاربر درآمد', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'ApproveAdmByDaramad') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'ApproveAdmByDaramad', N'تایید پرونده توسط درآمد', @AdmArtifactId, 2, @RoleDaramad, N'تایید پرونده توسط نقش درآمد و ارسال به مرحله بعد', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'CancelAdmByDaramad') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'CancelAdmByDaramad', N'ابطال پرونده توسط درآمد', @AdmArtifactId, 2, @RoleDaramad, N'ابطال پرونده توسط درآمد؛ پرونده باید در صورت نیاز مجدداً ارسال شود', N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Actions WHERE ActionKey=N'ReleaseAdmWorkByDaramad') INSERT INTO Actions (ActionKey, TitleFa, TargetArtifactId, ExecutorKind, ExecutorActorId, Description, DefaultParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (N'ReleaseAdmWorkByDaramad', N'انصراف از پذیرش فعالیت', @AdmArtifactId, 2, @RoleDaramad, N'برداشتن قفل پذیرش و قابل مشاهده شدن پرونده برای درآمدهای دیگر', N'{}', @now, @now);

-- Kartabls
IF NOT EXISTS (SELECT 1 FROM Kartabls WHERE KartablKey=N'DaramadKartabl') INSERT INTO Kartabls (KartablKey, TitleFa, Description, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'DaramadKartabl', N'کارتابل درآمد', N'فقط پرونده‌هایی را نمایش می‌دهد که هیچ درآمدی پذیرش نکرده یا خود همین کاربر درآمد پذیرش کرده است', N'Daramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Kartabls WHERE KartablKey=N'TosiKonandehKartabl') INSERT INTO Kartabls (KartablKey, TitleFa, Description, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'TosiKonandehKartabl', N'کارتابل توزیع‌کننده', N'مقصد پرونده‌های تاییدشده درآمد برای بیمارستان چندمقومه', N'TosiKonandeh', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Kartabls WHERE KartablKey=N'MoghavemKartabl') INSERT INTO Kartabls (KartablKey, TitleFa, Description, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'MoghavemKartabl', N'کارتابل مقوم', N'مقصد پرونده‌های تاییدشده درآمد برای بیمارستان تک‌مقومه یا بعد از توزیع', N'Moghavem', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Kartabls WHERE KartablKey=N'AdmClosed') INSERT INTO Kartabls (KartablKey, TitleFa, Description, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmClosed', N'پرونده‌های ابطال‌شده / خارج از چرخه', N'پرونده‌هایی که توسط درآمد ابطال یا از چرخه جاری خارج شده‌اند', N'System', @now, @now);
DECLARE @DaramadKartabl int = (SELECT Id FROM Kartabls WHERE KartablKey=N'DaramadKartabl');
DECLARE @TosiKartabl int = (SELECT Id FROM Kartabls WHERE KartablKey=N'TosiKonandehKartabl');
DECLARE @MoghavemKartabl int = (SELECT Id FROM Kartabls WHERE KartablKey=N'MoghavemKartabl');
DECLARE @AdmClosedKartabl int = (SELECT Id FROM Kartabls WHERE KartablKey=N'AdmClosed');

-- Conditions
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsDaryaftShode') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsDaryaftShode', N'پرونده دریافت‌شده است', N'Asnadm == ""DaryaftShode""', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsOdatShodeBeDaramad') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsOdatShodeBeDaramad', N'پرونده عودت‌شده به درآمد است', N'Asnadm == ""OdatShodeBeDaramad""', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmCanBeOpenedByDaramad') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmCanBeOpenedByDaramad', N'پرونده برای باز شدن در درآمد آماده است', N'(Asnadm == ""DaryaftShode"" || Asnadm == ""OdatShodeBeDaramad"")', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsDarHaleResidegiDaramad') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsDarHaleResidegiDaramad', N'پرونده در حال رسیدگی درآمد است', N'Asnadm == ""DarHaleResidegiDaramad""', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsAcceptedByNoOne') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsAcceptedByNoOne', N'پرونده توسط هیچ درآمدی پذیرش نشده است', N'AcceptedByUserId == null', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsAcceptedByCurrentUser') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsAcceptedByCurrentUser', N'پرونده توسط همین کاربر درآمد پذیرش شده است', N'AcceptedByUserId == CurrentUserId', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmVisibleInDaramadKartabl') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmVisibleInDaramadKartabl', N'پرونده برای کاربر درآمد قابل نمایش است', N'AcceptedByUserId == null || AcceptedByUserId == CurrentUserId', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmHasMultipleMoghavem') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmHasMultipleMoghavem', N'بیمارستان چند مقومه است', N'HasMultipleMoghavem == true', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmHasSingleMoghavem') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmHasSingleMoghavem', N'بیمارستان تک مقومه است', N'HasMultipleMoghavem == false', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsApprovedByDaramad') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsApprovedByDaramad', N'پرونده توسط درآمد تایید شده است', N'Asnadm == ""TaeidShodeTavasoteDaramad""', NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM Conditions WHERE ConditionKey=N'AdmIsCanceledByDaramad') INSERT INTO Conditions (ConditionKey, TitleFa, Expression, FailMessage, CreatedAtUtc, UpdatedAtUtc) VALUES (N'AdmIsCanceledByDaramad', N'پرونده توسط درآمد ابطال شده است', N'Asnadm == ""EbtalShodeTavasoteDaramad""', NULL, @now, @now);

-- ConditionFactUsed links
DECLARE @F_Asnadm int=(SELECT Id FROM Facts WHERE FactKey=N'Asnadm');
DECLARE @F_AcceptedByUserId int=(SELECT Id FROM Facts WHERE FactKey=N'AcceptedByUserId');
DECLARE @F_HasMultiple int=(SELECT Id FROM Facts WHERE FactKey=N'HasMultipleMoghavem');
DECLARE @C_AdmIsDaryaft int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsDaryaftShode');
DECLARE @C_AdmIsOdat int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsOdatShodeBeDaramad');
DECLARE @C_CanOpen int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmCanBeOpenedByDaramad');
DECLARE @C_InReview int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsDarHaleResidegiDaramad');
DECLARE @C_NoOne int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsAcceptedByNoOne');
DECLARE @C_CurrentUser int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsAcceptedByCurrentUser');
DECLARE @C_Visible int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmVisibleInDaramadKartabl');
DECLARE @C_Multi int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmHasMultipleMoghavem');
DECLARE @C_Single int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmHasSingleMoghavem');
DECLARE @C_Approved int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsApprovedByDaramad');
DECLARE @C_Canceled int=(SELECT Id FROM Conditions WHERE ConditionKey=N'AdmIsCanceledByDaramad');
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_AdmIsDaryaft AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_AdmIsDaryaft, @F_Asnadm);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_AdmIsOdat AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_AdmIsOdat, @F_Asnadm);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_CanOpen AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_CanOpen, @F_Asnadm);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_InReview AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_InReview, @F_Asnadm);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_NoOne AND FactId=@F_AcceptedByUserId) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_NoOne, @F_AcceptedByUserId);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_CurrentUser AND FactId=@F_AcceptedByUserId) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_CurrentUser, @F_AcceptedByUserId);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_Visible AND FactId=@F_AcceptedByUserId) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_Visible, @F_AcceptedByUserId);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_Multi AND FactId=@F_HasMultiple) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_Multi, @F_HasMultiple);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_Single AND FactId=@F_HasMultiple) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_Single, @F_HasMultiple);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_Approved AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_Approved, @F_Asnadm);
IF NOT EXISTS (SELECT 1 FROM ConditionFactUsed WHERE ConditionId=@C_Canceled AND FactId=@F_Asnadm) INSERT INTO ConditionFactUsed (ConditionId, FactId) VALUES (@C_Canceled, @F_Asnadm);

-- Scenarios
IF NOT EXISTS (SELECT 1 FROM Scenarios WHERE ScenarioKey=N'OpenAdmAndAcceptWork') INSERT INTO Scenarios (ScenarioKey, TitleFa, Description, StageId, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'OpenAdmAndAcceptWork', N'باز کردن پرونده و پذیرش انجام فعالیت', N'باز کردن پرونده در کارتابل درآمد؛ پرونده برای همان کاربر درآمد قفل می‌شود و کارتابل تغییر نمی‌کند', @DaramadReviewStageId, N'Daramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Scenarios WHERE ScenarioKey=N'ApproveAdmByDaramadScenario') INSERT INTO Scenarios (ScenarioKey, TitleFa, Description, StageId, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'ApproveAdmByDaramadScenario', N'تایید پرونده توسط درآمد', N'تایید پرونده توسط درآمد و آماده شدن برای خروج از کارتابل درآمد', @DaramadReviewStageId, N'Daramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Scenarios WHERE ScenarioKey=N'CancelAdmByDaramadScenario') INSERT INTO Scenarios (ScenarioKey, TitleFa, Description, StageId, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'CancelAdmByDaramadScenario', N'ابطال پرونده توسط درآمد', N'ابطال پرونده توسط درآمد و خروج از چرخه جاری', @DaramadReviewStageId, N'Daramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM Scenarios WHERE ScenarioKey=N'ReleaseAdmWorkByDaramadScenario') INSERT INTO Scenarios (ScenarioKey, TitleFa, Description, StageId, OwnerSubdomain, CreatedAtUtc, UpdatedAtUtc) VALUES (N'ReleaseAdmWorkByDaramadScenario', N'انصراف از پذیرش فعالیت', N'برداشتن قفل پرونده و قابل مشاهده شدن آن برای درآمدهای دیگر، بدون تغییر کارتابل', @DaramadReviewStageId, N'Daramad', @now, @now);
DECLARE @S_Open int=(SELECT Id FROM Scenarios WHERE ScenarioKey=N'OpenAdmAndAcceptWork');
DECLARE @S_Approve int=(SELECT Id FROM Scenarios WHERE ScenarioKey=N'ApproveAdmByDaramadScenario');
DECLARE @S_Cancel int=(SELECT Id FROM Scenarios WHERE ScenarioKey=N'CancelAdmByDaramadScenario');
DECLARE @S_Release int=(SELECT Id FROM Scenarios WHERE ScenarioKey=N'ReleaseAdmWorkByDaramadScenario');

-- Scenario availability in DaramadKartabl
IF NOT EXISTS (SELECT 1 FROM ScenarioKartabls WHERE ScenarioId=@S_Open AND KartablId=@DaramadKartabl) INSERT INTO ScenarioKartabls (ScenarioId, KartablId) VALUES (@S_Open, @DaramadKartabl);
IF NOT EXISTS (SELECT 1 FROM ScenarioKartabls WHERE ScenarioId=@S_Approve AND KartablId=@DaramadKartabl) INSERT INTO ScenarioKartabls (ScenarioId, KartablId) VALUES (@S_Approve, @DaramadKartabl);
IF NOT EXISTS (SELECT 1 FROM ScenarioKartabls WHERE ScenarioId=@S_Cancel AND KartablId=@DaramadKartabl) INSERT INTO ScenarioKartabls (ScenarioId, KartablId) VALUES (@S_Cancel, @DaramadKartabl);
IF NOT EXISTS (SELECT 1 FROM ScenarioKartabls WHERE ScenarioId=@S_Release AND KartablId=@DaramadKartabl) INSERT INTO ScenarioKartabls (ScenarioId, KartablId) VALUES (@S_Release, @DaramadKartabl);

-- Preconditions: keep them runtime-safe (no CurrentUserId requirement in API yet)
IF NOT EXISTS (SELECT 1 FROM ScenarioPreconditions WHERE ScenarioId=@S_Open AND ConditionId=@C_CanOpen) INSERT INTO ScenarioPreconditions (ScenarioId, ConditionId) VALUES (@S_Open, @C_CanOpen);
IF NOT EXISTS (SELECT 1 FROM ScenarioPreconditions WHERE ScenarioId=@S_Approve AND ConditionId=@C_InReview) INSERT INTO ScenarioPreconditions (ScenarioId, ConditionId) VALUES (@S_Approve, @C_InReview);
IF NOT EXISTS (SELECT 1 FROM ScenarioPreconditions WHERE ScenarioId=@S_Cancel AND ConditionId=@C_InReview) INSERT INTO ScenarioPreconditions (ScenarioId, ConditionId) VALUES (@S_Cancel, @C_InReview);
IF NOT EXISTS (SELECT 1 FROM ScenarioPreconditions WHERE ScenarioId=@S_Release AND ConditionId=@C_InReview) INSERT INTO ScenarioPreconditions (ScenarioId, ConditionId) VALUES (@S_Release, @C_InReview);

-- Scenario actions
DECLARE @A_Open int=(SELECT Id FROM Actions WHERE ActionKey=N'OpenAdmAndAcceptWork');
DECLARE @A_Approve int=(SELECT Id FROM Actions WHERE ActionKey=N'ApproveAdmByDaramad');
DECLARE @A_Cancel int=(SELECT Id FROM Actions WHERE ActionKey=N'CancelAdmByDaramad');
DECLARE @A_Release int=(SELECT Id FROM Actions WHERE ActionKey=N'ReleaseAdmWorkByDaramad');
IF NOT EXISTS (SELECT 1 FROM ScenarioActions WHERE ScenarioId=@S_Open AND ActionId=@A_Open) INSERT INTO ScenarioActions (ScenarioId, ActionId, ParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @A_Open, N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioActions WHERE ScenarioId=@S_Approve AND ActionId=@A_Approve) INSERT INTO ScenarioActions (ScenarioId, ActionId, ParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Approve, @A_Approve, N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioActions WHERE ScenarioId=@S_Cancel AND ActionId=@A_Cancel) INSERT INTO ScenarioActions (ScenarioId, ActionId, ParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Cancel, @A_Cancel, N'{}', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioActions WHERE ScenarioId=@S_Release AND ActionId=@A_Release) INSERT INTO ScenarioActions (ScenarioId, ActionId, ParamsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @A_Release, N'{}', @now, @now);

-- Scenario fact changes. Op => Set=1, Unset=2
DECLARE @F_IsAccepted int=(SELECT Id FROM Facts WHERE FactKey=N'IsAccepted');
DECLARE @F_AcceptedByRole int=(SELECT Id FROM Facts WHERE FactKey=N'AcceptedByRole');
DECLARE @F_AcceptedAt int=(SELECT Id FROM Facts WHERE FactKey=N'AcceptedAt');
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Open AND FactId=@F_Asnadm AND Value=N'DarHaleResidegiDaramad') INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @F_Asnadm, 1, 10, N'DarHaleResidegiDaramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Open AND FactId=@F_AcceptedByUserId) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @F_AcceptedByUserId, 1, 20, N'CurrentUserId', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Open AND FactId=@F_AcceptedByRole) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @F_AcceptedByRole, 1, 30, N'Daramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Open AND FactId=@F_IsAccepted) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @F_IsAccepted, 1, 40, N'true', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Open AND FactId=@F_AcceptedAt) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Open, @F_AcceptedAt, 1, 50, N'Now', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Approve AND FactId=@F_Asnadm) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Approve, @F_Asnadm, 1, 10, N'TaeidShodeTavasoteDaramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Cancel AND FactId=@F_Asnadm) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Cancel, @F_Asnadm, 1, 10, N'EbtalShodeTavasoteDaramad', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Cancel AND FactId=@F_IsAccepted) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Cancel, @F_IsAccepted, 1, 20, N'false', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Release AND FactId=@F_Asnadm) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @F_Asnadm, 1, 10, N'DaryaftShode', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Release AND FactId=@F_AcceptedByUserId) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @F_AcceptedByUserId, 2, 20, NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Release AND FactId=@F_AcceptedByRole) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @F_AcceptedByRole, 2, 30, NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Release AND FactId=@F_IsAccepted) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @F_IsAccepted, 1, 40, N'false', @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioFactChanges WHERE ScenarioId=@S_Release AND FactId=@F_AcceptedAt) INSERT INTO ScenarioFactChanges (ScenarioId, FactId, Op, SortOrder, Value, CreatedAtUtc, UpdatedAtUtc) VALUES (@S_Release, @F_AcceptedAt, 2, 50, NULL, @now, @now);

-- Decision metadata for UI/Mermaid understanding
IF NOT EXISTS (SELECT 1 FROM ScenarioDecisions WHERE ScenarioId=@S_Open AND DecisionKey=N'DaramadDecision')
BEGIN
    INSERT INTO ScenarioDecisions (ScenarioId, DecisionKey, TitleFa, UiActionKey, CreatedAtUtc, UpdatedAtUtc)
    VALUES (@S_Open, N'DaramadDecision', N'تصمیم درآمد روی پرونده', N'DaramadDecision', @now, @now);
END
DECLARE @DaramadDecisionId int = (SELECT Id FROM ScenarioDecisions WHERE ScenarioId=@S_Open AND DecisionKey=N'DaramadDecision');
IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Approve') INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@DaramadDecisionId, N'Approve', N'تایید', NULL, NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Cancel') INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@DaramadDecisionId, N'Cancel', N'ابطال', NULL, NULL, @now, @now);
IF NOT EXISTS (SELECT 1 FROM ScenarioDecisionOptions WHERE ScenarioDecisionId=@DaramadDecisionId AND OptionKey=N'Release') INSERT INTO ScenarioDecisionOptions (ScenarioDecisionId, OptionKey, TitleFa, ConditionIdsJson, ActionIdsJson, CreatedAtUtc, UpdatedAtUtc) VALUES (@DaramadDecisionId, N'Release', N'انصراف از پذیرش', NULL, NULL, @now, @now);

-- Routing rules: acceptance/release do not route; approval/cancel do.
IF NOT EXISTS (SELECT 1 FROM KartablRoutingRules WHERE RuleKey=N'Route_DaramadApproved_To_TosiKonandeh')
BEGIN
    INSERT INTO KartablRoutingRules (RuleKey, OwnerSubdomain, Priority, FromKartablId, TargetKartablId, ConditionIdsJson, TitleFa, Description, CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'Route_DaramadApproved_To_TosiKonandeh', N'Daramad', 10, @DaramadKartabl, @TosiKartabl, N'[' + CAST(@C_Approved AS nvarchar(20)) + N',' + CAST(@C_Multi AS nvarchar(20)) + N']', N'تایید درآمد و چندمقومه؛ ارسال به توزیع‌کننده', N'اگر پرونده توسط درآمد تایید شده و بیمارستان چند مقومه باشد، پرونده به کارتابل توزیع‌کننده می‌رود', @now, @now);
END
IF NOT EXISTS (SELECT 1 FROM KartablRoutingRules WHERE RuleKey=N'Route_DaramadApproved_To_Moghavem')
BEGIN
    INSERT INTO KartablRoutingRules (RuleKey, OwnerSubdomain, Priority, FromKartablId, TargetKartablId, ConditionIdsJson, TitleFa, Description, CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'Route_DaramadApproved_To_Moghavem', N'Daramad', 20, @DaramadKartabl, @MoghavemKartabl, N'[' + CAST(@C_Approved AS nvarchar(20)) + N',' + CAST(@C_Single AS nvarchar(20)) + N']', N'تایید درآمد و تک‌مقومه؛ ارسال به مقوم', N'اگر پرونده توسط درآمد تایید شده و بیمارستان تک مقومه باشد، پرونده مستقیم به کارتابل مقوم می‌رود', @now, @now);
END
IF NOT EXISTS (SELECT 1 FROM KartablRoutingRules WHERE RuleKey=N'Route_DaramadCanceled_To_AdmClosed')
BEGIN
    INSERT INTO KartablRoutingRules (RuleKey, OwnerSubdomain, Priority, FromKartablId, TargetKartablId, ConditionIdsJson, TitleFa, Description, CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'Route_DaramadCanceled_To_AdmClosed', N'Daramad', 30, @DaramadKartabl, @AdmClosedKartabl, N'[' + CAST(@C_Canceled AS nvarchar(20)) + N']', N'ابطال درآمد؛ خروج از چرخه', N'اگر درآمد پرونده را ابطال کند، پرونده از چرخه جاری خارج می‌شود', @now, @now);
END
IF NOT EXISTS (SELECT 1 FROM KartablRoutingRules WHERE RuleKey=N'Route_MoghavemReturned_To_DaramadKartabl')
BEGIN
    INSERT INTO KartablRoutingRules (RuleKey, OwnerSubdomain, Priority, FromKartablId, TargetKartablId, ConditionIdsJson, TitleFa, Description, CreatedAtUtc, UpdatedAtUtc)
    VALUES (N'Route_MoghavemReturned_To_DaramadKartabl', N'Moghavem', 40, @MoghavemKartabl, @DaramadKartabl, N'[' + CAST(@C_AdmIsOdat AS nvarchar(20)) + N']', N'عودت مقوم به کارتابل درآمد', N'اگر مقوم پرونده را به درآمد عودت دهد، پرونده دوباره در کارتابل درآمد قرار می‌گیرد', @now, @now);
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- Delete seeded runtime model in dependency order.
DELETE FROM KartablRoutingRules WHERE RuleKey IN (N'Route_DaramadApproved_To_TosiKonandeh', N'Route_DaramadApproved_To_Moghavem', N'Route_DaramadCanceled_To_AdmClosed', N'Route_MoghavemReturned_To_DaramadKartabl');

DELETE dofc
FROM DecisionOptionFactChanges dofc
JOIN ScenarioDecisionOptions o ON o.Id = dofc.ScenarioDecisionOptionId
JOIN ScenarioDecisions d ON d.Id = o.ScenarioDecisionId
JOIN Scenarios s ON s.Id = d.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE o
FROM ScenarioDecisionOptions o
JOIN ScenarioDecisions d ON d.Id = o.ScenarioDecisionId
JOIN Scenarios s ON s.Id = d.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE d
FROM ScenarioDecisions d
JOIN Scenarios s ON s.Id = d.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE sa
FROM ScenarioActions sa
JOIN Scenarios s ON s.Id = sa.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE sfc
FROM ScenarioFactChanges sfc
JOIN Scenarios s ON s.Id = sfc.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE sp
FROM ScenarioPreconditions sp
JOIN Scenarios s ON s.Id = sp.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE sk
FROM ScenarioKartabls sk
JOIN Scenarios s ON s.Id = sk.ScenarioId
WHERE s.ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE FROM Scenarios WHERE ScenarioKey IN (N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramadScenario', N'CancelAdmByDaramadScenario', N'ReleaseAdmWorkByDaramadScenario');

DELETE FROM Actions WHERE ActionKey IN (N'ParseSepasXml', N'CalculateWarnings', N'CalculateAutoDeductions', N'OpenAdmAndAcceptWork', N'ApproveAdmByDaramad', N'CancelAdmByDaramad', N'ReleaseAdmWorkByDaramad');

DELETE FROM ConditionFactUsed WHERE ConditionId IN (SELECT Id FROM Conditions WHERE ConditionKey IN (N'AdmIsDaryaftShode', N'AdmIsOdatShodeBeDaramad', N'AdmCanBeOpenedByDaramad', N'AdmIsDarHaleResidegiDaramad', N'AdmIsAcceptedByNoOne', N'AdmIsAcceptedByCurrentUser', N'AdmVisibleInDaramadKartabl', N'AdmHasMultipleMoghavem', N'AdmHasSingleMoghavem', N'AdmIsApprovedByDaramad', N'AdmIsCanceledByDaramad'));
DELETE FROM Conditions WHERE ConditionKey IN (N'AdmIsDaryaftShode', N'AdmIsOdatShodeBeDaramad', N'AdmCanBeOpenedByDaramad', N'AdmIsDarHaleResidegiDaramad', N'AdmIsAcceptedByNoOne', N'AdmIsAcceptedByCurrentUser', N'AdmVisibleInDaramadKartabl', N'AdmHasMultipleMoghavem', N'AdmHasSingleMoghavem', N'AdmIsApprovedByDaramad', N'AdmIsCanceledByDaramad');

DELETE FROM Kartabls WHERE KartablKey IN (N'DaramadKartabl', N'TosiKonandehKartabl', N'MoghavemKartabl', N'AdmClosed');
DELETE FROM Actors WHERE ActorKey IN (N'Daramad', N'RasaSystem', N'TosiKonandeh', N'Moghavem');

DELETE FROM Facts WHERE FactKey IN (N'Asnadm', N'CurrentKartablId', N'AcceptedByUserId', N'AcceptedByRole', N'AcceptedAt', N'IsAccepted', N'AdmissionNo', N'NationalCode', N'PatientName', N'AdmType', N'AdmissionDate', N'DischargeDate', N'ReceivedDate', N'HasWarning', N'HasLevelMismatchWarning', N'HasDateMismatchWarning', N'HasTitekWarning', N'HasPharmacopoeiaWarning', N'HasEligibilityWarning', N'HasHotelingWarning', N'HasAutoDeduction', N'TotalAmount', N'CoveredAmount', N'GovernmentSubsidy', N'PatientShare', N'InsuranceShare', N'AutoDeductionAmount', N'CurrencySubsidyAmount', N'CurrencySubsidyDeduction', N'ApprovedAmount', N'NursingAmount', N'PayableOrganizationShare', N'HasMultipleMoghavem');
DELETE FROM Artifacts WHERE ArtifactKey = N'Adm';

DELETE FROM Stages WHERE StageKey IN (N'AdmReceivedStage', N'DaramadReviewStage') AND ProcessId IN (SELECT Id FROM Processes WHERE ProcessKey=N'RasaElectronicAdm');
DELETE FROM SubProcesses WHERE SubProcessKey IN (N'ReceiveAdmFromSepas', N'DaramadHospitalReview') AND ProcessId IN (SELECT Id FROM Processes WHERE ProcessKey=N'RasaElectronicAdm');
DELETE FROM Processes WHERE ProcessKey=N'RasaElectronicAdm';
");
        }
    }
}
