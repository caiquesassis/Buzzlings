namespace Buzzlings.BusinessLogic.Services.Simulation
{
    public interface ISimulationService
    {
        Task<int> ProcessSimulationAsync(string? userId);
        Task<int> IncrementHiveAge(string? userId);
        Task<(List<string>, int)> GetLatestEventLogs(string? userId, int lastLogIndex);
    }
}
