package dk.via.re_sep3.mainbackendserver.repositories;

import com.google.protobuf.Timestamp;
import com.google.type.DateTime;
import dk.via.re_sep3.mainbackendserver.domain.Project;
import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.EntityTransaction;

import java.sql.SQLException;
import java.time.Instant;
import java.util.List;

public class ProjectRepositoryImpl implements ProjectRepository
{
  private final EntityManagerFactory entityManagerFactory;

  public ProjectRepositoryImpl(EntityManagerFactory entityManagerFactory)
  {
    this.entityManagerFactory = entityManagerFactory;
  }

  @Override
  public Project registerProject(String title, String description, int creatorId)
  {
    EntityManager entityManager = entityManagerFactory.createEntityManager();

    EntityTransaction transaction = entityManager.getTransaction();
    try{
      transaction.begin();

    Project project = new Project();
    project.setTitle(title);
    project.setDescription(description);
    project.setStatus("Not Started");
    project.setCreatorId(creatorId);
    project.setCreatedAt(Instant.now());

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
    finally
    {
      entityManager.close();
    }
  }

  @Override
  public List<Project> getProjects()
  {
    EntityManager entityManager = entityManagerFactory.createEntityManager();
    try
    {
      return entityManager.createQuery("SELECT p FROM Project p", Project.class)
          .getResultList();
    }
    finally
    {
      entityManager.close();
    }
  }
}
