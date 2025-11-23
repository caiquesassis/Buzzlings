using Buzzlings.Api.Dtos.User;
using Buzzlings.BusinessLogic.Services.User;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            User user = new User()
            {
                UserName = createUserDto.Username
            };

            await _userService.Create(user, createUserDto.Password);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User> users = await _userService.GetAll();

            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User user = await _userService.GetById(id);

            if(user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            User user = await _userService.GetById(id);

            if (user is null)
            {
                return NotFound();
            }

            user.UserName = updateUserDto.Username;

            await _userService.Update(user);

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await _userService.GetById(id);

            if (user is null)
            {
                return NotFound();
            }

            await _userService.Delete(user);

            return Ok();
        }
    }
}
