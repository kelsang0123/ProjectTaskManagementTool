namespace DTOs;

public class CreateProjectDto
{
    public CreateProjectDto(string title, string description, int creatorId)
    {
        this.Title = title;
        this.Description = description;
        this.CreatorId = creatorId;
    }
    public string Title{get; set;}
    public string Description{get; set;}
    public int CreatorId{get; set;}
   }
