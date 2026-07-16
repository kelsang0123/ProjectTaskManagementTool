package dk.via.re_sep3.mainbackendserver.repositories;

import com.google.protobuf.Timestamp;
import com.google.type.DateTime;
import dk.via.re_sep3.mainbackendserver.domain.Project;

import java.sql.SQLException;
import java.time.Instant;
import java.util.List;

public interface ProjectRepository
{
  List<Project> getProjects() throws SQLException;
  Project registerProject(String title, String description, int creatorId);
}
