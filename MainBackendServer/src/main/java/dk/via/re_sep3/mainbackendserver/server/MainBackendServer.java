package dk.via.re_sep3.mainbackendserver.server;

import dk.via.re_sep3.mainbackendserver.dtofactory.DTOFactory;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepository;
import dk.via.re_sep3.mainbackendserver.repositories.ProjectRepositoryImpl;
import dk.via.re_sep3.mainbackendserver.service.ProjectServiceImpl;
import dk.via.re_sep3.mainbackendserver.utility.JPAUtil;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.Persistence;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

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
    int port = 7892;

    String dbUser = System.getenv("POSTGRES_USER");
    String dbPassword = System.getenv("POSTGRES_PASSWORD");

    if (dbUser == null || dbPassword == null) {
      throw new IllegalStateException(
          "POSTGRES_USER or POSTGRES_PASSWORD environment variable is missing!"
      );
    }

    Map<String, Object> properties = new HashMap<>();

    properties.put(
        "jakarta.persistence.jdbc.user",
        dbUser
    );

    properties.put(
        "jakarta.persistence.jdbc.password",
        dbPassword
    );

    EntityManagerFactory emf =
        Persistence.createEntityManagerFactory("projectPU", properties);

    ProjectRepository projectRepo = new ProjectRepositoryImpl(emf);

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

    System.out.println("Main server started on port " + port);

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
