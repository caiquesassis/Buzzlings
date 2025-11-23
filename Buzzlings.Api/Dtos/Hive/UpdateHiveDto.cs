namespace Buzzlings.Api.Dtos.Hive
{
    public record class UpdateHiveDto(string Name, int Age, ICollection<Data.Models.Buzzling> Buzzlings, ICollection<string> EventLog);
}
