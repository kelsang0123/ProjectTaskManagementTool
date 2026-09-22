package dk.via.re_sep3.mainbackendserver.service;

import dk.via.re_sep3.mainbackendserver.*;
import dk.via.re_sep3.mainbackendserver.domain.Project;
import dk.via.re_sep3.mainbackendserver.dtofactory.DTOFactory;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepository;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.junit.jupiter.MockitoExtension;

import java.sql.SQLException;
import java.time.Instant;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class ProjectServiceImplTest
{

  private ProjectRepository projectRepo;
  private DTOFactory dtoFactory;
  private ProjectServiceImpl service;

  private StreamObserver registerObserver;
  private StreamObserver getObserver;

  @BeforeEach void setUp()
  {
    projectRepo = mock(ProjectRepository.class);
    dtoFactory = mock(DTOFactory.class);

    service = new ProjectServiceImpl(
        projectRepo,
        dtoFactory
    );
    registerObserver = mock(StreamObserver.class);
    getObserver = mock(StreamObserver.class);
  }

  @AfterEach void tearDown()
  {
  }

  @Test
  void registerProject_Z_EmptyInput()
  {
    // Z = Zero / empty values

    RegisterProjectRequest request =
        RegisterProjectRequest.newBuilder()
            .setTitle("")
            .setDescription("")
            .setCreatorId(0)
            .build();


    Project project =
        new Project("", "", "", 0, Instant.EPOCH);


    when(projectRepo.registerProject("", "", 0))
        .thenReturn(project);


    service.registerProject(
        request,
        registerObserver
    );


    verify(projectRepo)
        .registerProject("", "", 0);


    verify(registerObserver)
        .onCompleted();
  }



  @Test
  void registerProject_O_OneValidProject()
  {
    // O = One valid value


    RegisterProjectRequest request =
        RegisterProjectRequest.newBuilder()
            .setTitle("Test")
            .setDescription("Description")
            .setCreatorId(1)
            .build();


    Project project =
        new Project(
            "Test",
            "Description",
            "OPEN",
            1,
            Instant.now()
        );


    when(projectRepo.registerProject(
        "Test",
        "Description",
        1))
        .thenReturn(project);



    service.registerProject(
        request,
        registerObserver
    );


    verify(registerObserver)
        .onNext(any(RegisterProjectResponse.class));


    verify(registerObserver)
        .onCompleted();
  }




  @Test
  void registerProject_M_ManyProjects()
  {
    Project project =
        new Project(
            "Project",
            "Description",
            "OPEN",
            1,
            Instant.now()
        );


    when(projectRepo.registerProject(
        anyString(),
        anyString(),
        anyInt()))
        .thenReturn(project);


    for(int i = 1; i <= 5; i++)
    {
      RegisterProjectRequest request =
          RegisterProjectRequest.newBuilder()
              .setTitle("Project " + i)
              .setDescription("Description")
              .setCreatorId(i)
              .build();


      service.registerProject(
          request,
          registerObserver
      );
    }


    verify(projectRepo, times(5))
        .registerProject(
            anyString(),
            anyString(),
            anyInt()
        );
  }




  @Test
  void registerProject_B_BoundaryCreatorId()
  {
    // B = Boundary values


    RegisterProjectRequest request =
        RegisterProjectRequest.newBuilder()
            .setTitle("Boundary")
            .setDescription("Test")
            .setCreatorId(Integer.MAX_VALUE)
            .build();



    Project project =
        new Project(
            "Boundary",
            "Test",
            "OPEN",
            Integer.MAX_VALUE,
            Instant.now()
        );


    when(projectRepo.registerProject(
        "Boundary",
        "Test",
        Integer.MAX_VALUE))
        .thenReturn(project);



    service.registerProject(
        request,
        registerObserver
    );


    verify(projectRepo)
        .registerProject(
            "Boundary",
            "Test",
            Integer.MAX_VALUE
        );
  }




  @Test
  void registerProject_E_Exception()
  {
    // E = Exception partition


    RegisterProjectRequest request =
        RegisterProjectRequest.newBuilder()
            .setTitle("Test")
            .build();


    when(projectRepo.registerProject(any(),any(),anyInt()))
        .thenThrow(new RuntimeException());


    assertThrows(
        RuntimeException.class,
        () ->
            service.registerProject(
                request,
                registerObserver
            )
    );
  }




  // ======================================================
  // getProjects()
  // WHITE BOX PATH TESTING
  // ======================================================



  @Test
  void getProjects_PathEmptyList() throws SQLException
  {
    // Path 1:
    // try -> empty list -> onNext -> complete


    when(projectRepo.getProjects())
        .thenReturn(Collections.emptyList());


    service.getProjects(
        GetProjectsRequest.newBuilder().build(),
        getObserver
    );


    verify(getObserver)
        .onNext(any(GetProjectsResponse.class));


    verify(getObserver)
        .onCompleted();
  }





  @Test
  void getProjects_PathOneProject() throws SQLException
  {
    // Path 2:
    // for loop executes once


    Project project =
        new Project(
            "Project",
            "Description",
            "OPEN",
            1,
            Instant.now()
        );


    when(projectRepo.getProjects())
        .thenReturn(List.of(project));


    when(dtoFactory.toDTO(project))
        .thenReturn(
            DTOProject.newBuilder()
                .setTitle("Project")
                .build()
        );



    service.getProjects(
        GetProjectsRequest.newBuilder().build(),
        getObserver
    );



    verify(dtoFactory)
        .toDTO(project);


    verify(getObserver)
        .onCompleted();
  }





  @Test
  void getProjects_PathManyProjects() throws SQLException
  {
    // Path 3:
    // for loop executes many times


    Project p1 =
        new Project("P1",
            "D1",
            "OPEN",
            1,
            Instant.now());


    Project p2 =
        new Project("P2",
            "D2",
            "OPEN",
            2,
            Instant.now());



    when(projectRepo.getProjects())
        .thenReturn(
            Arrays.asList(p1,p2)
        );


    when(dtoFactory.toDTO(any(Project.class)))
        .thenReturn(
            DTOProject.newBuilder().build()
        );



    service.getProjects(
        GetProjectsRequest.newBuilder().build(),
        getObserver
    );



    verify(dtoFactory,times(2))
        .toDTO(any(Project.class));


    verify(getObserver)
        .onCompleted();
  }






  @Test
  void getProjects_PathException() throws SQLException
  {
    // Path 4:
    // try -> exception -> catch -> onError


    when(projectRepo.getProjects())
        .thenThrow(
            new RuntimeException()
        );



    service.getProjects(
        GetProjectsRequest.newBuilder().build(),
        getObserver
    );


    verify(getObserver)
        .onError(any(Throwable.class));
  }

}