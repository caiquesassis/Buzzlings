using Buzzlings.Api.Dtos.Buzzling;
using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BuzzlingsController : ControllerBase
    {
        private readonly BuzzlingService _buzzlingService;

        public BuzzlingsController(BuzzlingService buzzlingService)
        {
            _buzzlingService = buzzlingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBuzzling(CreateBuzzlingDto createBuzzlingDto)
        {
            Buzzling buzzling = new Buzzling()
            {
                Name = createBuzzlingDto.Name,
                Role = createBuzzlingDto.Role
            };

            await _buzzlingService.CreateAsync(buzzling);

            return CreatedAtAction(nameof(GetBuzzlingById), new { id = buzzling.Id }, buzzling);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuzzlings()
        {
            IEnumerable<Buzzling?> buzzlings = await _buzzlingService.GetAllAsync();

            return Ok(buzzlings);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBuzzlingById(int id)
        {
            Buzzling? buzzling = await _buzzlingService.GetByIdAsync(id);

            if (buzzling is null)
            {
                return NotFound();
            }

            return Ok(buzzling);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateBuzzling(int id, UpdateBuzzlingDto updateBuzzlingDto)
        {
            Buzzling? buzzling = await _buzzlingService.GetByIdAsync(id);

            if (buzzling is null)
            {
                return NotFound();
            }

            buzzling.Name = updateBuzzlingDto.Name;
            buzzling.Role = updateBuzzlingDto.Role;

            await _buzzlingService.UpdateAsync(buzzling);

            return Ok(buzzling);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteBuzzling(int id)
        {
            Buzzling? buzzling = await _buzzlingService.GetByIdAsync(id);

            if (buzzling is null)
            {
                return NotFound();
            }

            await _buzzlingService.DeleteAsync(buzzling);

            return Ok();
        }
    }
}
