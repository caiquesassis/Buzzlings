using Buzzlings.Data.Models;

namespace Buzzlings.Api.Dtos.Buzzling
{
    public record class UpdateBuzzlingDto(string Name, BuzzlingRole Role, int Mood, ICollection<Data.Models.Buzzling> RivalBuzzlings);
}
