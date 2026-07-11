namespace Entities;

public class Project
{

    public Project(string title, string description, string status, DateTime createdAt)
    {
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
    }

    public int Id{get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Status{get; set;}
    public DateTime CreatedAt{get; set;}

    private Project(){}
}
