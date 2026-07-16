package dk.via.re_sep3.mainbackendserver.server;

import dk.via.re_sep3.mainbackendserver.dtofactory.DTOFactory;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepository;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepositoryImpl;
import dk.via.re_sep3.mainbackendserver.service.ProjectServiceImpl;
import dk.via.re_sep3.mainbackendserver.utility.JPAUtil;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import jakarta.persistence.EntityManager;
import jakarta.persistence.Persistence;

import java.io.IOException;

public class MainBackendServer
{
  private Server server;

  public static void main(String[] args) throws IOException, InterruptedException {

    MainBackendServer mainBackendServer = new MainBackendServer();

    mainBackendServer.start();

    mainBackendServer.blockUntilShutdown();
  }

  private void start() throws IOException
  {
    int port = 7891;

    EntityManager entityManager = JPAUtil.getEntityManager();

    ProjectRepository projectRepo = new ProjectRepositoryImpl(entityManager);

    DTOFactory dtoFactory =
        new DTOFactory();

    ProjectServiceImpl projectService =
        new ProjectServiceImpl(
            projectRepo,
            dtoFactory
        );

    server = ServerBuilder
        .forPort(port)
        .addService(projectService)
        .build()
        .start();

    System.out.println("Main server started on port" + port);

    Runtime.getRuntime()
        .addShutdownHook(
            new Thread(() -> {
              System.out.println(
                  "Shutting down server..."
              );
              MainBackendServer.this.stop();
            })
        );
  }

  private void stop()
  {
    if(server!=null){
      server.shutdown();
    }
  }

  private void blockUntilShutdown() throws InterruptedException
  {
    if(server!=null){
      server.awaitTermination();
    }

  }
}
