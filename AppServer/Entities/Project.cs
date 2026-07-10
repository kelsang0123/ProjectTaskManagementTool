namespace Entities;

public class Project
{
    public int Id{get; set;}
    public required string Title{get; set;}
    public required string Description{get; set;}
    public required string Status{get; set;}
    public DateTime CreatedDateTime{get; set;}
}
