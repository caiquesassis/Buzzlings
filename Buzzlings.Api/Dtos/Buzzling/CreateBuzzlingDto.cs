using Buzzlings.Data.Models;

namespace Buzzlings.Api.Dtos.Buzzling
{
    public record class CreateBuzzlingDto(string Name, BuzzlingRole Role);
}
