namespace DTOs;

public class ProjectDto
{
    public int Id{get; set;}
    public required string Title{get; set;}
    public required string Description{get; set;}
    public required string Status{get; set;}
    public int CreatorId{get; set;}
    public DateTime CreatedAt{get; set;}
}
