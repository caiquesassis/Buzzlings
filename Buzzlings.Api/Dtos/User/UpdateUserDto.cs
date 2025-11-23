namespace Buzzlings.Api.Dtos.User
{
    public record class UpdateUserDto(string Username, string Password, Data.Models.Hive hive);
}
