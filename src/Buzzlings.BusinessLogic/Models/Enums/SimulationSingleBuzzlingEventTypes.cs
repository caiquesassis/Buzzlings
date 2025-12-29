using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzzlings.BusinessLogic.Models.Enums
{
    public enum SimulationSingleBuzzlingEventType
    {
        SelfName,
        SelfRole,
        RivalName,
        RivalRole,
        Action,
        Mood
    }

    public enum SimulationSingleBuzzlingActionEventType
    {
        Worker,
        Guard,
        Forager,
        Nurse,
        Attendant,
        Drone
    }
}
