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
            await VerifyEmailExists(request.Email);
            await VerifyEmailFormat(request.Email);
            await VerifyPasswordStrength(request.Password);
            User user = new(request.FirstName, request.LastName,request.Email, request.Password);
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                FirstName = created.FirstName,
                LastName = created.LastName,
                Email = created.Email
            };
            return Created($"/users/{dto.Id}", created);
        }

        private async Task VerifyPasswordStrength(string password)
        {
            if(password.Length < 8)
            {
                throw new FormatException($"Password, {password} is not strong enough! It must be at least 8 characters long.");
            }
            else if(!password.Any(char.IsUpper))
            {
                throw new FormatException($"Password, {password} is not strong enough! It must contain at least one uppercase letter.");
            }
            else if(!password.Any(char.IsLower))
            {
                throw new FormatException($"Password, {password} is not strong enough! It must contain at least one lowercase letter.");
            }
            else if(!password.Any(char.IsDigit))
            {
                throw new FormatException($"Password, {password} is not strong enough! It must contain at least one digit.");
            }
            else if(!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new FormatException($"Password, {password} is not strong enough! It must contain at least one special character.");
            }
        }

        private async Task VerifyEmailFormat(string email)
        {
          if(!(email.Contains("@") && email.Contains(".com")))
            {
              throw new FormatException($"Email, {email} is not in a valid format!");
            }
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
             FirstName = user.FirstName,
             LastName = user.LastName,
             Email = user.Email
         };
         return Results.Ok(dto);
        }

        [HttpGet("manyUsers")]
        public async Task<IResult> GetUsers(
            [FromQuery] string? email = null,
            [FromQuery] int? userId = null,
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null        
          ) 
        {
            IQueryable<User> queryableUsers = userRepo.GetMany();
            if(email!=null)
            {
                queryableUsers = queryableUsers.Where(u=>u.Email.Contains(email));
            }
            if(firstName != null)
            {
                queryableUsers = queryableUsers.Where(u=>u.FirstName.Contains(firstName));
            }
            if(lastName != null)
            {
                queryableUsers = queryableUsers.Where(u=>u.LastName.Contains(lastName));
            }
            if(userId != null)
            {
                queryableUsers = queryableUsers.Where(u=>u.Id == userId);
            }
            
            List<UserDto> users = queryableUsers.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
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
        private async Task VerifyEmailExists(string email)
        {
            List<User> users = userRepo.GetMany().AsQueryable().ToList();
            User? existingUser = users.SingleOrDefault(u=>u.Email == email);
            if(existingUser!=null)
            {
                throw new KeyNotFoundException($"User account with email, {email} already exists!");
            }
        }
       
        private async Task VerifyUserExists(int RoomCreatorId)
        {
         _ = await userRepo.GetSingleAsync(RoomCreatorId);
        }
    }
}
