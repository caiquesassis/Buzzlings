using Buzzlings.BusinessLogic.Dtos;
using Buzzlings.BusinessLogic.Models.Enums;
using Buzzlings.BusinessLogic.Services.Buzzling;
using Buzzlings.BusinessLogic.Utils;
using Buzzlings.Data.Constants;
using Buzzlings.Data.Models;

namespace Buzzlings.BusinessLogic.Simulation
{
    public class SimulationEventHandler
    {
        private const int BuzzlingHappyMoodThreshold = BuzzlingConstants.MoodMaxRange / 2;
        private const int HappinessImpactMinValue = -10;
        private const int HappinessImpactMaxValue = 10;
        private const int BuzzlingLowMoodLowImpactMinValue = -6;
        private const int BuzzlingLowMoodLowImpactMaxValue = 2;
        private const int BuzzlingHighMoodLowImpactMinValue = -4;
        private const int BuzzlingHighMoodLowImpactMaxValue = 4;
        private const int BuzzlingLowMoodHighImpactMinValue = -8;
        private const int BuzzlingLowMoodHighImpactMaxValue = 2;
        private const int BuzzlingHighMoodHighImpactMinValue = -6;
        private const int BuzzlingHighMoodHighImpactMaxValue = 2;
        private const int BuzzlingDecorationMoodMinValue = 2;
        private const int BuzzlingDecorationMoodMaxValue = 3;

        public SimulationEventDto GenerateEvent(List<Buzzling> buzzlings, string? lastEventLog)
        {
            int aggregateMoodChange = 0;

            string? eventLog = lastEventLog;
            int buzzlingsToDelete = 0;
            SimulationTopEventType eventType = SimulationTopEventType.Decoration;

            while (eventLog is null || eventLog.Equals(lastEventLog))
            {
                (eventLog, buzzlingsToDelete, eventType) = GenerateEventLog(buzzlings, ref aggregateMoodChange);
            }

            int happinessImpact = CalculateHappinessImpact(buzzlings, eventType, aggregateMoodChange);

            return new SimulationEventDto(eventLog, happinessImpact, buzzlingsToDelete);
        }

        private (string EventLog, int BuzzlingsToDelete, SimulationTopEventType EventType) GenerateEventLog(List<Buzzling> buzzlings, ref int aggregateMoodChange)
        {
            string eventLog;
            int buzzlingsToDelete = 0;

            SimulationTopEventType topEventType = RandomUtils.GetRandomEnumValue<SimulationTopEventType>();

            while (buzzlings.Any() is false &&
                (topEventType.Equals(SimulationTopEventType.HiveDisaster) ||
                topEventType.Equals(SimulationTopEventType.SingleBuzzling)))
            {
                topEventType = RandomUtils.GetRandomEnumValue<SimulationTopEventType>();
            }

            switch (topEventType)
            {
                case SimulationTopEventType.BuzzlingCreationMotivator:
                    eventLog = HandleBuzzlingCreationMotivatorEvent(buzzlings, ref aggregateMoodChange);
                    break;

                case SimulationTopEventType.HiveDisaster:
                    (eventLog, buzzlingsToDelete) = HandleHiveDisasterEvent(buzzlings, ref aggregateMoodChange);
                    break;

                case SimulationTopEventType.SingleBuzzling:
                    eventLog = HandleSingleBuzzlingEvent(buzzlings, ref aggregateMoodChange);
                    break;

                case SimulationTopEventType.Decoration:
                    eventLog = HandleDecorationEvent(buzzlings, ref aggregateMoodChange);
                    break;

                default:
                    eventLog = "Event not found";
                    break;
            }

            return (EventLog: eventLog, BuzzlingsToDelete: buzzlingsToDelete, EventType: topEventType);
        }

        private string HandleBuzzlingCreationMotivatorEvent(List<Buzzling> buzzlings, ref int aggregateMoodChange)
        {
            string eventLog;

            SimulationBuzzlingCreationMotivatorEventType simulationBuzzlingCreationMotivatorEventType = RandomUtils.GetRandomEnumValue<SimulationBuzzlingCreationMotivatorEventType>();
            eventLog = GetRandomEventLog(SimulationEventLogs.BuzzlingCreationMotivatorLogs, simulationBuzzlingCreationMotivatorEventType);

            foreach (Buzzling b in buzzlings)
            {
                AdjustBuzzlingMood(b, BuzzlingLowMoodLowImpactMinValue, BuzzlingLowMoodLowImpactMaxValue,
                    BuzzlingHighMoodLowImpactMinValue, BuzzlingHighMoodLowImpactMaxValue, ref aggregateMoodChange);
            }

            return eventLog;
        }

