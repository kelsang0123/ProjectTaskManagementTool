package dk.via.re_sep3.mainbackendserver.repositories;

import com.google.protobuf.Timestamp;
import com.google.type.DateTime;
import dk.via.re_sep3.mainbackendserver.domain.Project;
import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityTransaction;

import java.sql.SQLException;
import java.time.Instant;
import java.util.List;

public class ProjectRepositoryImpl implements ProjectRepository
{
  private final EntityManager entityManager;

  public ProjectRepositoryImpl(EntityManager entityManager)
  {
    this.entityManager = entityManager;
  }

  @Override
  public Project registerProject(String title, String description, int creatorId)
  {
    Project project = new Project();
    project.setTitle(title);
    project.setDescription(description);
    project.setStatus("Not Started");
    project.setCreatorId(creatorId);
    project.setCreatedAt(Instant.now());

    EntityTransaction transaction = entityManager.getTransaction();

    try
    {
      transaction.begin();

      entityManager.persist(project);

      transaction.commit();

      return project;
    }
    catch (RuntimeException e)
    {
      if(transaction.isActive()){
        transaction.rollback();
      }
      throw e;
    }
  }

  @Override
  public List<Project> getProjects() throws SQLException
  {
    return entityManager
        .createQuery(
            "SELECT p FROM Project p",
            Project.class
        )
        .getResultList();
  }
}
