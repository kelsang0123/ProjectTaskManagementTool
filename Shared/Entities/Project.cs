namespace Entities;

public class Project
{
    public Project()
    {
        
    }
     public Project(string title, string description, string status, int creatorId, DateTime createdAt)
    {
        Title = title;
        Description = description;
        Status = status;
        CreatorId = creatorId;
        CreatedAt = createdAt;  //all parameters name should be similar to its properties
    }

    public int Id{get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Status{get; set;}
    public int CreatorId{get; set;}
    public DateTime CreatedAt{get; set;}
}