        private (string EventLog, int BuzzlingsToDelete) HandleHiveDisasterEvent(List<Buzzling> buzzlings, ref int aggregateMoodChange)
        {
            string eventLog;

            SimulationHiveDisasterEventType simulationHiveDisasterEventType = RandomUtils.GetRandomEnumValue<SimulationHiveDisasterEventType>();
            eventLog = GetRandomEventLog(SimulationEventLogs.HiveDisasterLogs, simulationHiveDisasterEventType);

            foreach (Buzzling b in buzzlings)
            {
                AdjustBuzzlingMood(b, BuzzlingLowMoodHighImpactMinValue, BuzzlingLowMoodHighImpactMaxValue,
                    BuzzlingHighMoodHighImpactMinValue, BuzzlingHighMoodHighImpactMaxValue, ref aggregateMoodChange);
            }

            return (EventLog: eventLog, BuzzlingsToDelete: buzzlings.Count > 0 ? RemoveBuzzlings(buzzlings) : 0);
        }

        private string HandleSingleBuzzlingEvent(List<Buzzling> buzzlings, ref int aggregateMoodChange)
        {
            string eventLog;

            Buzzling buzzling = RandomUtils.GetRandomListElement(buzzlings);

            SimulationSingleBuzzlingEventType simulationSingleBuzzlingEventType = RandomUtils.GetRandomEnumValue<SimulationSingleBuzzlingEventType>();

            if (simulationSingleBuzzlingEventType == SimulationSingleBuzzlingEventType.Action)
            {
                SimulationSingleBuzzlingActionEventType simulationSingleBuzzlingActionEventType = buzzling.Role!.Name switch
                {
                    "Worker" => SimulationSingleBuzzlingActionEventType.Worker,
                    "Guard" => SimulationSingleBuzzlingActionEventType.Guard,
                    "Forager" => SimulationSingleBuzzlingActionEventType.Forager,
                    "Nurse" => SimulationSingleBuzzlingActionEventType.Nurse,
                    "Attendant" => SimulationSingleBuzzlingActionEventType.Attendant,
                    "Drone" => SimulationSingleBuzzlingActionEventType.Drone,
                    _ => SimulationSingleBuzzlingActionEventType.Worker
                };

                eventLog = buzzling.Role!.Name switch
                {
                    "Worker" => "🛠️ ",
                    "Guard" => "🛡️ ",
                    "Forager" => "🌻 ",
                    "Nurse" => "💉 ",
                    "Attendant" => "💅 ",
                    "Drone" => "🕶️ ",
                    _ => "🛠️ "
                };

                eventLog += buzzling.Name + GetRandomEventLog(SimulationEventLogs.SingleBuzzlingActionLogs, simulationSingleBuzzlingActionEventType);
            }
            else
            {
                bool hasNoRival = false;

                do
                {
                    hasNoRival = false;

                    eventLog = GetRandomEventLog(SimulationEventLogs.SingleBuzzlingLogs, simulationSingleBuzzlingEventType);

                    switch (simulationSingleBuzzlingEventType)
                    {
                        case SimulationSingleBuzzlingEventType.SelfName:
                        case SimulationSingleBuzzlingEventType.SelfRole:
                            eventLog = "🐝 " + buzzling.Name + eventLog;
                            break;

                        case SimulationSingleBuzzlingEventType.Mood:
                            eventLog = "❗" + buzzling.Name + eventLog;
                            break;

                        case SimulationSingleBuzzlingEventType.RivalName:
                        case SimulationSingleBuzzlingEventType.RivalRole:
                            if (buzzlings.Count > 1)
                            {
                                Buzzling rivalBuzzling = RandomUtils.GetRandomListElement(buzzlings);

                                while (buzzling.Equals(rivalBuzzling))
                                {
                                    rivalBuzzling = RandomUtils.GetRandomListElement(buzzlings);
                                }

                                eventLog = "🐝 " + buzzling.Name + eventLog.Replace("RIVAL", rivalBuzzling.Name);
                            }
                            else
                            {
                                hasNoRival = true;
                            }
                            break;
                    }
                } while (hasNoRival);
            }

            AdjustBuzzlingMood(buzzling, BuzzlingLowMoodLowImpactMinValue, BuzzlingLowMoodLowImpactMaxValue,
                BuzzlingHighMoodLowImpactMinValue, BuzzlingHighMoodLowImpactMaxValue, ref aggregateMoodChange);

            return eventLog;
        }

