using Buzzlings.Api.Dtos.TopHive;
using Buzzlings.BusinessLogic.Services.TopHive;
using Buzzlings.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buzzlings.Api.Controllers
{
    [Route("api/v1/hives")]
    [ApiController]
    public class TopHivesController : ControllerBase
    {
        private readonly ITopHiveService _topHiveService;

        public TopHivesController(ITopHiveService topHiveService)
        {
            _topHiveService = topHiveService;
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetTopHives()
        {
            IEnumerable<TopHive>? topHives = await _topHiveService.GetTopHivesAsync();

            //We can use Select to map the results directly into a new object
            IEnumerable<TopHiveDto> formattedTopHives = topHives!.Select(h => new TopHiveDto
            (
                h.User!.UserName!,
                h.HiveName!,
                h.HiveAge
            ));

            return Ok(formattedTopHives);
        }
    }
}
