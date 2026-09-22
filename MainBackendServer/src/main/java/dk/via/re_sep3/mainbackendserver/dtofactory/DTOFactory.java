package dk.via.re_sep3.mainbackendserver.dtofactory;

import com.google.protobuf.Timestamp;
import dk.via.re_sep3.mainbackendserver.*;
import dk.via.re_sep3.mainbackendserver.domain.Project;

import java.time.Instant;
import java.time.ZoneOffset;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;

public class DTOFactory
{
  private static Timestamp convertToTimestamp(Instant instant)
  {
    return Timestamp.newBuilder().setSeconds(instant.getEpochSecond())
        .setNanos(instant.getNano()).build();
  }

  private static Instant convertToInstant(Timestamp timestamp)
  {
    return Instant.ofEpochSecond(timestamp.getSeconds(), timestamp.getNanos());
  }

  public static DTOProject createDTOProject(Project project)
  {
    DTOProject.Builder builder = DTOProject.newBuilder().setId(project.getId())
        .setTitle(project.getTitle()).setDescription(project.getDescription())
        .setStatus(project.getStatus()).setCreatorId(project.getCreatorId())
        .setCreatedAt(convertToTimestamp(project.getCreatedAt()));
    return builder.build();
  }

  public static GetProjectRequest createGetProjectRequest(int id)
  {
    return GetProjectRequest.newBuilder().setId(id).build();
  }

  public static GetProjectResponse createGetProjectResponse(Project project)
  {
    return GetProjectResponse.newBuilder().setProject(createDTOProject(project))
        .build();
  }

  public static GetProjectsRequest createGetProjectsRequest()
  {
    return GetProjectsRequest.newBuilder().build();
  }

  public static GetProjectsResponse createGetProjectsResponse(
      Project[] projects)
  {
    ArrayList<DTOProject> list = new ArrayList<>();
    for (Project p : projects)
      list.add(DTOProject.newBuilder().setId(p.getId()).setTitle(p.getTitle())
          .setDescription(p.getDescription()).setStatus(p.getStatus())
          .setCreatorId(p.getCreatorId())
          .setCreatedAt(convertToTimestamp(p.getCreatedAt())).build());
    return GetProjectsResponse.newBuilder().addAllProjects(list).build();
  }

  /**
   * Converts domain Project entity to protobuf DTOProject.
   */
  public DTOProject toDTO(Project project)
  {

    return DTOProject.newBuilder().setId(project.getId())
        .setTitle(project.getTitle()).setDescription(project.getDescription())
        .setStatus(project.getStatus()).setCreatorId(project.getCreatorId())
        .setCreatedAt(convertToTimestamp(project.getCreatedAt())).build();
  }

}