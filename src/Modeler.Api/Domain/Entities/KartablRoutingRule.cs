using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

/// <summary>
/// قانون مسیردهی کارتابل.
/// 
/// ایده:
/// - هر زمان Factها تغییر کردند، Ruleها با اولویت (Priority) بررسی می‌شوند.
/// - اولین Rule که Conditionهایش برقرار باشد، کارتابل مقصد (TargetKartabl) را تعیین می‌کند.
/// 
/// نکته:
/// - شرط‌ها به صورت لیست ConditionId ها نگهداری می‌شوند (JSON string) مشابه DecisionOption.
/// - FromKartablId اختیاری است؛ اگر پر باشد Rule فقط وقتی اعمال می‌شود که پرونده «الان» در آن کارتابل باشد.
/// </summary>
public sealed class KartablRoutingRule : BaseEntity
{
    [MaxLength(150)]
    public string RuleKey { get; set; } = default!;

    [MaxLength(150)]
    public string? OwnerSubdomain { get; set; }

    /// <summary>
    /// عدد کوچکتر = اولویت بالاتر.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// اگر پر باشد، Rule فقط وقتی اعمال می‌شود که کارتابل فعلی همین باشد.
    /// </summary>
    public int? FromKartablId { get; set; }
    public Kartabl? FromKartabl { get; set; }

    public int TargetKartablId { get; set; }
    public Kartabl TargetKartabl { get; set; } = default!;

    /// <summary>
    /// JSON array of condition ids, مثال: [1,2,3]
    /// </summary>
    [Required]
    public string ConditionIdsJson { get; set; } = "[]";

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
}
