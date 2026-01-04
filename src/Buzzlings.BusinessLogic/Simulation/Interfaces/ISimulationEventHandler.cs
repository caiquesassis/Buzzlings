using Buzzlings.BusinessLogic.Dtos;
using Buzzlings.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzzlings.BusinessLogic.Simulation.Interfaces
{
    public interface ISimulationEventHandler
    {
        SimulationEventDto GenerateEvent(List<Buzzling> buzzlings, string? lastEventLog);
    }
}
