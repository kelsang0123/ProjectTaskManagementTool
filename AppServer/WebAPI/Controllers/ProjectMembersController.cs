using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMemberRepository memberRepo;

        public ProjectMembersController(IProjectMemberRepository memberRepo)
        {
            this.memberRepo = memberRepo;
        }

        
    }
}
