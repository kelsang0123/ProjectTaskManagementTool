package dk.via.re_sep3.mainbackendserver.repositories;

import dk.via.re_sep3.mainbackendserver.domain.Project;

import java.sql.SQLException;
import java.util.List;

public interface ProjectRepository
{  List<Project> getProjects() throws SQLException;
  Project registerProject(String title, String description, int creatorId);
}
