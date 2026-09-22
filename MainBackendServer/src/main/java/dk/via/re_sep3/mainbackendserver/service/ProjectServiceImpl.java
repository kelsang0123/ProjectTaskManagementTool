package dk.via.re_sep3.mainbackendserver.service;

import dk.via.re_sep3.mainbackendserver.*;
import dk.via.re_sep3.mainbackendserver.domain.Project;
import dk.via.re_sep3.mainbackendserver.dtofactory.DTOFactory;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepository;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;

import java.util.List;

public class ProjectServiceImpl extends ProjectServiceGrpc.ProjectServiceImplBase
{
  private final ProjectRepository projectRepo;
  private final DTOFactory dtoFactory;

  public ProjectServiceImpl(ProjectRepository projectRepo, DTOFactory dtoFactory)
  {
    this.projectRepo = projectRepo;
    this.dtoFactory = dtoFactory;
  }

  @Override
  public void registerProject(RegisterProjectRequest request, StreamObserver<RegisterProjectResponse> responseObserver)
  {
    Project project = projectRepo.registerProject(
        request.getTitle(),
        request.getDescription(),
        request.getCreatorId()
    );

    DTOProject dtoProject = DTOFactory.createDTOProject(project);

    RegisterProjectResponse response =
        RegisterProjectResponse.newBuilder()
            .setProject(dtoProject)
            .build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

  @Override
  public void getProjects(GetProjectsRequest request, StreamObserver<GetProjectsResponse> responseObserver)
  {
    try
    {
      List<Project> projects = projectRepo.getProjects();

      GetProjectsResponse.Builder builder = GetProjectsResponse.newBuilder();

      for (Project project : projects)
      {
        builder.addProjects(dtoFactory.toDTO(project));
      }

      responseObserver.onNext(builder.build());
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      responseObserver.onError(
          Status.INTERNAL.withDescription("Failed to retrieve projects")
              .withCause(e).asRuntimeException());
    }
  }
}
