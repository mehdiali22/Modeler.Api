namespace Modeler.Api.Dtos;

public sealed class PagedResultDto<T>
{
    public int Total { get; set; }
    public List<T> Items { get; set; } = new();
}
