using Buzzlings.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzzlings.BusinessLogic.Dtos
{
    public record class SimulationEventDto(string log, int happinessImpact, int buzzlingsToDelete);
}
