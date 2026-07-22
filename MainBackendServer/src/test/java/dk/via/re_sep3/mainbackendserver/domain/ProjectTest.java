package dk.via.re_sep3.mainbackendserver.domain;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.time.Clock;
import java.time.Instant;

import static org.junit.jupiter.api.Assertions.*;

class ProjectTest
{
 private Project  project;
  @BeforeEach void setUp()
  {
    System.out.println("--> setUp()");
    project = new Project("","","",0,null);
  }

  @AfterEach void tearDown()
  {
   System.out.println("<-- tearDown()");
   project=null;
  }

  @Test void testGetId_DefaultValue()
  {
      assertEquals(0,project.getId());
  }

  @Test void testGetTitle_Z_Empty()
  {
    assertEquals("", project.getTitle());
  }
  @Test void testGetTitle_O_One()
  {
   project.setTitle("A");
   assertEquals("A", project.getTitle());
  }
  @Test void testGetTitle_M_Many()
  {
   String title = "Project Task Management system";
   project.setTitle(title);
   assertEquals("Project Task Management system", project.getTitle());
  }
  @Test void testGetTitle_B_Boundary_max()
  {
   String title = "A".repeat(500);
   project.setTitle(title);
   assertEquals(title, project.getTitle());
  }
 @Test void testGetTitle_E_Null()
 {
  project.setTitle(null);
  assertNull(project.getTitle());
 }

  @Test void testSetTitle_Z_Empty()
  {
   project.setTitle("");
   assertEquals("", project.getTitle());
  }
  @Test void testSetTitle_O_One()
   {
   project.setTitle("A");
   assertEquals("A", project.getTitle());
   }

 @Test void testSetTitle_M_Many()
 {
  project.setTitle("Project Title");
  assertEquals("Project Title", project.getTitle());
 }

 @Test void testSetTitle_B_Boundary_max()
 {
   String longTitle = "X".repeat(500);
   project.setTitle(longTitle);
   assertEquals(longTitle, project.getTitle());
 }
 @Test void testSetTitle_E_Null()
 {
  project.setTitle(null);
  assertNull(project.getTitle());
 }

 @Test void testGetDescription_Z_Empty()
 {
  assertEquals("", project.getDescription());
 }

 @Test void testGetDescription_O_One()
 {
  project.setDescription("Description");
  assertEquals("Description", project.getDescription());
 }

 @Test void testGetDescription_M_Many()
 {
  String longText = "D".repeat(1000);
  project.setDescription(longText);
  assertEquals(longText, project.getDescription());
 }

 @Test void testGetDescription_E_Null()
 {
  project.setDescription(null);
  assertNull(project.getDescription());
 }

 @Test void testSetDescription_Z_Empty()
 {
  project.setDescription("");
  assertEquals("", project.getDescription());
 }
 @Test void testSetDescription_O_One()
 {
  project.setDescription("One");
  assertEquals("One", project.getDescription());
 }

 @Test void testSetDescription_M_Many()
 {
  project.setDescription("Many words in description.");
  assertEquals("Many words in description.", project.getDescription());
 }

 @Test void testSetDescription_E_Null()
 {
  project.setDescription(null);
  assertNull(project.getDescription());
 }

 @Test void testGetStatus_Z_Empty()
 {
  assertEquals("", project.getStatus());
 }

 @Test void testGetStatus_Not_Started()
 {
  project.setStatus("Not Started");
  assertEquals("Not Started", project.getStatus());
 }

 @Test void testGetStatus_Started()
 {
  project.setStatus("Started");
  assertEquals("Started", project.getStatus());
 }

 @Test void testGetStatus_Completed()
 {
  project.setStatus("Completed");
  assertEquals("Completed", project.getStatus());
 }

 @Test void testGetStatus_E_Null()
 {
  project.setStatus(null);
  assertNull(project.getStatus());
 }

 @Test void testSetStatus_Z_Empty()
 {
  project.setStatus("");
  assertEquals("", project.getStatus());
 }

 @Test void testSetStatus_Not_Started()
 {
  project.setStatus("Not Started");
  assertEquals("Not Started", project.getStatus());
 }

 @Test void testSetStatus_Started()
 {
  project.setStatus("Started");
  assertEquals("Started", project.getStatus());
 }

 @Test void testSetStatus_Completed()
 {
  project.setStatus("Completed");
  assertEquals("Completed", project.getStatus());
 }

 @Test void testSetStatus_E_Null()
 {
  project.setStatus(null);
  assertNull(project.getStatus());
 }

  @Test void testGetCreatorId_Z_Zero()
  {
   assertEquals(0,project.getCreatorId());
  }

 @Test void testGetCreatorId_One_One()
 {
  project.setCreatorId(1);
  assertEquals(1, project.getCreatorId());
 }

  @Test void testSetCreatedAt_Z_E_Null()
  {
   project.setCreatedAt(null);
   assertNull(project.getCreatedAt());
  }
  @Test void testSetCreatedAt_O_One()
  {
   Instant now = Instant.now();
   project.setCreatedAt(now);
   assertEquals(now, project.getCreatedAt());
  }
  @Test void testSetCreatedAt_M_Many()
  {
   Instant another = Instant.parse("2025-01-01T00:00:00Z");
   project.setCreatedAt(another);
   assertEquals(another, project.getCreatedAt());
  }

 @Test void testSetCreatedAt_B_Boundary_Min()
 {
  project.setCreatedAt(Instant.MIN);
  assertEquals(Instant.MIN, project.getCreatedAt());
 }

 @Test void testSetCreatedAt_B_Boundary_Max()
 {
  project.setCreatedAt(Instant.MAX);
  assertEquals(Instant.MAX, project.getCreatedAt());
 }

  @Test void testGetCreatedAt_Z_Null()
  {
   project.setCreatedAt(null);
   assertNull(project.getCreatedAt());
  }
  @Test void testGetCreatedAt_O_One()
  {
   Instant now = Instant.now();
   project.setCreatedAt(now);
   assertEquals(now, project.getCreatedAt());
  }
}