        private string HandleDecorationEvent(List<Buzzling> buzzlings, ref int aggregateMoodChange)
        {
            string eventLog;

            eventLog = RandomUtils.GetRandomListElement(SimulationEventLogs.DecorationLogs);

            foreach (Buzzling b in buzzlings)
            {
                AdjustBuzzlingMood(b, BuzzlingDecorationMoodMinValue, BuzzlingDecorationMoodMaxValue,
                    BuzzlingDecorationMoodMinValue, BuzzlingDecorationMoodMaxValue, ref aggregateMoodChange);
            }

            return eventLog;
        }

        private int CalculateHappinessImpact(List<Buzzling> buzzlings, SimulationTopEventType eventType, int aggregateMoodChange)
        {
            int happinessImpact = 0;

            if (buzzlings.Count > 0)
            {
                aggregateMoodChange = eventType switch
                {
                    SimulationTopEventType.HiveDisaster => -Math.Abs(aggregateMoodChange),
                    SimulationTopEventType.Decoration => Math.Abs(aggregateMoodChange),
                    _ when aggregateMoodChange < 0 && RandomUtils.GetRandomRangeValue(0, 100) >= 75 => Math.Abs(aggregateMoodChange),
                    _ => aggregateMoodChange
                };

                happinessImpact = (Math.Abs(aggregateMoodChange) / buzzlings.Count) switch
                {
                    0 => RandomUtils.GetRandomRangeValue(-7, -4),
                    int n when (n > 9 && n <= 12) => aggregateMoodChange / 2,
                    > 13 => aggregateMoodChange / 3,
                    _ => aggregateMoodChange
                };
            }
            else
            {
                happinessImpact = RandomUtils.GetRandomRangeValue(-7, -4);
            }

            int happyBuzzlingsModifier = buzzlings.Where(b => b.Mood >= BuzzlingHappyMoodThreshold).Count();
            happyBuzzlingsModifier -= RandomUtils.GetRandomRangeValue(0, happyBuzzlingsModifier);
            happinessImpact += Math.Clamp(happyBuzzlingsModifier, 0, 5);

            foreach (Buzzling b in buzzlings.Where(b => b.Mood < BuzzlingHappyMoodThreshold))
            {
                happinessImpact -= RandomUtils.GetRandomRangeValue(1, 3);
            }

            happinessImpact = eventType switch
            {
                SimulationTopEventType.HiveDisaster => -Math.Abs(happinessImpact),
                SimulationTopEventType.Decoration when buzzlings.Count > 0 => Math.Abs(happinessImpact),
                _ => happinessImpact
            };

            happinessImpact = Math.Clamp(happinessImpact, HappinessImpactMinValue, HappinessImpactMaxValue);

            Console.WriteLine("AGGREGATE MOOD CHANGE: " + aggregateMoodChange + "\nHAPPINESS IMPACT: " + happinessImpact + "\n=============================================================");

            return happinessImpact;
        }

        private string GetRandomEventLog<TKey>(Dictionary<TKey, List<string>> eventLogs, TKey eventType)
        {
            if (eventLogs.TryGetValue(eventType, out List<string> logs))
            {
                return RandomUtils.GetRandomListElement(logs);
            }

            return "No log found";
        }

        private void AdjustBuzzlingMood(Buzzling buzzling, int lowMoodMin, int lowMoodMax, int highMoodMin, int highMoodMax, ref int aggregateMoodChange)
        {
            int randomMoodFluctuation;

            randomMoodFluctuation = buzzling.Mood >= BuzzlingHappyMoodThreshold ?
                RandomUtils.GetRandomRangeValue(highMoodMin, highMoodMax) :
                RandomUtils.GetRandomRangeValue(lowMoodMin, lowMoodMax);

            buzzling.Mood += randomMoodFluctuation;
            buzzling.Mood = Math.Clamp(buzzling.Mood!.Value, 0, BuzzlingConstants.MoodMaxRange);

            aggregateMoodChange += randomMoodFluctuation;
        }

        private int RemoveBuzzlings(List<Buzzling> buzzlings)
        {
            int buzzlingsToRemove = buzzlings.Count / RandomUtils.GetRandomRangeValue(2, 4);

            if (buzzlingsToRemove == 0)
            {
                buzzlingsToRemove = 1;
            }

            return buzzlingsToRemove;
        }
    }
}
