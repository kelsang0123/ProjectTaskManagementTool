package dk.via.re_sep3.mainbackendserver.repositories;

import dk.via.re_sep3.mainbackendserver.domain.Project;
import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.EntityTransaction;
import jakarta.persistence.Persistence;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.sql.SQLException;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class ProjectRepositoryImplTest
{

  private static EntityManagerFactory emf;
  private EntityManager entityManager;

  private ProjectRepositoryImpl repository;



  @BeforeEach
  void setUp()
  {
    emf = Persistence
        .createEntityManagerFactory("testProjectPU");

    entityManager = emf.createEntityManager();
    repository = new ProjectRepositoryImpl(entityManager);

    entityManager.getTransaction().begin();
    entityManager.createQuery("DELETE FROM Project").executeUpdate();
    entityManager.getTransaction().commit();
  }

  @AfterEach
  void tearDown()
  {
    if (entityManager != null && entityManager.isOpen())
    {
      entityManager.close();
    }

    if (emf != null && emf.isOpen())
    {
      emf.close();
    }
  }


  // ==================================================
  // BLACK BOX TESTING - ZOMBE
  // ==================================================



  @Test
  void registerProject_Z_EmptyValues()
  {
    // Z = Empty values


    Project project =
        repository.registerProject(
            "",
            "",
            0
        );


    assertNotNull(project);

    assertEquals("", project.getTitle());

    assertEquals("", project.getDescription());

    assertEquals(0, project.getCreatorId());

    assertEquals(
        "Not Started",
        project.getStatus()
    );
  }





  @Test
  void registerProject_O_OneValidProject()
  {

    Project project =
        repository.registerProject(
            "Test Project",
            "Description",
            1
        );


    assertNotNull(project);

    assertEquals(
        "Test Project",
        project.getTitle()
    );


    assertEquals(
        "Description",
        project.getDescription()
    );


    assertEquals(
        "Not Started",
        project.getStatus()
    );
  }





  @Test
  void registerProject_M_ManyProjects() throws SQLException
  {

    for(int i=1;i<=5;i++)
    {
      repository.registerProject(
          "Project "+i,
          "Description",
          i
      );
    }


    List<Project> projects =
        repository.getProjects();



    assertEquals(
        5,
        projects.size()
    );
  }





  @Test
  void registerProject_B_BoundaryCreatorId()
  {

    Project min =
        repository.registerProject(
            "Min",
            "Boundary",
            Integer.MIN_VALUE
        );


    Project max =
        repository.registerProject(
            "Max",
            "Boundary",
            Integer.MAX_VALUE
        );


    assertEquals(
        Integer.MIN_VALUE,
        min.getCreatorId()
    );


    assertEquals(
        Integer.MAX_VALUE,
        max.getCreatorId()
    );
  }





  @Test
  void registerProject_E_NullValues()
  {

    Project project =
        repository.registerProject(
            null,
            null,
            1
        );


    assertNull(project.getTitle());

    assertNull(project.getDescription());
  }





  // ==================================================
  // WHITE BOX PATH TESTING
  // ==================================================



  @Test
  void registerProject_PathSuccessfulTransaction()
  {
        /*
        Path:

        begin()
          |
        persist()
          |
        commit()
          |
        return project

        */


    Project project =
        repository.registerProject(
            "WhiteBox",
            "Success",
            1
        );


    assertNotNull(project);

    assertTrue(
        entityManager.contains(project)
    );
  }





  @Test
  void getProjects_PathEmptyDatabase()
      throws Exception
  {
        /*
        Path:

        createQuery()
             |
        getResultList()
             |
        return empty list

        */


    List<Project> projects =
        repository.getProjects();


    assertNotNull(projects);

    assertTrue(
        projects.isEmpty()
    );
  }





  @Test
  void getProjects_PathManyResults()
      throws Exception
  {

        /*
        Path:

        query
          |
        result list
          |
        return many objects

        */


    repository.registerProject(
        "P1",
        "D1",
        1
    );


    repository.registerProject(
        "P2",
        "D2",
        2
    );



    List<Project> projects =
        repository.getProjects();



    assertEquals(
        2,
        projects.size()
    );
  }





  @Test
  void registerProject_PathExceptionRollback()
  {

        /*
        Exception path:

        begin()
          |
        persist()
          |
        exception
          |
        rollback()
          |
        throw exception

        */


    EntityManager mockEntityManager =
          mock(EntityManager.class);

    EntityTransaction transaction =
        mock(EntityTransaction.class);

    when(mockEntityManager.getTransaction())
        .thenReturn(transaction);

    when(transaction.isActive())
        .thenReturn(true);

    doThrow(new RuntimeException())
        .when(mockEntityManager)
        .persist(any(Project.class));

    ProjectRepositoryImpl repository =
        new ProjectRepositoryImpl(mockEntityManager);

    assertThrows(
        RuntimeException.class,
        () ->
            repository.registerProject(
                "Test",
                "Test",
                1
            )
    );
    verify(transaction)
        .begin();

    verify(mockEntityManager)
        .persist(any(Project.class));

    verify(transaction)
        .rollback();
  }
}