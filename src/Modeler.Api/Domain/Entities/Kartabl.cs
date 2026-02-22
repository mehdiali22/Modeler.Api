using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

/// <summary>
/// تعریف کارتابل (Inbox/Queue Definition).
/// این موجودیت «تعریفی» است؛ اینکه هر پرونده/فرایند الان در کدام کارتابل است، در Runtime/Fact نگهداری می‌شود.
/// </summary>
public sealed class Kartabl : BaseEntity
{
    [MaxLength(150)]
    public string KartablKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(150)]
    public string? OwnerSubdomain { get; set; }
}
