package dk.via.re_sep3.mainbackendserver.domain;

import jakarta.persistence.*;
import java.time.Instant;

@Entity
@Table(name="projects")
public class Project
{
  @Id
  @GeneratedValue(strategy = GenerationType.IDENTITY)
  private int id;
  private String title;
  private String description;
  private String status;
  private int creatorId;
  @Column(name = "createdAt")
  private Instant createdAt;

  public Project()
  {}
  public Project(String title, String description, String status, int creatorId, Instant createdAt)
  {
    this.title = title;
    this.description = description;
    this.status = status;
    this.creatorId = creatorId;
    this.createdAt = createdAt;
  }

  public int getId(){return id;}
  public String getTitle(){
    return title;
  }
  public void setTitle(String title)
  {
    this.title = title;
  }
  public String getDescription(){
    return description;
  }
  public void setDescription(String description){
    this.description = description;
  }
  public String getStatus(){
    return status;
  }
  public void setStatus(String status){
    this.status = status;
  }
  public void setCreatorId(int creatorId){this.creatorId = creatorId;}
  public int getCreatorId(){return creatorId;}

  public void setCreatedAt(Instant createdAt)
  {
    this.createdAt = createdAt;
  }
  public Instant getCreatedAt(){
    return createdAt;
  }

}
