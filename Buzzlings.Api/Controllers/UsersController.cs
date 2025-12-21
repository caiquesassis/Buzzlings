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
            await _userService.RegisterNewUserAsync(createUserDto.Username, createUserDto.Password);

            User? user = await _userService.GetUserByUserNameAsync(createUserDto.Username);

            return CreatedAtAction(nameof(GetUserById), new { id = user!.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User> users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User? user = await _userService.GetUserByIdAsync(id.ToString());

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
            User? user = await _userService.GetUserByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            user.UserName = updateUserDto.Username;

            await _userService.UpdateUserAsync(user);

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _userService.GetUserByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(user.Id.ToString());

            return Ok();
        }
    }
}
