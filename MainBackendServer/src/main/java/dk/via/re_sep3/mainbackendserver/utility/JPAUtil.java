package dk.via.re_sep3.mainbackendserver.utility;

import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.Persistence;

public class JPAUtil
{
  private static final EntityManagerFactory factory =
      Persistence.createEntityManagerFactory("projectPU");


  public static EntityManager getEntityManager() {
    return factory.createEntityManager();
  }
}
