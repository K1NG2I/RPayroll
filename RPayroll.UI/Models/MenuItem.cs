namespace RPayroll.UI.Models;

public class MenuItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public int? ParentId { get; set; }
    public List<MenuItem> Children { get; set; } = new();
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
}
