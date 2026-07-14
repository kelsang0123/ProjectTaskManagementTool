namespace DTOs;

public class CreateProjectDto
{
    public CreateProjectDto(string title, string description)
    {
        this.Title = title;
        this.Description = description;
    }
    public string Title{get; set;}
    public string Description{get; set;}
   }
