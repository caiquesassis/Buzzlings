using Buzzlings.Api.Dtos.Hive;
using Buzzlings.BusinessLogic.Services.Hive;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HivesController : ControllerBase
    {
        private readonly HiveService _hiveService;

        public HivesController(HiveService hiveService)
        {
            _hiveService = hiveService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHive(CreateHiveDto createHiveDto)
        {
            Hive hive = new Hive()
            {
                Name = createHiveDto.Name
            };

            await _hiveService.CreateHiveAsync(hive);

            return CreatedAtAction(nameof(GetHiveById), new { id = hive.Id }, hive);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHives()
        {
            IEnumerable<Hive?> hives = await _hiveService.GetAllHivesAsync();

            return Ok(hives);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetHiveById(int id)
        {
            Hive? hive = await _hiveService.GetHiveByIdAsync(id);

            if (hive is null)
            {
                return NotFound();
            }

            return Ok(hive);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateHive(int id, UpdateHiveDto updateHiveDto)
        {
            Hive? hive = await _hiveService.GetHiveByIdAsync(id);

            if (hive is null)
            {
                return NotFound();
            }

            hive.Name = updateHiveDto.Name;

            await _hiveService.UpdateHiveAsync(hive);

            return Ok(hive);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteHive(int id)
        {
            Hive? hive = await _hiveService.GetHiveByIdAsync(id);

            if (hive is null)
            {
                return NotFound();
            }

            await _hiveService.DeleteHiveAsync(hive);

            return Ok();
        }
    }
}
