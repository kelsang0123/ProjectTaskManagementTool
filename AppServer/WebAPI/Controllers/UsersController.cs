using DTOs.UserDtos;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepo;

        public UsersController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        
        [HttpPost("createUser")]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
        {
            await VerifyUserNameIsAvailable(request.Username);
            DateTime dateTime = DateTime.Now;
            User user = new(request.Username, request.Password, request.FullName);
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                Email = created.Email,
                FullName = created.FullName
            };
            return Created($"/users/{dto.Id}", created);
        }

        [HttpGet("{UserId:int}/getSingleUser")]
        public async Task<IResult> GetSingleUser(
            [FromRoute] int UserId
        )
        {
         User user = await userRepo.GetSingleAsync(UserId);
         UserDto dto = new()
         {
             Id = user.Id,
             Email = user.Email,
             FullName = user.FullName
         };
         return Results.Ok(dto);
        }

        [HttpGet("manyUsers")]
        public async Task<IResult> GetUsers(
            [FromQuery] string? email = null,
            [FromQuery] int? userId = null            
          ) 
        {
            IQueryable<User> queryableUsers = userRepo.GetMany();
            if(email!=null)
            {
                queryableUsers = queryableUsers.Where(u=>u.Email.Contains(email));
            }
            if(userId != null)
            {
                queryableUsers = queryableUsers.Where(u=>u.Id == userId);
            }
            
            List<UserDto> users = queryableUsers.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            })
            .ToList();
            return Results.Ok(users);
        }

        [HttpDelete("{userId:int}/deleteUser")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
           await userRepo.DeleteAsync(userId);
           return NoContent();
        }
        private async Task VerifyUserNameIsAvailable(string email)
        {
            List<User> users = userRepo.GetMany().AsQueryable().ToList();
            User? existingUser = users.SingleOrDefault(u=>u.Email == email);
            if(existingUser!=null)
            {
                throw new KeyNotFoundException("Username already exists!");
            }
        }
       
        private async Task VerifyUserExists(int RoomCreatorId)
        {
         _ = await userRepo.GetSingleAsync(RoomCreatorId);
        }
    }
}
